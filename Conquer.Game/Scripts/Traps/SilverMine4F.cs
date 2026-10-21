namespace Conquer.Game.Scripts.Traps;

public static class SilverMine4F
{
    [Trap(328)]
    public static void ClimbUpFromFloor4E(Player player) => player.Teleport(2044, 113, 155, force: true);

    [Trap(329)]
    public static void ClimbUpFromFloor4F(Player player) => player.Teleport(2044, 162, 111, force: true);

    [Trap(334)]
    public static void ClimbUpFromFloor4G(Player player) => player.Teleport(2045, 113, 155, force: true);

    [Trap(335)]
    public static void ClimbUpFromFloor4H(Player player) => player.Teleport(2045, 162, 111, force: true);

    [Trap(424)]
    public static void ClimbUpFromFloor4A(Player player) => player.Teleport(2042, 113, 155, force: true);

    [Trap(425)]
    public static void ClimbUpFromFloor4B(Player player) => player.Teleport(2042, 162, 111, force: true);

    [Trap(430)]
    public static void ClimbUpFromFloor4C(Player player) => player.Teleport(2043, 113, 155, force: true);

    [Trap(431)]
    public static void ClimbUpFromFloor4D(Player player) => player.Teleport(2043, 162, 111, force: true);
}
