namespace Conquer.Game.Scripts.Drops;

public static class TestIsland
{
    [Monster(7001)]
    public static void WarriorDevil(Player player, Monster monster) => monster.Drop(710016, player);

    [Monster(7002)]
    public static void TrojanDevil(Player player, Monster monster) => monster.Drop(710017, player);

    [Monster(7003)]
    public static void ArcherDevil(Player player, Monster monster) => monster.Drop(710020, player);

    [Monster(7004)]
    public static void FireDevil(Player player, Monster monster) => monster.Drop(710018, player);

    [Monster(7005)]
    public static void WaterDevil(Player player, Monster monster) => monster.Drop(710019, player);
}
