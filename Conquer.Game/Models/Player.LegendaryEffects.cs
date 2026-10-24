namespace Conquer.Game.Models;

// Legendary gear: every kind of item has its own Legendary perk (modelled on the Classic Conquer system).
//   Weapon            - hits make the target bleed for extra damage over a few seconds
//   Bag (Taoist)      - bigger spell areas, half mana cost, triple healing
//   Necklace          - more max HP, potions heal twice as much
//   Bracelet (Taoist) - magic penetration, more magic damage the more max mana you have
//   Ring              - lifesteal, cheaper stamina, melee passive skills reach 3x as far
//   Boots             - ignore one player attack every 10 seconds
//   Shield            - less magic damage and much less area damage taken
//   Taoist robe       - extra mana; while above 30% mana, half the damage you take drains mana instead
//   Warrior armor     - standing still builds up physical damage reduction and fast regeneration
//   Archer coat       - fan skills hit a wider arc (all around while flying), extra HP per Agility
//   Trojan armor      - spells cost HP instead of mana, killing a player refunds stamina
//   Earring           - magic damage reduction that grows while you avoid magic attacks
//   Dress             - more experience
//   Warrior helmet    - absorbs part of the damage nearby teammates take
//   Archer hat        - stronger Scatter, more damage to poles for the whole team
//   Taoist cap        - stronger buffs that also land on your team, reflect immunity, Tornado true damage
//   Trojan coronet    - faster stamina regeneration for you and your team, stronger FastBlade/ScentSword
// Players wearing Legendary gear also give off a purple aura now and then.
public partial class Player
{
    public enum LegendKind
    {
        None,
        Weapon,
        Bag,
        Necklace,
        Bracelet,
        Ring,
        Boots,
        Shield,
        TaoistRobe,
        WarriorArmor,
        ArcherCoat,
        TrojanArmor,
        Earring,
        Dress,
        WarriorHelmet,
        ArcherHat,
        TaoistCap,
        TrojanCoronet
    }

    // Weapon
    private const int BleedPercent = 30;
    private const int BleedTicks = 3;
    private const ushort BleedEffectType = 10010;

    // Bag
    private const int BagAreaPercent = 130;
    private const int BagManaPercent = 50;
    private const int BagHealPercent = 300;

    // Necklace
    private const int NecklaceHealthPercent = 15;
    private const int NecklacePotionPercent = 200;

    // Bracelet
    private const int BraceletPenetration = 10;
    private const int BraceletManaPerPercent = 400;

    // Ring
    private const int DrainPerMillePlayers = 50;
    private const int DrainPerMilleMonsters = 5;
    private const int MaxDrain = 2000;
    private const int DrainEffectChance = 10;
    private const int RingStaminaPercent = 75;
    private const int RingPassiveReachPercent = 300;

    // Boots
    private static readonly TimeSpan BootsCooldown = TimeSpan.FromSeconds(10);

    // Shield
    private const int ShieldMagicPercent = 67;
    private const int ShieldAreaPercent = 50;

    // Taoist robe
    private const int RobeMana = 3000;
    private const int RobeManaFloorPercent = 30;

    // Warrior armor
    private static readonly TimeSpan StillAfter = TimeSpan.FromSeconds(2.5);
    private static readonly TimeSpan StillMaxAfter = TimeSpan.FromSeconds(6.5);
    private const int StillMinReduction = 15;
    private const int StillMaxReduction = 45;
    private const int StillRegenPercent = 5;
    private const int StillStaminaPercent = 3;

    // Archer coat
    private const int CoatHealthPerAgility = 25;
    private const double CoatFanHalfWidth = Math.PI * 3 / 4;

    // Trojan armor
    private const int TrojanStaminaRefundPercent = 75;

    // Earring
    private const int EarringBaseReduction = 20;
    private const int EarringPerStack = 5;
    private const int EarringMaxStacks = 6;
    private static readonly TimeSpan EarringStackEvery = TimeSpan.FromSeconds(5);

    // Dress
    private const double DressExperience = 1.5;

    // Warrior helmet
    private const int HelmetAbsorbPercent = 30;
    private const int HelmetRange = 12;

    // Archer hat
    private const int HatScatterPercent = 135;
    private const int HatPoleArcherPercent = 125;
    private const int HatPoleOtherPercent = 110;
    private const ushort Scatter = 8001;

    // Taoist cap
    private const double CapBuffPower = 1.5;
    private const int CapTornadoPercent = 3;
    private const ushort Tornado = 1002;

