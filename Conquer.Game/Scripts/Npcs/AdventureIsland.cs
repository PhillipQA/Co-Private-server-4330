namespace Conquer.Game.Scripts.Npcs;

public static class AdventureIsland
{
    [Npc(30125)]
    public static async Task GreenSnake(Player player, Dialog dialog)
    {
        dialog.Avatar = 53;

        if (await dialog.Ask("Are you leaving the snake array?", "I had better come later.",
            "No. I never give up.") == 0)
        {
            player.Teleport(1015, 717, 577, force: true);
        }
    }
}
