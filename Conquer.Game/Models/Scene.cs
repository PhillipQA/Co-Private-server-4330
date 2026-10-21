using System.Text;
using System.Text.RegularExpressions;

namespace Conquer.Game.Models;

public sealed partial record Scene(List<(int X, int Y, DMapCell Cell)> Cells)
{
    private const int PartHeader = 332;

    public IEnumerable<(int X, int Y, DMapCell Cell)> On(DMap map, int x, int y) => Cells
        .Select(cell => (X: x + cell.X, Y: y + cell.Y, cell.Cell))
        .Where(cell => map.InBounds(cell.X, cell.Y))
        .Select(cell => (cell.X, cell.Y, cell.Cell with { Altitude = (short)(map.Cells[cell.X, cell.Y].Altitude + cell.Cell.Altitude) }));

    private static Scene Read(BinaryReader reader)
    {
        var cells = new List<(int, int, DMapCell)>();

        for (var parts = reader.ReadUInt32(); parts > 0; parts--)
        {
            reader.BaseStream.Seek(PartHeader, SeekOrigin.Current);
            var (width, height) = (reader.ReadInt32(), reader.ReadInt32());
            reader.BaseStream.Seek(4, SeekOrigin.Current);
            var (offsetX, offsetY) = (reader.ReadInt32(), reader.ReadInt32());
            reader.BaseStream.Seek(4, SeekOrigin.Current);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var (mask, _, altitude) = ((ushort)reader.ReadUInt32(), reader.ReadUInt32(), (short)reader.ReadInt32());
                    cells.Add((offsetX - x, offsetY - y, new(mask, altitude)));
                }
            }
        }

        return new(cells);
    }

    public static Scene Load(string path)
    {
        using var reader = new BinaryReader(File.OpenRead(path));
        return Read(reader);
    }

    public static Dictionary<uint, Scene> LoadTerrainNpcs(string clientPath)
    {
        var scenes = new Dictionary<uint, Scene>();
        var files = new Dictionary<string, Scene>();
        var type = 0u;

        foreach (var line in File.ReadLines(Path.Combine(clientPath, "ini/terrainnpc.ini"), Encoding.Latin1))
        {
            if (NpcType().Match(line) is { Success: true } section)
            {
                type = uint.Parse(section.Groups[1].Value);
            }
            else if (Direction().Match(line) is { Success: true } direction &&
                     Path.Combine(clientPath, direction.Groups[2].Value.Trim()) is var path && File.Exists(path))
            {
                scenes[type * 10 + uint.Parse(direction.Groups[1].Value)] = files.TryGetValue(path, out var scene) ? scene : files[path] = Load(path);
            }
        }

        return scenes;
    }

    [GeneratedRegex(@"^\[NpcType(\d+)\]")]
    private static partial Regex NpcType();

    [GeneratedRegex(@"^Dir(\d)=(.+)$")]
    private static partial Regex Direction();
}
