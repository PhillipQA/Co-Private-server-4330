using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Services;

public partial class GameStore
{
    private const uint WeatherRegion = 2;

    public async Task LoadWorldAsync()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.OfflineMessages.Where(word => word.SentAt < world.Now - OfflineMessage.Lifetime).ExecuteDeleteAsync();
        world.ItemIds.Resume((await db.Items.MaxAsync(item => (uint?)item.Id) ?? 0) + 1);

        foreach (var info in await db.Maps.ToListAsync())
        {
            world.Open(info);
        }

        var lastNpcId = await db.Npcs.Select(npc => npc.Id).Concat(db.DynaNpcs.Select(npc => npc.Id)).Where(id => id <= World.LastNpcId)
            .MaxAsync(id => (uint?)id) ?? 0;
        world.NpcIds.Resume(lastNpcId + 1);

        var lastStatueId = await db.DynaNpcs.Select(npc => npc.Id).Where(id => id >= World.FirstStatueId && id <= World.LastStatueId)
            .MaxAsync(id => (uint?)id) ?? World.FirstStatueId - 1;
        world.StatueIds.Resume(lastStatueId + 1);
        world.MapIds.Resume(world.Maps.Keys.Max() + 1);

        var portals = await db.Portals.ToListAsync();

        foreach (var passway in await db.Passways.ToListAsync())
        {
            if (world.Maps.GetValueOrDefault(passway.MapId) is not { } map ||
                portals.Find(p => p.MapId == passway.PortalMapId && p.Idx == passway.PortalIdx) is not { } exit)
            {
                continue;
            }

            foreach (var entrance in map.DMap.Portals.Where(p => p.Idx == passway.PasswayIdx))
            {
                map.Portals[(entrance.PortalX, entrance.PortalY)] = exit;
            }
        }

        var rosters = await db.Players.Where(player => player.GuildId != null).GroupBy(player => player.GuildId!.Value)
            .Select(members => new
            {
                GuildId = members.Key,
                Count = members.Count(),
                Leader = members.Where(member => member.GuildRank == GuildRank.Leader).Select(member => member.Name).FirstOrDefault()
            })
            .ToDictionaryAsync(roster => roster.GuildId);

        foreach (var guild in await db.Guilds.Include(guild => guild.Allies).Include(guild => guild.Enemies).AsSplitQuery().AsNoTracking().ToListAsync())
        {
            var roster = rosters.GetValueOrDefault(guild.Id);
            guild.MemberCount = roster?.Count ?? 0;
            guild.LeaderName = roster?.Leader ?? "";
            world.Guilds[guild.Id] = guild;
        }

        world.Bounties.Load(await db.Bounties.AsNoTracking().ToListAsync());
        world.KoBoard.Load((await db.Players.Where(player => player.KoRecord >= KoBoard.MinKills)
            .OrderByDescending(player => player.KoRecord).Take(KoBoard.Size)
            .Select(player => new { player.Id, player.Name, player.KoRecord }).ToListAsync())
            .Select(player => (player.Id, player.Name, player.KoRecord)));

        var dynaNpcs = await db.DynaNpcs.AsNoTracking().ToListAsync();
        dynaNpcs.ForEach(npc => npc.Initialize());

        var boxes = dynaNpcs.Where(npc => npc.IsTrunk).ToDictionary(npc => npc.Id);
        var boxIds = boxes.Keys.ToList();

        foreach (var item in await db.Items.Where(item => item.StorageId != null && EF.Parameter(boxIds).Contains(item.StorageId.Value)).AsNoTracking()
                     .ToListAsync())
        {
            boxes[item.StorageId!.Value].Contents.Add(item);
        }

        var traps = Resolved(await db.Traps.AsNoTracking().ToListAsync(), trap => world.Data.TrapTypes.ContainsKey(trap.Type), trap => trap.Id, "trap");

        foreach (var fixture in (await db.Npcs.AsNoTracking().ToListAsync()).Concat<Entity>(dynaNpcs).Concat(traps))
        {
            world.Maps.GetValueOrDefault(fixture.MapId)?.Add(fixture, fixture.X, fixture.Y);
        }

        foreach (var npc in dynaNpcs.Where(npc => npc.Map is not null))
        {
            if (npc.IsScene)
            {
                npc.Map!.Build(npc);
            }

            if (npc.IsPole)
            {
                npc.Map!.War = new(npc.Map, npc);
            }
        }

        foreach (var region in await db.Regions.AsNoTracking().ToListAsync())
        {
            if (world.Maps.GetValueOrDefault(region.MapId) is not { } map)
            {
                continue;
            }

            map.Regions.Add(region);

            if (region.Type == WeatherRegion)
            {
                map.Weather = new(region, world.Random, world.UtcNow);
            }
        }

        var generators = Resolved(await db.Generators.AsNoTracking().ToListAsync(),
            generator => world.Data.MonsterTypes.ContainsKey(generator.MonsterTypeId), generator => generator.Id, "generator");

        foreach (var generator in generators)
        {
            world.Maps.GetValueOrDefault(generator.MapId)?.Generators.Add(generator);
        }

        foreach (var (owner, rows) in Capture())
        {
            Remember(owner, rows);
        }
    }

    private List<T> Resolved<T>(List<T> rows, Func<T, bool> resolves, Func<T, uint> id, string kind)
    {
        foreach (var row in rows.Where(row => !resolves(row)))
        {
            logger.LogWarning("Skipping {Kind} {Id}: its type is not in the game data.", kind, id(row));
        }

        return [.. rows.Where(resolves)];
    }
}
