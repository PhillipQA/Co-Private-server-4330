namespace Conquer.Game.Scripts.Traps;

public static class EvilAbyss
{
    [Trap(500)]
    public static void ExitToCanyon(Player player) => player.Teleport(1020, 438, 306);
}
