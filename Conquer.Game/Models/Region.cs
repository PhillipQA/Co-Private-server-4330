namespace Conquer.Game.Models;

public class Region
{
    public uint Id { get; set; }
    public uint MapId { get; set; }
    public uint Type { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort Width { get; set; }
    public ushort Height { get; set; }
    public string DataStr { get; set; } = "";
    public uint Data0 { get; set; }
    public uint Data1 { get; set; }
    public uint Data2 { get; set; }
    public uint Data3 { get; set; }
}
