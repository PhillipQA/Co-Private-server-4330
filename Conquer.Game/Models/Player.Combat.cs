namespace Conquer.Game.Models;

public partial class Player
{
    private const PlayerStatus KeptOnDeath = PlayerStatus.RedName | PlayerStatus.BlackName | PlayerStatus.TeamLeader;
    private const PlayerStatus Frenzy = PlayerStatus.Superman | PlayerStatus.Cyclone;
    private const uint Standby = 100;
    private const double KillBonus = 0.05;
    private const byte MaxLevel = 130;
    private const int HalvedExperienceLevel = 120;
    private const int AutoAllotLevel = 120;
    private const int PointsPerLevel = 3;
    private const int ShareRange = 32;
    private const int NewbieGap = 20;
    private const int LevelShare = 30;
    private const int SpouseLevelShare = 36;
    private const int WaterSharePercent = 250;
    private const int GuideLevel = 70;
    private const int CoachLevel = 40;
    private const int GuideGap = 20;
    private const int VirtueRate = 30;
    private const double StepVirtueShare = 0.9;
    private const double LevelVirtueShare = 0.1;
    private const int MasterSkillLevel = 12;
    private const double GoalSkillRate = 0.2;

    private static readonly TimeSpan ReviveDelay = TimeSpan.FromSeconds(20);
    private static readonly TimeSpan FrenzyPerKill = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MaxFrenzy = TimeSpan.FromSeconds(120);

    private static readonly (byte Level, int Chance, int Cap)[] Reflections = [(100, 20, 1700), (70, 15, 1200), (40, 10, 700), (15, 5, 300)];

    private DateTimeOffset _died;
    private DateTimeOffset _frenzySince;

    public override void Tick(DateTimeOffset now)
    {
        base.Tick(now);

        if (Kills > 0 && !Is(Frenzy))
        {
            RecordKills(Kills);
            Kills = 0;
        }

        Regenerate(now);
        DecayPkPoints(now);
        Elapse(now);
        TickCast(now);

        if (Form is not null && now >= _formUntil)
        {
            Transform(null);
        }

        if (_mining)
        {
            Mine(now);
        }

        TickHunt(now);

        if (Target is not null && now >= NextAttack && !((AttackKind != InteractAction.Shoot || Arrows > 0) && Attack(Target, now)))
        {
            Target = null;
        }
    }

    private void RecordKills(int kills)
    {
        if (IsGm || kills < KoBoard.MinKills || kills <= KoRecord)
        {
            return;
        }

        KoRecord = kills;

        if (World.KoBoard.Enter(Id, Name, kills) is not { } placing)
        {
            return;
        }

        var news = $"{Name} has killed {kills} monsters with XP skills and ranks {placing.Place} on KO board.";

        if (placing.Promoted)
        {
            World.Announce(news, TalkChannel.Normal);
        }
        else
        {
            Tell(news, TalkChannel.Normal);
        }

        foreach (var (id, rank) in placing.Changes)
        {
            World.Players.GetValueOrDefault(id)?.Send(new MsgUserAttrib(id, AttributeType.TimeAdd, rank));
        }
    }

    public bool Revive(DateTimeOffset now, bool inPlace = false)
    {
        if (IsAlive || !inPlace && now - _died < ReviveDelay || Map is null)
        {
            return false;
        }

        AddHealth(Stats.MaxHealth);
        AddMana(Stats.MaxMana);
        Flag(PlayerStatus.Die | PlayerStatus.Freeze, false);
        _ghost = false;
        Show(AttributeType.Look, Look);
        SetXp(0);
        SetStamina(Full);

        if (!inPlace && RevivalMap is { } revival)
        {
            Teleport(revival, force: true);
        }

        Protect(now);
        Map.Broadcast(this, Act(ActionType.Revive));
        return true;
    }

    public ulong GainExperience(double amount, bool bonused = false)
    {
        if (amount <= 0)
        {
            return 0;
        }

        var gained = (ulong)(bonused ? amount * ExperienceBonus : amount);
        var (level, before) = (Level, Experience);
        Experience += gained;

        while (Level < MaxLevel && World.Data.LevelExps.TryGetValue(Level, out var next) && Experience >= next.Experience)
        {
            Experience -= next.Experience;
            GainLevel();
        }

        if (Level > level)
        {
            OnLevelUp();
        }

        AwardLeaderVirtue(Level > level ? LevelVirtue(level) : StepVirtue(before));
        Update(AttributeType.Exp, (uint)Experience);
        return gained;
    }

