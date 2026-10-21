namespace Conquer.Game.Scripts.Drops;

public static class SnakeDen
{
    [Monster(6018)]
    public static void Python(Player player, Monster monster) => monster.Drop(723011, player);
}
