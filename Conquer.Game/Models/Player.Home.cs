namespace Conquer.Game.Models;

public partial class Player
{
    private const int TeamReach = 5;

    public Map? Home => World.Maps.GetValueOrDefault(HomeId);

    public bool CreateHome(string name, ushort docId, uint type, ushort portalX, ushort portalY, uint rebornMap, uint rebornPortal, byte level)
    {
        var info = new MapInfo
        {
            Id = World.MapIds.Next(),
            Name = name,
            DescribeText = name,
            DocId = docId,
            Type = (MapFlags)type,
            OwnerId = Id,
            Level = level,
            PortalX = portalX,
            PortalY = portalY,
            RevivalMapId = rebornMap,
            RevivalPortal = rebornPortal,
            LinkMapId = MapId,
            LinkX = X,
            LinkY = Y,
            Light = uint.MaxValue
        };

        if (World.Open(info) is not { } home)
        {
            return false;
        }

        HomeId = home.Id;
        return true;
    }

    public bool GoHome()
    {
        var followers = Team?.Leader == this
            ? Team.Members.Where(member => member != this && member.Map == Map && member.DistanceTo(this) <= TeamReach)
                .ToList()
            : [];

        if (!Visit(HomeId))
        {
            return false;
        }

        followers.ForEach(member => member.Visit(HomeId));
        return true;
    }

    public bool Visit(uint homeId) =>
        World.Maps.GetValueOrDefault(homeId) is { } home && Teleport(home, force: true);

    public bool PlaceNpc(DynaNpc npc, ushort x, ushort y, int direction, uint region = 0, ushort frame = 0, byte pose = 0)
    {
        var map = Map!;

        if (!map.IsWalkable(x, y) || map.HasNpcAt(x, y))
        {
            return false;
        }

        if (npc.IsTeleporter && map.Info.Is(MapFlags.TeleportDisable))
        {
            Tell("This kind of NPC is not allowed to place here!");
            return false;
        }

        if (region != 0 && !map.InRegion(region, x, y) || !npc.Claim(this))
        {
            return false;
        }

        if (npc.IsStatue)
        {
            npc.Name = Name;
            npc.Statue = new(Look, Hair, Worn(ItemPosition.Head)?.Id ?? 0, Worn(ItemPosition.Armor)?.Id ?? 0,
                Worn(ItemPosition.Right)?.Id ?? 0, Worn(ItemPosition.Left)?.Id ?? 0, frame, pose, ShownGuildRank);
        }

        npc.Look = npc.Look / 10 * 10 + (uint)direction;
        (npc.MapId, npc.X, npc.Y) = (map.Id, x, y);
        return World.CreateNpc(npc);
    }
}
