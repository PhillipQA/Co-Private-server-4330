namespace Conquer.Game.Models;

public partial class Monster
{
    private const int MaxThoughts = 10;
    private const int ClimbCap = 26;
    private const int GuardLeave = 48;
    private const int GuardJump = 6;
    private const int WanderRate = 5;
    private const int ShortWeapon = 2;
    private const int Boldness = 80;

    private static readonly TimeSpan DormancyCheck = TimeSpan.FromSeconds(60);

    private readonly List<Direction> _path = [];

    private Mode _mode;
    private bool _ahead = true;
    private (int X, int Y) _goal;
    private (int X, int Y)? _home;
    private DateTimeOffset _thought;
    private DateTimeOffset _stepped;
    private DateTimeOffset _restless;

    private enum Mode
    {
        Idle,
        Escape,
        Forward,
        Attack,
        Dormant
    }

    public bool IsDormant => _mode == Mode.Dormant;

    internal void Settle(DateTimeOffset now)
    {
        _restless = now + DormancyCheck;

        if (Owner is null)
        {
            Doze();
        }
    }

    internal void WakeUp(DateTimeOffset now)
    {
        _restless = now + DormancyCheck;

        if (IsDormant)
        {
            Change(Mode.Idle);
        }
    }

    private bool Dozes(DateTimeOffset now)
    {
        if (IsDormant)
        {
            return true;
        }

        if (now < _restless)
        {
            return false;
        }

        _restless = now + DormancyCheck;
        return Doze();
    }

    private bool Doze()
    {
        if (!Map!.IsDeserted(this))
        {
            return false;
        }

        Change(Mode.Dormant);
        return true;
    }

    private (int X, int Y) Home => Generator?.Center ?? (_home ??= (X, Y));
    private bool AtHome => Generator?.Contains(X, Y) ?? (X, Y) == Home;
    private bool CanMove => !Is(PlayerStatus.Freeze) && (!MonsterType.Is(Behavior.Fixed) || MonsterType.Is(Behavior.Jump));
    private bool IsFarWeapon => MonsterType.AttackRange > ShortWeapon;
    private TimeSpan ThinkInterval => TimeSpan.FromSeconds(Math.Max(1, MonsterType.AttackSpeed * 3 / 1000));

    protected internal override void OnAttacked(Fighter attacker)
    {
        if (!IsAlive || !CanMove || Owner is not null || MonsterType.IsReviver)
        {
            return;
        }

        if (!MonsterType.Is(Behavior.Passive))
        {
            Target = attacker;
            Change(Mode.Escape);
            return;
        }

        if (Health < MonsterType.FleeHealth)
        {
            Change(Mode.Escape);
            return;
        }

        var distance = DistanceTo(attacker);

        if (IsFarWeapon && !IsFarWeaponOf(attacker) && distance <= ShortWeapon)
        {
            Target = attacker;
            var steps = ReachTo(attacker) - distance;
            PlanPathToTarget(steps);
            _path.RemoveRange(Math.Min(steps, _path.Count), _path.Count - Math.Min(steps, _path.Count));
            Change(Mode.Forward);
            return;
        }

        if (attacker.Is(PlayerStatus.Flying) && IsCloseAttack || _mode is not (Mode.Idle or Mode.Escape) ||
            _mode == Mode.Escape && Random.Next(100) < Boldness)
        {
            return;
        }

        Target = attacker;

        if (Random.Next(100) < Boldness)
        {
            Change(Mode.Forward);
            Route(attacker.X, attacker.Y);

            if (_path.Count > 0)
            {
                return;
            }
        }

        Change(Mode.Escape);
        Route(attacker.X, attacker.Y, MonsterType.ViewRange);
    }

    private void Think(DateTimeOffset now)
    {
        for (var thought = 0; thought < MaxThoughts && !Process(now); thought++)
        {
        }
    }

    private bool Process(DateTimeOffset now) => _mode switch
    {
        Mode.Escape => Escape(now),
        Mode.Forward => Advance(now),
        Mode.Attack => Fight(now),
        _ => Idle(now)
    };

