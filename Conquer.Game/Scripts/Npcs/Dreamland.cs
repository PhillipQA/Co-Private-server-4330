namespace Conquer.Game.Scripts.Npcs;

public static class Dreamland
{
    [Npc(30055)]
    public static async Task Daniel(Player player, Dialog dialog)
    {
        dialog.Avatar = 8;

        if (player.Has(721109))
        {
            await dialog.Confirm("Congratulations! You have gained the prize token of Sky Pass. I wil send you to claim the prize.", "Thanks.");

            if (player.Has("SkyPrizeToken"))
            {
                player.Teleport(1040, 192, 250);
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }

            return;
        }

        await dialog.Confirm("This is the way to Sky Pass. Are you here to challenge the Sky Pass?", "Please show me the way.", "Just passing by.");

        await dialog.Confirm("The Sky Pass was designed by my master Maggie. She trapped the strongest monsters in the Pass to test out the real hero.",
            "Can I have a try?");

        await dialog.Confirm("Sure. Everyone has a chance to challenge the pass. If you pass 5 floors at a time, you will see our great master.",
            "What does it look like?", "I got to go. Goodbye.");

        await dialog.Confirm("The Pass consists of 5 floors, and each floor is guarded by the monsters of different level.", "How can I pass?");

        await dialog.Confirm("If you are lucky enough, the guard will teleport you to the next floor; or you will be teleported to the battle stage.",
            "Give me some advice.");

        await dialog.Confirm("Player of level 90 or above can choose from two battle stages. It is more possible to bypass the battle in the tougher stage.",
            "I would like to try.", "I see. Thanks. Bye.");

        player.Teleport(1040, 595, 383, record: (1020, 566, 565));
    }

    [Npc(30120)]
    public static async Task DevineArtisan(Player player, Dialog dialog)
    {
        dialog.Avatar = 48;

        if (player.Has(721150))
        {
            await dialog.Confirm("Thank you for retrieve WaterRing. Here is my token of gratitude. Please take it.", "Thanks a lot.");

            if (!player.Take(721150))
            {
                return;
            }

            if (player.Roll(2) < 1)
            {
                if (player.Roll(5) < 1)
                {
                    player.Give(150137);
                }
                else
                {
                    player.Give(150136);
                }
            }
            else if (player.Roll(5) < 1)
            {
                player.Give(152127);
            }
            else
            {
                player.Give(152126);
            }
        }
        else if (player.Has(721152))
        {
            await dialog.Confirm("Hades made so bold as to change the Roll of Metempsychosis. But that is none of my business. Take my Seal to fetch WaterRing.",
                "Ok, I will be right back.");

            if (player.Take(721152))
            {
                player.Give(721153);
            }
        }
        else if (player.Has(721151))
        {
            dialog.Say("Hades will know why you come to him at the sight of my letter.", "Oh, I see.");
        }
        else
        {
            await dialog.Confirm("There are five worlds since the beginning: human world, ghostdom, spirit world, kingdom of god and devildom.\nAs ages passed, and things changed, the communication among the five worlds died out gradually.",
                "And then?", "Oh, nice story.");

            await dialog.Confirm("I conjured all the power of the universe and made five mighty treasures: HeartMirror, KeyWand, LoveLoop,\nUniverseSword and WaterRing, in the hope that they will unite the five worlds again.",
                "Isn`t that good?");

            if (await dialog.Ask("But things went out of my control. The five worlds coveted the treasures and launched wars to snatch them.",
                "Pity! I got to go.", "How do you deal with them?") != 1)
            {
                return;
            }

            await dialog.Confirm("Retrieve the 5 treasures and seal them first. Then I`ll unseal them and give them to the 5 worlds, so they will reunite again.",
                "It won`t be hard for you.");

            await dialog.Confirm("Yeah. I`ve taken back four. But WaterRing, having been held by Hades, disappeared from the world. He must be looking\nfor it too.I was wondering if you could help me find WaterRing back?",
                "No problem.");

            if (player.Give(721151) is not null)
            {
                dialog.Say("Thanks very much. Hades lives in the Love Canyon. You may get some clues from him.",
                    "Ok. I will try my best.");
            }
        }
    }

    [Npc(30123)]
    public static async Task JailGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 53;

        if (!player.Has(721156))
        {
            dialog.Say("Without my master`s passport, you can`t access to the prison.", "Then forget it.");
            return;
        }

