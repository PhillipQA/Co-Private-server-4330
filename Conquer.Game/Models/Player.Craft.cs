namespace Conquer.Game.Models;

public partial class Player
{
    private const uint Gems = 70;
    private const uint Stones = 73;
    private const byte EndgameLevel = 120;
    private const byte MaxPlus = 9;
    private const byte GemPlusFloor = 5;
    private const int MaxComposeGems = 2;
    private const uint MeteorTear = 1088002;
    private const int TearBoost = 130;
    private const int QualitySocketOdds = 300;
    private const int LevelSocketOdds = 1200;
    private const int LuckySocketChance = 50;
    private const int ReinforceChance = 2;
    private const int WeakenChance = 5;
    private const int DurabilityUnit = 100;
    private const int ArtisanRange = 20;
    private const int LowestDye = 2;
    private const int HighestDye = 9;
    private const uint LowestQuality = 3;
    private const uint NormalQuality = 5;
    private const uint SuperQuality = 9;
    private const int PlatedTop = 9;
    private const int WeaponTop = 22;
    private const int UltraTop = 99;
    private const int FreeImproveLevel = 70;

    public bool IsNear(byte npcType) => Map?.Npcs.Any(npc => npc.Type == npcType && DistanceTo(npc) <= ArtisanRange) == true;

    public bool UpgradeQuality(Item item, Item dragonBall)
    {
        if (!TypeOf(item).IsEquipment || NextQuality(item) is not { } next || dragonBall.ItemTypeId != ItemType.DragonBall ||
            !TakeMaterial(item, dragonBall))
        {
            return false;
        }

        var quality = item.ItemTypeId % 10;

        if (World.Random.Next(100) >= QualityChance(item))
        {
            DamageOnFailedUpgrade(item);
            return true;
        }

        Upgrade(item, next, QualitySocketOdds);
        LuckyRoll(quality switch { 6 => 5, 7 => 20, 8 => 50, _ => 0 });
        return true;
    }

    public bool UpgradeLevel(Item item, Item meteor)
    {
        if (!TypeOf(item).IsEquipment || NextLevel(item, false) is not { RequiredLevel: < EndgameLevel } next ||
            meteor.ItemTypeId is not (ItemType.Meteor or MeteorTear) || !TakeMaterial(item, meteor))
        {
            return false;
        }

        var chance = meteor.ItemTypeId == MeteorTear ? LevelChance(item) * TearBoost / 100 : LevelChance(item);

        if (World.Random.Next(100) < chance)
        {
            if (Upgrade(item, next, LevelSocketOdds))
            {
                LuckyRoll(LuckySocketChance);
            }

            return true;
        }

        if (LuckyRoll(SpareChance))
        {
            Tell("The treasure shattered.");
            return true;
        }

        DamageOnFailedUpgrade(item);
        return true;
    }

    public bool Compose(Item main, Item first, Item second, List<Item> gems)
    {
        List<Item> used = [main, first, second, .. gems];
        var needed = main.Plus < GemPlusFloor ? 0 : main.ItemTypeId / 100_000 is 4 or 5 ? 2 : 1;

        if (!IsAlive || used.DistinctBy(item => item.Id).Count() != used.Count ||
            !used.All(IsLoose) ||
            !IsComposable(main.ItemTypeId) || main.Plus >= MaxPlus || !Feeds(main, first) || !Feeds(main, second) ||
            gems.Count < needed || gems.Count > MaxComposeGems || gems.Any(gem => !IsGem(gem.ItemTypeId)))
        {
            return false;
        }

        foreach (var fed in used.Skip(1))
        {
            Take(fed);
        }

        main.Plus++;
        ItemChanged(main);
        return true;
    }

    public bool Enchant(Item item, Item gem)
    {
        if (!IsAlive || !IsLoose(item) || !IsLoose(gem) || !TypeOf(item).IsEquipment || item.Enchant == byte.MaxValue ||
            !IsGem(gem.ItemTypeId) || EnchantRange(gem.ItemTypeId) is not var (least, most) || !Take(gem))
        {
            return false;
        }

        var enchant = (byte)World.Random.Next(least, most + 1);
        Send(new MsgItem { Id = item.Id, Data = enchant, Action = ItemAction.Enchant });

        if (enchant > item.Enchant)
        {
            item.Enchant = enchant;
            ItemChanged(item);
        }

        return true;
    }

