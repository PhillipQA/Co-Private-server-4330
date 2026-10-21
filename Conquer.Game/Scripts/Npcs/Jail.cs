namespace Conquer.Game.Scripts.Npcs;

public static class Jail
{
    [Npc(140)]
    public static async Task Warden(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("What is the matter? If there is nothing special, do not disturb me.", "Can you let me out?", "Just passing by.");

        if (!player.World.Now.IsWeekly(DayOfWeek.Friday, new(14, 0), DayOfWeek.Friday, new(23, 59)) &&
            !player.World.Now.IsWeekly(DayOfWeek.Saturday, new(0, 0), DayOfWeek.Saturday, new(19, 59)))
        {
            if (await dialog.Ask("It is time for general pardon. You may leave now.", "Thanks.",
                "I shall stay here.") == 0)
            {
                await Thanks();
            }

            return;
        }

        if (!player.World.Now.IsHourly(0, 5) && !player.World.Now.IsHourly(30, 35))
        {
            dialog.Say("Calm down and stay here. Learn what peace and love is. I will let you out later.", "I see.");
            return;
        }

        if (await dialog.Ask("It is time for general pardon. You may leave now.", "Thanks.", "I shall stay here.") == 0)
        {
            await Thanks();
        }

        async Task Thanks()
        {
            player.Teleport(1002, 430, 380, force: true);
        }
    }
}
