namespace Conquer.Game.Models;

public class Rebirth
{
    public uint Id { get; set; }
    public byte RequiredProfession { get; set; }
    public byte NewProfession { get; set; }
    public byte RequiredLevel { get; set; }
    public byte NewLevel { get; set; }
    public byte Rebirths { get; set; }
}
