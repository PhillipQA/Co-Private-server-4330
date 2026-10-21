namespace Conquer.Game.Models;

public sealed class Map(MapInfo info, DMap dMap, World world)
{
    public const int ViewRange = 18;

    private const int DropAttempts = 5;
    private const int MaxSize = 4;
    private const int MaxReach = (MaxSize + 1) / 2;

    private static readonly (int X, int Y)[] Beside = [(0, 0), (1, 1), (-1, -1), (1, 0), (0, 1), (1, -1), (-1, 1), (-1, 0), (0, -1)];

    private List<Entity>?[,] _blocks = Blocks(dMap);
    private bool[,] _awake = Awake(dMap);

    private readonly Dictionary<uint, Entity> _entities = [];
    private readonly Dictionary<(int X, int Y), DMapCell> _terrain = [];
    private readonly Dictionary<uint, List<(int X, int Y)>> _scenery = [];

    public uint Id => info.Id;
    public MapInfo Info => info;
    public DMap DMap => dMap;
    public ushort DocId => info.DocId;
    public uint OwnerId => War?.Pole.OwnerId ?? info.OwnerId;
    public byte Level { get => info.Level; set => info.Level = value; }
    public ushort PortalX { get => info.PortalX; set => info.PortalX = value; }
    public ushort PortalY { get => info.PortalY; set => info.PortalY = value; }
    public Dictionary<(ushort X, ushort Y), Portal> Portals { get; } = [];
    public List<Region> Regions { get; } = [];
    public List<Generator> Generators { get; } = [];
    public int PlayerCount { get; private set; }
    public IEnumerable<Player> Players => _entities.Values.OfType<Player>();
    public IEnumerable<Monster> Monsters => _entities.Values.OfType<Monster>();
    public IEnumerable<Npc> Npcs => _entities.Values.OfType<Npc>();
    public IEnumerable<DynaNpc> DynaNpcs => _entities.Values.OfType<DynaNpc>();
    public IEnumerable<MapItem> Items => _entities.Values.OfType<MapItem>();
    public World World => world;
    public GuildWar? War { get; set; }
    public Weather? Weather { get; set; }

    public T? Find<T>(uint id) where T : Entity => _entities.GetValueOrDefault(id) as T;

    public Portal? PortalBeside(Entity entity) => Beside
        .Select(offset => Portals.GetValueOrDefault(((ushort)(entity.X + offset.X), (ushort)(entity.Y + offset.Y))))
        .FirstOrDefault(portal => portal is not null);

    public bool IsWalkable(int x, int y) => InBounds(x, y) && Cell(x, y).Mask == 0;

    public DMapCell Cell(int x, int y) => _terrain.TryGetValue((x, y), out var cell) ? cell : dMap.Cells[x, y];

    public void Redesign(ushort docId)
    {
        info.DocId = docId;
        dMap = world.DMaps[docId];
        _blocks = Blocks(dMap);
        _awake = Awake(dMap);
        Link();

        foreach (var entity in _entities.Values)
        {
            Block(entity.X, entity.Y).Add(entity);
        }

        foreach (var player in Players)
        {
            Wake(player.X, player.Y);
        }
    }

    public void Link()
    {
        if (info.LinkMapId == 0)
        {
            return;
        }

        Portals.Clear();

        foreach (var exit in dMap.Portals)
        {
            Portals[(exit.PortalX, exit.PortalY)] = new() { MapId = info.LinkMapId, PortalX = info.LinkX, PortalY = info.LinkY };
        }
    }

    public bool InRegion(uint type, int x, int y) => Regions.Any(region => region.Type == type &&
        x >= region.X && x < region.X + region.Width && y >= region.Y && y < region.Y + region.Height);

    public int CountNpcs(byte type) => Npcs.Count(npc => npc.Type == type) + DynaNpcs.Count(npc => npc.Type == type);

