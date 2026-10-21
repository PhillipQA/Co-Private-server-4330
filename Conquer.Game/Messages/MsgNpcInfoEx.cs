namespace Conquer.Game.Messages;

public record MsgNpcInfoEx : IMessage
{
    public MsgNpcInfoEx()
    {
    }

    public MsgNpcInfoEx(DynaNpc dynaNpc)
    {
        Id = dynaNpc.Id;
        MaxHealth = dynaNpc.MaxHealth;
        Health = (uint)dynaNpc.Health;
        X = dynaNpc.X;
        Y = dynaNpc.Y;
        Look = (ushort)dynaNpc.Look;
        NpcType = dynaNpc.Type;
        NpcSort = dynaNpc.Sort;
        Strings = dynaNpc.IsPole ? [dynaNpc.Title] : [];
    }

    public uint Id { get; set; }
    public uint MaxHealth { get; set; }
    public uint Health { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort Look { get; set; }
    public ushort NpcType { get; set; }
    public ushort NpcSort { get; set; }
    public List<string> Strings { get; set; } = null!;
    public ushort Size => (ushort)(26 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgNpcInfoEx;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        MaxHealth = ReadUInt32LittleEndian(buffer[8..]);
        Health = ReadUInt32LittleEndian(buffer[12..]);
        X = ReadUInt16LittleEndian(buffer[16..]);
        Y = ReadUInt16LittleEndian(buffer[18..]);
        Look = ReadUInt16LittleEndian(buffer[20..]);
        NpcType = ReadUInt16LittleEndian(buffer[22..]);
        NpcSort = ReadUInt16LittleEndian(buffer[24..]);
        Strings = buffer[26..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], MaxHealth);
        WriteUInt32LittleEndian(buffer[12..], Health);
        WriteUInt16LittleEndian(buffer[16..], X);
        WriteUInt16LittleEndian(buffer[18..], Y);
        WriteUInt16LittleEndian(buffer[20..], Look);
        WriteUInt16LittleEndian(buffer[22..], NpcType);
        WriteUInt16LittleEndian(buffer[24..], NpcSort);
        buffer[26..].WriteStrings(Strings);
    }
}