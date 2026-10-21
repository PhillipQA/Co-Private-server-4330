namespace Conquer.Game.Scripts.Npcs;

public static class CentralPlain
{
    [Npc(8, 44, 4101, 10011, 10012, 10027, 10028)]
    public static async Task Warehouseman(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.CheckPin(""))
        {
            player.Inform("To secure your possessions, you are recommended to set a Warehouse Lock with WhsGuardian [409 392] in Twin City.");
            player.OpenDialog(4);
        }
        else if (player.GetRegister(0) == 2)
        {
            player.OpenDialog(4);
        }
        else if (player.GetRegister(1) >= 3)
        {
            dialog.Say("You have input the wrong passwords for three successive times. You must re-login if you wanna use the warehouse.",
                "That is ok.");
        }
        else
        {
            var input = await dialog.Input("You have created a password for your warehouse. Please input the password to open it.",
                "", 9, "Let me see.");
            await CheckPassword(input);
        }

        async Task CheckPassword(string input0)
        {
            if (player.CheckPin(input0))
            {
                player.SetRegister(0, 2);
                player.OpenDialog(4);
            }
            else if (player.GetRegister(1) == 0)
            {
                player.SetRegister(1, 1);

                if (await dialog.Ask("Wrong password. Enter again.", "Ok.", "Let me see.") == 0)
                {
                    await RetryPassword();
                }
            }
            else if (player.GetRegister(1) == 1)
            {
                player.SetRegister(1, 2);

                if (await dialog.Ask("Wrong password. Enter again.", "Ok.", "Let me see.") == 0)
                {
                    await RetryPassword();
                }
            }
            else if (player.GetRegister(1) == 2)
            {
                player.SetRegister(1, 3);

                dialog.Say("You have input the wrong passwords for three successive times. You must re-login if you wanna use the warehouse.",
                    "That is ok.");
            }
        }

