namespace Conquer.Game.Models;

public sealed class KoBoard
{
    public const int MinKills = 25;
    public const int Size = 100;

    private const int Leaders = 3;
    private const int Contenders = 50;

    private readonly List<(uint Id, string Name, int Kills)> _entries = [];

    public void Load(IEnumerable<(uint Id, string Name, int Kills)> entries) => _entries.AddRange(entries);

    public uint RankOf(uint id) => Rank(_entries.FindIndex(entry => entry.Id == id));

    public KoPlacing? Enter(uint id, string name, int kills)
    {
        if (_entries.Count >= Size && kills <= _entries[^1].Kills)
        {
            return null;
        }

        var before = Ranks();
        _entries.RemoveAll(entry => entry.Id == id);
        var place = _entries.FindIndex(entry => entry.Kills < kills) is >= 0 and var index ? index : _entries.Count;
        _entries.Insert(place, (id, name, kills));

        if (_entries.Count > Size)
        {
            _entries.RemoveAt(Size);
        }

        var after = Ranks();
        var changes = before.Keys.Union(after.Keys).Where(key => before.GetValueOrDefault(key) != after.GetValueOrDefault(key))
            .Select(key => (key, after.GetValueOrDefault(key))).ToList();

        return new(place + 1, after[id] > before.GetValueOrDefault(id), changes);
    }

    private Dictionary<uint, uint> Ranks() => _entries.Select((entry, index) => (entry.Id, Rank(index))).ToDictionary();

    private static uint Rank(int index) => index switch
    {
        < 0 => 0,
        < Leaders => 3,
        < Contenders => 2,
        < Size => 1,
        _ => 0
    };
}

public readonly record struct KoPlacing(int Place, bool Promoted, List<(uint Id, uint Rank)> Changes);
