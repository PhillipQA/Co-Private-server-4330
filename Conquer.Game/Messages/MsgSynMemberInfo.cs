namespace Conquer.Game.Messages;

public record MsgSynMemberInfo : IMessage
{
    public uint Donation { get; set; }
    public GuildRank Rank { get; set; }
    public string Name { get; set; } = "";
    public ushort Size => 28;
    public MessageType Type => MessageType.MsgSynMemberInfo;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Donation = ReadUInt32LittleEndian(buffer[4..]);
        Rank = (GuildRank)buffer[8];
        Name = buffer[9..25].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Donation);
        buffer[8] = (byte)Rank;
        buffer[9..25].WriteString(Name);
    }

    public void Handle(Player player, World world)
    {
        if (player.Guild is { } guild && world.FamilyMembers(guild).FirstOrDefault(member => member.Name == Name) is { } member)
        {
            player.Send(this with { Donation = member.GuildDonation, Rank = member.ShownGuildRank, Name = member.Name });
        }
    }
}
