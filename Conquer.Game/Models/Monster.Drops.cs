namespace Conquer.Game.Models;

public partial class Monster
{
    private const uint TwinCity = 1002;
    private const byte NoDrop = 99;
    private const byte PlatedTop = 9;
    private const byte WeaponTop = 23;
    private const byte LowLevel = 60;
    private const byte HighLevel = 80;
    private const byte GemTierLevel = 117;
    private const int GemLevelGap = 5;
    private const int Jackpot = 123;
    private const int GemJackpot = 13;
    private const uint LeastGear = 100;
    private const uint LeastHighLevelMoney = 10;
    private const int GearChance = 40;
    private const int PotionChance = 3;
    private const int LuckySuperChance = 75;
    private const double PileSpread = 0.8;
    private const double DurabilitySpread = 0.3;
    private const ushort Boots = 160;
    private const ushort Earrings = 117;
    private const ushort Shield = 900;
    private const ushort Whip = 440;
    private const byte EarringColor = 3;

    private static readonly ushort[] Necklaces = [120, 121];
    private static readonly ushort[] Rings = [150, 151, 152];
    private static readonly ushort[] Headwear = [111, 113, 114, Earrings, 118];
    private static readonly ushort[] Armors = [130, 131, 132, 133, 134];
    private static readonly ushort[] OneHanded = [410, 420, ItemType.Backsword, 430, Whip, 450, 460, 480, 481, 490, 500];
    private static readonly ushort[] TwoHanded = [510, 530, 560, 561, 580, Shield];

    private static readonly (int Below, uint Gem)[] GemOdds =
        [(100, 700011), (200, 700001), (300, 700031), (475, 700061), (650, 700051), (825, 700021), (1000, 700041)];

    private Random Random => World.Random;

    public bool Drop(uint itemTypeId, Player owner) => Drop(itemTypeId, owner, World.UtcNow);

    private bool Drop(uint itemTypeId, Player? owner, DateTimeOffset now, Action<Item>? configure = null)
    {
        if (!World.Data.ItemTypes.TryGetValue(itemTypeId, out var type))
        {
            return false;
        }

        var item = Item.Of(type);
        configure?.Invoke(item);
        return Map!.Drop(new MapItem { Item = item, OwnerId = owner?.Id ?? 0 }, this, now);
    }

    private void Drop(Fighter? killer, Player? looter, DateTimeOffset now)
    {
        DropPiles(looter, now);
        DropRareGear(looter, now);

        if (Random.Next(100) < PotionChance)
        {
            DropPotions(looter, now);
        }

        if (MapId != TwinCity)
        {
            DropTreasures(looter, now);
        }

        if (Random.Next(GemOddsFor(killer)) == GemJackpot)
        {
            var roll = Random.Next(1000);
            Drop(GemOdds.First(odds => roll < odds.Below).Gem, looter, now);
        }
    }

    private void DropPiles(Player? looter, DateTimeOffset now)
    {
        var purse = 2 * Random.Next((int)MonsterType.DropMoney);
        var (burst, scatter, burstShare, scatterShare) = Level <= LowLevel ? (2, 27, 2000, 240) : (3, 35, 1320, 180);
        var roll = Random.Next(100);

        var (piles, share) = roll < burst ? (Random.Next(5, 9), burstShare)
            : roll < scatter ? (Random.Next(100) switch { < 10 => 3, < 40 => 2, _ => 1 }, scatterShare)
            : (0, 0);

        for (var pile = 0; pile < piles; pile++)
        {
            var amount = (uint)(purse * share / 100 / piles * Random.Spread(PileSpread));

            if (Random.Next(100) < GearChance)
            {
                DropEquipment(looter, now, amount);
            }
            else
            {
                DropMoney(looter, now, amount);
            }
        }
    }

    private void DropRareGear(Player? looter, DateTimeOffset now)
    {
        var high = Level > HighLevel;

        var (quality, worth, kept) = Random.Next(50_000) switch
        {
            < 1 => (9, 100, high ? Random.Next(10) == 7 : Random.Next(3) == 1),
            < 6 => (8, 100, !high || Random.Next(5) == 3),
            < 31 => (7, 50, !high || Random.Next(3) == 1),
            < 131 => (6, 30, true),
            _ => (0, 0, false)
        };

        if (kept)
        {
            DropEquipment(looter, now, (uint)(Level * worth), quality);
        }
    }

