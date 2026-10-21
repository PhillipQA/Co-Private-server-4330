namespace Conquer.Game.Messages;

public record MsgUserInfo : IMessage
{
    public MsgUserInfo()
    {
    }

    public MsgUserInfo(Player player)
    {
        Id = player.Id;
        Look = player.Look;
        Hair = player.Hair;
        Money = player.Money;
        Experience = player.Experience;
        Strength = player.Strength;
        Agility = player.Agility;
        Vitality = player.Vitality;
        Spirit = player.Spirit;
        AttributePoints = player.AttributePoints;
        Health = (ushort)player.Health;
        Mana = player.Mana;
        PkPoints = player.PkPoints;
        Level = player.Level;
        Profession = player.Profession;
        AutoAllot = player.Rebirths == 0;
        Rebirths = player.Rebirths;
        Strings = [player.Name, player.Spouse];
    }

    public uint Id { get; set; }
    public uint Look { get; set; }
    public ushort Hair { get; set; }
    public uint Money { get; set; }
    public ulong Experience { get; set; }
    public ushort Strength { get; set; }
    public ushort Agility { get; set; }
    public ushort Vitality { get; set; }
    public ushort Spirit { get; set; }
    public ushort AttributePoints { get; set; }
    public ushort Health { get; set; }
    public ushort Mana { get; set; }
    public short PkPoints { get; set; }
    public byte Level { get; set; }
    public Profession Profession { get; set; }
    public bool AutoAllot { get; set; }
    public byte Rebirths { get; set; }
    public List<string> Strings { get; set; } = null!;

    public ushort Size => (ushort)(61 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgUserInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Look = ReadUInt32LittleEndian(buffer[8..]);
        Hair = ReadUInt16LittleEndian(buffer[12..]);
        Money = ReadUInt32LittleEndian(buffer[16..]);
        Experience = ReadUInt32LittleEndian(buffer[20..]);
        Strength = ReadUInt16LittleEndian(buffer[40..]);
        Agility = ReadUInt16LittleEndian(buffer[42..]);
        Vitality = ReadUInt16LittleEndian(buffer[44..]);
        Spirit = ReadUInt16LittleEndian(buffer[46..]);
        AttributePoints = ReadUInt16LittleEndian(buffer[48..]);
        Health = ReadUInt16LittleEndian(buffer[50..]);
        Mana = ReadUInt16LittleEndian(buffer[52..]);
        PkPoints = ReadInt16LittleEndian(buffer[54..]);
        Level = buffer[56];
        Profession = (Profession)buffer[57];
        AutoAllot = buffer[58] != 0;
        Rebirths = buffer[59];
        var hasName = buffer[60] != 0;
        if (hasName)
        {
            Strings = buffer[61..].ReadStrings();
        }
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Look);
        WriteUInt16LittleEndian(buffer[12..], Hair);
        WriteUInt32LittleEndian(buffer[16..], Money);
        WriteUInt32LittleEndian(buffer[20..], (uint)Experience);
        WriteUInt16LittleEndian(buffer[40..], Strength);
        WriteUInt16LittleEndian(buffer[42..], Agility);
        WriteUInt16LittleEndian(buffer[44..], Vitality);
        WriteUInt16LittleEndian(buffer[46..], Spirit);
        WriteUInt16LittleEndian(buffer[48..], AttributePoints);
        WriteUInt16LittleEndian(buffer[50..], Health);
        WriteUInt16LittleEndian(buffer[52..], Mana);
        WriteInt16LittleEndian(buffer[54..], PkPoints);
        buffer[56] = Level;
        buffer[57] = (byte)Profession;
        buffer[58] = (byte)(AutoAllot ? 1 : 0);
        buffer[59] = Rebirths;
        buffer[60] = (byte)(Strings.Count != 0 ? 1 : 0);
        buffer[61..].WriteStrings(Strings);
    }
}