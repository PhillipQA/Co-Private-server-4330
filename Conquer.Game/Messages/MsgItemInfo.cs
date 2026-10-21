namespace Conquer.Game.Messages;

public enum ItemInfoAction : byte
{
    None = 0,
    AddItem = 1,
    Trade = 2,
    Update = 3,
    OtherPlayerEquipment = 4,
    Auction = 5
}

public record MsgItemInfo : IMessage
{
    public MsgItemInfo()
    {
    }

    public MsgItemInfo(Item item, ItemInfoAction action)
    {
        ItemId = item.Id;
        Item = item;
        Action = action;
    }

    public uint ItemId { get; set; }
    public Item Item { get; set; } = new();
    public ItemInfoAction Action { get; set; }

    public ushort Size => 32;
    public MessageType Type => MessageType.MsgItemInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        ItemId = ReadUInt32LittleEndian(buffer[4..]);
        Item = ItemBlock.Read(buffer[8..]);
        Action = (ItemInfoAction)buffer[16];
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], ItemId);
        ItemBlock.Write(buffer[8..], Item, (byte)Action);
    }
}