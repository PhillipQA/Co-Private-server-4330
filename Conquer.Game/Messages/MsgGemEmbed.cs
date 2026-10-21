namespace Conquer.Game.Messages;

public enum GemAction : ushort
{
    Embed = 0,
    TakeOut = 1
}

public record MsgGemEmbed : IMessage
{
    private const byte Jeweler = 7;

    public uint PlayerId { get; set; }
    public uint ItemId { get; set; }
    public uint GemId { get; set; }
    public ushort Slot { get; set; }
    public GemAction Action { get; set; }

    public ushort Size => 20;
    public MessageType Type => MessageType.MsgGemEmbed;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        PlayerId = ReadUInt32LittleEndian(buffer[4..]);
        ItemId = ReadUInt32LittleEndian(buffer[8..]);
        GemId = ReadUInt32LittleEndian(buffer[12..]);
        Slot = ReadUInt16LittleEndian(buffer[16..]);
        Action = (GemAction)ReadUInt16LittleEndian(buffer[18..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], PlayerId);
        WriteUInt32LittleEndian(buffer[8..], ItemId);
        WriteUInt32LittleEndian(buffer[12..], GemId);
        WriteUInt16LittleEndian(buffer[16..], Slot);
        WriteUInt16LittleEndian(buffer[18..], (ushort)Action);
    }

    public void Handle(Player player)
    {
        if (player.Id != PlayerId || !player.IsNear(Jeweler) || player.FindItem(ItemId) is not { } item)
        {
            return;
        }

        if (Action == GemAction.TakeOut)
        {
            player.RemoveGem(item, Slot);
        }
        else if (player.FindItem(GemId) is { } gem)
        {
            player.Embed(item, gem, Slot);
        }
    }
}
