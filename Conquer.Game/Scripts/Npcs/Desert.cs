namespace Conquer.Game.Scripts.Npcs;

public static class Desert
{
    [Npc(127)]
    public static async Task Assistant(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("Are you heading for silver mine? The rock monsters are very fierce. You cannot enter this cave before you are level 70.",
            "Please teleport me there.", "Just passing by.");

        if (player.Level < 70)
        {
            dialog.Say("Sorry, you are not allowed to enter this cave before you are level 70.", "I see.");
        }
        else
        {
            player.Teleport(1027, 138, 103);
        }
    }

    [Npc(10040)]
    public static async Task SpaceMarkNortheast(Player player, Dialog dialog)
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

            if (player.Give(1060034) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10041)]
    public static async Task SpaceMarkEast(Player player, Dialog dialog)
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

            if (player.Give(1060035) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10051)]
    public static async Task Conductress(Player player, Dialog dialog)
    {
        var choice = await dialog.Ask("Where are you heading for? I can teleport you for a price of 100 silver.",
            "Twin City", "Mystic Castle", "Market", "Just passing by.");

        switch (choice)
        {
            case 0 or 1:
                ushort x = choice switch { 0 => 971, _ => 85 };
                ushort y = choice switch { 0 => 666, _ => 323 };

                if (player.Spend(100))
                {
                    player.Teleport(1000, x, y);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;

            case 2:
                if (player.IsWanted)
                {
                    dialog.Avatar = 10;

                    await dialog.Confirm("You are the wanted criminal. It would be dangerous for me to teleport you to the market, so I shall charge you 1000 silver.",
                        "OK. Here you are.", "I changed my mind.");

                    if (player.Spend(1000))
                    {
                        player.Record(1000, 493, 650);
                        player.Teleport(1036, 266, 239);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have 1,000 silver.", "I see.");
                    }
                }
                else if (player.Spend(100))
                {
                    player.Record(1000, 493, 650);
                    player.Teleport(1036, 266, 239);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;
        }
    }

    [Npc(30000)]
    public static async Task Matchmaker(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        await dialog.Confirm("The fate brings lovers together. I hope that all can get married and live happily with their lover. What can I do for you?",
            "I want to get married.");

        await dialog.Confirm("Marriage is the promise forever. Are you sure that you want to spend the rest of your life with your lover?",
            "I want to get married.", "I prefer being single.");

        dialog.Require(!player.IsMarried);

        if (player.Level < 20)
        {
            dialog.Say("Sorry, you cannot get married before you are level 20 or above.", "I see.");
            return;
        }

        await dialog.Confirm("Are you ready to propose?", "Yeah. I am ready.", "Let me think it over.");

        if (!player.IsMarried)
        {
            player.Post(ClientCommand.Propose);

            dialog.Say("Please click on your lover to propose.", "OK.");
        }
        else
        {
            player.Inform("Error: Failed to meet condition.");
        }
    }

    [Npc(30102)]
    public static async Task VillageHead(Player player, Dialog dialog)
    {
        dialog.Avatar = 15;

        if (player.Has(721127))
        {
            await dialog.Confirm("I know sometimes the feudal official is not trustful. But they are the only person we can ask for help from.",
                "Sorry, I am unable to help you though I hope I can.");

            await dialog.Confirm("Please accept the money though it is not much.", "Thank you.");

            if (player.Take(721127))
            {
                player.Earn(1000);
            }
        }
        else if (player.Has(721129))
        {
            switch (await dialog.Ask("You are a wonderful guy to save our village. We are poor but please accept the money though it is not much.",
                "How can I accept your money?", "Thank you."))
            {
                case 0:
                    if (player.Roll(200) >= 1)
                    {
                        await dialog.Confirm("You are so kind to help us. We will remember you forever.", "You are welcome.");

                        player.Take(721129);
                        return;
                    }

                    await dialog.Confirm("Thanks. To express my appreciation, take it. It is left by a visitor and we have no use for it and hope it is useful to you.",
                        "Thanks. I will take it away.");

                    if (player.Take(721129))
                    {
                        player.Give(150116);
                    }
                    break;

                case 1:
                    if (player.Take(721129))
                    {
                        player.Earn(5000);
                    }
                    break;
            }
        }
        else if (player.HasAny(721125, 721126))
        {
            dialog.Say("Hope you can help us and we can use the water securely one day.", "Wait for my good news.");
        }
        else if (player.Level < 70)
        {
            dialog.Say("This small village lies in the vast desert. We support ourselves by selling some stuff to the adventurers and travellers.",
                "I see.");
        }
        else
        {
            await dialog.Confirm("Could you help me?", "What`s wrong with you.", "Sorry. I am busy now.");

            await dialog.Confirm("It lacks rain in desert. God has presented us a rich water source-MoonSpring in the west of our village.",
                "It is great, isn`t it?");

            await dialog.Confirm("But something unexpected happened soon. One day some StoneBandits occupied MoonSpring by force.", "And then?");

            await dialog.Confirm("We should buy water from him. We can not afford it if it lasts for a long time.",
                "What can I do to help you?", "It`s none of my business.");

            if (await dialog.Ask("Go teach them a lesson. Hope they won`t come here to bother us. Take something from them as keepsake.",
                "I will help you. Wait for my good news.") == 0)
            {
                player.Give(721126);
            }
        }
    }

    [Npc(30103)]
    public static async Task StoneBandit(Player player, Dialog dialog)
    {
        dialog.Avatar = 22;

        if (player.Has(721129))
        {
            dialog.Say("Anything else? Rock Bandits are faithful too though sometimes... We have given you the promise.",
                "Well done.");
        }
        else if (player.Has(721126))
        {
            if (!player.Has(721128))
            {
                if (await dialog.Ask("It`s none of your business. You`d better go out of my sight.",
                    "I don`t fear you at all.") == 0)
                {
                    dialog.Say("If you can defeat the StoneBandits and get BrokenSword, I will stop my behavior.",
                        "Don`t regret for what you said.");
                }

                return;
            }

            await dialog.Confirm("We acknowlege the corn. This is our promise. We won`t charge the water fee from the villager.", "Haha...");

            if (player.Take(721126) && player.Take(721128))
            {
                player.Give(721129);
            }
        }
        else
        {
            dialog.Say("Go out from my sight or I will kill you.", "I am busy.");
        }
    }

    [Npc(30104)]
    public static async Task GeneralShou(Player player, Dialog dialog)
    {
        dialog.Avatar = 30;

        if (player.Has(721127))
        {
            dialog.Say("Ok, we will deal with it when we are free.", "So mean.");
        }
        else if (player.Has(721125))
        {
            await dialog.Confirm("I know it`s VillagerHead asked you to come here. Go to tell him we will deal with it once we are free. We are so busy now.",
                "I can`t believe that you are so irresponsible.");

            if (player.Take(721125))
            {
                player.Give(721127);
            }
        }
        else
        {
            dialog.Say("I am in charge of the security of the city. No bad guys can escape here.",
                "Wow, it is wonderful.");
        }
    }

    [Npc(30164)]
    public static async Task Mike(Player player, Dialog dialog)
    {
        dialog.Avatar = 72;

        if (player.Has(721220))
        {
            dialog.Say("Joy is a good girl.", "I think so.");
        }
        else if (player.Has(721221))
        {
            await dialog.Confirm("I am very pleased to help Joy, but I need some raw material. Can you give me?",
                "What material do you need?", "Sorry, I am helpless.");

            if (await dialog.Ask("I need two normal Fury Gems, one normal Dragon Gem and 5 Euxenite Ores.",
                "Sorry, I am helpless.", "Here you are.") != 1)
            {
                return;
            }

            if (!player.Has(700021, 2))
            {
                dialog.Say("Sorry, you do not have two normal Fury Gems.", "I see.");
                return;
            }

            if (!player.Has(700011))
            {
                dialog.Say("Sorry, you do not have a normal Dragon Gem.", "I see.");
                return;
            }

            if (!player.Has(1072031, 5))
            {
                dialog.Say("Sorry, you do not have 5 Euxenite Ores.", "I see.");
                return;
            }

            if (!player.Take(721221))
            {
                return;
            }

            player.Take(700021, 2);
            player.Take(700011);
            player.Take(1072031, 5);

            if (player.Give(721220) is not null)
            {
                dialog.Say("The magic bow is made. You may give it to Joy now.", "Thanks.");
            }
        }
        else
        {
            dialog.Say("Please do not disturb me while I am working.", "Good Bye.");
        }
    }

    [Npc(30166)]
    public static async Task Shopboy(Player player, Dialog dialog)
    {
        dialog.Avatar = 46;

        if (player.Has(721252))
        {
            dialog.Say("Are there any other things I can do for you?", "No, thanks.");
        }
        else if (player.Has(721251))
        {
            await dialog.Confirm("What can I do for you?", "Inquire about escort team.", "No. Thanks.");

            await dialog.Confirm("You are asking the right person. At the time, more than 100 guards arrived here. But they were killed the following day.",
                "Did you find anything?", "It is terrible.");

            await dialog.Confirm("Let me think. I found a bloody blade in the well a couple of days ago. I don`t know whether it relates to the case or not.",
                "Show me the blade?", "I don`t think it helful.");

            await dialog.Confirm("If you give me 10,000 silver. I will send you the blade.", "Ok, give me the blade.", "Forget it.");

            if (!player.Spend(10000))
            {
                dialog.Say("Come here when you have enough money.", "Ok.");
                return;
            }

            if (player.Give(721252) is not null)
            {
                dialog.Say("You are generous. I will tell you another clue. Miss Roy in the maple forest is well-informed and knows much about \nweapons. You may speak to her for more information.",
                    "I will go right away.");
            }
        }
        else
        {
            dialog.Say("It is hot and dry in the desert. You can take a rest and have a cup of tea here.",
                "Thank you.");
        }
    }

    [Npc(600002)]
    public static async Task Joe(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has("MeteorTear"))
        {
            dialog.Say("I am so delighted to hear that Minner can understand me. I shall love and miss her wherever I go.",
                "I shall try to help you.");
        }
        else if (player.Has("SadMeteor"))
        {
            dialog.Say("Thanks a lot. Please kindly give this meteor to Minner. I hope she can understand me and wish her happy all her life.",
                "I hope so.");
        }
        else
        {
            await dialog.Confirm("It is so hot. It would be very nice If I have a bottle of wine. Can you give me some wine?",
                "I have a bottle of Amrita.", "I do not have either.");

            if (!player.Has(1000030) || !player.Has(ItemType.Meteor) || !player.Has(721001))
            {
                dialog.Say("I cannot take your Amrita. There is little water in the vast desert. You may feel thirsty and need it later.",
                    "See you then.");
                return;
            }

            switch (await dialog.Ask("Thank you. You are very kind. I often have no food and water when I travel around. Where did you get this guardian star?",
                "It is from Minner.", "I picked it on the road."))
            {
                case 0:
                    await dialog.Confirm("So she sent you to look for me. I have left so long. She must be heartbroken.", "Why did you leave her?");

                    await dialog.Confirm("We love each other deeply. I wish I could stay with her, but my life dream is to travel around. I cannot give her a warm home.",
                        "You are right.");

                    await dialog.Confirm("What a nice meteor you have. I have promised Minner that I shall give her a meteor. Would you please give your meteor to me.",
                        "Here you are.", "I like it very much.");

                    await dialog.Confirm("Thanks. I shall engrave my love on this meteor. Please kindly give it to Minner. She will understand why I leave her.",
                        "I shall give it to her.", "Do not touch my meteor.");

                    if (player.Take(721001) && player.Take(ItemType.Meteor))
                    {
                        player.Give(721002);
                    }
                    break;

                case 1:
                    dialog.Say("I must have hurt her a lot. She used to carry this bag wherever she goes. I hope she can forget me and be happy all her life.",
                        "I got to go. Bye.");
                    break;
            }
        }
    }

    [Npc(600060)]
    public static async Task MrPine(Player player, Dialog dialog)
    {
        dialog.Avatar = 92;

        if (player.Has("Letter.Pine"))
        {
            dialog.Say("Take my recommendation to Gossiper Wang in Bird Island, and he will tell you more about the Moon Gem.",
                "Ok, thanks.");
        }
        else if (player.Has(152126))
        {
            await dialog.Confirm("Wow, you have a refined Bone Bracelet! If you give it to me, I will tell you the clue of Moon Gem.",
                "Ok, It is a deal.", "Sorry, I need it too.");

            await dialog.Confirm("Thanks for your generous help. You are a good guy. I hope that we will defeat the monsters together one day.\nMy friend GossiperWang in BirdIsland is well-informed. You may get something about Moon Gem from him with my letter.",
                "I will pay him a visit.", "Sorry, I am busy.");

            if (player.Take(152126))
            {
                player.Give(723000);
            }
        }
        else
        {
            await dialog.Confirm("Brigt moon is where my mind lies. Although you are far away, I know that I will have it one day.",
                "Why do you like it?", "You are dreaming.");

            await dialog.Confirm("It is not the moon in the sky. It is a precious stone which possesses the power of controlling the evils.\nIf the Taoist has it socketed in his equipment, it will give him extra spell experience.",
                "Wow, it is marvelous.", "I do not believe it.");

            await dialog.Confirm("Yes. It is what I am dreaming about as the rest of our Taoists. My master obtained it after he killed five powerful monsters.\nIt is a pity that I did not see the heroic scene with my own eyes. Sigh!",
                "Yes, it is.", "I have got to leave.");

            if (await dialog.Ask("However, the evils are getting the upper hand. The world of Conquer is infested with the evil monsters.\nI wish I could rescue the people from the suffering. But it is difficult to achieve that without support.",
                "Let me help you.", "I have got to leave.") == 0)
            {
                dialog.Say("Great! I need a refined Bone Bracelet badly to defeat the monsters outside the village, and RichmanZhang in Desert City has it.\nIf you can help me get one from him, I will tell you more about the moon gem.",
                    "I will go to find him.", "I am not interested.");
            }
        }
    }

    [Npc(600061)]
    public static async Task RichmanZhang(Player player, Dialog dialog)
    {
        dialog.Avatar = 100;

        switch (await dialog.Ask("Hi, my friend, I am selling a variety of goods. What are you interested?",
            "What do you have?", "I have what you want.", "No. Thanks."))
        {
            case 0:
                await dialog.Confirm("Anything. You can name it. For the sake of safety, I never display them. Just tell me what you want.",
                    "Refined Bone Bracelet.", "No. Thanks.");

                await dialog.Confirm("Yes, I have it. It is very rare, and I am not going to sell it.", "But I need it badly.", "Oh, forget it.");

                if (await dialog.Ask("If that is the case, you can barter with me. I need 3 emeralds and 4 gold ores of rate 3 at least.\nI will exchange the bracelet for those items. How about it?",
                    "It is a deal.", "Forget it.") == 0)
                {
                    await TradeForBoneBracelet();
                }
                break;

            case 1:
                await TradeForBoneBracelet();
                break;
        }

        async Task TradeForBoneBracelet()
        {
            await dialog.Confirm("If you have 3 emeralds and 4 gold ores of rate 3 at least, I will give you a refined Bone Bracelet in return.",
                "Let`s exchange now.", "Let me think it over.");

            if (player.Has(1080001, 3) && player.HasAny(1072052, 1072059, 4))
            {
                player.Take(1072052, 1072059, 4);
                player.Take(1080001, 3);
                player.Give(152126);
            }
            else
            {
                dialog.Say("Sorry, you do not have 3 emeralds and 4 gold ores of rate 3 at least.", "Oh, I see.");
            }
        }
    }

    [Npc(600066)]
    public static async Task ToughWei(Player player, Dialog dialog)
    {
        dialog.Avatar = 8;

        if (await dialog.Ask("Bug off at once if you have nothing to do here!", "It is terrible.",
            "I come here for help.") != 1)
        {
            return;
        }

        await dialog.Confirm("Hum, what? Tell me right away. Do not waste my time. I am busy.", "I have a MoonLetter.", "I will go.");

        if (!player.Has(723005) && !player.Has(723006))
        {
            dialog.Say("Get out of here right away.", "Well.");
            return;
        }

        if (await dialog.Ask("Ha-ha... It is wonderful. I have been looking for it so long.\nI will give you 40,000 silver. Now I can report it to my boss. Ha-ha!",
            "Here you are.", "I leave it to myself.") == 0)
        {
            await HereYouAre();
        }

        async Task HereYouAre()
        {
            if (player.Take(723005) || player.Take(723006))
            {
                player.Earn(40000);
            }
        }
    }

    [Npc(600077)]
    public static async Task BoxerChang(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        await dialog.Confirm("If you are level 20 or above, you may level up at the training grounds. It does not consume any durability, life and mana.\nDo you want me to teleport you there? It is free for my guild members, and 1000 silver for others.",
            "Please teleport me there.", "Just passing by.");

        if (npc.OwnerId == (player.GuildId ?? 0))
        {
            player.Record(1000, 499, 649);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("When you gain experience at the training ground, you can only attack the dummies or stakes which level is lower than yours.",
                    "I see.");
            }
        }
        else if (player.Spend(1000))
        {
            player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(1000);
            player.Record(1000, 499, 649);

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
