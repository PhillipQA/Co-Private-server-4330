namespace Conquer.Game.Messages;

public record MsgPetInfo : IMessage
{
    public MsgPetInfo()
    {
    }

    public MsgPetInfo(Monster pet, ushort x, ushort y)
    {
        Id = pet.Id;
        Look = pet.MonsterType.Look;
        AiType = pet.MonsterType.AiType;
        X = x;
        Y = y;
        Name = pet.MonsterType.Name;
    }

    public uint Id { get; set; }
    public uint Look { get; set; }
    public uint AiType { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public string Name { get; set; } = "";
    public ushort Size => 36;
    public MessageType Type => MessageType.MsgPetInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Look = ReadUInt32LittleEndian(buffer[8..]);
        AiType = ReadUInt32LittleEndian(buffer[12..]);
        X = ReadUInt16LittleEndian(buffer[16..]);
        Y = ReadUInt16LittleEndian(buffer[18..]);
        Name = buffer[20..36].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Look);
        WriteUInt32LittleEndian(buffer[12..], AiType);
        WriteUInt16LittleEndian(buffer[16..], X);
        WriteUInt16LittleEndian(buffer[18..], Y);
        buffer[20..36].WriteString(Name);
    }
}
