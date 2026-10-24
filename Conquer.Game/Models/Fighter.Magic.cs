using System.Collections.Frozen;

namespace Conquer.Game.Models;

public abstract partial class Fighter
{
    protected const uint PassiveTarget = 8;
    protected const uint SelfOnlyTarget = 2;

    private const int TargetsPerEffect = (SizeTypePrefixedProtocol.MaximumMessageSize - 20) / 8;
    private const int FanReach = 2;
    private const double FanHalfWidth = 2 * Math.PI / 3;
    private const double BurstSlack = 0.9;
    private const int LineClimb = 26;
    private const byte SelfActivated = 1 | 4;

    private const int DashPercent = 80;
    private const int MaxBinding = 90;
    private const int SapPerLevel = 30;
    private const int Blocked = 8;
    private const uint NoKnockBack = 0x10000000;
    private const int DispatchedXp = 20;
    private const int TeleportTries = 10;

    private static readonly FrozenDictionary<uint, PlayerStatus> Buffs = new Dictionary<uint, PlayerStatus>
    {
        [MagicStatus.Accuracy] = PlayerStatus.Accuracy,
        [MagicStatus.MagicShield] = PlayerStatus.MagicShield,
        [MagicStatus.Shield] = PlayerStatus.MagicShield,
        [MagicStatus.Stigma] = PlayerStatus.Stigma,
        [MagicStatus.Superman] = PlayerStatus.Superman,
        [MagicStatus.Cyclone] = PlayerStatus.Cyclone,
        [MagicStatus.Invisible] = PlayerStatus.Invisible,
        [MagicStatus.Dodge] = PlayerStatus.Dodge,
        [MagicStatus.Poison] = PlayerStatus.Poison,
        [MagicStatus.Flying] = PlayerStatus.Flying,
        [MagicStatus.Intensify] = PlayerStatus.Intensify,
        [MagicStatus.CastingPray] = PlayerStatus.CastingPray
    }.ToFrozenDictionary();

    private static readonly PlayerStatus[] Unveiled =
        [PlayerStatus.Accuracy, PlayerStatus.MagicShield, PlayerStatus.Stigma, PlayerStatus.Dodge];

    public bool Cast(MagicType spell, Fighter? target, int x, int y, DateTimeOffset now, bool followUp = false) =>
        CanCast(spell, target, x, y) && PayCost(spell) && Perform(spell, target, x, y, now, followUp);

    protected bool CanCast(MagicType spell, Fighter? target, int x, int y) =>
        Map is not null && IsAlive && !(spell.Sort == MagicSort.RecallingTeam && Map.Info.Is(MapFlags.ChangeMapDisable)) && (spell.GroundTargeted != 0
            ? DistanceTo(x, y) <= spell.CastRange
            : target is not null && Reaches(spell, target));

    protected bool Reaches(MagicType spell, Fighter target) =>
        target.Map == Map && (target.IsAlive || spell.Sort == MagicSort.Reviving) && DistanceTo(target) <= spell.CastRange;

    private bool Succeeds(MagicType spell) =>
        (spell.AutoActive & SelfActivated) != 0 || spell.SuccessChance is 0 or >= 100 || Map!.World.Random.Next(100) < spell.SuccessChance;

    protected bool Perform(MagicType spell, Fighter? target, int x, int y, DateTimeOffset now, bool followUp = false)
    {
        if (Map is null || !IsAlive)
        {
            return false;
        }

        NextAttack = now.AddMilliseconds(Stats.AttackSpeed * EffectPower(PlayerStatus.Cyclone));

        if (spell.Sort == MagicSort.Dashing)
        {
            Dash(spell, now);
            return true;
        }

        if (spell.Sort == MagicSort.Bomb && !followUp)
        {
            (x, y) = spell.GroundTargeted != 0 || spell.Target == SelfOnlyTarget ? (X, Y) : target is null ? (x, y) : (target.X, target.Y);
        }

        if (!Succeeds(spell))
        {
            Map.Broadcast(this, Effect(spell, target, x, y, []));
            return true;
        }

        var strike = spell.WeaponSubtype == ItemType.Bow ? Strike.Bow
            : spell.WeaponHit != 0 || spell.WeaponSubtype != 0 || spell.Sort == MagicSort.Striking ? Strike.Melee
            : Strike.Magic;

        CurrentSpell = spell;
        List<(Fighter Victim, int Amount, int Reflected)> hits;

        try
        {
            hits = Targets(spell, target, x, y)
                .Concat(spell.Sort == MagicSort.Buffing ? ExtraBuffTargets(spell) : [])
                .Distinct()
                .Where(victim => Affects(spell, victim, now))
                .Select(victim => (Victim: victim, Amount: Amount(spell, strike, victim)))
                .Select(hit => spell.Damages && hit.Amount > 0 && !IgnoresReflection && hit.Victim.Reflection() is > 0 and var cap
                    ? (hit.Victim, Amount: 0, Reflected: Math.Min(hit.Amount, cap))
                    : (hit.Victim, hit.Amount, Reflected: 0))
                .ToList();
        }
        finally
        {
            CurrentSpell = null;
        }

        if (spell.Offensive != 0)
        {
            foreach (var hit in hits)
            {
                OnAttacking(hit.Victim, now);
            }
        }

        foreach (var batch in hits.Chunk(TargetsPerEffect).DefaultIfEmpty([]))
        {
            Map.Broadcast(this, Effect(spell, target, x, y, batch.Select(hit => (hit.Victim.Id, (uint)hit.Amount))));
        }

        var (practice, experience) = (0.0, 0.0);

        foreach (var hit in hits)
        {
            var gain = ApplySpell(spell, hit.Victim, hit.Amount, (x, y), now);
            practice += gain.Practice;
            experience += gain.Experience;
        }

        OnPracticed(spell, practice, experience);

        foreach (var hit in hits.Where(hit => hit.Reflected > 0 && IsAlive))
        {
            Reflect(hit.Victim, hit.Reflected, strike == Strike.Magic ? InteractAction.ReflectMagic : InteractAction.ReflectWeapon, now);
        }

        return true;
    }

