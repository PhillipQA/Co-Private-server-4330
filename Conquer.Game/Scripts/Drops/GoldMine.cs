namespace Conquer.Game.Scripts.Drops;

public static class GoldMine
{
    [Monster(105)]
    public static void RockMonsterL17(Player player, Monster monster) => monster.Drop(723007, player, 1, 300);
}