    public void SetLevel(byte level)
    {
        while (Level < level)
        {
            GainLevel();
        }

        Level = level;
        OnLevelUp();
    }

    private void GainLevel()
    {
        Level++;

        if (Rebirths == 0 && Level <= AutoAllotLevel &&
            World.Data.PointAllots.TryGetValue(((byte)(ProfessionClass == TaoistClass ? 10 : ProfessionClass), Level), out var allot))
        {
            Strength = allot.Strength;
            Agility = allot.Agility;
            Vitality = allot.Vitality;
            Spirit = allot.Spirit;
        }
        else
        {
            AttributePoints += PointsPerLevel;
        }
    }

    private void OnLevelUp()
    {
        RecalculateStats();
        AddHealth(Stats.MaxHealth);
        SendAttributes();
        Show(AttributeType.Level, Level);
        Map?.Broadcast(this, Act(ActionType.LevelUp));
    }

    public void SendAttributes() => Send(new MsgUserAttrib
    {
        Id = Id,
        Attributes =
        {
            (AttributeType.Strength, Strength), (AttributeType.Agility, Agility), (AttributeType.Vitality, Vitality),
            (AttributeType.Spirit, Spirit), (AttributeType.AttributePoints, AttributePoints)
        }
    });

    private uint LevelVirtue(byte level)
    {
        if (level >= GuideLevel || !World.Data.LevelExps.TryGetValue(level, out var next))
        {
            return 0;
        }

        if (level < CoachLevel)
        {
            return next.Experience / (uint)(level * VirtueRate);
        }

        return (uint)(next.Experience * StepVirtueShare / (VirtueSteps(level) * level * VirtueRate) +
                      next.Experience * LevelVirtueShare / (level * VirtueRate));
    }

    private uint StepVirtue(ulong before)
    {
        if (Level is < CoachLevel or >= GuideLevel || !World.Data.LevelExps.TryGetValue(Level, out var next))
        {
            return 0;
        }

        var steps = VirtueSteps(Level);
        var step = (ulong)((1000 + steps - 1) / steps);

        return before * 1000 / next.Experience / step == Experience * 1000 / next.Experience / step
            ? 0
            : (uint)(next.Experience * StepVirtueShare / (steps * Level * VirtueRate));
    }

    private static int VirtueSteps(int level) => level switch
    {
        < 50 => 3,
        < 60 => 4,
        < 65 => 6,
        _ => 10
    };

    private void AwardLeaderVirtue(uint virtue)
    {
        if (virtue == 0 || Team is not { Leader: var leader } team || leader == this || leader.Level < GuideLevel ||
            leader.Level < Level + GuideGap || leader.Map != Map || leader.DistanceTo(this) > ShareRange)
        {
            return;
        }

        leader.EarnVirtue(virtue);
        team.Send(MsgTalk.System($"{leader.Name} has earned {virtue} virtue points for helping newbies.", TalkChannel.Normal));
    }

    public void BoostExperience(int percent, int seconds) =>
        (ExperienceBoost, ExperienceBoostTime) = ((ushort)percent, (uint)seconds * 1000);

    public uint ExperienceForTime(int units)
    {
        var (experience, level, left, progress) = (0.0, Level, (double)units, (double)Experience);

        while (World.Data.LevelExps.TryGetValue(level, out var next) && next.LevelUpTime > 0)
        {
            var remaining = (1 - progress / next.Experience) * next.LevelUpTime;

            if (left <= remaining)
            {
                return (uint)(experience + left / next.LevelUpTime * next.Experience);
            }

            experience += next.Experience - progress;
            left -= remaining;
            progress = 0;
            level++;
        }

        return (uint)experience;
    }

    private double ExperienceBonus =>
        Level >= MaxLevel ? 0
        : (100 + Stats.RainbowGems) / 100.0 * (ExperienceBoostTime > 0 && MapId != TrainingGround ? ExperienceBoost / 100.0 : 1) /
          (Level >= HalvedExperienceLevel ? 2 : 1) / (Rebirths > 1 ? Rebirths * 1.5 : 1);

