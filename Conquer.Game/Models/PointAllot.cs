namespace Conquer.Game.Models;

public class PointAllot
{
    public uint Id { get; set; }
    public byte Profession { get; set; }
    public byte Level { get; set; }
    public ushort Strength { get; set; }
    public ushort Agility { get; set; }
    public ushort Vitality { get; set; }
    public ushort Spirit { get; set; }
}