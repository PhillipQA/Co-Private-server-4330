using Conquer.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Conquer.Account.Messages;

public record MsgAccount : IMessage
{
    public string? UserName { get; set; }
    public byte[]? EncryptedPassword { get; set; }
    public string? ServerName { get; set; }
    public ushort Size => 52;
    public MessageType Type => MessageType.MsgAccount;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        UserName = buffer[4..20].ReadString();
        EncryptedPassword = buffer[20..36].ToArray();
        ServerName = buffer[36..52].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        buffer[4..20].WriteString(UserName);
        EncryptedPassword.CopyTo(buffer[20..36]);
        buffer[36..52].WriteString(ServerName);
    }

    public async Task HandleAsync(AccountClient client, AccountDbContext db, IPasswordCipher passwordCipher,
        UserManager<User> userManager, HttpClient httpClient, ILogger<MsgAccount> logger)
    {
        var password = DecryptPassword();
        var user = await userManager.FindByNameAsync(UserName!);

        if (user is null)
        {
            client.Write(new MsgConnectEx(RejectionCode.InvalidAccount));
            return;
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            client.Write(new MsgConnectEx(RejectionCode.TryAgainLater));
            return;
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            await userManager.AccessFailedAsync(user);
            client.Write(new MsgConnectEx(RejectionCode.InvalidAccount));
            return;
        }

        if (user.IsBanned)
        {
            client.Write(new MsgConnectEx(RejectionCode.Banned));
            return;
        }

        var server = await db.Servers.FirstOrDefaultAsync(s => s.Name == ServerName);

        if (server is null)
        {
            client.Write(new MsgConnectEx(RejectionCode.ServerDown));
            return;
        }

        if (await RequestTokenAsync() is not { } token)
        {
            client.Write(new MsgConnectEx(RejectionCode.ServerDown));
            return;
        }

        client.Write(new MsgConnectEx(token, server.IpAddress, server.Port));

        async Task<ulong?> RequestTokenAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<ulong>($"{server.Url}/transfer/{user.Id}");
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                logger.LogWarning("Game server {Server} is unreachable: {Reason}", server.Name, ex.Message);
                return null;
            }
        }

        string DecryptPassword()
        {
            Span<byte> decryptedPassword = stackalloc byte[EncryptedPassword!.Length];
            passwordCipher.Decrypt(EncryptedPassword, decryptedPassword);
            return decryptedPassword.ReadString();
        }
    }
}