namespace Conquer.Game.Scripts.Npcs;

public static class GlobeExit
{
    [Npc(300654)]
    public static async Task Boatman(Player player, Dialog dialog)
    {
        dialog.Avatar = 12;

        if (await dialog.Ask("I am here to lead the people out of the island. Are you leaving the island.", "Yeah.",
            "Just passing by.") == 0)
        {
            player.Teleport(1213, 448, 272, force: true);
        }
    }
}
