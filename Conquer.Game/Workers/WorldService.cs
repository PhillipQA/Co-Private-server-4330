using Microsoft.Extensions.Options;

namespace Conquer.Game.Workers;

public class WorldService(
    World world,
    GameData data,
    GameStore store,
    ScriptService scripts,
    IOptions<GameServerOptions> options,
    TimeProvider time,
    ILogger<WorldService> logger) : BackgroundService
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan SaveInterval = TimeSpan.FromSeconds(10);

    private readonly List<DiceTable> _tables = [];
    private int _minute;
    private DateOnly _feeDay;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await data.LoadAsync();
        world.DMaps = DMapLoader.Load(options.Value.ClientPath);
        world.Scenes = Scene.LoadTerrainNpcs(options.Value.ClientPath);
        await store.LoadWorldAsync();

        foreach (var table in world.Maps.Values.SelectMany(map => map.Npcs).Where(npc => npc.Type == DiceTable.TableType))
        {
            _tables.Add(table.Dice = new(table));
        }

        _minute = world.Now.Minute;
        _feeDay = world.Now.Hour == 0 ? DateOnly.FromDateTime(world.Now) : default;
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        await store.SaveAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.WhenAll(
        EveryAsync(TickInterval, () => world.Run(Tick), stoppingToken),
        EveryAsync(SaveInterval, store.SaveAsync, stoppingToken));

    private async Task EveryAsync(TimeSpan interval, Func<Task> work, CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(interval, time);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await work();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "World work failed.");
            }
        }
    }

    private void Safely<T>(IEnumerable<T> entities, Action<T> work)
    {
        foreach (var entity in entities)
        {
            try
            {
                work(entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ticking {Entity} failed.", entity);
            }
        }
    }

    private void Tick()
    {
        var now = world.UtcNow;
        Safely(world.DueTimers(now), timer => timer());

        Safely(world.Players.Values, player =>
        {
            player.Client.Beat(now, world.Random);
            player.Tick(now);
        });

        Safely(world.Players.Values.Where(player => player.Team?.Leader == player).Select(leader => leader.Team!), team => team.Tick(now));
        Safely(world.Maps.Values.Select(map => map.War).OfType<GuildWar>(), war => war.Tick(now));
        Safely(_tables, table => table.Tick(now));

        if (world.Now.Minute != _minute)
        {
            _minute = world.Now.Minute;
            Safely("minute scripts", scripts.Tick);

            if (world.Now.Hour == 0 && DateOnly.FromDateTime(world.Now) != _feeDay)
            {
                _feeDay = DateOnly.FromDateTime(world.Now);
                Safely("teleporter fees", world.ChargeTeleporters);
            }
        }

        Safely(world.Maps.Values.Where(map => map.Weather is not null), map =>
        {
            if (map.Weather!.Tick(world.Random, now))
            {
                map.Players.Send(map.Weather.Message);
            }
        });

        Safely("spawning", () => world.Generate(now));

        foreach (var map in world.Maps.Values.Where(map => map.PlayerCount > 0))
        {
            Safely(map.Monsters.ToList(), monster => monster.Tick(now));
            Safely(map.Items.Where(drop => drop.IsExpired(now)).ToList(), map.Remove);
        }
    }

    private void Safely(string phase, Action work)
    {
        try
        {
            work();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "The world tick failed at {Phase}.", phase);
        }
    }
}
