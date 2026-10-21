namespace Conquer.Game.Scripts.Drops;

public static class Peace
{
    [Monster(6000)]
    public static void ToughHorn(Player player, Monster monster)
    {
        if (player.Roll(45) < 1)
        {
            monster.Drop(721010, player);
            player.Take(721064);
        }
    }
}
