namespace Conquer.Game.Scripts.Drops;

public static class GlobeForest
{
    [Monster(1400, 1401, 1402)]
    public static void Leaders(Player player, Monster monster)
    {
        if (player.Roll(10) < 1)
        {
            monster.Drop(player.Pick<uint>(1000010, 1000020, 1000030, 1001020, 1001010, 1001000, 1002000, 1002010),
                player);
        }
        else if (player.Roll(1000) < 1)
        {
            monster.Drop(1060102, player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(1060020, player);
        }
    }
}
