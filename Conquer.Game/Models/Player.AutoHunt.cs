namespace Conquer.Game.Models;

// Server-side autohunt: the server picks monsters, jumps the player toward them and attacks through the normal
// combat code (melee/bow via Target, or a chosen spell via StartCast), so damage, cooldowns, mana and experience
// behave exactly as in manual play. The client only renders the jumps and attacks it is sent.
public partial class Player
{
    private const int HuntSearchRange = 15;
    private const int HuntLeash = 30;
    private const int HuntJump = 12;
    private const int LootRange = 8;
    private const int WanderMin = 6;
    private const int WanderMax = 10;
    private const int HealthPotionPercent = 50;
    private const int ManaPotionPercent = 30;
    private const int ManaPotionPercentWithMeditation = 10;
    private const int MeditatePercent = 40;
    private const ushort Meditation = 1195;

    // Attack skills each class hunts with, best first. The first one the player knows and can pay for is cast.
    private static readonly ushort[] TaoistHuntSpells = [1002, 1001, 1000]; // Tornado, Fire, Thunder
    private static readonly ushort[] ArcherHuntSpells = [8001];              // Scatter

    private static readonly TimeSpan HuntMoveInterval = TimeSpan.FromMilliseconds(600);
    private static readonly TimeSpan WanderInterval = TimeSpan.FromSeconds(3);
    private static readonly TimeSpan UnreachableFor = TimeSpan.FromSeconds(15);

    private readonly Dictionary<uint, DateTimeOffset> _huntSkipped = [];

    private bool _hunting;
    private uint _huntMapId;
    private (ushort X, ushort Y) _huntHome;
    private ushort _huntSpell;
    private bool _huntMelee;
    private Monster? _prey;
    private DateTimeOffset _nextHuntMove;
    private DateTimeOffset _nextWander;
    private bool _warnedNoPotions;

    public bool IsHunting => _hunting;

    public void ToggleHunt(string? argument)
    {
        switch (argument?.Trim().ToLowerInvariant())
        {
            case null or "":
                if (_hunting)
                {
                    StopHunt("Autohunt stopped.");
                }
                else
                {
                    StartHunt(null);
                }
                break;

            case "off" or "stop" when _hunting:
                StopHunt("Autohunt stopped.");
                break;

            case "off" or "stop":
                Tell("Autohunt is not running.", TalkChannel.System);
                break;

            case "on" or "start":
                StartHunt(null);
                break;

            case "melee" or "physical":
                StartHunt(null, melee: true);
                break;

            default:
                StartHunt(argument);
                break;
        }
    }

    private void StartHunt(string? skill, bool melee = false)
    {
        if (!IsAlive || Map is null)
        {
            Tell("You cannot autohunt right now.", TalkChannel.System);
            return;
        }

        if (Map.Info.Is(MapFlags.BoothEnable) || Map.Info.Is(MapFlags.PkDisable) && !Map.Monsters.Any())
        {
            Tell("There is nothing to hunt here.", TalkChannel.System);
            return;
        }

        ushort spellType = 0;

        if (!string.IsNullOrWhiteSpace(skill))
        {
            if (FindHuntSpell(skill) is not { } spell)
            {
                Tell($"You don't know an attack skill called \"{skill}\".", TalkChannel.System);
                return;
            }

            spellType = spell.Type;
        }

        _hunting = true;
        _huntMapId = MapId;
        _huntHome = (X, Y);
        _huntSpell = spellType;
        _huntMelee = melee;
        _prey = null;
        _huntSkipped.Clear();
        _warnedNoPotions = false;

        var with = PreferredHuntSpell() is { } preferred ? $"using {preferred.Name}" : "using physical attacks";
        var meditating = !_huntMelee && KnownMagic(Meditation) is not null ? ", meditating when mana is low" : "";
        Tell($"Autohunt started {with}{meditating}. Type /autohunt again or move to stop.", TalkChannel.System);
    }

