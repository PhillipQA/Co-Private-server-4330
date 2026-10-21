namespace Conquer.Game.Scripts.Drops;

public static class NewDesert
{
    [Monster(125)]
    public static void LapidatingMob(Player player, Monster monster)
    {
        if (!player.Has(722883) || player.Roll(20) >= 1)
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case 1 or 3 or 5 or 7:
                if (player.Map!.CountNpcs("PrincessDi") != 1)
                {
                    player.World.CreateNpc(new()
                    {
                        Name = "PrincessDi",
                        Type = 2,
                        Sort = 1,
                        Look = 8910,
                        MapId = player.MapId,
                        X = player.X,
                        Y = player.Y,
                        Task0 = 9501100
                    });
                }
                break;

            case 2 or 4 or 6 or 8:
                if (player.Map!.CountNpcs("YoungMasterWen") != 1)
                {
                    player.World.CreateNpc(new()
                    {
                        Name = "YoungMasterWen",
                        Type = 2,
                        Sort = 1,
                        Look = 8940,
                        MapId = player.MapId,
                        X = player.X,
                        Y = player.Y,
                        Task0 = 9501200
                    });
                }
                break;
        }
    }
}
