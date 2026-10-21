namespace Conquer.Game.Scripts.Npcs;

public static class NewDesert
{
    [Npc(30168)]
    public static async Task Yougo(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.Has(721250))
        {
            dialog.Say("What else can I do for you?", "No, thanks.");
        }
        else if (player.Has(721253))
        {
            await dialog.Confirm("Roy is really smart. I will tell you the truth. It is me who killed the dartists and stole the jade.",
                "Surrender your PeaceJade.");

            await dialog.Confirm("But BladeGhost stole my Kylin Jade and told me that if I killed the bodyguards and got the jade, he will return the KylinJade.",
                "I will get it for you.");

            await dialog.Confirm("If you can find out the Kylin Jade for me, I will surrender.", "Here is the Kylin Jade.", "I will find it soon.");

            if (!player.Has(721254))
            {
                dialog.Say("You are kidding. Didn`t you say that you have taken the Kylin Jade with you?",
                    "I will get it for you.");
                return;
            }

            if (player.Take(721253) && player.Take(721254) && player.Give(721250) is not null)
            {
                dialog.Say("Thank you for finding the KylinJade back for me. Now take the PeaceJade to EscortChief.\nConsider my earnest resipiscence, please let me go home for a few days, is it ok?",
                    "It is ok.");
            }
        }
        else
        {
            dialog.Say("Sigh, when can I get back the Kylin Jade?", "Is JadeKylin valuable?");
        }
    }

    [Npc(300651)]
    public static async Task Explorer(Player player, Dialog dialog)
    {
        dialog.Avatar = 12;

        if (await dialog.Ask("This is the way to the dangerous areas. Are you sure you will go ahead?", "Yeah.",
            "Just passing by.") == 0)
        {
            player.Teleport(1205, 1351, 1198, force: true);
        }
    }
}