    // Trojan coronet
    private const int CoronetStaminaPercent = 160;
    private const int CoronetTeamStaminaPercent = 130;
    private const int CoronetSkillPercent = 110;
    private const ushort FastBlade = 1045;
    private const ushort ScentSword = 1046;

    private const string StrikeEffect = "goldendragon";
    private const string DrainEffect = "rainbow";
    private const string GuardEffect = "BodyShield";
    private const string AuraEffect = "purpleray";

    private static readonly TimeSpan AuraInterval = TimeSpan.FromSeconds(10);
    private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

    private readonly Dictionary<uint, DateTimeOffset> _bleeding = [];

    private DateTimeOffset _nextAura;
    private DateTimeOffset _nextLegendSecond;
    private DateTimeOffset _bootsReady;
    private DateTimeOffset _lastMoved;
    private DateTimeOffset _nextEarringStack;
    private int _earringStacks;
    private bool _legendaryStriking;
    private bool _legendaryDebug;

    public static LegendKind KindOf(ItemType type) => type.Subtype switch
    {
        111 => LegendKind.WarriorHelmet,
        113 => LegendKind.ArcherHat,
        114 => LegendKind.TaoistCap,
        118 => LegendKind.TrojanCoronet,
        112 => type.RequiredProfession switch
        {
            15 => LegendKind.TrojanCoronet,
            25 => LegendKind.WarriorHelmet,
            45 => LegendKind.ArcherHat,
            >= 100 => LegendKind.TaoistCap,
            _ => LegendKind.None
        },
        117 => LegendKind.Earring,
        120 => LegendKind.Necklace,
        121 => LegendKind.Bag,
        130 or 135 => LegendKind.TrojanArmor,
        131 or 136 => LegendKind.WarriorArmor,
        132 or 137 => LegendKind.Dress,
        133 or 138 => LegendKind.ArcherCoat,
        134 or 139 => LegendKind.TaoistRobe,
        150 or 151 => LegendKind.Ring,
        152 => LegendKind.Bracelet,
        160 => LegendKind.Boots,
        900 => LegendKind.Shield,
        _ when type.IsOneHanded || type.IsTwoHanded => LegendKind.Weapon,
        _ => LegendKind.None
    };

    private static bool IsWorking(Item item) => item.MaxDurability == 0 || item.Durability > 0;

    private bool Has(LegendKind kind) =>
        Equipment.Any(item => item.Legendary && IsWorking(item) && KindOf(TypeOf(item)) == kind);

    // "/legend debug" makes every Legendary perk report in chat when it triggers, for testing.
    public bool ToggleLegendaryDebug() => _legendaryDebug = !_legendaryDebug;

    private void LegendLog(string text)
    {
        if (_legendaryDebug)
        {
            Tell($"[Legendary] {text}", TalkChannel.System);
        }
    }

    private void MarkMoved() => _lastMoved = World.UtcNow;

    private IEnumerable<Player> NearbyTeammates(int range) =>
        Team?.Members.Where(member => member != this && member.IsAlive && member.Map == Map && member.DistanceTo(this) <= range) ?? [];

    // ---- Stats -------------------------------------------------------------------------------------------------

    private Stats WithLegendaryStats(Stats stats)
    {
        var health = stats.MaxHealth;
        var mana = stats.MaxMana;

        if (Has(LegendKind.Necklace))
        {
            health += health * NecklaceHealthPercent / 100;
        }

        if (Has(LegendKind.ArcherCoat))
        {
            health += Agility * CoatHealthPerAgility;
        }

        if (Has(LegendKind.TaoistRobe))
        {
            mana += RobeMana;
        }

        return WithAffixStats(stats with { MaxHealth = health, MaxMana = mana });
    }

    private double LegendaryExperience => (Has(LegendKind.Dress) ? DressExperience : 1) * AffixExperience;

    private int PotionPercent => Has(LegendKind.Necklace) ? NecklacePotionPercent : 100;

    private int StaminaRegenPercent =>
        Has(LegendKind.TrojanCoronet) ? CoronetStaminaPercent
        : NearbyTeammates(HelmetRange).Any(member => member.Has(LegendKind.TrojanCoronet)) ? CoronetTeamStaminaPercent
        : 100;

    // ---- Spell costs -------------------------------------------------------------------------------------------

    private int LegendManaCost(MagicType spell) => Has(LegendKind.Bag) ? spell.ManaCost * BagManaPercent / 100 : spell.ManaCost;

    private int RingStaminaCost(MagicType spell) =>
        Has(LegendKind.Ring) ? spell.StaminaCost * RingStaminaPercent / 100 : spell.StaminaCost;

    private bool SpellsCostLife => Has(LegendKind.TrojanArmor);

