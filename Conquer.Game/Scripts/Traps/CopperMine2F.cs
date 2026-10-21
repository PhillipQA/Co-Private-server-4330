namespace Conquer.Game.Scripts.Traps;

public static class CopperMine2F
{
    [Trap(308)]
    public static void ClimbUpFromFloor2A(Player player) => player.Teleport(1026, 162, 186, force: true);

    [Trap(309)]
    public static void ClimbUpFromFloor2B(Player player) => player.Teleport(1026, 187, 159, force: true);

    [Trap(386)]
    public static void DescendToFloor3A(Player player) => player.Teleport(2022, 72, 15, force: true);

    [Trap(387)]
    public static void DescendToFloor3B(Player player) => player.Teleport(2023, 15, 77, force: true);

    [Trap(392)]
    public static void DescendToFloor3C(Player player) => player.Teleport(2024, 72, 15, force: true);

    [Trap(393)]
    public static void DescendToFloor3D(Player player) => player.Teleport(2025, 15, 77, force: true);
}
