namespace Conquer.Game.Models;

public class TrapType
{
    public ushort Id { get; set; }
    public byte Sort { get; set; }
    public uint Look { get; set; }
    public uint ActionId { get; set; }
    public uint Level { get; set; }
    public uint AttackMode { get; set; }
}
