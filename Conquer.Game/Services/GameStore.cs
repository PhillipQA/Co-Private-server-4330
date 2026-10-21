using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Services;

public partial class GameStore(IDbContextFactory<GameDbContext> dbContextFactory, World world, ILogger<GameStore> logger)
{
    private static readonly TimeSpan OutageReminder = TimeSpan.FromMinutes(1);

    private readonly SemaphoreSlim _saving = new(1);
    private (DateTimeOffset Since, DateTimeOffset Reminded)? _outage;

    private async Task<T> UseAsync<T>(Func<GameDbContext, Task<T>> work)
    {
        await _saving.WaitAsync();

        try
        {
            if (!_retired.IsEmpty)
            {
                await PersistAsync();
            }

            await using var db = await dbContextFactory.CreateDbContextAsync();
            return await work(db);
        }
        finally
        {
            _saving.Release();
        }
    }

    private Task UseAsync(Func<GameDbContext, Task> work) => UseAsync(async db =>
    {
        await work(db);
        return true;
    });

    public Task AddAsync(params IEnumerable<object> rows) => UseAsync(async db =>
    {
        var added = rows.ToList();
        db.AddRange(added);
        await db.SaveChangesAsync();

        foreach (var guild in added.OfType<Guild>())
        {
            Remember(OwnerOf(guild), [guild.Snapshot()]);
        }
    });

    private static bool IsOutage(Exception ex) =>
        ex is DbException or TimeoutException or InvalidOperationException { InnerException: DbException or TimeoutException };

    private void Unreachable(Exception ex)
    {
        var now = world.UtcNow;

        if (_outage is not { } outage)
        {
            _outage = (now, now);
            logger.LogError(ex, "The database cannot be reached. Changes stay in memory and are retried.");
        }
        else if (now - outage.Reminded >= OutageReminder)
        {
            _outage = (outage.Since, now);
            logger.LogWarning("The database has been unreachable since {Since}.", outage.Since);
        }
    }

    private void Reached()
    {
        if (_outage is { } outage)
        {
            _outage = null;
            logger.LogInformation("The database is reachable again after {Downtime}.", world.UtcNow - outage.Since);
        }
    }
}
