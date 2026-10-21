namespace Conquer.Game.Scripts.Npcs;

public static class Island
{
    [Npc(41)]
    public static async Task ArtisanOu(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        await dialog.Confirm("It is very difficult to own a gemmy weapon. I am famous for my excellent skills. What can I do for you?",
            "Make a socket in my weapon.", "Just passing by.");

        await dialog.Confirm("If you give me a Dragon Ball, I shall try my best to help you.", "OK. Here is a Dragon Ball.", "I changed my mind.");

        await dialog.Confirm("Please equip the right handed weapon you want to socket first. Now I am going to make a socket in your weapon.",
            "I am ready.", "I changed my mind.");

        if (!player.Has("DragonBall"))
        {
            dialog.Say("Sorry, you do not have the required Dragon Ball(s).", "I see.");
            return;
        }

        if (player.HasSocket(1))
        {
            await dialog.Confirm("Wow, your weapon has a socket. It is so rare. Are you here for another socket. If yes, 5 Dragon Balls will be charged.",
                "Here are 5 Dragon Balls.", "I changed my mind.");

            if (!player.Has(ItemType.DragonBall, 5))
            {
                dialog.Say("Sorry, you do not have the required Dragon Ball(s).", "I see.");
                return;
            }

            if (player.MakeSocket(2) && player.Take(ItemType.DragonBall, 5))
            {
                dialog.Say("Your weapon has been socketed. Please kindly check.", "Thanks.");
            }
        }
        else if (player.MakeSocket(1) && player.Take(ItemType.DragonBall))
        {
            dialog.Say("Your weapon has been socketed. Please kindly check.", "Thanks.");
        }
    }

    [Npc(3010)]
    public static async Task AntiqueDealer(Player player, Dialog dialog)
    {
        dialog.Avatar = 90;

        if (player.Has(722809))
        {
            dialog.Say("Have you got anything else?", "No. Thanks.");
        }
        else if (player.Has(722817))
        {
            if (!player.Has(722818) || !player.Has(722819))
            {
                switch (await dialog.Ask("Hi, man, you`ve got good stuffs. Are your antiques for sale?",
                    "How much do you offer it?", "No. I wanna exchange for something."))
                {
                    case 0:
                        await SellAntique();
                        break;

                    case 1:
                        await NoIWannaExchange();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Hi, man, you`re got good stuffs. Are your antiques for sale? I really love them.",
                "How much do you offer it?", "No. I wanna exchange for something."))
            {
                case 0:
                    await dialog.Confirm("How about 3000 silvers for one? I will buy whatever you have.",
                        "Ok, it is a deal.", "The price is much too low.");

                    await dialog.Confirm("Great! Then I will take away your antiques and give you the money.",
                        "No problem.", "Wait, I changed my mind.");

                    if (player.Take(722817) && player.Take(722818) && player.Take(722819) && player.Earn(9000))
                    {
                        player.Inform("Congratulations! You received 9000 silvers.");
                    }
                    break;

                case 1:
                    await dialog.Confirm("Oh, what are you going to exchange for?", "SaviorPill");

                    await dialog.Confirm("SaviorPill? I do have one. It is so precious that I will exchange it for three antiques.",
                        "Ok, what are they?");

                    switch (await dialog.Ask("TangFresco, KingSword and AmberCup.", "Aren`t you asking for too much?",
                        "Ok, it is a deal."))
                    {
                        case 0:
                            dialog.Say("Well. It is up to you whether to exchange it or not.", "Then forget it.");
                            break;

                        case 1:
                            await dialog.Confirm("Great! Here is the Savior Pill. And I will take away the antiques.", "Thank you!");

                            if (player.Take(722817) && player.Take(722818) && player.Take(722819) &&
                                player.Give(722809) is not null)
                            {
                                player.Inform("Gained a Savior Pill.");
                            }
                            break;
                    }
                    break;
            }
        }
        else if (player.Has(722818) || player.Has(722819))
        {
            switch (await dialog.Ask("Hi, man, you`ve got good stuffs. Are your antiques for sale?",
                "How much do you offer it?", "No. I wanna exchange for something."))
            {
                case 0:
                    await SellAntique();
                    break;

                case 1:
                    await NoIWannaExchange();
                    break;
            }
        }
        else
        {
            dialog.Say("What can I do for you?", "No. Thanks.");
        }

        async Task SellAntique()
        {
            await dialog.Confirm("How about 3000 silvers for one antique?", "Ok, it is a deal.", "The price is much too low.");

            if (player.Has(722818))
            {
                if (player.Take(722818) && player.Earn(3000))
                {
                    player.Inform("Congratulations! You received 3000 silvers.");
                }
            }
            else if (player.Has(722819))
            {
                if (player.Take(722819) && player.Earn(3000))
                {
                    player.Inform("Congratulations! You received 3000 silvers.");
                }
            }
            else if (player.Has(722817) && player.Take(722817) && player.Earn(3000))
            {
                player.Inform("Congratulations! You received 3000 silvers.");
            }
        }

