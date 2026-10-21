namespace Conquer.Game.Models;

public partial class Player
{
    public const uint MoneyLimit = 1_000_000_000;
    private const int StorageSize = 20;
    private const int MarketStorageSize = 40;
    private const uint MarketStorageId = 44;
    private const uint StorageUnlockedRegister = 0;
    private const uint StorageUnlocked = 2;
    private const byte NotStorable = 0x02;

    private readonly Dictionary<uint, uint> _registers = [];

    public bool CanOpenStorage => StoragePin == 0 || GetRegister(StorageUnlockedRegister) == StorageUnlocked;

    public uint GetRegister(uint index) => _registers.GetValueOrDefault(index);

    public void SetRegister(uint index, uint value) => _registers[index] = value;

    public bool CheckPin(string pin) => StoragePin == 0 || uint.TryParse(pin, out var value) && value == StoragePin;

    public bool SetPin(string pin)
    {
        if (!uint.TryParse(pin, out var value) && pin.Length > 0)
        {
            return false;
        }

        StoragePin = value;
        return true;
    }

    public IEnumerable<Item> StoredIn(uint storageId) => Items.Where(item => item.StorageId == storageId);

    public bool Store(Item item, uint storageId)
    {
        if (item.Position != ItemPosition.Inventory || (TypeOf(item).Restrictions & NotStorable) != 0 ||
            StoredIn(storageId).Count() >= (storageId == MarketStorageId ? MarketStorageSize : StorageSize))
        {
            return false;
        }

        item.StorageId = storageId;
        Send(MsgItem.Drop(item));
        return true;
    }

    public bool Withdraw(Item item)
    {
        if (!HasRoom())
        {
            return false;
        }

        item.StorageId = null;
        Send(new MsgItemInfo(item, ItemInfoAction.AddItem));
        return true;
    }

    public bool Store(Item item, DynaNpc box)
    {
        var type = TypeOf(item);

        if (!IsLoose(item) || !Items.Contains(item) || type.IsBound || (type.Restrictions & NotStorable) != 0 || box.Contents.Count >= box.Data3)
        {
            return false;
        }

        Items.Remove(item);
        (item.PlayerId, item.StorageId) = (box.OwnerId, box.Id);
        box.Contents.Add(item);
        Send(MsgItem.Drop(item));
        return true;
    }

    public bool Withdraw(Item item, DynaNpc box)
    {
        if (!HasRoom())
        {
            return false;
        }

        box.Contents.Remove(item);
        (item.PlayerId, item.StorageId) = (Id, null);
        Items.Add(item);
        Send(new MsgItemInfo(item, ItemInfoAction.AddItem));
        return true;
    }

    public bool Deposit(uint amount)
    {
        if (StorageMoney + (ulong)amount > MoneyLimit || !Spend(amount))
        {
            return false;
        }

        StorageMoney += amount;
        return true;
    }

    public bool Withdraw(uint amount)
    {
        if (StorageMoney < amount || Money + (ulong)amount > MoneyLimit)
        {
            return false;
        }

        StorageMoney -= amount;
        Earn(amount);
        return true;
    }
}
