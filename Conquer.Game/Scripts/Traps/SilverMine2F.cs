namespace Conquer.Game.Scripts.Traps;

public static class SilverMine2F
{
    [Trap(310)]
    public static void ClimbUpFromFloor2A(Player player) => player.Teleport(1027, 163, 187, force: true);

    [Trap(311)]
    public static void ClimbUpFromFloor2B(Player player) => player.Teleport(1027, 187, 159, force: true);

    [Trap(388)]
    public static void DescendToFloor3A(Player player) => player.Teleport(2042, 72, 15, force: true);

    [Trap(389)]
    public static void DescendToFloor3B(Player player) => player.Teleport(2043, 15, 77, force: true);

    [Trap(394)]
    public static void DescendToFloor3C(Player player) => player.Teleport(2044, 72, 15, force: true);

    [Trap(395)]
    public static void DescendToFloor3D(Player player) => player.Teleport(2045, 15, 77, force: true);
}