        async Task NoIWannaExchange()
        {
            if (await dialog.Ask("Oh, what are you going to exchange for?", "Savior Pill") == 0)
            {
                dialog.Say("Yeah, you know the ropes. It is the most effective panacea. Unless you`ve got TangFresco, KingSword and AmberCup.",
                    "Ok, I will get them soon.");
            }
        }
    }

    [Npc(10056)]
    public static async Task Conductress(Player player, Dialog dialog)
    {
        switch (await dialog.Ask("Where are you heading for? I can teleport you for a price of 100 silver.",
            "Twin City", "Market", "Just passing by."))
        {
            case 0:
                if (player.Spend(100))
                {
                    player.Teleport(1015, 1010, 710);
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
                        player.Record(1015, 717, 577);
                        player.Teleport(1036, 266, 239);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have 1,000 silver.", "I see.");
                    }
                }
                else if (player.Spend(100))
                {
                    player.Record(1015, 717, 577);
                    player.Teleport(1036, 266, 239);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;
        }
    }

    [Npc(30115)]
    public static async Task GreenSnake(Player player, Dialog dialog)
    {
        dialog.Avatar = 110;

        await dialog.Confirm("Aha, What a beautiful scene. We are very lucky to have so many rare items.",
            "Devil, I must kill you.", "Ah, I had better run away.");

        await dialog.Confirm("If you spare my life, I will let you enter our territory. Once you break through the snake array, you may get our rare items.",
            "What is snake array?");

        await dialog.Confirm("This battle array consists of 17 islands. Our boss guards the last one. If you can defeat our boss, you may get the rare items.",
            "I will spare your life.");

        await dialog.Confirm("Many brave people have come for the rare item, but all are killed by us. Are you sure you want to challenge our battle array?",
            "I fear nothing.", "I changed my mind.");

        player.Teleport(1063, 449, 358, force: true, record: (1015, 717, 577));
    }

    [Npc(300010)]
    public static async Task Exorcist(Player player, Dialog dialog)
    {
        dialog.Avatar = 87;

        await dialog.Confirm("The Ancient Devil was sealed in this island, The seal`s power is very weak now. The devil is gonna wake up. Can you help us?",
            "How can I help you?", "Just passing by.");

        if (await dialog.Ask("First, get the 5 Amulets. Each amulet is protected by a Guard of different professions. Only if you are of the same\nprofession, can you challenge the Guard. So you had better ask your friend for help. After you gather the 5 Amulets,\nclick on the yellow marks on the ground to bring out the devil and its guards. Enable PK mode to kill them. Will you help us?",
            "Yes. I shall try.", "Let me think it over.") == 0)
        {
            player.Teleport(1052, 191, 232);
        }
    }

    [Npc(600001)]
    public static async Task Miner(Player player, Dialog dialog)
    {
        dialog.Avatar = 2;

        if (player.Has("MeteorTear"))
        {
            dialog.Say("Thanks for your news. Please take this meteor tear. I hope you are happy all your life.",
                "Thanks.");
        }
        else if (player.Has("SadMeteor"))
        {
            await dialog.Confirm("I cannot help crying. Where did you get this sad meteor? What happened?",
                "It is from Joe.", "Sorry, I cannot tell you.");

            dialog.Require(player.Has("SadMeteor"));

            await dialog.Confirm("Joe! You have seen him? Where is he now? Is everything going on well with him?",
                "Do not cry.", "Do not ruin the meteor!");

            dialog.Require(player.Has("SadMeteor"));

            player.Alert("Joe loves and misses you very much. He engraved his love on this meteor. He said you will understand after you see it.");

            await dialog.Confirm("It is said that true love can move the meteor to cry and turn it to meteor tear. I am so glad that Joe does love me.",
                "All will be fine.", "Return my meteor.");

            dialog.Require(player.Has("SadMeteor"));

            await dialog.Confirm("Joe, Can you hear me? I love you. wherever you go, whatever you do, I will be right here waiting for you.",
                "Sad love story. Sigh.");

            dialog.Require(player.Has("SadMeteor"));

            await dialog.Confirm("Since Joe enjoys traveling round the world, I only wish he is happy. I shall be very delighted Whenever he thinks of me.",
                "How about you then?");

            dialog.Require(player.Has("SadMeteor"));

            await dialog.Confirm("It is said a star in the sky represents a person on the earth. When I miss him, I can look at his star and it will twinkle.",
                "I hope Joe will return.");

            dialog.Require(player.Has("SadMeteor"));

            if (player.Take(721002))
            {
                player.Give(1088002);
            }
        }
        else if (player.Has("GuardianStar"))
        {
            dialog.Say("You have not found him yet? Wherever he goes and whatever he does, I shall be right here waiting for him.",
                "I am trying to find him.");
        }
        else
        {
            await dialog.Confirm("Should auld acquaintance be forgotten and never brought to mind? Should auld acquaintance be forgotten and auld lang syne?",
                "Why are you so sad?", "Nice song.");

            if (!player.Has("Milly`sLetter"))
            {
                dialog.Say("I miss you like crazy every time of every day. I love you deeply all my life. Why are you so cruel to break my heart.",
                    "I got to go.");
                return;
            }

            await dialog.Confirm("Thanks. I am so pleased to hear from my sister. I have not seen her for a long time. It is very thoughtful of her. Sigh!",
                "Why are you so sad?", "I had better leave now.");

            dialog.Require(player.Has("Milly`sLetter"));

            switch (await dialog.Ask("My lover has left me without any reason. I tried to find him but got no news since he left. Can you help me to find him?",
                "OK. I shall help you.", "Sorry, I am too busy."))
            {
                case 0:
                    dialog.Require(player.Has("Milly`sLetter"));

                    if (!player.Take(721000))
                    {
                        return;
                    }

                    if (await dialog.Ask("Thanks. Joe gave me this bag as our love token. Give it to him when you find him and tell him I am always here waiting for him.",
                        "I shall try my best!") == 0)
                    {
                        player.Give(721001);
                    }
                    break;

                case 1:
                    dialog.Require(player.Has("Milly`sLetter"));

                    if (player.Take(721000))
                    {
                        dialog.Say("Thank you for the letter. Good bye.", "See you.");
                    }
                    break;
            }
        }
    }

    [Npc(600062)]
    public static async Task GossiperWang(Player player, Dialog dialog)
    {
        dialog.Avatar = 28;

        if (!player.Has("Hankerchief"))
        {
            if (await dialog.Ask("LOL. It is ridiculous that cheesy Rogger can not fall asleep because of a woman.",
                "You are a real gossiper!", "Here is a letter for you.") != 1)
            {
                return;
            }

            if (!player.Has(723000))
            {
                dialog.Say("Ha-ha, you are kidding.", "It is only a joke.");
                return;
            }

            await dialog.Confirm("Mr. Pine again! He got a pair of shoes from me and then went saying nothing. Now come for the Moon Gem. I have no idea!",
                "Do you know everything!", "Oh, I come here in vain.");

            await dialog.Confirm("Ha-ha. You are sweet. I do know something. But it is not in Bird Island. Dr. Rogger in this city may know it exactly.",
                "Dr. Rogger?", "You are flamming again!");

            await dialog.Confirm("Yes, Rogger is a famous tea doctor. People from different places come to visit him, and nothing escapes his knowledge.\nI hear that he is distressed by insomnia recently. If you find a Balsamine to cure him, he may tell you where the Gem is.",
                "Balsamine? How to get it?", "I will give up.");

            await dialog.Confirm("A herborist said that Balsamine is dominated by Lonely Tyrant, the bandit king in Maple Forest. I will give you a sketch map.\nThis is my sister Rachel`s handkerchief. I draw the map in it. But you must return after you get it, or she will blame me.",
                "Ok, I will go.", "I got to go.");

            if (player.Take(723000))
            {
                player.Give(723001);
            }

            return;
        }

        if (await dialog.Ask("What a nice day. What brings you here?", "Return your hankerchief.", "Nothing.") == 0)
        {
            dialog.Say("Handkerchief? I do not need it. Take it away.", "Alright.");
        }
    }

    [Npc(600063)]
    public static async Task Roger(Player player, Dialog dialog)
    {
        dialog.Avatar = 25;

        if (player.Has(723003))
        {
            dialog.Say("If you get the unidentified letter, you can take my letter with you to ask my good friend William to identify it.",
                "Oh, I see.");
        }
        else
        {
            await dialog.Confirm("Where do I begin to tell a story of true love?", "Have a cup of tea first.");

            if (player.Has(723002))
            {
                if (!player.Has(723001))
                {
                    await dialog.Confirm("I am wondering what kind of flower you are carrying. It gives out mystic scent.",
                        "It is balsamine.", "Keep it secret.");

                    if (await dialog.Ask("Sigh! Even the magic balsamine can`t help. I am crazy about Rachel and can hardly get sleep without seeing her.",
                        "Poor guy.", "How can I help you?") == 1)
                    {
                        dialog.Say("I heard that Balsamine is good for health. Could you give it to Rachel? She lives in the furthest village of Bird Island.",
                            "It is ok.", "Sorry, I am busy.");
                    }

                    return;
                }

                if (await dialog.Ask("Isn`t it Rachel`s handkerchief? How do you get it? Why is it so dirty? Why do not you treasure it?",
                    "Inquire about MoonGem.", "It is from GossiperWang.") != 1)
                {
                    return;
                }

                if (await dialog.Ask("You mean GossiperWang? Please give it to me. It will remind me of her. Do you have any requirements?",
                    "I cannot give you.", "Here you are.") != 1)
                {
                    return;
                }

                await dialog.Confirm("The painting on the handkerchief is the map of the place where to find Balsamine. It is so considerate of him.\nThanks for the Balsamine and the handkerchief. Tell me what you need, I will do my best to help you.",
                    "Where is the MoonGem?");

                await dialog.Confirm("I heard that a master wrote a Moon Letter to indicate its location. Unfortunately, the letter has lost for years.\nThe other day, I happened to know that King of Blade Ghost in the desert once collected it.",
                    "How to get it?");

                await dialog.Confirm("The king of Blade Ghost is very foxy. He asked an artificer to forge one. They are alike and hard to be identified.\nYou may get it if you defeat him. I will recommend you to William in PhoenixCastle to distinguish the authenticity of the lette",
                    "Thank you, sir.", "I will give it up.");

                if (player.Take(723001) && player.Take(723002))
                {
                    player.Give(723003);
                }
            }
            else if (player.Has(723001))
            {
                await dialog.Confirm("Isn`t it Rachel`s Hankchief? How can you get it and why is it so dirty?", "I do not know.", "I did it.");

                if (await dialog.Ask("Please give it to me, I will give you 30,000 silver. It is my lover`s hankerchief.",
                    "No, I need it too.", "Alrigth, here you are.") != 1)
                {
                    return;
                }

                if (player.Take(723001))
                {
                    player.Earn(30000);
                }
            }
            else
            {
                dialog.Say("It is hot and I am so tired. So it goes out of business today.", "You are so lazy.");
            }
        }
    }

    [Npc(600064)]
    public static async Task Rachel(Player player, Dialog dialog)
    {
        dialog.Avatar = 158;

        if (await dialog.Ask("I live in the western of the Yongtse River, you live in the eastern. Miss you but can not see you. We only share the same river\nWhen will the river stop flowing? When will the time stop? I only know that we miss each other.",
            "People are sensitive.", "Why are you so sad?") != 1)
        {
            return;
        }

        if (player.Has(723002))
        {
            if (!player.Has(723001))
            {
                await dialog.Confirm("My parents died early. Nobody takes care of me except for Roger. But he got illed now. Is the herb in your hand Balsamine?",
                    "Yeah.", "It is only ordinary weed.");

                await dialog.Confirm("Rogger fell sick due to insomnia. I hear that it is good for curing insomnia. Could you sell it to me at 2,000 silver?",
                    "Ok. It is a deal.", "Let me think it over.");

                if (player.Take(723002))
                {
                    player.Earn(2000);
                }

                return;
            }

            await dialog.Confirm("Sigh, my parents died early. Nobody cares me except for Roger. But he fell ill because of insomnia. Your Balsamine can cure him",
                "Right.", "It is only usual weed.");

            await dialog.Confirm("The Hankerchief in your hand is mine. Where do you get it and can you return it to me?",
                "It is from GossiperWang.", "It is still helpful.");

            await dialog.Confirm("Oh, the map shows where you can find Balsamine. Give me Balsamine and Hankerchief and I will give you 2,000 silver.",
                "No problem.", "I won`t sell it.");

            if (player.Take(723001) && player.Take(723002))
            {
                player.Earn(2000);
            }
        }
        else if (player.Has(723001))
        {
            await dialog.Confirm("Isn`t it my handkerchief? How do you get it? Return it to me!", "Here you are.", "It is still useful.");

            if (player.Take(723001))
            {
                dialog.Say("Oh, my god. What did my brother print on it?", "You are his sister.");
            }
        }
        else
        {
            dialog.Say("Miss you but can not see you. We only share the same river.", "Well.");
        }
    }

    [Npc(600080)]
    public static async Task BoxerZao(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        await dialog.Confirm("If you are level 20 or above, you may level up at the training grounds. It does not consume any durability, life and mana.\nDo you want me to teleport you there? It is free for my guild members, and 1000 silver for others.",
            "Please teleport me there.", "Just passing by.");

        if (npc.OwnerId == (player.GuildId ?? 0))
        {
            player.Record(1015, 721, 576);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("When you gain experience at the training ground, you can only attack the dummies or stakes which level is lower than yours.",
                    "I see.");
            }
        }
        else if (player.Spend(1000))
        {
            player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(1000);
            player.Record(1015, 721, 576);

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
