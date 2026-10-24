namespace Conquer.Game.Models;

public enum Strike
{
    Melee,
    Bow,
    Magic
}

public abstract partial class Fighter
{
    protected const int PowerIsPercent = 30000;
    private const int MinHitChance = 10;
    private const int MaxHitChance = 90;
    private const int ShieldedFromArrows = 12;
    private const int PlayerArrowDodge = 50;
    private const int MonsterArrowDodge = 25;
    private const int MaxMagicDefense = 90;
    private const int DragonFury = 210;
    private const int PhoenixFury = 180;
    private const byte LevelCap = 120;
    private const int UnfundedPoleDamage = 10;

    public virtual bool IsShielded(DateTimeOffset now) => false;

    public virtual bool TakesBlowsFrom(Fighter attacker) => true;

    public virtual bool TakesSpellsFrom(Fighter caster) => true;

    public bool Attack(Fighter target, DateTimeOffset now)
    {
        if (Map is null || target == this || !IsAlive || !target.IsAlive || target.Map != Map || target.IsShielded(now) ||
            DistanceTo(target) > ReachTo(target) || !CanAttack(target) || !target.TakesBlowsFrom(this))
        {
            return false;
        }

        OnAttacking(target, now);

        if (TryPassiveSpell(target, now))
        {
            return IsAlive && target.IsAlive;
        }

        if (target.Is(PlayerStatus.Flying) && StrikeKind == Strike.Melee)
        {
            return false;
        }

        var interval = TimeSpan.FromMilliseconds(Stats.AttackSpeed * EffectPower(PlayerStatus.Cyclone));
        NextAttack = (now - NextAttack < interval ? NextAttack : now) + interval;

        if (Map.World.Random.Next(100) >= HitChance(target))
        {
            Map.Broadcast(this, AttackMessage(target, 0));
            target.OnAttacked(this);
            return true;
        }

        var damage = AdjustAttackDamage(target, DamageTo(target, StrikeKind, normal: true));

        if (!IgnoresReflection && target.Reflection() is > 0 and var cap)
        {
            var action = StrikeKind == Strike.Magic ? InteractAction.ReflectMagic : InteractAction.ReflectWeapon;
            Reflect(target, Math.Min(damage, cap), action, now);
            return IsAlive;
        }

        Map.Broadcast(this, AttackMessage(target, damage));
        DealDamage(target, damage, now, blow: StrikeKind != Strike.Magic);
        return target.IsAlive;
    }

    protected virtual Strike StrikeKind => AttackKind == InteractAction.Shoot ? Strike.Bow : Strike.Melee;

    protected virtual IMessage AttackMessage(Fighter target, int damage) => Interact(target, AttackKind, damage);

    protected static bool IsMonsterId(Fighter target) => target.Id is >= World.FirstMonsterId and <= World.LastMonsterId;

    protected virtual int Reflection() => 0;

    protected void Reflect(Fighter reflector, int damage, InteractAction action, DateTimeOffset now)
    {
        var reflected = Math.Min(damage, Health);
        Map!.Broadcast(reflector, reflector.Interact(this, action, reflected));
        AddHealth(-reflected);
        OnDamaged(action == InteractAction.ReflectWeapon);

        if (!IsAlive)
        {
            Map!.Broadcast(this, reflector.Interact(this, InteractAction.Kill, 1));
            DieReflected(reflector, now);
        }
    }

    protected virtual void DieReflected(Fighter reflector, DateTimeOffset now) => Die(null, now);

    public virtual int HitChance(Fighter target)
    {
        var chance = (int)((100 + Stats.Agility / 2 - target.Evasion) * (100 + Stats.AgilityBonus) / 100 * EffectPower(PlayerStatus.Accuracy));
        var shielded = AttackKind == InteractAction.Shoot && target is Player bearer && bearer.Worn(ItemPosition.Left) is { IsShield: true };
        chance = Math.Clamp(shielded ? chance * ShieldedFromArrows / 100 : chance, MinHitChance, MaxHitChance);

        return this is not Player || target is not Monster ? chance
            : Is(PlayerStatus.Superman) ? 100
            : chance + Math.Max(0, Level - target.Level) / 5 * 10;
    }

    private int Evasion => (int)(Stats.Dodge * EffectPower(PlayerStatus.Dodge));

    public virtual bool CanAttack(Fighter target) => true;

    protected internal virtual void OnAttacking(Fighter target, DateTimeOffset now)
    {
    }

    protected virtual bool TryPassiveSpell(Fighter target, DateTimeOffset now) => false;

    protected double ExperienceGap(Fighter target) => (Math.Min(Level, LevelCap) - target.Level) switch
    {
        > 20 => 0.05,
        > 10 => 0.1,
        > 5 => 0.2,
        > 2 => 0.7,
        >= 0 => 1,
        >= -5 => 1.3,
        >= -10 => 1.5,
        >= -20 => 1.8,
        _ => 2.3
    };

    protected bool DealDamage(Fighter target, int damage, DateTimeOffset now, bool blow = true)
    {
        var dealt = -target.AddHealth(-damage);
        target.OnDamaged(blow);
        OnDamageDealt(target, dealt, now, blow);
        target.OnAttacked(this);

        if (target.IsAlive)
        {
            return false;
        }

        Map?.Broadcast(target, Interact(target, InteractAction.Kill, Kills > 0 && IsMonsterId(target) ? KoTally : 1));
        target.Die(this, now);
        return true;
    }

