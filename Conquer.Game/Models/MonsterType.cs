namespace Conquer.Game.Models;

public class MonsterType
{
    private const byte Reviver = 8;

    public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public byte Type { get; set; }
    public byte AiType { get; set; }
    public uint Look { get; set; }
    public byte Level { get; set; }
    public ushort MaxHealth { get; set; }
    public ushort FleeHealth { get; set; }
    public Behavior Behavior { get; set; } = Behavior.Passive | Behavior.Active;
    public byte SizeAdd { get; set; }
    public uint AttackMin { get; set; }
    public uint AttackMax { get; set; }
    public uint Defense { get; set; }
    public byte Agility { get; set; }
    public byte Dodge { get; set; }
    public uint MagicType { get; set; }
    public uint MagicDefense { get; set; }
    public uint MagicHitRate { get; set; }
    public byte ViewRange { get; set; }
    public byte AttackRange { get; set; }
    public ushort AttackSpeed { get; set; }
    public ushort MoveSpeed { get; set; }
    public ushort RunSpeed { get; set; }
    public byte DropArmet { get; set; }
    public byte DropNecklace { get; set; }
    public byte DropArmor { get; set; }
    public byte DropRing { get; set; }
    public byte DropWeapon { get; set; }
    public byte DropShield { get; set; }
    public byte DropShoes { get; set; }
    public uint DropMoney { get; set; }
    public uint DropHealthPotion { get; set; }
    public uint DropManaPotion { get; set; }
    public ushort ExtraExp { get; set; }
    public ushort ExtraDamage { get; set; }

    public bool IsGuardian => Is(Behavior.Guard | Behavior.PkKiller);
    public bool IsReviver => AiType == Reviver;

    public bool Is(Behavior behavior) => (Behavior & behavior) != 0;
}

[Flags]
public enum Behavior : byte
{
    Passive = 0x01,
    Active = 0x02,
    Righteous = 0x04,
    Guard = 0x08,
    PkKiller = 0x10,
    Jump = 0x20,
    Fixed = 0x40,
    FastBack = 0x80
}