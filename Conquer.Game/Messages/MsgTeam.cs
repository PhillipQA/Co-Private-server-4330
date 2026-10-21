namespace Conquer.Game.Messages;

public enum TeamAction
{
    Create = 0,
    ApplyJoin = 1,
    Leave = 2,
    AcceptInvite = 3,
    Invite = 4,
    AgreeJoin = 5,
    Dismiss = 6,
    KickOut = 7,
    CloseTeam = 8,
    OpenTeam = 9,
    CloseMoneyAccess = 10,
    OpenMoneyAccess = 11,
    CloseItemAccess = 12,
    OpenItemAccess = 13
}

public record MsgTeam : IMessage
{
    public TeamAction Action { get; set; }
    public uint Id { get; set; }
    public ushort Size => 12;
    public MessageType Type => MessageType.MsgTeam;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Action = (TeamAction)ReadUInt32LittleEndian(buffer[4..]);
        Id = ReadUInt32LittleEndian(buffer[8..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], (uint)Action);
        WriteUInt32LittleEndian(buffer[8..], Id);
    }

    public void Handle(Player player, World world)
    {
        var other = world.Players.GetValueOrDefault(Id);
        var leading = player.Team is { } own && own.Leader == player ? own : null;

        switch (Action)
        {
            case TeamAction.Create when player.Team is null && MayTeam(player):
                _ = new Team(player);
                player.Flag(PlayerStatus.TeamLeader);
                player.Send(this);
                break;

            case TeamAction.Leave when player.Team is { } leaving:
                leaving.Remove(player);
                break;

            case TeamAction.Dismiss when leading is not null:
                leading.Dismiss();
                break;

            case TeamAction.ApplyJoin when Joinable(other, player) is { } applied:
                applied.Applicants.Add(player.Id);
                applied.Leader.Send(new MsgTeam { Id = player.Id, Action = TeamAction.ApplyJoin });
                player.Tell("Your request has been sent.");
                break;

            case TeamAction.AcceptInvite when Inviting(other, player) is { } invited:
                invited.Add(player);
                break;

            case TeamAction.Invite when leading is { IsFull: false } && other is { Team: null }:
                leading.Invitees.Add(other.Id);
                other.Send(new MsgTeam { Id = player.Id, Action = TeamAction.Invite });
                player.Tell("Your invitation has been sent.");
                break;

            case TeamAction.AgreeJoin when leading is { IsFull: false } && other is { Team: null } && MayTeam(other) && leading.Applicants.Remove(Id):
                other.Send(new MsgTeam { Id = player.Id, Action = TeamAction.AgreeJoin });
                leading.Add(other);
                break;

            case TeamAction.KickOut when leading is not null && other is not null && other != player && leading.Has(other.Id):
                leading.Remove(other, TeamAction.KickOut);
                break;

            case TeamAction.CloseTeam or TeamAction.OpenTeam when leading is not null:
                leading.IsClosed = Action == TeamAction.CloseTeam;
                break;

            case TeamAction.CloseMoneyAccess or TeamAction.OpenMoneyAccess when leading is not null:
                leading.SharesMoney = Action == TeamAction.OpenMoneyAccess;
                break;

            case TeamAction.CloseItemAccess or TeamAction.OpenItemAccess when leading is not null:
                leading.SharesItems = Action == TeamAction.OpenItemAccess;
                break;
        }
    }

    private static Team? Joinable(Player? leader, Player player) =>
        leader?.Team is { IsClosed: false, IsFull: false } team && team.Leader == leader && player.Team is null && MayTeam(player) ? team : null;

    private static Team? Inviting(Player? leader, Player player) =>
        leader?.Team is { IsFull: false } team && team.Leader == leader && player.Team is null && MayTeam(player) &&
        leader.Map == player.Map && player.DistanceTo(leader) <= Map.ViewRange && team.Invitees.Remove(player.Id) ? team : null;

    private static bool MayTeam(Player player) => !player.Map!.Info.Is(MapFlags.TeamDisable);
}