    protected int KoTally => 0xFFFF * (Kills + 1);

    public void DieInstantly(Fighter? killer, DateTimeOffset now)
    {
        AddHealth(-Health);
        Map?.Broadcast(this, (killer ?? this).Interact(this, InteractAction.Kill, 1));
        Die(killer, now);
    }

    protected int DamageTo(Fighter target, Strike strike, int power = 0, bool powersAttack = false, bool normal = false) => target.MitigateDamage(this, strike,
        strike == Strike.Magic ? MagicDamageTo(target, power) : WeaponDamageTo(target, strike == Strike.Bow, power, powersAttack, normal));

    private int WeaponDamageTo(Fighter target, bool bow, int power, bool powersAttack, bool normal)
    {
        var attack = Map!.World.Random.Next(Stats.MinAttack, Stats.MaxAttack);
        attack += attack * Stats.DragonGems / 100;
        attack += attack * Mastery / 100;
        attack = (int)(attack * DamageGap(target) * (target is Monster ? EffectPower(PlayerStatus.Superman) : 1) * EffectPower(PlayerStatus.Stigma));
        attack = powersAttack ? Math.Max(0, Adjust(attack, power)) : attack;

        var armor = target.Stats.Defense;
        var defense = (int)(armor * target.EffectPower(PlayerStatus.MagicShield)) / (Is(PlayerStatus.Superman) ? 2 : 1);
        var damage = bow
            ? Math.Max(0, attack - attack * Math.Max(0, target.Evasion - (target is Player ? PlayerArrowDodge : MonsterArrowDodge)) / 100)
            : Math.Max(0, attack - defense);

        damage = LuckyStrike(target) ? damage * 2 : damage;
        damage = powersAttack ? damage : Math.Max(0, Adjust(damage, power));

        if (defense <= armor)
        {
            damage = Math.Max(damage, Math.Max(1, Stats.Strength / (target.Stats.ArmorQuality switch { 6 => 100, 7 => 200, 8 => 500, 9 => 1000, _ => 10 })));
        }

        if (bow && target is Player)
        {
            damage = damage * (target.Level switch { < 22 => 35, < 50 => 25, < 90 => 20, < 110 => 15, _ => 12 }) / 100;
        }

        return normal && Stats.DragonGems >= DragonFury ? damage + damage / 2 : damage;
    }

    private int MagicDamageTo(Fighter target, int power)
    {
        var damage = this is Player ? Adjust(Stats.MagicAttack, power) : Stats.MagicAttack;
        damage = target is Player ? damage - damage / 4 : (int)(damage * DamageGap(target));
        damage -= damage * Math.Max(0, Math.Min(target.Stats.MagicDefense, MaxMagicDefense) - MagicPenetration) / 100;
        damage += damage * Stats.PhoenixGems / 100 + Stats.MagicBonus;
        damage += Stats.PhoenixGems >= PhoenixFury ? damage / 20 : 0;
        damage = LuckyStrike(target) ? damage * 2 : damage;
        return Math.Max(0, damage - target.Stats.MagicBlock);
    }

    private int MitigateDamage(Fighter attacker, Strike strike, int damage)
    {
        damage -= damage / 2 * Math.Min(Stats.TortoiseGems, 100) / 100;
        damage *= attacker is Player && this is DynaNpc { IsPole: true } && Map?.War?.Owner is { Fund: 0 } ? UnfundedPoleDamage : 1;
        damage = LuckyEscape() ? 0 : damage * DamageTakenPercent(attacker) / 100;
        return AdjustIncomingDamage(attacker, strike, Math.Max(1, damage - damage * Math.Min(Stats.Bless, 100) / 100));
    }

    // Hooks for equipment that changes combat (Legendary gear). The defaults change nothing.
    protected virtual bool IgnoresReflection => false;

    protected virtual int MagicPenetration => 0;

    protected virtual int AdjustAttackDamage(Fighter target, int damage) => damage;

    protected virtual int AdjustIncomingDamage(Fighter attacker, Strike strike, int damage) => damage;

    public virtual int DamageTakenPercent(Fighter attacker) => 100;

    protected virtual int Mastery => 0;

    protected virtual bool LuckyStrike(Fighter target) => false;

    protected virtual bool LuckyEscape() => false;

    private static int Adjust(int value, int power) => power > PowerIsPercent ? value * (power - PowerIsPercent) / 100 : value + power;

    private double DamageGap(Fighter target) => (this, target) switch
    {
        (Player, Monster) => (Math.Min(Level, LevelCap) - target.Level) switch { > 20 => 3, > 10 => 2.5, > 5 => 2, > 2 => 1.5, _ => 1 },
        (Monster, Player) => (target.Level - Math.Min(Level, LevelCap)) switch { < -20 => 5, < -10 => 3.5, < -5 => 2, < 0 => 1.5, _ => 1 },
        _ => 1
    };

    private MsgInteract Interact(Fighter target, InteractAction action, int data)
    {
        var at = action == InteractAction.Kill ? target : this;
        return new() { Sender = Id, Target = target.Id, X = at.X, Y = at.Y, Action = action, Data = (uint)data };
    }
}
