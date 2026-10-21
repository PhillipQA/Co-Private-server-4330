namespace Conquer.Game.Models;

public record Bounty
{
    public uint Id { get; set; }
    public string Target { get; set; } = null!;
    public string Payer { get; set; } = null!;
    public uint Reward { get; set; }
}
