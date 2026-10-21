namespace Conquer.Game.Models;

public partial class Player
{
    private const uint MaleGhost = 980_000_000;
    private const uint FemaleGhost = 990_000_000;
    private const uint FormLook = 10_000_000;
    private const int BareRange = 1;
    private const int BareSpeed = 1000;
    private const int BaseDodge = 50;

    private static readonly double[] TrojanHealth = [1, 1.05, 1.08, 1.10, 1.12, 1.15];
    private static readonly int[] MinorGem = [5, 10, 15];
    private static readonly int[] RainbowGem = [10, 15, 25];
    private static readonly int[] VioletGem = [30, 50, 100];
    private static readonly int[] MoonGem = [15, 30, 50];
    private static readonly int[] TortoiseGem = [4, 8, 12];

    private bool _ghost;

    public int BaseMaxHealth => (int)((Strength * 3 + Agility * 3 + Vitality * 24 + Spirit * 3) *
                                      ((int)Profession / 10 == 1 ? TrojanHealth[(int)Profession % 10] : 1));

    public int BaseMaxMana => Spirit * 5 * (ProfessionClass == TaoistClass ? 1 + (int)Profession % 10 : 1);

    public bool BecomeGhost()
    {
        if (IsAlive || _ghost)
        {
            return false;
        }

        _ghost = true;
        Flag(PlayerStatus.Die);
        return true;
    }

    public void RecalculateStats()
    {
        var (maxHealth, health, magic) = (Stats.MaxHealth, Health, Mana);

        Stats = Form is null ? Calculate() : Stats.Of(Form) with { MaxMana = Stats.MaxMana };
        Health = Math.Min(Health, Stats.MaxHealth);
        Mana = (ushort)Math.Min(Mana, Stats.MaxMana);

        if (Stats.MaxHealth != maxHealth)
        {
            Update(AttributeType.MaxHealth, (uint)Stats.MaxHealth);
        }

        if (Stats.MaxHealth != maxHealth || Health != health)
        {
            OnHealthChanged();
        }

        if (Mana != magic)
        {
            Update(AttributeType.Mana, Mana);
        }
    }

    public override int DamageTakenPercent(Fighter attacker) => Form is not null ? 100 : Rebirths switch
    {
        0 => 100,
        1 => (int)Profession % 10 < 3 ? 100 : 70,
        _ => attacker is Player { Rebirths: < 2 } ? 50 : 70
    };

    private Stats Calculate()
    {
        var data = World.Data;

        var worn = Equipment.Where(item => item.MaxDurability == 0 || item.Durability > 0)
            .Select(item => (Item: item, Type: TypeOf(item)))
            .Select(worn => (worn.Item, worn.Type, Half: worn.Item.Position == ItemPosition.Left && !worn.Type.IsShield ? 2 : 1,
                Plus: data.ItemPluses.GetValueOrDefault((worn.Type.PlusId, worn.Item.Plus)))).ToList();

        int Gems(int kind, int[] points, bool offHand = true) => worn.Where(w => offHand || w.Item.Position != ItemPosition.Left)
            .SelectMany(w => new[] { w.Item.Gem1, w.Item.Gem2 }).Where(gem => (int)gem / 10 == kind && (int)gem % 10 is >= 1 and <= 3)
            .Sum(gem => points[(int)gem % 10 - 1]);

        var weapons = worn.Where(w => w.Item.Position is ItemPosition.Right or ItemPosition.Left && !w.Type.IsShield && !w.Type.IsArrow)
            .Select(w => w.Type).ToList();
        int Wielded(Func<ItemType, int> stat, int bare) => weapons.Count > 0 ? (int)weapons.Average(stat) : bare;

        var ranges = worn.Where(w => w.Item.Position is ItemPosition.Right or ItemPosition.Left && w.Type.AttackRange > 0)
            .Select(w => (int)w.Type.AttackRange).ToList();

        return new(
            MinAttack: Strength + worn.Sum(w => (w.Type.AttackMin + (w.Plus?.AttackMin ?? 0)) / w.Half),
            MaxAttack: Strength + worn.Sum(w => (w.Type.AttackMax + (w.Plus?.AttackMax ?? 0)) / w.Half),
            Defense: worn.Sum(w => w.Type.Defense + (w.Plus?.Defense ?? 0)),
            MagicAttack: worn.Sum(w => w.Type.MagicAttack),
            MagicDefense: worn.Sum(w => w.Type.MagicDefense),
            Agility: Agility,
            Dodge: BaseDodge + worn.Sum(w => w.Type.Dodge + (w.Plus?.Dodge ?? 0)),
            Range: ranges.Count > 0 ? ranges.Sum() / ranges.Count : BareRange,
            AttackSpeed: Wielded(type => type.AttackSpeed, BareSpeed),
            MaxHealth: Math.Max(0, BaseMaxHealth + worn.Sum(w => w.Type.Health + w.Item.Enchant + (w.Plus?.Health ?? 0))),
            MaxMana: BaseMaxMana + worn.Sum(w => w.Type.Mana),
            Strength: Strength,
            AgilityBonus: worn.Where(w => w.Item.Position != ItemPosition.Left).Sum(w => w.Type.Agility + (w.Plus?.Agility ?? 0)) +
                          Gems(2, MinorGem, offHand: false),
            MagicBonus: worn.Sum(w => w.Plus?.MagicAttack ?? 0),
            MagicBlock: worn.Sum(w => w.Plus?.MagicDefense ?? 0),
            DragonGems: Gems(1, MinorGem),
            PhoenixGems: Gems(0, MinorGem),
            TortoiseGems: Gems(7, TortoiseGem),
            Bless: worn.Sum(w => w.Item.Bless),
            RainbowGems: Gems(3, RainbowGem),
            VioletGems: Gems(5, VioletGem),
            MoonGems: Gems(6, MoonGem),
            ArmorQuality: (int)(ItemAt(ItemPosition.Armor)?.ItemTypeId % 10 ?? 0));
    }
}
