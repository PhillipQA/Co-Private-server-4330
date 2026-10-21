namespace Conquer.Game.Scripts.Drops;

public static class Disturbed
{
    [Monster(6004)]
    public static void ToughHorn(Player player, Monster monster)
    {
        if (player.Roll(65) < 1)
        {
            monster.Drop(721014, player);
            player.Take(721064);
        }
    }
}
