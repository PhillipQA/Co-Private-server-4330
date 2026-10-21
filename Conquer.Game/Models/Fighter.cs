namespace Conquer.Game.Models;

public abstract partial class Fighter : Entity
{
    private const ushort PoisonEffect = 10010;
    private const int MaxPoisonDamage = 200;

    private static readonly TimeSpan PoisonInterval = TimeSpan.FromSeconds(2);

    private readonly Dictionary<PlayerStatus, (DateTimeOffset Until, double Power)> _effects = [];

    private DateTimeOffset _nextPoison;
    private DateTimeOffset _nextAttack;
    private TimeSpan _stun;

    public int Health { get; set; }
    public byte Level { get; set; }
    public Direction Direction { get; set; }
    public PlayerStatus Status { get; set; }

    public Stats Stats { get; protected set; }
    public Fighter? Target { get; set; }
    public virtual InteractAction AttackKind => InteractAction.Attack;
    public DateTimeOffset NextAttack
    {
        get => _nextAttack + _stun;
        set => (_nextAttack, _stun) = (value, TimeSpan.Zero);
    }

    public int Kills { get; protected set; }
    public bool IsAlive => Health > 0;
    public virtual bool GivesExperience => false;
    public virtual int SizeAdd => 0;

    public int ReachTo(Fighter target) => Stats.Range + (SizeAdd + target.SizeAdd + 1) / 2;

    public void Stun(TimeSpan delay) => _stun = delay;

    public virtual void Tick(DateTimeOffset now)
    {
        if (_effects.Count > 0)
        {
            foreach (var status in _effects.Where(effect => now >= effect.Value.Until).Select(effect => effect.Key).ToList())
            {
                ClearEffect(status);
            }
        }

        if (IsAlive && Is(PlayerStatus.Poison) && now >= _nextPoison)
        {
            _nextPoison = now + PoisonInterval;
            var damage = Math.Min(MaxPoisonDamage, (int)Math.Round(Health * EffectPower(PlayerStatus.Poison), MidpointRounding.AwayFromZero));
            var lost = -AddHealth(-damage);
            Map?.Broadcast(this, new MsgMagicEffect
            {
                Sender = Id,
                Target = Id,
                MagicType = PoisonEffect,
                Targets = [(Id, (uint)lost)]
            });

            if (!IsAlive)
            {
                DieInstantly(null, now);
            }
        }
    }

    public bool Is(PlayerStatus status) => (Status & status) != 0;

    public double EffectPower(PlayerStatus status) => _effects.TryGetValue(status, out var effect) ? effect.Power : 1;

    public void ApplyEffect(PlayerStatus status, double power, DateTimeOffset until)
    {
        _effects[status] = (until, power);
        Flag(status);
        OnEffectApplied(status);
    }

    protected virtual void OnEffectApplied(PlayerStatus status)
    {
    }

    public void ExtendEffect(PlayerStatus status, TimeSpan by, DateTimeOffset limit)
    {
        if (_effects.TryGetValue(status, out var effect))
        {
            _effects[status] = (effect.Until + by < limit ? effect.Until + by : limit, effect.Power);
        }
    }

    public void ClearEffect(PlayerStatus status)
    {
        if (_effects.Remove(status))
        {
            Flag(status, false);
        }
    }

    public void Flag(PlayerStatus status, bool on = true)
    {
        Status = on ? Status | status : Status & ~status;
        Target = on && status.HasFlag(PlayerStatus.Freeze) ? null : Target;
        Show(AttributeType.Flags, (uint)Status);
    }

    public void Show(AttributeType type, uint value) => Map?.Broadcast(this, new MsgUserAttrib(Id, type, value));

    public int AddHealth(int amount)
    {
        var before = Health;
        Health = (int)Math.Clamp((long)Health + amount, 0, Stats.MaxHealth);
        OnHealthChanged();
        return Health - before;
    }

    public abstract void Die(Fighter? killer, DateTimeOffset now);

    protected virtual void OnHealthChanged()
    {
    }

    protected virtual void OnDamaged(bool blow)
    {
    }

    protected internal virtual void OnAttacked(Fighter attacker)
    {
    }

    protected internal virtual void OnDamageDealt(Fighter target, int damage, DateTimeOffset now, bool blow)
    {
    }

    protected void ClearEffects()
    {
        foreach (var status in _effects.Keys.ToList())
        {
            ClearEffect(status);
        }
    }
}
