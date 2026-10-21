namespace Conquer.Game.Messages;

public record MsgFriendInfo : IMessage
{
    public MsgFriendInfo()
    {
    }

    public MsgFriendInfo(Player player, bool isEnemy = false)
    {
        Id = player.Id;
        Look = player.Look;
        Level = player.Level;
        Profession = player.Profession;
        PkPoints = player.PkPoints;
        GuildRank = player.ShownGuildRank;
        GuildId = player.GuildId ?? 0;
        IsEnemy = isEnemy;
        Spouse = player.Spouse;
    }

    public uint Id { get; set; }
    public uint Look { get; set; }
    public byte Level { get; set; }
    public Profession Profession { get; set; }
    public short PkPoints { get; set; }
    public GuildRank GuildRank { get; set; }
    public uint GuildId { get; set; }
    public string Spouse { get; set; } = null!;
    public bool IsEnemy { get; set; }

    public ushort Size => 40;
    public MessageType Type => MessageType.MsgFriendInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Look = ReadUInt32LittleEndian(buffer[8..]);
        Level = buffer[12];
        Profession = (Profession)buffer[13];
        PkPoints = ReadInt16LittleEndian(buffer[14..]);
        var guildIdRank = ReadUInt32LittleEndian(buffer[16..]);
        GuildRank = (GuildRank)(guildIdRank >> 24);
        GuildId = guildIdRank & 0x00FFFFFF;
        Spouse = buffer[20..36].ReadString();
        IsEnemy = buffer[36] != 0;
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Look);
        buffer[12] = Level;
        buffer[13] = (byte)Profession;
        WriteInt16LittleEndian(buffer[14..], PkPoints);
        WriteUInt32LittleEndian(buffer[16..], ((uint)GuildRank << 24) | (GuildId & 0x00FFFFFF));
        buffer[20..36].WriteString(Spouse);
        buffer[36] = (byte)(IsEnemy ? 1 : 0);
    }
}