    private bool Idle(DateTimeOffset now)
    {
        if (!MonsterType.IsGuardian)
        {
            if (_path.Count > 0)
            {
                Walk(now, false);
                return true;
            }

            if (!(MonsterType.Is(Behavior.FastBack) && !AtHome) && !Thinks(now))
            {
                return true;
            }
        }

        if (FindTarget())
        {
            if (InRange(Target!))
            {
                Change(Mode.Attack);
                return false;
            }

            if (CanMove)
            {
                if (_path.Count > 0)
                {
                    Change(Mode.Forward);
                    return false;
                }

                if (Random.Next(100) < Boldness)
                {
                    return true;
                }

                Change(Mode.Escape);
                return false;
            }
        }

        if (MonsterType.IsGuardian)
        {
            if (_path.Count > 0)
            {
                Walk(now, false);
                return true;
            }

            if (!Thinks(now))
            {
                return true;
            }
        }

        if (!CanMove)
        {
            return true;
        }

        if (AtHome)
        {
            Wander(now);
        }
        else if (MonsterType.IsGuardian || MonsterType.Is(Behavior.FastBack) || Random.Next(WanderRate) == 0)
        {
            ReturnHome(now);
        }

        return true;
    }

    private bool Advance(DateTimeOffset now)
    {
        if (Target is { } target && IsValid(target) && InRange(target))
        {
            Change(Mode.Attack);
            return false;
        }

        if ((MonsterType.IsGuardian || MonsterType.Is(Behavior.FastBack)) && Generator is { } generator &&
            (generator.IsFar(X, Y, GuardLeave) || Target is { } fled && (fled.Map != Map || generator.IsFar(fled.X, fled.Y, GuardLeave))))
        {
            Target = null;
            FarJump(Home);
            Change(Mode.Idle);
            return true;
        }

        if (MonsterType.IsGuardian && Target is { } quarry && IsValid(quarry) && DistanceTo(quarry) >= GuardJump)
        {
            JumpTo(quarry.X, quarry.Y);
            return true;
        }

        if (_path.Count > 0)
        {
            Walk(now, Target is not null);
            return true;
        }

        if (!FindTarget())
        {
            Change(Mode.Idle);
            return true;
        }

        if (_path.Count > 0)
        {
            return false;
        }

        if (MonsterType.Is(Behavior.Jump))
        {
            JumpBlock((Target!.X, Target.Y));
            return true;
        }

        Change(Mode.Idle);
        return true;
    }

    private bool Fight(DateTimeOffset now)
    {
        if (Target is { } target && IsValid(target) && InRange(target))
        {
            if (Thinks(now) && Map!.IsStacked(this))
            {
                _ahead = false;
                FindDetour(null);
                _ahead = true;
                Walk(now, true);
            }

            if (now >= NextAttack)
            {
                Attack(target, now);
            }

            return true;
        }

        if (!FindTarget())
        {
            Change(Mode.Idle);
        }
        else if (!InRange(Target!))
        {
            Change(_path.Count > 0 && CanMove ? Mode.Forward : Mode.Idle);
        }

        return true;
    }

    private bool Escape(DateTimeOffset now)
    {
        if (MonsterType.IsGuardian && Target is { } target)
        {
            JumpTo(target.X, target.Y);
            Change(Mode.Forward);
            return true;
        }

        if (_path.Count == 0)
        {
            PlanPathToTarget(MonsterType.ViewRange * 2);
        }

        if (Target is null)
        {
            Change(Mode.Idle);
        }
        else if (_path.Count == 0)
        {
            Change(Mode.Forward);
        }
        else
        {
            Walk(now, true);
        }

        return true;
    }

    public bool Step(Direction direction, MsgWalk walk)
    {
        Direction = direction;
        return IsAlive && Map!.Move(this, (ushort)(X + direction.Step().X), (ushort)(Y + direction.Step().Y), walk);
    }

    public bool Leap(ushort x, ushort y, MsgAction jump) => IsAlive && (CanSee(x, y) || Owner!.CanSee(x, y)) && JumpTo(x, y, jump);

    private void PetAttack(DateTimeOffset now)
    {
        if (Target is { } prey && now >= NextAttack && !Attack(prey, now))
        {
            Target = null;
        }
    }

