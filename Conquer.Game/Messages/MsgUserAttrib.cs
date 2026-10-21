namespace Conquer.Game.Messages;

public enum AttributeType : uint
{
    Health = 0,
    MaxHealth = 1,
    Mana = 2,
    MaxMana = 3,
    Money = 4,
    Exp = 5,
    PkPoints = 6,
    Profession = 7,
    Size = 8,
    Stamina = 9,
    AttributePoints = 11,
    Look = 12,
    Level = 13,
    Spirit = 14,
    Vitality = 15,
    Strength = 16,
    Agility = 17,
    BlessTime = 18,
    DoubleExpTime = 19,
    Donation = 20,
    CurseTime = 21,
    TimeAdd = 22,
    Rebirths = 23,
    Flags = 26,
    Hair = 27,
    Xp = 28,
    LuckyTime = 29
}

public record MsgUserAttrib : IMessage
{
    public MsgUserAttrib()
    {
    }

    public MsgUserAttrib(uint id, AttributeType type, uint value)
    {
        Id = id;
        Attributes = [(type, value)];
    }

    public uint Id { get; set; }
    public List<(AttributeType Type, uint Value)> Attributes { get; set; } = [];
    public ushort Size => (ushort)(12 + Attributes.Count * 8);
    public MessageType Type => MessageType.MsgUserAttrib;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        var count = ReadInt32LittleEndian(buffer[8..]);

        for (var i = 0; i < count; i++)
        {
            Attributes.Add(((AttributeType)ReadUInt32LittleEndian(buffer[(12 + i * 8)..]), ReadUInt32LittleEndian(buffer[(16 + i * 8)..])));
        }
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteInt32LittleEndian(buffer[8..], Attributes.Count);

        foreach (var (i, (type, value)) in Attributes.Index())
        {
            WriteUInt32LittleEndian(buffer[(12 + i * 8)..], (uint)type);
            WriteUInt32LittleEndian(buffer[(16 + i * 8)..], value);
        }
    }
}