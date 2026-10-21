namespace Conquer.Game.Scripts.Drops;

public static class NewCanyon
{
    [Monster(120)]
    public static void MinMacaque(Player player, Monster monster) => monster.Drop(725021, player, 1, 30000);

    [Monster(121)]
    public static void ElfApe(Player player, Monster monster) => monster.Drop(725022, player, 1, 30000);

    [Monster(122)]
    public static void SlowApe(Player player, Monster monster) => monster.Drop(725023, player, 1, 30000);

    [Monster(123)]
    public static void SnakeMonster(Player player, Monster monster) => monster.Drop(725024, player, 1, 30000);
}
