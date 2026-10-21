namespace Conquer.Game.Messages;

public enum MapItemAction : uint
{
    Create = 1,
    Delete = 2,
    Pick = 3,
    CastTrap = 10,
    SyncTrap = 11,
    DropTrap = 12
}

public record MsgMapItem : IMessage
{
    private const uint PlentyOfMoney = 1000;

    public MsgMapItem()
    {
    }

    public MsgMapItem(MapItem mapItem, MapItemAction action)
    {
        Id = mapItem.Id;
        ItemTypeId = mapItem.ItemTypeId;
        X = mapItem.X;
        Y = mapItem.Y;
        Action = action;
    }

    public uint Id { get; set; }
    public uint ItemTypeId { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public MapItemAction Action { get; set; }

    public ushort Size => 20;
    public MessageType Type => MessageType.MsgMapItem;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        ItemTypeId = ReadUInt32LittleEndian(buffer[8..]);
        X = ReadUInt16LittleEndian(buffer[12..]);
        Y = ReadUInt16LittleEndian(buffer[14..]);
        Action = (MapItemAction)ReadUInt32LittleEndian(buffer[16..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], ItemTypeId);
        WriteUInt16LittleEndian(buffer[12..], X);
        WriteUInt16LittleEndian(buffer[14..], Y);
        WriteUInt32LittleEndian(buffer[16..], (uint)Action);
    }

    public void Handle(Player player, Map map, World world)
    {
        if (Action != MapItemAction.Pick)
        {
            return;
        }

        if (!player.SyncPosition(X, Y))
        {
            player.Client.Close();
            return;
        }

        if (!player.IsAlive || map.Find<MapItem>(Id) is not { } drop || (drop.X, drop.Y) != (player.X, player.Y))
        {
            return;
        }

        if (drop.IsReservedFrom(player, world.UtcNow))
        {
            player.Tell("This item belongs to someone else for now.");
            return;
        }

        if (drop.Item is null)
        {
            if (!player.Earn(drop.Money))
            {
                player.Tell("You cannot carry any more silver.");
                return;
            }

            if (drop.Money >= PlentyOfMoney)
            {
                player.Send(new MsgAction
                {
                    Id = player.Id,
                    X = player.X,
                    Y = player.Y,
                    Direction = (ushort)player.Direction,
                    Data = drop.Money,
                    Action = ActionType.GetMoney
                });
            }

            player.Tell($"You have picked up {drop.Money} silver.");
        }
        else if (!player.Receive(drop.Item))
        {
            return;
        }

        map.Broadcast(player, new MsgMapItem
        {
            Id = player.Id,
            X = player.X,
            Y = player.Y,
            Action = MapItemAction.Pick
        }, self: false);
        map.Remove(drop);
    }
}
