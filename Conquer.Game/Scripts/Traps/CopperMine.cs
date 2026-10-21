namespace Conquer.Game.Scripts.Traps;

public static class CopperMine
{
    [Trap(302)]
    public static void DescendToFloor2A(Player player) => player.Teleport(2020, 72, 15, force: true);

    [Trap(303)]
    public static void DescendToFloor2B(Player player) => player.Teleport(2021, 15, 77, force: true);
}
