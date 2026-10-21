namespace Conquer.Game.Models;

public class DMap
{
    public uint Width { get; set; }
    public uint Height { get; set; }
    public DMapCell[,] Cells { get; set; } = null!;
    public Portal[] Portals { get; set; } = null!;

    public bool InBounds(int x, int y) => (uint)x < Width && (uint)y < Height;
}

public readonly record struct DMapCell(ushort Mask, short Altitude);