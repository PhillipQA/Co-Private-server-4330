namespace Conquer.Game.Models;

public sealed class World
{
    private const uint DynamicMapIds = 1000001;
    public const uint LastNpcId = 399999;
    public const uint FirstStatueId = 100001;
    public const uint LastStatueId = 199999;
    public const uint FirstMonsterId = 400001;
    public const uint LastMonsterId = 499999;
    public const uint FirstPetId = 700001;
    public const uint LastPetId = 799999;

    private const uint TeleporterFee = 10000;
    private const int SpawnsPerTick = 20;

    private readonly TaskFactory _loop = new(new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler);
    private readonly PriorityQueue<Action, (DateTimeOffset Due, long Order)> _timers = new();

    private long _timerOrder;
    private int _nextGenerator;
    private int _spawnStep = 1;

    internal Dictionary<uint, Entity> Entities { get; } = [];
    public Dictionary<uint, Map> Maps { get; } = [];
    public Dictionary<uint, Player> Players { get; } = [];
    public Dictionary<uint, Guild> Guilds { get; } = [];

    public Task Run(Action work) => _loop.StartNew(work);
    public Task Run(Func<Task> work) => _loop.StartNew(work).Unwrap();
    public Task<T> Run<T>(Func<T> work) => _loop.StartNew(work);

    public void After(TimeSpan delay, Action work) => _timers.Enqueue(work, (UtcNow + delay, _timerOrder++));

    public void Generate(DateTimeOffset now)
    {
        var generators = Maps.Values.SelectMany(map => map.Generators, (map, generator) => (Map: map, Generator: generator)).ToList();
        var budget = SpawnsPerTick;

        for (var visited = 0; visited < generators.Count; visited++)
        {
            _nextGenerator %= generators.Count;
            var (map, generator) = generators[_nextGenerator];
            budget -= generator.Spawn(map, Data.MonsterTypes[generator.MonsterTypeId], now, Math.Min(budget, _spawnStep));

            if (budget == 0)
            {
                break;
            }

            _nextGenerator++;
        }

        if (budget == 0)
        {
            _spawnStep = Math.Max(1, _spawnStep - 1);
        }
        else
        {
            var active = (SpawnsPerTick - budget) / _spawnStep;
            _spawnStep = active > 0 ? Math.Clamp(SpawnsPerTick / active, 1, SpawnsPerTick) : SpawnsPerTick;
        }
    }

    public IEnumerable<Action> DueTimers(DateTimeOffset now)
    {
        while (_timers.TryPeek(out _, out var due) && due.Due <= now)
        {
            yield return _timers.Dequeue();
        }
    }

    public World(GameData data, ScriptService scripts, TimeProvider clock)
    {
        Data = data;
        Scripts = scripts;
        Clock = clock;
        MonsterIds = new(FirstMonsterId, LastMonsterId, IsLive);
        ScriptMonsterIds = new(500001, 599999, IsLive);
        PetIds = new(FirstPetId, LastPetId, IsLive);
        MapItemIds = new(2000001, 2999999, IsLive);
        TrapIds = new(900001, 989999, IsLive);
        NpcIds = new(360001, LastNpcId, IsLive);
        StatueIds = new(FirstStatueId, LastStatueId, IsLive);
        MapIds = new(DynamicMapIds, uint.MaxValue, Maps.ContainsKey);
    }

    public IdSequence MonsterIds { get; }
    public IdSequence ScriptMonsterIds { get; }
    public IdSequence PetIds { get; }
    public IdSequence MapItemIds { get; }
    public IdSequence TrapIds { get; }
    public IdSequence ItemIds { get; } = new(1, uint.MaxValue);
    public IdSequence NpcIds { get; }
    public IdSequence StatueIds { get; }
    public IdSequence MapIds { get; }
    public MessageBoard Board { get; } = new();
    public Dictionary<uint, DMap> DMaps { get; set; } = [];
    public BountyBoard Bounties { get; } = new();
    public Dictionary<string, uint> FoundingGuilds { get; } = new(StringComparer.OrdinalIgnoreCase);
    public KoBoard KoBoard { get; } = new();
    public List<Player> Fugitives { get; } = [];
    public HashSet<string> Divorcing { get; } = [];
    public Dictionary<uint, Scene> Scenes { get; set; } = [];
    public Random Random { get; } = Random.Shared;
    public GameData Data { get; }
    public ScriptService Scripts { get; }
    public TimeProvider Clock { get; }
    public DateTime Now => Clock.GetLocalNow().DateTime;
    public DateTimeOffset UtcNow => Clock.GetUtcNow();

