using Microsoft.AspNetCore.Connections;

namespace Conquer.Game;

public class GameClient(ConnectionContext connection) : Client(connection)
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan TickTimeout = TimeSpan.FromSeconds(25);

    private DateTimeOffset _asked;
    private DateTimeOffset _answered;

    public string? UserId { get; set; }
    public Player Player { get; set; } = null!;
    public Map Map => Player.Map!;

    public void Beat(DateTimeOffset now, Random random)
    {
        if (_asked == default)
        {
            _answered = now;
        }

        if (now - _answered >= TickTimeout)
        {
            Drop($"No tick answer in {TickTimeout.TotalSeconds} seconds.");
        }
        else if (now - _asked >= TickInterval)
        {
            _asked = now;
            Write(new MsgTick(Player, random));
        }
    }

    public void Answer(DateTimeOffset now) => _answered = now;
}
