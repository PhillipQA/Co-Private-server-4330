namespace Conquer.Game.Scripts.Traps;

public static class IronMine2F
{
    [Trap(306)]
    public static void ClimbUpFromFloor2A(Player player) => player.Teleport(1025, 113, 155, force: true);

    [Trap(307)]
    public static void ClimbUpFromFloor2B(Player player) => player.Teleport(1025, 162, 111, force: true);

    [Trap(384)]
    public static void DescendToFloor3A(Player player) => player.Teleport(2002, 72, 15, force: true);

    [Trap(385)]
    public static void DescendToFloor3B(Player player) => player.Teleport(2003, 15, 77, force: true);

    [Trap(390)]
    public static void DescendToFloor3C(Player player) => player.Teleport(2004, 72, 15, force: true);

    [Trap(391)]
    public static void DescendToFloor3D(Player player) => player.Teleport(2005, 15, 77, force: true);
}
