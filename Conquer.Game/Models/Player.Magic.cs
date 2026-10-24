namespace Conquer.Game.Models;

public partial class Player
{
    private const byte Full = 100;
    private const uint Sitting = 250;
    private const int WeaponSpellBase = 10000;
    private const double TrainingExperience = 0.1;
    private const int TrainingTrickleChance = 20;
    private const int CastWearChance = 10;
    private const byte Certain = 100;
    private const int StandingStamina = 3;
    private const int SittingStamina = 10;
    private const int SittingHeal = 6;
    private const byte XpLevel = 3;
    private const int XpPerTick = 10;

    private static readonly TimeSpan StaminaInterval = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan SittingEnergyInterval = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan HealInterval = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan XpInterval = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan MagicDelay = TimeSpan.FromMilliseconds(800);
    private static readonly TimeSpan ProtectionTime = TimeSpan.FromSeconds(10);

    private static readonly ItemPosition[] CastWorn =
    [
        ItemPosition.Head, ItemPosition.Necklace, ItemPosition.Armor, ItemPosition.Right, ItemPosition.Ring, ItemPosition.Bottle,
        ItemPosition.Garment
    ];

    private DateTimeOffset _energized;
    private DateTimeOffset _nextHeal;
    private DateTimeOffset _nextXp;
    private DateTimeOffset _formUntil;
    private DateTimeOffset _launched;
    private TimeSpan _delay = MagicDelay;
    private bool _autoCasting;
    private DateTimeOffset _protectedUntil;
    private PendingCast? _casting;
    private PendingCast? _autoRecast;

    public byte Stamina { get; private set; } = Full;
    public byte Xp { get; private set; }
    public uint Pose { get; set; }
    public MonsterType? Form { get; private set; }
    public override int SizeAdd => Form?.SizeAdd ?? 0;
    public Monster? Pet { get; private set; }

    public override bool IsShielded(DateTimeOffset now) =>
        now < _protectedUntil || Pet is { IsAlive: true } pet && pet.Map is not null && pet.Map == Map && pet.IsStandIn;

    public void Interrupt()
    {
        Halt();
        AbortCast();
    }

    private void Halt()
    {
        Target = null;
        Pose = 0;
        _mining = false;
        _autoRecast = null;
    }

    public void Protect(DateTimeOffset now) => _protectedUntil = now + ProtectionTime;

    private void EndProtection() => _protectedUntil = default;

    public bool StartCast(MagicType spell, Fighter? target, int x, int y, DateTimeOffset now)
    {
        Halt();
        EndProtection();
        _casting = null;

        if (!Ready(spell, now) || !CanCast(spell, target, x, y) || !Allows(spell, target) || !PayCost(spell))
        {
            Send(Act(ActionType.AbortMagic));
            return false;
        }

        Sparkle();

        if (spell.ManaCost > 0 && MapId != TrainingGround)
        {
            WearCastingGear();
        }

        _autoCasting = !(target is Player && spell.FollowUp == spell.Type);
        var aimed = spell.GroundTargeted == 0 ? target : null;
        var chant = new PendingCast(spell, target?.Id ?? 0, aimed?.X ?? x, aimed?.Y ?? y, now.AddMilliseconds(spell.CastTime));

        if (spell.CastTime > 0)
        {
            _casting = chant;
        }
        else
        {
            FinishCast(chant, now);
        }

        return true;
    }

    private bool Ready(MagicType spell, DateTimeOffset now) =>
        spell.Sort == MagicSort.Dashing || now >= _launched + (spell.FollowUpDelay > 0 ? DelayOf(spell) : _delay);

    private static TimeSpan DelayOf(MagicType spell) =>
        spell.FollowUpDelay > 0 ? TimeSpan.FromMilliseconds(spell.FollowUpDelay) : MagicDelay;

