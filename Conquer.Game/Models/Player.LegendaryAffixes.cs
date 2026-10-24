namespace Conquer.Game.Models;

// The two random bonus attributes on every Legendary item (see LegendAffix), on top of its fixed Legendary perk.
public partial class Player
{
    public const int ReforgeShards = 5;

    private const int CriticalMultiplier = 2;

    // Total of one attribute kind over all working Legendary gear being worn.
    private int AffixTotal(AffixKind kind) => Equipment
        .Where(item => item.Legendary && IsWorking(item))
        .SelectMany(item => new[] { LegendAffix.Decode(item.Affix1), LegendAffix.Decode(item.Affix2) })
        .Where(affix => affix.Kind == kind)
        .Sum(affix => affix.Value);

    public int LegendaryDropBonus => AffixTotal(AffixKind.DropChance);

    public static IEnumerable<LegendAffix> AffixesOf(Item item) =>
        new[] { LegendAffix.Decode(item.Affix1), LegendAffix.Decode(item.Affix2) }.Where(affix => affix.Kind != AffixKind.None);

    private AffixGrade RollAffixes(Item item)
    {
        var (first, second) = LegendAffix.RollPair(World.Random);
        (item.Affix1, item.Affix2) = (first.Encode(), second.Encode());
        return LegendAffix.GradeOf(item);
    }

    // Legendary items made before attributes existed get theirs the first time their owner logs in.
    private void EnsureLegendaryAffixes()
    {
        foreach (var item in Items.Where(item => item.Legendary && item.Affix1 == 0))
        {
            RollAffixes(item);
        }
    }

    private void AnnounceRoll(Item item, AffixGrade grade, string verb)
    {
        var affixes = string.Join(", ", AffixesOf(item));

        switch (grade)
        {
            case AffixGrade.Godly:
                World.Announce($"GODLY! {Name} has {verb} a Legendary {NameOf(item)} with {affixes}!", TalkChannel.Gm);
                break;

            case AffixGrade.Perfect:
                World.Announce($"{Name} has {verb} a Legendary {NameOf(item)} with a Perfect attribute: {affixes}!");
                break;

            default:
                World.Announce($"{Name} has {verb} a Legendary {NameOf(item)}!");
                break;
        }

        Tell($"{NameOf(item)} attributes: {affixes}.", TalkChannel.System);
    }

    public enum ReforgeResult
    {
        NotLegendary,
        NoShards,
        Reforged
    }

    // Rerolls both attributes of a worn Legendary item for Dragon Shards; the item's perk stays the same.
    public ReforgeResult Reforge(Item item)
    {
        if (!item.Legendary || !Equipment.Contains(item))
        {
            return ReforgeResult.NotLegendary;
        }

        if (!Has(DragonShard, ReforgeShards) || !Take(DragonShard, ReforgeShards))
        {
            return ReforgeResult.NoShards;
        }

        var grade = RollAffixes(item);
        ItemChanged(item);

        if (grade == AffixGrade.Normal)
        {
            Tell($"{NameOf(item)} attributes: {string.Join(", ", AffixesOf(item))}.", TalkChannel.System);
        }
        else
        {
            AnnounceRoll(item, grade, "reforged");
        }

        return ReforgeResult.Reforged;
    }

    private Stats WithAffixStats(Stats stats)
    {
        var physical = AffixTotal(AffixKind.PhysicalAttack);
        var magic = AffixTotal(AffixKind.MagicAttack);
        var health = AffixTotal(AffixKind.MaxHealth);
        var speed = Math.Min(50, AffixTotal(AffixKind.AttackSpeed));

        return stats with
        {
            MinAttack = stats.MinAttack * (100 + physical) / 100,
            MaxAttack = stats.MaxAttack * (100 + physical) / 100,
            MagicAttack = stats.MagicAttack * (100 + magic) / 100,
            MaxHealth = stats.MaxHealth * (100 + health) / 100,
            AttackSpeed = stats.AttackSpeed * (100 - speed) / 100,
            Dodge = stats.Dodge + AffixTotal(AffixKind.Dodge)
        };
    }

    private int CriticalHit(int damage)
    {
        var chance = damage > 0 ? AffixTotal(AffixKind.CriticalChance) : 0;

        if (chance <= 0 || Roll(100) >= chance)
        {
            return damage;
        }

        LegendLog($"Critical strike: {damage * CriticalMultiplier} damage.");
        return damage * CriticalMultiplier;
    }

    private int AffixDamageReduction(int damage) =>
        damage - damage * Math.Min(50, AffixTotal(AffixKind.DamageReduction)) / 100;

    private double AffixExperience => (100 + AffixTotal(AffixKind.Experience)) / 100.0;
}