    private MsgMagicEffect Effect(MagicType spell, Fighter? target, int x, int y, IEnumerable<(uint, uint)> hits)
    {
        var effect = new MsgMagicEffect
        {
            Sender = Id,
            MagicType = spell.Type,
            Level = spell.Level,
            Targets = [..hits]
        };

        if (target is not null && spell.GroundTargeted == 0 && !spell.IsArea)
        {
            effect.Target = target.Id;
        }
        else
        {
            (effect.X, effect.Y) = ((ushort)x, (ushort)y);
        }

        return effect;
    }

    private void Dash(MagicType spell, DateTimeOffset now)
    {
        var (dx, dy) = Direction.Step();
        var (x, y) = (X + dx, Y + dy);
        var victim = Map!.InRange(this).OfType<Fighter>().FirstOrDefault(other => other.X == x && other.Y == y);
        var struck = victim is not null && Affects(spell, victim, now) ? victim : null;
        var pushes = struck is not (null or DynaNpc) && Map.IsWalkable(x + dx, y + dy);
        var power = struck is null ? 0
            : pushes || struck is DynaNpc ? DamageTo(struck, Strike.Melee) * DashPercent / 100
            : DamageTo(struck, Strike.Melee, spell.Power);
        var heading = struck is not null && !pushes ? Blocked : (int)Direction;

        Map.Broadcast(this, new MsgInteract
        {
            Sender = Id,
            Target = victim?.Id ?? 0,
            X = (ushort)x,
            Y = (ushort)y,
            Action = InteractAction.Bump,
            Data = (uint)(heading << 24 | power) | (pushes ? 0 : NoKnockBack)
        });

        if (struck is null)
        {
            return;
        }

        if (pushes)
        {
            Map.Shift(this, (ushort)x, (ushort)y);
            Map.Shift(struck, (ushort)(x + dx), (ushort)(y + dy));
        }

        if (spell.Offensive != 0)
        {
            OnAttacking(struck, now);
        }

        if (power > 0 && struck.Reflection() is > 0 and var cap)
        {
            Reflect(struck, Math.Min(power, cap), InteractAction.ReflectMagic, now);
            return;
        }

        var (practice, experience) = ApplySpell(spell, struck, power, (x, y), now);
        OnPracticed(spell, practice, experience);
    }

    protected virtual bool PayCost(MagicType spell) => true;

    // The spell this fighter is resolving right now (null outside spell damage), so defenders can tell area damage apart.
    public MagicType? CurrentSpell { get; private set; }

    // How far an area spell reaches for this fighter, in percent of normal.
    protected virtual int AreaReachPercent(MagicType spell) => 100;

    protected virtual double FanHalfWidthFor(MagicType spell) => FanHalfWidth;

    protected virtual int AdjustSpellAmount(MagicType spell, Strike strike, Fighter victim, int amount) => amount;

    protected virtual IEnumerable<Fighter> ExtraBuffTargets(MagicType spell) => [];

    protected virtual double AdjustBuffPower(MagicType spell, double power) => power;

    protected virtual void OnPracticed(MagicType spell, double practice, double experience)
    {
    }