    public void StopHunt(string? reason = null)
    {
        if (!_hunting)
        {
            return;
        }

        _hunting = false;
        _prey = null;
        _huntSkipped.Clear();
        Target = null;

        if (reason is not null)
        {
            Tell(reason, TalkChannel.System);
        }
    }

    private MagicType? FindHuntSpell(string skill)
    {
        IEnumerable<MagicType?> spells = ushort.TryParse(skill, out var type)
            ? [Known(type)]
            : KnownMagics.Select(magic => Known(magic.Type))
                .Where(spell => spell is not null &&
                                spell.Name.Replace(" ", "").Equals(skill.Replace(" ", ""), StringComparison.OrdinalIgnoreCase));

        return spells.FirstOrDefault(spell => spell is
        {
            Offensive: not 0, XpSkill: 0, AutoActive: 0, Sort: not MagicSort.Dashing
        } && (spell.Target & SelfOnlyTarget) == 0 && KnownMagic(spell.Type) is not null);
    }

    private void TickHunt(DateTimeOffset now)
    {
        if (!_hunting)
        {
            return;
        }

        if (!IsAlive)
        {
            StopHunt("Autohunt stopped because you died.");
            return;
        }

        if (Map is not { } map || MapId != _huntMapId)
        {
            StopHunt("Autohunt stopped because you left the map.");
            return;
        }

        if (AttackKind == InteractAction.Shoot && Arrows == 0)
        {
            StopHunt("Autohunt stopped because you are out of arrows.");
            return;
        }

        if (Trade is not null || Booth is not null)
        {
            return;
        }

        DrinkIfNeeded();

        if (_casting is not null || TryMeditate(now))
        {
            return;
        }

        foreach (var expired in _huntSkipped.Where(skip => now >= skip.Value).Select(skip => skip.Key).ToList())
        {
            _huntSkipped.Remove(expired);
        }

        if (!IsValidPrey(_prey))
        {
            _prey = null;
            Target = null;

            if (TryLoot(map, now))
            {
                return;
            }

            _prey = FindPrey(map);
        }

        if (_prey is not { } prey)
        {
            Wander(map, now);
            return;
        }

        var spell = HuntSpell();
        var range = spell?.CastRange ?? ReachTo(prey);

        if (DistanceTo(prey) > range)
        {
            if (now < _nextHuntMove)
            {
                return;
            }

            if (HuntLanding(map, prey.X, prey.Y, range) is not { } landing || !HuntJumpTo(landing.X, landing.Y))
            {
                _huntSkipped[prey.Id] = now + UnreachableFor;
                _prey = null;
                return;
            }

            _nextHuntMove = now + HuntMoveInterval;

            if (DistanceTo(prey) > range)
            {
                return;
            }
        }

        if (spell is not null)
        {
            if (Ready(spell, now))
            {
                StartCast(spell, prey, prey.X, prey.Y, now);
            }

            return;
        }

        if (Target != prey)
        {
            Target = prey;
        }
    }

    private ushort[] HuntSpellChoices => _huntMelee ? []
        : _huntSpell != 0 ? [_huntSpell]
        : Profession >= Profession.InternTaoist ? TaoistHuntSpells
        : Profession is >= Profession.InternArcher and <= Profession.ArcherMaster ? ArcherHuntSpells
        : [];

    // The skill autohunt would like to use, ignoring whether it can be paid for right now.
    private MagicType? PreferredHuntSpell() => HuntSpellChoices
        .Where(type => KnownMagic(type) is not null)
        .Select(type => Known(type))
        .FirstOrDefault(spell => spell is not null);

    // The best skill that can be cast right now; null means fall back to normal attacks.
    private MagicType? HuntSpell() => HuntSpellChoices
        .Where(type => KnownMagic(type) is not null)
        .Select(type => Known(type))
        .FirstOrDefault(spell => spell is not null && Mana >= spell.ManaCost && Stamina >= spell.StaminaCost && Arrows >= spell.ArrowCost);

