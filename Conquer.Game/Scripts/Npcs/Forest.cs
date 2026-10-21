namespace Conquer.Game.Scripts.Npcs;

public static class Forest
{
    [Npc(40)]
    public static async Task Alchemist(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has("BrilliantStone"))
        {
            switch (await dialog.Ask("The Stone is hard-won. I would like to spend $2,000 buying it, or I can help to grind it into powder for a fee of $800.",
                "Ok, I will sell it to you.", "Grind it to GlitterPowder.", "Let me think it over."))
            {
                case 0:
                    if (player.Take(723007))
                    {
                        player.Earn(2000);
                    }
                    break;

                case 1:
                    if (player.Money < 800)
                    {
                        dialog.Say("You do not have enough money. Come when you have $800.", "Oh.");
                    }
                    else if (player.Take(723007) && player.Spend(800) && player.Give(723008) is not null)
                    {
                        dialog.Say("The powder is ok,and I will take the money away.", "Thank you.");
                    }
                    break;
            }
        }
        else if (player.Has(1072031))
        {
            await dialog.Confirm("Do you come for inquirying about alchemy?", "Exactly.", "Just passing by.");

            await dialog.Confirm("I hear people often dig out mysterious euxenite ore not knowing their usage.",
                "What is it for?", "Do not disturb me.");

            await dialog.Confirm("Based on the recipe handed down from my ancestors, I need them to refine a super armor. If you have, I shall buy from you.",
                "How much do you offer?", "I have no ores.");

            await dialog.Confirm("I shall offer 3000 silvers for five euxenite ores. How about that?", "OK. A deal.", "Too cheap.");

            if (player.Take(1072031, 5))
            {
                player.Earn(3000);
            }
            else
            {
                dialog.Say("Sorry, you do not have five ores. 0", "OK.");
            }
        }
        else if (player.HasAny(1072050, 1072059))
        {
            switch (await dialog.Ask("Have you got the gold ores yet?", "Yeah.", "How can I get ores?"))
            {
                case 0:
                    if (player.Has(1072059, 3))
                    {
                        await dialog.Confirm("You have three rate 10 gold ores? That is great. I can do the experiment with them. May I exchange them with a Kylin Gem?",
                            "Ok, it is a deal.", "I need those ores too.");

                        if (player.Take(1072059, 3))
                        {
                            player.Give(700041);
                        }
                    }
                    else if (player.HasAny(1072052, 1072059, 3))
                    {
                        player.Take(1072052, 1072059, 3);

                        if (await dialog.Ask("Thanks a lot for your great help. This is a refined bag for Taoist. May it bring good luck to you.",
                            "Thank you.") == 0)
                        {
                            player.Give(121126);
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry.You do not have three gold ores above rate 3.", "I will bring ores soon.");
                    }
                    break;

                case 1:
                    dialog.Say("Do not stick to the same place, change position from time to time. If you can see much more dust, then there are more ores.",
                        "I shall try again.");
                    break;
            }
        }
        else
        {
            await dialog.Confirm("The miner providing material for my experiment got hurt, while the experiment is in the turning point. How can I carry on?",
                "What material do you need?", "Just passing by.");

            switch (await dialog.Ask("I need three gold ores above rate 3. Could you help me?", "No problem.",
                "Sorry, I am helpless."))
            {
                case 0:
                    await NoProblem();
                    break;

                case 1:
                    if (await dialog.Ask("That is ok if you can bring me more ores.", "OK.") == 0)
                    {
                        await NoProblem();
                    }
                    break;
            }
        }

        async Task NoProblem()
        {
            dialog.Say("If you can bring me the gold ores, I shall offer a good reward.", "Thanks in advance.");
        }
    }

