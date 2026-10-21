using Microsoft.AspNetCore.Connections;

namespace Conquer.Account;

public class AccountServer(MessageService messages) : ConnectionHandler
{
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var client = new AccountClient(connection);

        while (await client.ReadAsync() is { } message)
        {
            await messages.HandleAsync(client, message);
        }
    }
}