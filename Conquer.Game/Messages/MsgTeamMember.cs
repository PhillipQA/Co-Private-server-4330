namespace Conquer.Game.Messages;

public enum TeamMemberAction : byte
{
    AddMember = 0,
    RemoveMember = 1
}

public record MsgTeamMember : IMessage
{
    public MsgTeamMember()
    {
    }

    public MsgTeamMember(Player player, TeamMemberAction action)
    {
        Action = action;
        Members = [(player.Name, player.Id, player.Look, (ushort)player.Stats.MaxHealth, (ushort)player.Health)];
    }

    public TeamMemberAction Action { get; set; }
    public List<(string Name, uint Id, uint LookFace, ushort MaxHealth, ushort Health)> Members { get; set; } = [];
    public ushort Size => (ushort)(8 + Members.Count * 28);
    public MessageType Type => MessageType.MsgTeamMember;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Action = (TeamMemberAction)buffer[4];

        for (var i = 0; i < buffer[5]; i++)
        {
            var entry = buffer[(8 + i * 28)..];
            Members.Add((entry[..16].ReadString(), ReadUInt32LittleEndian(entry[16..]), ReadUInt32LittleEndian(entry[20..]),
                ReadUInt16LittleEndian(entry[24..]), ReadUInt16LittleEndian(entry[26..])));
        }
    }

    public void Write(Span<byte> buffer)
    {
        buffer[4] = (byte)Action;
        buffer[5] = (byte)Members.Count;

        foreach (var (i, (name, id, lookFace, maxHealth, health)) in Members.Index())
        {
            var entry = buffer[(8 + i * 28)..];
            entry[..16].WriteString(name);
            WriteUInt32LittleEndian(entry[16..], id);
            WriteUInt32LittleEndian(entry[20..], lookFace);
            WriteUInt16LittleEndian(entry[24..], maxHealth);
            WriteUInt16LittleEndian(entry[26..], health);
        }
    }
}