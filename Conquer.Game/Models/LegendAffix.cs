namespace Conquer.Game.Models;

// The random bonus attributes a Legendary item rolls when it is forged (two per item, never the same kind twice).
// An attribute is stored on the item as one number: kind * 1000 + value (0 = none).
public enum AffixKind
{
    None = 0,
    PhysicalAttack = 1,
    MagicAttack = 2,
    CriticalChance = 3,
    DamageReduction = 4,
    MaxHealth = 5,
    AttackSpeed = 6,
    Dodge = 7,
    Experience = 8,
    DropChance = 9
}

public enum AffixGrade
{
    Normal,
    Perfect,
    Godly
}

public readonly record struct LegendAffix(AffixKind Kind, int Value)
{
    // Chance that one attribute rolls its maximum value ("Perfect"). Two Perfect attributes make a "Godly" item.
    public const int PerfectChance = 5;

    private static readonly Dictionary<AffixKind, (int Min, int Max, string Name)> Ranges = new()
    {
        [AffixKind.PhysicalAttack] = (3, 10, "Physical attack"),
        [AffixKind.MagicAttack] = (3, 10, "Magic attack"),
        [AffixKind.CriticalChance] = (2, 6, "Critical strike chance"),
        [AffixKind.DamageReduction] = (2, 8, "Damage reduction"),
        [AffixKind.MaxHealth] = (4, 12, "Max HP"),
        [AffixKind.AttackSpeed] = (3, 8, "Attack speed"),
        [AffixKind.Dodge] = (2, 6, "Dodge"),
        [AffixKind.Experience] = (5, 15, "Experience"),
        [AffixKind.DropChance] = (5, 20, "Silver and item drops")
    };

    public bool IsPerfect => Ranges.TryGetValue(Kind, out var range) && Value >= range.Max;

    public static LegendAffix Decode(int code) =>
        code <= 0 || !Enum.IsDefined((AffixKind)(code / 1000)) ? default : new((AffixKind)(code / 1000), code % 1000);

    public int Encode() => Kind == AffixKind.None ? 0 : (int)Kind * 1000 + Value;

    public override string ToString() =>
        Ranges.TryGetValue(Kind, out var range) ? $"+{Value}% {range.Name}{(IsPerfect ? " (Perfect)" : "")}" : "";

    public static (LegendAffix First, LegendAffix Second) RollPair(Random random)
    {
        var kinds = Ranges.Keys.ToList();
        var first = kinds[random.Next(kinds.Count)];
        kinds.Remove(first);
        var second = kinds[random.Next(kinds.Count)];
        return (Roll(first, random), Roll(second, random));
    }

    private static LegendAffix Roll(AffixKind kind, Random random)
    {
        var (min, max, _) = Ranges[kind];
        return new(kind, random.Next(100) < PerfectChance ? max : random.Next(min, max));
    }

    public static AffixGrade GradeOf(Item item)
    {
        var (first, second) = (Decode(item.Affix1), Decode(item.Affix2));
        return first.IsPerfect && second.IsPerfect ? AffixGrade.Godly
            : first.IsPerfect || second.IsPerfect ? AffixGrade.Perfect
            : AffixGrade.Normal;
    }
}
