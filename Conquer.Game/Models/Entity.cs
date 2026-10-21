namespace Conquer.Game.Models;

public abstract class Entity
{
    public uint Id { get; set; }
    public uint MapId { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }

    public Map? Map { get; internal set; }
    public abstract IMessage Spawn { get; }
    public virtual IMessage Despawn => new MsgAction { Id = Id, Action = ActionType.LeaveMap };

    public virtual void ShowTo(Player player) => player.Send(Spawn);

    public MsgAction Act(ActionType action, uint data = 0) => new()
    {
        Id = Id,
        X = X,
        Y = Y,
        Data = data,
        Action = action
    };

    public static int Distance(int x, int y, int otherX, int otherY) => Math.Max(Math.Abs(x - otherX), Math.Abs(y - otherY));

    public int DistanceTo(int x, int y) => Distance(X, Y, x, y);
    public int DistanceTo(Entity other) => DistanceTo(other.X, other.Y);

    public bool CanSee(int x, int y) => DistanceTo(x, y) <= Map.ViewRange;
    public bool CanSee(Entity other) => other != this && CanSee(other.X, other.Y);
}
