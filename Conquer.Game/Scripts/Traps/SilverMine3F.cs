namespace Conquer.Game.Scripts.Traps;

public static class SilverMine3F
{
    [Trap(316)]
    public static void ClimbUpFromFloor3A(Player player) => player.Teleport(2040, 113, 155, force: true);

    [Trap(317)]
    public static void ClimbUpFromFloor3B(Player player) => player.Teleport(2040, 162, 111, force: true);

    [Trap(322)]
    public static void ClimbUpFromFloor3C(Player player) => player.Teleport(2041, 113, 155, force: true);

    [Trap(323)]
    public static void ClimbUpFromFloor3D(Player player) => player.Teleport(2041, 162, 111, force: true);

    [Trap(400)]
    public static void DescendToFloor4A(Player player) => player.Teleport(2046, 72, 15, force: true);

    [Trap(401)]
    public static void DescendToFloor4B(Player player) => player.Teleport(2047, 15, 77, force: true);

    [Trap(406)]
    public static void DescendToFloor4C(Player player) => player.Teleport(2048, 72, 15, force: true);

    [Trap(407)]
    public static void DescendToFloor4D(Player player) => player.Teleport(2049, 15, 77, force: true);

    [Trap(412)]
    public static void DescendToFloor4E(Player player) => player.Teleport(2050, 72, 15, force: true);

    [Trap(413)]
    public static void DescendToFloor4F(Player player) => player.Teleport(2051, 15, 77, force: true);

    [Trap(418)]
    public static void DescendToFloor4G(Player player) => player.Teleport(2052, 72, 15, force: true);

    [Trap(419)]
    public static void DescendToFloor4H(Player player) => player.Teleport(2053, 15, 77, force: true);
}
