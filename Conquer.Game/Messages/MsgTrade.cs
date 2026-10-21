namespace Conquer.Game.Messages;

public enum TradeAction : uint
{
    Apply = 1,
    Quit = 2,
    Open = 3,
    Success = 4,
    Fail = 5,
    AddItem = 6,
    AddMoney = 7,
    AllMoney = 8,
    SelfAllMoney = 9,
    Ok = 10,
    AddItemFail = 11
}

public record MsgTrade : IMessage
{
    public uint Id { get; set; }
    public TradeAction Action { get; set; }
    public ushort Size => 12;
    public MessageType Type => MessageType.MsgTrade;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Action = (TradeAction)ReadUInt32LittleEndian(buffer[8..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], (uint)Action);
    }

    public void Handle(Player player, World world)
    {
        switch (Action)
        {
            case TradeAction.Apply:
                Apply(player, world.Players.GetValueOrDefault(Id));
                break;

            case TradeAction.Quit:
                player.Trade?.Cancel();
                break;

            case TradeAction.AddItem when player.Trade is { } trade:
                if (player.FindItem(Id) is not { } item || !trade.Offer(player, item))
                {
                    player.Send(new MsgTrade { Id = Id, Action = TradeAction.AddItemFail });
                }
                break;

            case TradeAction.AddMoney:
                player.Trade?.Offer(player, Id);
                break;

            case TradeAction.Ok:
                player.Trade?.Accept(player);
                break;
        }
    }

    private static void Apply(Player player, Player? other)
    {
        if (other is null || other == player || other.Map != player.Map || !player.CanSee(other))
        {
            player.Tell("There is nobody there to trade with.");
            return;
        }

        if (player.Trade is not null || other.Trade is not null)
        {
            player.Tell("Someone is trading already.");
            return;
        }

        if (other.TradeRequest != player.Id)
        {
            player.TradeRequest = other.Id;
            other.Send(new MsgTrade { Id = player.Id, Action = TradeAction.Apply });
            player.Tell("Your trade request has been sent.");
            return;
        }

        player.TradeRequest = 0;
        other.TradeRequest = 0;
        _ = new Trade(player, other);
    }
}