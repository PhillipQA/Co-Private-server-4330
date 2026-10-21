namespace Conquer.Game.Models;

public sealed class Trade
{
    public const int Capacity = 20;

    private readonly Side _first;
    private readonly Side _second;

    public Trade(Player first, Player second)
    {
        _first = new(first);
        _second = new(second);
        first.Trade = this;
        second.Trade = this;
        first.Send(new MsgTrade { Id = second.Id, Action = TradeAction.Open });
        second.Send(new MsgTrade { Id = first.Id, Action = TradeAction.Open });
    }

    public bool Offer(Player player, Item item)
    {
        var (mine, theirs) = Sides(player);

        if (mine.Accepted || mine.Items.Count >= Capacity || !player.IsLoose(item) || !player.Carried.Contains(item) || item.StorageId is not null ||
            player.TypeOf(item).IsBound || theirs.Player.FreeSlots + theirs.Items.Count <= mine.Items.Count)
        {
            return false;
        }

        mine.Items.Add(item);
        theirs.Player.Send(new MsgItemInfo(item, ItemInfoAction.Trade));
        player.Send(new MsgTrade { Id = item.Id, Action = TradeAction.AddItem });
        return true;
    }

    public void Offer(Player player, uint money)
    {
        var (mine, theirs) = Sides(player);

        if (mine.Accepted || money > player.Money)
        {
            return;
        }

        mine.Money = money;
        theirs.Player.Send(new MsgTrade { Id = money, Action = TradeAction.AllMoney });
        player.Send(new MsgTrade { Id = money, Action = TradeAction.SelfAllMoney });
    }

    public bool Holds(Item item) => _first.Items.Contains(item) || _second.Items.Contains(item);

    public void Accept(Player player)
    {
        var (mine, theirs) = Sides(player);
        mine.Accepted = true;

        if (!theirs.Accepted)
        {
            theirs.Player.Send(new MsgTrade { Id = player.Id, Action = TradeAction.Ok });
            return;
        }

        if (!CanSwap())
        {
            Close(TradeAction.Fail, "The trade failed.");
            return;
        }

        Close(TradeAction.Success, "The trade is done.");
        Hand(_first, _second);
        Hand(_second, _first);
    }

    public void Cancel() => Close(TradeAction.Fail, "The trade was called off.");

    private void Close(TradeAction outcome, string reason)
    {
        foreach (var side in (Side[])[_first, _second])
        {
            side.Player.Trade = null;
            side.Player.TradeRequest = 0;
            side.Player.Send(new MsgTrade { Action = outcome });
            side.Player.Tell(reason);
        }
    }

    private bool CanSwap() => _first.Player.Map == _second.Player.Map && _first.Player.CanSee(_second.Player) &&
                              Ready(_first, _second) && Ready(_second, _first);

    private static bool Ready(Side from, Side to) =>
        from.Player.Money >= from.Money && to.Player.CanEarn(from.Money) &&
        to.Player.FreeSlots + to.Items.Count >= from.Items.Count &&
        from.Items.All(item => item.Position == ItemPosition.Inventory && from.Player.Carried.Contains(item));

    private static void Hand(Side from, Side to)
    {
        from.Player.Pay(to.Player, from.Money);
        from.Items.ForEach(item => from.Player.Hand(to.Player, item));
    }

    private (Side Mine, Side Theirs) Sides(Player player) => player == _first.Player ? (_first, _second) : (_second, _first);

    private sealed class Side(Player player)
    {
        public Player Player { get; } = player;
        public List<Item> Items { get; } = [];
        public uint Money { get; set; }
        public bool Accepted { get; set; }
    }
}
