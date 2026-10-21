namespace Conquer.Game.Models;

public class ItemPlus
{
    public uint Id { get; set; }
    public uint ItemTypeId { get; set; }
    public byte Level { get; set; }
    public short Health { get; set; }
    public short AttackMax { get; set; }
    public short AttackMin { get; set; }
    public short Defense { get; set; }
    public short MagicAttack { get; set; }
    public short MagicDefense { get; set; }
    public short Agility { get; set; }
    public short Dodge { get; set; }
}