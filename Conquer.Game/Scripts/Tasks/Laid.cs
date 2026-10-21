namespace Conquer.Game.Scripts.Tasks;

public static class Laid
{
    [Task(150100)]
    public static async Task GuildTeleporter(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 3;

        await dialog.Confirm("Would you like to go back to Guild Area in Twin City? I will teleport our members there free and charge nonmembers 500.",
            "Please teleport me there.", "Just passing by.");

        if (npc.OwnerId == 0)
        {
            player.Inform("Invalid transport NPC.");
        }
        else if (npc.OwnerId == (player.GuildId ?? 0))
        {
            player.Teleport(1038, 350, 339);
        }
        else if (player.Spend(500))
        {
            player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(500);
            player.Teleport(1038, 350, 339);
        }
        else
        {
            dialog.Say("You do not have enough money to use Guild Teleporter.", "I see.");
        }
    }

    [Task(7521755)]
    public static async Task TeleportStone(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 159;

        if (player.PkPoints > 1000)
        {
            return;
        }

        var choice = await dialog.Ask("The TeleportStone uses the power of crystal to take you to any of the five cities. Its power is limited: it works only once.",
            "Twin City", "Phoenix Castle", "Ape Mountain", "Desert City", "Bird Island", "Remove the stone.");

        switch (choice)
        {
            case <= 4:
                uint mapId = choice switch { 0 => 1002, 1 => 1011, 2 => 1020, 3 => 1000, _ => 1015 };
                ushort x = choice switch { 0 => 429, 1 => 188, 2 => 565, 3 => 500, _ => 717 };
                ushort y = choice switch { 0 => 378, 1 => 264, 2 => 562, 3 => 650, _ => 571 };

                dialog.Require(player.PkPoints <= 1000);

                if (player.World.DeleteNpc(npc) && player.Teleport(mapId, x, y))
                {
                    player.ShowEffect("relive1");
                }
                break;

            case 5:
                if (player.PkPoints <= 1000)
                {
                    player.World.DeleteNpc(npc);
                }
                else
                {
                    player.Inform("Error: Failed to meet condition.");
                }
                break;
        }
    }
}
