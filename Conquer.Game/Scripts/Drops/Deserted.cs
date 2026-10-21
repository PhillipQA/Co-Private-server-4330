namespace Conquer.Game.Scripts.Drops;

public static class Deserted
{
    [Monster(6002)]
    public static void ToughHorn(Player player, Monster monster)
    {
        if (player.Roll(40) < 1)
        {
            monster.Drop(721012, player);
            player.Take(721064);
        }
    }
}
