namespace Conquer.Game.Services;

public class GuildService(World world, GameStore store)
{
    public const int MaxAnnouncementLength = 255;
    public const int MaxNameLength = 15;

    private const uint LeaveDonation = 20_000;
    private const int MaxRelations = 5;
    private const int MinNameLength = 2;

    private static readonly string[] Reserved = ["None", "GM", "PM", "SYSTEM"];

    public async Task<bool> CreateAsync(Player player, byte level, uint cost, uint fund, string name)
    {
        if (player.GuildId is not null || player.Level < level || IsFounding(player) || !IsAvailable(player, name) || !player.Spend(cost))
        {
            return false;
        }

        var guild = await FoundAsync(player, cost, name, fund);
        await AddMemberAsync(Current(player), guild, GuildRank.Leader, fund);
        world.Announce($"{player.Name} has founded the guild {name}.");
        return true;
    }

    public async Task<bool> DisbandAsync(Player player)
    {
        if (Led(player) is not { } guild)
        {
            return false;
        }

        var master = guild.ParentGuildId is { } parentId ? world.Guilds.GetValueOrDefault(parentId) : null;

        foreach (var member in Online(guild.Id).ToList())
        {
            if (master is null)
            {
                RemoveMember(member);
            }
            else
            {
                member.SetGuild(master.Id, GuildRank.Member, member.GuildDonation);
            }

            SendGuildInfo(member);
        }

        foreach (var other in world.Guilds.Values)
        {
            if (other.Allies.RemoveAll(ally => ally.AllyGuildId == guild.Id) > 0)
            {
                SendToFamily(other, SyndicateAction.ClearAlly, guild.Id);
            }

            if (other.Enemies.RemoveAll(enemy => enemy.EnemyGuildId == guild.Id) > 0)
            {
                SendToFamily(other, SyndicateAction.ClearEnemy, guild.Id);
            }

            if (other.ParentGuildId == guild.Id)
            {
                other.ParentGuildId = null;

                foreach (var member in Online(other.Id))
                {
                    member.Map?.Broadcast(member, member.Spawn);
                    guild.Allies.ForEach(ally => member.Send(new MsgSyndicate { Data = ally.AllyGuildId, Action = SyndicateAction.ClearAlly }));
                    guild.Enemies.ForEach(enemy => member.Send(new MsgSyndicate { Data = enemy.EnemyGuildId, Action = SyndicateAction.ClearEnemy }));
                    SendGuildInfo(member);
                }
            }
        }

        world.Guilds.Remove(guild.Id);
        world.Announce($"The guild {guild.Name} is no more.");

        if (master is null)
        {
            await store.DisbandAsync(guild.Id);
        }
        else
        {
            master.MemberCount += guild.MemberCount;
            await store.MergeAsync(guild.Id, master.Id);
        }

        await store.DeleteGuildAsync(guild);

        return true;
    }

    public bool Donate(Player player, uint amount)
    {
        if (amount == 0 || player.Guild is not { } guild || !guild.CanDeposit(amount) || !player.Spend(amount))
        {
            return false;
        }

        guild.Deposit(amount);
        player.GuildDonation += amount;
        TellGuild(guild, $"{player.Name} has donated {amount} silver to the guild.");
        world.ShowFund(guild);
        return true;
    }

    public bool Leave(Player player)
    {
        if (player.Guild is not { } guild)
        {
            return false;
        }

        if (player.GuildRank == GuildRank.Leader)
        {
            player.Tell("A leader must hand the guild on or disband it.");
            return false;
        }

        if (player.GuildDonation is > 0 and < LeaveDonation)
        {
            player.Tell($"You must donate {LeaveDonation} silver before you leave.");
            return false;
        }

        RemoveMember(player);
        TellGuild(guild, $"{player.Name} has left the guild.");
        SendGuildInfo(player);
        return true;
    }

