namespace Conquer.Game.Models;

public partial class Player
{
    private const int UnrestrictedLevel = 70;

    private const int TaoistClass = 19;
    private const int WearChance = 25;
    private const ushort Sword = 420;
    private const int MeteorsToMend = 5;
    private const int FragileGear = 150;
    private const int FragileWeapon = 190;
    private const int MostRepairLoss = 5;
    private const int LeastDurability = 100;
    private const uint TrainingGround = 1039;
    private const int WarriorClass = 2;
    private const int ShieldLevel = 40;
    private const int Super = 9;
    private const uint Cool = 230;
    private const uint FullSuperCool = 0x0100_0000;

    private static readonly TimeSpan CoolRepeat = TimeSpan.FromSeconds(3);

    private static readonly ItemPosition[] SuperSet =
        [ItemPosition.Head, ItemPosition.Necklace, ItemPosition.Armor, ItemPosition.Right, ItemPosition.Ring, ItemPosition.Boots];

    private DateTimeOffset _nextCool;

    private int ProfessionClass => (int)Profession >= 100 ? TaoistClass : (int)Profession / 10;

    internal IEnumerable<Item> Equipment => Carried.Where(item => item.Position != ItemPosition.Inventory);

    private IEnumerable<Item> Held => new[] { ItemPosition.Right, ItemPosition.Left }.Select(ItemAt).OfType<Item>();

    private IEnumerable<ItemType> Hands => Held.Select(TypeOf);

    private int Arrows => ItemAt(ItemPosition.Left) is { } item && TypeOf(item).IsArrow ? item.Durability : 0;

    public override InteractAction AttackKind =>
        Form is null && Worn(ItemPosition.Right) is { IsBow: true } ? InteractAction.Shoot : InteractAction.Attack;

    private bool IsAllSuper => SuperSet.All(position => Worn(position)?.Quality == Super) && Worn(ItemPosition.Left) switch
    {
        _ when Worn(ItemPosition.Right)!.IsTwoHanded => true,
        null => ProfessionClass != WarriorClass || Level < ShieldLevel,
        var left => left.Quality == Super
    };

    public uint Emote(uint emotion, DateTimeOffset now)
    {
        Interrupt();
        Pose = emotion;

        if (emotion != Cool || now < _nextCool)
        {
            return emotion;
        }

        _nextCool = now + CoolRepeat;
        var profession = (uint)Profession << 16;

        return IsAllSuper ? emotion | profession | FullSuperCool
            : Worn(ItemPosition.Armor)?.Quality == Super ? emotion | profession
            : emotion;
    }

    public Item? ItemAt(ItemPosition position) => Carried.FirstOrDefault(item => item.Position == position);

    public bool Wears(ItemPosition position) => ItemAt(position) is not null;

    public ItemType? Worn(ItemPosition position) => ItemAt(position) is { } item ? TypeOf(item) : null;

    public bool IsRepaired(ItemPosition position) => ItemAt(position) is { } item && !NeedsRepair(item);

    public bool Repair(Item item)
    {
        var type = TypeOf(item);

        if (!IsAlive || item.MaxDurability == 0 || item.Durability >= item.MaxDurability || type.IsExpendable)
        {
            return false;
        }

        if (item.Durability == 0)
        {
            if (!Take(ItemType.Meteor, MeteorsToMend))
            {
                Tell($"Mending a broken item takes {MeteorsToMend} meteors.");
                return false;
            }
        }
        else if (RepairCost(item) is var cost && !Spend(cost))
        {
            Tell($"Repairing that costs {cost} silver.");
            return false;
        }

        var fragile = type.IsOneHanded || type.IsTwoHanded || type.Fits(ItemPosition.Ring) ? FragileWeapon : FragileGear;

        if (type.IsEquipment && item.Durability < fragile)
        {
            var lost = Math.Min(item.MaxDurability * (item.Durability * 2 / 3) / fragile, item.MaxDurability / MostRepairLoss);
            item.MaxDurability = (ushort)Math.Max(LeastDurability, item.MaxDurability - lost);
        }

        item.Durability = item.MaxDurability;
        Send(new MsgItemInfo(item, ItemInfoAction.Update));

        if (item.Position != ItemPosition.Inventory)
        {
            RecalculateStats();
        }

        return true;
    }

    public void RepairAll()
    {
        foreach (var item in Carried.ToList())
        {
            Repair(item);
        }
    }

    private uint RepairCost(Item item)
    {
        var quality = (item.ItemTypeId % 10) switch { 9 => 1.125, 8 => 0.975, 7 => 0.9, 6 => 0.825, _ => 0.75 };
        var worn = TypeOf(item).Price * (item.MaxDurability - item.Durability) / item.MaxDurability;
        return (uint)Math.Max(1, worn * quality);
    }