    // ---- Outgoing damage ---------------------------------------------------------------------------------------

    protected override bool IgnoresReflection => Has(LegendKind.TaoistCap);

    protected override int MagicPenetration => Has(LegendKind.Bracelet) ? BraceletPenetration : 0;

    protected override int AreaReachPercent(MagicType spell)
    {
        if (!spell.IsArea)
        {
            return 100;
        }

        var passive = (spell.Target & PassiveTarget) != 0;
        var percent = passive && Has(LegendKind.Ring) ? RingPassiveReachPercent : 100;
        return !passive && Has(LegendKind.Bag) ? percent * BagAreaPercent / 100 : percent;
    }

    protected override double FanHalfWidthFor(MagicType spell) =>
        spell.Sort == MagicSort.Fan && Has(LegendKind.ArcherCoat) ? Is(PlayerStatus.Flying) ? Math.PI : CoatFanHalfWidth
        : base.FanHalfWidthFor(spell);

    protected override int AdjustAttackDamage(Fighter target, int damage) => CriticalHit(PoleBonus(target, damage));

    protected override int AdjustSpellAmount(MagicType spell, Strike strike, Fighter victim, int amount)
    {
        if (spell.Sort == MagicSort.Heal)
        {
            return Has(LegendKind.Bag) ? amount * BagHealPercent / 100 : amount;
        }

        if (!spell.Damages || amount <= 0)
        {
            return amount;
        }

        if (spell.Type == Scatter && Has(LegendKind.ArcherHat))
        {
            amount = amount * HatScatterPercent / 100;
        }

        if (spell.Type is FastBlade or ScentSword && Has(LegendKind.TrojanCoronet))
        {
            amount = amount * CoronetSkillPercent / 100;
        }

        if (strike == Strike.Magic && Has(LegendKind.Bracelet))
        {
            amount += amount * (Stats.MaxMana / BraceletManaPerPercent) / 100;
        }

        if (spell.Type == Tornado && Has(LegendKind.TaoistCap))
        {
            amount += Math.Max(1, victim.Stats.MaxHealth * CapTornadoPercent / 100);
        }

        return CriticalHit(PoleBonus(victim, amount));
    }

    private int PoleBonus(Fighter target, int damage)
    {
        if (target is not DynaNpc { IsPole: true })
        {
            return damage;
        }

        var hatWorn = Has(LegendKind.ArcherHat) || NearbyTeammates(Map.ViewRange).Any(member => member.Has(LegendKind.ArcherHat));

        if (!hatWorn)
        {
            return damage;
        }

        var archer = Profession is >= Profession.InternArcher and <= Profession.ArcherMaster;
        return damage * (archer ? HatPoleArcherPercent : HatPoleOtherPercent) / 100;
    }

    protected override IEnumerable<Fighter> ExtraBuffTargets(MagicType spell) =>
        spell.XpSkill == 0 && spell.Status != MagicStatus.Flying && Has(LegendKind.TaoistCap) ? NearbyTeammates(Map.ViewRange) : [];

    protected override double AdjustBuffPower(MagicType spell, double power) =>
        Has(LegendKind.TaoistCap) && power != 1 ? 1 + (power - 1) * CapBuffPower : power;

    // Called after this player damages something, with a weapon or a spell.
    private void OnLegendaryHit(Fighter target, int damage, bool blow)
    {
        if (damage <= 0 || _legendaryStriking || Map is not { } map)
        {
            return;
        }

        if (Has(LegendKind.Ring) && IsAlive && Health < Stats.MaxHealth)
        {
            var perMille = target is Player ? DrainPerMillePlayers : DrainPerMilleMonsters;
            var drained = (int)Math.Min(MaxDrain, (long)damage * perMille / 1000);

            if (drained > 0)
            {
                AddHealth(drained);
                LegendLog($"Ring lifesteal healed {drained} HP.");

                if (Roll(100) < DrainEffectChance)
                {
                    ShowEffect(DrainEffect);
                }
            }
        }

        if (blow && target.IsAlive && Has(LegendKind.Weapon))
        {
            Bleed(target, map, damage * BleedPercent / 100);
        }
    }