    private bool TryMeditate(DateTimeOffset now)
    {
        if (_huntMelee || Mana * 100 >= Stats.MaxMana * MeditatePercent || KnownMagic(Meditation) is null ||
            Known(Meditation) is not { } meditation || !Ready(meditation, now) || Stamina < meditation.StaminaCost)
        {
            return false;
        }

        return StartCast(meditation, this, X, Y, now);
    }

    private bool IsValidPrey(Monster? monster) =>
        monster is not null && monster.IsAlive && monster.Map == Map && DistanceTo(monster) <= Map.ViewRange &&
        Entity.Distance(monster.X, monster.Y, _huntHome.X, _huntHome.Y) <= HuntLeash + HuntSearchRange;

    private Monster? FindPrey(Map map) => map.Around(X, Y, HuntSearchRange)
        .OfType<Monster>()
        .Where(monster => monster.IsAlive && monster.Owner is null && IsHuntedMonster(monster) && CanAttack(monster) &&
                          !_huntSkipped.ContainsKey(monster.Id) &&
                          (monster.Target is not Player fighting || fighting == this))
        .Where(monster => Entity.Distance(monster.X, monster.Y, _huntHome.X, _huntHome.Y) <= HuntLeash)
        .OrderBy(entity => DistanceTo(entity))
        .FirstOrDefault();

    private bool TryLoot(Map map, DateTimeOffset now)
    {
        var drop = map.Around(X, Y, LootRange)
            .OfType<MapItem>()
            .Where(item => (item.Item is null || item.ItemTypeId is ItemType.DragonBall or ItemType.Meteor) &&
                           !item.IsReservedFrom(this, now) && !_huntSkipped.ContainsKey(item.Id) &&
                           (item.Item is not null ? FreeSlots > 0 : CanEarn(item.Money)))
            .OrderBy(entity => DistanceTo(entity))
            .FirstOrDefault();

        if (drop is null)
        {
            return false;
        }

        if ((drop.X, drop.Y) != (X, Y))
        {
            if (now < _nextHuntMove)
            {
                return true;
            }

            if (!CanLand(map, drop.X, drop.Y) || !HuntJumpTo(drop.X, drop.Y))
            {
                _huntSkipped[drop.Id] = now + UnreachableFor;
                return false;
            }

            _nextHuntMove = now + HuntMoveInterval;
        }

        if (!PickUp(drop, now))
        {
            _huntSkipped[drop.Id] = now + UnreachableFor;
        }

        return true;
    }

    private void Wander(Map map, DateTimeOffset now)
    {
        if (now < _nextWander)
        {
            return;
        }

        _nextWander = now + WanderInterval;

        for (var attempt = 0; attempt < 12; attempt++)
        {
            var angle = World.Random.NextDouble() * Math.PI * 2;
            var length = World.Random.Next(WanderMin, WanderMax + 1);
            var tooFar = Entity.Distance(X, Y, _huntHome.X, _huntHome.Y) > HuntLeash / 2;
            var (fromX, fromY) = tooFar ? (_huntHome.X, _huntHome.Y) : (X, Y);
            var x = fromX + (int)Math.Round(Math.Cos(angle) * length);
            var y = fromY + (int)Math.Round(Math.Sin(angle) * length);

            if (tooFar)
            {
                (x, y) = PartWay(x, y, HuntJump);
            }

            if (Entity.Distance(x, y, _huntHome.X, _huntHome.Y) <= HuntLeash && CanLand(map, x, y) && HuntJumpTo((ushort)x, (ushort)y))
            {
                return;
            }
        }
    }

    private (int X, int Y) PartWay(int x, int y, int limit)
    {
        var distance = DistanceTo(x, y);
        return distance <= limit ? (x, y) : (X + (x - X) * limit / distance, Y + (y - Y) * limit / distance);
    }