    protected override int Mastery =>
        Form is null && Worn(ItemPosition.Right) is { } weapon &&
        WeaponSkills.FirstOrDefault(skill => skill.Type == weapon.WeaponSkill) is { Unlearned: false, Level: > MasterSkillLevel } skill
            ? skill.Level - MasterSkillLevel
            : 0;

    private void TrainWeaponSkills(Fighter target, int damage)
    {
        var practice = damage * (target is DynaNpc ? GoalSkillRate : 1);

        if (Worn(ItemPosition.Right) is not { } weapon)
        {
            Train(0, practice);
            return;
        }

        Train(weapon.WeaponSkill, practice);

        if (Worn(ItemPosition.Left) is { IsArrow: false } offHand)
        {
            Train(offHand.WeaponSkill, damage / 2.0);
        }
    }

    private bool Share(Fighter prey, int spoils)
    {
        if (Team is not { Members.Count: > 1 } team)
        {
            return false;
        }

        var sharing = team.Members
            .Where(member => member.IsAlive && member.Map == prey.Map && member.DistanceTo(prey) <= ShareRange).ToList();
        var newbie = sharing.Any(member => member.Level + NewbieGap < prey.Level);

        foreach (var member in sharing.Where(member => member != this))
        {
            var amount = member.ShareOf(prey, spoils, newbie, member.Spouse == Name);
            member.Tell($"{member.GainExperience(amount, bonused: true)} team experience points gained.", TalkChannel.System);
        }

        return newbie;
    }

    private int ShareOf(Fighter prey, int spoils, bool newbie, bool spouse)
    {
        if (prey.Level >= Level + NewbieGap)
        {
            return Level * (spouse ? SpouseLevelShare : LevelShare);
        }

        var percent = (prey.Level - Level) switch
        {
            > 10 => 130,
            > 5 => 120,
            _ => 100
        };
        var share = Math.Max(1, spoils * percent / 100);
        var water = Profession is >= Profession.WaterWizard and <= Profession.WaterSaint ? WaterSharePercent : 100;

        return share * water / 100 * (newbie ? 2 : 1) * (spouse ? 2 : 1);
    }

    public override bool CanAttack(Fighter target) => target switch
    {
        Monster { Owner: { } owner } => owner != this && CanAttack(owner),
        Monster { MonsterType.IsReviver: true } => false,
        Monster { MonsterType.IsGuardian: true } => PkMode is PkMode.Pk or PkMode.Team,
        DynaNpc { IsPole: true } => Map!.War is { IsOn: true } war && !war.Defends(this),
        DynaNpc npc => npc.IsGoal || npc.IsGate || npc.IsStatue,
        Player other => !Map!.Info.Is(MapFlags.PkDisable) && PkMode switch
        {
            PkMode.Peace => false,
            PkMode.Capture => other.Is(Evil),
            PkMode.Team => !Spares(other),
            _ => true
        },
        _ => true
    };

    private bool Spares(Player other) => Team?.Has(other.Id) == true || IsFriend(other.Id) || Spouse == other.Name || IsGuildmate(other);

    private bool IsGuildmate(Player other) =>
        other.MainGuild is { } theirs && (theirs == MainGuild || MainGuild?.IsAlliedWith(theirs.Id) == true);

    protected override void OnHealthChanged()
    {
        Update(AttributeType.Health, (uint)Health);
        Team?.Send(new MsgUserAttrib
        {
            Id = Id,
            Attributes = { (AttributeType.MaxHealth, (uint)Stats.MaxHealth), (AttributeType.Health, (uint)Health) }
        });
    }

    protected override int Reflection() =>
        PreviousProfession == Profession.WarriorMaster && ProfessionClass == WarriorClass &&
        Reflections.FirstOrDefault(tier => Level >= tier.Level) is { Chance: > 0 } tier && World.Random.Next(100) < tier.Chance
            ? tier.Cap
            : 0;