    private void Bleed(Fighter target, Map map, int total)
    {
        var now = World.UtcNow;

        if (total < BleedTicks || _bleeding.TryGetValue(target.Id, out var until) && now < until)
        {
            return;
        }

        foreach (var expired in _bleeding.Where(bleed => now >= bleed.Value).Select(bleed => bleed.Key).ToList())
        {
            _bleeding.Remove(expired);
        }

        _bleeding[target.Id] = now + OneSecond * BleedTicks;
        LegendLog($"{(target is Player victim ? victim.Name : "The target")} is bleeding for {total} damage.");

        for (var tick = 1; tick <= BleedTicks; tick++)
        {
            World.After(OneSecond * tick, () =>
            {
                if (!IsAlive || !target.IsAlive || target.Map != map || Map != map || !CanAttack(target))
                {
                    return;
                }

                var damage = total / BleedTicks;
                map.Broadcast(target, new MsgMagicEffect
                {
                    Sender = Id,
                    Target = target.Id,
                    MagicType = BleedEffectType,
                    Targets = [(target.Id, (uint)damage)]
                });

                _legendaryStriking = true;

                try
                {
                    DealDamage(target, damage, World.UtcNow, blow: false);
                }
                finally
                {
                    _legendaryStriking = false;
                }
            });
        }
    }

    // ---- Incoming damage ---------------------------------------------------------------------------------------

    protected override int AdjustIncomingDamage(Fighter attacker, Strike strike, int damage)
    {
        var now = World.UtcNow;
        var assailant = attacker is Monster { Owner: { } master } ? master : attacker;
        var magic = strike == Strike.Magic;
        var area = attacker.CurrentSpell?.IsArea == true;

        if (assailant is Player && assailant != this && Has(LegendKind.Boots) && now >= _bootsReady)
        {
            _bootsReady = now + BootsCooldown;
            ShowEffect(GuardEffect);
            LegendLog($"Boots ignored an attack ({damage} damage).");
            return 0;
        }

        if (Has(LegendKind.Shield))
        {
            damage = magic ? damage * ShieldMagicPercent / 100 : damage;
            damage = area ? damage * ShieldAreaPercent / 100 : damage;
        }

        if (magic && Has(LegendKind.Earring))
        {
            damage -= damage * (EarringBaseReduction + EarringPerStack * _earringStacks) / 100;
            _earringStacks = Math.Max(0, _earringStacks - 1);
        }

        if (!magic && Has(LegendKind.WarriorArmor) && StillReduction(now) is > 0 and var reduction)
        {
            damage -= damage * reduction / 100;
        }

        if (Has(LegendKind.TaoistRobe) && Mana * 100 > Stats.MaxMana * RobeManaFloorPercent)
        {
            var floor = Stats.MaxMana * RobeManaFloorPercent / 100;
            var mitigated = damage / 2;
            damage -= mitigated;
            AddMana(-Math.Min(damage / 2, Mana - floor));
        }

        if (NearbyTeammates(HelmetRange).FirstOrDefault(member => member.Has(LegendKind.WarriorHelmet)) is { } guardian)
        {
            var absorbed = damage * HelmetAbsorbPercent / 100;
            damage -= absorbed;
            guardian.AddHealth(-Math.Min(absorbed / 2, guardian.Health - 1));
            guardian.LegendLog($"Your helmet absorbed {absorbed} damage for {Name}.");
        }

        return Math.Max(0, AffixDamageReduction(damage));
    }

    private int StillReduction(DateTimeOffset now)
    {
        var still = now - _lastMoved;

        if (still < StillAfter)
        {
            return 0;
        }

        var progress = Math.Min(1, (still - StillAfter) / (StillMaxAfter - StillAfter));
        return StillMinReduction + (int)((StillMaxReduction - StillMinReduction) * progress);
    }

    private void OnLegendaryPlayerKill()
    {
        if (Has(LegendKind.TrojanArmor) && Stamina < Full)
        {
            SetStamina(Stamina + (Full - Stamina) * TrojanStaminaRefundPercent / 100);
            LegendLog("Trojan armor refunded stamina.");
        }
    }

    // ---- Every tick --------------------------------------------------------------------------------------------

    private void TickLegendaryAura(DateTimeOffset now)
    {
        if (!IsAlive || Map is null)
        {
            return;
        }

        if (now >= _nextAura)
        {
            _nextAura = now + AuraInterval;

            if (Equipment.Any(item => item.Legendary && IsWorking(item)))
            {
                ShowEffect(AuraEffect);
            }
        }

        if (now < _nextLegendSecond)
        {
            return;
        }

        _nextLegendSecond = now + OneSecond;

        if (Has(LegendKind.Earring) && now >= _nextEarringStack)
        {
            _nextEarringStack = now + EarringStackEvery;
            _earringStacks = Math.Min(EarringMaxStacks, _earringStacks + 1);
        }

        if (Has(LegendKind.WarriorArmor) && now - _lastMoved >= StillAfter)
        {
            AddHealth(Stats.MaxHealth * StillRegenPercent / 100);
            AddMana(Stats.MaxMana * StillRegenPercent / 100);
            SetStamina(Stamina + Full * StillStaminaPercent / 100);
        }
    }

