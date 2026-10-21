namespace Conquer.Game.Models;

public record Enemy
{
    public uint PlayerId { get; set; }
    public uint EnemyPlayerId { get; set; }
    public string Name { get; set; } = null!;
}
