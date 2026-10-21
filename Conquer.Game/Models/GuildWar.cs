namespace Conquer.Game.Models;

public readonly record struct GuildWarWindow(TimeSpan Start, TimeSpan End)
{
    public static GuildWarWindow Of(DynaNpc pole) => new(Moment(pole.Data1), Moment(pole.Data2));

    public bool Contains(DateTime local)
    {
        var intoWeek = local - local.Date.AddDays(-(int)local.DayOfWeek);
        return Start <= End ? intoWeek >= Start && intoWeek < End : intoWeek >= Start || intoWeek < End;
    }

    private static TimeSpan Moment(int value) => new(value / 1_000_000 % 7, value / 10_000 % 100, value / 100 % 100, 0);
}

public sealed class GuildWar(Map map, DynaNpc pole)
{
    private const int DonationPercent = 1;
    private const int SilverPercent = 2;
    private const uint MaxRepair = 1000;
    private const int Standings = 5;

    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan RepairInterval = TimeSpan.FromSeconds(10);

    private readonly Dictionary<uint, long> _scores = [];
    private DateTimeOffset _nextCheck;
    private DateTimeOffset _nextRepair;

    public DynaNpc Pole => pole;
    public bool IsOn { get; private set; }
    public Guild? Owner => map.World.Guilds.GetValueOrDefault(pole.OwnerId);

    public bool Defends(Player player) =>
        player.Guild is { } guild && Owner is { } owner && map.World.AreFamily(guild, owner);

    public void Tick(DateTimeOffset now)
    {
        if (now >= _nextRepair)
        {
            _nextRepair = now + RepairInterval;
            Repair();
        }

        if (now < _nextCheck)
        {
            return;
        }

        _nextCheck = now + CheckInterval;

        if (IsOn)
        {
            ShowStandings();
        }

        var due = GuildWarWindow.Of(pole).Contains(map.World.Now);

        if (due && !IsOn)
        {
            IsOn = true;
            map.World.Announce(map.Id, "Guild battle started!", TalkChannel.Talk);
        }
        else if (!due && IsOn)
        {
            IsOn = false;
            map.World.Announce(map.Id, "Guild battle ended!", TalkChannel.Talk);
            StopFighting();
        }
    }

    public void Struck(Player attacker, int lifeLost)
    {
        if (attacker.Guild is not { } guild)
        {
            return;
        }

        var master = map.World.MainGuildOf(guild);
        _scores[master.Id] = _scores.GetValueOrDefault(master.Id) + lifeLost;
        attacker.GuildDonation += (uint)(lifeLost * DonationPercent / 100);
        attacker.Send(new MsgUserAttrib(attacker.Id, AttributeType.Donation, attacker.GuildDonation));

        if (Owner?.Withdraw((uint)(lifeLost * SilverPercent / 100)) is uint silver and > 0)
        {
            attacker.Earn(silver);
            map.World.ShowFund(Owner);
        }
    }

    public void ResetOwner()
    {
        var winner = _scores.Where(score => map.World.Guilds.ContainsKey(score.Key)).OrderByDescending(score => score.Value)
            .Select(score => score.Key).FirstOrDefault();

        if (winner != 0)
        {
            foreach (var npc in map.World.Maps.Values.SelectMany(each => each.DynaNpcs).Where(npc => npc.OwnerType == DynaNpc.GuildOwned))
            {
                npc.OwnerId = winner;
            }

            map.Broadcast(pole, pole.Spawn);
        }

        _scores.Clear();
        Stop();
        StopFighting();

        if (Owner is { } owner)
        {
            map.World.Announce($"{owner.Name} has won!", TalkChannel.Gm);
        }
    }

    public void Stop() => IsOn = false;

    private void ShowStandings()
    {
        var leaders = _scores.Where(score => score.Value > 0 && map.World.Guilds.ContainsKey(score.Key))
            .OrderByDescending(score => score.Value).Take(Standings);

        foreach (var (place, (guildId, score)) in leaders.Index())
        {
            var channel = place == 0 ? TalkChannel.GuildWarFirst : TalkChannel.GuildWarNext;
            map.World.Announce(map.Id, $"No. {place + 1}: {map.World.Guilds[guildId].Name}({score})", channel);
        }
    }

    private void Repair()
    {
        var lost = (uint)(pole.Stats.MaxHealth - pole.Health);

        if (lost == 0 || pole.Data0 <= 0 || Owner is not { } owner)
        {
            return;
        }

        var repair = Math.Min(Math.Min((uint)pole.Data0, MaxRepair), lost);
        pole.Data0 -= (int)repair;
        var paid = owner.Withdraw(repair);

        if (paid == 0)
        {
            pole.Data0 = 0;
            return;
        }

        pole.AddHealth((int)paid);
        map.World.ShowFund(owner);
    }

    private void StopFighting()
    {
        foreach (var player in map.Players)
        {
            player.Target = null;
        }
    }
}