    private IEnumerable<Fighter> Targets(MagicType spell, Fighter? target, int x, int y)
    {
        var nearby = Map!.InRange(this).OfType<Fighter>();
        var (dx, dy) = target is null ? (x - X, y - Y) : (target.X - X, target.Y - Y);
        var reach = AreaReachPercent(spell);
        var halfWidth = FanHalfWidthFor(spell);

        return spell.Sort switch
        {
            MagicSort.DispatchingXp => Teammates().Where(member => member != this),
            MagicSort.Heal when spell.MultiTarget != 0 => Teammates(),
            _ when !spell.IsArea && spell.Target == SelfOnlyTarget => [this],
            _ when !spell.IsArea => target is null ? [] : [target],
            MagicSort.Fan => nearby.Where(other => DistanceTo(other) <= Math.Min(spell.Range + FanReach, ReachTo(other)) * reach / 100 && InFan(other, dx, dy, halfWidth)),
            MagicSort.Bomb => Map.Around(x, y, spell.Range * reach / 100).OfType<Fighter>().Where(other => other != this && InBurst(other, x, y, spell.Range * reach / 100)),
            MagicSort.Line => Line(dx, dy, spell.Range * reach / 100).Where(cell => Map.InBounds(cell.X, cell.Y) &&
                    Math.Abs(Map.Cell(cell.X, cell.Y).Altitude - Map.Cell(X, Y).Altitude) < LineClimb)
                .Select(cell => Map.Around(cell.X, cell.Y, 0).OfType<Fighter>().FirstOrDefault()).OfType<Fighter>(),
            _ => []
        };
    }

    private IEnumerable<Player> TeammatesToRecall() =>
        (this as Player)?.Team?.Members.Where(member => member != this).TakeWhile(member => member.IsAlive)
        .Where(member => member.Map == Map || !member.Map!.Info.Is(MapFlags.ChangeMapDisable)) ?? [];

    private IEnumerable<Fighter> Teammates() =>
        (this as Player)?.Team is { } team
            ? team.Members.Where(member => member.Map == Map && DistanceTo(member) <= Map.ViewRange)
            : [this];

    private bool InFan(Entity other, int dx, int dy, double halfWidth)
    {
        if (dx == 0 && dy == 0 || other.X == X && other.Y == Y)
        {
            return false;
        }

        var delta = Math.Abs(Math.Atan2(other.Y - Y, other.X - X) - Math.Atan2(dy, dx));
        return delta <= halfWidth || delta >= 2 * Math.PI - halfWidth;
    }

    private List<(int X, int Y)> Line(int dx, int dy, int range)
    {
        if (dx == 0 && dy == 0)
        {
            return [];
        }

        var scale = range / Math.Sqrt(dx * dx + dy * dy);
        var (ex, ey) = ((int)(0.5 + scale * dx + X) - X, (int)(0.5 + scale * dy + Y) - Y);
        var (width, height) = (Math.Abs(ex), Math.Abs(ey));

        return width > height
            ? [..Enumerable.Range(1, width).Select(step => (X + Math.Sign(ex) * step, Y + Along(ey, step, width)))]
            : [..Enumerable.Range(1, height).Select(step => (X + Along(ex, step, height), Y + Math.Sign(ey) * step))];
    }

    private static int Along(int offset, int step, int length) => (offset * 2 * step + length * (offset > 0 ? 1 : -1)) / (length * 2);

    private static bool InBurst(Entity other, int x, int y, int range) =>
        Math.Sqrt((x - other.X) * (x - other.X) + (y - other.Y) * (y - other.Y)) < range + BurstSlack;

    private bool Affects(MagicType spell, Fighter victim, DateTimeOffset now) =>
        victim.IsAlive != spell.Revives && !(spell.Revives && Map!.Info.Is(MapFlags.PkField)) &&
        (spell.Offensive == 0 || victim != this && CanAttack(victim) && !victim.IsShielded(now) && !IsImmune(spell, victim)) &&
        (spell.Offensive != 0 || victim is not DynaNpc) && victim.TakesSpellsFrom(this);

    private bool IsImmune(MagicType spell, Fighter victim) =>
        victim.Is(PlayerStatus.Flying) && !Is(PlayerStatus.Flying) && spell.WeaponHit != 0 && spell.WeaponSubtype != ItemType.Bow ||
        spell.IsArea && victim is Monster { MonsterType: var type } && type.Is(Behavior.Guard);

    private int Amount(MagicType spell, Strike strike, Fighter victim) => AdjustSpellAmount(spell, strike, victim, BaseAmount(spell, strike, victim));

