namespace Conquer.Game.Scripts.Npcs;

public static class Labyrinth
{
    [Npc(1153)]
    public static async Task EastGeneral(Player player, Dialog dialog)
    {
        dialog.Avatar = 31;

        switch (await dialog.Ask("I am EastGeneral. This is the first floor of Labyrinth. If you want to enter the next floor, you should give me a\nSkyToken. Or do you want to go back to Twin City?",
            "To the next floor.", "To Twin City.", "I will stay here."))
        {
            case 0:
                if (!player.Has(721537))
                {
                    dialog.Say("You haven`t got a SkyToken, I cannot teleport you there.", "I will stay here.");
                    return;
                }

                await dialog.Confirm("The next floor is more dangerous. You should be more careful.", "Thank you very much.");

                if (player.Take(721537) && player.Teleport(1352, 27, 222))
                {
                    player.Record(1002, 430, 380);
                }
                break;

            case 1:
                player.Teleport(1002, 430, 380, record: (1002, 430, 380));
                break;
        }
    }

    [Npc(1154)]
    public static async Task WestGeneral(Player player, Dialog dialog)
    {
        dialog.Avatar = 31;

        switch (await dialog.Ask("I am WestGeneral. This is the second floor of Labyrinth. If you want to enter the next floor, you should give me an\nEarthToken, or would you like to go back to Twin City?",
            "To the next floor.", "To Twin City.", "I will stay here."))
        {
            case 0:
                if (!player.Has(721538))
                {
                    dialog.Say("You don`t have an EarthToken. I cannot teleport you to the next floor.",
                        "I will stay here.");
                    return;
                }

                await dialog.Confirm("The next floor is more dangerous. You should be more careful.", "Thank you very much.");

                if (player.Take(721538) && player.Teleport(1353, 28, 270))
                {
                    player.Record(1002, 430, 380);
                }
                break;

            case 1:
                player.Teleport(1002, 430, 380, record: (1002, 430, 380));
                break;
        }
    }

    [Npc(1155)]
    public static async Task SouthGeneral(Player player, Dialog dialog)
    {
        dialog.Avatar = 31;

        switch (await dialog.Ask("I am SouthGeneral. This is the third floor of Labyrinth. If you want to enter the next floor, you should give me a\nSoulToken. Or would you like to go back to Twin City?",
            "To the next floor.", "To Twin City.", "I will stay here."))
        {
            case 0:
                if (!player.Has(721539))
                {
                    dialog.Say("You don`t have a SoulToken, I cannot teleport you there.", "I will stay here.");
                    return;
                }

                await dialog.Confirm("The next floor is more dangerous. You should be more careful.", "Thank you very much.");

                if (player.Take(721539) && player.Teleport(1354, 8, 289))
                {
                    player.Record(1002, 430, 380);
                }
                break;

            case 1:
                player.Teleport(1002, 430, 380, record: (1002, 430, 380));
                break;
        }
    }

    [Npc(1156)]
    public static async Task NorthGeneral(Player player, Dialog dialog)
    {
        dialog.Avatar = 31;

        await dialog.Confirm("I am NorthGeneral. This is the fourth floor. If you want to go back to Twin City. I can help you.",
            "To Twin City.", "I will stay here.");

        player.Teleport(1002, 430, 380, record: (1002, 430, 380));
    }
}
