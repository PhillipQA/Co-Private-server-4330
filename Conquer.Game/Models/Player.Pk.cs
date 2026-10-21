namespace Conquer.Game.Models;

public partial class Player
{
    private const PlayerStatus Evil = PlayerStatus.BlueName | PlayerStatus.RedName | PlayerStatus.BlackName;
    private const MapFlags Arena = MapFlags.PkField | MapFlags.GuildMap;
    private const MapFlags Lawless = Arena | MapFlags.Prison;
    private const int RedNamePoints = 30;
    private const int BlackNamePoints = 100;
    private const int MaxPkPoints = 30000;
    private const int KillPoints = 10;
    private const int EnemyKillPoints = 5;
    private const int HostileKillPoints = 3;
    private const int WhiteExperienceLoss = 2;
    private const int RedExperienceLoss = 10;
    private const int BlackExperienceLoss = 20;
    private const int KillerShare = 50;
    private const int KillerSharePerLevel = 2;
    private const int MinKillerShare = 10;
    private const int KillerCapSeconds = 300;
    private const int BaseMoneyDrop = 10;
    private const int MaxMoneyDrop = 95;
    private const int BaseItemDrop = 20;
    private const int MaxItemDrop = 90;
    private const int ArenaDrop = 30;
    private const int MonsterDrop = 33;
    private const int NewbieMonsterDrop = 5;
    private const int NewbieLevel = 10;
    private const int SparedLevel = 15;
    private const uint JailMap = 6000;

    private static readonly TimeSpan CrimeDuration = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan MurderCrimeDuration = TimeSpan.FromSeconds(120);
    private static readonly TimeSpan PkDecay = TimeSpan.FromMinutes(6);

    public static readonly (uint Map, ushort X, ushort Y) JailCell = (JailMap, 29, 72);
    public static readonly (uint Map, ushort X, ushort Y) GuildMapRecall = (1002, 438, 398);

    private DateTimeOffset _nextPkDecay;

    public void SetPkPoints(int points)
    {
        PkPoints = (short)Math.Clamp(points, 0, MaxPkPoints);
        Update(AttributeType.PkPoints, (uint)PkPoints);

        Status = Status & ~(PlayerStatus.RedName | PlayerStatus.BlackName) |
                 (PkPoints >= BlackNamePoints ? PlayerStatus.BlackName : PkPoints >= RedNamePoints ? PlayerStatus.RedName : 0);
        Flag(PlayerStatus.None);
    }

    private void DecayPkPoints(DateTimeOffset now)
    {
        if (PkPoints == 0 || now < _nextPkDecay)
        {
            return;
        }

        if (_nextPkDecay != default)
        {
            SetPkPoints(PkPoints - 1);
        }

        _nextPkDecay = now + PkDecay;
    }

    private static bool IsEvil(Fighter fighter) => fighter switch
    {
        Player player => player.Is(Evil),
        Monster { Owner: { } owner } => IsEvil(owner),
        Monster monster => !monster.MonsterType.Is(Behavior.Righteous),
        _ => true
    };

    private void OnPlayerKill(Player victim, DateTimeOffset now)
    {
        if (!victim.Is(PlayerStatus.RedName | PlayerStatus.BlackName) && Is(PlayerStatus.BlackName))
        {
            World.Outlaw(this);
        }

        var loss = victim.Is(PlayerStatus.RedName) ? RedExperienceLoss
            : victim.Is(PlayerStatus.BlackName) ? BlackExperienceLoss
            : WhiteExperienceLoss;
        var lost = victim.Experience * (ulong)loss / 100;
        victim.LoseExperience(lost);
        victim.AddMana(-(victim.Mana / 2));

        if (Level <= victim.Level && World.Data.LevelExps.TryGetValue(Level, out var next))
        {
            var share = Math.Max(MinKillerShare, KillerShare + KillerSharePerLevel * (Level - victim.Level));
            GainExperience(Math.Min(lost * (ulong)share / 100, (ulong)next.Experience * KillerCapSeconds / next.LevelUpTime));
        }

        if (!victim.Is(Evil))
        {
            SetPkPoints(PkPoints + KillPointsFor(victim));

            if (Is(PlayerStatus.BlueName))
            {
                ApplyEffect(PlayerStatus.BlueName, 1, now + MurderCrimeDuration);
            }
        }

        Collect(victim);
    }

