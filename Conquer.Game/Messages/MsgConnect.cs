using Conquer.Cryptography;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Conquer.Game.Messages;

public record MsgConnect : IMessage
{
    public uint Id { get; set; }
    public uint Data { get; set; }
    public string Info { get; set; } = null!;
    public ushort Size => 28;
    public MessageType Type => MessageType.MsgConnect;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt32LittleEndian(buffer[8..]);
        Info = buffer[12..28].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Data);
        buffer[12..28].WriteString(Info);
    }

    public async Task HandleAsync(GameClient client, IOptions<GameServerOptions> options, World world, GameStore store,
        IMemoryCache cache)
    {
        if (client.UserId is not null)
        {
            return;
        }

        if (client.ConnectionCipher is TqConnectionCipher connectionCipher)
        {
            connectionCipher.SetKeys(Id, Data);
        }

        var key = $"Transfer:{((ulong)Data << 32) | Id}";
        client.UserId = cache.Get<string>(key);
        cache.Remove(key);

        if (client.UserId is null)
        {
            client.Write(MsgTalk.System("Invalid user.", TalkChannel.Entrance));
            client.Close();
            return;
        }

        if (world.Players.Values.FirstOrDefault(p => p.UserId == client.UserId) is { } existing)
        {
            try
            {
                store.LogOut(existing);
            }
            finally
            {
                existing.Client.Close();
            }
        }

        if (!await store.FlushAsync(client.UserId))
        {
            Busy();
            return;
        }

        var player = await store.LoadAsync(client.UserId);

        if (player is null)
        {
            client.Write(MsgTalk.System("NEW_ROLE", TalkChannel.Entrance));
            return;
        }

        if (world.Players.ContainsKey(player.Id))
        {
            Busy();
            return;
        }

        client.Player = player;
        player.Client = client;
        world.Enter(player);
        player.Resume();

        client.Write(MsgTalk.System("ANSWER_OK", TalkChannel.Entrance));
        client.Write(new MsgUserInfo(player));
        player.Tell($"Welcome to {options.Value.Name}!");

        if (player.IsMarried && world.Find(player.Spouse) is { } spouse)
        {
            spouse.Tell(player.IsFemale ? "Your wife is online now." : "Your husband get online.");
        }

        if (player.Prizes.Count > 0)
        {
            player.Tell($"Congratulations! You have got {player.Prizes.Count} prizes.  Please go to trade market to claim prize!",
                TalkChannel.Gm);
        }

        void Busy()
        {
            client.Write(MsgTalk.System("Server is busy. Please try again later.", TalkChannel.Entrance));
            client.Close();
        }
    }
}