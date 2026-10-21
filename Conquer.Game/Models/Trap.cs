namespace Conquer.Game.Models;

public class Trap : Entity
{
    private const uint HitsPlayers = 1;

    private static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(1);

    private DateTimeOffset _ready;

    public ushort Type { get; set; }
    public ushort Look { get; set; }
    public uint Data { get; set; }
    public override IMessage Spawn => Message(MapItemAction.SyncTrap);
    public override IMessage Despawn => Message(MapItemAction.DropTrap);
    public IMessage Cast => Message(MapItemAction.CastTrap);

    public void Spring(Player player, DateTimeOffset now)
    {
        if (now < _ready || (Map!.World.Data.TrapTypes[Type].AttackMode & HitsPlayers) == 0)
        {
            return;
        }

        _ready = now + Cooldown;
        Map.World.Scripts.OnStep(player, this);
    }

    public void Remove() => Map?.Remove(this);

    private MsgMapItem Message(MapItemAction action) => new()
    {
        Id = Id,
        ItemTypeId = Look,
        X = X,
        Y = Y,
        Action = action
    };
}
