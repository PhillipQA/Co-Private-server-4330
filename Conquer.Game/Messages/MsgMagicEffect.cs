namespace Conquer.Game.Messages;

public record MsgMagicEffect : IMessage
{
    public uint Sender { get; set; }
    public uint Target { get; set; }

    public ushort X
    {
        get => (ushort)Target;
        set => Target = (Target & 0xFFFF0000) | value;
    }

    public ushort Y
    {
        get => (ushort)(Target >> 16);
        set => Target = (Target & 0x0000FFFF) | ((uint)value << 16);
    }

    public ushort MagicType { get; set; }
    public ushort Level { get; set; }
    public List<(uint Target, uint Data)> Targets { get; set; } = [];
    public ushort Size => (ushort)(20 + Targets.Count * 8);
    public MessageType Type => MessageType.MsgMagicEffect;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Sender = ReadUInt32LittleEndian(buffer[4..]);
        Target = ReadUInt32LittleEndian(buffer[8..]);
        MagicType = ReadUInt16LittleEndian(buffer[12..]);
        Level = ReadUInt16LittleEndian(buffer[14..]);

        for (var i = 0; i < buffer[16]; i++)
        {
            Targets.Add((ReadUInt32LittleEndian(buffer[(20 + i * 8)..]), ReadUInt32LittleEndian(buffer[(24 + i * 8)..])));
        }
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Sender);
        WriteUInt32LittleEndian(buffer[8..], Target);
        WriteUInt16LittleEndian(buffer[12..], MagicType);
        WriteUInt16LittleEndian(buffer[14..], Level);
        buffer[16] = (byte)Targets.Count;

        foreach (var (i, (target, data)) in Targets.Index())
        {
            WriteUInt32LittleEndian(buffer[(20 + i * 8)..], target);
            WriteUInt32LittleEndian(buffer[(24 + i * 8)..], data);
        }
    }
}