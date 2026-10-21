namespace Conquer.Game.Messages;

public record MsgNpcInfo : IMessage
{
    public MsgNpcInfo()
    {
    }

    public MsgNpcInfo(Npc npc)
    {
        Id = npc.Id;
        X = npc.X;
        Y = npc.Y;
        Look = (ushort)npc.Look;
        NpcType = npc.Type;
        NpcSort = npc.Sort;
        Strings = [];
    }

    public MsgNpcInfo(DynaNpc npc)
    {
        Id = npc.Id;
        X = npc.X;
        Y = npc.Y;
        Look = (ushort)npc.Look;
        NpcType = npc.Type;
        NpcSort = npc.Sort;
        Strings = [];
    }

    public uint Id { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort Look { get; set; }
    public ushort NpcType { get; set; }
    public ushort NpcSort { get; set; }
    public List<string> Strings { get; set; } = null!;

    public ushort Size => (ushort)(18 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgNpcInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        X = ReadUInt16LittleEndian(buffer[8..]);
        Y = ReadUInt16LittleEndian(buffer[10..]);
        Look = ReadUInt16LittleEndian(buffer[12..]);
        NpcType = ReadUInt16LittleEndian(buffer[14..]);
        NpcSort = ReadUInt16LittleEndian(buffer[16..]);
        Strings = buffer[18..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt16LittleEndian(buffer[8..], X);
        WriteUInt16LittleEndian(buffer[10..], Y);
        WriteUInt16LittleEndian(buffer[12..], Look);
        WriteUInt16LittleEndian(buffer[14..], NpcType);
        WriteUInt16LittleEndian(buffer[16..], NpcSort);
        buffer[18..].WriteStrings(Strings);
    }

    public void Handle(Player player) => player.Dialog?.SpotPicked(this);
}