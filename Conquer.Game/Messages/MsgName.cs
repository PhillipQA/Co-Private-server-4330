namespace Conquer.Game.Messages;

public enum NameAction : byte
{
    Fireworks = 1,
    CreateGuild = 2,
    Guild = 3,
    ChangeTitle = 4,
    DeleteCharacter = 5,
    Spouse = 6,
    QueryNpc = 7,
    Wanted = 8,
    MapEffect = 9,
    RoleEffect = 10,
    MemberList = 11,
    KickOutGuildMember = 12,
    QueryWanted = 13,
    QueryPoliceWanted = 14,
    PoliceWanted = 15,
    QuerySpouse = 16,
    AddDicePlayer = 17,
    RemoveDicePlayer = 18,
    DiceBonus = 19,
    Sound = 20,
    GuildAlly = 21,
    GuildEnemy = 22
}

public record MsgName : IMessage
{
    private const int MemberListPage = 18;

    public uint Id { get; set; }

    public ushort X
    {
        get => (ushort)Id;
        set => Id = (Id & 0xFFFF0000) | value;
    }

    public ushort Y
    {
        get => (ushort)(Id >> 16);
        set => Id = (Id & 0x0000FFFF) | ((uint)value << 16);
    }

    public NameAction Action { get; set; }
    public List<string> Strings { get; set; } = null!;

    public ushort Size => (ushort)(9 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgName;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Action = (NameAction)buffer[8];
        Strings = buffer[9..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        buffer[8] = (byte)Action;
        buffer[9..].WriteStrings(Strings);
    }

    public async Task HandleAsync(Player player, Map map, World world, GameStore store, ILogger<MsgName> logger)
    {
        switch (Action)
        {
            case NameAction.MemberList:
                if (player.Guild is not { } guild)
                {
                    break;
                }

                var family = world.Family(guild).Select(other => other.Id).ToList();
                var saved = await store.GuildMembersAsync(family);

                var strings = world.Players.Values
                    .Where(online => online.GuildId is { } guildId && family.Contains(guildId))
                    .Select(online => (online.Name, online.Level, Rank: online.GuildRank, IsOnline: true))
                    .Concat(saved.Where(member => !world.Players.ContainsKey(member.Id))
                        .Select(member => (member.Name, member.Level, member.Rank, IsOnline: false)))
                    .OrderByDescending(member => member.IsOnline)
                    .ThenByDescending(member => member.Rank)
                    .Skip((int)Id)
                    .Take(MemberListPage)
                    .Select(member => $"{member.Name} {member.Level} {(member.IsOnline ? 1 : 0)}")
                    .ToList();

                player.Send(new MsgName { Id = Id, Action = NameAction.MemberList, Strings = strings });
                break;

            case NameAction.AddDicePlayer:
                map.Find<Npc>(Id)?.Dice?.Join(player, world.UtcNow);
                break;

            case NameAction.RemoveDicePlayer:
                map.Find<Npc>(Id)?.Dice?.Leave(player);
                break;

            case NameAction.QueryWanted:
                player.SeekBounty();
                break;

            case NameAction.QueryPoliceWanted:
                player.SeekFugitive();
                break;

            case NameAction.QuerySpouse:
                if (world.Players.GetValueOrDefault(Id) is { } queried)
                {
                    player.Send(new MsgName { Id = queried.Id, Action = NameAction.QuerySpouse, Strings = [queried.Spouse] });
                }
                break;

            default:
                logger.WarnOnce("Unhandled action {Action}.", Action);
                break;
        }
    }
}