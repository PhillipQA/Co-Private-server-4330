namespace Conquer.Game.Messages;

public enum ItemAction : uint
{
    Buy = 1,
    Sell = 2,
    Drop = 3,
    Use = 4,
    Equip = 5,
    Unequip = 6,
    SplitItem = 7,
    CombineItem = 8,
    QueryMoneySaved = 9,
    SaveMoney = 10,
    DrawMoney = 11,
    DropMoney = 12,
    SpendMoney = 13,
    Repair = 14,
    RepairAll = 15,
    Identify = 16,
    Durability = 17,
    DropEquipment = 18,
    UpgradeQuality = 19,
    UpgradeLevel = 20,
    BoothQuery = 21,
    BoothAdd = 22,
    BoothRemove = 23,
    BoothBuy = 24,
    SyncDurability = 25,
    Fireworks = 26,
    CompleteTask = 27,
    Enchant = 28
}

public record MsgItem : IMessage
{
    public uint Id { get; set; }
    public uint Data { get; set; }
    public ItemAction Action { get; set; }
    public uint Timestamp { get; set; } = (uint)Environment.TickCount;

    private const byte NotSellable = 0x20;
    private const byte Blacksmith = 6;

    public ushort Size => 20;
    public MessageType Type => MessageType.MsgItem;

    public static MsgItem Drop(Item item) => new() { Id = item.Id, Action = ItemAction.Drop };

    public static MsgItem SyncDurability(Item item) => new() { Id = item.Id, Data = item.Durability, Action = ItemAction.SyncDurability };

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt32LittleEndian(buffer[8..]);
        Action = (ItemAction)ReadUInt32LittleEndian(buffer[12..]);
        Timestamp = ReadUInt32LittleEndian(buffer[16..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Data);
        WriteUInt32LittleEndian(buffer[12..], (uint)Action);
        WriteUInt32LittleEndian(buffer[16..], Timestamp);
    }

    public void Handle(Player player, Map map, World world, GameData data, ScriptService scripts, ILogger<MsgItem> logger)
    {
        switch (Action)
        {
            case ItemAction.Buy:
                if (IsShopping() && data.ItemTypes.TryGetValue(Data, out var itemType) && data.Shops[Id].Contains(itemType.Id) &&
                    player.Money >= itemType.Price && player.Give(itemType) is not null)
                {
                    player.Spend(itemType.Price);
                }
                break;

            case ItemAction.Sell:
                if (!IsShopping() || player.FindItem(Data) is not { } sold || !player.IsLoose(sold))
                {
                    break;
                }

                var type = player.TypeOf(sold);
                var worth = sold.MaxDurability == 0 ? 0 : ShareOf(type.Price / 3, Math.Min(sold.Durability, sold.MaxDurability), sold.MaxDurability);

                if ((type.Restrictions & NotSellable) == 0 && player.CanEarn(worth) && player.Take(sold))
                {
                    player.Earn(worth);
                }
                break;

            case ItemAction.Drop:
                if (player.FindItem(Id) is { } dropped && player.IsLoose(dropped) &&
                    (player.TypeOf(dropped).IsBound || map.Drop(new MapItem { Item = dropped }, player, world.UtcNow)))
                {
                    player.Take(dropped);
                }
                break;

            case ItemAction.DropMoney:
                if (Id > 0 && player.Money >= Id && map.Drop(new MapItem { Money = Id }, player, world.UtcNow))
                {
                    player.Spend(Id);
                }
                break;

            case ItemAction.Use:
                if (player.FindItem(Id) is not { } used)
                {
                    break;
                }

                if (player.TypeOf(used).IsScroll && !player.UseCharge(used))
                {
                    break;
                }

                if (scripts.OnUse(player, used) || player.Drink(used))
                {
                    break;
                }

                var position = player.TypeOf(used).IsArrow ? ItemPosition.Left : (ItemPosition)Data;

                if (position != ItemPosition.Inventory)
                {
                    player.Equip(used, position);
                }
                break;

            case ItemAction.Unequip:
                if (player.FindItem(Id) is { } worn)
                {
                    player.Unequip(worn, (ItemPosition)Data);
                }
                break;

            case ItemAction.Repair:
                if (player.IsNear(Npc.ShopType) && player.FindItem(Id) is { } repaired)
                {
                    player.Repair(repaired);
                }
                break;

            case ItemAction.RepairAll:
                if (player.IsNear(Npc.ShopType))
                {
                    player.RepairAll();
                }
                break;

            case ItemAction.CombineItem:
                Pair(player.Combine);
                break;

            case ItemAction.UpgradeQuality:
                if (player.IsNear(Blacksmith))
                {
                    Pair(player.UpgradeQuality);
                }
                break;

            case ItemAction.UpgradeLevel:
                if (player.IsNear(Blacksmith))
                {
                    Pair(player.UpgradeLevel);
                }
                break;

            case ItemAction.Enchant:
                Pair(player.Enchant);
                break;

            case ItemAction.QueryMoneySaved or ItemAction.SaveMoney or ItemAction.DrawMoney:
                if (map.Find<Npc>(Id) is not { IsStorage: true } storage || !player.CanSee(storage) || !player.CanOpenStorage)
                {
                    break;
                }

                if (Action == ItemAction.SaveMoney)
                {
                    player.Deposit(Data);
                }
                else if (Action == ItemAction.DrawMoney)
                {
                    player.Withdraw(Data);
                }

                player.Send(new MsgItem { Id = Id, Data = player.StorageMoney, Action = ItemAction.QueryMoneySaved });
                break;

            case ItemAction.CompleteTask:
                player.Send(this);
                break;

            case ItemAction.BoothQuery:
                Stall(Id)?.Show(player);
                break;

            case ItemAction.BoothAdd:
                if (player.Booth is { } booth && player.FindItem(Id) is { } stocked && booth.AddItem(stocked, Data))
                {
                    player.Send(this);
                }
                break;

            case ItemAction.BoothRemove:
                player.Booth?.RemoveItem(Id);
                break;

            case ItemAction.BoothBuy:
                Stall(Data)?.Sell(player, Id);
                break;

            default:
                logger.WarnOnce("Unhandled action {Action}.", Action);
                break;
        }

        bool IsShopping() => map.Find<Npc>(Id) is { IsShop: true } npc && player.IsAlive && player.CanSee(npc);

        Booth? Stall(uint id) => map.Find<Booth>(id) is { } stall && player.CanSee(stall) ? stall : null;

        void Pair(Func<Item, Item, bool> apply)
        {
            if (player.FindItem(Id) is { } item && player.FindItem(Data) is { } other)
            {
                apply(item, other);
            }
        }

        static uint ShareOf(uint whole, int part, int of) => (uint)Math.Round((double)whole * part / of, MidpointRounding.AwayFromZero);
    }
}