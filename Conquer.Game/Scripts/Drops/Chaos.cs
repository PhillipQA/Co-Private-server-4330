namespace Conquer.Game.Scripts.Drops;

public static class Chaos
{
    [Monster(6001)]
    public static void ToughHorn(Player player, Monster monster)
    {
        if (player.Roll(60) < 1)
        {
            monster.Drop(721011, player);
            player.Take(721064);
        }
    }
}
