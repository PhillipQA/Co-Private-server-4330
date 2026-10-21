using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Services;

public partial class GameStore
{
    public Task<Player?> LoadAsync(string userId) => UseAsync(async db =>
    {
        var player = await db.Players
            .Include(p => p.Items.Where(item =>
                item.StorageId == null || !db.DynaNpcs.Any(npc => npc.Id == item.StorageId && npc.Type == DynaNpc.TrunkType)))
            .Include(p => p.Magics)
            .Include(p => p.WeaponSkills)
            .Include(p => p.Quests)
            .Include(p => p.Friends)
            .Include(p => p.Enemies)
            .Include(p => p.Prizes)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (player is not null)
        {
            Remember(OwnerOf(player), player.Snapshot().Rows());
        }

        return player;
    });

    public Task<bool> HasCharacterAsync(string userId) => UseAsync(db => db.Players.AnyAsync(row => row.UserId == userId));

    public Task<bool> IsNameTakenAsync(string name) => UseAsync(db => db.Players.AnyAsync(row => row.Name == name));

    public bool LogOut(Player player)
    {
        if (world.Players.GetValueOrDefault(player.Id) != player)
        {
            return false;
        }

        try
        {
            world.Leave(player);
        }
        finally
        {
            _retired[player.UserId] = player.Snapshot();
        }

        return true;
    }

    public async Task<bool> FlushAsync(string userId)
    {
        await _saving.WaitAsync();

        try
        {
            if (_retired.ContainsKey(userId))
            {
                await PersistAsync();
            }

            return !_retired.ContainsKey(userId);
        }
        finally
        {
            _saving.Release();
        }
    }

    public Task DeleteAsync(Player player) => UseAsync(async db =>
    {
        var id = player.Id;

        await db.Friends.Where(row => row.FriendPlayerId == id).ExecuteDeleteAsync();
        await db.Enemies.Where(row => row.EnemyPlayerId == id).ExecuteDeleteAsync();

        if (player.IsMarried)
        {
            await db.Players.Where(row => row.Name == player.Spouse).ExecuteUpdateAsync(set => set.SetProperty(row => row.Spouse, Player.Unmarried));
        }

        await db.Players.Where(row => row.Id == id).ExecuteDeleteAsync();
        Forget(OwnerOf(player));
    });

    public Task RemoveFriendAsync(uint playerId, uint friendId) =>
        UseAsync(db => db.Friends.Where(row => row.PlayerId == playerId && row.FriendPlayerId == friendId).ExecuteDeleteAsync());

    public Task SaveOfflineMessageAsync(OfflineMessage word) => AddAsync(word);

    public Task<OfflineMessage?> TakeOfflineMessageAsync(uint playerId) => UseAsync(async db =>
    {
        var word = await db.OfflineMessages.Where(row => row.PlayerId == playerId).OrderBy(row => row.Id).FirstOrDefaultAsync();

        if (word is not null)
        {
            await db.OfflineMessages.Where(row => row.Id == word.Id).ExecuteDeleteAsync();
        }

        return word;
    });

    public Task DivorceAsync(string spouse, Item tear) => UseAsync(async db =>
    {
        var id = await db.Players.Where(row => row.Name == spouse).Select(row => row.Id).SingleAsync();
        await db.Players.Where(row => row.Id == id).ExecuteUpdateAsync(set => set.SetProperty(row => row.Spouse, Player.Unmarried));
        db.Items.Add(tear with { PlayerId = id });
        await db.SaveChangesAsync();
    });

    public Task<bool> BanAsync(string name) => UseAsync(async db =>
        await db.Players.Where(row => row.Name == name).Select(row => row.UserId).FirstOrDefaultAsync() is { } userId &&
        await db.Database.ExecuteSqlAsync($"UPDATE AspNetUsers SET IsBanned = 1 WHERE Id = {userId}") > 0);

    public Task<bool> JailAsync(string name, uint mapId, ushort x, ushort y) => UseAsync(async db =>
        await db.Players.Where(row => row.Name == name).ExecuteUpdateAsync(set => set
            .SetProperty(row => row.MapId, mapId).SetProperty(row => row.X, x).SetProperty(row => row.Y, y)
            .SetProperty(row => row.RecordMapId, mapId).SetProperty(row => row.RecordX, x).SetProperty(row => row.RecordY, y)) > 0);

    public async Task<uint> HomeOfAsync(string name) =>
        world.Find(name)?.HomeId ?? await UseAsync(db => db.Players.Where(row => row.Name == name).Select(row => row.HomeId).FirstOrDefaultAsync());
}
