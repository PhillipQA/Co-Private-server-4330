namespace Conquer.Game.Scripts.Traps;

public static class SilverMine
{
    [Trap(304)]
    public static void DescendToFloor2A(Player player) => player.Teleport(2040, 72, 15, force: true);

    [Trap(305)]
    public static void DescendToFloor2B(Player player) => player.Teleport(2041, 15, 77, force: true);
}
