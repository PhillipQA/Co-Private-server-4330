using Microsoft.AspNetCore.Connections;

namespace Conquer.Game;

public class GameServerOptions
{
    public string Name { get; set; } = null!;
    public string ClientPath { get; set; } = null!;
}

public class GameServer(MessageService messages, World world, GameStore store) : ConnectionHandler
{
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var client = new GameClient(connection);

        try
        {
            while (await client.ReadAsync() is { } message)
            {
                if (client.Player is not null || message is MsgConnect or MsgRegister)
                {
                    await world.Run(() => messages.HandleAsync(client, message));
                }
            }
        }
        finally
        {
            await world.Run(() => client.Player is { } player && store.LogOut(player));

            if (client.UserId is { } userId)
            {
                await store.FlushAsync(userId);
            }
        }
    }
}