    private void ReviveNearbyDead(DateTimeOffset now)
    {
        if (Target is not Player { IsAlive: false } patient || patient.Map != Map || DistanceTo(patient) > MonsterType.ViewRange)
        {
            Target = Map!.Scan(this).OfType<Player>().Where(player => !player.IsAlive && DistanceTo(player) <= MonsterType.ViewRange)
                .OrderBy(DistanceTo).FirstOrDefault();
            _path.Clear();
        }

        if (Target is not { } dead)
        {
            return;
        }

        if (InRange(dead))
        {
            if (now >= NextAttack && Spell is { } revival)
            {
                Cast(revival, dead, dead.X, dead.Y, now);
            }
        }
        else
        {
            if (_path.Count == 0)
            {
                Route(dead.X, dead.Y);
            }

            Walk(now, false);
        }
    }

    private bool FindTarget()
    {
        Target = null;

        if (!MonsterType.Is(Behavior.Active))
        {
            return false;
        }

        Target = Nearest();

        if (Target is not null)
        {
            PlanPathToTarget();
        }

        return Target is not null;
    }

    private Fighter? Nearest()
    {
        var (found, nearest) = ((Fighter?)null, (int)MonsterType.ViewRange);

        foreach (var prey in Map!.Scan(this).OfType<Fighter>().Where(Hunts))
        {
            var distance = DistanceTo(prey);

            if (distance <= nearest && !prey.Is(PlayerStatus.Invisible) ||
                prey is Player && distance <= Math.Min(MonsterType.ViewRange, ReachTo(prey)) && Stats.Range + (SizeAdd + 1) / 2 > 1)
            {
                (found, nearest) = (prey, distance);
            }
        }

        return found;
    }

    private bool PlanPathToTarget(int escape = 0)
    {
        _ahead = escape == 0;
        _path.Clear();

        if (Target is not { } target || !IsValid(target) || DistanceTo(target) > MonsterType.ViewRange)
        {
            Target = null;
            return false;
        }

        if (!Route(target.X, target.Y, escape))
        {
            return false;
        }

        if (!Map!.CanStep(X, Y, _path[0], MonsterType.SizeAdd))
        {
            return FindDetour(_path[0]);
        }

        var (start, end) = (target.DistanceTo(this), target.DistanceTo(_goal.X, _goal.Y));

        if (_ahead ? end >= start : end <= start)
        {
            _path.Clear();
        }

        return _path.Count > 0;
    }

    private bool Route(int x, int y, int escape = 0)
    {
        _path.Clear();
        _path.AddRange(Pathfinder.Find(Map!, (X, Y), (x, y), escape));
        _goal = _path.Aggregate((X: (int)X, Y: (int)Y), (at, direction) => (at.X + direction.Step().X, at.Y + direction.Step().Y));
        return _path.Count > 0;
    }

    private bool FindDetour(Direction? excluded)
    {
        _path.Clear();
        var goal = Target is { } target ? (target.X, target.Y) : Home;
        var (best, found) = (DistanceTo(goal.X, goal.Y), (Direction?)null);
        var first = Random.Next(8);

        for (var i = first; i < first + 8; i++)
        {
            var direction = (Direction)(i % 8);

            if (direction == excluded || !Map!.CanStep(X, Y, direction, MonsterType.SizeAdd, ClimbCap))
            {
                continue;
            }

            var distance = Distance(X + direction.Step().X, Y + direction.Step().Y, goal.X, goal.Y);

            if ((best - distance) * (_ahead ? 1 : -1) > 0)
            {
                (best, found) = (distance, direction);
            }
        }

        if (found is { } step)
        {
            _path.Add(step);
        }

        return found is not null;
    }

    private void Wander(DateTimeOffset now)
    {
        if (!MonsterType.IsGuardian)
        {
            if (Random.Next(WanderRate) == 0 && (Direction)Random.Next(8) is var direction && Map!.CanStep(X, Y, direction, MonsterType.SizeAdd, ClimbCap))
            {
                _path.Add(direction);
                Walk(now, false);
            }

            return;
        }

        if (Generator is { } generator && (Random.Next(WanderRate) == 0 && generator.Width > 1 || generator.Height > 1) &&
            Route(generator.X + Random.Next(generator.Width), generator.Y + Random.Next(generator.Height)))
        {
            Walk(now, false);
        }
    }

