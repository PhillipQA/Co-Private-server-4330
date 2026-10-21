using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Services;

public partial class GameStore
{
    private readonly ConcurrentDictionary<string, Player> _retired = [];
    private readonly Dictionary<(Type, object), (Owner Owner, object Row)> _saved = [];
    private readonly HashSet<Owner> _failing = [];

    public async Task SaveAsync()
    {
        await _saving.WaitAsync();

        try
        {
            await PersistAsync();
        }
        finally
        {
            _saving.Release();
        }
    }

    private async Task PersistAsync()
    {
        var captured = await world.Run(Capture);
        var retired = _retired.ToList();

        foreach (var (_, player) in retired)
        {
            captured[OwnerOf(player)] = [..player.Rows()];
        }

        var changes = Changes(captured);

        try
        {
            await WriteAsync(changes);
            _failing.Clear();
        }
        catch (Exception ex) when (IsOutage(ex))
        {
            Unreachable(ex);
            return;
        }
        catch (Exception)
        {
            if (!await WriteEachAsync(changes))
            {
                return;
            }
        }

        foreach (var entry in retired.Where(entry => !_failing.Contains(OwnerOf(entry.Value))))
        {
            _retired.TryRemove(entry);
            Forget(OwnerOf(entry.Value));
        }
    }

    private Dictionary<Owner, List<object>> Capture()
    {
        var captured = new Dictionary<Owner, List<object>>();

        foreach (var player in world.Players.Values)
        {
            captured[OwnerOf(player)] = [..player.Snapshot().Rows()];
        }

        foreach (var map in world.Maps.Values)
        {
            captured[new(typeof(Map), map.Id)] = [map.Info.Snapshot(), ..map.DynaNpcs.SelectMany(npc => npc.Snapshot().Rows())];
        }

        foreach (var guild in world.Guilds.Values)
        {
            captured[OwnerOf(guild)] = [guild.Snapshot()];
        }

        captured[new(typeof(BountyBoard), 0)] = [..world.Bounties.Snapshot()];
        return captured;
    }

    private List<Change> Changes(Dictionary<Owner, List<object>> captured)
    {
        var current = captured
            .SelectMany(entry => entry.Value, (entry, row) => (Owner: entry.Key, Row: row))
            .ToDictionary(entry => Key(entry.Row));

        var changes = new List<Change>();

        foreach (var (key, (owner, row)) in current)
        {
            if (!_saved.TryGetValue(key, out var saved) || saved.Owner != owner || !saved.Row.Equals(row))
            {
                changes.Add(new(owner, key, _saved.GetValueOrDefault(key).Row, row));
            }
        }

        foreach (var (key, (owner, row)) in _saved)
        {
            if (captured.ContainsKey(owner) && !current.ContainsKey(key))
            {
                changes.Add(new(owner, key, row, null));
            }
        }

        return changes;
    }

    private async Task WriteAsync(List<Change> changes)
    {
        if (changes.Count > 0)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync();
            db.ChangeTracker.AutoDetectChangesEnabled = false;
            changes.ForEach(change => Apply(db, change));
            await db.SaveChangesAsync();
            Reached();
        }

        foreach (var change in changes)
        {
            if (change.Current is null)
            {
                _saved.Remove(change.Key);
            }
            else
            {
                _saved[change.Key] = (change.Owner, change.Current);
            }
        }
    }

    private async Task<bool> WriteEachAsync(List<Change> changes)
    {
        foreach (var group in changes.GroupBy(change => change.Owner))
        {
            try
            {
                await WriteAsync([..group]);
                _failing.Remove(group.Key);
            }
            catch (Exception ex) when (IsOutage(ex))
            {
                Unreachable(ex);
                return false;
            }
            catch (Exception ex)
            {
                if (_failing.Add(group.Key))
                {
                    logger.LogError(ex, "Failed to save {Owner}.", group.Key);
                }
            }
        }

        return true;
    }

    private static void Apply(GameDbContext db, Change change)
    {
        switch (change)
        {
            case { Saved: null }:
                db.Add(change.Current!);
                break;

            case { Current: null }:
                db.Remove(change.Saved);
                break;

            case { Current: DynaNpc npc, Saved: DynaNpc saved } when npc.Statue != saved.Statue:
                db.Update(npc);
                break;

            default:
                var entry = db.Entry(change.Current);
                entry.State = EntityState.Unchanged;
                entry.OriginalValues.SetValues(change.Saved);

                foreach (var property in entry.Properties.Where(property => !property.Metadata.IsPrimaryKey()))
                {
                    property.IsModified = !Equals(property.OriginalValue, property.CurrentValue);
                }

                break;
        }
    }

    private void Remember(Owner owner, IEnumerable<object> rows)
    {
        foreach (var row in rows)
        {
            _saved[Key(row)] = (owner, row);
        }
    }

    private void Forget(Owner owner)
    {
        foreach (var key in _saved.Where(entry => entry.Value.Owner == owner).Select(entry => entry.Key).ToList())
        {
            _saved.Remove(key);
        }
    }

    private static Owner OwnerOf(Player player) => new(typeof(Player), player.Id);

    private static Owner OwnerOf(Guild guild) => new(typeof(Guild), guild.Id);

    private static (Type, object) Key(object row) => (row.GetType(), row switch
    {
        Player player => player.Id,
        Item item => item.Id,
        Magic magic => (magic.PlayerId, magic.Type),
        WeaponSkill skill => (skill.PlayerId, skill.Type),
        Quest quest => (quest.PlayerId, quest.QuestId, quest.Slot),
        Friend friend => (friend.PlayerId, friend.FriendPlayerId),
        Enemy enemy => (enemy.PlayerId, enemy.EnemyPlayerId),
        Prize prize => prize.Id,
        MapInfo map => map.Id,
        DynaNpc npc => npc.Id,
        Guild guild => guild.Id,
        Bounty bounty => bounty.Id,
        _ => throw new ArgumentException($"{row.GetType().Name} is not saved by the game store.", nameof(row))
    });

    private readonly record struct Owner(Type Type, uint Id)
    {
        public override string ToString() => $"{Type.Name} {Id}";
    }

    private sealed record Change(Owner Owner, (Type, object) Key, object? Saved, object? Current);
}
