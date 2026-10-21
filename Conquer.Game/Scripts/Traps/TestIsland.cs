namespace Conquer.Game.Scripts.Traps;

public static class TestIsland
{
    [Trap(50)]
    public static void SummonAncientDevil(Player player)
    {
        var devil = new DynaNpc
        {
            Name = "AncientDevil",
            Type = 24,
            Sort = 17,
            Look = 7700,
            OwnerType = 2,
            OwnerId = 4,
            MapId = 1052,
            X = 218,
            Y = 208,
            MaxHealth = 48683250,
            LinkId = 511500
        };

        if (player.Has(710011) && player.Has(710012) && player.Has(710013) && player.Has(710014) &&
            player.Has(710015) && player.World.CreateNpc(devil) &&
            player.Take(710011) && player.Take(710012) && player.Take(710013) && player.Take(710014))
        {
            player.Take(710015);
        }
    }
}