    public async Task OnDeletingAsync(Player player)
    {
        if (Led(player) is not null)
        {
            await DisbandAsync(player);
        }
        else if (player.Guild is { } guild)
        {
            RemoveMember(player);
            TellGuild(guild, $"{player.Name} has left the guild.");
        }
    }

    public bool Join(Player player, Player officer)
    {
        if (player.GuildId is not null || officer.Guild is not { } guild || officer.GuildRank < GuildRank.DeputyLeader)
        {
            return false;
        }

        AddMember(player, guild, GuildRank.Member, 0);
        TellGuild(guild, $"{player.Name} has joined the guild.");
        SendGuildInfo(player);
        Greet(player);
        return true;
    }

    public bool SetAnnouncement(Player player, string announcement)
    {
        if (announcement.Length > MaxAnnouncementLength || LedMainGuild(player) is not { } guild)
        {
            return false;
        }

        guild.SetAnnouncement(announcement);

        foreach (var member in world.FamilyMembers(guild))
        {
            member.Tell(announcement, TalkChannel.GuildBulletin);
        }

        return true;
    }

    public void Greet(Player player)
    {
        if (player.Guild is { } guild && world.MainGuildOf(guild) is { Announcement.Length: > 0 } master)
        {
            player.Tell(master.Announcement, TalkChannel.GuildBulletin);
        }
    }

    public bool Kick(Player officer, Player member)
    {
        if (officer.Guild is not { } guild || officer.GuildId != member.GuildId || officer.GuildRank <= member.GuildRank)
        {
            return false;
        }

        RemoveMember(member);
        TellGuild(guild, $"{member.Name} has been expelled from the guild.");
        SendGuildInfo(member);
        return true;
    }

    private bool Promote(Player leader, Player member, GuildRank rank)
    {
        if (Led(leader) is null || leader.GuildId != member.GuildId || leader == member || rank is < GuildRank.Member or >= GuildRank.Leader)
        {
            return false;
        }

        member.SetGuild(member.GuildId, rank, member.GuildDonation);
        SendGuildInfo(member);
        return true;
    }

    private bool TransferLeadership(Player leader, Player heir)
    {
        if (Led(leader) is not { } guild || leader.GuildId != heir.GuildId || leader == heir)
        {
            return false;
        }

        guild.LeaderName = heir.Name;
        leader.SetGuild(leader.GuildId, GuildRank.DeputyLeader, leader.GuildDonation);
        heir.SetGuild(heir.GuildId, GuildRank.Leader, heir.GuildDonation);
        SendGuildInfo(leader);
        SendGuildInfo(heir);
        return true;
    }

    public async Task<bool> CreateBranchAsync(Player leader, byte level, uint cost, uint fund, string name)
    {
        if (LedMainGuild(leader) is not { } master || Partner(leader, level) is not { } manager || manager.GuildId != master.Id ||
            IsFounding(leader) || !IsAvailable(leader, name) || !leader.Spend(cost))
        {
            return false;
        }

        var branch = await FoundAsync(leader, cost, name, fund, master.Id);
        manager = Current(manager);

        if (manager.Guild is not null)
        {
            RemoveMember(manager);
        }

        await AddMemberAsync(manager, branch, GuildRank.Leader, fund);
        TellGuild(master, $"{manager.Name} now leads the branch {name}.");
        return true;
    }