    public bool Embed(Item item, Item gem, int slot)
    {
        var fitted = (Gem)(gem.ItemTypeId % 100);

        if (!IsAlive || !IsLoose(item) || !IsLoose(gem) || !IsGem(gem.ItemTypeId) || Socket(item, slot) != Gem.Empty || !Take(gem))
        {
            return false;
        }

        Socket(item, slot, fitted);
        item.MaxDurability = (ushort)(item.MaxDurability * (100 + Kylin(fitted)) / 100);
        Send(new MsgItemInfo(item, ItemInfoAction.Update));
        return true;
    }

    public bool RemoveGem(Item item, int slot)
    {
        if (!IsAlive || !IsLoose(item) || Socket(item, slot) is Gem.None or Gem.Empty)
        {
            return false;
        }

        item.MaxDurability = (ushort)(item.MaxDurability * 100 / (100 + Kylin(Socket(item, slot))));
        item.Durability = Math.Min(item.Durability, item.MaxDurability);
        Socket(item, slot, Gem.Empty);
        Send(new MsgItemInfo(item, ItemInfoAction.Update));
        return true;
    }

    public int QualityCost(ItemPosition position) =>
        ItemAt(position) is { } item && NextQuality(item) is not null ? 100 / QualityChance(item) + 1 : 0;

    public int LevelCost(ItemPosition position) =>
        ItemAt(position) is { } item && NextLevel(item, false) is not null ? (100 / LevelChance(item) + 1) * 12 / 10 : 0;

    public bool UpgradeQuality(ItemPosition position)
    {
        if (ItemAt(position) is not { } item || NeedsRepair(item) || NextQuality(item) is not { } next ||
            !Take(ItemType.DragonBall, QualityCost(position)))
        {
            return false;
        }

        Upgrade(item, next, QualitySocketOdds);
        return true;
    }

    public bool UpgradeLevel(ItemPosition position, bool ultra = false)
    {
        if (ItemAt(position) is not { } item || NeedsRepair(item) || NextLevel(item, ultra) is not { } next)
        {
            return false;
        }

        if (next.RequiredLevel > Level)
        {
            Tell("Level requirement of the new equipment is higher than your level!");
            return false;
        }

        if (!ultra && !Take(ItemType.Meteor, MeteorTear, LevelCost(position)))
        {
            return false;
        }

        Upgrade(item, next, ultra ? 0 : LevelSocketOdds);
        return true;
    }

    public uint? RestoreCost(ItemPosition position) => ItemAt(position) is { } item ? RestoreCost(item) : null;

    public bool RestoreMaxDurability(ItemPosition position)
    {
        if (ItemAt(position) is not { } item || RestoreCost(item) is var cost && (cost == 0 || !Spend(cost)))
        {
            return false;
        }

        item.MaxDurability = FullDurability(item, TypeOf(item));
        Send(new MsgItemInfo(item, ItemInfoAction.Update));
        return true;
    }

    public bool Dye(ItemPosition position, int color)
    {
        if (color is < LowestDye or > HighestDye || ItemAt(position) is not { } item ||
            !TypeOf(item).IsPlated)
        {
            return false;
        }

        var dyed = item.ItemTypeId - item.ItemTypeId % 1000 / 100 * 100 + (uint)color * 100;

        if (!World.Data.ItemTypes.ContainsKey(dyed))
        {
            return false;
        }

        item.ItemTypeId = dyed;
        ItemChanged(item);
        return true;
    }

    public bool Bless(ItemPosition position, byte bless)
    {
        if (ItemAt(position) is not { } item)
        {
            return false;
        }

        item.Bless = bless;
        ItemChanged(item);
        return true;
    }

    public ItemPosition? PickUnblessed()
    {
        var unblessed = Equipment.Where(item => item.Bless == 0 && !TypeOf(item).IsArrow).ToList();
        return unblessed.Count == 0 ? null : unblessed[Roll(unblessed.Count)].Position;
    }

    private ItemType? NextQuality(Item item)
    {
        var quality = item.ItemTypeId % 10;
        var basis = quality < NormalQuality ? item.ItemTypeId - quality + NormalQuality : item.ItemTypeId;

        return quality is >= LowestQuality and < SuperQuality ? World.Data.ItemTypes.GetValueOrDefault(basis + 1) : null;
    }

