namespace Conquer.Game.Scripts.Npcs;

public static class AFKerJail
{
    [Npc(420)]
    public static async Task Warden(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        await dialog.Confirm("What can I do for you?", "Please let me out.", "Just passing by.");

        if (!player.World.Now.IsHourly(0, 1) && !player.World.Now.IsHourly(30, 31))
        {
            dialog.Say("Sorry, I cannot release you. Please wait until it is time for half hourly pardon.", "I see.");
            return;
        }

        if (player.Teleport(1002, 430, 380, force: true))
        {
            dialog.Say("You can leave now. Please do not use macro or AFK processor any more, otherwise, your account may be deleted.",
                "I see.");
        }
    }
}