    public async Task<bool> ChangeBranchLeaderAsync(Player leader, byte level, string name)
    {
        if (LedMainGuild(leader) is not { } master || world.FindGuild(name) is not { } branch || branch.ParentGuildId != master.Id ||
            Partner(leader, level) is not { } manager || manager.GuildId != master.Id && manager.GuildId != branch.Id ||
            manager.GuildId == branch.Id && manager.GuildRank == GuildRank.Leader)
        {
            return false;
        }

        if (Online(branch.Id).FirstOrDefault(member => member.GuildRank == GuildRank.Leader) is { } deposed)
        {
            deposed.SetGuild(branch.Id, GuildRank.DeputyLeader, deposed.GuildDonation);
            SendGuildInfo(deposed);
        }

        var donation = manager.GuildId == branch.Id ? manager.GuildDonation : 0;
        RemoveMember(manager);
        AddMember(manager, branch, GuildRank.Leader, donation);
        await store.ChangeBranchLeaderAsync(branch.Id, manager.Id);
        TellGuild(master, $"{manager.Name} now leads the branch {name}.");
        SendGuildInfo(manager);
        return true;
    }

    public async Task<bool> AddAllyAsync(Player leader)
    {
        if (leader.Team?.Members.FirstOrDefault(member => member != leader && member.ShownGuildRank == GuildRank.Leader) is not { } partner)
        {
            leader.Tell("Both guild leaders must be in the same team.");
            return false;
        }

        if (LedMainGuild(leader) is not { } mine || LedMainGuild(partner) is not { } theirs || mine == theirs)
        {
            return false;
        }

        if (mine.IsAlliedWith(theirs.Id))
        {
            return true;
        }

        if (mine.Allies.Count >= MaxRelations || theirs.Allies.Count >= MaxRelations)
        {
            leader.Tell("One of the guilds has too many allies already.");
            return false;
        }

        GuildAlly[] pact =
        [
            new() { GuildId = mine.Id, AllyGuildId = theirs.Id },
            new() { GuildId = theirs.Id, AllyGuildId = mine.Id }
        ];
        mine.Allies.Add(pact[0]);
        theirs.Allies.Add(pact[1]);
        SendToFamily(mine, SyndicateAction.SetAlly, theirs.Id);
        SendToFamily(theirs, SyndicateAction.SetAlly, mine.Id);

        await store.AddAsync(pact);
        return true;
    }

    public async Task<bool> RemoveAllyAsync(Player leader, string name)
    {
        if (LedMainGuild(leader) is not { } mine || world.FindGuild(name) is not { } theirs || mine == theirs)
        {
            return false;
        }

        mine.Allies.RemoveAll(ally => ally.AllyGuildId == theirs.Id);
        theirs.Allies.RemoveAll(ally => ally.AllyGuildId == mine.Id);
        SendToFamily(mine, SyndicateAction.ClearAlly, theirs.Id);
        SendToFamily(theirs, SyndicateAction.ClearAlly, mine.Id);

        await store.RemoveAllianceAsync(mine.Id, theirs.Id);
        return true;
    }

    public async Task<bool> SetEnemyAsync(Player leader, string name, bool hostile)
    {
        if (LedMainGuild(leader) is not { } mine || world.FindGuild(name) is not { } theirs || mine == theirs)
        {
            return false;
        }

        if (!hostile)
        {
            mine.Enemies.RemoveAll(enemy => enemy.EnemyGuildId == theirs.Id);
            await store.RemoveEnemyAsync(mine.Id, theirs.Id);
        }
        else if (!mine.IsEnemyOf(theirs.Id))
        {
            if (mine.Enemies.Count >= MaxRelations)
            {
                leader.Tell($"Your guild already has {MaxRelations} enemies.");
                return false;
            }

            var enemy = new GuildEnemy { GuildId = mine.Id, EnemyGuildId = theirs.Id };
            mine.Enemies.Add(enemy);
            await store.AddAsync(enemy);
        }

        SendToFamily(mine, hostile ? SyndicateAction.SetEnemy : SyndicateAction.ClearEnemy, theirs.Id);
        return true;
    }

    public void SendGuildInfo(Player player)
    {
        player.Send(new MsgSyndicateAttributeInfo(player));

        if (player.Guild is not { } guild)
        {
            return;
        }

        var master = world.MainGuildOf(guild);

        foreach (var ally in master.Allies)
        {
            player.Send(new MsgSyndicate { Data = ally.AllyGuildId, Action = SyndicateAction.SetAlly });
        }

        foreach (var enemy in master.Enemies)
        {
            player.Send(new MsgSyndicate { Data = enemy.EnemyGuildId, Action = SyndicateAction.SetEnemy });
        }
    }

