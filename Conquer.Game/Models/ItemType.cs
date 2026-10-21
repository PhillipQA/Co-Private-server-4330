namespace Conquer.Game.Models;

public class ItemType
{
    public const uint DragonBall = 1088000;
    public const uint Meteor = 1088001;
    public const uint CelestialStone = 721259;
    public const ushort Backsword = 421;
    public const ushort Bow = 500;

    private const byte Bound = 0x01;
    private const byte KeptOnDeath = 0x08;

    private uint Group => Id / 10000;

    public ushort WeaponSkill => (ushort)(Id / 1000);
    public bool IsOneHanded => Group is >= 41 and <= 49;
    public bool IsTwoHanded => Group is >= 50 and <= 58;
    public bool IsBow => WeaponSkill == Bow;
    public bool IsShield => Group == 90;
    public bool IsArrow => Group == 105;
    public bool IsBound => (Restrictions & Bound) != 0;
    public bool IsKeptOnDeath => (Restrictions & KeptOnDeath) != 0;
    public bool IsPlated => Group is 11 or 13 or 90;
    public bool IsEquipment => Group is 11 or 12 or 13 or 15 or 16 || IsOneHanded || IsTwoHanded || IsShield;
    public bool IsExpendable => Id / 100_000 == 10;
    public bool IsPotion => Group is 100 or 101;
    public bool IsScroll => Group == 106;
    public bool IsArmor => Group == 13;
    public uint Subtype => Id / 1000;
    public int Level => (int)(IsPlated ? Id % 100 / 10 : Id % 1000 / 10);
    public int Quality => (int)(Id % 10);

    public uint PlusId => IsPlated ? Id - Id % 10 - Id % 1000 / 100 * 100
        : IsOneHanded && WeaponSkill != Backsword ? 444000 + Id % 1000 / 10 * 10
        : IsTwoHanded && !IsBow ? 555000 + Id % 1000 / 10 * 10
        : Id - Id % 10;

    public bool Fits(ItemPosition position) => position switch
    {
        ItemPosition.Head => Group == 11,
        ItemPosition.Necklace => Group == 12,
        ItemPosition.Armor => Group == 13,
        ItemPosition.Ring => Group == 15,
        ItemPosition.Boots => Group == 16,
        ItemPosition.Right => IsOneHanded || IsTwoHanded,
        ItemPosition.Left => IsOneHanded || IsShield || IsArrow,
        _ => false
    };

    public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public byte RequiredProfession { get; set; }
    public byte RequiredWeaponSkill { get; set; }
    public byte RequiredLevel { get; set; }
    public byte RequiredSex { get; set; }
    public ushort RequiredStrength { get; set; }
    public ushort RequiredAgility { get; set; }
    public ushort RequiredVitality { get; set; }
    public ushort RequiredSpirit { get; set; }
    public byte Restrictions { get; set; }
    public ushort Weight { get; set; }
    public uint Price { get; set; }
    public int Task { get; set; }
    public ushort AttackMax { get; set; }
    public ushort AttackMin { get; set; }
    public short Defense { get; set; }
    public short Agility { get; set; }
    public short Dodge { get; set; }
    public short Health { get; set; }
    public short Mana { get; set; }
    public ushort Durability { get; set; }
    public ushort MaxDurability { get; set; }
    public byte Status { get; set; }
    public byte Gem1 { get; set; }
    public byte Gem2 { get; set; }
    public byte Magic1 { get; set; }
    public byte Magic2 { get; set; }
    public byte Plus { get; set; }
    public ushort MagicAttack { get; set; }
    public ushort MagicDefense { get; set; }
    public ushort AttackRange { get; set; }
    public ushort AttackSpeed { get; set; }
}