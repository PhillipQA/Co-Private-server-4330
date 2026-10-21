namespace Conquer.Game.Models;

public readonly record struct Stats(
    int MinAttack = 0,
    int MaxAttack = 0,
    int Defense = 0,
    int MagicAttack = 0,
    int MagicDefense = 0,
    int Agility = 0,
    int Dodge = 0,
    int Range = 0,
    int AttackSpeed = 0,
    int MaxHealth = 0,
    int MaxMana = 0,
    int Strength = 0,
    int AgilityBonus = 0,
    int MagicBonus = 0,
    int MagicBlock = 0,
    int DragonGems = 0,
    int PhoenixGems = 0,
    int TortoiseGems = 0,
    int Bless = 0,
    int RainbowGems = 0,
    int VioletGems = 0,
    int MoonGems = 0,
    int ArmorQuality = 0)
{
    public static Stats Of(MonsterType type) => new(
        MinAttack: (int)Math.Min(type.AttackMin, type.AttackMax),
        MaxAttack: (int)Math.Max(type.AttackMin, type.AttackMax),
        Defense: (int)type.Defense,
        MagicAttack: (int)(type.AttackMin + type.AttackMax) / 2,
        MagicDefense: (int)type.MagicDefense,
        Agility: type.Agility,
        Dodge: type.Dodge,
        Range: type.AttackRange,
        AttackSpeed: type.AttackSpeed,
        MaxHealth: type.MaxHealth,
        Strength: (int)(type.AttackMin + type.AttackMax) / 2);
}