    private ItemType? NextLevel(Item item, bool ultra) => NextLevel(TypeOf(item), ultra);

    private ItemType? NextLevel(ItemType type, bool ultra)
    {
        var types = World.Data.ItemTypes;

        if (type.Subtype is 112 or (>= 135 and <= 139) || type.Quality < LowestQuality)
        {
            return null;
        }

        if (type.IsPlated && type.Level >= PlatedTop)
        {
            return types.GetValueOrDefault(Promoted(type.Id));
        }

        var top = type.IsPlated ? PlatedTop : ultra ? UltraTop : WeaponTop;

        for (var (id, level) = (type.Id + 10, type.Level + 1); level <= top; (id, level) = (id + 10, level + 1))
        {
            if (types.TryGetValue(id, out var next))
            {
                return next;
            }
        }

        return null;
    }

    private void Degrade(Item item)
    {
        var type = TypeOf(item);

        if (type.IsEquipment && LowestLevel(type) is var lowest && lowest != type)
        {
            item.ItemTypeId = lowest.Id;
            item.MaxDurability = FullDurability(item, lowest);
            item.Durability = Math.Min(item.Durability, item.MaxDurability);
            ItemChanged(item);
        }
    }

    private ItemType LowestLevel(ItemType type) =>
        World.Data.ItemTypes.Values.FirstOrDefault(lower => lower.Id / 10000 == type.Id / 10000 && NextLevel(lower, true)?.Id == type.Id) is
            { } previous
            ? LowestLevel(previous)
            : type;

    private static uint Promoted(uint id) => (id / 10000) switch
    {
        13 => id + 5000,
        11 => id - id / 1000 % 10 * 1000 + 2000 - 90 + id / 1000 % 10 * 10,
        _ => 0
    };

    private uint RestoreCost(Item item)
    {
        var type = TypeOf(item);
        var lost = type.MaxDurability - item.MaxDurability * 100 / DurabilityPercent(item);

        if (lost <= 0)
        {
            return 0;
        }

        var (rate, least) = type.Quality switch { 9 => (5_000_000, 500_000), 8 => (3_500_000, 350_000), _ => (1_500_000, 150_000) };
        return (uint)Math.Max(least, Math.Round((double)rate * lost / type.MaxDurability));
    }

    private bool Upgrade(Item item, ItemType next, int socketOdds)
    {
        var limit = next.MaxDurability + (World.Random.Next(100) < ReinforceChance ? (World.Random.Next(3) + 1) * DurabilityUnit : 0);

        item.ItemTypeId = next.Id;
        item.MaxDurability = item.Durability = (ushort)(limit * DurabilityPercent(item) / 100);
        var socketed = socketOdds > 0 && World.Random.Next(socketOdds) == 0 && TryAddSocket(item);
        ItemChanged(item);
        return socketed;
    }

    private void DamageOnFailedUpgrade(Item item)
    {
        if (World.Random.Next(100) < WeakenChance && item.MaxDurability / DurabilityUnit > 1)
        {
            item.MaxDurability -= DurabilityUnit;
        }

        item.Durability = (ushort)Math.Max(DurabilityUnit, item.MaxDurability - TypeOf(item).MaxDurability / 2);
        Tell("The treasure shattered and the item was damaged.");
        Send(new MsgItemInfo(item, ItemInfoAction.Update));
    }

    private bool TakeMaterial(Item item, Item treasure) => IsAlive && IsLoose(item) && !NeedsRepair(item) && IsLoose(treasure) && Take(treasure);

    private static bool NeedsRepair(Item item) => item.Durability / DurabilityUnit < item.MaxDurability / DurabilityUnit;

    private void ItemChanged(Item item)
    {
        Send(new MsgItemInfo(item, ItemInfoAction.Update));

        if (item.Position != ItemPosition.Inventory)
        {
            RecalculateStats();
            Map?.Broadcast(this, Spawn, false);
        }
    }

    private static ushort FullDurability(Item item, ItemType type) => (ushort)(type.MaxDurability * DurabilityPercent(item) / 100);

    private static int DurabilityPercent(Item item) => 100 + Kylin(item.Gem1) + Kylin(item.Gem2);

    private static bool TryAddSocket(Item item)
    {
        if (item.Gem1 == Gem.None)
        {
            item.Gem1 = Gem.Empty;
        }
        else if (item.Gem2 == Gem.None)
        {
            item.Gem2 = Gem.Empty;
        }
        else
        {
            return false;
        }

        return true;
    }

