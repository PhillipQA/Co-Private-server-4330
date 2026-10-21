namespace Conquer.Game.Models;

public partial class Player
{
    private const uint MaxBounty = 9_999_999;
    private const uint MinBounty = 100_000;
    private const uint MinRaise = 10_000;
    private const uint HuntFee = 1_000;
    private const uint HunterShare = 70;

    public uint Hunting { get; private set; }
    public uint Pursuing { get; private set; }
    public bool IsNotorious => PkPoints >= BlackNamePoints && RecordMapId != JailMap;

    public string ListBounties(ref uint cursor, int count)
    {
        var lines = "";

        for (var i = 0; i < count && World.Bounties.At(++cursor) is not null; i++)
        {
            lines += World.Bounties.Describe(cursor);
        }

        return lines;
    }

    public string ListFugitives(ref uint cursor, int count)
    {
        var lines = "";

        for (var i = 0; i < count; i++)
        {
            lines += World.Fugitives.ElementAtOrDefault((int)cursor) is { } fugitive
                ? $"{cursor:D3} {fugitive.Name,-15} {fugitive.Guild?.Name ?? "NULL",-15} {fugitive.Level:D3} {fugitive.PkPoints}"
                : "--";

            if (++cursor >= World.Fugitives.Count)
            {
                break;
            }
        }

        return lines;
    }

    public bool PostBounty(string target, string reward)
    {
        var amount = Number(reward);

        if (amount > MaxBounty)
        {
            Tell($"You should offer a reward of {MaxBounty} silvers at most!");
            return false;
        }

        if (reward.Length == 0)
        {
            return false;
        }

        if (amount < MinBounty)
        {
            Tell($"You should offer a reward of {MinBounty} silvers at least!");
            return false;
        }

        if (!Afford(amount))
        {
            return false;
        }

        World.Bounties.Post(target, Name, amount);
        return true;
    }

    public Bounty? FindBounty(string id)
    {
        if (World.Bounties.Find(Number(id)) is { } bounty)
        {
            return bounty;
        }

        Tell("Invalid Pursuing Number!");
        return null;
    }

    public bool HuntBounty(string id)
    {
        if (FindBounty(id) is not { } bounty || !Afford(HuntFee))
        {
            return false;
        }

        Hunting = bounty.Id;
        Send(new MsgName { Action = NameAction.Wanted, Strings = [bounty.Target] });
        Tell($"{HuntFee} silvers earnest money is deducted.  You have got No.{bounty.Id} Pursuing Order. " +
             $"Target: {bounty.Target}; Reward: {bounty.Reward} silvers.");

        if (World.Find(bounty.Target)?.Map is { } haunt)
        {
            Tell($"It is said that the wanted target appears around [{haunt.Info.Name}].");
        }

        return true;
    }

    public bool CancelBounty(string id)
    {
        if (FindBounty(id) is not { } bounty || !Afford(bounty.Reward * 2))
        {
            return false;
        }

        World.Bounties.Remove(bounty);
        Tell($"Pursuing Order for {bounty.Id} has been canceled.");
        return true;
    }

    public bool RaiseBounty(string id, string amount)
    {
        if (FindBounty(id) is not { } bounty)
        {
            return false;
        }

        var raise = Number(amount);

        if (raise < MinRaise)
        {
            Tell($"You must increase {MinRaise} silvers at least!");
            return false;
        }

        if (bounty.Reward + raise > MaxBounty)
        {
            Tell($"You should offer a reward of {MaxBounty} silvers at most!");
            return false;
        }

        if (!Afford(raise))
        {
            return false;
        }

        World.Bounties.Raise(bounty, raise);
        Tell($"The reward for No.{bounty.Id} Pursuing Order is increased to {bounty.Reward} silvers!");
        return true;
    }

    public bool PursueFugitive(string position)
    {
        if (World.Fugitives.ElementAtOrDefault((int)Number(position)) is not { } fugitive)
        {
            Tell("Invalid Pursuing Number!");
            return false;
        }

        Pursuing = fugitive.Id;
        Send(new MsgName { Action = NameAction.PoliceWanted, Strings = [fugitive.Name] });
        Tell($"Confirm: Target {fugitive.Name}, this person was saw at {fugitive.Map?.Info.Name} recently.");
        return true;
    }

    public void SeekBounty()
    {
        if (World.Bounties.Find(Hunting) is { } bounty && World.Find(bounty.Target) is { } target && Sees(target))
        {
            Tell($"The wanted target [{target.Name}] is found! ");
        }
    }

    public void SeekFugitive()
    {
        if (World.Fugitives.Find(fugitive => fugitive.Id == Pursuing) is { } fugitive && Sees(fugitive))
        {
            Tell($"The governmental wanted target [{fugitive.Name}] is found! ");
        }
    }

    private bool Sees(Player other) => other.Map == Map && CanSee(other.X, other.Y);

    private void Collect(Player prey)
    {
        if (World.Bounties.Find(Hunting) is { } bounty && bounty.Target == prey.Name)
        {
            Earn(bounty.Reward * HunterShare / 100);
            World.Bounties.Remove(bounty);
            Hunting = 0;
            Send(new MsgName { Action = NameAction.Wanted, Strings = [""] });

            prey.LoseExperience(Math.Min(Math.Min(bounty.Reward, World.Data.LevelExps.GetValueOrDefault(Level)?.Experience ?? 0), prey.Experience));
        }

        if (Pursuing == prey.Id)
        {
            Arrest(prey);
        }
        else if (Team?.Leader is { } leader && leader != this && leader.Pursuing == prey.Id)
        {
            leader.Arrest(prey);
            World.Announce(MapId, $"Notorious {prey.Name} was finally hunted down by the detective team headed by {leader.Name}!",
                TalkChannel.System);
        }
    }

    private void Arrest(Player fugitive)
    {
        Pursuing = 0;
        SetPkPoints(PkPoints - Math.Max(1, (fugitive.Level - 40) / 10));
    }

    private bool Afford(uint amount)
    {
        if (Spend(amount))
        {
            return true;
        }

        Tell($"You don't have {amount} silvers!");
        return false;
    }

    private static uint Number(string text) => uint.TryParse(text, out var number) ? number : 0;
}