    private void ReturnHome(DateTimeOffset now)
    {
        var home = Home;

        if (Route(home.X, home.Y))
        {
            Walk(now, false);
        }
        else if (MonsterType.IsGuardian || MonsterType.Is(Behavior.Jump))
        {
            JumpBlock(home);
        }
    }

    private bool Walk(DateTimeOffset now, bool running)
    {
        if (!CanMove)
        {
            _path.Clear();
            return false;
        }

        if (!Steps(now, running ? MonsterType.RunSpeed : MonsterType.MoveSpeed))
        {
            return true;
        }

        if (_path.Count == 0)
        {
            return false;
        }

        var direction = _path[0];
        _path.RemoveAt(0);

        if (Step(direction, running))
        {
            return true;
        }

        if (FindDetour(direction))
        {
            var detour = _path[0];
            _path.Clear();
            return Step(detour, running);
        }

        return MonsterType.Is(Behavior.Jump) && JumpBlock(Home);
    }

    private bool Step(Direction direction, bool running)
    {
        Direction = direction;

        return Map!.CanStep(X, Y, direction, MonsterType.SizeAdd) && Map.Move(this, (ushort)(X + direction.Step().X), (ushort)(Y + direction.Step().Y),
            new MsgWalk { Id = Id, Direction = direction, Mode = running ? WalkMode.Run : WalkMode.Walk });
    }

    private bool JumpBlock((int X, int Y) to)
    {
        var steps = DistanceTo(to.X, to.Y);

        for (var i = 1; i <= steps; i++)
        {
            var (x, y) = (X + (to.X - X) * i / steps, Y + (to.Y - Y) * i / steps);

            if (Map!.IsWalkable(x, y))
            {
                return JumpTo(x, y);
            }
        }

        return steps == 0;
    }

    private void FarJump((int X, int Y) to)
    {
        var steps = DistanceTo(to.X, to.Y);

        for (var i = 0; i <= steps; i++)
        {
            var (x, y) = steps == 0 ? to : (to.X + (X - to.X) * i / steps, to.Y + (Y - to.Y) * i / steps);

            if (Map!.IsWalkable(x, y))
            {
                JumpTo(x, y);
                return;
            }
        }
    }

    private bool JumpTo(int x, int y, MsgAction? jump = null)
    {
        _path.Clear();

        if (Map is not { } map || !map.IsWalkable(x, y))
        {
            return false;
        }

        jump ??= new MsgAction
        {
            Id = Id,
            X = (ushort)x,
            Y = (ushort)y,
            DataX = (ushort)x,
            DataY = (ushort)y,
            Direction = (ushort)Direction,
            Action = ActionType.Jump
        };

        if (CanSee(x, y))
        {
            return map.Move(this, (ushort)x, (ushort)y, jump);
        }

        map.Relocate(this, (ushort)x, (ushort)y, jump);
        return true;
    }

    private void Change(Mode mode)
    {
        if (mode != Mode.Forward)
        {
            _path.Clear();
        }

        _mode = mode;
    }

    private bool Thinks(DateTimeOffset now)
    {
        if (now < _thought + ThinkInterval)
        {
            return false;
        }

        _thought = now;
        return true;
    }

    private bool Steps(DateTimeOffset now, int milliseconds)
    {
        var interval = TimeSpan.FromMilliseconds(milliseconds);

        if (now < _stepped + interval)
        {
            return false;
        }

        _stepped = now >= _stepped + 2 * interval ? now : _stepped + interval;
        return true;
    }

    private bool IsValid(Fighter target) => target.Map == Map && (MonsterType.IsReviver ? !target.IsAlive : Hunts(target));

    private bool InRange(Fighter target) => DistanceTo(target) <= ReachTo(target);

    private static bool IsFarWeaponOf(Fighter attacker) =>
        attacker is Monster monster ? monster.IsFarWeapon : attacker.AttackKind == InteractAction.Shoot;
}