    public bool Donate(Player player, string amount) => uint.TryParse(amount, out var silver) && Donate(player, silver);

    public bool TransferLeadership(Player leader, string name) => world.Find(name) is { } heir && TransferLeadership(leader, heir);

    public bool Promote(Player leader, string name, GuildRank rank) => world.Find(name) is { } member && Promote(leader, member, rank);

    public async Task<bool> KickAsync(Player officer, string name)
    {
        if (world.Find(name) is { } member)
        {
            return Kick(officer, member);
        }

        if (officer.Guild is not { } guild || !await store.KickAsync(guild.Id, name, officer.GuildRank))
        {
            return false;
        }

        guild.MemberCount--;
        TellGuild(guild, $"{name} has been expelled from the guild.");
        return true;
    }

    private bool IsAvailable(Player player, string name)
    {
        if (name.Length is < MinNameLength or > MaxNameLength || !Player.HasAllowedCharacters(name) ||
            Reserved.Any(word => name.Contains(word, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        if (world.FindGuild(name) is not null || world.FoundingGuilds.ContainsKey(name))
        {
            player.Tell("A guild by that name already exists.");
            return false;
        }

        return true;
    }

    private bool IsFounding(Player player) => world.FoundingGuilds.ContainsValue(player.Id);

    private async Task<Guild> FoundAsync(Player founder, uint cost, string name, uint fund, uint? parentId = null)
    {
        world.FoundingGuilds[name] = founder.Id;

        try
        {
            var guild = new Guild { Name = name, ParentGuildId = parentId };
            guild.Deposit(fund);
            await store.AddAsync(guild);
            return world.Guilds[guild.Id] = guild;
        }
        catch
        {
            Current(founder).Earn(cost);
            throw;
        }
        finally
        {
            world.FoundingGuilds.Remove(name);
        }
    }

    private Player Current(Player player) => world.Players.GetValueOrDefault(player.Id) ?? player;

    private async Task AddMemberAsync(Player player, Guild guild, GuildRank rank, uint donation)
    {
        AddMember(player, guild, rank, donation);

        if (world.Players.GetValueOrDefault(player.Id) == player)
        {
            SendGuildInfo(player);
        }
        else
        {
            await store.SetGuildAsync(player.Id, guild.Id, rank, donation);
        }
    }

    private static void AddMember(Player player, Guild guild, GuildRank rank, uint donation)
    {
        player.SetGuild(guild.Id, rank, donation);
        guild.MemberCount++;

        if (rank == GuildRank.Leader)
        {
            guild.LeaderName = player.Name;
        }
    }

    private static void RemoveMember(Player player)
    {
        player.Guild!.MemberCount--;
        player.SetGuild(null, GuildRank.None, 0);
    }

    private static Player? Partner(Player leader, byte level) =>
        leader.Team?.Members.FirstOrDefault(member => member != leader && member.Level >= level);

    private static Guild? LedMainGuild(Player player) => Led(player) is { ParentGuildId: null } master ? master : null;

    private static Guild? Led(Player player) => player.GuildRank == GuildRank.Leader ? player.Guild : null;

    private IEnumerable<Player> Online(uint guildId) => world.Players.Values.Where(player => player.GuildId == guildId);

    private void TellGuild(Guild guild, string text)
    {
        foreach (var member in world.FamilyMembers(guild))
        {
            member.Tell(text, TalkChannel.Guild);
        }
    }

    private void SendToFamily(Guild guild, SyndicateAction action, uint data) =>
        world.FamilyMembers(guild).Send(new MsgSyndicate { Action = action, Data = data });
}