    public int CountNpcs(string name) => Npcs.Count(npc => npc.Name == name) + DynaNpcs.Count(npc => npc.Name == name);

    public void Build(DynaNpc npc)
    {
        Demolish(npc);

        if (!world.Scenes.TryGetValue(npc.Look, out var scene))
        {
            return;
        }

        var cells = scene.On(dMap, npc.X, npc.Y).ToList();
        cells.ForEach(placed => _terrain[(placed.X, placed.Y)] = placed.Cell);
        _scenery[npc.Id] = [.. cells.Select(placed => (placed.X, placed.Y))];
    }

    public void Demolish(DynaNpc npc)
    {
        if (_scenery.Remove(npc.Id, out var built))
        {
            built.ForEach(at => _terrain.Remove(at));
        }
    }

    public bool CanStep(int x, int y, Direction direction, int size, int climb = 0)
    {
        size = Math.Min(size, MaxSize);
        var (nx, ny) = (x + direction.Step().X, y + direction.Step().Y);

        if (!IsWalkable(nx, ny) || size <= 2 && Roles(nx, ny) > size || climb > 0 && Cell(nx, ny).Altitude - Cell(x, y).Altitude > climb)
        {
            return false;
        }

        var spare = size % 2;

        if (size == 0)
        {
            return true;
        }

        if (size > 2)
        {
            var reach = (size + 1) / 2;

            for (var i = nx - reach; i <= nx + reach; i++)
            {
                for (var j = ny - reach; j <= ny + reach; j++)
                {
                    if (Entity.Distance(i, j, x, y) > reach && InBounds(i, j) && Roles(i, j) > spare)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        var wide = (int)direction % 2 == 0 ? 2 : 1;

        for (var turn = -wide; turn <= wide; turn++)
        {
            var side = ((Direction)(((int)direction + turn + 8) % 8)).Step();

            if (InBounds(nx + side.X, ny + side.Y) && Roles(nx + side.X, ny + side.Y) > spare)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsStacked(Entity entity) =>
        Block(entity.X, entity.Y).Any(other => other is Player && other != entity && (other.X, other.Y) == (entity.X, entity.Y));

    public IEnumerable<Entity> Scan(Entity around) => BlocksAround(around.X, around.Y, ViewRange)
        .SelectMany(block => block.Where(entity => entity is Player).Concat(block.Where(entity => entity is Monster)));

    public bool HasNpcAt(int x, int y) => Block(x, y).Any(entity => entity is Npc or DynaNpc && entity.X == x && entity.Y == y);

    public bool HasTrapAt(int x, int y, ushort type) =>
        Block(x, y).Any(entity => entity is Trap trap && trap.Type == type && trap.X == x && trap.Y == y);

    public bool Drop(MapItem drop, Entity near, DateTimeOffset now)
    {
        for (var attempt = 0; attempt < DropAttempts; attempt++)
        {
            if (Drop(drop, near.X + world.Random.Next(-1, 2), near.Y + world.Random.Next(-1, 2), now))
            {
                return true;
            }
        }

        return false;
    }

    public bool Drop(MapItem drop, int x, int y, DateTimeOffset now)
    {
        if (!IsWalkable(x, y) || Block(x, y).Any(entity => entity is MapItem && entity.X == x && entity.Y == y))
        {
            return false;
        }

        drop.Id = world.MapItemIds.Next();
        drop.Dropped = now;
        Add(drop, (ushort)x, (ushort)y);
        return true;
    }

    public void Add(Entity entity, ushort x, ushort y, IMessage? entrance = null)
    {
        entity.Map = this;
        (entity.MapId, entity.X, entity.Y) = (Id, x, y);
        _entities[entity.Id] = world.Entities[entity.Id] = entity;
        Block(x, y).Add(entity);
        PlayerCount += entity is Player ? 1 : 0;
        (entity as Player)?.Send(new MsgAction
        {
            Id = entity.Id,
            X = x,
            Y = y,
            Data = Info.Light,
            Action = ActionType.MapColor
        });

        foreach (var other in InRange(entity))
        {
            Introduce(entity, other, entrance);
        }

        if (entity is Player player)
        {
            Wake(x, y);

            if (Weather is { } weather)
            {
                player.Send(weather.Message);
            }
        }
        else if (entity is Monster monster)
        {
            monster.Settle(world.UtcNow);
        }

        Arrive(entity);
    }

    internal bool IsDeserted(Entity entity)
    {
        var (blockX, blockY) = (entity.X / ViewRange, entity.Y / ViewRange);

        if (!_awake[blockX, blockY])
        {
            return true;
        }

        if (BlocksAround(entity.X, entity.Y, ViewRange).Any(block => block.Any(other => other is Player)))
        {
            return false;
        }

        _awake[blockX, blockY] = false;
        return true;
    }

    public void Remove(Entity entity)
    {
        if (entity is Player player && Weather is not null)
        {
            player.Send(MsgWeather.Fine);
        }

        Broadcast(entity, entity.Despawn, false);
        _entities.Remove(entity.Id);
        world.Entities.Remove(entity.Id);
        Block(entity.X, entity.Y).Remove(entity);
        PlayerCount -= entity is Player ? 1 : 0;
        entity.Map = null;
    }

    public bool Move(Entity entity, ushort x, ushort y, IMessage because)
    {
        if (!IsWalkable(x, y) || !entity.CanSee(x, y))
        {
            (entity as Player)?.KickBack();
            return false;
        }

        Broadcast(entity, because);
        Shift(entity, x, y);
        return true;
    }

    public void Relocate(Entity entity, ushort x, ushort y, IMessage because)
    {
        foreach (var other in InRange(entity).OfType<Player>())
        {
            other.Send(other.CanSee(x, y) ? because : entity.Despawn);
        }

        Shift(entity, x, y);
    }

    public void Shift(Entity entity, ushort x, ushort y)
    {
        var (oldX, oldY) = (entity.X, entity.Y);
        var (from, to) = (Block(oldX, oldY), Block(x, y));
        (entity.X, entity.Y) = (x, y);

        if (from != to)
        {
            from.Remove(entity);
            to.Add(entity);

            if (entity is Player)
            {
                Wake(x, y);
            }
        }

        foreach (var other in InRange(entity))
        {
            if (!other.CanSee(oldX, oldY))
            {
                Introduce(entity, other);
            }
        }

        Arrive(entity);
    }

    public void Transfer(Player player, Map to, ushort x, ushort y)
    {
        if (!to.InBounds(x, y))
        {
            throw new ArgumentOutOfRangeException(nameof(x), $"({x},{y}) is outside map {to.Id} ({to.DMap.Width}x{to.DMap.Height}).");
        }

        player.Trade?.Cancel();
        player.Booth?.Close();
        player.DismissPet();
        player.Interrupt();

        if (to.Info.Is(MapFlags.WingDisable))
        {
            player.ClearEffect(PlayerStatus.Flying);
        }

        Remove(player);
        player.Send(new MsgAction
        {
            Id = player.Id,
            Data = to.DocId,
            X = x,
            Y = y,
            Direction = (ushort)player.Direction,
            Action = ActionType.EnterMap
        });
        player.Send(new MsgMapInfo(to));
        to.Add(player, x, y);
        player.Protect(world.UtcNow);
    }

    public void Broadcast(Entity around, IMessage message, bool self = true)
    {
        if (self)
        {
            (around as Player)?.Send(message);
        }

        InRange(around).OfType<Player>().Send(message);
    }

    public void Each(int limit, Action<Player, Dialog> script)
    {
        foreach (var player in Players.Take(limit < 0 ? int.MaxValue : limit).ToList())
        {
            script(player, new Dialog(player));
        }
    }

    public void ShowEffect(ushort x, ushort y, string effect) =>
        Players.Where(player => player.CanSee(x, y)).Send(new MsgName
        {
            X = x,
            Y = y,
            Action = NameAction.MapEffect,
            Strings = [effect]
        });

    public IEnumerable<Entity> InRange(Entity around) =>
        BlocksAround(around.X, around.Y, ViewRange).SelectMany(block => block).Where(around.CanSee);

    public IEnumerable<Entity> Around(int x, int y, int range) => BlocksAround(x, y, range)
        .SelectMany(block => block)
        .Where(entity => entity.DistanceTo(x, y) <= range);

    private void Arrive(Entity entity)
    {
        if (entity is Player player && Block(player.X, player.Y).OfType<Trap>().FirstOrDefault(trap => (trap.X, trap.Y) == (player.X, player.Y)) is { } trap)
        {
            trap.Spring(player, world.UtcNow);
        }
    }

    private static void Introduce(Entity a, Entity b, IMessage? entrance = null)
    {
        if (a is Player viewer)
        {
            b.ShowTo(viewer);
        }

        if (b is not Player other)
        {
            return;
        }

        if (entrance is null)
        {
            a.ShowTo(other);
        }
        else
        {
            other.Send(entrance);
        }
    }

    private int Roles(int x, int y) => BlocksAround(x, y, MaxReach).SelectMany(block => block).Sum(entity => entity switch
    {
        Player { IsAlive: true } => Footprint(entity, 0, x, y),
        Monster { IsAlive: true } monster => Footprint(entity, Math.Min((int)monster.MonsterType.SizeAdd, MaxSize), x, y),
        _ => 0
    });

    private IEnumerable<List<Entity>> BlocksAround(int x, int y, int range)
    {
        var (left, top, right, bottom) = BlockSpan(x, y, range);

        for (var blockX = left; blockX <= right; blockX++)
        {
            for (var blockY = top; blockY <= bottom; blockY++)
            {
                if (_blocks[blockX, blockY] is { } block)
                {
                    yield return block;
                }
            }
        }
    }

    private void Wake(int x, int y)
    {
        var (left, top, right, bottom) = BlockSpan(x, y, ViewRange);
        var now = world.UtcNow;

        for (var blockX = left; blockX <= right; blockX++)
        {
            for (var blockY = top; blockY <= bottom; blockY++)
            {
                if (_awake[blockX, blockY])
                {
                    continue;
                }

                _awake[blockX, blockY] = true;

                foreach (var monster in _blocks[blockX, blockY]?.OfType<Monster>() ?? [])
                {
                    monster.WakeUp(now);
                }
            }
        }
    }

    private (int Left, int Top, int Right, int Bottom) BlockSpan(int x, int y, int range) => (
        Math.Max((x - range) / ViewRange, 0), Math.Max((y - range) / ViewRange, 0),
        Math.Min((x + range) / ViewRange, _blocks.GetLength(0) - 1), Math.Min((y + range) / ViewRange, _blocks.GetLength(1) - 1));

    private static int Footprint(Entity entity, int size, int x, int y)
    {
        var reach = (size + 1) / 2;
        var (dx, dy) = (Math.Abs(entity.X - x), Math.Abs(entity.Y - y));

        return dx > reach || dy > reach ? 0 : size % 2 != 0 && (dx == reach || dy == reach) ? 1 : 2;
    }

    public bool InBounds(int x, int y) => dMap.InBounds(x, y);

    private List<Entity> Block(int x, int y) => _blocks[x / ViewRange, y / ViewRange] ??= [];

    private static List<Entity>?[,] Blocks(DMap map) => new List<Entity>?[map.Width / ViewRange + 1, map.Height / ViewRange + 1];

    private static bool[,] Awake(DMap map) => new bool[map.Width / ViewRange + 1, map.Height / ViewRange + 1];
}
