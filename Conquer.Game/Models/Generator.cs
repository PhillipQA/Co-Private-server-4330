namespace Conquer.Game.Models;

public class Generator
{
    private const int RandomRest = 600;

    private TimeSpan _rest;
    private DateTimeOffset? _waved;
    private int _budget;

    public uint Id { get; set; }
    public uint MapId { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort Width { get; set; }
    public ushort Height { get; set; }
    public ushort MaxMonsters { get; set; }
    public ushort RespawnSeconds { get; set; }
    public ushort MaxPerWave { get; set; }
    public uint MonsterTypeId { get; set; }

    internal int Alive { get; private set; }

    public int Capacity => MaxMonsters < 1 ? 1 : Math.Max(1, Width / MaxMonsters * (Height / MaxMonsters));
    public (int X, int Y) Center => (X + Width / 2, Y + Height / 2);

    public bool Contains(int x, int y) => x >= X && x < X + Width && y >= Y && y < Y + Height;

    public bool IsFar(int x, int y, int range) =>
        x < X - range || x >= X + Width + range || y < Y - range || y >= Y + Height + range;

    public void Release() => Alive--;

    public int Spawn(Map map, MonsterType monsterType, DateTimeOffset now, int limit)
    {
        var random = map.World.Random;

        if (_waved is null)
        {
            _rest = Rest(random);
            _waved = now;
        }

        if (Alive >= Capacity)
        {
            return 0;
        }

        if (now >= _waved + _rest)
        {
            _waved = now;
            _budget = MaxPerWave;
            _rest = RespawnSeconds >= RandomRest ? Rest(random) : _rest;
        }

        var wave = Math.Min(limit, Math.Min(_budget, Capacity - Alive));

        for (var spawned = 0; spawned < wave; spawned++)
        {
            if (Place(map, random) is not (var x, var y))
            {
                return spawned;
            }

            _budget--;
            Alive++;
            var monster = new Monster(monsterType, this) { Id = map.World.MonsterIds.Next(), Direction = (Direction)random.Next(8) };
            map.Add(monster, (ushort)x, (ushort)y);
        }

        return wave;
    }

    private TimeSpan Rest(Random random) => TimeSpan.FromSeconds(RespawnSeconds + random.Next(RespawnSeconds));

    private (int X, int Y)? Place(Map map, Random random)
    {
        for (var tries = Math.Max(1, Width * Height); tries > 0; tries--)
        {
            var (x, y) = (X + random.Next(Width), Y + random.Next(Height));

            if (map.IsWalkable(x, y))
            {
                return (x, y);
            }
        }

        return null;
    }
}