    public int Roll(int sides) => Random.Next(sides);

    public static bool IsPetId(uint id) => id is >= FirstPetId and <= LastPetId;

    public int Roll(int first, int last) => Random.Next(first, last + 1);

    public T Pick<T>(params ReadOnlySpan<T> choices) => choices[Roll(choices.Length)];

    public void Enter(Player player)
    {
        Players.Add(player.Id, player);
        player.World = this;

        if (player.IsNotorious)
        {
            Outlaw(player);
        }

        if (Divorcing.Remove(player.Name))
        {
            player.Spouse = Player.Unmarried;
        }
    }

    public void Outlaw(Player player)
    {
        if (Fugitives.Contains(player))
        {
            return;
        }

        var index = Fugitives.FindIndex(fugitive => fugitive.Level < player.Level);
        Fugitives.Insert(index < 0 ? Fugitives.Count : index, player);
    }

    public int CountAlivePlayers(uint mapId) => Maps.TryGetValue(mapId, out var map) ? map.Players.Count(player => player.IsAlive) : 0;

    public Player? Find(string name) => Players.Values.FirstOrDefault(player => player.Name == name);

    public Guild? FindGuild(string name) => Guilds.Values.FirstOrDefault(guild => guild.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public Guild MainGuildOf(Guild guild) => guild.ParentGuildId is { } parentId && Guilds.TryGetValue(parentId, out var master) ? master : guild;

    public bool AreFamily(Guild guild, Guild other) => MainGuildOf(guild) == MainGuildOf(other);

    public IEnumerable<Guild> Family(Guild guild) => Guilds.Values.Where(other => AreFamily(other, guild));

    public IEnumerable<Player> FamilyMembers(Guild guild) =>
        Players.Values.Where(player => player.Guild is { } joined && AreFamily(joined, guild));

    public void ShowFund(Guild guild)
    {
        foreach (var member in Players.Values.Where(player => player.GuildId == guild.Id))
        {
            member.Send(new MsgSyndicateAttributeInfo(member));
        }
    }

    public bool Spawn(uint monsterTypeId, uint mapId, ushort x, ushort y) => Spawn(monsterTypeId, mapId, x, y, ScriptMonsterIds);

    public bool Spawn(uint monsterTypeId, uint mapId, ushort x, ushort y, IdSequence ids)
    {
        if (!Maps.TryGetValue(mapId, out var map) || !map.IsWalkable(x, y) ||
            !Data.MonsterTypes.TryGetValue(monsterTypeId, out var type))
        {
            return false;
        }

        map.Add(new Monster(type) { Id = ids.Next() }, x, y);
        return true;
    }

    public bool MoveNpc(uint npcId, uint mapId, ushort x, ushort y)
    {
        if (!Maps.TryGetValue(mapId, out var destination) || Find<Npc>(npcId) is not { } npc)
        {
            return false;
        }

        npc.Map!.Remove(npc);
        destination.Add(npc, x, y);
        return true;
    }

    public Map? Open(MapInfo info)
    {
        if (!DMaps.TryGetValue(info.DocId, out var dMap))
        {
            return null;
        }

        var map = Maps[info.Id] = new(info, dMap, this);
        map.Link();
        return map;
    }

    public bool CreateNpc(DynaNpc npc)
    {
        if (!Maps.TryGetValue(npc.MapId, out var map))
        {
            return false;
        }

        npc.Id = (npc.Statue is null ? NpcIds : StatueIds).Next();
        npc.Initialize();
        map.Add(npc, npc.X, npc.Y);

        if (npc.IsScene)
        {
            map.Build(npc);
        }

        return true;
    }

    public bool DeleteNpc(DynaNpc npc)
    {
        if (npc.Map is not { } map)
        {
            return false;
        }

        map.Demolish(npc);
        map.Remove(npc);
        return true;
    }

    public bool DeleteNpcs(uint mapId, byte type)
    {
        if (!Maps.TryGetValue(mapId, out var map))
        {
            return false;
        }

        foreach (var npc in map.DynaNpcs.Where(npc => npc.Type == type).ToList())
        {
            DeleteNpc(npc);
        }

        return true;
    }

    public bool Mark(uint npcId, Entity spot)
    {
        if (FindNpc(npcId) is not { } npc)
        {
            return false;
        }

        (npc.Data0, npc.Data1, npc.Data2) = ((int)spot.MapId, spot.X, spot.Y);
        return true;
    }

    public void ChargeTeleporters()
    {
        foreach (var teleporter in Maps.Values.SelectMany(map => map.DynaNpcs).Where(npc => npc is { IsTeleporter: true, OwnerType: DynaNpc.GuildOwned }))
        {
            Guilds.GetValueOrDefault(teleporter.OwnerId)?.Withdraw(TeleporterFee);
        }
    }

    public void Announce(string text, TalkChannel channel = TalkChannel.System) => Players.Values.Send(MsgTalk.System(text, channel));

    public bool Announce(uint mapId, string text, TalkChannel channel)
    {
        if (Maps.GetValueOrDefault(mapId) is not { } map)
        {
            return false;
        }

        map.Players.Send(MsgTalk.System(text, channel));
        return true;
    }

    public DynaNpc? FindNpc(uint id) => Find<DynaNpc>(id);

    public bool DropItem(uint itemTypeId, uint mapId, ushort x, ushort y) =>
        Maps.TryGetValue(mapId, out var map) && Data.ItemTypes.TryGetValue(itemTypeId, out var type) &&
        map.Drop(new MapItem { Item = Item.Of(type) }, x, y, UtcNow);

    public void RemoveMonsters(uint mapId, uint monsterTypeId) => Vanish(mapId, monster => monster.MonsterType.Id == monsterTypeId);

    public void RemoveGenerated(uint mapId, uint generatorId) => Vanish(mapId, monster => monster.Generator?.Id == generatorId);

    public bool CreateTrap(ushort type, ushort look, uint mapId, ushort x, ushort y, uint data)
    {
        if (!Maps.TryGetValue(mapId, out var map) || map.HasTrapAt(x, y, type))
        {
            return false;
        }

        var trap = new Trap { Id = TrapIds.Next(), Type = type, Look = look, Data = data, X = x, Y = y };
        map.Add(trap, x, y, trap.Cast);
        return true;
    }

    public int CountMonsters(string name, uint mapId) =>
        Maps.GetValueOrDefault(mapId) is { } map ? map.Monsters.Count(monster => monster.MonsterType.Name == name) : 0;

    public bool Leave(Player player)
    {
        if (Players.GetValueOrDefault(player.Id) != player)
        {
            return false;
        }

        try
        {
            player.Depart();
            player.NotifyFriends(false);
            player.Dialog?.Cancel();
            player.Dialog = null;
            player.Trade?.Cancel();
            player.Booth?.Close();
            player.Team?.Remove(player);
            player.DismissPet();

            foreach (var table in player.Map?.Npcs.Select(npc => npc.Dice).OfType<DiceTable>() ?? [])
            {
                table.Leave(player);
            }
        }
        finally
        {
            player.Map?.Remove(player);
            Fugitives.Remove(player);
            Players.Remove(player.Id);
        }

        return true;
    }

    private T? Find<T>(uint id) where T : Entity => Entities.GetValueOrDefault(id) as T;

    private bool IsLive(uint id) => Entities.ContainsKey(id);

    private void Vanish(uint mapId, Func<Monster, bool> match)
    {
        foreach (var monster in Maps.GetValueOrDefault(mapId)?.Monsters.Where(match).ToList() ?? [])
        {
            monster.Vanish();
        }
    }
}
