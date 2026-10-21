namespace Conquer.Game.Models;

public partial class Player
{
    public const string Unmarried = "None";

    public uint ProposedTo { get; internal set; }

    public bool Propose(Player other)
    {
        if (IsMarried || other.IsMarried || other.Map != Map || !CanSee(other) || other.IsFemale == IsFemale)
        {
            return false;
        }

        ProposedTo = other.Id;
        other.Send(new MsgInteract { Sender = Id, Target = other.Id, Action = InteractAction.Propose });
        return true;
    }

    public bool Marry(Player suitor)
    {
        if (IsMarried || suitor.IsMarried || suitor.Map != Map || !CanSee(suitor) || suitor.ProposedTo != Id)
        {
            return false;
        }

        suitor.ProposedTo = 0;
        SetSpouse(suitor.Name);
        suitor.SetSpouse(Name);
        World.Announce($"{Name} and {suitor.Name} are pronounced the husband and wife. They promise to love and care for each other for better or worse.",
            TalkChannel.Gm);
        Fireworks();
        return true;
    }

    public (string Spouse, Item? Tear)? Divorce()
    {
        if (!IsMarried)
        {
            return null;
        }

        var spouse = Spouse;
        Map?.Broadcast(this, MsgTalk.System($"{Name} and {spouse} got divorced because the love is not there anymore!"));
        SetSpouse(Unmarried);

        if (World.Find(spouse) is { } partner)
        {
            partner.SetSpouse(Unmarried);

            if (partner.Give(MeteorTear) is not null)
            {
                return (spouse, null);
            }
        }
        else
        {
            World.Divorcing.Add(spouse);
        }

        return (spouse, Item.Of(World.Data.ItemTypes[MeteorTear]) with { Id = World.ItemIds.Next() });
    }

    public void SetSpouse(string spouse)
    {
        Spouse = spouse;
        Send(new MsgName { Id = Id, Action = NameAction.Spouse, Strings = [spouse] });
    }
}
