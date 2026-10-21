namespace Conquer.Game.Models;

public sealed class Team
{
    public const int Capacity = 4;

    private static readonly TimeSpan PositionInterval = TimeSpan.FromSeconds(5);

    private DateTimeOffset _nextPosition;

    public Team(Player leader)
    {
        Leader = leader;
        Members = [leader];
        leader.Team = this;
    }

    public Player Leader { get; }
    public List<Player> Members { get; }
    public HashSet<uint> Applicants { get; } = [];
    public HashSet<uint> Invitees { get; } = [];
    public bool IsClosed { get; set; }
    public bool SharesMoney { get; set; } = true;
    public bool SharesItems { get; set; } = true;

    public bool IsFull => Members.Count >= Capacity;

    public bool Has(uint playerId) => Members.Any(member => member.Id == playerId);

    public bool Add(Player player)
    {
        if (IsClosed || IsFull || player.Team is not null)
        {
            return false;
        }

        foreach (var member in Members)
        {
            member.Send(new MsgTeamMember(player, TeamMemberAction.AddMember));
            player.Send(new MsgTeamMember(member, TeamMemberAction.AddMember));
        }

        player.Team = this;
        Applicants.Remove(player.Id);
        Members.Add(player);
        player.Send(new MsgTeamMember(player, TeamMemberAction.AddMember));
        return true;
    }

    public void Remove(Player player, TeamAction reason = TeamAction.Leave)
    {
        if (player == Leader)
        {
            Dismiss();
            return;
        }

        Send(new MsgTeam { Id = player.Id, Action = reason });
        Members.Remove(player);
        player.Team = null;
    }

    public void Dismiss()
    {
        foreach (var member in Members)
        {
            member.Send(new MsgTeam { Id = Leader.Id, Action = TeamAction.Dismiss });
            member.Team = null;
        }

        Leader.Flag(PlayerStatus.TeamLeader, false);
        Members.Clear();
    }

    public void Tick(DateTimeOffset now)
    {
        if (now < _nextPosition)
        {
            return;
        }

        _nextPosition = now + PositionInterval;
        Members.Where(member => member != Leader && member.MapId == Leader.MapId).Send(Leader.Act(ActionType.TeamMemberPos, Leader.MapId));
    }

    public void Send(IMessage message, Player? sender = null) => Members.Where(member => member != sender).Send(message);
}