    private int BaseAmount(MagicType spell, Strike strike, Fighter victim) => spell.Sort switch
    {
        MagicSort.Heal or MagicSort.AddMana => spell.Power,
        MagicSort.DispatchingXp => DispatchedXp,
        MagicSort.Sapping => Math.Min(victim.Health * (spell.Power - PowerIsPercent) / 100, Level * SapPerLevel),
        MagicSort.Striking when spell.Status == MagicStatus.Bind => Math.Min(DamageTo(victim, strike, spell.Power), victim.Stats.MaxHealth * MaxBinding / 100),
        MagicSort.Striking => DamageTo(victim, strike, spell.Status == MagicStatus.WeaponDamage ? spell.Power : 0, powersAttack: true),
        _ when spell.Damages => DamageTo(victim, strike, spell.Power),
        _ => 0
    };

    private (double Practice, double Experience) ApplySpell(MagicType spell, Fighter victim, int amount, (int X, int Y) cell, DateTimeOffset now)
    {
        switch (spell.Sort)
        {
            case MagicSort.Heal:
                return (victim.AddHealth(amount), 0);

            case MagicSort.AddMana:
                return ((victim as Player)?.AddMana(amount) ?? 0, 0);

            case MagicSort.DispatchingXp when victim is Player member && this is Player roarer:
                member.AddXp(amount);
                member.Tell($"{roarer.Name} roared, and the teammates' XP boosts immediately!", TalkChannel.System);
                return default;

            case MagicSort.Buffing:
                return (ApplyStatus(spell, victim, now) ? 1 : 0, 0);

            case MagicSort.Reviving when spell.Status == MagicStatus.Revive:
                (victim as Player)?.Revive(now, inPlace: true);
                return default;

            case MagicSort.Reviving when spell.Status == MagicStatus.Purge:
                foreach (var status in Buffs.Values.Distinct())
                {
                    victim.ClearEffect(status);
                }

                return default;

            case MagicSort.Reviving:
                victim.ClearEffect(PlayerStatus.Poison);
                return default;

            case MagicSort.RecallingTeam:
                foreach (var member in TeammatesToRecall())
                {
                    member.Teleport(MapId, X, Y);
                }

                return default;

            case MagicSort.Teleporting when victim is Player jumper:
                RandomTeleport(jumper);
                return default;

            case MagicSort.Transforming or MagicSort.CallingPet when Map!.World.Data.MonsterTypes.TryGetValue((uint)spell.Power, out var type) && victim is Player player:
                var until = spell.DurationSeconds == 0 ? DateTimeOffset.MaxValue : now.AddSeconds(spell.DurationSeconds);

                if (spell.Sort == MagicSort.Transforming)
                {
                    player.Transform(type, until);
                }
                else
                {
                    player.SummonPet(type, until, cell.X, cell.Y);
                }

                return (1, 0);
        }

        if (amount == 0 || !spell.Damages)
        {
            if (spell.Offensive != 0)
            {
                victim.OnAttacked(this);
            }

            return default;
        }

        var dealt = Math.Min(amount, victim.Health);
        var experience = victim.GivesExperience ? dealt * ExperienceGap(victim) : 0;
        var survived = !DealDamage(victim, amount, now, blow: false);

        if (survived && spell.Status == MagicStatus.Stun)
        {
            victim.Stun(TimeSpan.FromMilliseconds(spell.Power));
        }
        else if (survived && spell.DurationSeconds > 0)
        {
            ApplyStatus(spell, victim, now);
        }

        return spell.Sort switch
        {
            MagicSort.Striking => (dealt > 0 ? 1 : 0, experience),
            MagicSort.Sapping => default,
            _ => (experience, experience)
        };
    }

    private void RandomTeleport(Player jumper)
    {
        var (width, height) = ((int)Map!.DMap.Width, (int)Map.DMap.Height);

        for (var tries = 0; tries < TeleportTries; tries++)
        {
            var (x, y) = (jumper.X + width / 2 - Map.World.Random.Next(width), jumper.Y + height / 2 - Map.World.Random.Next(height));

            if (Map.IsWalkable(x, y))
            {
                jumper.Teleport(MapId, (ushort)x, (ushort)y);
                return;
            }
        }

        jumper.Tell("Magic failed!", TalkChannel.System);
    }

    private bool ApplyStatus(MagicType spell, Fighter victim, DateTimeOffset now)
    {
        if (!Buffs.TryGetValue(spell.Status, out var status) || status == PlayerStatus.Invisible && victim is not Player)
        {
            return false;
        }

        if (status == PlayerStatus.Invisible)
        {
            foreach (var unveiled in Unveiled)
            {
                victim.ClearEffect(unveiled);
            }
        }

        victim.ApplyEffect(status, AdjustBuffPower(spell, spell.Power > PowerIsPercent ? (spell.Power - PowerIsPercent) / 100.0 : 1),
            now.AddSeconds(spell.DurationSeconds * Math.Max(1u, spell.ActiveTimes)));
        return true;
    }
}
