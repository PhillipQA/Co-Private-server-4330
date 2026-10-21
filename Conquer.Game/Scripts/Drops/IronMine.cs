namespace Conquer.Game.Scripts.Drops;

public static class IronMine
{
    [Monster(106)]
    public static void RockMonsterL42(Player player, Monster monster)
    {
        if (player.Roll(60) < 1)
        {
            monster.Drop(721132, player);
        }
        else if (player.Roll(260) < 1)
        {
            monster.Drop(723007, player);
        }
    }
}
