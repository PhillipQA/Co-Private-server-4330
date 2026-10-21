namespace Conquer.Game.Scripts.Drops;

public static class Prosperous
{
    [Monster(6003)]
    public static void ToughHorn(Player player, Monster monster)
    {
        if (player.Roll(45) < 1)
        {
            monster.Drop(721013, player);
            player.Take(721064);
        }
    }
}
