namespace Conquer.Game.Models;

[Flags]
public enum PlayerStatus : uint
{
    None = 0x00000000,
    BlueName = 0x00000001,
    Poison = 0x00000002,
    XpFull = 0x00000010,
    Freeze = 0x00000020,
    TeamLeader = 0x00000040,
    Accuracy = 0x00000080,
    MagicShield = 0x00000100,
    Stigma = 0x00000200,
    Die = 0x00000400,
    FadeOut = 0x00000800,
    RedName = 0x00004000,
    BlackName = 0x00008000,
    Superman = 0x00040000,
    Dodge = 0x00200000,
    Invisible = 0x00400000,
    Cyclone = 0x00800000,
    Flying = 0x08000000,
    Intensify = 0x10000000,
    CastingPray = 0x40000000,
    Praying = 0x80000000
}