    private bool Allows(MagicType spell, Fighter? target) =>
        ((spell.Target & SelfOnlyTarget) == 0 || target == this) &&
        (!Map!.Info.Is(MapFlags.BoothEnable) || spell is { Sort: MagicSort.Buffing, Status: MagicStatus.CastingPray }) &&
        !(spell.Sort == MagicSort.CallingPet && (Map.Info.Is(MapFlags.BoothEnable) || MapId == TrainingGround)) &&
        !(spell.Sort == MagicSort.Transforming && Is(PlayerStatus.Flying)) &&
        !(spell is { Sort: MagicSort.Buffing, Status: MagicStatus.Flying } && Map.Info.Is(MapFlags.WingDisable)) &&
        !(spell.Sort == MagicSort.Buffing && target is not null && target.Is(PlayerStatus.MagicShield));

    private void WearCastingGear()
    {
        foreach (var position in CastWorn)
        {
            if (ItemAt(position) is { Durability: > 0 } item && World.Random.Next(CastWearChance) == 0)
            {
                Consume(item, 1);
            }
        }
    }

    public void AbortCast()
    {
        _autoRecast = null;

        if (_casting is null)
        {
            return;
        }

        _casting = null;
        Send(Act(ActionType.AbortMagic));
    }

    private void TickCast(DateTimeOffset now)
    {
        if (_casting is { } intoning && now >= intoning.Due)
        {
            _casting = null;
            FinishCast(intoning, now);
        }
        else if (_casting is null && _autoRecast is { } repeating && now >= repeating.Due)
        {
            _autoRecast = null;
            StartCast(repeating.Spell, repeating.Target == 0 ? null : Map?.Find<Fighter>(repeating.Target), repeating.X, repeating.Y, now);
        }
    }

    private void FinishCast(PendingCast chant, DateTimeOffset now)
    {
        var spell = chant.Spell;
        var target = chant.Target == 0 ? null : Map?.Find<Fighter>(chant.Target);
        var launched = spell.GroundTargeted != 0
            ? Perform(spell, target, chant.X, chant.Y, now)
            : target is not null && Reaches(spell, target) && Perform(spell, target, target.X, target.Y, now);

        (_launched, _delay) = (now, DelayOf(spell));

        if (launched && target is not null && spell is { Offensive: not 0, GroundTargeted: 0 } && !IsWeaponSpell(spell))
        {
            ProcWeaponSpell(target, now);
        }

        if (_autoCasting && Follows(spell) is { } next)
        {
            _autoRecast = chant with { Spell = next, Due = now + _delay };
        }
    }

    private MagicType? Follows(MagicType spell) => MapId == TrainingGround
        ? spell is { XpSkill: not 1, AutoActive: 0, Sort: not MagicSort.Dashing } ? Known(spell.Type) ?? spell : null
        : spell.FollowUp == 0 ? null
        : spell.FollowUp == spell.Type ? Known(spell.Type) ?? spell
        : Known(spell.FollowUp);

    private MagicType? Known(ushort type) =>
        World.Data.MagicTypes.GetValueOrDefault((type, KnownMagic(type)?.Level ?? 0));

    private static bool IsWeaponSpell(MagicType spell) => spell.Type is >= WeaponSpellBase and <= WeaponSpellBase + byte.MaxValue;

    private void ProcWeaponSpell(Fighter target, DateTimeOffset now)
    {
        if (ItemAt(ItemPosition.Right) is { Magic1: > 0 } weapon && ItemAt(ItemPosition.Left) is not { Magic1: > 0 } &&
            WeaponSpell(weapon) is { } spell)
        {
            TryProc(spell, target, now);
        }
    }

    private MagicType? WeaponSpell(Item item) => World.Data.MagicTypes.GetValueOrDefault(((ushort)(item.Magic1 + WeaponSpellBase), (ushort)0));

    private sealed record PendingCast(MagicType Spell, uint Target, int X, int Y, DateTimeOffset Due);

    public void Transform(MonsterType? form, DateTimeOffset until = default)
    {
        if (form == Form)
        {
            _formUntil = until;
            return;
        }

        var (health, max) = (Health, Math.Max(1, Stats.MaxHealth));
        (Form, _formUntil) = (form, until);
        RecalculateStats();
        Health = health == 0 ? 0 : Math.Max(1, (int)Math.Round((double)health * Stats.MaxHealth / max));

        OnHealthChanged();
        Show(AttributeType.Look, Look);
    }

