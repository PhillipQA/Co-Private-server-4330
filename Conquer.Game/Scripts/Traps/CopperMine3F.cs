namespace Conquer.Game.Scripts.Traps;

public static class CopperMine3F
{
    [Trap(314)]
    public static void ClimbUpFromFloor3A(Player player) => player.Teleport(2020, 113, 155, force: true);

    [Trap(315)]
    public static void ClimbUpFromFloor3B(Player player) => player.Teleport(2020, 162, 111, force: true);

    [Trap(320)]
    public static void ClimbUpFromFloor3C(Player player) => player.Teleport(2021, 113, 155, force: true);

    [Trap(321)]
    public static void ClimbUpFromFloor3D(Player player) => player.Teleport(2021, 162, 111, force: true);

    [Trap(398)]
    public static void DescendToFloor4A(Player player) => player.Teleport(2026, 72, 15, force: true);

    [Trap(399)]
    public static void DescendToFloor4B(Player player) => player.Teleport(2027, 15, 77, force: true);

    [Trap(404)]
    public static void DescendToFloor4C(Player player) => player.Teleport(2028, 72, 15, force: true);

    [Trap(405)]
    public static void DescendToFloor4D(Player player) => player.Teleport(2029, 15, 77, force: true);

    [Trap(410)]
    public static void DescendToFloor4E(Player player) => player.Teleport(2030, 72, 15, force: true);

    [Trap(411)]
    public static void DescendToFloor4F(Player player) => player.Teleport(2031, 15, 77, force: true);

    [Trap(416)]
    public static void DescendToFloor4G(Player player) => player.Teleport(2032, 72, 15, force: true);

    [Trap(417)]
    public static void DescendToFloor4H(Player player) => player.Teleport(2033, 15, 77, force: true);
}
