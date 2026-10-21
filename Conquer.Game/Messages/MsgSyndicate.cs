namespace Conquer.Game.Messages;

public enum SyndicateAction : uint
{
    None = 0,
    ApplyJoin = 1,
    InviteJoin = 2,
    LeaveGuild = 3,
    KickOutMember = 4,
    QueryGuildName = 6,
    SetAlly = 7,
    ClearAlly = 8,
    SetEnemy = 9,
    ClearEnemy = 10,
    DonateMoney = 11,
    QueryGuildInfo = 12,
    SetGuild = 14,
    MergeBranch = 15,
    MergeGuild = 16,
    SetWhiteGuild = 17,
    SetBlackGuild = 18,
    DisbandGuild = 19,
    SetMantle = 20
}

public record MsgSyndicate : IMessage
{
    public SyndicateAction Action { get; set; }
    public uint Data { get; set; }
    public ushort Size => 12;
    public MessageType Type => MessageType.MsgSyndicate;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Action = (SyndicateAction)ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt32LittleEndian(buffer[8..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], (uint)Action);
        WriteUInt32LittleEndian(buffer[8..], Data);
    }

    public void Handle(Player player, World world, GuildService guilds, ILogger<MsgSyndicate> logger)
    {
        switch (Action)
        {
            case SyndicateAction.QueryGuildName:
                if (world.Guilds.GetValueOrDefault(Data) is { } guild && world.MainGuildOf(guild) is var master)
                {
                    player.Send(new MsgName
                    {
                        Id = Data,
                        Action = NameAction.Guild,
                        Strings = master == guild ? [guild.Name] : [master.Name, guild.Name]
                    });
                }
                break;

            case SyndicateAction.QueryGuildInfo:
                guilds.SendGuildInfo(player);
                break;

            case SyndicateAction.ApplyJoin:
                Apply();
                break;

            case SyndicateAction.InviteJoin:
                Invite();
                break;

            case SyndicateAction.LeaveGuild:
                guilds.Leave(player);
                break;

            case SyndicateAction.KickOutMember:
                if (world.Players.GetValueOrDefault(Data) is { } member)
                {
                    guilds.Kick(player, member);
                }
                break;

            case SyndicateAction.DonateMoney:
                guilds.Donate(player, Data);
                break;

            default:
                logger.WarnOnce("Unhandled action {Action}.", Action);
                break;
        }

        void Apply()
        {
            if (world.Players.GetValueOrDefault(Data) is not { GuildId: not null, GuildRank: >= GuildRank.DeputyLeader } officer ||
                player.GuildId is not null)
            {
                return;
            }

            if (officer.GuildInvitation == player.Id)
            {
                officer.GuildInvitation = 0;
                guilds.Join(player, officer);
                return;
            }

            player.GuildApplication = officer.Id;
            officer.Send(new MsgSyndicate { Action = SyndicateAction.ApplyJoin, Data = player.Id });
        }

        void Invite()
        {
            if (world.Players.GetValueOrDefault(Data) is not { GuildId: null } invited || player.GuildId is null ||
                player.GuildRank < GuildRank.DeputyLeader)
            {
                return;
            }

            if (invited.GuildApplication == player.Id)
            {
                guilds.Join(invited, player);
                return;
            }

            player.GuildInvitation = invited.Id;
            invited.Send(new MsgSyndicate { Action = SyndicateAction.InviteJoin, Data = player.Id });
        }
    }
}
