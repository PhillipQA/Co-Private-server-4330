namespace Conquer.Game.Models;

public partial class Monster : Fighter
{
    private const uint SimpleMagic = 1020;
    private const uint SelfHealing = 1005;

    private static readonly TimeSpan FadeTime = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan CorpseTime = TimeSpan.FromSeconds(10);

    private DateTimeOffset _died;
    private bool _released;

    public Monster(MonsterType type, Generator? generator = null, Player? owner = null)
    {
        MonsterType = type;
        Generator = generator;
        Owner = owner;
        Health = type.MaxHealth;
        Level = type.Level;
        Stats = Stats.Of(type);
    }

    public override IMessage Spawn => new MsgPlayer(this);
    public MonsterType MonsterType { get; }
    public Generator? Generator { get; }
    public Player? Owner { get; }
    public DateTimeOffset Expires { get; init; } = DateTimeOffset.MaxValue;
    public override bool GivesExperience => !MonsterType.IsGuardian;
    public override int SizeAdd => MonsterType.SizeAdd;
    public bool IsStandIn => Passive?.Sort == MagicSort.StandIn;

    private World World => Map!.World;
    private MagicType? Spell => World.Data.MagicTypes.GetValueOrDefault(((ushort)MonsterType.MagicType, (ushort)0));
    private MagicType? Passive => MonsterType.MagicHitRate == 0 ? Spell : null;

    private bool IsRighteous => MonsterType.Is(Behavior.Righteous);
    private bool IsCloseAttack => MonsterType.MagicType is 0 or SimpleMagic;
    private bool IsCaster => MonsterType.MagicType is not (0 or SelfHealing) && MonsterType.MagicHitRate != 0;

    public void Vanish()
    {
        Release();
        Leave();
    }

    public override void Tick(DateTimeOffset now)
    {
        base.Tick(now);

        if (!IsAlive)
        {
            Decay(now);
            return;
        }

        if (now >= Expires || Owner is { } owner && (!owner.IsAlive || owner.Map != Map))
        {
            Vanish();
            return;
        }

        if (Map is null || Is(PlayerStatus.Freeze))
        {
            return;
        }

        if (Owner is not null)
        {
            PetAttack(now);
        }
        else if (Dozes(now))
        {
            return;
        }
        else if (MonsterType.IsReviver)
        {
            ReviveNearbyDead(now);
        }
        else
        {
            Think(now);
        }
    }

    public override bool CanAttack(Fighter target) => Hunts(target);

    public override int HitChance(Fighter target) => IsCaster ? (int)MonsterType.MagicHitRate : base.HitChance(target);

    protected override Strike StrikeKind => IsCaster ? Strike.Magic : base.StrikeKind;

    protected override IMessage AttackMessage(Fighter target, int damage) => IsCaster
        ? new MsgMagicEffect
        {
            Sender = Id,
            Target = target.Id,
            MagicType = (ushort)MonsterType.MagicType,
            Targets = [(target.Id, (uint)damage)]
        }
        : base.AttackMessage(target, damage);

    protected internal override void OnAttacking(Fighter target, DateTimeOffset now) => Owner?.OnAttacking(target, now);

    protected internal override void OnDamageDealt(Fighter target, int damage, DateTimeOffset now, bool blow)
    {
        if (Owner is not { } owner)
        {
            return;
        }

        owner.OnDamageDealt(target, damage, now, false);

        if (Passive is { Sort: MagicSort.Vampire } vampire)
        {
            owner.AddHealth(damage * (vampire.Power - PowerIsPercent) / 100);
        }

        if (target.GivesExperience)
        {
            owner.GainExperience(damage * ExperienceGap(target));
        }
    }

    public override void Die(Fighter? killer, DateTimeOffset now)
    {
        if (IsStandIn && Owner is { IsAlive: true } owner)
        {
            owner.DieInstantly(null, now);
        }

        ClearEffects();
        _died = now;
        Target = null;
        _path.Clear();
        Release();
        Flag(PlayerStatus.Die | PlayerStatus.Freeze);

        if (Owner is not null || MonsterType.IsReviver)
        {
            return;
        }

        var looter = killer as Player ?? (killer as Monster)?.Owner;
        Drop(killer, looter, now);

        if (looter is not null)
        {
            World.Scripts.OnKill(looter, this);
        }
    }

    private void Decay(DateTimeOffset now)
    {
        if (now >= _died + CorpseTime)
        {
            Leave();
        }
        else if (now >= _died + FadeTime && !Is(PlayerStatus.FadeOut))
        {
            Flag(PlayerStatus.FadeOut);
        }
    }

    private void Release()
    {
        if (!_released)
        {
            _released = true;
            Generator?.Release();
        }
    }

    private void Leave()
    {
        if (Owner?.Pet == this)
        {
            Owner.DismissPet();
        }
        else
        {
            Map?.Remove(this);
        }
    }

    private bool Hunts(Fighter prey)
    {
        if (!prey.IsAlive || prey == this || prey.IsShielded(World.UtcNow) || prey.Is(PlayerStatus.Flying) && IsCloseAttack)
        {
            return false;
        }

        if (Owner is { } owner)
        {
            return prey != owner && owner.CanAttack(prey);
        }

        return prey switch
        {
            Player player => !MonsterType.IsReviver &&
                             (!IsRighteous && !MonsterType.Is(Behavior.PkKiller) ||
                              MonsterType.Is(Behavior.Guard) && player.Is(PlayerStatus.BlueName) ||
                              MonsterType.Is(Behavior.PkKiller) && player.Is(PlayerStatus.BlackName)),
            Monster other => !MonsterType.IsReviver && other.IsRighteous != IsRighteous,
            _ => false
        };
    }
}
