namespace Conquer.Game.Scripts.Traps;

public static class CopperMine4F
{
    [Trap(326)]
    public static void ClimbUpFromFloor4E(Player player) => player.Teleport(2024, 113, 155, force: true);

    [Trap(327)]
    public static void ClimbUpFromFloor4F(Player player) => player.Teleport(2024, 162, 111, force: true);

    [Trap(332)]
    public static void ClimbUpFromFloor4G(Player player) => player.Teleport(2025, 113, 155, force: true);

    [Trap(333)]
    public static void ClimbUpFromFloor4H(Player player) => player.Teleport(2025, 162, 111, force: true);

    [Trap(422)]
    public static void ClimbUpFromFloor4A(Player player) => player.Teleport(2022, 113, 155, force: true);

    [Trap(423)]
    public static void ClimbUpFromFloor4B(Player player) => player.Teleport(2022, 162, 111, force: true);

    [Trap(428)]
    public static void ClimbUpFromFloor4C(Player player) => player.Teleport(2023, 113, 155, force: true);

    [Trap(429)]
    public static void ClimbUpFromFloor4D(Player player) => player.Teleport(2023, 162, 111, force: true);
}
