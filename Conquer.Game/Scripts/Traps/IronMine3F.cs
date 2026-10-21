namespace Conquer.Game.Scripts.Traps;

public static class IronMine3F
{
    [Trap(312)]
    public static void ClimbUpFromFloor3A(Player player) => player.Teleport(2000, 113, 155, force: true);

    [Trap(313)]
    public static void ClimbUpFromFloor3B(Player player) => player.Teleport(2000, 162, 111, force: true);

    [Trap(318)]
    public static void ClimbUpFromFloor3C(Player player) => player.Teleport(2001, 113, 155, force: true);

    [Trap(319)]
    public static void ClimbUpFromFloor3D(Player player) => player.Teleport(2001, 162, 111, force: true);

    [Trap(396)]
    public static void DescendToFloor4A(Player player) => player.Teleport(2006, 72, 15, force: true);

    [Trap(397)]
    public static void DescendToFloor4B(Player player) => player.Teleport(2007, 15, 77, force: true);

    [Trap(402)]
    public static void DescendToFloor4C(Player player) => player.Teleport(2008, 72, 15, force: true);

    [Trap(403)]
    public static void DescendToFloor4D(Player player) => player.Teleport(2009, 15, 77, force: true);

    [Trap(408)]
    public static void DescendToFloor4E(Player player) => player.Teleport(2010, 72, 15, force: true);

    [Trap(409)]
    public static void DescendToFloor4F(Player player) => player.Teleport(2011, 15, 77, force: true);

    [Trap(414)]
    public static void DescendToFloor4G(Player player) => player.Teleport(2012, 72, 15, force: true);

    [Trap(415)]
    public static void DescendToFloor4H(Player player) => player.Teleport(2013, 15, 77, force: true);
}