    private void DropPotions(Player? looter, DateTimeOffset now)
    {
        var count = Random.Next(100) switch { < 2 => 8, < 3 => 6, < 5 => 5, < 8 => 4, < 10 => 3, < 25 => 2, _ => 1 };

        for (var potion = 0; potion < count; potion++)
        {
            var type = Random.Next(2) == 1 ? MonsterType.DropHealthPotion : MonsterType.DropManaPotion;

            if (type != 0)
            {
                Drop(type, looter, now);
            }
        }
    }

    private void DropTreasures(Player? looter, DateTimeOffset now)
    {
        if (Random.Next(750) == Jackpot)
        {
            Drop(ItemType.Meteor, looter, now);
        }

        if (Random.Next(300_000) == Jackpot && Drop(ItemType.DragonBall, looter, now) && looter is not null)
        {
            World.Announce($"{looter.Name} has found a DragonBall!");
        }
    }

    private int GemOddsFor(Fighter? killer) =>
        Level >= GemTierLevel ? 1400 : killer is not null && killer.Level > Level + GemLevelGap ? 9000 : 900;

    private void DropMoney(Player? looter, DateTimeOffset now, uint amount)
    {
        if (amount > 0 && (Level < HighLevel || amount > LeastHighLevelMoney))
        {
            Map!.Drop(new MapItem { Money = amount, OwnerId = looter?.Id ?? 0 }, this, now);
        }
    }

    private void DropEquipment(Player? looter, DateTimeOffset now, uint value, int quality = 0)
    {
        if (value < LeastGear || Forge(value, quality) is not var (item, type))
        {
            DropMoney(looter, now, value);
            return;
        }

        if (type.Quality == 9 && looter is { IsLucky: true } && Random.Next(100) < LuckySuperChance)
        {
            looter.ShowEffect(Player.LuckyGuy);
        }

        Map!.Drop(new MapItem { Item = item, OwnerId = looter?.Id ?? 0, Price = type.Price }, this, now);
    }

    private (Item Item, ItemType Type)? Forge(uint value, int quality)
    {
        quality = quality != 0 ? quality : Random.Next(100) switch { < 30 => 5, < 70 => 4, _ => 3 };
        ushort Pick(ushort[] sorts) => sorts[Random.Next(sorts.Length)];

        ushort? sort = Random.Next(1200) switch
        {
            < 20 => Boots,
            < 50 => Pick(Necklaces),
            < 100 => Pick(Rings),
            < 400 => Pick(Headwear),
            < 700 => Pick(Armors),
            _ => Random.Next(100) switch { < 20 => ItemType.Backsword, < 40 => null, < 80 => Pick(OneHanded), _ => Pick(TwoHanded) }
        };

        if (sort is not { } kind)
        {
            return null;
        }

        var (level, isPlated) = (kind / 10) switch
        {
            16 => (MonsterType.DropShoes, false),
            12 => (MonsterType.DropNecklace, false),
            15 => (MonsterType.DropRing, false),
            11 => (MonsterType.DropArmet, true),
            13 => (MonsterType.DropArmor, true),
            90 => (MonsterType.DropShield, true),
            _ => (MonsterType.DropWeapon, false)
        };

        if (level == NoDrop)
        {
            return null;
        }

        level = Random.Next(100) switch
        {
            < 50 => (byte)Math.Max(0, Random.Next(level / 2) + level / 3 - 1),
            >= 80 => (byte)Math.Min(level + 1, isPlated ? PlatedTop : WeaponTop),
            _ => level
        };

        var color = kind == Earrings ? EarringColor : isPlated ? Random.Next(3, 10) : 0;

        if (!World.Data.ItemTypes.TryGetValue((uint)(kind * 1000 + color * 100 + level * 10 + quality), out var type))
        {
            return null;
        }

        var item = Item.Of(type);
        item.MaxDurability = (ushort)Math.Max(1, (int)(type.MaxDurability * Random.Spread(DurabilitySpread)));

        item.Durability = (ushort)(quality > 5
            ? item.MaxDurability * (Random.Next(50) + 50) / 100
            : Math.Clamp(3L * item.MaxDurability * value / Math.Max(1u, type.Price), 1, item.MaxDurability));

        if (type.IsOneHanded || type.IsTwoHanded)
        {
            (item.Gem1, item.Gem2) = Random.Next(100) switch { < 5 => (Gem.Empty, Gem.Empty), < 20 => (Gem.Empty, Gem.None), _ => (Gem.None, Gem.None) };
        }

        item.Plus = (byte)(Random.Next(300) == Jackpot ? 1 : 0);

        if (item.Gem1 != Gem.Empty && Random.Next(kind == Whip ? 60 : 300) == GemJackpot)
        {
            item.Bless = (byte)(Random.Next(100) < 70 ? 3 : 5);
        }

        return (item, type);
    }
}
