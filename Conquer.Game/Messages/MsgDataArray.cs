namespace Conquer.Game.Messages;

public enum DataArrayAction : byte
{
    Compose = 0
}

public record MsgDataArray : IMessage
{
    private const int Capacity = 5;

    public DataArrayAction Action { get; set; }
    public List<uint> Data { get; set; } = [];

    public ushort Size => 28;
    public MessageType Type => MessageType.MsgDataArray;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Action = (DataArrayAction)buffer[4];
        Data = [];

        for (var i = 0; i < Math.Min((int)buffer[5], Capacity); i++)
        {
            Data.Add(ReadUInt32LittleEndian(buffer[(8 + i * 4)..]));
        }
    }

    public void Write(Span<byte> buffer)
    {
        buffer[4] = (byte)Action;
        buffer[5] = (byte)Data.Count;

        foreach (var (i, data) in Data.Index())
        {
            WriteUInt32LittleEndian(buffer[(8 + i * 4)..], data);
        }
    }

    public void Handle(Player player)
    {
        if (Action != DataArrayAction.Compose ||
            Data.Take(3).Concat(Data.Skip(3).Where(id => id != 0)).Select(player.FindItem).ToList() is not
                [{ } main, { } first, { } second, .. var gems] || gems.Contains(null))
        {
            return;
        }

        player.Compose(main, first, second, [.. gems.OfType<Item>()]);
    }
}
