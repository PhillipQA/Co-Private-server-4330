using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Services;

public partial class GameStore
{
    public Task<List<(uint Id, string Name, byte Level, GuildRank Rank)>> GuildMembersAsync(List<uint> guildIds) => UseAsync(async db =>
        (await db.Players.Where(row => row.GuildId != null && EF.Parameter(guildIds).Contains(row.GuildId.Value))
            .Select(row => new { row.Id, row.Name, row.Level, row.GuildRank }).ToListAsync())
        .Select(row => (row.Id, row.Name, row.Level, row.GuildRank)).ToList());

    public Task DisbandAsync(uint guildId) => UseAsync(db => UnguildAsync(db.Players.Where(row => row.GuildId == guildId)));

    public Task MergeAsync(uint branchId, uint masterId) =>
        UseAsync(db => db.Players.Where(row => row.GuildId == branchId).ExecuteUpdateAsync(set => set
            .SetProperty(row => row.GuildId, masterId)
            .SetProperty(row => row.GuildRank, GuildRank.Member)));

    public Task DeleteGuildAsync(Guild guild) => UseAsync(async db =>
    {
        await db.GuildAllies.Where(ally => ally.GuildId == guild.Id || ally.AllyGuildId == guild.Id).ExecuteDeleteAsync();
        await db.GuildEnemies.Where(enemy => enemy.GuildId == guild.Id || enemy.EnemyGuildId == guild.Id).ExecuteDeleteAsync();
        await db.Guilds.Where(row => row.ParentGuildId == guild.Id).ExecuteUpdateAsync(set => set.SetProperty(row => row.ParentGuildId, (uint?)null));
        await db.Guilds.Where(row => row.Id == guild.Id).ExecuteDeleteAsync();
        Forget(OwnerOf(guild));
    });

    public Task RemoveAllianceAsync(uint guildId, uint allyId) =>
        UseAsync(db => db.GuildAllies.Where(ally => ally.GuildId == guildId && ally.AllyGuildId == allyId ||
                                                    ally.GuildId == allyId && ally.AllyGuildId == guildId).ExecuteDeleteAsync());

    public Task RemoveEnemyAsync(uint guildId, uint enemyId) =>
        UseAsync(db => db.GuildEnemies.Where(enemy => enemy.GuildId == guildId && enemy.EnemyGuildId == enemyId).ExecuteDeleteAsync());

    public Task ChangeBranchLeaderAsync(uint branchId, uint heirId) =>
        UseAsync(db => db.Players.Where(row => row.GuildId == branchId && row.GuildRank == GuildRank.Leader && row.Id != heirId)
            .ExecuteUpdateAsync(set => set.SetProperty(row => row.GuildRank, GuildRank.DeputyLeader)));

    public Task SetGuildAsync(uint playerId, uint guildId, GuildRank rank, uint donation) =>
        UseAsync(db => db.Players.Where(row => row.Id == playerId).ExecuteUpdateAsync(set => set
            .SetProperty(row => row.GuildId, guildId)
            .SetProperty(row => row.GuildRank, rank)
            .SetProperty(row => row.GuildDonation, donation)));

    public Task<bool> KickAsync(uint guildId, string name, GuildRank below) =>
        UseAsync(async db => await UnguildAsync(db.Players.Where(row => row.Name == name && row.GuildId == guildId && row.GuildRank < below)) > 0);

    private static Task<int> UnguildAsync(IQueryable<Player> rows) => rows.ExecuteUpdateAsync(set => set
        .SetProperty(row => row.GuildId, (uint?)null)
        .SetProperty(row => row.GuildRank, GuildRank.None)
        .SetProperty(row => row.GuildDonation, 0u));
}
