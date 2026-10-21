namespace Conquer.Game.Scripts.Npcs;

public static class Grocery
{
    [Npc(10064)]
    public static async Task Tinter(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.Profession >= Profession.InternTaoist)
        {
            switch (await dialog.Ask("Welcome! Please wear the armors first. What armors do you want me to dye?", "Cap",
                "Robe"))
            {
                case 0:
                    await Cap();
                    break;

                case 1:
                    await Robe();
                    break;
            }
        }
        else if (player.Profession <= Profession.InternWarrior)
        {
            if (!player.Wears(ItemPosition.Armor))
            {
                dialog.Say("Sorry, you do not wear the armors you want me to dye. Please wear first.",
                    "Oh. I am sorry.");
                return;
            }

            switch (await dialog.Ask("There are seven colors for you to choose. You can try all the colors for no more charges. What color do you like best?",
                "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"))
            {
                case 0:
                    await Color1();
                    break;

                case 1:
                    await Color2();
                    break;

                case 2:
                    await Color3();
                    break;

                case 3:
                    await Color4();
                    break;

                case 4:
                    await Color5();
                    break;

                case 5:
                    await Color6();
                    break;

                case 6:
                    await Color7();
                    break;
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            switch (await dialog.Ask("Welcome! Please wear the equipment you like to dye. What do you want me to dye?",
                "Helmet", "Armor", "Shield"))
            {
                case 0:
                    await Cap();
                    break;

                case 1:
                    await Robe();
                    break;

                case 2:
                    if (!player.Wears(ItemPosition.Left))
                    {
                        dialog.Say("Sorry, you do not hold a shield. Please equip your shield first.",
                            "Oh. I am sorry.");
                        return;
                    }

                    var choice = await dialog.Ask("There are seven colors for you to choose. You can try all the colors for no more charges. What color do you like best?",
                        "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7");

                    if (player.Dye(ItemPosition.Left, choice + 3))
                    {
                        dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                            "Oh. I am sorry.");
                    }
                    break;
            }
        }
        else
        {
            switch (await dialog.Ask("Welcome! Please wear the armors first. What armors do you want me to dye?", "Cap",
                "Coat"))
            {
                case 0:
                    await Cap();
                    break;

                case 1:
                    await Robe();
                    break;
            }
        }

        async Task Cap()
        {
            if (!player.Wears(ItemPosition.Head))
            {
                dialog.Say("Please wear the helmets you want me to dye first.", "Oh. I am sorry.");
                return;
            }

            var choice = await dialog.Ask("There are seven colors for you to choose. You can try all the colors for no more charges. What color do you like best?",
                "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7");

            if (player.Dye(ItemPosition.Head, choice + 3))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Robe()
        {
            if (!player.Wears(ItemPosition.Armor))
            {
                dialog.Say("Sorry, you do not wear the armors you want me to dye. Please wear first.",
                    "Oh. I am sorry.");
                return;
            }

            switch (await dialog.Ask("There are seven colors for you to choose. You can try all the colors for no more charges. What color do you like best?",
                "Color 1", "Color 2", "Color 3", "Color 4", "Color 5", "Color 6", "Color 7"))
            {
                case 0:
                    await Color1();
                    break;

                case 1:
                    await Color2();
                    break;

                case 2:
                    await Color3();
                    break;

                case 3:
                    await Color4();
                    break;

                case 4:
                    await Color5();
                    break;

                case 5:
                    await Color6();
                    break;

                case 6:
                    await Color7();
                    break;
            }
        }

        async Task Color1()
        {
            if (player.Dye(ItemPosition.Armor, 3))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Color2()
        {
            if (player.Dye(ItemPosition.Armor, 4))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Color3()
        {
            if (player.Dye(ItemPosition.Armor, 5))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Color4()
        {
            if (player.Dye(ItemPosition.Armor, 6))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Color5()
        {
            if (player.Dye(ItemPosition.Armor, 7))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Color6()
        {
            if (player.Dye(ItemPosition.Armor, 8))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }

        async Task Color7()
        {
            if (player.Dye(ItemPosition.Armor, 9))
            {
                dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                    "Oh. I am sorry.");
            }
        }
    }
}