    public bool Equip(Item item, ItemPosition position)
    {
        var type = TypeOf(item);

        if (!IsAlive || item.Position != ItemPosition.Inventory || !type.Fits(position) || !Meets(type) ||
            !FitsHands(type, position, Worn(ItemPosition.Right)) || Is(PlayerStatus.Flying) && (type.IsOneHanded || type.IsTwoHanded) && !type.IsBow)
        {
            return false;
        }

        var displaced = Carried.Where(other => other.Position == position ||
            position == ItemPosition.Right && other.Position == ItemPosition.Left && !Pairs(type, TypeOf(other))).ToList();

        if (!HasRoom(displaced.Count - 1))
        {
            return false;
        }

        AbortCast();
        displaced.ForEach(other => other.Position = ItemPosition.Inventory);
        item.Position = position;
        Send(new MsgItem { Id = item.Id, Data = (uint)position, Action = ItemAction.Equip });
        RecalculateStats();
        Map?.Broadcast(this, Spawn, false);
        return true;
    }

    public bool Unequip(Item item, ItemPosition position)
    {
        if (!IsAlive || position == ItemPosition.Inventory || item.Position != position)
        {
            return false;
        }

        var left = position == ItemPosition.Right ? ItemAt(ItemPosition.Left) : null;
        var shedsLeft = left is not null && TypeOf(left) is { IsShield: true } or { IsArrow: true };

        if (!HasRoom(shedsLeft ? 2 : 1))
        {
            return false;
        }

        AbortCast();
        item.Position = ItemPosition.Inventory;
        Send(new MsgItem { Id = item.Id, Data = (uint)position, Action = ItemAction.Unequip });

        left?.Position = shedsLeft ? ItemPosition.Inventory : ItemPosition.Right;
        RecalculateStats();
        Map?.Broadcast(this, Spawn, false);
        return true;
    }

    private bool Meets(ItemType type)
    {
        if (Level < type.RequiredLevel || type.RequiredSex != 0 && type.RequiredSex != (IsFemale ? 2 : 1))
        {
            return false;
        }

        if (Rebirths > 0 && Level >= UnrestrictedLevel && type.RequiredLevel <= UnrestrictedLevel)
        {
            return true;
        }

        if (type.RequiredWeaponSkill > 0 && (type.IsOneHanded || type.IsTwoHanded) && !IsSkilled(type.WeaponSkill, type.RequiredWeaponSkill))
        {
            return false;
        }

        return MeetsProfession(type.RequiredProfession) && Strength >= type.RequiredStrength && Agility >= type.RequiredAgility &&
               Vitality >= type.RequiredVitality && Spirit >= type.RequiredSpirit;
    }

    private bool MeetsProfession(byte required) =>
        required == 0 || required / 10 == ProfessionClass && (int)Profession % 10 >= required % 10;

    private bool FitsHands(ItemType type, ItemPosition position, ItemType? right) => position switch
    {
        ItemPosition.Left when type.IsArrow => right is { IsBow: true },
        ItemPosition.Left when type.IsShield => right is { IsOneHanded: true },
        ItemPosition.Left => right is { IsOneHanded: true } && ProfessionClass == 1,
        _ => true
    };

    private static bool Pairs(ItemType right, ItemType left) => left.IsArrow ? right.IsBow : right.IsOneHanded;

    private void WearDown(params ReadOnlySpan<ItemPosition> positions)
    {
        if (MapId == TrainingGround)
        {
            return;
        }

        foreach (var position in positions)
        {
            if (ItemAt(position) is { Durability: > 0 } item &&
                (TypeOf(item).IsArrow || World.Random.Next(100) < WearChance))
            {
                Consume(item, 1);
            }
        }
    }

    private void Consume(Item item, int amount)
    {
        item.Durability = (ushort)Math.Max(0, item.Durability - amount);
        Send(MsgItem.SyncDurability(item));

        if (item.Durability > 0)
        {
            return;
        }

        if (!TypeOf(item).IsArrow)
        {
            RecalculateStats();
            return;
        }

        Send(new MsgItem { Id = item.Id, Data = (uint)item.Position, Action = ItemAction.Unequip });
        Take(item);

        if (Inventory.FirstOrDefault(spare => TypeOf(spare).IsArrow) is { } quiver)
        {
            Equip(quiver, ItemPosition.Left);
        }
        else
        {
            Map?.Broadcast(this, Spawn, self: false);
        }
    }
}
