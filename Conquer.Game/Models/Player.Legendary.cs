namespace Conquer.Game.Models;

// Legendary gear: Super-quality equipment can be forged into a Legendary version at the Legendsmith, which boosts
// that item's attack, defense, magic attack, magic defense and HP. The materials are new items:
//   Dragon Shard       - dropped by bosses (and very rarely by strong monsters)
//   Super Dragon Ball  - made from 10 Dragon Balls; a 30% chance to forge an item Legendary
//   Legend Orb         - made from 3 Super Dragon Balls + 7 Dragon Shards; forging always succeeds
public partial class Player
{
    public const uint DragonShard = 1088100;
    public const uint SuperDragonBall = 1088101;
    public const uint LegendOrb = 1088102;

    public const int DragonBallsPerSuper = 10;
    public const int SupersPerOrb = 3;
    public const int ShardsPerOrb = 7;
    public const int SuperForgeChance = 30;
    // Legendary items get their own perk (Player.LegendaryEffects.cs); set this above 0 to also raise their stats.
    public const int LegendaryBonusPercent = 0;

    private static readonly ItemPosition[] ForgeableSlots =
    [
        ItemPosition.Head, ItemPosition.Necklace, ItemPosition.Armor, ItemPosition.Right, ItemPosition.Left, ItemPosition.Ring,
        ItemPosition.Boots
    ];

    // A Legendary item's own stats are raised by LegendaryBonusPercent.
    private static int Legend(Item item, int stat) => item.Legendary ? stat * (100 + LegendaryBonusPercent) / 100 : stat;

    public string NameOf(Item item) => TypeOf(item).Name;

    // Worn items that can still be forged: Super quality, real equipment, not already Legendary.
    public IReadOnlyList<Item> ForgeableGear() => ForgeableSlots
        .Select(ItemAt)
        .OfType<Item>()
        .Where(item => !item.Legendary && TypeOf(item) is { IsArrow: false } type && type.Quality == (int)SuperQuality &&
                       type.IsEquipment)
        .ToList();

    public IReadOnlyList<Item> LegendaryGear() => Equipment.Where(item => item.Legendary).ToList();

    public bool MakeSuperDragonBall()
    {
        if (!Has(ItemType.DragonBall, DragonBallsPerSuper) || !Take(ItemType.DragonBall, DragonBallsPerSuper))
        {
            return false;
        }

        Give(SuperDragonBall);
        return true;
    }

    public bool MakeLegendOrb()
    {
        if (!Has(SuperDragonBall, SupersPerOrb) || !Has(DragonShard, ShardsPerOrb))
        {
            return false;
        }

        Take(SuperDragonBall, SupersPerOrb);
        Take(DragonShard, ShardsPerOrb);
        Give(LegendOrb);
        return true;
    }

    public enum ForgeResult
    {
        NoMaterial,
        NotForgeable,
        Failed,
        Succeeded
    }

    // Spends one Super Dragon Ball or Legend Orb trying to make a worn item Legendary.
    public ForgeResult Forge(Item item, uint material)
    {
        if (!ForgeableGear().Contains(item))
        {
            return ForgeResult.NotForgeable;
        }

        if (material is not (SuperDragonBall or LegendOrb) || !Take(material))
        {
            return ForgeResult.NoMaterial;
        }

        if (material == SuperDragonBall && Roll(100) >= SuperForgeChance)
        {
            return ForgeResult.Failed;
        }

        item.Legendary = true;
        var grade = RollAffixes(item);
        ItemChanged(item);
        AnnounceRoll(item, grade, "forged");
        return ForgeResult.Succeeded;
    }
}
