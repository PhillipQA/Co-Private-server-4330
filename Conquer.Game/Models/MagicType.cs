namespace Conquer.Game.Models;

public class MagicType
{
    public uint Id { get; set; }
    public ushort Type { get; set; }
    public MagicSort Sort { get; set; }
    public string Name { get; set; } = null!;
    public byte Offensive { get; set; }
    public byte GroundTargeted { get; set; }
    public byte MultiTarget { get; set; }
    public uint Target { get; set; }
    public ushort Level { get; set; }
    public ushort ManaCost { get; set; }
    public int Power { get; set; }
    public ushort CastTime { get; set; }
    public byte SuccessChance { get; set; }
    public uint DurationSeconds { get; set; }
    public byte Range { get; set; }
    public byte CastRange { get; set; }
    public uint Status { get; set; }
    public ushort RequiredProfession { get; set; }
    public uint RequiredExp { get; set; }
    public byte RequiredLevel { get; set; }
    public byte XpSkill { get; set; }
    public ushort WeaponSubtype { get; set; }
    public uint ActiveTimes { get; set; }
    public byte AutoActive { get; set; }
    public uint FloorAttr { get; set; }
    public byte AutoLearn { get; set; }
    public ushort LearnLevel { get; set; }
    public byte DropWeapon { get; set; }
    public byte StaminaCost { get; set; }
    public byte WeaponHit { get; set; }
    public uint UseItem { get; set; }
    public ushort FollowUp { get; set; }
    public ushort FollowUpDelay { get; set; }
    public byte ArrowCost { get; set; }
    public ushort SenderAction { get; set; }
    public string UpgradeDesc { get; set; } = null!;
    public string Desc { get; set; } = null!;
    public string CastEffect { get; set; } = null!;
    public string CastSound { get; set; } = null!;
    public string SenderEffect { get; set; } = null!;
    public string SenderSound { get; set; } = null!;
    public uint TargetDelay { get; set; }
    public string TargetEffect { get; set; } = null!;
    public string TargetSound { get; set; } = null!;
    public string GroundEffect { get; set; } = null!;
    public string TraceEffect { get; set; } = null!;
    public uint ScreenRepresent { get; set; }
    public byte UsableInMarket { get; set; }
    public uint TargetWoundDelay { get; set; }

    public bool IsArea => Sort is MagicSort.Fan or MagicSort.Bomb or MagicSort.Line;
    public bool Damages => IsArea || Sort is MagicSort.Attack or MagicSort.Dashing or MagicSort.Striking or MagicSort.Sapping;
    public bool Revives => Sort == MagicSort.Reviving && Status == MagicStatus.Revive;
    public bool AimsAtTarget => Sort is MagicSort.Attack or MagicSort.Line or MagicSort.Striking;
}

public enum MagicSort : byte
{
    Attack = 1,
    Heal = 2,
    Fan = 4,
    Bomb = 5,
    Buffing = 6,
    Reviving = 7,
    Teleporting = 10,
    DispatchingXp = 11,
    Dashing = 12,
    Line = 14,
    Striking = 16,
    RecallingTeam = 17,
    Transforming = 19,
    AddMana = 20,
    CallingPet = 23,
    Vampire = 24,
    StandIn = 25,
    Sapping = 26
}

public static class MagicStatus
{
    public const uint Revive = 1;
    public const uint Poison = 4;
    public const uint Purge = 8;
    public const uint Accuracy = 32;
    public const uint MagicShield = 64;
    public const uint Stigma = 128;
    public const uint WeaponDamage = 128;
    public const uint Shield = 256;
    public const uint Superman = 8192;
    public const uint Bind = 16384;
    public const uint Stun = 65536;
    public const uint Invisible = 131072;
    public const uint Cyclone = 262144;
    public const uint Dodge = 2097152;
    public const uint CastingPray = 33554432;
    public const uint Flying = 134217728;
    public const uint Intensify = 268435456;
}