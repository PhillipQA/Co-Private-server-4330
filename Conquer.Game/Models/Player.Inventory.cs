namespace Conquer.Game.Models;

public partial class Player
{
    private const int InventorySize = 40;
    private const uint Consumable = 1000000;

    public IEnumerable<Item> Carried => Items.Where(item => item.StorageId is null);
    public IEnumerable<Item> Inventory => Carried.Where(item => item.Position == ItemPosition.Inventory);
    public int FreeSlots => InventorySize - Inventory.Count();

    public bool HasRoom(int slots = 1)
    {
        if (FreeSlots >= slots)
        {
            return true;
        }

        Tell("Your inventory is full.");
        return false;
    }

    public ItemType TypeOf(Item item) => World.Data.ItemTypes[item.ItemTypeId];

    private bool IsOffered(Item item) => Trade?.Holds(item) == true || Booth?.Holds(item.Id) == true;

    public bool IsLoose(Item item) => item.Position == ItemPosition.Inventory && !IsOffered(item);

    public Item? FindItem(uint id) => Carried.FirstOrDefault(item => item.Id == id);

    public bool Has(uint itemTypeId, int count = 1) => Inventory.Count(item => item.ItemTypeId == itemTypeId) >= count;

    public bool Has(string itemName) => Inventory.Any(item => TypeOf(item).Name == itemName);

    public bool HasAny(uint firstItemTypeId, uint lastItemTypeId, int count = 1) =>
        Inventory.Count(item => item.ItemTypeId >= firstItemTypeId && item.ItemTypeId <= lastItemTypeId) >= count;

    public Item? Give(uint itemTypeId, Action<Item>? configure = null) =>
        World.Data.ItemTypes.GetValueOrDefault(itemTypeId) is { } type ? Give(type, configure) : null;

    public bool Give(uint itemTypeId, int count, Action<Item>? configure = null) =>
        World.Data.ItemTypes.GetValueOrDefault(itemTypeId) is { } type && Give(type, count, configure);

    public Item? Give(ItemType type, Action<Item>? configure = null)
    {
        var item = Item.Of(type);

        configure?.Invoke(item);
        return Receive(item) ? item : null;
    }

    public bool Give(ItemType type, int count, Action<Item>? configure = null) =>
        Enumerable.Range(0, count).All(_ => Give(type, configure) is not null);

    public bool Receive(Item item)
    {
        if (!HasRoom())
        {
            return false;
        }

        item.Id = World.ItemIds.Next();
        (item.PlayerId, item.Position) = (Id, ItemPosition.Inventory);
        Items.Add(item);
        Send(new MsgItemInfo(item, ItemInfoAction.AddItem));
        return true;
    }

    public void Hand(Player other, Item item)
    {
        Items.Remove(item);
        Send(MsgItem.Drop(item));

        (item.PlayerId, item.Position) = (other.Id, ItemPosition.Inventory);
        other.Items.Add(item);
        other.Send(new MsgItemInfo(item, ItemInfoAction.AddItem));
    }

    public bool ClaimPrize()
    {
        if (Prizes.Count == 0)
        {
            return false;
        }

        if (FreeSlots == 0)
        {
            Tell("Your bag is full!");
            return false;
        }

        var prize = Prizes[0];

        if (Give(prize.ItemTypeId) is null)
        {
            return false;
        }

        Prizes.Remove(prize);
        return true;
    }

    public bool Take(Item item)
    {
        if (!Items.Remove(item))
        {
            return false;
        }

        Send(MsgItem.Drop(item));
        return true;
    }

    public bool UseCharge(Item item)
    {
        var charged = item.Durability > 0;

        if (charged)
        {
            item.Durability--;
        }

        if (item.Durability == 0)
        {
            Take(item);
        }
        else
        {
            Send(new MsgItemInfo(item, ItemInfoAction.Update));
        }

        return charged;
    }

    public bool Take(uint itemTypeId, int count = 1)
    {
        if (!Has(itemTypeId, count))
        {
            return false;
        }

        foreach (var item in Inventory.Where(item => item.ItemTypeId == itemTypeId).Take(count).ToList())
        {
            Take(item);
        }

        return true;
    }

    public bool Take(uint firstItemTypeId, uint lastItemTypeId, int count)
    {
        var spent = Inventory.Where(item => item.ItemTypeId >= firstItemTypeId && item.ItemTypeId <= lastItemTypeId).Take(count).ToList();

        return spent.Count == count && spent.All(Take);
    }

    public bool Take(string itemName) =>
        Inventory.FirstOrDefault(item => World.Data.ItemTypes.GetValueOrDefault(item.ItemTypeId)?.Name == itemName) is { } found && Take(found);

    public bool Drink(Item item)
    {
        if (!IsAlive || item.Position != ItemPosition.Inventory ||
            World.Data.ItemTypes.GetValueOrDefault(item.ItemTypeId) is not { Id: >= Consumable } potion ||
            (potion.Health == 0 && potion.Mana == 0) || !Take(item))
        {
            return false;
        }

        AddHealth(potion.Health);
        AddMana(potion.Mana);
        return true;
    }

    public bool Combine(Item item, Item other)
    {
        if (item == other || item.ItemTypeId != other.ItemTypeId || item.Position != ItemPosition.Inventory ||
            other.Position != ItemPosition.Inventory || !TypeOf(item).IsArrow)
        {
            return false;
        }

        var total = item.Durability + other.Durability;

        if (total > item.MaxDurability)
        {
            item.Durability = item.MaxDurability;
            other.Durability = (ushort)(total - item.MaxDurability);
            Send(MsgItem.SyncDurability(other));
        }
        else
        {
            item.Durability = (ushort)total;
            Take(other);
        }

        Send(MsgItem.SyncDurability(item));
        return true;
    }
}