    public bool Transform(ushort magicType, ushort level, uint monsterTypeId, int seconds)
    {
        if (!World.Data.MonsterTypes.TryGetValue(monsterTypeId, out var form))
        {
            return false;
        }

        Map?.Broadcast(this, new MsgMagicEffect
        {
            Sender = Id,
            Target = Id,
            MagicType = magicType,
            Level = level,
            Targets = [(Id, 0)]
        });
        Transform(form, World.UtcNow.AddSeconds(seconds));
        return true;
    }

    public void DismissPet()
    {
        if (Pet is { Map: { } map } pet)
        {
            var seen = map == Map && pet.CanSee(this);
            map.Remove(pet);

            if (!seen)
            {
                Send(pet.Despawn);
            }
        }

        Pet = null;
    }

    public void SummonPet(MonsterType type, DateTimeOffset until, int x, int y)
    {
        DismissPet();
        Pet = new(type, owner: this) { Id = World.PetIds.Next(), Expires = until };
        (x, y) = Map!.IsWalkable(x, y) && CanSee(x, y) ? (x, y) : (X, Y);
        Send(new MsgPetInfo(Pet, (ushort)x, (ushort)y));
        Map.Add(Pet, (ushort)x, (ushort)y);
    }

    public void Refill()
    {
        AddHealth(Stats.MaxHealth);
        AddMana(Stats.MaxMana);
        Stamina = Full;
        Update(AttributeType.Stamina, Stamina);
        SetXp(Full);
    }

    public int AddMana(int mana)
    {
        var before = Mana;
        Mana = (ushort)Math.Clamp(Mana + mana, 0, Stats.MaxMana);
        Update(AttributeType.Mana, Mana);
        return Mana - before;
    }

    protected override bool PayCost(MagicType spell)
    {
        var free = MapId == TrainingGround;
        var arrows = !free && spell.WeaponSubtype == ItemType.Bow && spell.Offensive != 0 ? spell.ArrowCost : 0;
        var (mana, stamina) = free ? (0, 0) : (LegendManaCost(spell), RingStaminaCost(spell));
        var life = 0;

        if (mana > 0 && SpellsCostLife)
        {
            (life, mana) = (mana, 0);
        }

        if (Form is not null && spell.Sort != MagicSort.Transforming || Mana < mana || Stamina < stamina || life > 0 && Health <= life ||
            spell.XpSkill == 1 && !Is(PlayerStatus.XpFull) || spell.WeaponSubtype != 0 && !Wields(spell.WeaponSubtype) || Arrows < arrows)
        {
            return false;
        }

        if (arrows > 0)
        {
            Consume(ItemAt(ItemPosition.Left)!, arrows);
        }

        AddMana(-mana);
        AddHealth(-life);
        SetStamina(Stamina - stamina);

        if (spell.XpSkill == 1)
        {
            SetXp(0);
        }

        return true;
    }

    protected override void OnPracticed(MagicType spell, double practice, double experience)
    {
        if (MapId == TrainingGround)
        {
            practice = practice == 1 && spell.Sort is MagicSort.Buffing or MagicSort.Striking or MagicSort.Transforming ? (World.Random.Next(TrainingTrickleChance) == 0 ? 1 : 0)
                : practice > 0 ? TrainingShare(practice)
                : 0;
            experience = experience > 0 ? TrainingShare(experience) : 0;
        }

        if (practice > 0 && KnownMagic(spell.Type) is { } magic)
        {
            Practice(magic, practice);
        }

        if (experience > 0 && spell.Offensive != 0)
        {
            GainExperience(experience, bonused: true);
        }
    }

    private double TrainingShare(double amount) => Math.Max(1, amount / (Shoots ? 2 : 1) * TrainingExperience);

    private bool Shoots => Hands.Any(type => type.IsBow);