    private (ushort X, ushort Y)? HuntLanding(Map map, int targetX, int targetY, int range)
    {
        var reach = Math.Max(1, range);
        (ushort X, ushort Y)? best = null;
        var bestDistance = int.MaxValue;

        for (var dx = -reach; dx <= reach; dx++)
        {
            for (var dy = -reach; dy <= reach; dy++)
            {
                var (x, y) = (targetX + dx, targetY + dy);

                if ((dx, dy) == (0, 0) || !CanLand(map, x, y) || DistanceTo(x, y) >= bestDistance)
                {
                    continue;
                }

                (best, bestDistance) = (((ushort)x, (ushort)y), DistanceTo(x, y));
            }
        }

        if (best is not null)
        {
            return best;
        }

        var distance = DistanceTo(targetX, targetY);

        for (var step = Math.Min(HuntJump, distance - 1); step >= 2; step--)
        {
            var (x, y) = (X + (targetX - X) * step / distance, Y + (targetY - Y) * step / distance);

            if (CanLand(map, x, y))
            {
                return ((ushort)x, (ushort)y);
            }
        }

        var (pathX, pathY) = ((int)X, (int)Y);
        (ushort X, ushort Y)? farthest = null;

        foreach (var direction in Pathfinder.Find(map, (X, Y), (targetX, targetY)))
        {
            var (stepX, stepY) = direction.Step();
            (pathX, pathY) = (pathX + stepX, pathY + stepY);

            if (DistanceTo(pathX, pathY) > HuntJump)
            {
                break;
            }

            if (CanLand(map, pathX, pathY))
            {
                farthest = ((ushort)pathX, (ushort)pathY);
            }
        }

        return farthest;
    }

    private bool CanLand(Map map, int x, int y) =>
        map.IsWalkable(x, y) && DistanceTo(x, y) <= HuntJump && !Vaults((ushort)x, (ushort)y) && !map.HasNpcAt(x, y);

    private bool HuntJumpTo(ushort x, ushort y)
    {
        if ((x, y) == (X, Y))
        {
            return true;
        }

        var jump = new MsgAction
        {
            Id = Id,
            X = X,
            Y = Y,
            DataX = x,
            DataY = y,
            Direction = (ushort)Directions.Toward(x - X, y - Y),
            Action = ActionType.Jump
        };

        if (!Jump(x, y, jump))
        {
            return false;
        }

        // The 4330 client may ignore a jump packet for its own character, so also tell it where the hero now
        // stands. KickBack makes the client place the hero at the coordinates sent (the new position).
        Send(Act(ActionType.KickBack));
        return true;
    }

    private void DrinkIfNeeded()
    {
        if (Health * 100 < Stats.MaxHealth * HealthPotionPercent && !DrinkBest(potion => potion.Health, Stats.MaxHealth - Health))
        {
            if (!_warnedNoPotions)
            {
                _warnedNoPotions = true;
                Tell("Autohunt: you are out of HP potions.", TalkChannel.System);
            }
        }

        var manaPercent = KnownMagic(Meditation) is null ? ManaPotionPercent : ManaPotionPercentWithMeditation;

        if (PreferredHuntSpell() is not null && Mana * 100 < Stats.MaxMana * manaPercent)
        {
            DrinkBest(potion => potion.Mana, Stats.MaxMana - Mana);
        }
    }

    // Drinks the smallest potion that covers what is missing, or the biggest one if none does.
    private bool DrinkBest(Func<ItemType, int> restores, int missing)
    {
        var potions = Inventory
            .Select(item => (Item: item, Amount: World.Data.ItemTypes.TryGetValue(item.ItemTypeId, out var type) ? restores(type) : 0))
            .Where(potion => potion.Amount > 0)
            .ToList();

        if (potions.Count == 0)
        {
            return false;
        }

        var choice = potions.Where(potion => potion.Amount >= missing).OrderBy(potion => potion.Amount).Select(potion => potion.Item).FirstOrDefault();
        return Drink(choice ?? potions.MaxBy(potion => potion.Amount).Item);
    }

    protected internal override void OnAttacked(Fighter attacker)
    {
        base.OnAttacked(attacker);

        var assailant = attacker is Monster { Owner: { } master } ? master : attacker;

        if (_hunting && assailant is Player other && other != this)
        {
            StopHunt($"Autohunt stopped because {other.Name} attacked you.");
        }
    }
}