        async Task RetryPassword()
        {
            if (player.GetRegister(1) >= 3)
            {
                dialog.Say("You have input the wrong passwords for three successive times. You must re-login if you wanna use the warehouse.",
                    "That is ok.");
                return;
            }

            var input = await dialog.Input("You have created a password for your warehouse. Please input the password to open it.",
                "", 9, "Let me see.");
            await CheckPassword(input);
        }
    }

    [Npc(43)]
    public static async Task CaptainLi(Player player, Dialog dialog)
    {
        dialog.Avatar = 2;

        await dialog.Confirm("What can I do for you?", "Visit the jail.", "Just passing by.");

        await dialog.Confirm("Give me 1000 silver, I will teleport you there. If your PK points are 100+, you will be put into the jail.",
            "Here are 1000 silver.", "If so, I will stay here.");

        if (player.Spend(1000))
        {
            player.Teleport(6000, 29, 72);
        }
    }

    [Npc(380)]
    public static async Task GuildController(Player player, Dialog dialog)
    {
        dialog.Avatar = 123;

        switch (await dialog.Ask("What can I do for you?", "Enter the guild area.", "Buy statue.", "Just passing by."))
        {
            case 0:
                if (player.World.Now.IsWeekly(DayOfWeek.Friday, new(14, 0), DayOfWeek.Friday, new(23, 59)) ||
                    player.World.Now.IsWeekly(DayOfWeek.Saturday, new(0, 0), DayOfWeek.Saturday, new(19, 59)))
                {
                    player.Teleport(1038, [(351, 341), (336, 279), (319, 282), (316, 324), (320, 357), (321, 372), (301, 373), (334, 341)]);
                }
                else
                {
                    player.Teleport(1038, 351, 341);
                }
                break;

            case 1:
                await dialog.Confirm("A statue costs 50,000 silver. Are you sure that you want to buy?", "Yeah.", "No, thanks.");

                if (player.Money < 50000)
                {
                    dialog.Say("Sorry, you do not have 50000 silver.", "I see.");
                }
                else if (player.Give(720020) is not null)
                {
                    player.Spend(50000);
                }
                break;
        }
    }

    [Npc(1061)]
    public static async Task WarehouseGuardian(Player player, Dialog dialog)
    {
        dialog.Avatar = 4;

        if (!player.CheckPin(""))
        {
            if (await dialog.Ask("You have created a password for your warehouse successfully. If you wanna change or cancel the password, just come to me.",
                "I wanna change it.", "OK. Thanks.") == 0)
            {
                await ChangePassword();
            }

            return;
        }

        if (await dialog.Ask("To protect your property, I recommend you to create a password for your warehouse. So every time you login, you\nmust check password with Warehouseman in order to use it. If you wanna change or cancel the password, just come to me.",
            "I want to set a password.", "Let me see.") == 0)
        {
            await SetPassword();
        }

        async Task SetPassword()
        {
            var input = await dialog.Input("A password must consist of numbers up to 9 bytes, and the first digit could not be 0. If you enter a character other\nthan numbers, only the valid numbers before it will work as the password. Please check carefully before you enter the password.",
                "", 9, "Let me think.");
            await VerifyPassword(input);
        }

        async Task ChangePassword()
        {
            if (player.GetRegister(1) >= 3)
            {
                dialog.Say("You have input the wrong passwords for three successive times. You must re-login if you wanna use the warehouse.",
                    "OK.");
                return;
            }

            var input = await dialog.Input("Please enter your old password in order to change it.",
                "", 9, "Let me think.");

            if (player.CheckPin(input))
            {
                switch (await dialog.Ask("Do you want to change the password or cancel it?", "Change it.",
                    "Cancel it."))
                {
                    case 0:
                        await SetPassword();
                        break;

                    case 1:
                        if (player.SetPin(input))
                        {
                            dialog.Say("You have canceled the password to your warehouse successfully. If you want, you can still come to me to set password.",
                                "OK. Thanks.");
                        }
                        break;
                }
            }
            else if (player.GetRegister(1) == 0)
            {
                player.SetRegister(1, 1);

                if (await dialog.Ask("Wrong password. Please enter again.", "OK.", "Let me think.") == 0)
                {
                    await ChangePassword();
                }
            }
            else if (player.GetRegister(1) == 1)
            {
                player.SetRegister(1, 2);

                if (await dialog.Ask("Wrong password. Please enter again.", "OK.", "Let me think.") == 0)
                {
                    await ChangePassword();
                }
            }
            else if (player.GetRegister(1) == 2)
            {
                player.SetRegister(1, 3);

                dialog.Say("You have input the wrong passwords for three successive times. You must re-login if you wanna use the warehouse.",
                    "OK.");
            }
        }

        async Task VerifyPassword(string input0)
        {
            if (!player.SetPin(input0))
            {
                return;
            }

            var input = await dialog.Input("Please verify your password. If you do not verify it, then I will take in your last password.",
                "", 9, null);

            if (player.CheckPin(input))
            {
                dialog.Say("You have created a password for your warehouse successfully. Please keep it safe. If you forget it, you must ask GMs for help.",
                    "Thank you.");
            }
            else
            {
                if (await dialog.Ask("Your passwords do not match. Please enter again. Otherwise I will take in your first password.",
                    "Alright.") != 0)
                {
                    return;
                }

                if (!player.SetPin(input))
                {
                    return;
                }

                var input2 = await dialog.Input("A password must consist of numbers up to 9 bytes, and the first digit could not be 0. If you enter a character other\nthan numbers, only the valid numbers before it will work as the password. Please check carefully before you enter the password.",
                    "", 9, "Let me think.");
                await VerifyPassword(input2);
            }
        }
    }

    [Npc(1152)]
    public static async Task Simon(Player player, Dialog dialog)
    {
        dialog.Avatar = 63;

        switch (await dialog.Ask("Great rewards will attract many brave people. I am looking for brave people to help me take my patrimony back.\nCan you help me? The rewards are handsome.",
            "Please tell me more.", "What rewards?", "I got SunDiamonds.", "I got MoonDiamonds.", "I got StarDiamonds.",
            "I got CloudDiamonds."))
        {
            case 0:
                await dialog.Confirm("My ancestors built a Labyrinth long before. Many treasures were stored there like SunDiamonds, MoonDiamonds, \nStarDiamond and so on. But it was occupied by fiece monsters soon. They expelled our clansmen and kept the treasure.",
                    "It`s a pity.", "I have no interest.");

                await dialog.Confirm("I have always been here waiting for brave people to help me. Of course I can`t trust in those who do not\nhave 2000 virtue points.",
                    "How about me?", "Sorry. That is too tough for me.");

                if (player.Virtue < 2000)
                {
                    dialog.Say("Sorry, you don`t have enough virtue points.", "Sigh...");
                    return;
                }

                await dialog.Confirm("Great. You are kind-hearted. I believe you are able to help me out. Let me tell you something about the Labyrinth.\nThen you can have a good preparation.",
                    "Thank you.");

                await dialog.Confirm("The Labyrinth contains 4 floors guarded by different monsters. If you can find the treasures and give them to me, I will reward",
                    "Please continue.");

                await dialog.Confirm("There are 3 kinds of monsters on each floor. The more monsters, the weaker they are. The ones in lesser\nquantities are strong. The fiercest monsters are always moving around. And they drop different tokens.",
                    "What do they drop?");

                await dialog.Confirm("SkyToken, EarthToken and SoulToken you need to enter the next floor. While the weak monsters usually drop treasure.\nAfter You get a token, find a general who will send you to the next floor. Some boss monsters drop rare items.",
                    "I see. Thanks.", "I must leave now.");

                await dialog.Confirm("Let me teleport you there, but I will deduct 2000 of your virtue points. Are you ready?", "Yeah.");

                if (player.SpendVirtue(2000))
                {
                    player.Teleport(1351, 16, 128, record: (1002, 430, 380));
                }
                else
                {
                    dialog.Say("Sorry, you don`t have enough virtue points.", "Sigh...");
                }
                break;

            case 1:
                if (await dialog.Ask("SunDiamond, MoonDiamond, StarDiamond and CloudDiamond are kept by different monsters.\nIf You get them for me, I will give you some rewards.",
                    "What rewards?") == 0)
                {
                    dialog.Say("2 meteors for 17 SunDiamonds, 4 meteors for 17 MoonDiamonds, a normal gem for 17 StarDiamonds and a AncestorBox\nfor 17 CloudDiamonds. If you are lucky enough, you will get a big surprise from the box.",
                        "I see. Thank you.");
                }
                break;

            case 2:
                if (!player.Has(721533, 17))
                {
                    dialog.Say("You are kidding on me. How dare you come here to claim prize with so few SunDiamonds?",
                        "Wait. I will get more.");
                    return;
                }

                if (player.Take(721533, 17) && player.Give(ItemType.Meteor, 2))
                {
                    dialog.Say("Well done. Here is the rewards I promised to you.", "Thanks.");
                }
                break;

            case 3:
                if (!player.Has(721534, 17))
                {
                    dialog.Say("You are kidding on me. How dare you come here to claim prize with so few MoonDiamonds?",
                        "Wait. I will get more.");
                    return;
                }

                if (player.Take(721534, 17) && player.Give(ItemType.Meteor, 4))
                {
                    dialog.Say("Well done. Here is the rewards I promised to you.", "Thanks.");
                }
                break;

            case 4:
                if (!player.Has(721535, 17))
                {
                    dialog.Say("You are kidding on me. How dare you come here to claim prize with so few StarDiamonds?",
                        "Wait. I will get more.");
                    return;
                }

                player.Take(721535, 17);

                var itemTypeId = player.Pick<uint>(700051, 700061, 700031, 700041, 700001, 700011, 700021, 700051);

                if (player.Give(itemTypeId) is not null)
                {
                    dialog.Say("Well done. Here is the rewards I promised to you.", "Thanks.");
                }
                break;

            case 5:
                if (!player.Has(721536, 17))
                {
                    dialog.Say("You are kidding on me. How dare you come here to claim prize with so few CloudDiamonds?",
                        "Wait. I will get more.");
                    return;
                }

                if (player.Take(721536, 17) && player.Give(721540) is not null)
                {
                    dialog.Say("Well done. Here is the rewards I promised to you.", "Thanks.");
                }
                break;
        }
    }

    [Npc(1611)]
    public static async Task GeneralJudd(Player player, Dialog dialog)
    {
        dialog.Avatar = 95;

        if (player.Has(722515))
        {
            await dialog.Confirm("This Secret Command is of great importance to me. Please return it to me, I will give you ten Meteors as the rewards.",
                "Great! Take it.", "But I need it too.");

            if (player.FreeSlots < 9)
            {
                player.Inform("You do not have enough space in your bag.");
                return;
            }

            if (!player.Take(722515))
            {
                return;
            }

            if (player.Roll(200) < 1)
            {
                if (player.Give(ItemType.DragonBall) is not null)
                {
                    player.Inform("Congratulations! You got a Dragon Ball.");
                }
            }
            else if (player.Give(ItemType.Meteor, 10))
            {
                player.Inform("Congratulations! You got 10 Meteors.");
            }
        }
        else if (player.Has(722512))
        {
            if (player.Has(722513))
            {
                if (!player.Has(722514))
                {
                    await dialog.Confirm("Thanks a lot for retrieving those two precious items. Please take my awards.",
                        "Thank you.", "I`d rather keep them.");

                    if (!player.Take(722513) || !player.Take(722512) || player.Give(ItemType.Meteor) is null)
                    {
                        return;
                    }

                    if (player.Roll(10) < 6)
                    {
                        if (player.Earn(50000))
                        {
                            player.Inform("Congratulations! You got 50000 silvers.");
                            player.Inform("Congratulations! You got a Meteor.");
                        }
                    }
                    else if (player.Give(ItemType.Meteor) is not null)
                    {
                        player.Inform("Congratulations! You got 2 Meteors.");
                    }

                    return;
                }

                await dialog.Confirm("Those three items ware awarded by the King. Were they not retrieved, we would be sentenced to death. Please give\nthem to me. I will repay your kindness with our precious treasures.",
                    "Here you are.", "I`d like to play for a while.");

                if (player.FreeSlots < 6)
                {
                    player.Inform("You do not have enough space in your bag.");
                    return;
                }

                if (!player.Take(722512) || !player.Take(722513) || !player.Take(722514))
                {
                    return;
                }

                if (player.Roll(200) < 1)
                {
                    if (player.Give(ItemType.DragonBall) is not null)
                    {
                        player.Inform("Congratulations! You got a Dragon Ball.");
                    }
                }
                else if (player.Give(ItemType.Meteor, 9))
                {
                    player.Inform("Congratulations! You got 9 Meteors.");
                }
            }
            else if (player.Has(722514))
            {
                await dialog.Confirm("Thanks a lot for retrieving those two precious items. Please take my awards.",
                    "Thank you.", "I`d rather keep them.");

                if (player.FreeSlots < 4)
                {
                    player.Inform("You do not have enough space in your bag.");
                    return;
                }

                if (!player.Take(722512) || !player.Take(722514) || !player.Give(ItemType.Meteor, 5))
                {
                    return;
                }

                if (player.Roll(10) < 6)
                {
                    if (player.Earn(20000))
                    {
                        player.Inform("Congratulations! You got 20000 silvers.");
                        player.Inform("Congratulations! You got 5 Meteors.");
                    }
                }
                else if (player.Give(ItemType.Meteor) is not null)
                {
                    player.Inform("Congratulations! You got 6 Meteors.");
                }
            }
            else
            {
                await dialog.Confirm("This AsterNecklace is of great importance to me. Please return it to me, I will pay you.",
                    "Ok, just take it.", "Wait. I wll get the other two.");

                if (!player.Take(722512))
                {
                    return;
                }

                if (player.Roll(10) < 6)
                {
                    if (player.Earn(50000))
                    {
                        player.Inform("Congratulations! You got 50000 silvers.");
                    }
                }
                else if (player.Give(ItemType.Meteor) is not null)
                {
                    player.Inform("Congratulations! You got a Meteor.");
                }
            }
        }
        else if (player.Has(722513))
        {
            if (!player.Has(722514))
            {
                await dialog.Confirm("The picture was awarded by the king. Please return it to me, I will send you a meteor as a token of my gratitude.",
                    "Ok, here you are.", "Wait. I wll get the other two.");

                if (player.Take(722513) && player.Give(ItemType.Meteor) is not null)
                {
                    player.Inform("Congratulations! You got a Meteor.");
                }

                return;
            }

            await dialog.Confirm("Thanks a lot for retrieving those two precious items. Please take my awards.",
                "Thank you.", "I`d rather keep them.");

            if (player.FreeSlots < 4)
            {
                player.Inform("You do not have enough space in your bag.");
                return;
            }

            if (player.Take(722513) && player.Take(722514) && player.Give(ItemType.Meteor, 6))
            {
                player.Inform("Congratulations! You got 6 Meteors.");
            }
        }
        else if (player.Has(722514))
        {
            await dialog.Confirm("This RoyalSword is of great importance to the country. Please give it to me, I will send you 5 meteors as a token of gratitude.",
                "Ok, take it.", "Wait. I wll get the other two.");

            if (player.FreeSlots < 4)
            {
                player.Inform("You do not have enough space in your bag.");
                return;
            }

            if (player.Take(722514) && player.Give(ItemType.Meteor, 5))
            {
                player.Inform("Congratulations! You got 5 Meteors.");
            }
        }
        else
        {
            await dialog.Confirm("Sigh! The damn thieves stole the most precious items of the General Mansion: AsterNecklace,PinetumPicture,Royal\nSword, together with the SecretCommand sent by the king. We have sent soldiers to trace the lost items.",
                "Did you get any clues?", "Poor guy.");

            await dialog.Confirm("Yeah. The thieves were captured. But we did not find out the lost items. They finally confessed that those items\nwere stolen by Blue Mouse when they passed the forest. Nobody ever saw a Blue Mouse except OldMiner outside the forest mine.",
                "Why not ask him?", "Oh, I see.");

            await dialog.Confirm("I did. But he does not know either. The King promises big rewards for those who can retrieve the lost items. Do you\nwant to give it a try?",
                "Sure. I do.", "I am quite busy now.");

            if (await dialog.Ask("Good! OldMiner has a kind of special needle to catch the mouse. Hope you find out the lost items soon.",
                "Ok. I will visit OldMiner now.", "Sorry that I can`t help you.") == 0)
            {
                dialog.Say("Thank you very much for you kindness. For any treasure you retrieve, I will repay you something. Good luck.",
                    "Thanks. Bye.");
            }
        }
    }

    [Npc(10002)]
    public static async Task Barber(Player player, Dialog dialog)
    {
        dialog.Avatar = 111;

        if (player.PkPoints > 1000)
        {
            return;
        }

        switch (await dialog.Ask("Now I can offer two types of hairstyles: New styles and nostalgic styles. Would you like to cost 500 silvers to make a change?",
            "New styles.", "Nostalgic styles.", "Keep my current style."))
        {
            case 0:
                await NewStyles();
                break;

            case 1:
                dialog.Require(player.PkPoints <= 1000);

                var choice = await dialog.Ask("Which style would you like to select from?", "Nostalgic01",
                    "Nostalgic02", "Nostalgic03", "Nostalgic04", "Nostalgic05", "Nostalgic06", "Bald head",
                    "I chanded my mind.");

                if (choice > 6)
                {
                    return;
                }

                int hair = choice switch { 0 => 10, 1 => 11, 2 => 12, 3 => 13, 4 => 14, 5 => 15, _ => 0 };

                dialog.Require(player.PkPoints <= 1000);

                if (!player.Spend(500))
                {
                    dialog.Say("You have not enough money.", "I`m sorry.");
                    return;
                }

                player.SetHair((ushort)(player.Hair - player.Hair % 100 + hair));
                player.PlaySound("sound/gethp.wav");
                player.ShowEffect("Health");

                if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool.",
                    "I want to change it.") == 1)
                {
                    await ChangeNostalgicStyle();
                }
                break;
        }

        async Task NewStyles()
        {
            dialog.Require(player.PkPoints <= 1000);

            var choice = await dialog.Ask("Which style would you like to select from?", "New HairStyle01",
                "New HairStyle02", "New HairStyle03", "New HairStyle04", "New HairStyle05", "New HairStyle06",
                "New HairStyle07", "Next.");

            switch (choice)
            {
                case <= 3 or 5 or 6:
                    int hair = choice switch { 0 => 30, 1 => 33, 2 => 34, 3 => 35, 5 => 37, _ => 38 };

                    dialog.Require(player.PkPoints <= 1000);

                    if (!player.Spend(500))
                    {
                        dialog.Say("You have not enough money.", "I`m sorry.");
                        return;
                    }

                    player.SetHair((ushort)(player.Hair - player.Hair % 100 + hair));
                    player.PlaySound("sound/gethp.wav");
                    player.ShowEffect("Health");

                    if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool!",
                        "I want to change it.") == 1)
                    {
                        await ChangeNewStyle();
                    }
                    break;

                case 4:
                    dialog.Require(player.PkPoints <= 1000);

                    if (!player.Spend(500))
                    {
                        return;
                    }

                    player.SetHair((ushort)(player.Hair - player.Hair % 100 + 36));
                    player.PlaySound("sound/gethp.wav");
                    player.ShowEffect("Health");

                    if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool!",
                        "I want to change it.") == 1)
                    {
                        await ChangeNewStyle();
                    }
                    break;

                case 7:
                    dialog.Require(player.PkPoints <= 1000);

                    var choice2 = await dialog.Ask("Which style would you like to select from?", "New HairStyle08",
                        "New HairStyle09", "Previous.");

                    switch (choice2)
                    {
                        case 0 or 1:
                            dialog.Require(player.PkPoints <= 1000);

                            if (!player.Spend(500))
                            {
                                dialog.Say("You have not enough money.", "I`m sorry.");
                                return;
                            }

                            player.SetHair((ushort)(player.Hair - player.Hair % 100 + (choice2 + 39)));
                            player.PlaySound("sound/gethp.wav");
                            player.ShowEffect("Health");

                            if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool!",
                                "I want to change it.") == 1)
                            {
                                await ChangeNewStyle();
                            }
                            break;

                        case 2:
                            await NewStyles();
                            break;
                    }
                    break;
            }
        }

        async Task ChangeNewStyle()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            var choice = await dialog.Ask("Which style would you like to change to?", "New HairStyle01",
                "New HairStyle02", "New HairStyle03", "New HairStyle04", "New HairStyle05", "New HairStyle06",
                "New HairStyle07", "Next.");

            switch (choice)
            {
                case <= 6:
                    int hair = choice switch { 0 => 30, 1 => 33, 2 => 34, 3 => 35, 4 => 36, 5 => 37, _ => 38 };

                    if (player.PkPoints > 1000)
                    {
                        player.Inform("Error: Failed to meet condition.");
                        return;
                    }

                    player.SetHair((ushort)(player.Hair - player.Hair % 100 + hair));
                    player.PlaySound("sound/gethp.wav");
                    player.ShowEffect("Health");

                    if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool!",
                        "I want to change it.") == 1)
                    {
                        await ChangeNewStyle();
                    }
                    break;

                case 7:
                    if (player.PkPoints > 1000)
                    {
                        player.Inform("Error: Failed to meet condition.");
                        return;
                    }

                    var choice2 = await dialog.Ask("Which style would you like to change to?", "New HairStyle08",
                        "New HairStyle09", "Previous.");

                    switch (choice2)
                    {
                        case 0 or 1:
                            if (player.PkPoints > 1000)
                            {
                                player.Inform("Error: Failed to meet condition.");
                                return;
                            }

                            player.SetHair((ushort)(player.Hair - player.Hair % 100 + (choice2 + 39)));
                            player.PlaySound("sound/gethp.wav");
                            player.ShowEffect("Health");

                            if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool!",
                                "I want to change it.") == 1)
                            {
                                await ChangeNewStyle();
                            }
                            break;

                        case 2:
                            await ChangeNewStyle();
                            break;
                    }
                    break;
            }
        }

        async Task ChangeNostalgicStyle()
        {
            dialog.Require(player.PkPoints <= 1000);

            var choice = await dialog.Ask("Which style would you like to change to?", "Nostalgic01", "Nostalgic02",
                "Nostalgic03", "Nostalgic04", "Nostalgic05", "Nostalgic06", "Bald head");

            int hair = choice switch { 0 => 10, 1 => 11, 2 => 12, 3 => 13, 4 => 14, 5 => 15, _ => 0 };

            dialog.Require(player.PkPoints <= 1000);

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + hair));
            player.PlaySound("sound/gethp.wav");
            player.ShowEffect("Health");

            if (await dialog.Ask("It`s completed. Are you satisfied with your new hairstyle?", "Cool.",
                "I want to change it.") == 1)
            {
                await ChangeNostalgicStyle();
            }
        }
    }

    [Npc(10003)]
    public static async Task GuildDirector(Player player, Dialog dialog, GuildService guilds)
    {
        dialog.Avatar = 7;

        switch (await dialog.Ask("I am in charge of all the guilds in Twin City. You may consult me for anything related to the guilds.",
            "Create a guild", "Disband my guild", "Donate money", "Pass my leadership", "Assign Deputy Guild Leader",
            "Remove sb. from office", "Inquire about a guild", "Others"))
        {
            case 0:
                if (player.Level < 90)
                {
                    dialog.Say("Sorry, you cannot create a guild before you reach level 90. Please train harder.",
                        "I see.");
                }
                else if (player.Money < 1000000)
                {
                    dialog.Say("To create a guild, 1,000,000 silver will be charged. You do not have the required money.",
                        "I see.");
                }
                else
                {
                    var input = await dialog.Input("What would you like to name your guild? A good name is better than great riches.",
                        "I name my guild...", 15, "Let me think it over.");

                    if (await guilds.CreateAsync(player, 90, 1000000, 500000, input))
                    {
                        dialog.Say("Congrats! You have created your guild successfully. I believe your guild will be expanded rapidly and enjoy a good reputation.",
                            "Thanks.");
                    }
                }
                break;

            case 1:
                if (await dialog.Ask("Your guild cannot be retrieved once you disband it. You had better think it over.",
                    "I decide to disband.", "I changed my mind.") == 0)
                {
                    await guilds.DisbandAsync(player);
                }
                break;

            case 2:
                var input2 = await dialog.Input("If a guild has enough fund, it may be expanded rapidly. How much would you like to donate?",
                    "I want to donate...", 8, "Let me think it over.");

                if (guilds.Donate(player, input2))
                {
                    dialog.Say("Thanks a lot for your donation. Your guild must be expanded rapidly.", "Thanks.");
                }
                else
                {
                    dialog.Say("You do not have the required money.", "I see.");
                }
                break;

            case 3:
                var input3 = await dialog.Input("Guild Leader bears heavy responsibilities. You had better pass your leadership to a competent candidate. Please think thrice be",
                    "I`ll pass my leadership to...", 15, "Let me think it over.");

                if (guilds.TransferLeadership(player, input3))
                {
                    dialog.Say("Congrats! You have passed your leadership successfully. I hope your guild could become more powerful.",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("You do not have this member.", "Thanks.");
                }
                break;

            case 4:
                var input4 = await dialog.Input("A talent Deputy Leader can help you a lot. Who do you want to appoint?",
                    "I appoint...", 15, "Let me think it over.");

                if (guilds.Promote(player, input4, GuildRank.DeputyLeader))
                {
                    dialog.Say("Congrats! You have assigned Deputy Leader successfully. I wish your guild could become more powerful.",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("You do not have this member.", "Thanks.");
                }
                break;

            case 5:
                var input5 = await dialog.Input("A good guild leader always rewards and punishes only those who really deserve. You had better appoint the best candidates.",
                    "I remove...from office", 15, "Let me think it over.");

                if (guilds.Promote(player, input5, GuildRank.Member))
                {
                    dialog.Say("Congrats! You have removed the target from office successfully. I wish your guild could become more powerful.",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("You do not have this member.", "Thanks.");
                }
                break;

            case 6:
                await dialog.Input("Which guild would you like to inquire about?",
                    "I want to inquire about...", 15, "Let me think it over.");
                await GuildList();
                break;

            case 7:
                switch (await dialog.Ask("I am in charge of all the guilds in Twin City. You may consult me for anything related to the guilds.",
                    "Hostile to a guild", "Reconcile with a guild", "Ally with a guild", "Un-ally with a guild",
                    "Guild List", "Online Members", "Expel Members", "others"))
                {
                    case 0:
                        var input = await dialog.Input("Kindness and enmity coexist in Kungfu circle. All guild members should be of one mind. Which guild do you want to hostile to?",
                            "We want to hostile to...", 15, "Let me think it over.");

                        if (await guilds.SetEnemyAsync(player, input, true))
                        {
                            dialog.Say("Congrats! Your guild is hostile to the target. I wish your guild could be more powerful.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("This guild does not exist.", "Thanks.");
                        }
                        break;

                    case 1:
                        var input6 = await dialog.Input("Better a lean peace than a fat victory. Which guild do you want to reconcile with?",
                            "We want to reconcile with...", 15, "Let me think it over.");

                        if (await guilds.SetEnemyAsync(player, input6, false))
                        {
                            dialog.Say("Congrats! You are reconciled with the target. I wish your guild could become more powerful.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("This guild does not exist.", "Thanks.");
                        }
                        break;

                    case 2:
                        await dialog.Confirm("Many hands make light work. Alliance will be a great help to your guild, but Guild Leaders must team up to ally.",
                            "We are ready for alliance.", "Let me think it over.");

                        if (await guilds.AddAllyAsync(player))
                        {
                            dialog.Say("Congrats! Your guild has allied with the target. I wish your guild could become more powerful.",
                                "Thanks.");
                        }
                        break;

                    case 3:
                        var input7 = await dialog.Input("If the allies cannot be of one mind, It makes no sense to ally. Which guild do you want to un-ally?",
                            "I want to un-ally with...", 15, "Let me think it over.");

                        if (await guilds.RemoveAllyAsync(player, input7))
                        {
                            dialog.Say("Congrats! You have unallied with the target successfully. I wish your guild could become more powerful.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("This guild does not exist.", "Thanks.");
                        }
                        break;

                    case 4:
                        await GuildList();
                        break;

                    case 5:
                        await GuildList();
                        break;

                    case 6:
                        var input8 = await dialog.Input("Which member would you like to expel?",
                            "Member Name", 15, "Let me think it over.");
                        await guilds.KickAsync(player, input8);
                        break;

                    case 7:
                        switch (await dialog.Ask("I am in charge of all the guilds in Twin City. You may consult me for anything related to the guilds.",
                            "Create Branch", "Assign Branch Manager", "Transfer fund.", "Others"))
                        {
                            case 0:
                                var input9 = await dialog.Input("What would you like to name your branch?",
                                    "I name my Branch...", 15, "Let me think it over.");

                                if (await guilds.CreateBranchAsync(player, 70, 500000, 500000, input9))
                                {
                                    dialog.Say("Congrats! You have created your branch successfully.", "Thanks.");
                                }
                                else
                                {
                                    dialog.Say("The Guild Leader and Branch Manager should party up first, 500,000 guild fund and Branch Manager level 70 or above required.",
                                        "I see.");
                                }
                                break;

                            case 1:
                                var input10 = await dialog.Input("What would you like to name your branch?",
                                    "I name my Branch...", 15, "Let me think it over.");

                                if (await guilds.ChangeBranchLeaderAsync(player, 70, input10))
                                {
                                    dialog.Say("Congrats! You have assigned the Branch Manager successfully.",
                                        "Thanks.");
                                }
                                else
                                {
                                    dialog.Say("To create a branch, the guild leader should team up with the branch manager who is level 70 or above.",
                                        "I see.");
                                }
                                break;

                            case 2:
                                if (player.ShownGuildRank is not (GuildRank.InternManager or GuildRank.BranchManager))
                                {
                                    dialog.Say("Sorry, only Branch Manager can tranfer fund to the guild.", "I see.");
                                    return;
                                }

                                await dialog.Input("You can transfer your branch`s fund to your guild, so that your guild will get more powerful.",
                                    "Guild Name", 15, "Let me think it over.");
                                await GuildList();
                                break;

                            case 3:
                                await GuildDirector(player, dialog, guilds);
                                break;
                        }
                        break;
                }
                break;
        }

        async Task GuildList()
        {
        }
    }

    [Npc(10021)]
    public static async Task ArenaGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        await dialog.Confirm("The arena is open. Welcome to challenge other people. The admission fee is only 50 silver.\nIf you PK in the arena, you will not gain or lose any experience or equipment, and you will get revived at the place you die.\nThe Kungfu circle is very dangerous, I suggest you PK in area.",
            "Enter the arena.", "Just passing by.");

        if (player.Spend(50))
        {
            player.Teleport(1005, 51, 68);
        }
        else
        {
            dialog.Say("Sorry, you do not have 50 silver.", "I see.", "Just passing by.");
        }
    }

    [Npc(10031)]
    public static async Task SpaceMarkSouthwest(Player player, Dialog dialog)
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

            if (player.Give(1060025) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10032)]
    public static async Task SpaceMarkNorthwest(Player player, Dialog dialog)
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

            if (player.Give(1060026) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10033)]
    public static async Task SpaceMarkSoutheast(Player player, Dialog dialog)
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

            if (player.Give(1060027) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10050)]
    public static async Task Conductress(Player player, Dialog dialog)
    {
        var choice = await dialog.Ask("Where are you heading for? I can teleport you for a price of 100 silver.",
            "Phoenix Castle", "Desert City", "Ape Mountain", "Bird Island.", "Mine Cave", "Market", "Just passing by.");

        switch (choice)
        {
            case <= 4:
                ushort x = choice switch { 0 => 958, 1 => 69, 2 => 555, 3 => 232, _ => 53 };
                ushort y = choice switch { 0 => 555, 1 => 473, 2 => 957, 3 => 190, _ => 399 };

                if (player.Spend(100))
                {
                    player.Teleport(1002, x, y);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;

            case 5:
                if (player.IsWanted)
                {
                    dialog.Avatar = 10;

                    await dialog.Confirm("You are the wanted criminal. It would be dangerous for me to teleport you to the market, so I shall charge you 1000 silver.",
                        "OK. Here you are.", "I changed my mind.");

                    if (player.Spend(1000))
                    {
                        player.Record(1002, 430, 380);
                        player.Teleport(1036, 211, 196);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have 1,000 silver.", "I see.");
                    }
                }
                else if (player.Spend(100))
                {
                    player.Record(1002, 430, 380);
                    player.Teleport(1036, 211, 196);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;
        }
    }

    [Npc(10054)]
    public static async Task GeneralPeace(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.Level >= 60)
        {
            if (await dialog.Ask("This is the way to the Desert City. Although you are excellent, it is dangerous to go ahead.",
                "I see.") == 0)
            {
                await PleaseTeleportMeThere();
            }

            return;
        }

        if (await dialog.Ask("This is the way to Desert City. The monsters there are much stronger than you. Are you sure you want to go there?",
            "No, I shall not go.", "Please teleport me there.") == 1)
        {
            await PleaseTeleportMeThere();
        }

        async Task PleaseTeleportMeThere()
        {
            player.Teleport(1000, 971, 666);
        }
    }

    [Npc(10060)]
    public static async Task Sunfer(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;
        var cursor = 0u;

        switch (await dialog.Ask("Everyone has secrets to hide and people they hate. If you want someone dead, but dare not kill them yourself, or you\ndon`t think it worth while, I`ll settle it for you. I charge you a minimum of 100,000 silvers. Tell me his/her name.\nIf you think your skill is fine and you need some money, I would like to hire you as a bounty hunter. You will gain\n70% of the bounty, and I get the rest. If you are the target of a bounty, I can also settle it if you pay me double bounty.",
            "Offer to kill an enemy.", "Accept an offer.", "Cancel an offer.", "Add reward."))
        {
            case 0:
                var input = await dialog.Input("Killing is not a easy job. However, many take risk to make a living. What is the name of your enemy? Don`t give me a wrong name",
                    "My enemy is:", 15, "I changed my mind.");

                if (input.Length == 0)
                {
                    return;
                }

                var input2 = await dialog.Input("How much do you offer? Do not be less than 100,000 silvers. Otherwise, nobody is interested at all",
                    "I offer", 7, "I changed my mind.");

                if (player.PostBounty(input, input2))
                {
                    dialog.Say("I have taken your offer. Please be assured that I will try to find a good bounty hunter for you.",
                        "Thanks.");
                }
                break;

            case 1:
                var answer = await dialog.Ask($"Offer     Target     Reward\n--------------------\n{player.ListBounties(ref cursor, 5)}\n----------------------------",
                    "I accept offer number", 7, "Next", "Close");

                switch (answer.Option)
                {
                    case 0:
                        await Next();
                        break;

                    case 2:
                        await AcceptOffer(answer.Input);
                        break;
                }
                break;

            case 2:
                var input3 = await dialog.Input("I can cancel an offer for double the reward money. Which offer do you want to cancel?",
                    "I want to cancel offer number", 7, "I changed my mind.");

                if (player.CancelBounty(input3))
                {
                    dialog.Say("Congrats! I have canceled this offer for you. Have a good day.", "Thanks.");
                }
                break;

            case 3:
                var input4 = await dialog.Input("Which offer number would you like to add your reward to?",
                    "The offer number is", 7, "I changed my mind.");

                if (player.FindBounty(input4) is null)
                {
                    return;
                }

                var input5 = await dialog.Input("How much would you like to increase? No less than 10,000 silvers each time.",
                    "I add", 7, "I changed my mind.");

                if (player.RaiseBounty(input4, input5))
                {
                    dialog.Say("You have offered a handsome reward. I am sure that many bounty hunters will come to accept your offer.",
                        "I hope so.");
                }
                break;
        }

        async Task Next()
        {
            var answer = await dialog.Ask($"Offer     Target     Reward\n--------------------\n{player.ListBounties(ref cursor, 5)}\n----------------------------",
                "I accept offer number", 7, "Next", "Close");

            switch (answer.Option)
            {
                case 0:
                    await Next();
                    break;

                case 2:
                    await AcceptOffer(answer.Input);
                    break;
            }
        }

        async Task AcceptOffer(string input0)
        {
            if (player.HuntBounty(input0))
            {
                dialog.Say("If you accept two or more offers, you can take only the last. Your offer will be canceled if you log out before it is finished.",
                    "I see.");
            }
            else
            {
                dialog.Say("Sorry, you cannot accept this offer. You have entered the wrong offer number or do not have the required 1000 silver.",
                    "I see.");
            }
        }
    }

    [Npc(10063)]
    public static async Task Shopboy(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        switch (await dialog.Ask("Our shop is famous for dyeing. If you want to have your equipment dyed, please wear them before you enter. You\nhave a wide choice of colors. One meteor will be charged before you try the colors. Do you want a try?",
            "Yes, here is a meteor.", "Can you dye my armor black?", "Is there any good wine in your restaurant?"))
        {
            case 0:
                if (player.Has(ItemType.Meteor))
                {
                    if (player.Take(ItemType.Meteor) && player.Teleport(1008, 22, 26))
                    {
                        player.Record(1002, 430, 380);
                    }
                }
                else
                {
                    dialog.Say("Sorry, you do not have a meteor.", "Oh. I am sorry.");
                }
                break;

            case 1:
                await dialog.Confirm("Sure. Once your armor is dyed black, it will never fade no matter how it is updated, unless I dye it to other\ncolors. I will charge 1 dragonball to do it. ",
                    "Yes. Here`s a Dragonball.", "Let me think it over.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragonball.", "Oh. I forgot it.");
                    return;
                }

                if (!player.Wears(ItemPosition.Armor))
                {
                    dialog.Say("Please make sure you are wearing any armor now.", "Oh, let me put it on first.");
                    return;
                }

                if (player.Worn(ItemPosition.Armor)?.Subtype is 137 or 132)
                {
                    player.Inform("Armors of this kind can not be dyed black.");
                    return;
                }

                player.Take(ItemType.DragonBall);

                if (player.Dye(ItemPosition.Armor, 2))
                {
                    dialog.Say("Do you like this color? If you are not satisfied, you may try other colors for free.",
                        "Oh. I am sorry.");
                }
                break;

            case 2:
                if (player.GetProgress(19, 5) < 1)
                {
                    dialog.Say("There are all kinds of wines prepared for you in our FlyingwithMoon Restaurant. Would you like to have a try?",
                        "Just passing by.");
                    return;
                }

                await dialog.Confirm("Good wine? Aha, don`t you know what is the best restaurant in Twin City, FlyingwithMoon Restaurant famous for? For its\nspeacial wine, DrunkCelestial. Generally speaking, the DrunkCelestail is for VIPs of the restaurant only. But, we\nshopboys sometimes can get a few bottles of DrunkCelestial. Do you want to buy some from me to have a try?\n",
                    "Give me one.", "No, thanks.");

                await dialog.Confirm("Give you one? No, no, do you know how precious is it? I tell you: I will not give you anything unless you give me 300 silvers.\n\n",
                    "Here are 300 silvers.", "What? 300 silvers!");

                if (player.Money < 300)
                {
                    dialog.Say("Don`t fool me! You don`t have 300 silvers at all.\n\n", "Just a joke.");
                }
                else if (player.FreeSlots > 0)
                {
                    if (player.Spend(300) && player.Give(722185) is not null)
                    {
                        player.Inform("Congratulations! You got a bottle of DrunkCelestial.");
                    }
                }
                else
                {
                    dialog.Say("Er...you don`t have enough space for a bottle of DrunkCelestial in your inventory. Do you mind if I keep it for\nyou? You may come here to bring it away whenever you have time and free slots in your inventory. Is it OK?\n",
                        "It`s Okay.");
                }
                break;
        }
    }

    [Npc(15140)]
    public static async Task Gouda(Player player, Dialog dialog)
    {
        dialog.Avatar = 54;

        if (await dialog.Ask("Kylin, a giant monster living in the jokul, is regarded as a holy creature. Its body is covered by the splendid squamas.\nIt`s said that good luck will befall whoever gets the squama. The other day, I happened to get this rare squama.\nI dare not keep it to myself. So I`m here to tell you this exciting discovery.",
            "Please kindly tell me.", "Forget it.") == 0)
        {
            dialog.Say("Every night after 8 o`clock, twinkling spots will appear on the ground, it is the squama. Pick it up and you will get a\nsecret present. But it depends on your luck to get better prize. You may get money, Meteor or a Dragon Ball. Wish you good luck",
                "Ok, thanks.");
        }
    }

    [Npc(20005)]
    public static async Task Celestine(Player player, Dialog dialog)
    {
        dialog.Avatar = 15;

        if (!player.Has(700001) || !player.Has(700011) || !player.Has(700021) || !player.Has(700031) ||
            !player.Has(700041) || !player.Has(700051) || !player.Has(700061))
        {
            if (await dialog.Ask("People are pursuing greater achievement during their lives, but none can make it due to the limit of human constitution.",
                "What does it mean?", "I don`t believe it.") == 0)
            {
                await WhatDoesItMean();
            }

            return;
        }

        if (!player.Has(721258))
        {
            dialog.Say("Although you get seven normal gems, I cannot refine the CelestialStone for you without the CleanWater. ",
                "I will get it ready.");
            return;
        }

        player.Take(700001);
        player.Take(700011);
        player.Take(700021);
        player.Take(700031);
        player.Take(700041);
        player.Take(700051);
        player.Take(700061);
        player.Take(721258);

        if (player.Give(ItemType.CelestialStone) is not null)
        {
            dialog.Say("The Celestial Stone is ready. Please take it to Eternity in SkyPass ,he will tell you more.",
                "Ok, Thank you.");
        }

        async Task WhatDoesItMean()
        {
            if (await dialog.Ask("Mortals are mundane. Only getting rid of it can help them accomplish their aims.\nIf you are high level enough, you can get reborn to learn more and stronger skills.",
                "I am satisfied.", "How to get reborn?") != 1)
            {
                return;
            }

            await dialog.Confirm("It is difficult. First, you should reach certain level. Second, you need a Celestial Stone.",
                "How to get CelestialStone?", "Forget it.");

            await dialog.Confirm("CelestialStone is made of 7 gems: VioletGem, KylinGem, RainbowGem, MoonGem, PhoenixGem, FuryGem, DragonGem, and CleanWater.",
                "What is Clean Water?", "It is difficult.");

            await dialog.Confirm("It is used to get rid of your mundaneness, and then you won`t be affected by the environment.\nBy the way, Clean Water comes from celestial rinsing.",
                "What are gems used for?");

            await dialog.Confirm("Only the seven gems can protect you during the rebirth.", "I will collect them now.", "I changed my mind.");

            await dialog.Confirm("It is easy to get the gems. But CleanWater...", "But what?");

            await dialog.Confirm("The Adventure island is the headstream of CleanWater. But it is occupied by WaterEvil and he uses spell to hide the stream.",
                "What can I do?", "I will give up.");

            if (await dialog.Ask("WaterEvilElder will go to get the water every certain time. If you defeat him, you may get the water.\nBut he is very hard to deal with.",
                "I see. Thank you.", "Anything else?") != 1)
            {
                return;
            }

            if (!player.Has(700001) || !player.Has(700011) || !player.Has(700021) || !player.Has(700031) ||
                !player.Has(700041) || !player.Has(700051) || !player.Has(700061))
            {
                dialog.Say("Come here to refine Celestial Stone when you get the seven gems and Clean Water.",
                    "That`s ok.");
                return;
            }

            if (!player.Has(721258))
            {
                dialog.Say("Although you get seven normal gems, I cannot refine the CelestialStone for you without the CleanWater. ",
                    "I will get it ready.");
                return;
            }

            player.Take(700001);
            player.Take(700011);
            player.Take(700021);
            player.Take(700031);
            player.Take(700041);
            player.Take(700051);
            player.Take(700061);
            player.Take(721258);

            if (player.Give(ItemType.CelestialStone) is not null)
            {
                dialog.Say("The Celestial Stone is ready. Please take it to Eternity in SkyPass ,he will tell you more.",
                    "Ok, Thank you.");
            }
        }
    }

    [Npc(30080)]
    public static async Task CoachLi(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.Has("Ref.Letter.Li"))
        {
            dialog.Say("Keep this in mind. Only when you reach level 15 can CaptainLi help you.", "I see.");
        }
        else if (player.Has("NameCard"))
        {
            if (await dialog.Ask("We call him General Cai. He lives outside Twin City. Take my name card with you. He may reward you some experience.",
                "I trust you.") == 0)
            {
                await ITrustYou();
            }
        }
        else if (player.Level < 16)
        {
            await dialog.Confirm("You seem to be new to the world of Conquer.", "Yes.");

            await dialog.Confirm("I am an elementary coach in Twin City. I suggest you practice outside Twin City, or you will be in danger.",
                "Thank you for your advice.", "I fear nothing.");

            if (await dialog.Ask("I give you a reference letter. When you reach level 16, you may get something useful from Coach Lin in Phoenix Castle.",
                "Thanks a lot.") == 0)
            {
                player.Give(721115);
            }
        }
        else if (player.Level < 31)
        {
            if (await dialog.Ask("The circle is beset with peril. You would come to grief if you are careless. The advice from others would be helpful to you.",
                "I am experienced.", "Who will help me?") != 1)
            {
                return;
            }

            if (await dialog.Ask("I have a friend who has been in circle for many years. If you give me 1000 silver, I can recommend you to him.",
                "Here is my money.", "You are cheating.") == 0)
            {
                await ITrustYou();
            }
        }
        else
        {
            dialog.Say("You are not newcomer. Go ahead. Hope you will be outstanding one day.", "I shall try my best.");
        }

        async Task ITrustYou()
        {
            if (player.Spend(1000))
            {
                player.Give(721116);
            }
            else
            {
                dialog.Say("Sorry, you do not have 1,000 silver.", "I see.");
            }
        }
    }

    [Npc(30082)]
    public static async Task GeneralCai(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (!player.Has("NameCard"))
        {
            dialog.Say("I have been in circle for many years and gained rich experience. I do not give my experience to others easily.",
                "You are too arrogant.");
            return;
        }

        if (player.Level < 16)
        {
            dialog.Say("Sorry, your level is too low. I may help you when you are level 16.", "I shall come later.");
        }
        else if (player.Level < 31)
        {
            await dialog.Confirm("Since Coach Li introduced you to me, I shall try to help you.", "Thanks, sir.");

            dialog.Require(player.Has("NameCard"));

            await dialog.Confirm("Please give me a Euxenite Ore before I reward you some experience.", "Here you are.");

            dialog.Require(player.Has("NameCard"));

            if (!player.Has(1072031))
            {
                dialog.Say("Sorry, you do not have a Euxenite Ore.", "I shall give you later.");
                return;
            }

            await dialog.Confirm("Since you are sincere, I shall give you 8,000 experience points.", "Thanks.");

            dialog.Require(player.Has("NameCard"));

            if (player.Take(1072031) && player.Take(721116))
            {
                player.GainExperience(8000);
            }
        }
        else
        {
            dialog.Say("You should have played Conquer for a while. I believe you would exceed me soon.", "I hope so.");
        }
    }

    [Npc(30084)]
    public static async Task GeneralQing(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.Has("ArmyToken"))
        {
            await dialog.Confirm("I never expected it was those bandits who robbed the Army Token. We are lucky to have your help. I shall reward you my sword.",
                "Thanks, sir.");

            dialog.Require(player.Has("ArmyToken"));

            if (player.Take(721117))
            {
                player.Give(420086);
            }

            return;
        }

        await dialog.Confirm("I have sent my soldiers out to fetch the monthly provisions with my army token, but I have received no news since they left.",
            "They must have troubles.", "All will be fine.");

        if (await dialog.Ask("That is what I am afraid of. I am worrying about the Army Token. If it is lost, I must be sentenced to death.",
            "I will try to find it.", "What a bad luck.") == 0)
        {
            dialog.Say("If you can bring the Army Token back to me, I will give you a handsome reward.",
                "I shall try my best.");
        }
    }

    [Npc(30085)]
    public static void FoodCarrier(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        dialog.Say("Something unexpected may happen at any time. We were attacked by monsters when we passed Phoenix Castle. I`m the only survivor.",
            "You were too careless.");
    }

    [Npc(30095)]
    public static async Task John(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.Has("Letter.John"))
        {
            dialog.Say("Please take care of it.", "I shall try my best.");
            return;
        }

        await dialog.Confirm("Hi, can you do me a favor?", "Sure.", "I am busy.");

        await dialog.Confirm("My brother Richard enjoys roaming around the city. Can you deliver this letter to him for me? He will reward you handsomely.",
            "OK.", "I am busy.");

        if (player.Give(721121) is not null)
        {
            dialog.Say("But he is an odd fellow. He will run away as soon as a stranger speaks to him. It is a little difficult to find him.",
                "I shall try my best!");
        }
    }

    [Npc(30096)]
    public static async Task Richard(Player player, Dialog dialog)
    {
        dialog.Avatar = 8;

        if (player.Money < 1 || player.Health < 93 || player.Roll(3) < 1)
        {
            var (x, y) = player.Pick<(ushort, ushort)>([(439, 290), (432, 330), (455, 394), (458, 375), (387, 363),
                (398, 357), (446, 221), (422, 295)]);
            player.World.MoveNpc(30096, 1002, x, y);

            dialog.Say("Do not bother me!", "Well.");
        }
        else if (player.Has(721121))
        {
            await dialog.Confirm("Oh, John sent you to find me? You see the situation is very bad to me due to the jewellery stolen case. What a pity.",
                "What did you mean?", "Everything is over. Bye.");

            await dialog.Confirm("Tell you a secret, I am the famous thief Richard. I have pried into the palace for a long time, but cannot find a chance.",
                "Guard! Here is a thief.", "I got to go.");

            if (!player.Take(721121))
            {
                return;
            }

            if (player.Roll(3) < 1)
            {
                player.AddHealth(-500);
                player.AddHealth(1);

                dialog.Say("How dare you reveal the inside story? I must teach you a lesson.", "You are a cruel guy.");
            }
            else if (player.Roll(2) < 1)
            {
                switch (await dialog.Ask("Be quiet. Here is 1000 silvers. Take it and do not say anything.",
                    "Guard! Guard!", "It is a deal."))
                {
                    case 0:
                        if (player.Roll(2) < 1)
                        {
                            dialog.Say("How dare you reveal the inside story? I must teach you a lesson.",
                                "You are a cruel guy.");
                        }
                        else
                        {
                            player.Tell("Justice overcomes evil. Thief Richard was put into the jail and you received a reward of 250 silvers.");

                            dialog.Say("Oh, my god! When can I be set free?", "You must pay for that.");
                        }
                        break;

                    case 1:
                        if (player.Roll(2) < 1)
                        {
                            player.Earn(1000);
                        }
                        else if (player.Spend(200))
                        {
                            player.Tell("You are too greedy and were cheated by Richard. He has stolen all your money.");

                            dialog.Say("Haha, you fool.", "You evil.");
                        }
                        else
                        {
                            player.AddHealth(-500);
                            player.AddHealth(1);

                            dialog.Say("It is ashamed for you to carry so little money with you. You just waste my time and I shall give you some suffering.",
                                "You will pay for that.");
                        }
                        break;
                }
            }
            else if (player.Earn(250))
            {
                player.Tell("Justice overcomes evil. Thief Richard was put into the jail and you received a reward of 250 silvers.");

                dialog.Say("Oh, my god! When can I be set free?", "You must pay for that.");
            }
        }
        else
        {
            if (await dialog.Ask("Money makes the mare to go.", "A moneygrubber.", "Man should not do so.") == 1)
            {
                dialog.Say("That is quite right.", "Yes.");
            }
        }
    }

    [Npc(30099)]
    public static async Task GeneralLong(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.Has("ToMountainKing"))
        {
            dialog.Say("Please take care of it.", "All right.");
        }
        else if (player.Level < 50)
        {
            dialog.Say("Do not bother me!", "Okay.");
        }
        else
        {
            await dialog.Confirm("You are very excellent. May I ask you for a help?", "What can I do?", "I am busy.");

            await dialog.Confirm("My brother Mountain King in Ape Mountain is in trouble, but I do not have soldiers to help him. Can you help him?",
                "I shall have a try.", "Let me think it over.");

            if (player.Give(721122) is not null)
            {
                dialog.Say("Take this letter with you, and he will know that you are on his side.", "Ok.");
            }
        }
    }

    [Npc(30106)]
    public static async Task David(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        await dialog.Confirm("My five brothers and I guard the arena in turn . We take pleasure in watching the PK Tournament and improving our skills.",
            "That is great!", "Just passing by.");

        await dialog.Confirm("My brothers and I want to watch the PK Tournament. but we are too busy to get some tickets from pheasants. Can you help us?",
            "Sure.", "Sorry. I am too busy.");

        await dialog.Confirm("Thanks a lot. If you give me 6 tickets, I shall pay you 6000 silvers. Are you ready to give your tickets to me?",
            "Yeah, here you are.", "Sorry, I need them, too.");

        if (!player.Has(710001, 6))
        {
            dialog.Say("Sorry, you do not have 6 tickets.", "I see.");
            return;
        }

        await dialog.Confirm("Then I will take away your six tickets and pay you 6000 silver.", "Thanks.");

        if (player.Take(710001, 6))
        {
            player.Earn(6000);
        }
    }

    [Npc(30110)]
    public static async Task MrLeisure(Player player, Dialog dialog)
    {
        dialog.Avatar = 56;

        if (player.Has(721131))
        {
            dialog.Say("He looks like the other blacksmith, but he is a weapon maker, while the other is a weapon retailer.",
                "I see.");
        }
        else if (player.Level < 40)
        {
            dialog.Say("I am always ready to help other guys.", "You are a kind man.");
        }
        else
        {
            await dialog.Confirm("My old friend, Blacksmith Li in Ape Mountain, needs help. Will you do him a favor?",
                "Sure.", "Sorry, I am too busy.");

            if (await dialog.Ask("Take my letter to him, and he will tell you more.", "OK.") == 0)
            {
                player.Give(721131);
            }
        }
    }

    [Npc(30112)]
    public static async Task GeneralChen(Player player, Dialog dialog)
    {
        dialog.Avatar = 37;

        if (player.Has(721133))
        {
            await dialog.Confirm("Well done! Since you are new to the world, I will send you a sword as a reward.", "Thank you.");

            if (player.Take(721133))
            {
                player.Give(410057);
            }
        }
        else if (player.Has(721134))
        {
            dialog.Say("He likes to travel around. If you cannot find him, you may ask his soldier for more information.",
                "I see.");
        }
        else if (player.Level < 41)
        {
            await dialog.Confirm("Hello! I have a quest for you to take. I am sure that you can finish it. Are you interested?",
                "Tell me more.", "No, thanks.");

            await dialog.Confirm("Here is an urgent mail for General Wu in Phoenix Castle. Can you help me to take this mail to him?",
                "Sure.", "Sorry, I am too busy.");

            if (player.Give(721134) is not null)
            {
                dialog.Say("Please bring me his letter in reply after you meet him. I shall give you a good reward.",
                    "I see.");
            }
        }
        else
        {
            dialog.Say("Quarrels and disputes come up from time to time. It seems that troubled times return.",
                "That is what I am coming for.");
        }
    }

    [Npc(30154)]
    public static async Task Luna(Player player, Dialog dialog)
    {
        dialog.Avatar = 116;

        if (player.World.Now.IsBetween(new(2006, 2, 28, 0, 0, 0), new(2006, 3, 2, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform1();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 3, 0, 0, 0), new(2006, 3, 5, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform2();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 6, 0, 0, 0), new(2006, 3, 8, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform3();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 9, 0, 0, 0), new(2006, 3, 11, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform4();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 12, 0, 0, 0), new(2006, 3, 14, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform5();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 15, 0, 0, 0), new(2006, 3, 17, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform6();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 18, 0, 0, 0), new(2006, 3, 20, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform7();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 21, 0, 0, 0), new(2006, 3, 23, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform8();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 24, 0, 0, 0), new(2006, 3, 26, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform9();
            }
        }
        else if (player.World.Now.IsBetween(new(2006, 3, 27, 0, 0, 0), new(2006, 3, 28, 23, 59, 0)))
        {
            if (await dialog.Ask("What can I do for you?", "Enter Moon Platform.", "Just passing by.") == 0)
            {
                await EnterMoonPlatform10();
            }
        }

        async Task EnterMoonPlatform1()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1100, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform2()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1101, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform3()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1102, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform4()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1103, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform5()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1104, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform6()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1105, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform7()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1106, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform8()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1107, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform9()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1108, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }

        async Task EnterMoonPlatform10()
        {
            await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
                "Teleport me there.", "I changed my mind.");

            if (player.Spend(300))
            {
                player.Teleport(1109, 187, 162, record: (1002, 439, 389));
            }
            else
            {
                dialog.Say("Sorry, you do not have 300 silver.", "I see.");
            }
        }
    }

    [Npc(30157)]
    public static async Task HouseAgent(Player player, Dialog dialog)
    {
        dialog.Avatar = 92;

        switch (await dialog.Ask("I am the houseagent in Twin City. If you want to buy a house, you must have my authorization.",
            "Buy a house.", "Upgrade my house.", "Just passing by."))
        {
            case 0:
                await dialog.Confirm("You should make an effort to buy a house.", "What shall I do?", "I changed my mind.");

                await dialog.Confirm("If you can give me five Timber Voucher, I shall issue a House Permit to you.",
                    "Here are the Vouchers.", "I shall come later.");

                if (!player.Take(721173, 5))
                {
                    dialog.Say("Sorry, you do not have five Timber Vouchers.", "I see.");
                    return;
                }

                if (player.Give(721170) is not null)
                {
                    dialog.Say("Here is a House Permit. You may carry it to the market and ask the House Admin to sell a house to you.",
                        "Thanks.");
                }
                break;

            case 1:
                dialog.Avatar = 28;

                await dialog.Confirm("Which class would you like to upgrade your house to?", "Second-class.", "Let me think it over.");

                await dialog.Confirm("Please exchange 10 Ore Vouchers for an Upgrade Certificate.", "Here are the vouchers.", "I shall come later.");

                if (player.Take(721176, 10))
                {
                    player.Give(721174);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 10 Ore Vouchers.", "I see.");
                }
                break;
        }
    }

    [Npc(30161)]
    public static async Task FurnitureNpc(Player player, Dialog dialog)
    {
        dialog.Avatar = 188;

        if (player.PkPoints > 1000)
        {
            return;
        }

        await dialog.Confirm("Welcome to Twin City Furniture Store. Currently, you have limited selection, but more furniture will come in soon.",
            "I wanna have a look.", "I am not interested.");

        if (player.PkPoints <= 1000)
        {
            player.Teleport(1511, 52, 70);
        }
        else
        {
            player.Inform("Error: Failed to meet condition.");
        }
    }

    [Npc(30165)]
    public static async Task EscortChief(Player player, Dialog dialog)
    {
        dialog.Avatar = 34;

        if (player.Has(721250))
        {
            await dialog.Confirm("You find out PeaceJade at last. I will give you an AmbergrisBag in return.",
                "Ok, thank you.", "Thanks. I do not need it.");

            if (player.Take(721250))
            {
                player.Give(121127);
            }
            else
            {
                dialog.Say("Don`t you take PeaceJade with you?", "I will bring it soon.");
            }
        }
        else if (player.Has(721253))
        {
            dialog.Say("Hope you can find out PeaceJade soon.", "I will get it soon.");
        }
        else if (player.Has(721251))
        {
            dialog.Say("How is the case going on?", "I will try my best.");
        }
        else
        {
            await dialog.Confirm("Forty days` deadline is coming. I have no clue at all. What can I do?",
                "What is wrong with you?", "I am just passing by.");

            await dialog.Confirm("Last month, the government entrusted us to escort the PeaceJade. It is a great mission, so we deployed hundreds of bodyguards.",
                "And what was wrong?");

            await dialog.Confirm("On the way to Desert City, all the bodyguards were killed and PeaceJade was stolen. But we have no clues of it at all.",
                "That is very bad.", "I had better leave now.");

            await dialog.Confirm("The government was furious about it. He commanded us to get it back in 40 days. Otherwise we`ll have to pay for it.\nNow, 30 days have passed and I still have no idea at all.",
                "Let me have a try.", "Sigh, god bless you.");

            if (player.Give(721251) is not null)
            {
                dialog.Say("That is very kind of you.", "You are welcome.");
            }
        }
    }

    [Npc(600050)]
    public static async Task Fortuneteller(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        await dialog.Confirm("Have you heard of Palace Method?", "Palace Method?", "Just passing by.");

        await dialog.Confirm("Are you interested? I would like to tell you more and hope you can work it out.",
            "Yeah, please go ahead.", "Sorry, I am very busy.");

        await dialog.Confirm("I discovered a mystic tactic a few days ago. I tried to work it out, but I failed and almost died from it.",
            "I`d like to have a try", "Too dangerous.");

        await dialog.Confirm("It is very dangerous. Are you sure you will try? If yes, I shall teleport you there. Talk to Maggie to enter the tactics.",
            "Yeah.", "I changed my mind.");

        if (player.Level < 70)
        {
            dialog.Say("I don`t think your skill is strong enough to pass it. Do not take risks.",
                "I will come later.");
        }
        else if (player.Level < 95)
        {
            if (await dialog.Ask("You are very weak. I am afraid you will die from the tactics. Are you sure you will take this risk?",
                "Yeah.", "I changed my mind.") == 0)
            {
                player.Teleport(1042, 28, 33);
            }
        }
        else
        {
            player.Teleport(1042, 28, 33);
        }
    }

    [Npc(600076)]
    public static async Task Boxer(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        await dialog.Confirm("If you are level 20 or above, you may train in the training ground. Would you like me to teleport you there for 1000 silver.",
            "Yes, please.", "No, thanks.");

        await dialog.Confirm("In the training ground, you will not lose any HP or MP. And no items nor durability will be consumed.",
            "Please teleport me there.", "No, I changed my mind.");

        if (player.Level < 20)
        {
            await dialog.Confirm("You haven`t reached level 20, so you cannot train inside. If you like you can still get in to take a look. I won`t charge you.",
                "Great. I wanna take a look.", "Thanks. I will come later.");

            player.Record(1002, 430, 380);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("The stake or scarecrow is level 20 or above. If its level is higher than yours, you cannot attack it.",
                    "I see.");
            }
        }
        else if (npc.OwnerId == (player.GuildId ?? 0))
        {
            player.Record(1002, 430, 380);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("The stake or scarecrow is level 20 or above. If its level is higher than yours, you cannot attack it.",
                    "I see.");
            }
        }
        else if (player.Spend(1000))
        {
            player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(1000);
            player.Record(1002, 430, 380);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("The stake or scarecrow is level 20 or above. If its level is higher than yours, you cannot attack it.",
                    "I see.");
            }
        }
        else
        {
            dialog.Say("Sorry, you do not have 1000 silver.", "I see.");
        }
    }
}
