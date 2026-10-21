namespace Conquer.Game.Scripts.Drops;

public static class Labyrinth
{
    [Monster(3141)]
    public static void Slinger(Player player, Monster monster) => monster.Drop(721533, player, 1, 50);

    [Monster(3142)]
    public static void GoldGhost(Player player, Monster monster) => monster.Drop(721537, player, 1, 150);

    [Monster(3143)]
    public static void Gibbon(Player player, Monster monster) =>
        monster.Drop(player.Pick<uint>(150076, 152086, 160076, 120086, 900706, 121086, 113916, 721541), player);

    [Monster(3144)]
    public static void Bladeling(Player player, Monster monster) => monster.Drop(721534, player, 1, 70);

    [Monster(3145)]
    public static void AgileRat(Player player, Monster monster) => monster.Drop(721538, player, 1, 150);

    [Monster(3146)]
    public static void NagaLord(Player player, Monster monster) =>
        monster.Drop(player.Pick<uint>(131436, 133626, 130836, 134536, 111346, 114946, 117346, 721542), player);

    [Monster(3147)]
    public static void BlueBird(Player player, Monster monster) => monster.Drop(721535, player, 1, 100);

    [Monster(3148)]
    public static void FiendBat(Player player, Monster monster) => monster.Drop(721539, player, 1, 150);

    [Monster(3149)]
    public static void Talon(Player player, Monster monster) =>
        monster.Drop(player.Pick<uint>(150097, 152107, 160097, 120097, 900427, 121097, 113927, 721543), player);

    [Monster(3155)]
    public static void MinotaurL120(Player player, Monster monster) => monster.Drop(721536, player, 1, 250);

    [Monster(3156)]
    public static void Syren(Player player, Monster monster) =>
        monster.Drop(player.Pick<uint>(131357, 133337, 130357, 134357, 111357, 114357, 117357, 721544), player);
}