    [Npc(125)]
    public static async Task Assistant(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("Are you heading for copper mine? The rock monsters are very fierce. You cannot enter this cave before you are level 40.",
            "Please teleport me there.", "Just passing by.");

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you are not allowed to enter this cave before you are level 40.", "I see.");
        }
        else
        {
            player.Teleport(1025, 32, 70);
        }
    }

    [Npc(421)]
    public static async Task Norbert(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.Has(721262))
        {
            if (!player.Has(721262, 5) || !player.Has(721263))
            {
                if (await dialog.Ask("Five pieces of Saltpeter and one piece of Sulphur are required to make the Bomb. And I charge a fee of 30,000 silvers.",
                    "What is the Bomb?", "I see.") == 0)
                {
                    await WhatIsTheBomb();
                }

                return;
            }

            if (player.Money >= 30000)
            {
                await dialog.Confirm("Hi, you have got the meterial I want. Do you come for the Bomb?", "Yeah.", "Just passing by.");

                if (!player.Has(721262, 5) || !player.Has(721263))
                {
                    dialog.Say("But you do not take the materials I need.", "Oh, I will get them ready.");
                    return;
                }

                if (player.Money < 30000)
                {
                    dialog.Say("But you do not take the materials I need.", "Oh, I will get them ready.");
                }
                else if (player.Take(721262, 5) && player.Take(721263) && player.Spend(30000) &&
                    player.Give(721261) is not null)
                {
                    player.AddProgress(3, 0, 1);

                    dialog.Say("You can take the Bomb away. Remember that you will be blown up too every time you use it.",
                        "Thank you.");
                }

                return;
            }

            if (await dialog.Ask("Five pieces of Saltpeter and one piece of Sulphur are required to make the Bomb. And I charge a fee of 30,000 silvers.",
                "What is the Bomb?", "I see.") == 0)
            {
                await WhatIsTheBomb();
            }
        }
        else if (player.Has(721263))
        {
            if (await dialog.Ask("Five pieces of Saltpeter and one piece of Sulphur are required to make the Bomb. And I charge a fee of 30,000 silvers.",
                "What is the Bomb?", "I see.") == 0)
            {
                await WhatIsTheBomb();
            }
        }
        else
        {
            await dialog.Confirm("Saltpeter, Sulphur, charcoal, and now make a fire...", "What are you doing?", "I`d better not bother you.");

            if (await dialog.Ask("I`m refining a bomb which is very powerful but dangerous.", "What is it used for?",
                "Wow, I`d better leave here.") == 0)
            {
                await WhatIsTheBomb();
            }
        }

        async Task WhatIsTheBomb()
        {
            await dialog.Confirm("Blow up the guild gate! I attended the guild war and witnessed lots of casualties in attacking the gate. Ever\nsince, I devoted myself to inventing a kind of powerful bomb which can smash the guild gate easily. I finally work it out.",
                "Can you make one for me?", "Oh. I see.");

            await dialog.Confirm("I`d be glad to. Mind you that this bomb is so powerful that it will exert enormous damage to the gate and kill the Bomb user.",
                "Thanks. I need it badly.", "Then forget it.");

            if (await dialog.Ask("Good. You prepare five pieces of Saltpeter and a piece of Sulphur. And I will charge 30000 silvers as fees and other materials.",
                "Where do I get the materials?", "I`ll give up.") == 0)
            {
                dialog.Say("You can get Saltpeter from my friend OldQuarrier in Ape City and get Sulphur from the Bandits nearby.",
                    "Ok, I`ll get them ready.");
            }
        }
    }

    [Npc(1610)]
    public static async Task OldMiner(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        await dialog.Confirm("It was said that some rare BlueMice appear in the forest mine cave, and carry treasures with them. I once caught\none, but got nothing from it. Do you want to try your luck?",
            "Yeah, why not?", "I don`t believe it.");

        switch (await dialog.Ask("You must use Silver or Gold Needle to catch this kind of mouse. I do have the stuff. If you want, I can exchange\nSilver Needle for a DragonGem, PhoenixGem KylinGem, or RainbowGem; and exchange GoldNeedle for FuryGem, MoonGem or VioletGem.",
            "What is the difference?", "I want Gold Needle.", "I want Silver Needle."))
        {
            case 0:
                switch (await dialog.Ask("The chance of getting treasures is a little bit higher if you use GoldNeedle. But it`s invariable that you must be patient.",
                    "So I want GoldNeedle.", "Even so, I want SilverNeedle.", "Oh, I see."))
                {
                    case 0:
                        await IWantGoldNeedle();
                        break;

                    case 1:
                        await IWantSilverNeedle();
                        break;
                }
                break;

            case 1:
                await IWantGoldNeedle();
                break;

            case 2:
                await IWantSilverNeedle();
                break;
        }

        async Task IWantGoldNeedle()
        {
            if (player.Has(722511) || player.Has(722510))
            {
                dialog.Say("You have got the needle. Hurry to the forest mine cave to catch BlueMouse, it is rare.",
                    "Ok, I will hurry up.");
            }
            else if (player.Has(700061))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Gold Needle?",
                    "Yes. I want GoldNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700061) && player.Give(722511) is not null)
                {
                    player.Inform("You got a GoldNeedle.");
                }
            }
            else if (player.Has(700021))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Gold Needle?",
                    "Yes. I want GoldNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700021) && player.Give(722511) is not null)
                {
                    player.Inform("You got a GoldNeedle.");
                }
            }
            else if (player.Has(700051))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Gold Needle?",
                    "Yes. I want GoldNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700051) && player.Give(722511) is not null)
                {
                    player.Inform("You got a GoldNeedle.");
                }
            }
            else
            {
                if (await dialog.Ask("I am sorry that you should use normal FuryGem, MoonGem or VioletGem in exchange for a GoldNeedle.",
                    "Ok, I will get it ready soon.", "Change it to SilverNeedle.") == 1)
                {
                    await IWantSilverNeedle();
                }
            }
        }

        async Task IWantSilverNeedle()
        {
            if (player.Has(722511) || player.Has(722510))
            {
                dialog.Say("You have got the needle. Hurry to the forest mine cave to catch BlueMouse, it is rare.",
                    "Ok, I will hurry up.");
            }
            else if (player.Has(700031))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Silver Needle?",
                    "Yes. I want SilverNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700031) && player.Give(722510) is not null)
                {
                    player.Inform("You got a SilverNeedle.");
                }
            }
            else if (player.Has(700041))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Silver Needle?",
                    "Yes. I want SilverNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700041) && player.Give(722510) is not null)
                {
                    player.Inform("You got a SilverNeedle.");
                }
            }
            else if (player.Has(700001))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Silver Needle?",
                    "Yes. I want SilverNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700001) && player.Give(722510) is not null)
                {
                    player.Inform("You got a SilverNeedle.");
                }
            }
            else if (player.Has(700011))
            {
                if (await dialog.Ask("You do have the gem I want. Are you sure to exchange Silver Needle?",
                    "Yes. I want SilverNeedle.", "No. I change my mind.") != 0)
                {
                    return;
                }

                if (player.Take(700011) && player.Give(722510) is not null)
                {
                    player.Inform("You got a SilverNeedle.");
                }
            }
            else
            {
                if (await dialog.Ask("I am sorry that you should use normal DragonGem, PhoenixGem KylinGem, or RainbowGem in exchange for a SilverNeedle.",
                    "Ok, I will get it ready soon.", "Then change it to GoldNeedle") == 1)
                {
                    await IWantGoldNeedle();
                }
            }
        }
    }

    [Npc(3216)]
    public static async Task MrFree(Player player, Dialog dialog)
    {
        dialog.Avatar = 74;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (!player.Has(723087))
        {
            dialog.Say("I love living a seclusive life. Don`t bother me!", "I`ll leave now.");
            return;
        }

        await dialog.Confirm("I love living a seclusive life. Don`t bother me!", "I heard that you need LuckyAmulets.");

        dialog.Require(player.PkPoints <= 1000);

        await dialog.Confirm("Yes. My only hobby is to collect LuckyAmulets.", "I happen to have one.", "I`d better keep it.");

        dialog.Require(player.PkPoints <= 1000);

        await dialog.Confirm("Great! If you present LuckyAmulet to me, I`ll make a unique hairstyle for you.", "Thank you.", "I`d better keep it.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.IsFemale)
        {
            switch (await dialog.Ask("Thank you very much. Which hairstyle would you like to select from?",
                "Lovely one.", "Comely one.", "Pretty one.", "Elegant one.", "Nifty one."))
            {
                case 0:
                    await DignifiedOne();
                    break;

                case 1:
                    await UnrulyOne();
                    break;

                case 2:
                    await StaidOne();
                    break;

                case 3:
                    await CoolOne();
                    break;

                case 4:
                    await HandsomeOne();
                    break;
            }

            return;
        }

        switch (await dialog.Ask("Thank you very much. Which hairstyle would you like to select from?",
            "Dignified one.", "Unruly one.", "Staid one.", "Cool one.", "Handsome one."))
        {
            case 0:
                await DignifiedOne();
                break;

            case 1:
                await UnrulyOne();
                break;

            case 2:
                await StaidOne();
                break;

            case 3:
                await CoolOne();
                break;

            case 4:
                await HandsomeOne();
                break;
        }

        async Task DignifiedOne()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Take(723087))
            {
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 21));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task UnrulyOne()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Take(723087))
            {
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 22));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task StaidOne()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Take(723087))
            {
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 23));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task CoolOne()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Take(723087))
            {
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 24));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task HandsomeOne()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Take(723087))
            {
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 25));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task TryAnotherStyle()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            if (player.IsFemale)
            {
                switch (await dialog.Ask("Which hairstyle would you like to select from?", "Lovely one.", "Comely one.",
                    "Pretty one.", "Elegant one.", "Nifty one."))
                {
                    case 0:
                        await DignifiedOneRetry();
                        break;

                    case 1:
                        await UnrulyOneRetry();
                        break;

                    case 2:
                        await StaidOneRetry();
                        break;

                    case 3:
                        await CoolOneRetry();
                        break;

                    case 4:
                        await HandsomeOneRetry();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Which hairstyle would you like to select from?", "Dignified one.", "Unruly one.",
                "Staid one.", "Cool one.", "Handsome one."))
            {
                case 0:
                    await DignifiedOneRetry();
                    break;

                case 1:
                    await UnrulyOneRetry();
                    break;

                case 2:
                    await StaidOneRetry();
                    break;

                case 3:
                    await CoolOneRetry();
                    break;

                case 4:
                    await HandsomeOneRetry();
                    break;
            }
        }

        async Task DignifiedOneRetry()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 21));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task UnrulyOneRetry()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 22));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task StaidOneRetry()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 23));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task CoolOneRetry()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 24));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }

        async Task HandsomeOneRetry()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 25));

            if (await dialog.Ask("Are you satisfied with your new hairsyle?", "Great! Thank you.",
                "I`d like to try another one.") == 1)
            {
                await TryAnotherStyle();
            }
        }
    }

    [Npc(10034)]
    public static async Task SpaceMarkSouth(Player player, Dialog dialog)
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

            if (player.Give(1060028) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10035)]
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

            if (player.Give(1060029) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10036)]
    public static async Task SpaceMarkWest(Player player, Dialog dialog)
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

            if (player.Give(1060038) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10052)]
    public static async Task Conductress(Player player, Dialog dialog)
    {
        switch (await dialog.Ask("Where are you heading for? I can teleport you for a price of 100 silver.",
            "Twin City", "Market", "Just passing by."))
        {
            case 0:
                if (player.Spend(100))
                {
                    player.Teleport(1011, 11, 376);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;

            case 1:
                if (player.IsWanted)
                {
                    dialog.Avatar = 10;

                    await dialog.Confirm("You are the wanted criminal. It would be dangerous for me to teleport you to the market, so I shall charge you 1000 silver.",
                        "OK. Here you are.", "I changed my mind.");

                    if (player.Spend(1000))
                    {
                        player.Record(1011, 193, 266);
                        player.Teleport(1036, 266, 239);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have 1,000 silver.", "I see.");
                    }
                }
                else if (player.Spend(100))
                {
                    player.Record(1011, 193, 266);
                    player.Teleport(1036, 266, 239);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;
        }
    }

    [Npc(20000)]
    public static async Task Shirley(Player player, Dialog dialog)
    {
        dialog.Avatar = 109;

        if (player.Has(721211))
        {
            await dialog.Confirm("When I was young, I liked Windbell very much. My brother often told me that I should take care of myself whatever happened.",
                "Wish you happy every day.");

            await dialog.Confirm("I must live happily as my brother wished. Thanks for your great help. I give you my favorite earring. Do you like it?",
                "Yeah. I like it best.", "Thanks. I have one, too.");

            if (player.Has(721210))
            {
                if (player.Take(721211) && player.Take(721210))
                {
                    player.Give(117346);
                }
            }
            else
            {
                dialog.Say("You have not brought my Jade back? That Jade and Windbell are my favorite. If I lose them, I cannot live.",
                    "I got to get it.");
            }
        }
        else if (player.Has(721210))
        {
            dialog.Say("If my brother has been dead, Would you please bring my Jade back.", "Okay.");
        }
        else if (player.Level < 50)
        {
            dialog.Say("How nice it would be if there is no wars in this world!", "I hope so.");
        }
        else
        {
            await dialog.Confirm("If I cannot hear from my brother, I would rather die than live lonely.",
                "Can I help you?", "Oh, another poor girl.");

            await dialog.Confirm("My parents died when I was a child. My brother brought me up. I have not heard from him since he joined the army last year.",
                "Can I help you?", "I am helpless.");

            if (await dialog.Ask("My brother`s name is Ryan. He gave me this Jade to celebrate my 10th birthday. Would you please take it to find him for me.",
                "Sure. I must find him.", "Sorry, I am too busy.") == 0)
            {
                player.Give(721210);
            }
        }
    }

    [Npc(30081)]
    public static async Task CoachLin(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has("Ref.Letter.Li"))
        {
            dialog.Say("It seems each new generation excels the last one.", "Of course.");
            return;
        }

        if (player.Level < 16)
        {
            dialog.Say("Since Coach Li introduced you, I must try my best to help you. Please come to me when you reach level 16.",
                "OK.");
            return;
        }

        await dialog.Confirm("Since you have come, I shall ask you a few questions. If you answer correctly, I will give you a reward.",
            "Ok. Go ahead.", "Too troublesome.");

        dialog.Require(player.Has("Ref.Letter.Li"));

        switch (player.Roll(1, 8))
        {
            case 1 or 5:
                switch (await dialog.Ask("When can a player get promoted for the second time?", "Level 40", "Level 30",
                    "Level 15", "Level 70"))
                {
                    case 0:
                        await Level40();
                        break;

                    case 1:
                        await Level30();
                        break;

                    case 2:
                        await Level30();
                        break;

                    case 3:
                        await Level30();
                        break;
                }
                break;

            case 2 or 6:
                switch (await dialog.Ask("Who teleports you into the Training Ground of Twin City?", "SouthBoxer",
                    "BoxerHuang", "NorthBoxer", "Boxer"))
                {
                    case 0:
                        await Level30();
                        break;

                    case 1:
                        await Level30();
                        break;

                    case 2:
                        await Level30();
                        break;

                    case 3:
                        await Level40();
                        break;
                }
                break;

            case 3 or 7:
                switch (await dialog.Ask("How many items can you store in a warehouse?", "15", "20", "25", "30"))
                {
                    case 0:
                        await Level30();
                        break;

                    case 1:
                        await Level40();
                        break;

                    case 2:
                        await Level30();
                        break;

                    case 3:
                        await Level30();
                        break;
                }
                break;

            case 4 or 8:
                switch (await dialog.Ask("When can Trojan be equipped with two-handed weapons?", "Level 15", "Level 30",
                    "Level 40", "Level 70"))
                {
                    case 0:
                        await Level30();
                        break;

                    case 1:
                        await Level30();
                        break;

                    case 2:
                        await Level40();
                        break;

                    case 3:
                        await Level30();
                        break;
                }
                break;
        }

        async Task Level40()
        {
            dialog.Require(player.Has("Ref.Letter.Li"));

            await dialog.Confirm("Good. Here is an item for you. This item will be useful to you.", "Thanks a lot.");

            dialog.Require(player.Has("Ref.Letter.Li"));

            if (!player.Take(721115))
            {
                return;
            }

            if (player.Roll(2) < 1)
            {
                player.Give(player.Pick<uint>(410036, 420036, 560036, 430036, 450036, 490036, 460036, 561036));
            }
            else
            {
                player.Give(player.Pick<uint>(481036, 510036, 480036, 440036, 580036, 540036, 440026, 421026));
            }
        }

        async Task Level30()
        {
            if (player.Has("Ref.Letter.Li"))
            {
                dialog.Say("Sorry, your answer is wrong. Please come again when you learn more about Conquer.", "OK.");
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }
        }
    }

    [Npc(30083)]
    public static async Task Killer(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has("Invigorant"))
        {
            await dialog.Confirm("The drug is enough for me to go to Desert City! Thank you and take away the money. ", "Of course, I deserve it.");

            dialog.Require(player.Has("Invigorant"));

            if (player.Take(721119))
            {
                player.Earn(2000);
            }
        }
        else if (player.Has("ShopList"))
        {
            dialog.Say("Please hurry up. I am afraid that I could not sustain any longer.", "Ok. Right back.");
        }
        else
        {
            if (await dialog.Ask("Do not tell others you have seen me, or I must kill you.", "I see.",
                "I never fear you.") != 1)
            {
                return;
            }

            await dialog.Confirm("You bear a murderous look on your face. We must be in the same line. I am in trouble. Will you help me?",
                "Tell me more details.", "Sorry, I am too busy.");

            await dialog.Confirm("I have killed too many people. I am wanted by the police. The patrol guard must kill me on sight.",
                "What can I do for you?");

            await dialog.Confirm("I had run out of all Invigorant. Can you help me to take some from Druggist in TwinCity?",
                "It`s easy. Wait me.", "It is so easy, why not ask others to help you?");

            if (player.Give(721118) is not null)
            {
                dialog.Say("Druggist are very busy and may be not in TwinCity. You should be hurry.", "Oh, I see.");
            }
        }
    }

    [Npc(30086)]
    public static async Task BanditHead(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("It is our domain. If you do not want to die, you had better get out of here right now.",
            "Did you rob food carrier?", "I got to go at once.");

        if (await dialog.Ask("We dare not rob the army. Only the unarmed Trade Caravans are our target.",
            "Who might do it?") == 0)
        {
            dialog.Say("There is another group of bandits here who are stronger and bigger than us. I am afraid it was them who did it.",
                "I trust you.");
        }
    }

    [Npc(30088)]
    public static async Task OldManTang(Player player, Dialog dialog)
    {
        dialog.Avatar = 8;

        if (player.Has(121066) || player.Has(151056) || player.Has(121067) || player.Has(151057) ||
            player.Has(121068) || player.Has(151058))
        {
            dialog.Say("Thank you for saving my life. Now I can detoxicate myself.", "You are welcome.");
        }
        else if (player.Has(721120))
        {
            await dialog.Confirm("With this rat fang, I can be saved. Please take it as a reward.", "Thanks a lot.");

            dialog.Require(player.Has("RatFang"));

            if (!player.Take(721120))
            {
                return;
            }

            if (player.Roll(3) < 1)
            {
                if (player.Roll(2) < 1)
                {
                    if (player.Roll(2) < 1)
                    {
                        if (player.Roll(1000) < 1)
                        {
                            player.Give(121067);
                        }
                        else if (player.Roll(2) < 1)
                        {
                            player.Give(121066);
                        }
                        else
                        {
                            player.Give(151056);
                        }
                    }
                    else if (player.Roll(1000) < 1)
                    {
                        player.Give(151057);
                    }
                    else if (player.Roll(2) < 1)
                    {
                        player.Give(121066);
                    }
                    else
                    {
                        player.Give(151056);
                    }
                }
                else if (player.Roll(2) < 1)
                {
                    if (player.Roll(2000) < 1)
                    {
                        player.Give(121068);
                    }
                    else if (player.Roll(2) < 1)
                    {
                        if (player.Roll(2) < 1)
                        {
                            player.Give(121066);
                        }
                        else
                        {
                            player.Give(151056);
                        }
                    }
                    else if (player.Roll(2) < 1)
                    {
                        if (player.Roll(1000) < 1)
                        {
                            player.Give(121067);
                        }
                        else if (player.Roll(2) < 1)
                        {
                            player.Give(121066);
                        }
                        else
                        {
                            player.Give(151056);
                        }
                    }
                    else if (player.Roll(1000) < 1)
                    {
                        player.Give(151057);
                    }
                    else if (player.Roll(2) < 1)
                    {
                        player.Give(121066);
                    }
                    else
                    {
                        player.Give(151056);
                    }
                }
                else if (player.Roll(2000) < 1)
                {
                    player.Give(151058);
                }
                else if (player.Roll(2) < 1)
                {
                    if (player.Roll(2) < 1)
                    {
                        player.Give(121066);
                    }
                    else
                    {
                        player.Give(151056);
                    }
                }
                else if (player.Roll(2) < 1)
                {
                    if (player.Roll(1000) < 1)
                    {
                        player.Give(121067);
                    }
                    else if (player.Roll(2) < 1)
                    {
                        player.Give(121066);
                    }
                    else
                    {
                        player.Give(151056);
                    }
                }
                else if (player.Roll(1000) < 1)
                {
                    player.Give(151057);
                }
                else if (player.Roll(2) < 1)
                {
                    player.Give(121066);
                }
                else
                {
                    player.Give(151056);
                }
            }
            else if (player.Roll(2) < 1)
            {
                player.Give(121066);
            }
            else
            {
                player.Give(151056);
            }
        }
        else
        {
            await dialog.Confirm("My prayer comes true in the end. I thought I would die in this untraversed area. Would you help me out?",
                "What can I do?", "Just passing by.");

            if (await dialog.Ask("I was hurt by poisonous rat on my way home. I can only restrain my wound from deteriorating with antidote.",
                "How can I help you?", "I got to go.") == 0)
            {
                dialog.Say("I need a rat fang as an ingredient.", "I shall bring one soon.", "It is too dangerous.");
            }
        }
    }

    [Npc(30114)]
    public static async Task GeneralWu(Player player, Dialog dialog)
    {
        dialog.Avatar = 63;

        if (player.Has(721133))
        {
            dialog.Say("Do not tell the General that I am hunting. Okay?", "Okay.");
        }
        else if (player.Has(721135))
        {
            if (!player.Has(721134))
            {
                dialog.Say("It is annoying. Tell my men that I will be back soon and do not send a stranger to disturb me for nothing.",
                    "Ok.");
                return;
            }

            await dialog.Confirm("Oh, it is the official letter from the General. Give it to me, let me take a look first.", "Here you are.");

            await dialog.Confirm("I have read the official letter. Here is my letter in reply. Please give it to the General.", "I see.");

            if (player.Take(721135) && player.Take(721134) && player.Give(721133) is not null)
            {
                var (x, y) = player.Pick<(ushort, ushort)>([(800, 471), (728, 490), (685, 426), (720, 375), (800, 471),
                    (728, 490), (685, 426), (720, 375)]);
                player.World.MoveNpc(30114, 1011, x, y);
            }
        }
        else
        {
            dialog.Say("Get out of here. I`ll never forgive you if you scare away the game.", "Ok.");
        }
    }

    [Npc(30163)]
    public static async Task Joy(Player player, Dialog dialog)
    {
        dialog.Avatar = 118;

        if (player.Has(721220))
        {
            await dialog.Confirm("Thanks a lot. I give my elite coat to you as a token of my gratitude.", "Thanks.", "Do not mention it.");

            if (player.Take(721220))
            {
                player.Give(133538);
            }
        }
        else if (player.Has(721221))
        {
            dialog.Say("Thanks.", "My pleasure.");
        }
        else
        {
            await dialog.Confirm("I feel very sad these days.", "What happened.", "That is life.");

            await dialog.Confirm("My father was fond of collecting bows and arrows. He has not touched them since I lost his magic bow.",
                "Why not make one for him.", "What a pity.");

            if (await dialog.Ask("My good friend, Mike, is skilled in making magic bows. Would you please take my letter to him and bring his bow back to me.",
                "Okay.", "Sorry, I am helpless.") == 0)
            {
                player.Give(721221);
            }
        }
    }

    [Npc(30167)]
    public static async Task Roy(Player player, Dialog dialog)
    {
        dialog.Avatar = 189;

        if (player.Has(721253))
        {
            dialog.Say("Are there other things I can help you?", "No. Bye-bye.");
        }
        else if (player.Has(721251) && player.Has(721252))
        {
            if (await dialog.Ask("EscortChief asked you to look into the case. Why do you come here?",
                "Just passing by.", "Please do me a favor.") != 1)
            {
                return;
            }

            await dialog.Confirm("My information costs a meteor. Give me a meteor then I will answer your question.", "No problem!", "Forget it.");

            if (!player.Has(ItemType.Meteor))
            {
                dialog.Say("Give a meteor and then I will answer your question.", "I will prepare it.");
                return;
            }

            await dialog.Confirm("Ok, what can I do for you?", "Identify my blade.");

            if (!player.Has(721252))
            {
                dialog.Say("You are kidding. You did not bring the blade.", "I forgot it.");
                return;
            }

            await dialog.Confirm("It is GreenBlade and belongs to Yougo in NewDesert.", "Tell me more details.");

            await dialog.Confirm("Judged from the bloodstain, it happened 2 months ago, when the PeaceJade was robbed. It may be the weapon used by the murderer.",
                "It seems like that.");

            if (await dialog.Ask("Right. I heard that the Blade Ghosts stole the KylinJade from Yougo. Yougo and Blade Ghost are enemies.",
                "It is complicated.", "What does it concern?") != 1)
            {
                return;
            }

            await dialog.Confirm("In my judgment, BladeGhost persuaded Yougo to kill the bodyguards and robbed the jade. Thus force Escort Bureau disappear.",
                "Where is PeaceJade?", "It is terrible.");

            await dialog.Confirm("Yougo has no chance to exchange for KylinJade with PeaceJade. So Yougo must still keep the jade.",
                "What can I do now?");

            await dialog.Confirm("I do not think it is possible to get back the jade by force for Yougo is a tough guy. You can only exchange it with JadeKylin.",
                "I will find JadeKylin.", "Wow, it is troublesome.");

            if (player.Take(ItemType.Meteor) && player.Take(721251) && player.Take(721252) &&
                player.Give(721253) is not null)
            {
                dialog.Say("I saw Yougo once. Take my letter to him. Hope he will return the PeaceJade and surrender.",
                    "Ok, thank you.");
            }
        }
        else
        {
            dialog.Say("People in the martial circle called me Roy, in spite of that it is a male name.",
                "You must be a heroine.");
        }
    }

    [Npc(600000)]
    public static async Task Milly(Player player, Dialog dialog)
    {
        dialog.Avatar = 4;

        if (player.Has("GuardianStar"))
        {
            dialog.Say("Have you seen my sister? She cannot cheer up until her Joe returns to her.",
                "I must help her.");
        }
        else if (player.Has("Milly`sLetter"))
        {
            dialog.Say("My sister is in Bird Island. She fell in love with Joe there. Since Joe left her with no news, she stays there to wait for him.",
                "I see.");
        }
        else
        {
            await dialog.Confirm("I cannot get near you now. You have got me going crazy. Wherever you go, whatever you do, I will be right here waiting for you.",
                "Why are you so sad?", "I got to go.");

            await dialog.Confirm("My sister taught me that song. .Her lover, Joe, is a brave and kind man. All said they make a good couple, but he left her.",
                "What a pity.", "Another old love story.");

            if (await dialog.Ask("My sister believes that Joe still loves her. She stays in Bird Island waiting for him. Can you take my letter to her?",
                "OK. I shall visit her.", "Just passing by.") == 0)
            {
                player.Give(721000);
            }
        }
    }

    [Npc(600065)]
    public static async Task William(Player player, Dialog dialog)
    {
        if (!player.Has("GlitterPowder") || !player.Has("SecretLetter"))
        {
            dialog.Avatar = 27;

            await dialog.Confirm("You see? It looks like a dreamland. I can only see the thick forest and hear the twitter of the happy birds.",
                "You are in good mood!");

            if (player.Has(723004))
            {
                if (!player.Has(723003))
                {
                    dialog.Say("It is so beautiful here. Why don`t you look around?", "Yes, I will.");
                    return;
                }

                await dialog.Confirm("Yes, indeed. What brings you here, my friend?", "Identify this letter.", "Nothing, just passing by.");

                if (player.Roll(100) < 50)
                {
                    if (!player.Take(723004) || player.Give(723006) is null || !player.Take(723003))
                    {
                        return;
                    }

                    if (await dialog.Ask("This is the real letter. It was written by the celestial wine and only shown its letters when sprinkled with the GlitterPowder.",
                        "How to get GlitterPowder?", "I will give up.") == 0)
                    {
                        dialog.Avatar = 8;

                        dialog.Say("GlitterPowder comes from BrilliantStone. Alchemist in this city can make it. StoneMonster in each mine cave has BrilliantStone.\nIf I have GlitterPowder, I can make the letter on the MoonLetter shown.",
                            "I will help you.");
                    }
                }
                else if (player.Take(723004) && player.Give(723005) is not null && player.Take(723003))
                {
                    dialog.Say("Do you come for the Gem? Frankly speaking, it is a FakeLetter.\nIt is so emulational that BladeGhost can`t distinguish it. You can sell it to ToughWei outside the MysticCastle.",
                        "Alright.", "Forget it.");
                }
            }
            else if (player.Has(723003))
            {
                dialog.Say("Oh, Roger recommended you here. But you should get the unidentified letter first so that I can distinguish it.",
                    "I see.");
            }
            else
            {
                dialog.Say("It is so beautiful here. Why don`t you look around?", "Yes, I will.");
            }

            return;
        }

        dialog.Avatar = 27;

        await dialog.Confirm("If you give me the GlitterPowder and the Secret Letter, I can reveal the letter on it.", "Thanks.", "Wait a moment.");

        dialog.Require(player.Has("GlitterPowder") && player.Has("SecretLetter"));

        if (player.Take(723006) && player.Take(723008) && player.Give(723009) is not null)
        {
            player.Alert("The Moon Letter displayed lines of letters when the GlitterPowder was sprinkled on it.");

            dialog.Say("The moon above Ape Mountain and in the midnight? It looks like a poem but it does not tell where the Gem is.\nYou can go to ask OldGeneral in ApeMountain for help with the letter.",
                "Thanks, I must go now.");
        }
    }

    [Npc(600078)]
    public static async Task BoxerLee(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        await dialog.Confirm("If you are level 20 or above, you may level up at the training grounds. It does not consume any durability, life and mana.\nDo you want me to teleport you there? It is free for my guild members, and 1000 silver for others.",
            "Please teleport me there.", "Just passing by.");

        if (npc.OwnerId == (player.GuildId ?? 0))
        {
            player.Record(1011, 195, 265);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("When you gain experience at the training ground, you can only attack the dummies or stakes which level is lower than yours.",
                    "I see.");
            }
        }
        else if (player.Spend(1000))
        {
            player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(1000);
            player.Record(1011, 195, 265);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("When you gain experience at the training ground, you can only attack the dummies or stakes which level is lower than yours.",
                    "I see.");
            }
        }
        else
        {
            dialog.Say("Sorry, you do not have 1000 silver.", "I see.");
        }
    }
}
