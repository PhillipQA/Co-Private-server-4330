namespace Conquer.Game.Models;

public sealed class BountyBoard
{
    private readonly List<Bounty> _bounties = [];
    private uint _lastId;

    public void Load(IEnumerable<Bounty> bounties)
    {
        foreach (var bounty in bounties)
        {
            Insert(bounty);
            _lastId = Math.Max(_lastId, bounty.Id);
        }
    }

    public List<Bounty> Snapshot() => [.._bounties.Select(bounty => bounty with { })];

    public Bounty? Find(uint id) => _bounties.Find(bounty => bounty.Id == id);

    public Bounty? At(uint position) => position >= 1 && position <= _bounties.Count ? _bounties[(int)position - 1] : null;

    public string Describe(uint position) => At(position) is { } bounty ? $"{bounty.Id:D6} {bounty.Target,-15} {bounty.Reward}" : "--";

    public void Post(string target, string payer, uint reward) =>
        Insert(new() { Id = ++_lastId, Target = target, Payer = payer, Reward = reward });

    public void Raise(Bounty bounty, uint amount) => bounty.Reward += amount;

    public void Remove(Bounty bounty) => _bounties.Remove(bounty);

    public void RemoveAll(string target) => _bounties.RemoveAll(bounty => bounty.Target == target);

    private void Insert(Bounty bounty)
    {
        var index = _bounties.FindIndex(existing => bounty.Reward >= existing.Reward);
        _bounties.Insert(index < 0 ? _bounties.Count : index, bounty);
    }
}
