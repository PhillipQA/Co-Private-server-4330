namespace Conquer.Game.Messages;

public enum ItemInfoExAction : byte
{
    Booth = 1,
    Equipment = 2,
    OtherPlayerEquipment = 4
}

public record MsgItemInfoEx : IMessage
{
    public MsgItemInfoEx()
    {
    }

    public MsgItemInfoEx(Booth booth, Item item, uint price)
    {
        Id = item.Id;
        OwnerId = booth.Id;
        Price = price;
        Item = item;
        Action = ItemInfoExAction.Booth;
    }

    public uint Id { get; set; }
    public uint OwnerId { get; set; }
    public uint Price { get; set; }
    public Item Item { get; set; } = new();
    public ItemInfoExAction Action { get; set; }
    public ushort Size => 40;
    public MessageType Type => MessageType.MsgItemInfoEx;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        OwnerId = ReadUInt32LittleEndian(buffer[8..]);
        Price = ReadUInt32LittleEndian(buffer[12..]);
        Item = ItemBlock.Read(buffer[16..]);
        Action = (ItemInfoExAction)buffer[24];
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], OwnerId);
        WriteUInt32LittleEndian(buffer[12..], Price);
        ItemBlock.Write(buffer[16..], Item, (byte)Action);
    }
}