    private int QualityChance(Item item)
    {
        var chance = (item.ItemTypeId % 10) switch { 6 => 50, 7 => 33, 8 => 20, _ => 100 };
        var required = TypeOf(item).RequiredLevel;

        return Math.Max(1, required > FreeImproveLevel ? chance * (100 - (required - FreeImproveLevel)) / 100 : chance);
    }

    private int LevelChance(Item item)
    {
        var type = TypeOf(item);

        var chance = type.IsPlated
            ? type.Level switch { 5 => 50, 6 => 40, 7 => 30, 8 or 9 => 20, _ => 500 }
            : type.Level switch { >= 11 and <= 22 => 95 - (type.Level - 11) * 5, _ => 500 };

        return Math.Clamp(chance * type.Quality switch { 6 => 90, 7 => 70, 8 => 30, 9 => 10, _ => 100 } / 100, 1, 100);
    }

    public bool HasSocket(int slot) => ItemAt(ItemPosition.Right) is { } held && Socket(held, slot) != Gem.None;

    public bool MakeSocket(int slot)
    {
        if (ItemAt(ItemPosition.Right) is not { } held || Socket(held, slot) != Gem.None ||
            (slot == 2 && held.Gem1 == Gem.None))
        {
            return false;
        }

        Socket(held, slot, Gem.Empty);
        Send(new MsgItemInfo(held, ItemInfoAction.Update));
        return true;
    }

    private static bool IsGem(uint itemTypeId) => itemTypeId / 10_000 == Gems;

    private static bool IsStone(uint itemTypeId) => itemTypeId / 10_000 == Stones;

    private static bool IsComposable(uint itemTypeId) => (itemTypeId / 100_000) switch
    {
        1 => itemTypeId / 10_000 % 100 is >= 11 and <= 17,
        4 or 5 or 9 => true,
        _ => false
    };

    private static bool Feeds(Item main, Item minor) =>
        minor.Plus >= Math.Max(main.Plus, (byte)1) &&
        (IsStone(minor.ItemTypeId) || IsComposable(minor.ItemTypeId) && Matches(main.ItemTypeId, minor.ItemTypeId) &&
            Grade(minor.ItemTypeId) <= Grade(main.ItemTypeId));

    private static bool Matches(uint main, uint minor) => (main / 1000, minor / 1000) switch
    {
        (ItemType.Backsword or ItemType.Bow, _) or (_, ItemType.Backsword or ItemType.Bow) => main / 1000 == minor / 1000,
        _ when main / 100_000 is 4 or 5 or 9 => minor / 100_000 == main / 100_000,
        _ => Kind(main) == Kind(minor)
    };

    private static uint Kind(uint itemTypeId) => (itemTypeId / 1000 % 1000) switch
    {
        112 => (itemTypeId / 10 % 10) switch { 1 => 111, 3 => 113, 4 => 114, 8 => 118, _ => 112 },
        171 => 111,
        173 => 113,
        174 => 114,
        177 => 117,
        178 => 118,
        135 or 140 => 130,
        136 or 141 => 131,
        138 or 143 => 133,
        139 or 144 => 134,
        var kind => kind
    };

    private static uint Grade(uint itemTypeId) => Math.Max(NormalQuality, itemTypeId % 10);

    private static (int Least, int Most)? EnchantRange(uint gem) => (gem % 100) switch
    {
        var normal when normal % 10 == 1 => (1, 59),
        2 or 52 or 62 => (60, 109),
        22 or 42 or 72 => (40, 89),
        12 => (100, 159),
        32 => (80, 129),
        3 or 33 or 73 => (170, 229),
        53 or 63 => (140, 199),
        13 => (200, 255),
        23 => (90, 149),
        43 => (70, 119),
        _ => null
    };

    private static int Kylin(Gem gem) => gem switch
    {
        Gem.NormalKylinGem => 50,
        Gem.RefinedKylinGem => 100,
        Gem.SuperKylinGem => 200,
        _ => 0
    };

    private static Gem Socket(Item item, int slot) => slot == 1 ? item.Gem1 : item.Gem2;

    private static void Socket(Item item, int slot, Gem gem)
    {
        if (slot == 1)
        {
            item.Gem1 = gem;
        }
        else
        {
            item.Gem2 = gem;
        }
    }
}
