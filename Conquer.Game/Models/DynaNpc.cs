namespace Conquer.Game.Models;

public class DynaNpc : Fighter
{
    public const byte PoleType = 10;
    public const byte GuildOwned = 2;
    public const byte TrunkType = 4;

    private const byte WeaponGoal = 21;
    private const byte MagicGoal = 22;
    private const byte BowGoal = 23;
    private const byte GateType = 26;
    private const byte TeleporterType = 15;
    private const byte StatueType = 9;
    private const byte SceneSort = 4;
    private const byte DeathActionSort = 16;
    private const byte DeletableSort = 32;
    private const byte UserOwned = 1;
    private const int PoleSize = 5;
    private const int NpcSize = 4;

    public override IMessage Spawn => Statue is not null ? new MsgPlayer(this) : MaxHealth > 0 ? new MsgNpcInfoEx(this) : new MsgNpcInfo(this);
    public uint OwnerId { get; set; }
    public byte OwnerType { get; set; }
    public string Name { get; set; } = null!;
    public byte Type { get; set; }
    public uint Look { get; set; }
    public uint MaxHealth { get; set; }
    public byte Base { get; set; }
    public byte Sort { get; set; }
    public ushort Defense { get; set; }
    public byte MagicDefense { get; set; }
    public uint LinkId { get; set; }
    public uint Task0 { get; set; }
    public int Data0 { get; set; }
    public int Data1 { get; set; }
    public int Data2 { get; set; }
    public int Data3 { get; set; }
    public Statue? Statue { get; set; }
    public List<Item> Contents { get; private set; } = [];
    public bool IsGoal => Type is WeaponGoal or MagicGoal or BowGoal;
    public bool IsPole => Type == PoleType;
    public bool IsGate => Type == GateType;
    public bool IsScene => (Sort & SceneSort) != 0;
    public bool IsDeletable => (Sort & DeletableSort) != 0;
    public bool IsTrunk => Type == TrunkType;
    public bool IsTeleporter => Type == TeleporterType;
    public bool IsStatue => Type == StatueType;
    public bool HasDeathAction => (Sort & DeathActionSort) != 0;
    public string Title => IsPole && Map?.War?.Owner is { } owner ? owner.Name : Name;
    public override bool GivesExperience => IsGoal;
    public override int SizeAdd => IsPole ? PoleSize : NpcSize;

    public void Initialize()
    {
        Stats = new(Defense: Defense, MagicDefense: MagicDefense, MaxHealth: (int)MaxHealth);
        Health = (int)MaxHealth;
    }

    public override bool TakesSpellsFrom(Fighter caster) => !IsGoal || Type == MagicGoal && caster.Level >= Level;

    public override bool TakesBlowsFrom(Fighter attacker) => !IsGoal || attacker is not Player || Type == WeaponGoal && attacker.Level >= Level;

    public bool IsOwnedBy(Player player) => OwnerType switch
    {
        UserOwned => OwnerId == player.Id,
        GuildOwned => OwnerId == player.GuildId && player.ShownGuildRank == GuildRank.Leader,
        _ => false
    };

    public bool Claim(Player owner)
    {
        OwnerId = OwnerType switch
        {
            UserOwned => owner.Id,
            GuildOwned => owner.Guild is { } guild ? owner.World.MainGuildOf(guild).Id : 0,
            _ => 0
        };

        return OwnerType != GuildOwned || OwnerId != 0;
    }

    public DynaNpc Snapshot()
    {
        var copy = (DynaNpc)MemberwiseClone();
        copy.Contents = [.. Contents.Select(item => item with { })];
        return copy;
    }

    public IEnumerable<object> Rows() => [this, ..Contents];

    public void SetLook(uint look)
    {
        Look = look;
        Map!.Build(this);
        Map.Broadcast(this, Spawn);
    }

    public void SetHealth(int health)
    {
        Health = health;
        OnHealthChanged();
    }

    public override void Die(Fighter? killer, DateTimeOffset now)
    {
        if (!HasDeathAction)
        {
            Map!.World.DeleteNpc(this);
            return;
        }

        if (killer is Player player && Map!.World.Scripts.OnDestroy(player, this))
        {
            return;
        }

        AddHealth(Stats.MaxHealth);
    }

    protected override void OnHealthChanged()
    {
        if (!IsStatue)
        {
            Show(AttributeType.Health, (uint)Health);
        }
    }
}
