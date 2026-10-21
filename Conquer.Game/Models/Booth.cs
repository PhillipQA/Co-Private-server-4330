namespace Conquer.Game.Models;

public sealed class Booth : Entity
{
    private const ushort BoothNpc = 14;
    private const ushort BoothLook = 400;
    private const int Capacity = 18;

    private readonly Dictionary<uint, uint> _prices = [];

    private Booth(Player owner) => Owner = owner;

    public Player Owner { get; }
    public string CryOut { get; private set; } = "";

    public override IMessage Spawn => new MsgNpcInfo
    {
        Id = Id,
        X = X,
        Y = Y,
        Look = (ushort)(BoothLook + (int)Owner.Direction),
        NpcType = BoothNpc,
        Strings = [Owner.Name]
    };

    public static Booth? Open(Player owner)
    {
        if (owner.Map is not { } map || !map.Info.Is(MapFlags.BoothEnable))
        {
            owner.Tell("You cannot set up a stall here.");
            return null;
        }

        owner.Booth?.Close();

        var booth = new Booth(owner) { Id = owner.World.NpcIds.Next() };

        map.Add(booth, owner.X, owner.Y);
        owner.Booth = booth;
        return booth;
    }

    private MsgTalk Hawk => new(TalkChannel.BoothSlogan, Owner.Name, "ALLUSERS", "", CryOut);

    public override void ShowTo(Player player)
    {
        base.ShowTo(player);

        if (CryOut.Length > 0)
        {
            player.Send(Hawk);
        }
    }

    public void Advertise(string words)
    {
        CryOut = words;
        Owner.Map?.Broadcast(Owner, Hawk, self: false);
    }

    public bool AddItem(Item item, uint price)
    {
        if (_prices.Count >= Capacity)
        {
            Owner.Tell("Your stall is full.");
            return false;
        }

        if (price == 0 || !Owner.IsLoose(item) || item.PlayerId != Owner.Id || Owner.TypeOf(item).IsBound || !_prices.TryAdd(item.Id, price))
        {
            return false;
        }

        Owner.Map?.Broadcast(Owner, new MsgItem { Id = item.Id, Data = price, Action = ItemAction.BoothAdd }, self: false);
        return true;
    }

    public void RemoveItem(uint itemId)
    {
        if (_prices.Remove(itemId))
        {
            Owner.Map?.Broadcast(Owner, new MsgItem { Id = itemId, Data = Id, Action = ItemAction.BoothRemove });
        }
    }

    public bool Holds(uint itemId) => _prices.ContainsKey(itemId);

    public bool Sell(Player buyer, uint itemId)
    {
        if (buyer == Owner || !_prices.TryGetValue(itemId, out var price))
        {
            return false;
        }

        if (Owner.FindItem(itemId) is not { Position: ItemPosition.Inventory } item)
        {
            RemoveItem(itemId);
            return false;
        }

        if (buyer.Money < price)
        {
            buyer.Tell("You don't have enough silver.");
            return false;
        }

        if (!buyer.HasRoom())
        {
            return false;
        }

        if (!Owner.CanEarn(price))
        {
            buyer.Tell($"{Owner.Name} cannot carry any more silver.");
            return false;
        }

        RemoveItem(itemId);
        buyer.Pay(Owner, price);
        Owner.Hand(buyer, item);
        Owner.Tell($"{buyer.Name} paid you {price} silver for {Owner.World.Data.ItemTypes.GetValueOrDefault(item.ItemTypeId)?.Name ?? "an item"}.");
        return true;
    }

    public void Show(Player to)
    {
        foreach (var (itemId, price) in _prices.ToList())
        {
            if (Owner.FindItem(itemId) is { Position: ItemPosition.Inventory } item)
            {
                to.Send(new MsgItemInfoEx(this, item, price));
            }
            else
            {
                RemoveItem(itemId);
            }
        }

        to.Send(new MsgAction { Id = to.Id, Data = Id, Action = ActionType.DestroyBooth });
    }

    public void Close()
    {
        Owner.Booth = null;
        Map?.Remove(this);

        foreach (var itemId in _prices.Keys)
        {
            Owner.Send(new MsgItem { Id = itemId, Data = Id, Action = ItemAction.BoothRemove });
        }

        _prices.Clear();
    }
}
