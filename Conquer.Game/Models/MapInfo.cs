namespace Conquer.Game.Models;

[Flags]
public enum MapFlags : uint
{
    PkField = 0x1,
    ChangeMapDisable = 0x2,
    RecordDisable = 0x4,
    PkDisable = 0x8,
    BoothEnable = 0x10,
    TeamDisable = 0x20,
    TeleportDisable = 0x40,
    GuildMap = 0x80,
    Prison = 0x100,
    WingDisable = 0x200,
    House = 0x400,
    MineField = 0x800
}

public class MapInfo
{
    public bool Is(MapFlags flags) => (Type & flags) != 0;

    public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public string DescribeText { get; set; } = null!;
    public ushort DocId { get; set; }
    public MapFlags Type { get; set; }
    public uint Weather { get; set; }
    public uint BgMusic { get; set; }
    public uint BgMusicShow { get; set; }
    public ushort PortalX { get; set; }
    public ushort PortalY { get; set; }
    public uint RevivalMapId { get; set; }
    public uint RevivalPortal { get; set; }
    public uint Light { get; set; }
    public uint OwnerId { get; set; }
    public byte Level { get; set; }
    public uint LinkMapId { get; set; }
    public ushort LinkX { get; set; }
    public ushort LinkY { get; set; }

    public MapInfo Snapshot() => (MapInfo)MemberwiseClone();
}