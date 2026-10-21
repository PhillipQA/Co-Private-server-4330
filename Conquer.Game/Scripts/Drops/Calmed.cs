namespace Conquer.Game.Scripts.Drops;

public static class Calmed
{
    [Monster(6005)]
    public static void ToughHorn(Player player, Monster monster)
    {
        if (player.Roll(45) < 1)
        {
            monster.Drop(721015, player);
            player.Take(721064);
        }
    }
}
