namespace Conquer.Game.Messages;

public enum GuildRank : byte
{
    None = 0,
    Member = 50,
    InternManager = 60,
    DeputyManager = 70,
    BranchManager = 80,
    DeputyLeader = 90,
    Leader = 100
}

public record MsgSyndicateAttributeInfo : IMessage
{
    public MsgSyndicateAttributeInfo()
    {
    }

    public MsgSyndicateAttributeInfo(Player player)
    {
        if (player.Guild is not { } guild)
        {
            LeaderName = "";
            return;
        }

        var world = player.World;
        var master = world.MainGuildOf(guild);
        GuildId = guild.Id;
        Donation = player.GuildDonation;
        Fund = guild.Fund;
        GuildRank = player.ShownGuildRank;
        LeaderName = master.LeaderName;
        MemberCount = (uint)world.Family(guild).Sum(other => other.MemberCount);
    }

    public uint GuildId { get; set; }
    public uint Donation { get; set; }
    public uint Fund { get; set; }
    public uint MemberCount { get; set; }
    public GuildRank GuildRank { get; set; }
    public string LeaderName { get; set; } = null!;
    public ushort Size => 40;
    public MessageType Type => MessageType.MsgSyndicateAttributeInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        GuildId = ReadUInt32LittleEndian(buffer[4..]);
        Donation = ReadUInt32LittleEndian(buffer[8..]);
        Fund = ReadUInt32LittleEndian(buffer[12..]);
        MemberCount = ReadUInt32LittleEndian(buffer[16..]);
        GuildRank = (GuildRank)buffer[20];
        LeaderName = buffer[21..37].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], GuildId);
        WriteUInt32LittleEndian(buffer[8..], Donation);
        WriteUInt32LittleEndian(buffer[12..], Fund);
        WriteUInt32LittleEndian(buffer[16..], MemberCount);
        buffer[20] = (byte)GuildRank;
        buffer[21..37].WriteString(LeaderName);
    }
}