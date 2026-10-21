namespace Conquer.Game.Scripts.Drops;

public static class NewForest
{
    [Monster(110)]
    public static void WindSnake(Player player, Monster monster) => monster.Drop(725028, player, 1, 20000);

    [Monster(111)]
    public static void AlienBandit(Player player, Monster monster) => monster.Drop(725018, player, 1, 30000);

    [Monster(112)]
    public static void FireRat(Player player, Monster monster) => monster.Drop(725019, player, 1, 30000);

    [Monster(113)]
    public static void FireSpirit(Player player, Monster monster) => monster.Drop(725020, player, 1, 30000);
}
