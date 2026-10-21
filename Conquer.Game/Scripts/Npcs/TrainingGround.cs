namespace Conquer.Game.Scripts.Npcs;

public static class TrainingGround
{
    [Npc(600075)]
    public static async Task BoxerHuang(Player player, Dialog dialog)
    {
        dialog.Avatar = 84;

        if (await dialog.Ask("Although nothing will be consumed here, you cannot level up as fast as slaying the monsters. Shall I teleport you out?",
            "Yes, please.", "No, thanks.") == 0)
        {
            player.Recall();
        }
    }
}
