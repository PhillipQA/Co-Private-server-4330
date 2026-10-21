namespace Conquer.Game.Models;

public record Magic
{
    public uint PlayerId { get; set; }
    public ushort Type { get; set; }
    public byte Level { get; set; }
    public uint Experience { get; set; }
    public byte OldLevel { get; set; }
    public bool Unlearned { get; set; }
}