    protected override bool TryPassiveSpell(Fighter target, DateTimeOffset now)
    {
        if (!target.TakesSpellsFrom(this))
        {
            return false;
        }

        var skills = KnownMagics.Select(magic => World.Data.MagicTypes.GetValueOrDefault((magic.Type, magic.Level)))
            .Where(spell => spell is not null && (spell.Target & PassiveTarget) != 0 &&
                            (spell.WeaponSubtype == 0 || Wields(spell.WeaponSubtype))).ToList();

        var enchantments = Held.Where(item => item.Magic1 > 0)
            .Select(item => (Item: item, Spell: WeaponSpell(item)))
            .Where(enchantment => enchantment.Spell is not null);

        if (skills.Any(skill => TryProc(skill!, target, now)))
        {
            return true;
        }

        foreach (var (item, spell) in enchantments)
        {
            if (!TryProc(spell!, target, now))
            {
                continue;
            }

            if (TypeOf(item).IsArrow && MapId != TrainingGround)
            {
                Consume(item, 1);
            }

            return true;
        }

        return false;
    }

    private bool TryProc(MagicType skill, Fighter target, DateTimeOffset now)
    {
        var aim = skill.Sort is MagicSort.Heal or MagicSort.Buffing or MagicSort.AddMana ? this : skill.AimsAtTarget ? target : null;

        if (World.Random.Next(100) >= skill.SuccessChance || !Cast(skill, aim, target.X, target.Y, now))
        {
            return false;
        }

        if (skill.SuccessChance >= Certain)
        {
            FollowUp(skill, target);
        }

        return true;
    }

    private void FollowUp(MagicType spell, Fighter target)
    {
        if (spell.FollowUp == 0 || !World.Data.MagicTypes.TryGetValue((spell.FollowUp, 0), out var next))
        {
            return;
        }

        World.After(TimeSpan.FromMilliseconds(spell.FollowUpDelay), () =>
        {
            if (Target == target && target.IsAlive &&
                Cast(next, next.AimsAtTarget ? target : null, target.X, target.Y, World.UtcNow, followUp: true))
            {
                FollowUp(next, target);
            }
        });
    }

    private bool Wields(ushort weaponSkill) => Hands.Any(type => type.WeaponSkill == weaponSkill || type.WeaponSkill == ItemType.Backsword && weaponSkill == Sword);

    private void Regenerate(DateTimeOffset now)
    {
        var sitting = Pose == Sitting;

        if (IsAlive && Form is null && !Is(PlayerStatus.Flying) && Stamina < Full &&
            now - _energized >= (sitting ? SittingEnergyInterval : StaminaInterval))
        {
            _energized = now;
            SetStamina(Stamina + (sitting ? SittingStamina : StandingStamina) * StaminaRegenPercent / 100);
        }

        if (now >= _nextHeal)
        {
            _nextHeal = now + HealInterval;

            if (sitting && IsAlive && Health < Stats.MaxHealth)
            {
                AddHealth(SittingHeal);
            }
        }

        if (now >= _nextXp)
        {
            if (_nextXp != default)
            {
                ChargeXp();
            }

            _nextXp = now + XpInterval;
        }
    }

    private void ChargeXp()
    {
        if (Level < XpLevel || !IsAlive)
        {
            return;
        }

        if (Is(PlayerStatus.XpFull))
        {
            SetXp(0);
        }
        else
        {
            AddXp(XpPerTick);
        }
    }

    public void SetXp(byte xp)
    {
        Xp = Math.Min(xp, Full);

        if (Xp < Full)
        {
            ClearEffect(PlayerStatus.XpFull);
        }
        else if (!Is(PlayerStatus.XpFull))
        {
            ApplyEffect(PlayerStatus.XpFull, 1, DateTimeOffset.MaxValue);
        }

        if (Xp == 0)
        {
            _nextXp = default;
        }

        Update(AttributeType.Xp, Xp);
    }

    public void AddXp(int amount)
    {
        if (!Is(PlayerStatus.XpFull))
        {
            SetXp((byte)Math.Min(Xp + amount, Full));
        }
    }

    public void AddStamina(int amount) => SetStamina(Stamina + amount);

    private void SetStamina(int energy)
    {
        if (Math.Clamp(energy, 0, Full) is var clamped && clamped != Stamina)
        {
            Stamina = (byte)clamped;
            Update(AttributeType.Stamina, Stamina);
        }
    }
}
