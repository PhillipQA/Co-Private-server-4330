namespace Conquer.Game.Scripts.Npcs;

public static class GlobeIsland
{
    [Npc(300653)]
    public static async Task Grandpa(Player player, Dialog dialog)
    {
        dialog.Avatar = 12;

        await dialog.Confirm("You are on the off-island. If you want to leave this island, the Boatman near the in-island (381,32) may help you.",
            "Thanks.", "Just passing by.");

        if (await dialog.Ask("The monsters on the in-island are very ferocious. Please be careful.",
            "Teleport me to in-island.") == 0)
        {
            player.Teleport(1219, 1016, 1292, force: true);
        }
    }
}
