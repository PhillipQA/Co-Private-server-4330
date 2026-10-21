namespace Conquer.Game.Scripts.Npcs;

public static class AncientCity
{
    [Npc(10043)]
    public static async Task SpaceMark(Player player, Dialog dialog)
    {
        if (player.Profession is not (Profession.WaterTaoist or Profession.WaterWizard or Profession.WaterMaster or
            Profession.WaterSaint))
        {
            dialog.Say("Only Water class can buy this teleport scroll.", "I see.");
            return;
        }

        if (await dialog.Ask("If you give me 100 silver, I can make a teleport scroll for you. You can use it to return here directly.",
            "Please make one for me.", "Just passing by.") == 0)
        {
            await MakeScroll();
        }

        async Task MakeScroll()
        {
            if (!player.Spend(100))
            {
                dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                return;
            }

            if (player.Give(1060037) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(600100)]
    public static async Task Alcoholist(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (!player.Has(723030))
        {
            await dialog.Confirm("Great people talk about ideas, average talk about things, and small people talk about wine.", "It sounds true.");

            await dialog.Confirm("Thanks. I have been guarding Mystic Cave for ages. When I am at leisure, I will write poems and drink some wine.",
                "What is Mystic Cave?");

            if (await dialog.Ask("You`d better not enter it, it is dangerous. There are ferocious DevilKings of level 120 and 122. So I guard here,\nin case people brush in mistakenly.",
                "I can deal with them.", "Luckily, I did not get in.") == 0)
            {
                dialog.Say("It is my duty, I won`t let you in.", "I see.");
            }

            return;
        }

        await dialog.Confirm("Wow, your wine is so spicy. It must be fine wine. Can I have a look?", "Here you are.", "Sorry, you can`t.");

        await dialog.Confirm("It is HealthWine. My favorite. It would be much appreciated if you let me have the wine.", "Well...");

        await dialog.Confirm("Do you have any requirements? Please tell me frankly.", "Can I enter Mystic Cave?", "No, I will keep the wine.");

        await dialog.Confirm("Nothing is left except for poems and wine. You can go there if you like.", "Thanks a lot.");

        if (player.Take(723030))
        {
            player.Record(1001, 673, 345);
            player.Teleport(1300, 312, 646);
        }
    }
}
