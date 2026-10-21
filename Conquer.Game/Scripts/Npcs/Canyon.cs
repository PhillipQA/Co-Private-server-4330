namespace Conquer.Game.Scripts.Npcs;

public static class Canyon
{
    [Npc(126)]
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
            player.Teleport(1026, 138, 103);
        }
    }

    [Npc(422)]
    public static async Task OldQuarrier(Player player, Dialog dialog)
    {
        dialog.Avatar = 91;

        if (player.Has(1072031, 10))
        {
            await dialog.Confirm("Since you take 10 EuxeniteOres, I`ll refine Saltpeter for you.", "Thank you.", "Oh, wait.");

            if (!player.Has(1072031, 10))
            {
                dialog.Say("I`m busy. Please don`t bother me if you don`t have 10 EuxeniteOres.", "I`m very sorry.");
                return;
            }

            if (player.Take(1072031, 10) && player.Give(721262) is not null)
            {
                dialog.Say("My pleasure. Here you are.", "Thank you.");
            }
        }
        else if (player.Has(1072031))
        {
            if (await dialog.Ask("Ten EuxeniteOres are required to make a piece of Saltpeter. Please get ores ready.",
                "What is Saltpeter used for?", "OK.") == 0)
            {
                await WhatIsSaltpeterUsed();
            }
        }
        else
        {
            switch (await dialog.Ask("Norbert recommended me to you, did he?", "Yes. I come for Saltpeter.",
                "No. Just passing by."))
            {
                case 0:
                    dialog.Say("Let`s make long story short. 10 EuxeniteOres are required for a piece of Saltpeter. You can dig EuxeniteOres in the plain mine.",
                        "I will get some.");
                    break;

                case 1:
                    await WhatIsSaltpeterUsed();
                    break;
            }
        }

        async Task WhatIsSaltpeterUsed()
        {
            if (await dialog.Ask("Are you coming for Saltpeter? Since Norbert invented Bomb, I have been kept busy with supplying Saltpeter for him.",
                "What is the Bomb?", "Sorry to bother you.") == 0)
            {
                dialog.Say("It is powerful and used to bomb out City Gate. If you`re interested in it, you can visit Norbert in the small village of the fo",
                    "I see.");
            }
        }
    }

    private const uint StarSword = 723088;
    private const uint SoulStone = 723085;
    private const uint DarkHorn = 790001;

    private static readonly string[] Colors = ["a white", "a brown", "a black", "a red", "an azury", "a cyaneous", "a purple"];
    private static readonly string[] Dresses = ["PhoenixDress", "ElegantDress", "CelestialtDress"];

    private static readonly (string Text, string Thanks)[] Strongholds =
    [
        ("HellGate is shielded from poisonous fog, so you can`t approach it. But demons do not fear the gas. They may turn\ninto SoulStones after they die. If you get five stones for me, I`ll help you break through the gate. To protect the\nunrelated persons, I`ll send the others back as soon as the first 60 persons pass through the gate.",
            "Thank you."),
        ("HellHall is the very spot where the demons swear their oaths of allegiance to UltimatePluto. Everybody must do his\nbest to make a way out. Due to limited time, I can lead only 30 persons to HellCloister!",
            "Thanks."),
        ("You will be divided into two groups to attack from the left and the right flank of HellCloister. Kill the Wraith\nas many as you can, because you can`t reach BattleFormation until the amount of Wraith is decreased to a certain level.",
            "Thank you."),
        ("BattleFormation is protected by Syrens. After they are killed out, UlitimatePluto will appear. Make the best\neffort to kill him, get his DarkHorn and give it to me. I can disable the BattleFormation with it. Then I`ll send\nyou back. But if you fail to do it, we have to retreat and wait for another opportunity.\n",
            "Thanks.")
    ];

    [Npc(3215)]
    public static async Task SolarSaint(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Has(StarSword))
        {
            var dress = await dialog.Ask("I have three exotic dresses sent by my foreign friends. I treasure them very much. But I love my StarSword as well.\nCould I exchange one of the dress for the StarSword?",
                "I want PhoenixDress.", "I want ElegantDress.", "I want CelestialDress.", "I will keep the StarSword.");

            if (dress is < 0 or > 2)
            {
                return;
            }

            var color = (player.Roll(1, 8) - 1) % Colors.Length;

            if (player.Take(StarSword) && player.Give(137320 + 100 * (uint)color + 10 * (uint)dress) is not null)
            {
                player.Inform($"Congratulations! You`ve got {Colors[color]} {Dresses[dress]}.");
            }

            return;
        }

        switch (await dialog.Ask("Our ancestors exerted their utmost efforts and defeated the demons. Since then the world has been kept in peace for\nhundreds of years. Now the demons have come back and the world is getting into turbulence again.\n",
            "Could you tell me more?", "I am here to drive away the evils!", "I`d better leave."))
        {
            case 0:
                await TellTheLegend(player, dialog);
                break;

            case 1:
                await JoinTheAttack(player, dialog);
                break;
        }
    }

    private static async Task TellTheLegend(Player player, Dialog dialog)
    {
        if (Refused(player) ||
            await dialog.Ask("The decisive battle between human and demon broke out here. The ferocious battle lasted for seven days and\nnights. Countless heroes lost their lives. And the justise won. Afterwards we can live in peace for hundreds of years.",
                "And then?") != 0 ||
            Refused(player) ||
            await dialog.Ask("UltimatePluto, leader of the demons sworn to take revenge in one thousand years before he managed to run away. To\nprevent the demons coming back, I have been scouting their land cautiously. Yesterday I found something unwanted.\nIt seems that UltimatePluto has come round to endanger humans again.",
                "What should we do?") != 0 ||
            Refused(player))
        {
            return;
        }

        switch (await dialog.Ask("Fortunately UltimatePluto is still unfledged. He must resort to a Battle Formation for the moment. So I am about\nto organize an army too infiltrate his land and destroy his formation before he becomes stronger.",
            "I`ll join in you.", "I`d like to know more.", "Sigh, I`m helpless."))
        {
            case 0:
                await JoinTheAttack(player, dialog);
                break;

            case 1:
                await ExplainTheStrongholds(player, dialog);
                break;
        }
    }

    private static async Task ExplainTheStrongholds(Player player, Dialog dialog)
    {
        while (!Refused(player))
        {
            var topic = await dialog.Ask("You must kill UltimatePluto rapidly to get DarkHorn so that I can use it to disable the formation. Before you can do\nthat, you must break into HellGate, enter the HellHall, and fight through the HellCloister. Countless ferocious demons\nare watching those strongholds. I`ll give you some strategies on breaking through those fortresses if you like.\n",
                "I want to know HellGate", "I want to know HellHall", "I want to know HellCloister", "I want to know BattleFormation",
                "Thanks. I know how to do.");

            if (topic is < 0 or > 3 || Refused(player) || await dialog.Ask(Strongholds[topic].Text, Strongholds[topic].Thanks, "I`d like to know more.") != 1)
            {
                return;
            }
        }
    }

    private static async Task JoinTheAttack(Player player, Dialog dialog)
    {
        if (Refused(player))
        {
            return;
        }

        var now = player.World.Now;

        if (!now.IsWeekly(DayOfWeek.Monday, new(20, 0), DayOfWeek.Monday, new(20, 5)) &&
            !now.IsWeekly(DayOfWeek.Tuesday, new(12, 0), DayOfWeek.Tuesday, new(12, 5)) &&
            !now.IsWeekly(DayOfWeek.Wednesday, new(20, 0), DayOfWeek.Wednesday, new(20, 5)) &&
            !now.IsWeekly(DayOfWeek.Thursday, new(12, 0), DayOfWeek.Thursday, new(12, 5)))
        {
            if (await dialog.Ask("I`m glad to see that you are such a knightly person. Please come next time.", "OK.") == 0 && !Refused(player))
            {
                player.Inform("Find SolarSaint to take the quest from 20:00 to 20:05 every Mon and Wed, from 12:00 - 12:05 every Tues and Thur.");
            }

            return;
        }

        if (player.Level < 110)
        {
            if (await dialog.Ask("You`re not strong enough for the moment. Don`t lose your life in vain.", "I see.") == 0 && !Refused(player))
            {
                player.Inform("As long as you reach level110, you can enter the Dis City.");
            }

            return;
        }

        if (await dialog.Ask("It is high time to attack now. Are you ready?", "Yes.", "Wait a moment, please.") != 0 || Refused(player))
        {
            return;
        }

        TakeAll(player, SoulStone);
        TakeAll(player, DarkHorn);
        player.Record(1020, 566, 565);

        if (player.Teleport(4021, [(320, 501), (370, 440), (420, 392), (470, 343), (269, 503), (216, 450), (166, 400), (116, 350)]))
        {
            player.SetRegister(3, 0);

            dialog.Say("Kill the demons to gather five SoulStones. Then go northeast through HellBridge, and look for me at HellGate.",
                "I see.");
        }
    }

    private static bool Refused(Player player)
    {
        if (player.PkPoints <= 1000)
        {
            return false;
        }

        player.Inform("Error: Failed to meet condition.");
        return true;
    }

    private static void TakeAll(Player player, uint itemTypeId) =>
        player.Take(itemTypeId, player.Inventory.Count(item => item.ItemTypeId == itemTypeId));

    private static readonly ushort[] SecondRebirthResets =
        [1045, 1046, 1250, 1260, 1270, 1280, 1290, 1300, 1350, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040];

    private static readonly Dictionary<Profession, Master> Masters = new()
    {
        [Profession.TrojanMaster] = new(35,
            "You have reached the highest rank as Trojan Master. You can choose a reborn class from 5 classes.",
            [
                new(Profession.Trojan,
                    "If you get reborn as Trojan, you can keep all skills and learn CruelShade later. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1115, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4050],
                    Drop: [], Learn: [9876, 3050]),

                new(Profession.Warrior,
                    "If you get reborn as Warrior, you can keep Cyclone and SpiritHealing, and learn IronShirt. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4050],
                    Drop: [1015, 1115], Learn: [9876, 5100]),

                new(Profession.Archer,
                    "If you get reborn as Archer, you can keep Cyclone and SpiritHealing. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4050],
                    Drop: [1015, 1115], Learn: [9876]),

                new(Profession.WaterTaoist,
                    "If you get reborn as Water Taoist, you can keep Cyclone, SpiritHealing and Robot. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4050],
                    Drop: [1015, 1115], Learn: [9876]),

                new(Profession.FireTaoist,
                    "If you get reborn as Fire Taoist, you can keep Cyclone, SpiritHealing and Robot. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4050],
                    Drop: [1015, 1115], Learn: [9876])
            ]),

        [Profession.WarriorMaster] = new(35,
            "You have reached the highest rank as Warrior Master. You can choose a reborn class from 5 classes.",
            [
                new(Profession.Trojan,
                    "If you get reborn as Trojan, you can keep Accuracy, Roar and FlyingMoon. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4060],
                    Drop: [1020, 1025, 1051], Learn: [9876]),

                new(Profession.Warrior,
                    "If you get reborn as Warrior, you can keep all skills and have a chance to reflect the damage taken. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 1320, 3050, 3080, 3090, 4000, 4060],
                    Drop: [], Learn: [9876]),

                new(Profession.Archer,
                    "If you get reborn as Archer, you can keep Shield and Roar. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4060],
                    Drop: [1015, 1025, 1051, 1320], Learn: [9876]),

                new(Profession.WaterTaoist,
                    "If you get reborn as Water Taoist, you can keep Shield, Superman and Roar. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4060],
                    Drop: [1015, 1051, 1320], Learn: [9876]),

                new(Profession.FireTaoist,
                    "If you get reborn as Fire Taoist, you can keep Shield and Roar. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1085, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4060],
                    Drop: [1015, 1025, 1051, 1320], Learn: [9876])
                {
                    BodyFace = 15
                }
            ]),

        [Profession.ArcherMaster] = new(15,
            "You have reached the highest rank as Archer Master. You can choose a reborn class from 5 classes.",
            [
                new(Profession.Trojan,
                    "If you get reborn as Trojan, you can use low-level bows and PoisonousArrows. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1075, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4070],
                    Drop: [1020, 8000, 8001, 8002, 8003, 8030, 9000], Learn: [5002, 9876]),

                new(Profession.Warrior,
                    "If you get reborn as Warrior, you can use low-level bows and PoisonousArrows. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1075, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4070],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: [5002, 9876]),

                new(Profession.Archer,
                    "If you get reborn as Archer, you can keep all skills and learn FreezingArrow later. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1075, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4070, 8000, 8001, 8003, 8030, 9000],
                    Drop: [], Learn: [5000, 9876]),

                new(Profession.WaterTaoist,
                    "If you get reborn as Water Taoist, you can use low-level bows and PoisonousArrows. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1075, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4070],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: [5002, 9876]),

                new(Profession.FireTaoist,
                    "If you get reborn as Fire Taoist, you can use low-level bows and PoisonousArrows. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1075, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4070],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: [5002, 9876])
            ]),

        [Profession.WaterSaint] = new(15,
            "You have reached the highest rank as Water Saint. You can choose a reborn class from 5 classes.",
            [
                new(Profession.Trojan,
                    "If you get reborn as Trojan, you can keep Cure, StarOfAccuracy, MagicShield, Stigma and Meditation. Will you take this class?",
                    Reset: [1005, 1085, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4020],
                    Drop: [1000, 1001, 1010, 1020, 1025, 1050, 1055, 1075, 1100, 1120, 1125, 1170, 1175, 5001], Learn: [9876]),

                new(Profession.Warrior,
                    "If you get reborn as Warrior, you can keep Cure, StarOfAccuracy, MagicShield, Stigma and Meditation. Will you take this class?",
                    Reset: [1005, 1085, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4020],
                    Drop: [1000, 1001, 1010, 1050, 1055, 1075, 1100, 1120, 1125, 1170, 1175, 5001], Learn: [9876]),

                new(Profession.Archer,
                    "If you get reborn as Archer, you can keep Cure, Invisibility, MagicShield, Stigma and Meditation. Will you take this class?",
                    Reset: [1005, 1075, 1090, 1095, 1190, 1195, 3050, 3080, 3090, 4000, 4020],
                    Drop: [1000, 1001, 1010, 1025, 1050, 1055, 1085, 1100, 1120, 1125, 1170, 1175, 5001], Learn: [9876]),

                new(Profession.WaterTaoist,
                    "If you get reborn as Water Taoist, you can keep all skills and learn Pervade later. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1055, 1075, 1085, 1090, 1095, 1120, 1170, 1175, 1190, 1195, 3050, 3080, 3090, 4000, 4020],
                    Drop: [], Learn: [3090, 9876]),

                new(Profession.FireTaoist,
                    "If you get reborn as Fire Taoist, you can keep Revive, AdvancedCure, Invisibility and HealingRain . Will you take this class?",
                    Reset: [1055, 1075, 1120, 1175, 1190, 3050, 3080, 3090, 4000, 4020],
                    Drop: [1000, 1001, 1005, 1010, 1025, 1085, 1090, 1095, 1100, 1125, 1170, 1195, 5001], Learn: [9876])
            ]),

        [Profession.FireSaint] = new(15,
            "You have reached the highest rank as Fire Saint. You can choose a reborn class from 5 classes.",
            [
                new(Profession.Trojan,
                    "If you get reborn Trojan, you can keep Thunder, Fire, Cure and Meditation. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1055, 1075, 1175, 1190, 1195, 3050, 3080, 3090, 4000, 4010],
                    Drop: [1002, 1010, 1020, 1120, 1125, 1150, 1160, 1165, 1180, 5001], Learn: [9876]),

                new(Profession.Warrior,
                    "If you get reborn Warrior, you can keep Thunder, Fire, Cure and Meditation. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1055, 1075, 1175, 1190, 1195, 3050, 3080, 3090, 4000, 4010],
                    Drop: [1002, 1010, 1120, 1125, 1150, 1160, 1165, 1180, 5001], Learn: [9876]),

                new(Profession.Archer,
                    "If you get reborn Archer, you can keep Thunder, Fire, Cure and Meditation. Will you take this class?",
                    Reset: [1000, 1001, 1005, 1055, 1075, 1175, 1190, 1195, 3050, 3080, 3090, 4000, 4010],
                    Drop: [1002, 1010, 1120, 1125, 1150, 1160, 1165, 1180, 5001], Learn: [9876]),

                new(Profession.WaterTaoist,
                    "If you get reborn as Water Taoist, you can keep FireCircle. Will you take this class?",
                    Reset: [1055, 1075, 1120, 1175, 1190, 3050, 3080, 3090, 4000, 4010],
                    Drop: [1000, 1001, 1002, 1005, 1010, 1125, 1150, 1160, 1165, 1180, 1195, 5001], Learn: [9876]),

                new(Profession.FireTaoist,
                    "If you get reborn as Fire Taoist, you can keep all skills and learn Dodge later. Will you take this class?",
                    Reset: [1000, 1001, 1002, 1005, 1055, 1075, 1120, 1150, 1160, 1165, 1175, 1180, 1190, 1195, 3050, 3080, 3090, 4000, 4010],
                    Drop: [], Learn: [3080, 9876])
            ])
    };

    [Npc(3600)]
    public static async Task Alex(Player player, Dialog dialog)
    {
        if (player.PkPoints > 1000)
        {
            return;
        }

        dialog.Avatar = 35;

        if (player.Rebirths > 1)
        {
            dialog.Say("Wow! You are already a second-reborn person. Nice to meet you. I hope you can continue blessing the public and working hard.\n",
                "I will try my best.");
            return;
        }

        if (player.Rebirths == 0 || player.Level < 120)
        {
            dialog.Say("Sorry, you are ineligible for second rebirth. Please wait until you are a level 120 reborn person.\n", "I see.");
            return;
        }

        if (player.GetProgress(61, 0) != 4)
        {
            await PushIntoTheEvilAbyss(player, dialog);
            return;
        }

        if (await dialog.Ask("You`ve completed your quests with perfect performance. You can take your second rebirth now. After second rebirth, \nyou can bear unique aura and learn Bless, and enjoy many benefits, but may pay more efforts to level up.\n\n",
                "I want second rebirth", "I don`t want second rebirth") != 0 ||
            !IsReady(player, dialog))
        {
            return;
        }

        if (!Masters.TryGetValue(player.Profession, out var master))
        {
            dialog.Say("You haven`t reached the highest rank yet. Please work harder.", "I see.");
            return;
        }

        dialog.Avatar = master.Face;

        if (master.Offers.ElementAtOrDefault(await dialog.Ask(master.Classes, "Trojan", "Warrior", "Archer", "Water Taoist", "Fire Taoist",
                "Let me think it over")) is not { } offer ||
            await dialog.Ask(offer.Text, "Yes, I will", "Let me think it over") != 0)
        {
            return;
        }

        if (offer.BodyFace is { } face)
        {
            dialog.Avatar = face;
        }

        var body = await dialog.Ask("When you get reborn, you have a chance to change your body size. Which do you prefer?",
            "Small character", "Big character", "Let me think it over");

        if (body is not (0 or 1) || !IsReady(player, dialog) || !player.Rebirth(offer.Class, body + (player.IsFemale ? 1 : 3)))
        {
            return;
        }

        player.ResetMagics(offer.Reset);
        player.RemoveMagics(offer.Drop);

        foreach (var magic in offer.Learn)
        {
            player.Learn(magic);
        }

        player.UnlearnWeaponSkills();
        player.ResetMagics(SecondRebirthResets);
        player.ShowEffect("zf2-e128");
        player.World.Announce($"{player.Name} has passed all tests and completed second rebirth. Congratulations!", TalkChannel.Gm);
    }

    private static bool IsReady(Player player, Dialog dialog)
    {
        if (player.Worn(ItemPosition.Armor)?.Subtype == 137)
        {
            dialog.Avatar = 15;
            dialog.Say("You can`t wear this kind of dress to get reborn.", "Oh, I see.");
            return false;
        }

        if (player.FreeSlots == 0)
        {
            dialog.Avatar = 35;
            dialog.Say("There is no free space in your inventory. Please prepare one free slot first.", "I see.");
            return false;
        }

        return true;
    }

    private static async Task PushIntoTheEvilAbyss(Player player, Dialog dialog)
    {
        if (await dialog.Ask("You are eligible for second rebirth, but you need to complete a great quest to eliminate the evil and save the \nSorrowful souls in EvilAbyss first. After second rebirth, you can learn Bless and bear unique aura, and enjoy many benefits.\n\n",
                "What quest?", "I don`t want second rebirth.") != 0 ||
            Refused(player) ||
            await dialog.Ask("You need to go through many tests in Evil Abyss. It is a very dangerous quest, moreover, after you complete your \n second rebirth, you will start anew and pay trible efforts to level up. You had better think it over before you \nmake such vital decisions.\n\n",
                "I decide to take this quest.", "I don`t want to risk my life.") != 0 ||
            Refused(player))
        {
            return;
        }

        if (player.PkPoints >= 100)
        {
            player.Inform("You`ve killed too many people. You can`t enter the EvilAbyss.");
            return;
        }

        player.Inform("Alex gave you a hard push. You`ve fallen into the Evil Abyss.");

        var (x, y) = player.Pick<(ushort, ushort)>([(260, 718), (459, 815), (598, 981), (863, 874), (728, 291), (827, 486), (992, 593),
            (434, 460)]);

        if (player.LedTeamSize > 1)
        {
            player.TeleportTeam(1700, x, y);
        }
        else
        {
            player.Teleport(1700, x, y);
        }
    }

    private sealed record Master(byte Face, string Classes, Offer[] Offers);

    private sealed record Offer(Profession Class, string Text, ushort[] Reset, ushort[] Drop, ushort[] Learn)
    {
        public byte? BodyFace { get; init; }
    }

    [Npc(3610)]
    public static async Task Carl(Player player, Dialog dialog)
    {
        dialog.Avatar = 62;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (await dialog.Ask("God has sent me here to collect a WhiteFlower and\nRedFlower, but I don`t know where to get them. I only know\nthat they are made of 6 petals,a Pistil and Stalk. If you\ncan find them for me, I will be very grateful to you and\nreward you.\n",
            "I`ve got to go.", "I`ve just got them.") != 1)
        {
            return;
        }

        dialog.Require(player.PkPoints <= 1000);

        if (player.Has(722732))
        {
            if (!player.Has(722736))
            {
                dialog.Say("Sorry, there is no WhiteFlower in your inventory. Don`t try to cheat me.",
                    "Sorry, I forgot to bring.");
                return;
            }

            await dialog.Confirm("Yeah, they are the right flowers I`m looking for. Thank you very much. I give you my token. You may exchange it for\nsome good prizes in the future. Please keep it well.\n",
                "Thanks a lot.");

            dialog.Require(player.PkPoints <= 1000);

            if (player.Take(722732) && player.Take(722736) && player.Give(722740) is not null)
            {
                player.Inform("You`ve got a HatefulToken!");
            }
        }
        else if (player.Has(722736))
        {
            dialog.Say("Sorry, there is no RedFlower in your inventory. Don`t try to cheat me.",
                "Sorry, I forgot to bring.");
        }
        else
        {
            dialog.Say("Sorry, there is no RedFlower or WhiteFlower in your inventory. Don`t try to cheat me.",
                "Sorry, I forgot to bring.");
        }
    }

    [Npc(10037)]
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
            await PleaseMakeOneFor();
        }

        async Task PleaseMakeOneFor()
        {
            if (!player.Spend(100))
            {
                dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                return;
            }

            if (player.Give(1060031) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10038)]
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
            await PleaseMakeOneFor();
        }

        async Task PleaseMakeOneFor()
        {
            if (!player.Spend(100))
            {
                dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                return;
            }

            if (player.Give(1060032) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10039)]
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
            await PleaseMakeOneFor();
        }

        async Task PleaseMakeOneFor()
        {
            if (!player.Spend(100))
            {
                dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                return;
            }

            if (player.Give(1060033) is not null)
            {
                dialog.Say("After you right click on this scroll to use, you will be teleported here from any place.",
                    "I see.");
            }
        }
    }

    [Npc(10053)]
    public static async Task Conductress(Player player, Dialog dialog)
    {
        switch (await dialog.Ask("Where are you heading for? I can teleport you for a price of 100 silver.",
            "Twin City", "Market", "Just passing by."))
        {
            case 0:
                if (player.Spend(100))
                {
                    player.Teleport(1020, 381, 21);
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
                        player.Record(1020, 566, 565);
                        player.Teleport(1036, 266, 239);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have 1,000 silver.", "I see.");
                    }
                }
                else if (player.Spend(100))
                {
                    player.Record(1020, 566, 565);
                    player.Teleport(1036, 266, 239);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 100 silver.", "I see.");
                }
                break;
        }
    }

    [Npc(20001)]
    public static async Task SoldierLeader(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has(721211) || !player.Has(721210))
        {
            dialog.Say("The life here is too dull and dangerous. I must go crazy if I stay any longer.",
                "I would rather die.");
            return;
        }

        await dialog.Confirm("You are looking for Ryan? Sorry, he sacrificed his life on his first duty. This is his item. Please give it to his relatives.",
            "Okay.", "Sorry, I do not know him.");

        if (player.Give(721211) is not null)
        {
            dialog.Say("Nobody wants to be a soldier, but we must defend our homeland against invasion. Many soldiers have sacrificed their lives.",
                "What a troubled world!");
        }
    }

    [Npc(30100)]
    public static async Task MountainKing(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has("SnakemanPro."))
        {
            await dialog.Confirm("You have got rid of our big trouble. Please take the money as rewards.", "Thank you.");

            dialog.Require(player.Has("SnakemanPro."));

            if (player.Take(721130))
            {
                player.Earn(3000);
            }
        }
        else if (player.Has("ChallengeLetter"))
        {
            dialog.Say("Please be very careful, the snakemen are known for their cruelty.", "Do not worry about me.");
        }
        else if (player.Has("ToMountainKing"))
        {
            await dialog.Confirm("Located in a remote mountain, we cannot get support from the army. So the snakemen often harry us and make us in misery.",
                "What can I do for you?", "Poor you.");

            dialog.Require(player.Has("ToMountainKing"));

            await dialog.Confirm("We do not have soldiers to send to defeat the snakemen. I would appreciate if you can help us defeat them.",
                "You can trust me.", "It is dangerous.");

            dialog.Require(player.Has("ToMountainKing"));

            if (player.Take(721122))
            {
                player.Give(721123);
            }
        }
        else
        {
            dialog.Say("The area is dangerous. You`d better leave here right now if you have nothing special to do.",
                "I am not a coward.");
        }
    }

    [Npc(30101)]
    public static async Task SnakemanLeader(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (!player.Has("ChallengeLetter"))
        {
            dialog.Say("Man, you are lucky. You would be my dinner if it was not for my satisfactory stomach.",
                "Are you scaring me?");
            return;
        }

        await dialog.Confirm("We are indifferent to the human being, and we never harry them.", "Who did it?");

        if (player.Has("ChallengeLetter"))
        {
            dialog.Say("The other day, heresy snakemen came. They never talk to us. I reckon they did it. Their color is different to ours.",
                "I shall go to find them.");
        }
        else
        {
            player.Inform("Error: Failed to meet condition.");
        }
    }

    [Npc(30105)]
    public static async Task HeresyLeader(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has(721130))
        {
            dialog.Say("With the written promise, we shall not harry human being any more.", "Well.");
        }
        else if (player.Has(721124))
        {
            if (!player.Has(721123))
            {
                dialog.Say("You do have some stuff. Even my tough men have given in.", "Just a piece of cake.");
                return;
            }

            await dialog.Confirm("I have thought you were unable to overcome us. I promise you that we will stick to this area in the future.",
                "I want a written promise.");

            await dialog.Confirm("That is alright. We always keep our promise.", "Give it to me.");

            if (player.Take(721123) && player.Take(721124))
            {
                player.Give(721130);
            }
        }
        else if (player.Has(721123))
        {
            await dialog.Confirm("Human being are coward. They are frightened to death and send out a novice like you. We shall eat you up sooner or later.",
                "You are arrogant.");

            if (await dialog.Ask("In my clan, everyone is cruel and tough. It is no way that human can defeat us, let alone a single effort from you.",
                "Let us wait and see.") == 0)
            {
                dialog.Say("OK. If you can defeat my men, I promise you not to harry human being.",
                    "Do not break your promise.");
            }
        }
        else
        {
            dialog.Say("Get out of my sight, or I shall eat you.", "Well.");
        }
    }

    [Npc(30111)]
    public static async Task BlacksmithLi(Player player, Dialog dialog)
    {
        dialog.Avatar = 34;

        if (player.Has(721136))
        {
            await dialog.Confirm("Have you got the stone spirit?", "Yes, here you are.", "Not yet.");

            if (!player.Has(721132, 5))
            {
                dialog.Say("Sorry, you do not have 5 stone spirits.", "I see.");
                return;
            }

            if (!player.Take(721136) || !player.Take(721132, 5))
            {
                return;
            }

            if (await dialog.Ask("Well done. This is what I have made. I am glad to give it to you as a reward.",
                "Thanks a lot.") == 0)
            {
                player.Give(player.Pick<uint>(900306, 900406, 900506, 900606, 900706, 900806, 900906, 900306));
            }
        }
        else if (player.Has(721131))
        {
            await dialog.Confirm("Oh, You are introduced by Mr. Leisure . He is not a reliable man. He has introduced many people, but no one can be helpful.",
                "I am different.", "You have prejudice.");

            await dialog.Confirm("Someone ordered from me some high quality weapons. However, I run out of the special material. I need you to get me some more.",
                "What material do you need?");

            await dialog.Confirm("It is Stone spirit. You may get some from rock monsters in the mine cave of southeastern Maple Forest. Five spirits will do.",
                "I can make it.");

            if (player.Take(721131))
            {
                player.Give(721136);
            }
        }
        else
        {
            dialog.Say("Nowadays, you cannot count on a person to do a good job. Nobody can take care of such a small case.",
                "You should do it yourself.");
        }
    }

    [Npc(30113)]
    public static async Task Soldier(Player player, Dialog dialog)
    {
        dialog.Avatar = 14;

        if (player.Has(721135))
        {
            dialog.Say("If he is not in the castle, you may find him near the village of southeastern Maple Forest. The fire rats are gathering there.",
                "Thanks a lot.");
        }
        else if (player.Has(721134))
        {
            await dialog.Confirm("Are you here to give him the official letter? He has gone hunting and will not come back soon.",
                "Where can I find him?", "What a bad luck.");

            await dialog.Confirm("It is hard to say. He often goes hunting near the village of southeastern Maple Forest. You can try your luck there.",
                "Thanks.");

            if (player.Give(721135) is not null)
            {
                dialog.Say("You had better take my letter with you, or he will not talk to you.", "Thank you.");
            }
        }
        else
        {
            dialog.Say("Our General has gone hunting alone. We are here to guard the castle.", "I see.");
        }
    }

    [Npc(30121)]
    public static async Task Hades(Player player, Dialog dialog)
    {
        dialog.Avatar = 73;

        if (player.Has(721152))
        {
            dialog.Say("Hope Lauren will come back soon.", "Wish you good luck.");
        }
        else if (player.Has(721151))
        {
            await dialog.Confirm("I know where WaterRing is. It belongs to Lauren, a human being, and it refuses to return to me. Because of it,\nLauren was enchanted and tortured by illness. If only she dies, it can come back to me.",
                "Are you going to kill her?", "Oh. Excuse me for a moment.");

            await dialog.Confirm("On the opposite, I went to the human world and bumpted into Lauren. I attempted to cheat WaterRing out of her. But\nthings went agaist my will. I fell in love with her. In order to prolong her life, I went back to Ghostdom and\nmodified the Roll of Metempsychosis. Just before I left, Lauren was taken by Tientsin from the kingdom of god.",
                "Why not hurry to rescue her?");

            await dialog.Confirm("I wish I could. A ghost does not have the access to the kingdom of god. If you want WaterRing, take my letter\nto DevineArtisan, and he will give you the Seal. With it, you can find Lauren and help her to take off WaterRing.\nSince I have changed the Roll of Metempsychosis, Tientsin can do nothing but set Lauren free if DevineArtisan gets WaterRing.",
                "I see. Where is Lauren?", "Sorry. I have got to go.");

            await dialog.Confirm("She was enjailed in the deepest of Violet Jail, the main jail of Tientsin.",
                "I shall find Tientsin first.", "Oh, I have to go.");

            await dialog.Confirm("Yeah, Tienstin stays at the peak of Dreamland every day from 13:00 to 18:00. If you get the passport, you can enter Violet Jail",
                "Ok. I will help you out.", "Oh, I am busy at the time.");

            if (player.Take(721151))
            {
                player.Give(721152);
            }
        }
        else
        {
            dialog.Say("There is only one thing that can reverse the universe. That is love.", "It sounds reasonable.");
        }
    }

    [Npc(600068)]
    public static async Task OldGeneral(Player player, Dialog dialog)
    {
        dialog.Avatar = 91;

        if (player.Has("PureGem"))
        {
            dialog.Say("MoonGem is very good for Tao to practice his spells. You should make good use of it.",
                "Thanks. I will.");
        }
        else if (player.Has(723009))
        {
            await dialog.Confirm("This is the MoonLetter which has lost for years. It seems that William has got the meaning in the letter.\nThe moon above Ape Mountain and in the midnight? It must indicate the position of the Phython.",
                "It sounds reasonable.");

            await dialog.Confirm("It is a pity that my soldiers can not go out to kill the Phython for they have to guard the city. Can you help me to kill it?\nIf you help me out, I will give you a MoonGem on behalf of the citizens.",
                "I will have a try.", "No, it is too dangerous.");

            await dialog.Confirm("Phython has a heeler called Emily. She usually dressed up like a beauty outside the city. She may send you to phython`s cave.\nFor fear that Phython disguises as you to get the Gem, take his heart and this AidToken to me after killing it.",
                "Thanks for reminding.", "I will give it up.");

            if (player.Take(723009))
            {
                player.Give(723010);
            }
        }
        else if (player.Has(723006))
        {
            dialog.Say("It is MoonLetter. But I am old and my eyes are not good enough. Can you find someone to explain the letter?",
                "Ok.");
        }
        else if (player.Has(723010))
        {
            if (!player.Has(723011))
            {
                dialog.Say("You can find Emily outside the city between 11:00 pm and 1:00 am. You must take care.",
                    "Thanks.");
                return;
            }

            await dialog.Confirm("It is the heart of Phython. You are so brave to kill Phython. I admire you so much.\nNow the force of the monsters is weakening and we will be in peace one day. I will give you a refined MoonGem.",
                "Thanks a lot.");

            if (player.Take(723010) && player.Take(723011))
            {
                player.Give(700062);
            }
        }
        else
        {
            await dialog.Confirm("Hi, my friend! Could you do me a favor?", "Sure. What is it?", "Sorry, I am busy.");

            await dialog.Confirm("It starts from Moon Gem. 500 years ago, my master faced down an ape to guard the city. That is why it was named Ape Mountain.\nHe left four supernatural gems to guard the four cities when he advanced to an immortal. And Moon Gem safeguarded this city.",
                "It is great, isn`t it?");

            if (await dialog.Ask("Yeah. But ever since endless monsters were attempting to snatch the gem. We can always defeat them and guard the city.\nRecently, a Python plotted Emily to entice my son. He almost got it. Then he called in monsters to besiege the city.\nI sent strong armies to raise the siege of it. But there was no news afterwards. It`s a pity we do not know where his den is.",
                "What can I do for you?", "It is bad. I must go.") == 0)
            {
                dialog.Say("My master once left a secret letter and told me to resort to it when we encounter troubles. But the letter was lost years ago.\nIf I could find the MoonLetter, it would be much easier.",
                    "Let me help you.");
            }
        }
    }

    [Npc(600079)]
    public static async Task BoxerWang(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        await dialog.Confirm("If you are level 20 or above, you may level up at the training grounds. It does not consume any durability, life and mana.\nDo you want me to teleport you there? It is free for my guild members, and 1000 silver for others.",
            "Please teleport me there.", "Just passing by.");

        if (npc.OwnerId == (player.GuildId ?? 0))
        {
            player.Record(1020, 566, 565);

            if (player.Teleport(1039, 217, 215))
            {
                dialog.Say("When you gain experience at the training ground, you can only attack the dummies or stakes which level is lower than yours.",
                    "I see.");
            }
        }
        else if (player.Spend(1000))
        {
            player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(1000);
            player.Record(1020, 566, 565);

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
