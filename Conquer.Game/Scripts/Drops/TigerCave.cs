namespace Conquer.Game.Scripts.Drops;

public static class TigerCave
{
    [Monster(19)]
    public static void HawKing(Player player, Monster monster)
    {
        if (player.Roll(100000) < 1)
        {
            monster.Drop(1060100, player);
        }
        else if (player.Roll(100000) < 1)
        {
            monster.Drop(1060101, player);
        }
    }
}
