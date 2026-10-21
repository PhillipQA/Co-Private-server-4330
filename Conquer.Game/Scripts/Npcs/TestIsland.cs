namespace Conquer.Game.Scripts.Npcs;

public static class TestIsland
{
    [Npc(300011)]
    public static async Task WarriorGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.Has(710016))
        {
            await dialog.Confirm("Since you have passed the test and obtained the Warrior Certificate, I will give you the Warrior Amulet.",
                "Thanks.");

            if (player.Take(710016))
            {
                player.Give(710011);
            }

            return;
        }

        await dialog.Confirm("To obtain the Warrior Amulet, you must enable PK mode to kill the WarriorDevil and give me the Warrior Cert. Are you ready?",
            "Yeah.", "Not yet.");

        if (player.Profession is Profession.Warrior or Profession.BrassWarrior or Profession.SilverWarrior or
            Profession.GoldWarrior or Profession.WarriorMaster)
        {
            player.Teleport(1052, 160, 291);
        }
        else
        {
            dialog.Say("Sorry, only Warrior can challenge this test.", "I see.");
        }
    }

    [Npc(300012)]
    public static async Task TrojanGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.Has(710017))
        {
            await dialog.Confirm("Since you have passed the test and obtained the Trojan Cert. I will give you the Trojan Amulet.", "Thanks.");

            if (player.Take(710017))
            {
                player.Give(710012);
            }

            return;
        }

        await dialog.Confirm("To obtain the Trojan Amulet, you must enable PK mode to kill the TrojanDevil and give me the Trojan Cert. Are you ready?",
            "Yeah.", "Not yet.");

        if (player.Profession is Profession.Trojan or Profession.VeteranTrojan or Profession.TigerTrojan or
            Profession.DragonTrojan or Profession.TrojanMaster)
        {
            player.Teleport(1052, 71, 172);
        }
        else
        {
            dialog.Say("Sorry, only Trojan can challenge this test.", "I see.");
        }
    }

    [Npc(300013)]
    public static async Task FireGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.Has(710018))
        {
            await dialog.Confirm("Since you have passed the test and obtained the Fire Certificate, I will give you the Fire Amulet.", "Thanks.");

            if (player.Take(710018))
            {
                player.Give(710013);
            }

            return;
        }

        await dialog.Confirm("To obtain the Fire Amulet, you must enable PK mode to kill the FireDevil and give me the Fire Cert. Are you ready for the test?",
            "Yeah.", "Not yet.");

        if (player.Profession is Profession.Taoist or Profession.FireTaoist or Profession.FireWizard or
            Profession.FireMaster or Profession.FireSaint)
        {
            player.Teleport(1052, 160, 63);
        }
        else
        {
            dialog.Say("Sorry, only FireTaoist can challenge this test.", "I see.");
        }
    }

    [Npc(300014)]
    public static async Task WaterGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.Has(710019))
        {
            await dialog.Confirm("Since you have passed the test and obtained the Water Certificate, I will give you the Water Amulet.", "Thanks.");

            if (player.Take(710019))
            {
                player.Give(710014);
            }

            return;
        }

        await dialog.Confirm("To obtain the WaterAmulet, you must enable PK mode to kill the WaterDevil and give me the Water Cert. Are you ready?",
            "Yeah.", "Not yet.");

        if (player.Profession is Profession.Taoist or Profession.WaterTaoist or Profession.WaterWizard or
            Profession.WaterMaster or Profession.WaterSaint)
        {
            player.Teleport(1052, 294, 162);
        }
        else
        {
            dialog.Say("Sorry, only WaterTaoist can challenge this test.", "I see.");
        }
    }

    [Npc(300015)]
    public static async Task ArcherGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.Has(710020))
        {
            await dialog.Confirm("Since you have passed the test and obtained the Archer Cert, I will give you the Archer Amulet.", "Thanks.");

            if (player.Take(710020))
            {
                player.Give(710015);
            }

            return;
        }

        await dialog.Confirm("To obtain the Archer Amulet, you must enable PK mode to kill the ArcherDevil and give me the Archer Cert. Are you ready?",
            "Yeah.", "Not yet.");

        if (player.Profession is Profession.Archer or Profession.EagleArcher or Profession.TigerArcher or
            Profession.DragonArcher or Profession.ArcherMaster)
        {
            player.Teleport(1052, 316, 313);
        }
        else
        {
            dialog.Say("Sorry, only Archer can challenge this test.", "I see.");
        }
    }
}
