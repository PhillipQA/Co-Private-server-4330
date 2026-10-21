namespace Conquer.Game.Models;

public partial class Player
{
    private const int RebornLevel = 15;
    private const int FirstRebirthPoints = 72;
    private const int StartingPoints = 10;
    private const int RebirthLevelFloor = 120;
    private const int WaterRebirthLevel = 110;

    public bool SetProfession(Profession profession)
    {
        if (NextRebirth(profession) is { } rebirth)
        {
            Reborn(profession, rebirth);
            return true;
        }

        Profession = profession;
        Show(AttributeType.Profession, (uint)Profession);
        RecalculateStats();
        return true;
    }

    public bool Rebirth(Profession profession, int body)
    {
        if (NextRebirth(profession) is not { } rebirth)
        {
            return false;
        }

        SetBody(body);
        Reborn(profession, rebirth);
        return true;
    }

    public void RedistributePoints()
    {
        var level = Rebirths == 0 ? Math.Min(Level, (byte)AutoAllotLevel) : 1;

        if (World.Data.PointAllots.GetValueOrDefault(((byte)(ProfessionClass == TaoistClass ? 10 : ProfessionClass), (byte)level)) is { } allot)
        {
            Strength = allot.Strength;
            Agility = allot.Agility;
            Vitality = allot.Vitality;
            Spirit = allot.Spirit;
        }

        AttributePoints = (ushort)(Rebirths == 0
            ? (Level - level) * PointsPerLevel
            : RebirthBonus + (Level - RebornLevel) * PointsPerLevel);
    }

    private int RebirthPoints(Rebirth rebirth)
    {
        var level = Math.Min(Level, MaxLevel);

        if (Rebirths == 0)
        {
            return FirstRebirthPoints +
                   Triangle(Profession == Profession.WaterSaint ? (level - WaterRebirthLevel) / 2 : level - RebirthLevelFloor);
        }

        var surplus = Strength + Agility + Vitality + Spirit + AttributePoints - StartingPoints - (Level - 1) * PointsPerLevel;
        return Triangle(Math.Max(0, level - RebirthLevelFloor)) + (rebirth.NewLevel - 1) * PointsPerLevel + Math.Max(0, surplus);
    }

    private static int Triangle(int n) => n * (n + 1) / 2;

    private Rebirth? NextRebirth(Profession profession) =>
        World.Data.Rebirths.GetValueOrDefault(((byte)Profession, (byte)profession, (byte)(Rebirths + 1))) is { } rebirth &&
        Level >= rebirth.RequiredLevel ? rebirth : null;

    private void Reborn(Profession profession, Rebirth rebirth)
    {
        RebirthBonus = (ushort)RebirthPoints(rebirth);
        PreviousProfession = Profession;
        Profession = profession;
        Level = rebirth.NewLevel;
        Rebirths = rebirth.Rebirths;
        Experience = 0;

        foreach (var item in Equipment.ToList())
        {
            Degrade(item);
        }

        RedistributePoints();
        RecalculateStats();
        AddHealth(Stats.MaxHealth);
        Mana = (ushort)Stats.MaxMana;

        Send(new MsgUserAttrib
        {
            Id = Id,
            Attributes =
            {
                (AttributeType.Level, Level), (AttributeType.Exp, 0), (AttributeType.Profession, (uint)Profession),
                (AttributeType.Health, (uint)Health), (AttributeType.Mana, Mana), (AttributeType.Strength, Strength),
                (AttributeType.Agility, Agility), (AttributeType.Vitality, Vitality), (AttributeType.Spirit, Spirit),
                (AttributeType.AttributePoints, AttributePoints), (AttributeType.Rebirths, Rebirths)
            }
        });

        Map?.Broadcast(this, new MsgUserAttrib
        {
            Id = Id,
            Attributes = { (AttributeType.Level, Level), (AttributeType.Rebirths, Rebirths) }
        });
        World.Announce($"{Name} has been reborn!");
    }
}