    private int KillPointsFor(Player victim) =>
        victim.MainGuild is { } theirs && MainGuild?.IsEnemyOf(theirs.Id) == true ? HostileKillPoints
        : Enemies.Any(enemy => enemy.EnemyPlayerId == victim.Id) ? EnemyKillPoints
        : KillPoints;

    private Guild? MainGuild => Guild is { } guild ? World.MainGuildOf(guild) : null;

    private void ApplyDeathPenalty(Fighter? killer, DateTimeOffset now, bool reflected)
    {
        if (Map!.Info.Is(Arena))
        {
            DropItems(ArenaDrop, IsArenaLoot, now);

            if (Map.Info.Is(MapFlags.GuildMap) && Map.War is not { IsOn: true })
            {
                Record(GuildMapRecall.Map, GuildMapRecall.X, GuildMapRecall.Y);
            }

            return;
        }

        if (Map.Info.Is(MapFlags.Prison))
        {
            DropInventory(now);
            return;
        }

        switch (killer)
        {
            case Player murderer:
                DeathPenaltyFromPlayer(murderer, now, reflected);
                break;

            case Monster monster:
                if (!IsHuntedMonster(monster) || Level > SparedLevel)
                {
                    DeathPenaltyFromMonster(now);
                }

                if (monster.MonsterType.IsGuardian && PkPoints >= BlackNamePoints)
                {
                    Imprison();
                }
                break;
        }
    }

    private static bool IsArenaLoot(ItemType type) => type.IsPotion || type.IsArmor && !type.IsKeptOnDeath;

    private static bool IsLoot(ItemType type) => !type.IsKeptOnDeath;

    private void DeathPenaltyFromPlayer(Player killer, DateTimeOffset now, bool reflected)
    {
        if (!reflected)
        {
            AddEnemy(killer);
        }

        DropMoney((uint)(Money * (ulong)Math.Min(PkPoints + BaseMoneyDrop, MaxMoneyDrop) / 100), now);

        DropInventory(now);

        if (World.Fugitives.Remove(this))
        {
            Imprison();
        }

        if (PkPoints >= BlackNamePoints)
        {
            World.Announce($"{killer.Name} has killed {Name} and sent him/her to the jail.");
            Jail();
        }
    }

    private void DeathPenaltyFromMonster(DateTimeOffset now)
    {
        DropMoney((uint)World.Random.Next((int)(Money / 3)), now);

        DropItems(Level < NewbieLevel ? NewbieMonsterDrop : MonsterDrop, IsLoot, now);
    }

    private void DropMoney(uint amount, DateTimeOffset now)
    {
        if (amount > 0 && Spend(amount))
        {
            Map!.Drop(new MapItem { Money = amount }, this, now);
        }
    }

    private void LoseExperience(ulong lost)
    {
        Experience -= lost;
        Update(AttributeType.Exp, (uint)Experience);
    }

    private void DropInventory(DateTimeOffset now) => DropItems(Math.Min(MaxItemDrop, BaseItemDrop + PkPoints / 2), IsLoot, now);

    private void Imprison()
    {
        World.Announce($"{Name} has killed too much and been put into the jail at last.");
        Jail();
    }

    public void Jail()
    {
        Team?.Remove(this);
        Teleport(JailCell.Map, JailCell.X, JailCell.Y, force: true);
        Record(JailCell.Map, JailCell.X, JailCell.Y);
    }

    private void DropItems(int chance, Func<ItemType, bool> droppable, DateTimeOffset now)
    {
        foreach (var item in Inventory.ToList())
        {
            var type = TypeOf(item);

            if (droppable(type) && World.Random.Next(100) < chance && Map!.Drop(new MapItem { Item = item, Price = type.Price }, this, now))
            {
                Take(item);
            }
        }
    }
}