        await dialog.Confirm("Will you go to Violet Jail?", "Yes.");

        await dialog.Confirm("Please don`t blame me for not reminding you. Monsters in the Jail are fierce.", "Thank you.");

        if (player.Take(721156))
        {
            player.Teleport(1061, 447, 251, force: true);
        }
    }

    private const string NoStone = "I cannot help you with the rebirth for you don`t have a CelestialStone.";
    private const string Dressed = "You can`t wear this kind of dress to get reborn.";

    private static readonly string[] Classes = ["Trojan", "Warrior", "Archer", "Water Taoist", "Fire Taoist", "Let me think it over."];

    private static readonly string[] SpacedGems =
        ["Super Phoenix Gem", "Super Dragon Gem", "Super Fury Gem", "Super Rainbow Gem", "Super Kylin Gem", "Super Violet Gem", "Super Moon Gem"];

    private static readonly Prompt Appearance =
        new("You have a chance to change your appearance. Which do you prefer?", "Small character", "Big character", "Let me think it over.");

    private static readonly Prompt AlsoAppearance =
        new("You can also change your appearance. Which one do you prefer?", "Small character.", "Big character.", "Let me think it over.");

    private static readonly Prompt Refinable = new("Your Celestial Stone can be refined as a super gem during rebirth. Which gem do you prefer?", SpacedGems);

    private static readonly Prompt RefinedToGem = new("Your CelestialStone is refined to a gem in the rebirth. Which gem do you like best?", SpacedGems);

    private static readonly Prompt Returnable = new("The Celestial Stone will return to a super gem during the rebirth. Which one do you prefer?",
        "Super PhoenixGem", "Super DragonGem", "Super FuryGem", "Super RainbowGem", "Super KylinGem", "Super VioletGem", "Super MoonGem");

    private static readonly Dictionary<Profession, (uint ItemTypeId, byte Magic)> Gifts = new()
    {
        [Profession.Trojan] = (410077, 200),
        [Profession.Warrior] = (410077, 200),
        [Profession.Archer] = (500067, 203),
        [Profession.WaterTaoist] = (421077, 202),
        [Profession.FireTaoist] = (421077, 201)
    };

    private static readonly Dictionary<Profession, Master> Masters = new()
    {
        [Profession.TrojanMaster] = new(120,
            "You have reached the highest rank sd Trojan Master. You can choose a reborn class from 5 classes.",
            "You are a Trojan King, there are five occupations for you to choose:",
            [
                new(Profession.Trojan,
                    "If you choose Trojan, you can keep all your skills and learn Cruel Shade. Will you get reborn as a Trojan?",
                    "You can remember all the skills at some time and learn CruelShade after the rebirth to a Trojan. Would you like to be a Trojan?",
                    Reset: [1045, 1046, 1115, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [], Learn: 3050)
                {
                    Yes = "Yes, I would.",
                    BodyType = "You also have a chance to change your body type. Which type will you choose?.",
                    Look = new("You can change your appearance. Which one do you like best?", "Small character.", "Big character.", "Let me think it over."),
                    Gems = [RefinedToGem, RefinedToGem]
                },

                new(Profession.Warrior,
                    "If you choose warrior, you can keep your Cyclone and Spirit Healing, and learn Iron Shirt. Will you get reborn as a Warrior?",
                    "If you are reborn as Warrior, you can keep Cyclone and Spirit Healing, and learn Iron Shirt. Do you want to be Warrior?",
                    Reset: [1045, 1046, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1115], Learn: 5100),

                new(Profession.Archer,
                    "If you choose Archer, you can keep your Cyclone and Spirit Healing. Will you get reborn as an Archer?",
                    "If you are reborn as Archer, you can keep Cyclone, Spirit Healing. Do you want to be Archer?",
                    Reset: [1045, 1046, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1115]),

                new(Profession.WaterTaoist,
                    "If you choose Water Taoist, you can keep your Cyclone, Spirit Healing and Robot. Will you get reborn as a Water Taoist?",
                    "If you are reborn as Water Taoist, you can keep Cyclone, Spirit Healing and Robot. Do you want to be Water Taoist?",
                    Reset: [1045, 1046, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1115]),

                new(Profession.FireTaoist,
                    "If you choose Fire Taoist, you can keep your Cyclone, Spirit Healing and Robot. Will you get reborn as a Fire Taoist?",
                    "If you are reborn as Fire Taoist, you can keep Cyclone, Spirit Healing and Robot. Do you want to be Fire Taoist?",
                    Reset: [1045, 1046, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1115])
                {
                    Look = new("You have a chance to change your build. Which do you prefer?", "Small character", "Big character", "Let me think it over.")
                }
            ]),

        [Profession.WarriorMaster] = new(120,
            "You have reached the highest rank as Warrior Master. You can choose a reborn class from 5 classes.",
            "You are a Warrior King, there are five occupations for you to choose:",
            [
                new(Profession.Trojan,
                    "If you choose Trojan, you can keep your Accuracy, Roar and Flying Moon. Will you get reborn as a Trojan?",
                    "If you are reborn as Trojan, you can keep Concentration, Howl, and Flying Moon. Do you want to be Trojan?",
                    Reset: [1045, 1046, 1250, 1260, 1290, 1300, 1320, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1020, 1025, 1051]),

                new(Profession.Warrior,
                    "If you choose Warrior, you can keep all your skills, and have a chance to reflect the damage. Will you get reborn as a Warrior?",
                    "If you are reborn as Warrior, you can keep all skills and may throw attack back later. Do you want to be  Warrior?",
                    Reset: [1045, 1046, 1051, 1250, 1260, 1290, 1300, 1320, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: []),

                new(Profession.Archer,
                    "If you choose Archer, you can keep your Shield and Roar. Will you get reborn as an Archer?",
                    "If you are reborn as Archer, you can keep Tri-Shield and Howl. Do you want to be Archer?",
                    Reset: [1045, 1046, 1250, 1260, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1025, 1051, 1320]),

                new(Profession.WaterTaoist,
                    "If you choose Water Taoist, you can keep your Shield, Superman and Roar. Will you get reborn as a Water Taoist?",
                    "If you are reborn as Water Taoist, you can keep Tri-Shield, Superman and Howl. Do you want to be Water Taoist?",
                    Reset: [1045, 1046, 1250, 1260, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1051, 1320]),

                new(Profession.FireTaoist,
                    "If you choose Fire Taoist, you can keep your Shield and Roar. Will you get reborn as a Fire Taoist?",
                    "If you are reborn as Fire Taoist, you can keep Shield and Roar. Do you want to be Fire Taoist?",
                    Reset: [1045, 1046, 1250, 1260, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [1015, 1025, 1051, 1320])
            ]),

        [Profession.ArcherMaster] = new(120,
            "You have reached the highest rank as Archer Master. You can choose a reborn class from 5 classes.",
            "You are an Archer King, there are five occupations for you to choose:",
            [
                new(Profession.Trojan,
                    "If you choose Trojan, you can use low-level bow and high-level poison arrows. Will you get reborn as a Trojan?",
                    "If you are reborn as Trojan, you can use low-level bows and Poisonous Arrows. Do you want to be Trojan?",
                    Reset: [1045, 1046, 1115, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: 5002),

                new(Profession.Warrior,
                    "If you choose Warrior, you can use low-level bow and high-level poison arrows. Will you get reborn as a Warrior?",
                    "If you are reborn as Warrior, you can use low-level bows and Poisonous Arrows. Do you want to be Warrior?",
                    Reset: [1045, 1046, 1115, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: 5002),

                new(Profession.Archer,
                    "If you choose Archer, you can keep all your skills and learn Freezing Arrow. Will you get reborn as an Archer?",
                    "If you are reborn as Archer, you can keep all skills and use Freezing Arrows. Do you want to be Archer?",
                    Reset: [1045, 1046, 1115, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040, 8000, 8001, 8003, 8030, 9000],
                    Drop: [], Learn: 5000),

                new(Profession.WaterTaoist,
                    "If you choose Water Taoist, you can use low-level bow and high-level poison arrows. Will you get reborn as a Water Taoist?",
                    "If you are reborn as Water Taoist, you can use low-level bows and Poisonous Arrows. Do you want to be Water Taoist?",
                    Reset: [1045, 1046, 1115, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: 5002)
                {
                    Look = new("You have a chance to change your appearance. Which do you prefer?", "Small character.", "Big character", "Let me think it over.")
                },

                new(Profession.FireTaoist,
                    "If you choose Fire Taoist, you can use low-level bow and high-level poison arrows. Will you get reborn as a Fire Taoist?",
                    "If you are reborn as Fire Taoist, you can use low-level bows and Poisonous Arrows. Do you want to be Fire Taoist?",
                    Reset: [1045, 1046, 1115, 1190, 1250, 1260, 1270, 1290, 1300, 1360, 5010, 5020, 5030, 5040, 5050, 7000, 7010, 7020, 7030, 7040],
                    Drop: [8000, 8001, 8002, 8003, 8030, 9000], Learn: 5002)
            ]),

        [Profession.WaterSaint] = new(110,
            "You have reached the highest rank as Water Saint. You can choose a reborn class from 5 classes.",
            "You are a Water Saint, there are five occupations for you to choose:",
            [
                new(Profession.Trojan,
                    "If you choose Trojan, you can keep your Cure, Star of Accuracy, Magic Shield, Stigma and Meditation. Will you choose Trojan?",
                    "If you are reborn as Trojan, you can keep Cure, Star of Accuracy, Magic Shield, Stigma and Meditation. Do you like it?",
                    Reset: [1005, 1045, 1046, 1085, 1090, 1095, 1195, 1280, 1350, 1360, 5030],
                    Drop: [1000, 1001, 1010, 1050, 1055, 1075, 1100, 1125, 1170, 1175, 5001])
                {
                    Look = new("You have a chance to change your configuration. Which do you prefer?", "Small character", "Big character", "Let me think it over."),
                    Gems = [new("Your Celestial Stone can be refined to a super gem during rebirth. Which gem do you prefer?", SpacedGems), Refinable]
                },

                new(Profession.Warrior,
                    "If you choose Warrior, you can keep your Cure, Star of Accuracy, Magic Shield, Stigma and Meditation. Will you choose Warrior?",
                    "If you are reborn as Warrior, you can keep Cure, Star of Accuracy, Magic Shield, Stigma and Meditation. Do you like it?",
                    Reset: [1005, 1045, 1046, 1085, 1090, 1095, 1195, 1280, 1350, 1360, 5030],
                    Drop: [1000, 1001, 1010, 1050, 1055, 1075, 1100, 1125, 1170, 1175, 5001]),

                new(Profession.Archer,
                    "If you choose Archer, you can keep your Cure, Invisibility, Magic Shield, Stigma and Meditation. Will you choose Archer?",
                    "If you are reborn as Archer, you can keep Cure, Invisibility, Magic Shield, Stigma and Meditation. Do you like it?",
                    Reset: [1005, 1045, 1046, 1075, 1090, 1095, 1195, 1280, 1350, 1360, 5030],
                    Drop: [1000, 1001, 1010, 1050, 1055, 1085, 1100, 1125, 1170, 1175, 5001]),

                new(Profession.WaterTaoist,
                    "If you choose Water Taoist, you can keep all your skills and learn Pervade. Will you get reborn as a Water Taoist?",
                    "If you are reborn as Water Taoist, you can keep all skills and learn Pervade later. Do you want to be a Water Taoist?",
                    Reset: [1000, 1001, 1005, 1010, 1045, 1046, 1050, 1055, 1075, 1085, 1090, 1095, 1100, 1125, 1170, 1175, 1195, 1280, 1350, 1360, 5001, 5030],
                    Drop: [], Learn: 3090),

                new(Profession.FireTaoist,
                    "If you choose Fire Taoist, you can keep your Revive, Advanced Cure, Invisibility and Healing Rain. Will you choose Fire Taoist?",
                    "If you are reborn as Fire Taoist, you can keep Resurrect, Advanced Cure, Invisibility and Healing Rain. Do you like it?",
                    Reset: [1045, 1046, 1055, 1075, 1175, 1280, 1350, 1360, 5030],
                    Drop: [1000, 1001, 1005, 1010, 1085, 1090, 1095, 1100, 1125, 1170, 1195, 5001])
            ]),

        [Profession.FireSaint] = new(120,
            "You have reached the highest rank as Fire Saint. You can choose a reborn class from 5 classes.",
            "You are a Fire Saint, there are five occupations for you to choose:",
            [
                new(Profession.Trojan,
                    "If you choose Trojan, you can keep your Thunder, Fire, Cure and Meditation. Will you get reborn as a Trojan?",
                    "If you are reborn as Trojan, you can keep Thunder Strike Bolt, Astral Fire bolt, Cure and Meditation. Do you want to be a Troja",
                    Reset: [1000, 1001, 1005, 1045, 1046, 1195, 1360, 5030],
                    Drop: [1002, 1010, 1120, 1125, 1150, 1160, 1165, 1180, 5001]),

                new(Profession.Warrior,
                    "If you choose Warrior, you can keep your Thunder, Fire, Cure and Meditation. Will you get reborn as a Warrior?",
                    "If you are reborn as Warrior, you can keep Thunder Strike Bolt, Astral Fire bolt, Cure and Meditation. Do you want to be a Warr",
                    Reset: [1000, 1001, 1005, 1045, 1046, 1195, 1360, 5030],
                    Drop: [1002, 1010, 1120, 1125, 1150, 1160, 1165, 1180, 5001])
                {
                    Gems = [new("Your Celestial Stone will be refined into a super gem during rebirth. Which gem do you prefer?", SpacedGems), Returnable]
                },

                new(Profession.Archer,
                    "If you choose Archer, you can keep your Thunder, Fire, Cure and Meditation. Will you get reborn as an Archer?",
                    "If you select Archer, you can retain the spell of Thunder, Fire, Cure and Meditation. Are you ready for rebirth to Archer?",
                    Reset: [1000, 1001, 1005, 1045, 1046, 1195, 1360, 5030],
                    Drop: [1002, 1010, 1120, 1125, 1150, 1160, 1165, 1180, 5001])
                {
                    Yes = "Yes, I am.",
                    Look = AlsoAppearance,
                    Gems = [Returnable, Returnable]
                },

                new(Profession.WaterTaoist,
                    "If you choose Water Taoist, you can keep your Fire Circle. Will you get reborn as a Water Taoist?",
                    "If you select Water Taoist, you can retain the spell of FireCircle. Are you ready for rebirth to that job?",
                    Reset: [1045, 1046, 1120, 1195, 1360, 5030],
                    Drop: [1000, 1001, 1002, 1005, 1010, 1125, 1150, 1160, 1165, 1180, 5001])
                {
                    Yes = "Yes, I am.",
                    Look = AlsoAppearance,
                    Gems = [Returnable, Returnable]
                },

                new(Profession.FireTaoist,
                    "If you choose Fire Taoist, you can keep all your skills and learn Dodge. Will you get reborn as a Fire Taoist?",
                    "If you select Fire Taoist, you can retain all the spells, and learn new skill Dodge. Are you ready for rebirth to that job?",
                    Reset: [1000, 1001, 1005, 1045, 1046, 1150, 1160, 1180, 1195, 1360, 5030],
                    Drop: [], Learn: 3080)
                {
                    Yes = "Yes. I am.",
                    Unlearn = [1002, 1120, 1165],
                    Look = AlsoAppearance,
                    Gems = [Returnable, Returnable]
                }
            ])
    };

    [Npc(300500)]
    public static async Task Eternity(Player player, Dialog dialog)
    {
        dialog.Avatar = 15;

        if (player.Rebirths != 0)
        {
            dialog.Say("You have got reborn. Please work harder.", "Oh, I see.");
            return;
        }

        await dialog.Confirm("I devote all my life to the research of eternity, finally I know the arcanum of rebirth of the life.",
            "I would like to know it.", "I just passed by here.");

        switch (await dialog.Ask("If you want to rebirth, you should reach a certain level, get the highest occupation title and get a CelestialStone.\nAfter the rebirth, you can distribute your attribute more freely. And you can learn more powerful skill.",
            "What is CelestialStone?", "Allot attribute freely.", "More powerful skill", "I would like to reborn.",
            "Let me think it over."))
        {
            case 0:
                dialog.Say("CelestialStone syncretizes seven gems in the world, and I will pave the way for rebirth.",
                    "I see.");
                break;

            case 1:
                dialog.Say("After the rebirth you start at level 15, and get some bonus points. You can distribute every ability points gained in level up.",
                    "I see.");
                break;

            case 2:
                dialog.Say("The skills before the rebirth will disappear. But some only disappear for a while, and it will come back when the level\nreaches half of the preexistence; you can learn a new skill if you remain the same occupation as preexistence.",
                    "I see.");
                break;

            case 3:
                switch (await dialog.Ask("You can choose from 2 ways to get reborn:", "Blessed rebirth.", "Normal rebirth."))
                {
                    case 0:
                        await BlessedRebirth(player, dialog);
                        break;

                    case 1:
                        await NormalRebirth(player, dialog);
                        break;
                }
                break;
        }
    }

    private static async Task BlessedRebirth(Player player, Dialog dialog)
    {
        var choice = await dialog.Ask("I`ve always dedicated to the blessed force of eternity, which has absorbed the essences of the universe. If you\nhave a Celestial Stone at rebirth, I can use it to add god blesses to your equipment.",
            "I want to learn more.", "I want blessed rebirth.", "Let me think it over.");

        if (choice == 0)
        {
            if (await dialog.Ask("Since Tortoise Gem appears with the essences of the universe, its holy aura has blessed all living beings. I\nhave mastered the holy force of Tortoise Gem. I can use Celestial Stone to add the spirit of Tortoise Gem to\nyour equipment, then you will have the god blesses and bear the holy aura.",
                    "Can blessed equipment reduce the damages taken?") == 0 &&
                await dialog.Ask("Sure. The blessed effects depend on your abilities. If you get reborn before level 120, I can only add  god blesses\nto your boots or weapons. If you get reborn after level 120, I can randomly add god blesses to the equipments you`re wearing.",
                    "More details.") == 0)
            {
                dialog.Say("Celestial Stone will be consumed during the process of adding god blesses, that it can`t be used to create a super\ngem. If you choose blessed rebirth, you can`t get a super gem.",
                    "I see. Thanks");
            }

            return;
        }

        if (choice != 1 ||
            await dialog.Ask("Your Celestial Stone will be consumed during the process of adding god blesses, that it can`t be used to create a\nsuper gem. As the holy force is so powerful, I can`t control it. I can only add god blesses to your equipments randomly.",
                "I agree.", "Let me think it over.") != 0 ||
            await dialog.Ask("Have you made up your mind to choose the force of god blesses?", "Yeah, I have.") != 0)
        {
            return;
        }

        if (player.FreeSlots < 2)
        {
            dialog.Say("You don`t have two free slots in your inventory. Please leave two free slots first.", "I see.");
            return;
        }

        if (!player.Has(ItemType.CelestialStone))
        {
            dialog.Say("I cannot help you with the rebirth for you have no CelestialStone.", "Oh, I see.");
            return;
        }

        if (player.Worn(ItemPosition.Armor)?.Subtype == 137)
        {
            dialog.Say(Dressed, "Oh, I see.");
            return;
        }

        if (MasterOf(player, dialog) is not { } master)
        {
            return;
        }

        var underLevel = player.Level < 120;

        if (underLevel &&
            await dialog.Ask("You have reached the highest rank. You may get reborn now, but you are under level 120, I can only add the mighty \nforce of god blesses to your boots or weapons randomly.",
                "I don`t care. Please help me get reborn.", "Let me think it over.") != 0)
        {
            return;
        }

        var classes = underLevel
            ? "Since you`ve made up your mind. I will be glad to help. You may choose a reborn class from 5 classes."
            : master.Blessed;

        if (master.Offers.ElementAtOrDefault(await dialog.Ask(classes, Classes)) is not { } offer ||
            await dialog.Ask(offer.Blessed, "Yes, I will.", "Let me think it over.") != 0)
        {
            return;
        }

        var body = await dialog.Ask(offer.BodyType, "I choose small character.", "I choose big character.", "Let me think it over.");

        if (body is not (0 or 1) ||
            await dialog.Ask("Since Celestial Stone has been turned into god blesses of eternity, it will lose its ability to create a super gem.",
                "I want god blesses.", "Let me think it over.") != 0)
        {
            return;
        }

        if (!player.Has(ItemType.CelestialStone))
        {
            dialog.Say(NoStone, "Oh, I see.");
            return;
        }

        if (player.Worn(ItemPosition.Armor)?.Subtype == 137)
        {
            dialog.Say(Dressed, "Oh, I see.");
            return;
        }

        if (!underLevel)
        {
            if (player.PickUnblessed() is { } position)
            {
                player.Bless(position, 1);
            }

            player.ShowEffect("Aegis1");
            Reborn(player, offer, body);
            return;
        }

        var (first, second) = player.Roll(2) < 1 ? (ItemPosition.Boots, ItemPosition.Right) : (ItemPosition.Right, ItemPosition.Boots);
        var blessed = Unblessed(player, first) ? first : second;
        var bootsFirst = first == ItemPosition.Boots && blessed == first;

        if (bootsFirst && body == 0 && offer.Class == Profession.WaterTaoist)
        {
            Reborn(player, offer, body);
            return;
        }

        if (Unblessed(player, blessed))
        {
            player.Bless(blessed, 1);
        }

        player.ShowEffect("Aegis1");

        if (bootsFirst && body == 0 && offer.Class == Profession.FireTaoist)
        {
            Reborn(player, master.Offers.Single(each => each.Class == Profession.WaterTaoist), 1);
            return;
        }

        Reborn(player, offer, body);
    }

    private static async Task NormalRebirth(Player player, Dialog dialog)
    {
        if (player.FreeSlots < 3)
        {
            dialog.Say("Please leave three places in your inventory.", "Oh, I see.");
            return;
        }

        if (!player.Has(ItemType.CelestialStone))
        {
            dialog.Say("I cannot help you with the rebirth for you have no CelestialStone.", "Oh, I see.");
            return;
        }

        if (MasterOf(player, dialog) is not { } master ||
            master.Offers.ElementAtOrDefault(await dialog.Ask(master.Normal, Classes)) is not { } offer ||
            await dialog.Ask(offer.Normal, offer.Yes, "Let me think it over.") != 0)
        {
            return;
        }

        var body = await dialog.Ask(offer.Look.Text, offer.Look.Options);

        if (body is not (0 or 1))
        {
            return;
        }

        if (!player.Has(ItemType.CelestialStone))
        {
            dialog.Say(NoStone, "Oh, I see.");
            return;
        }

        var gem = await dialog.Ask(offer.Gems[body].Text, offer.Gems[body].Options);
        var stoneTrusted = (player.Profession, offer.Class, body, gem) is (Profession.WarriorMaster, Profession.FireTaoist, 1, 0);

        if (!stoneTrusted && !player.Has(ItemType.CelestialStone))
        {
            dialog.Say(NoStone, "Oh, I see.");
            return;
        }

        if (player.Give(700003 + 10 * Math.Min((uint)gem, 6u)) is not null)
        {
            Reborn(player, offer, body);
        }
    }

    private static Master? MasterOf(Player player, Dialog dialog)
    {
        if (!Masters.TryGetValue(player.Profession, out var master))
        {
            dialog.Say("You have not got the highest title of your occupation, please work hard.", "Oh, I see.");
            return null;
        }

        if (player.Level < master.Level)
        {
            dialog.Say("Your Level is lower than the required. Please train harder.", "I see.");
            return null;
        }

        return master;
    }

    private static bool Unblessed(Player player, ItemPosition position) => player.ItemAt(position)?.Bless == 0;

    private static void Reborn(Player player, Offer offer, int body)
    {
        if (!player.Rebirth(offer.Class, body + (player.IsFemale ? 1 : 3)) || !player.Has(ItemType.CelestialStone))
        {
            return;
        }

        player.ResetMagics(offer.Reset);
        player.RemoveMagics(offer.Drop);
        player.UnlearnMagics(offer.Unlearn);

        if (offer.Learn is { } magic)
        {
            player.Learn(magic);
        }

        var (gift, power) = Gifts[offer.Class];
        player.Give(gift, 1, item => item.Magic1 = power);
        player.Take(ItemType.CelestialStone);
        player.Fireworks();
        player.UnlearnWeaponSkills();
        player.World.Announce($"Congratulations! {player.Name} got reborn!", TalkChannel.Gm);
    }

    private sealed record Prompt(string Text, params string[] Options);

    private sealed record Master(byte Level, string Blessed, string Normal, Offer[] Offers);

    private sealed record Offer(Profession Class, string Blessed, string Normal, ushort[] Reset, ushort[] Drop, ushort? Learn = null)
    {
        public string Yes { get; init; } = "Yeah.";
        public string BodyType { get; init; } = "You also have a chance to change your body type. Which type will you choose?";
        public Prompt Look { get; init; } = Appearance;
        public Prompt[] Gems { get; init; } = [Refinable, Refinable];
        public ushort[] Unlearn { get; init; } = [];
    }
}