    // ---- Descriptions ------------------------------------------------------------------------------------------

    public string DescribeLegendary(Item item)
    {
        var type = TypeOf(item);
        var perk = KindOf(type) switch
        {
            LegendKind.Weapon => $"Hits make the target bleed for {BleedPercent}% extra damage over {BleedTicks} seconds",
            LegendKind.Bag => $"+{BagAreaPercent - 100}% spell area, {100 - BagManaPercent}% less mana cost, +{BagHealPercent - 100}% healing",
            LegendKind.Necklace => $"+{NecklaceHealthPercent}% max HP, HP/MP potions restore {NecklacePotionPercent / 100}x as much",
            LegendKind.Bracelet => $"{BraceletPenetration}% magic penetration, +1% magic damage per {BraceletManaPerPercent} max mana",
            LegendKind.Ring => $"Heal {DrainPerMillePlayers / 10.0:0.#}% of damage dealt ({DrainPerMilleMonsters / 10.0:0.#}% vs monsters, " +
                               $"max {MaxDrain:N0}), {100 - RingStaminaPercent}% less stamina cost, {RingPassiveReachPercent / 100}x passive skill reach",
            LegendKind.Boots => $"Ignore one player attack every {BootsCooldown.TotalSeconds:0} seconds",
            LegendKind.Shield => $"{100 - ShieldMagicPercent}% less magic damage, {100 - ShieldAreaPercent}% less area damage taken",
            LegendKind.TaoistRobe => $"+{RobeMana:N0} mana. Above {RobeManaFloorPercent}% mana, half the damage you take is taken from mana",
            LegendKind.WarriorArmor => $"After {StillAfter.TotalSeconds:0.#}s standing still: {StillMinReduction}-{StillMaxReduction}% less physical " +
                                       $"damage, regenerate {StillRegenPercent}% HP/MP and {StillStaminaPercent}% stamina per second",
            LegendKind.ArcherCoat => $"Fan skills hit a 270 degree arc (all around while flying), +{CoatHealthPerAgility} HP per Agility",
            LegendKind.TrojanArmor => $"Spells cost HP instead of mana, killing a player restores {TrojanStaminaRefundPercent}% of used stamina",
            LegendKind.Earring => $"{EarringBaseReduction}% less magic damage, +{EarringPerStack}% more every {EarringStackEvery.TotalSeconds:0}s " +
                                  $"(up to {EarringMaxStacks}); each magic hit removes one",
            LegendKind.Dress => $"+{(DressExperience - 1) * 100:0}% experience",
            LegendKind.WarriorHelmet => $"Absorb {HelmetAbsorbPercent}% of the damage nearby teammates take (you take half of it)",
            LegendKind.ArcherHat => $"+{HatScatterPercent - 100}% Scatter damage, team pole damage +{HatPoleArcherPercent - 100}% for archers, " +
                                    $"+{HatPoleOtherPercent - 100}% for others",
            LegendKind.TaoistCap => $"Buffs +{(CapBuffPower - 1) * 100:0}% stronger and also land on your team, immune to reflection, " +
                                    $"Tornado deals +{CapTornadoPercent}% of the target's max HP",
            LegendKind.TrojanCoronet => $"+{CoronetStaminaPercent - 100}% stamina regeneration (+{CoronetTeamStaminaPercent - 100}% for your team), " +
                                        $"+{CoronetSkillPercent - 100}% FastBlade and ScentSword damage",
            _ => "No Legendary perk for this kind of item"
        };

        var affixes = AffixesOf(item).ToList();
        var grade = LegendAffix.GradeOf(item) switch
        {
            AffixGrade.Godly => " [GODLY]",
            AffixGrade.Perfect => " [Perfect]",
            _ => ""
        };

        return affixes.Count == 0
            ? $"[Legendary] {type.Name}: {perk}"
            : $"[Legendary{grade}] {type.Name}: {perk} | {string.Join(", ", affixes)}";
    }

    public void ShowLegendaryGear(Player viewer)
    {
        var legendary = LegendaryGear();

        if (legendary.Count == 0)
        {
            viewer.Tell(viewer == this ? "You are not wearing any Legendary gear." : $"{Name} is not wearing any Legendary gear.",
                TalkChannel.System);
            return;
        }

        viewer.Tell(viewer == this ? "Your Legendary gear:" : $"{Name}'s Legendary gear:", TalkChannel.System);

        foreach (var item in legendary)
        {
            viewer.Tell(DescribeLegendary(item), TalkChannel.System);
        }
    }
}