    protected override void OnDamaged(bool blow)
    {
        if (blow)
        {
            WearDown(ItemPosition.Head, ItemPosition.Necklace, ItemPosition.Armor, ItemPosition.Boots);
        }

        Shine();
        ClearEffect(PlayerStatus.Invisible);
        _mining = false;

        if (Pose == Sitting)
        {
            Pose = Standby;
            SetStamina(Stamina / 2);
            Map?.Broadcast(this, new MsgAction
            {
                Id = Id,
                X = X,
                Y = Y,
                Direction = (ushort)Direction,
                Data = Standby,
                Action = ActionType.Emotion
            });
        }
    }

    public override void ShowTo(Player player)
    {
        base.ShowTo(player);

        if (Kills > 0)
        {
            player.Send(new MsgInteract { Sender = Id, Action = InteractAction.Kill, Data = (uint)KoTally });
        }
    }

    protected internal override void OnAttacking(Fighter target, DateTimeOffset now)
    {
        EndProtection();

        if (!IsEvil(target) && !Map!.Info.Is(Arena))
        {
            ApplyEffect(PlayerStatus.BlueName, 1, now + CrimeDuration);
        }
    }

    protected internal override void OnDamageDealt(Fighter target, int damage, DateTimeOffset now, bool blow)
    {
        if (blow)
        {
            WearDown(ItemPosition.Right, ItemPosition.Left, ItemPosition.Ring);
        }

        if (blow && damage > 0)
        {
            Sparkle();
        }

        if (target is DynaNpc { IsPole: true })
        {
            Map!.War!.Struck(this, damage);
        }

        if (!target.IsAlive)
        {
            OnKill(target);
        }

        if (!target.GivesExperience)
        {
            return;
        }

        var spoils = target.IsAlive || MapId == TrainingGround ? 0 : (int)(target.Stats.MaxHealth * KillBonus);
        var newbie = spoils > 0 && target is Monster && Share(target, spoils);
        var bonus = spoils * (newbie ? 2 : 1);

        if (blow && spoils > 0)
        {
            bonus = Flourish(bonus);
        }

        var shown = (uint)(bonus * ExperienceGap(target));

        if (shown > 0)
        {
            Tell($"{shown} extra experience points gained for killing.", TalkChannel.System);
        }

        if (!blow)
        {
            GainExperience(bonus * ExperienceGap(target), bonused: true);
            return;
        }

        if (Form is null && damage > 0)
        {
            TrainWeaponSkills(target, damage);
        }

        GainExperience((target is DynaNpc ? TrainingShare(damage + bonus) : damage + bonus) * ExperienceGap(target), bonused: true);
    }

    private void OnKill(Fighter prey)
    {
        if (Xp < Full - 1)
        {
            SetXp((byte)(Xp + 1));
        }

        if (Is(Frenzy))
        {
            ExtendEffect(PlayerStatus.Superman, FrenzyPerKill, _frenzySince + MaxFrenzy);
            ExtendEffect(PlayerStatus.Cyclone, FrenzyPerKill, _frenzySince + MaxFrenzy);
        }

        if (!IsHuntedMonster(prey))
        {
            return;
        }

        if (Is(Frenzy))
        {
            Kills++;
        }

        Charge(prey);
    }

    private static bool IsHuntedMonster(Fighter prey) =>
        IsMonsterId(prey) && prey is Monster { MonsterType.IsGuardian: false };

    public void Sacrifice(uint mapId, ushort x, ushort y)
    {
        DieInstantly(null, World.UtcNow);
        Teleport(mapId, x, y, force: true);
    }

    public override void Die(Fighter? killer, DateTimeOffset now) => Die(killer, now, reflected: false);

    protected override void DieReflected(Fighter reflector, DateTimeOffset now) => Die(reflector, now, reflected: true);

    private void Die(Fighter? killer, DateTimeOffset now, bool reflected)
    {
        var slayer = killer is Monster { Owner: { } master } ? master : killer;

        if (RevivalMap is { } revival)
        {
            Record(revival.Id, revival.PortalX, revival.PortalY);
        }

        if (slayer is Player murderer && !reflected && !Map!.Info.Is(Lawless))
        {
            murderer.OnPlayerKill(this, now);
        }

        _died = now;
        Target = null;
        AbortCast();
        Trade?.Cancel();
        Booth?.Close();
        ClearEffects();
        Transform(null);
        Status &= KeptOnDeath;
        SetXp(0);
        Flag(PlayerStatus.Freeze);
        ApplyDeathPenalty(slayer, now, reflected);
    }
}
