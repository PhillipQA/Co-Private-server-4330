using System.Text;

namespace Conquer.Game.Models;

public static class DMapLoader
{
    private enum Layer : uint
    {
        Terrain = 1,
        Cover = 4,
        Effect = 10,
        Sound = 15,
        NewEffect = 19
    }

    public static Dictionary<uint, DMap> Load(string clientPath)
    {
        var dMaps = new Dictionary<uint, DMap>();

        using var gameMapReader = new BinaryReader(File.OpenRead(Path.Combine(clientPath, "ini/GameMap.dat")));

        var gameMapCount = gameMapReader.ReadUInt32();

        for (var i = 0; i < gameMapCount; i++)
        {
            var dMap = new DMap();
            var mapDocId = gameMapReader.ReadUInt32();
            var dMapPathLength = gameMapReader.ReadInt32();
            var dMapPath = Encoding.Latin1.GetString(gameMapReader.ReadBytes(dMapPathLength));
            gameMapReader.BaseStream.Seek(4, SeekOrigin.Current);

            using var dMapReader = new BinaryReader(File.OpenRead(Path.Combine(clientPath, dMapPath)));

            dMapReader.BaseStream.Seek(268, SeekOrigin.Current);
            dMap.Width = dMapReader.ReadUInt32();
            dMap.Height = dMapReader.ReadUInt32();

            dMap.Cells = new DMapCell[dMap.Width, dMap.Height];

            for (var y = 0; y < dMap.Height; y++)
            {
                for (var x = 0; x < dMap.Width; x++)
                {
                    var (mask, _, altitude) = (dMapReader.ReadUInt16(), dMapReader.ReadUInt16(), dMapReader.ReadInt16());
                    dMap.Cells[x, y] = new(mask, altitude);
                }

                dMapReader.BaseStream.Seek(4, SeekOrigin.Current);
            }

            var portalCount = dMapReader.ReadUInt32();
            dMap.Portals = new Portal[portalCount];

            for (var j = 0; j < dMap.Portals.Length; j++)
            {
                dMap.Portals[j] = new()
                {
                    PortalX = (ushort)dMapReader.ReadUInt32(),
                    PortalY = (ushort)dMapReader.ReadUInt32(),
                    Idx = dMapReader.ReadUInt32()
                };
            }

            var layerCount = dMapReader.ReadUInt32();

            for (var j = 0; j < layerCount; j++)
            {
                switch ((Layer)dMapReader.ReadUInt32())
                {
                    case Layer.Cover:
                        dMapReader.BaseStream.Seek(416, SeekOrigin.Current);
                        break;

                    case Layer.Terrain:
                        var scenePath = Encoding.Latin1.GetString(dMapReader.ReadBytes(260)).Split('\0')[0];
                        var (startX, startY) = (dMapReader.ReadInt32(), dMapReader.ReadInt32());

                        foreach (var (x, y, cell) in Scene.Load(Path.Combine(clientPath, scenePath)).On(dMap, startX, startY).ToList())
                        {
                            dMap.Cells[x, y] = cell;
                        }
                        break;

                    case Layer.Sound:
                        dMapReader.BaseStream.Seek(276, SeekOrigin.Current);
                        break;

                    case Layer.Effect:
                        dMapReader.BaseStream.Seek(72, SeekOrigin.Current);
                        break;

                    case Layer.NewEffect:
                        dMapReader.BaseStream.Seek(96, SeekOrigin.Current);
                        break;
                }
            }

            dMaps[mapDocId] = dMap;
        }

        return dMaps;
    }
}