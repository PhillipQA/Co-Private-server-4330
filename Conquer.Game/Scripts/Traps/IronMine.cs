namespace Conquer.Game.Scripts.Traps;

public static class IronMine
{
    [Trap(300)]
    public static void DescendToFloor2A(Player player) => player.Teleport(2000, 72, 15, force: true);

    [Trap(301)]
    public static void DescendToFloor2B(Player player) => player.Teleport(2001, 15, 77, force: true);
}
