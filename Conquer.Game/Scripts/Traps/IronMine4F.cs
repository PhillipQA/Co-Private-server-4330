namespace Conquer.Game.Scripts.Traps;

public static class IronMine4F
{
    [Trap(324)]
    public static void ClimbUpFromFloor4E(Player player) => player.Teleport(2004, 113, 155, force: true);

    [Trap(325)]
    public static void ClimbUpFromFloor4F(Player player) => player.Teleport(2004, 162, 111, force: true);

    [Trap(330)]
    public static void ClimbUpFromFloor4G(Player player) => player.Teleport(2005, 113, 155, force: true);

    [Trap(331)]
    public static void ClimbUpFromFloor4H(Player player) => player.Teleport(2005, 162, 111, force: true);

    [Trap(420)]
    public static void ClimbUpFromFloor4A(Player player) => player.Teleport(2002, 113, 155, force: true);

    [Trap(421)]
    public static void ClimbUpFromFloor4B(Player player) => player.Teleport(2002, 162, 111, force: true);

    [Trap(426)]
    public static void ClimbUpFromFloor4C(Player player) => player.Teleport(2003, 113, 155, force: true);

    [Trap(427)]
    public static void ClimbUpFromFloor4D(Player player) => player.Teleport(2003, 162, 111, force: true);
}
