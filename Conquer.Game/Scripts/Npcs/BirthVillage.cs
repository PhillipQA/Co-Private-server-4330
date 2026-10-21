namespace Conquer.Game.Scripts.Npcs;

public static class BirthVillage
{
    [Npc(425)]
    public static async Task OldGeneralYang(Player player, Dialog dialog)
    {
        dialog.Avatar = 91;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Profession is not (Profession.InternTrojan or Profession.InternWarrior))
        {
            dialog.Say("I am here to teach weapon skill to Warrior and Trojan. Sorry that I am unable to help you.",
                "Thanks anyway.");
            return;
        }

        if (!player.HasTask(1))
        {
            if (await dialog.Ask("My old fighting days were called to mind at sight of you. Let me tell you some knacks to protect you for the future.",
                "Thank you for your teaching.", "Stroll in the other place.") == 0)
            {
                await LearnWeaponSkill();
            }

            return;
        }

        if (await dialog.Ask("KnowItAll on the bridge will teleport you to Twin City to start playing. I`ve a strong feeling that you`ll be a hero.",
            "I have questions still.", "Let`s say goodbye.") == 0)
        {
            await IHaveQuestionsStill();
        }

        async Task IHaveQuestionsStill()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            switch (await dialog.Ask("Well, what questions?", "Where to get weapons from?",
                "Where to learn skills from?", "No problem."))
            {
                case 0:
                    if (player.PkPoints <= 1000)
                    {
                        dialog.Say("Pedlar in Twin City sells all sorts of elementary weapons. For better weapons you need buy from Blacksmith.\nIf you are lucky enough you may get weapons after the monsters you`ve killed.",
                            "Thank you.");
                    }
                    else
                    {
                        player.Inform("Error: Failed to meet condition.");
                    }
                    break;

                case 1:
                    if (player.PkPoints <= 1000)
                    {
                        dialog.Say("NPCs in Job Center of Twin City will teach you skills and spells. Monsters may drop skill books too. Pharmacist in\nthe Market also sell them at considerable price.",
                            "Thank you.");
                    }
                    else
                    {
                        player.Inform("Error: Failed to meet condition.");
                    }
                    break;
            }
        }

        async Task LearnWeaponSkill()
        {
            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("I`ll teach you a weapon skill. Are you ready?", "I`m ready.");

            dialog.Require(player.PkPoints <= 1000);

            switch (player.Roll(1, 8))
            {
                case 1 or 5:
                    player.PlaySound("sound/train.wav");

                    dialog.Later(2, async () =>
                    {
                        player.Inform("You`ve learned WideStrike!");

                        dialog.Later(2, async () =>
                        {
                            player.Learn(1250);

                            if (player.Give(510005) is null)
                            {
                                return;
                            }

                            player.SetTask(1, true);

                            if (await dialog.Ask("You are so talented that you`ve learned WideStrike. I will send you a WoodGlaive. You can arm it after some\ntrainings. WideStrike will be activated automatically and become more powerful as you use glaives.",
                                "Thank you very much!") == 0)
                            {
                                await ThankYouVeryMuch();
                            }
                        });
                    });
                    break;

                case 2 or 6:
                    player.PlaySound("sound/train.wav");

                    dialog.Later(2, async () =>
                    {
                        player.Inform("You`ve learned Boreas!");

                        dialog.Later(2, async () =>
                        {
                            player.Learn(5050);

                            if (player.Give(530005) is null)
                            {
                                return;
                            }

                            player.SetTask(1, true);

                            if (await dialog.Ask("You are so talented that you have learned Boreas quickly. I present you a WoodPoleAxe that you can arm after some\ntrainings. Boreas will be activated automatically as you use axes. If you make more exercises, it`ll be more and more powerful!",
                                "Thank you very much!") == 0)
                            {
                                await ThankYouVeryMuch();
                            }
                        });
                    });
                    break;

                case 3 or 7:
                    player.PlaySound("sound/train.wav");

                    dialog.Later(2, async () =>
                    {
                        player.Inform("You`ve learned Halt!");

                        dialog.Later(2, async () =>
                        {
                            player.Learn(1300);

                            if (player.Give(540005) is null)
                            {
                                return;
                            }

                            player.SetTask(1, true);

                            if (await dialog.Ask("You are so talented that you`ve learned Halt. I send you a RatanLonghammer that you can arm after some trainings.\nHalt will be activated automatically as you use longhammer. If you make more exercises, it`ll become more powerful.",
                                "Thank you very much!") == 0)
                            {
                                await ThankYouVeryMuch();
                            }
                        });
                    });
                    break;

                case 4 or 8:
                    player.PlaySound("sound/train.wav");

                    dialog.Later(2, async () =>
                    {
                        player.Inform("You`ve learned StrandedMonster!");

                        dialog.Later(2, async () =>
                        {
                            player.Learn(5020);

                            if (player.Give(580005) is null)
                            {
                                return;
                            }

                            player.SetTask(1, true);

                            if (await dialog.Ask("You`re so genius that you`ve learned StrandedMonster quickly. I send you a WoodHammer that you can arm after\nafter some trainings. StrandedMonster will launch automatically as you use hammers. If you exercise more, it`ll be stronger!",
                                "Thank you very much!") == 0)
                            {
                                await ThankYouVeryMuch();
                            }
                        });
                    });
                    break;
            }
        }

        async Task ThankYouVeryMuch()
        {
            if (player.PkPoints > 1000)
            {
                player.Inform("Error: Failed to meet condition.");
                return;
            }

            if (await dialog.Ask("KnowItAll on the bridge will teleport you to Twin City to start playing. I`ve a strong feeling that you`ll be a hero.",
                "I have questions still.", "Let`s say goodbye.") == 0)
            {
                await IHaveQuestionsStill();
            }
        }
    }

    [Npc(1060)]
    public static async Task Guide(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        await dialog.Confirm("Hi, are you new to Conquer Online? What can I do for you?", "Where am I?");

        switch (await dialog.Ask("This is the way to Birth Village, where people can teach you some basic knowledge of the game and your profession.\nAnd they may also teach you useful spells.",
            "How to go to the village?", "I want to skip the Birth Village and start playing."))
        {
            case 0:
                if (await dialog.Ask("Follow the stone path all the way to the right. You can switch to run by click on the Walk/Run button on the left\nbottom of this screen. You can also jump by pressing Ctrl on you keyboard. I can give you a ride if you like.",
                    "Cool. Please send me to the village.", "Thanks. I`d rather go by myself.") == 0)
                {
                    await SendToVillage();
                }
                break;

            case 1:
                if (player.Profession == Profession.InternTaoist)
                {
                    if (await dialog.Ask("You`re a Taoist! I`ll recommend you to TaoistStar. He can teach you some basic spells.",
                        "Thanks a lot.") == 0)
                    {
                        await SendToVillage();
                    }

                    return;
                }

                player.Give(410301);

                if (player.Roll(100) < 50)
                {
                    player.Give(132305);
                }
                else
                {
                    player.Give(132405);
                }

                player.Give(1000000, 3);

                if (player.Teleport(1002, 440, 390))
                {
                    dialog.Say("I give you a Coat, Wooden Sword and some healing potions. Wish you a pleasant journey.",
                        "I see. Thanks.");
                }
                break;
        }

        async Task SendToVillage()
        {
            if (player.Teleport(1010, 83, 59))
            {
                dialog.Say("Here it is. You see the flickering PK in you lower screen? It means you are in PK mode. Switch it to Peace. By the\nway, KnowItAll can send you to Twin City where you can start playing. And he will also give you some starting gears and money.",
                    "Thanks a lot.");
            }
        }
    }

    [Npc(10004)]
    public static async Task BirthManager(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Profession == Profession.InternTaoist)
        {
            if (await dialog.Ask("There are NPCs standing around the town. You may click on them to sell, buy, repair items, learn info, get promoted...",
                "How about my job?", "Consult others.") == 0)
            {
                dialog.Say("You have chosen a great class. Taoists are good at spells. When you are level 40, you may select fire or water path.\nYou have chosen a great class. The merchants will tell you what the best items are. Ask Mr. Nosy and KnowItAll for more info.",
                    "I see. Thanks.");
            }
        }
        else if (player.Profession == Profession.InternWarrior)
        {
            if (await dialog.Ask("There are NPCs standing around the town. You may click on them to sell, buy, repair items, learn info, get promoted...",
                "How about my job?", "Consult others.") == 0)
            {
                dialog.Say("It`s a great class. Warrior prefers melee. Only warrior can wear shield and helmet. So warrior is the best defender.\nYou have chosen a great class. The merchants will tell you what the best items are. Ask Mr. Nosy and KnowItAll for more info.",
                    "I see. Thanks.");
            }
        }
        else if (player.Profession == Profession.InternTrojan)
        {
            if (await dialog.Ask("There are NPCs standing around the town. You may click on them to sell, buy, repair items, learn info, get promoted...",
                "How about my job?", "Consult others.") == 0)
            {
                dialog.Say("You made the right choice. Trojan is able to use two handed weapons and delivers more damages. Trojan has higher HP.\nYou have chosen a great class. The merchants will tell you what the best items are. Ask Mr. Nosy and KnowItAll for more info.",
                    "I see. Thanks.");
            }
        }
        else if (player.Profession == Profession.InternArcher)
        {
            if (await dialog.Ask("There are NPCs standing around the town. You may click on them to sell, buy, repair items, learn info, get promoted...",
                "How about my job?", "Consult others.") == 0)
            {
                dialog.Say("Archers are good at bow and arrow. They are fast, adept at long range attack and rapid shoot. They can also fly.\nYou have chosen a great class. The merchants will tell you what the best items are. Ask Mr. Nosy and KnowItAll for more info.",
                    "I see. Thanks.");
            }
        }
        else if ((int)player.Profession == 30)
        {
            if (await dialog.Ask("There are NPCs standing around the town. You may click on them to sell, buy, repair items, learn info, get promoted...",
                "How about my job?", "Consult others.") == 0)
            {
                dialog.Say("With rich skills and unique mana, Knight always takes advantages of either distance or close combat.\nYou have chosen a great class. The merchants will tell you what the best items are. Ask Mr. Nosy and KnowItAll for more info.",
                    "I see. Thanks.");
            }
        }
        else
        {
            dialog.Say("I am so pleased to learn that you have achieved such great success in a very short period. Congrats!",
                "Thanks.");
        }
    }

    [Npc(10005)]
    public static async Task Blacksmith(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("I am selling different weapons in different city. In order to slay the enemies, you had better equip the best weapons.",
            "How to buy and sell?", "Consult others.");

        await dialog.Confirm("Before you buy an item, you`d better check its stats. Red stats means you cannot wear it until all stats are white.\nClick on me to open my shop window, and then right click on an items to buy it. To equip it, just right click on it.\nIf you want to sell an item, you may click on me, then drag and drop your item into my slots. I have multipage items.",
            "How to repair?", "Consult others.");

        if (await dialog.Ask("Disarm your item, click on a shopkeeper, then click on repair button and your item. The better the quality, the higher the fee.",
            "What are super items?", "Consult others.") == 0)
        {
            dialog.Say("Items are graded as normal, refined, unique, elite and super. NPC sells only normal items. Mobs gives better ones.\nThat is all. If you have not talked to other NPCs, you had better have a chat with them so that you can learn more.",
                "Thanks.");
        }
    }

    [Npc(10006)]
    public static async Task Warehouseman(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (await dialog.Ask("Welcome! I run a warehouse in every city. You can store your money and items in my warehouses, and retrieve them for free.",
            "How to use the warehouse.", "Consult others.") == 0)
        {
            dialog.Say("To deposit money, just click on me, enter the amount, and then click on Deposit. Withdrawing is in the same way.\nTo store an item, just drag it to my slots and then release it. To take it out, just click on it. There is one\nwarehouse available in each city and the market. You had better store your valuable items and do not carry too much money.",
                "I see. Thanks.");
        }
    }

    [Npc(10007)]
    public static async Task Armorer(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (await dialog.Ask("Glad to meet you. I am selling different armors in different city. To level up quickly, you had better equip the best armors.",
            "How to buy and sell?", "Consult others.") == 0)
        {
            dialog.Say("Right click on an armor to buy it. Drag it to the shop window to sell it. Different armors give different stats.\nThat is all. If you have not talked to other NPCs, you had better have a chat with them so that you can learn more.",
                "I see. Thanks.");
        }
    }

    [Npc(10008)]
    public static async Task Pharmacist(Player player, Dialog dialog)
    {
        if (await dialog.Ask("Hi! I am selling all kinds of potions and City Gate Scrolls in the cities. I also sell fireworks and skill books in the market.",
            "What potions?", "Consult others.") == 0)
        {
            dialog.Say("Healing and mana potions. Healing potions can make you healthy, and mana potions will enable you to cast spells.\nThat is all. If you have not talked to other NPCs, you had better have a chat with them so that you can learn more.",
                "I see. Thanks.");
        }
    }

    [Npc(10009)]
    public static async Task MrNosy(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Profession == Profession.InternTaoist)
        {
            if (await dialog.Ask("Hey! You have chosen Taoist class. Taoist are skilled at spells. You can cast spells to kill enemies from a distance.",
                "Please tell me more.", "Consult others.") == 0)
            {
                dialog.Say("You can learn basic spells from Taoist Star and learn more in Job Center of Twin City later. Do not let enemies\napproach you.To cast a spell, just click on Skill button to select a spell, then right click on the enemies within the\nattack range. Take mana potions and HP potions when you are hunting. Go to get promoted in Job Center after level 15.\nIf a monster`s in Green name, it`s weaker than you; in white, it`s about the same; in red, it`s stronger; in black, it`s deadly",
                    "Thanks.");
            }
        }
        else if (player.Profession == Profession.InternWarrior)
        {
            if (await dialog.Ask("You chose to be a brave warrior. A great choice! Warrior can wear all kinds of weapons and armors, and is good at close combat.",
                "Please tell me more.", "Consult others.") == 0)
            {
                dialog.Say("Warrior is skilled at melee. I suggest you equip the best armors and weapons, then click on the enemies to hit.\nAfter you reach level 15, you can go to the Job Center in Twin City to get promoted and learn XP skills.\nIf a monster`s in Green name, it`s weaker than you; in white, it`s about the same; in red, it`s stronger; in black, it`s deadly",
                    "Thanks.");
            }
        }
        else if (player.Profession == Profession.InternTrojan)
        {
            if (await dialog.Ask("Welcome! A great choice. Trojans have more health points than any other class, and Trojans can equip two handed weapons.",
                "Please tell me more.", "Consult others.") == 0)
            {
                dialog.Say("Remember to equip good armors and weapons, carry enough potions, and then go to the wild to kill monsters.\nAfter you reach level 15, you can go to the Job Center in Twin City to get promoted and learn XP skills.\nIf a monster`s in Green name, it`s weaker than you; in white, it`s about the same; in red, it`s stronger; in black, it`s deadly",
                    "Thanks.");
            }
        }
        else if (player.Profession == Profession.InternArcher)
        {
            if (await dialog.Ask("Archers use bow and arrow to shoot. They can fly and learn special skills, fire flames at any who dare cross their path.",
                "Please tell me more.", "Consult others.") == 0)
            {
                dialog.Say("Archers use arrows to shoot, so do not forget to carry enough arrows and Healing potions with you. As you\nare weak at melee, you had better get away from a group of monsters and attack from distance.\nIf a monster`s in Green name, it`s weaker than you; in white, it`s about the same; in red, it`s stronger; in black, it`s deadly",
                    "Thanks.");
            }
        }
        else if ((int)player.Profession == 30)
        {
            if (await dialog.Ask("You have chosen to be a knight. A great choice.", "Please tell more.",
                "Consult others.") == 0)
            {
                dialog.Say("As knights use mana to attack enemies, mana is very important to knights.\nattack range. Take mana potions and HP potions when you are hunting. Go to get promoted in Job Center after level 15.\nIf a monster`s in Green name, it`s weaker than you; in white, it`s about the same; in red, it`s stronger; in black, it`s deadly",
                    "Thanks.");
            }
        }
        else
        {
            dialog.Say("Welcome back! Is everything going on well with you? My advice must have been of great help to you.",
                "Your advice is helpful.");
        }
    }

    [Npc(10010)]
    public static async Task KnowItAll(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Profession == Profession.InternTaoist)
        {
            if (!player.Knows(1000))
            {
                dialog.Say("The circle is very dangerous. You had better ask Taoist Star to teach you elementary spells before you leave Birth Village.",
                    "I see. Thanks.");
                return;
            }

            if (player.Level >= 15)
            {
                if (await dialog.Ask("Welcome back! Is everything going on well with you? My advice must have been of great help to you.",
                    "Teleport me to Twin City.") == 0)
                {
                    await TeleportToTwinCity();
                }

                return;
            }

            switch (await dialog.Ask("This is the way to Twin City. Many people are gathering there. Shall I give you some advice before I teleport you there.",
                "Yes, please.", "Teleport me to Twin City.", "Consult others."))
            {
                case 0:
                    await YesPlease();
                    break;

                case 1:
                    await TeleportToTwinCity();
                    break;
            }
        }
        else if (player.Profession is Profession.InternTrojan or Profession.InternWarrior)
        {
            if (!player.HasTask(1))
            {
                dialog.Say("Firstly you`d better visit OldGeneralYang nearby who is a warmhearted man and is ready to help.",
                    "OK, I`m going.");
                return;
            }

            if (player.Level >= 15)
            {
                if (await dialog.Ask("Welcome back! Is everything going on well with you? My advice must have been of great help to you.",
                    "Teleport me to Twin City.") == 0)
                {
                    await TeleportToTwinCity();
                }

                return;
            }

            switch (await dialog.Ask("This is the way to Twin City. Many people are gathering there. Shall I give you some advice before I teleport you there.",
                "Yes, please.", "Teleport me to Twin City.", "Consult others."))
            {
                case 0:
                    await YesPlease();
                    break;

                case 1:
                    await TeleportToTwinCity();
                    break;
            }
        }
        else if (player.Level < 15)
        {
            switch (await dialog.Ask("This is the way to Twin City. Many people are gathering there. Shall I give you some advice before I teleport you there.",
                "Yes, please.", "Teleport me to Twin City.", "Consult others."))
            {
                case 0:
                    await YesPlease();
                    break;

                case 1:
                    await TeleportToTwinCity();
                    break;
            }
        }
        else
        {
            if (await dialog.Ask("Welcome back! Is everything going on well with you? My advice must have been of great help to you.",
                "Teleport me to Twin City.") == 0)
            {
                await TeleportToTwinCity();
            }
        }

        async Task YesPlease()
        {
            dialog.Say("Make good use of Hot Key (F1-F10) will save you a lot of troubles. You can drag potions and spells to F1-F10. Then\nyou just press on the corresponding key on your keyboard to use it. You like PK? Be careful. If you get blue even\nblack name after you kill other players, you will lose the equipments you are wearing. Remember to switch PK to Peace.\nThat is all. Have a good journey.",
                "Thanks.");
        }

        async Task TeleportToTwinCity()
        {
            if (player.Profession == Profession.InternTrojan)
            {
                if (player.Give(410301) is null)
                {
                    return;
                }

                var itemTypeId = player.Pick<uint>(132304, 132404, 132504, 132604, 132704, 132804, 132904);

                if (player.Give(itemTypeId) is null || !player.Give(1000000, 3))
                {
                    return;
                }

                player.SetTask(1, false);

                if (player.Teleport(1002, 440, 390))
                {
                    dialog.Say("I will give you some weapons and drugs. Don`t forget to use them. Enjoy Conquer online!",
                        "Thanks.");
                }
            }
            else if (player.Profession == Profession.InternWarrior)
            {
                if (player.Give(410301) is null)
                {
                    return;
                }

                var itemTypeId = player.Pick<uint>(132304, 132404, 132504, 132604, 132704, 132804, 132904);

                if (player.Give(itemTypeId) is null || !player.Give(1000000, 3))
                {
                    return;
                }

                player.SetTask(1, false);

                if (player.Teleport(1002, 440, 390))
                {
                    dialog.Say("I will give you some weapons and drugs. Don`t forget to use them. Enjoy Conquer online!",
                        "Thanks.");
                }
            }
            else if (player.Profession == Profession.InternArcher)
            {
                if (player.Give(500301) is null)
                {
                    return;
                }

                var itemTypeId = player.Pick<uint>(132304, 132404, 132504, 132604, 132704, 132804, 132904);

                if (player.Give(itemTypeId) is not null && player.Give(1000000, 3) && player.Give(1050000, 10) &&
                    player.Teleport(1002, 440, 390))
                {
                    dialog.Say("I will give you some weapons and drugs. Remember that WoodArrow can`t be used until you arm it. Enjoy Conquer online!",
                        "Thanks.");
                }
            }
            else if (player.Profession == Profession.InternTaoist && player.Give(421301) is not null)
            {
                var itemTypeId = player.Pick<uint>(132304, 132404, 132504, 132604, 132704, 132804, 132904);

                if (player.Give(itemTypeId) is not null && player.Give(1000000, 3) && player.Give(1001000, 3) &&
                    player.Teleport(1002, 440, 390))
                {
                    dialog.Say("I will give you some weapons and drugs. Remember to click on the Skill button to select a spell to cast. You can\nalso drag the skill icon to the hot key F1-F10 to use it expediently. Enjoy Conquer online!",
                        "Thanks a lot!");
                }
            }
        }
    }

    [Npc(10055)]
    public static async Task TaoistStar(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Profession != Profession.InternTaoist)
        {
            dialog.Say("Sorry, you are not Taoist. I am here to teach Taoist some elementary spells.",
                "I see. Thanks.");
            return;
        }

        if (await dialog.Ask("I can teach you Thunder and Cure. After you learn, you may cast Thunder to kill enemies, use Cure to heal yourself and others.",
            "I want to learn.", "Just passing by.") == 0)
        {
            player.Learn(1000);
            player.Learn(1005);

            dialog.Say("You have learned Thunder and Cure. Please remember that spells are only used to punish the devils and help the kind people.",
                "I see. Thanks.");
        }
    }
}
