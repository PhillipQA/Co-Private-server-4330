namespace Conquer.Game.Scripts.Npcs;

public static class EvilAbyss
{
    [Npc(3601)]
    public static async Task Arthur(Player player, Dialog dialog)
    {
        dialog.Avatar = 32;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths == 2)
        {
            dialog.Say("Wow! You`ve completed your second rebirth. Glad to see you again.", "Nice to meet you.");
        }
        else if (player.Rebirths == 0)
        {
            dialog.Say("You haven`t completed your first rebirth yet. I only offer my service to the level 120+ reborn people.",
                "I see.");
        }
        else if (player.Level < 120)
        {
            dialog.Say("You haven`t reached level 120 yet. This place is very dangerous. You`d better leave here now.",
                "OK.");
        }
        else if (player.GetProgress(61, 0) >= 1)
        {
            dialog.Say("I feel I am full of strength. After thousands years` wait, my dreams have come true. Thanks a lot for your great \nhelp. I recommended you to Bryan. Let him guide you through your test journey.\n",
                "Thank you very much.");
        }
        else if (player.Has(722731, 3))
        {
            await dialog.Confirm("I am so glad that you`ve made my dreams come true. I will return to the heaven very soon. I`ve reported your \nmerits to Bryan. He will help you with your tests.\n",
                "Thank you very much.");

            dialog.Require(player.PkPoints <= 1000);

            if (player.Take(722731, 3))
            {
                player.SetProgress(61, 0, 1);
                player.SetProgress(61, 1, 0);
                player.Inform("Congratulations! You`ve completed the first stage of your tests.");
            }
        }
        else if (player.Has(722731))
        {
            switch (await dialog.Ask("I need three Pure Vigors to restore my force. What else can I do for you?",
                "Make an EvilTooth", "Make an ImmortalStone", "Make an ImpureVigor", "Just passing by."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722730))
        {
            switch (await dialog.Ask("I used to live in Heaven. Since I was trapped by devils, I have been here for thousands years and lost my \nsupernatural force. My force can be restored by three PureVigors. Take your ImpureVigor to Cleansing Stove, click\nthat stove to summon a CleansingDevil. Kill that devil to get a PureVigor.\n",
                "Make an EvilTooth.", "Make an ImmortalStone.", "Make an ImpureVigor", "I see."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722729))
        {
            switch (await dialog.Ask("Kill a Banshee to get a VigorFragment. After you have three VigorFragments, I can make an ImpureVigor for you.\n",
                "Make an EvilTooth.", "Make an ImmortalStone.", "Make an ImpureVigor", "I see."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722728))
        {
            switch (await dialog.Ask("Take an ImmortalStone, Moss, DreamGrass and SoulAroma to FireSeal. Click that seal to summon a Banshee. kill that \nBanshee to get a VigorFragment.\n",
                "Make an EvilTooth.", "Make an ImmortalStone.", "Make an ImpureVigor", "I see."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722726))
        {
            switch (await dialog.Ask("Kill a SwiftDevil to get a FeatherStone. After you have three FeatherStones, I can make an ImmortalStone for you.\n",
                "Make an EvilTooth.", "Make an ImmortalStone.", "Make an ImpureVigor", "I see."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722721))
        {
            switch (await dialog.Ask("Take an EvilTooth, Moss, DreamGrass and SoulAroma to WaterSeal. Click that seal to summon a SwiftDevil. kill \nthat Devil to get a FeatherStone.\n",
                "Make an EvilTooth.", "Make an ImmortalStone.", "Make an ImpureVigor", "I see."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722722))
        {
            switch (await dialog.Ask("Kill a HillSpirit to get a GhostHorn. After you have three GhostHorns, I can make an EvilTooth for you.\n",
                "Make an EvilTooth.", "Make an ImmortalStone.", "Make an ImpureVigor", "I see."))
            {
                case 0:
                    await MakeAnEvilTooth();
                    break;

                case 1:
                    await MakeAnImmortalStone();
                    break;

                case 2:
                    await MakeAnImpureVigor();
                    break;
            }
        }
        else if (player.Has(722723) || player.Has(722724) || player.Has(722725))
        {
            dialog.Say("After you get a Moss, DreamGrass and SoulAroma from HillSpirit`s minions, click EarthSeal to summon a \nHillSpirit. Kill that Spirit to get a GhostHorn.\n",
                "I see.");
        }
        else
        {
            await dialog.Confirm("Halt! This place is filled with danger. If you move forward, you will risk your life.",
                "I come to accept the tests.", "Too terrible.");

            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("So many people have come here for rebirth. All wanted to get reborn and start anew, but they didn`t know that they \nhad to go through many pains and dangers, they could lose their lives at any time. Since you can arrive here, you \nmust be powerful. Are you sure that you can pass the tests?",
                "Of course.", "Let me think it over.");

            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("The monsters here are very ferocious. They are commanded by some devils. These devils are hiding in the seals. \nThey won`t appear unless the minions summon them. If you can make their boss appear and kill the boss, you will be \neligible to accept the tests.",
                "What can I do?", "I changed my mind.");

            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("Get a Moss, DreamGrass and SoulAroma from HillSpirit`s\nminions, then click EarthSeal to summon a Hill Spirit.\nAfter you get three GhostHorns from HillSpirits, I can make\nan EvilTooth for you. Take an EvilTooth, Moss, DreamGrass\nand SoulAroma to WaterSeal. Click that seal to Summon a\nSwiftDevil.",
                "Is SwiftDevil their boss?");

            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("Their boss is Banshee hiding in FireSeal. Kill a SwiftDevil\nto get a FeatherStone. I can make an ImmortalStone from\nthree FeatherStones. Take an ImmortalStone, Moss,\nDreamGrass and SoulAroma to FireSeal, click that seal to\nsummon a Banshee. Kill that Banshee to get a VigorFragment.",
                "I`ve got to kill them.", "Too dangerous.");

            if (player.PkPoints <= 1000)
            {
                dialog.Say("Wait! That Banshee was born of the essence of the evil\nspirits. Your killing can only destroy part of her vigor.\nAfter a short rest, she can come out to do all kinds of\nevil. You need to bring 3 VigorFragments to prove your ability.",
                    "I see.");
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }
        }

        async Task MakeAnEvilTooth()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Has(722722, 3))
            {
                dialog.Say("If you want me to make an Evil Tooth for you, please bring three Ghost Horns from the Hill Spirits.",
                    "I will bring them soon.");
                return;
            }

            if (player.Take(722722, 3) && player.Give(722721) is not null)
            {
                player.Inform("You`ve got an Evil Tooth!");

                dialog.Say("After you have an EvilTooth, Moss, DreamGrass and SoulAroma, you may click Water Seal to summon a Swift \nDevil, kill that devil to get a FeatherStone.",
                    "I see.");
            }
        }

        async Task MakeAnImmortalStone()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Has(722726, 3))
            {
                dialog.Say("If you want me to make an ImmortalStone for you, please bring three FeatherStones from Swift Devils.",
                    "I will bring them soon.");
                return;
            }

            if (player.Take(722726, 3) && player.Give(722728) is not null)
            {
                player.Inform("You`ve got an ImmortalStone!");

                dialog.Say("After you have an ImmortalStone, Moss, DreamGrass and SoulAroma, you may click FireSeal to summon a Banshee, \nkill that Banshee to get a VigorFragment.",
                    "I see.");
            }
        }

        async Task MakeAnImpureVigor()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (!player.Has(722729, 3))
            {
                dialog.Say("If you want me to make an ImpureVigor for you, please bring three VigorFragments from Banshees.",
                    "I will bring them soon.");
                return;
            }

            if (player.Take(722729, 3) && player.Give(722730) is not null)
            {
                player.Inform("You`ve got an ImpureVigor!");

                dialog.Say("After you have an ImpureVigor, you may click CleansingStove to summon a CleansingDevil, kill that devil to get a PureVigor.",
                    "Thanks a lot.");
            }
        }
    }

    [Npc(3602)]
    public static async Task Bryan(Player player, Dialog dialog)
    {
        dialog.Avatar = 54;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths == 2)
        {
            dialog.Say("Wow! You have completed your second rebirth. You look so cool. I couldn`t recognize you at the first sight.",
                "Glad to hear that.");
        }
        else if (player.Rebirths == 0)
        {
            await dialog.Confirm("You haven`t completed your first rebirth yet. How dare you come here?", "I...I...");

            if (player.PkPoints <= 1000)
            {
                player.Inform("Bryan has wielded a sword at you. You are scared to run away.");
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }
        }
        else if (player.Level >= 120 && player.GetProgress(61, 0) >= 1)
        {
            if (player.GetProgress(61, 0) != 1)
            {
                dialog.Say("Well done! You`ve completed your quest with good merits. You have a chance to get reborn soon. Angela will guide \nyou through the next tests.\n",
                    "Thanks for your advice.");
                return;
            }

            if (player.GetProgress(61, 1) == 0)
            {
                await dialog.Confirm("The grievance of the dead souls here has formed a mountain of 40000 meters high. That mountain will sink for a few\nmeters for every monster here you kill. When that mountain is razed to the ground, you will release many dead souls\nfrom suffering. Your good merits will be infinite and boundless.\n\n",
                    "I will kill those monsters.", "Too difficult.");

                if (player.PkPoints <= 1000)
                {
                    dialog.Say("I will record your scores. If you want to check how high the mountain of grievance remains, you may return to check\nat any time.",
                        "I see.");
                }
                else
                {
                    player.Inform("Error: Failed to meet condition.");
                }
            }
            else if (player.GetProgress(61, 1) >= 40000)
            {
                player.SetProgress(61, 0, 2);
                player.SetProgress(61, 2, 0);
                player.SetProgress(61, 21, 0);

                await dialog.Confirm("Well done! You`ve completed your quest with good merits. You have a chance to get reborn soon. Angela will guide \nyou through the next tests.\n",
                    "Thanks for your advice.");

                if (player.PkPoints <= 1000)
                {
                    player.Inform("You have completed the second stage of your tests!");
                }
                else
                {
                    player.Inform("Error: Failed to meet condition.");
                }
            }
            else if (player.GetProgress(61, 11) == 1)
            {
                await dialog.Confirm("The mountain of grievance has sunk down, but you still need to work harder to raze it to the ground.\n",
                    "How high is it now?", "I am too tired.");

                if (player.PkPoints <= 1000)
                {
                    player.SetProgress(61, 11, 2);
                    player.SetRegister(6, 0);

                    dialog.Say($"The mountain of grievance has sunk for {player.GetRegister(6)} meters.\nMany souls of the dead have been released from suffering. Your merits and virtues will be remembered forever.",
                        "Glad to hear that.");
                }
                else
                {
                    player.Inform("Error: Failed to meet condition.");
                }
            }
            else
            {
                dialog.Say("Those souls of the dead haven`t been completely released from suffering. Please try to kill more monsters.",
                    "I will.");
            }
        }
        else
        {
            dialog.Say("You can`t take the second stage of this quest until you completed the first stage. Please work harder.\n",
                "I see.");
        }
    }

    [Npc(3603)]
    public static async Task Angela(Player player, Dialog dialog)
    {
        dialog.Avatar = 153;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths == 2)
        {
            dialog.Say("Wow! You`ve completed your second rebirth. Congratulations!", "Thanks!");
        }
        else if (player.Rebirths != 0 && player.Level >= 120 && player.GetProgress(61, 0) >= 2)
        {
            if (player.GetProgress(61, 0) == 2)
            {
                if (player.GetProgress(61, 21) >= 9)
                {
                    await dialog.Confirm("The BindingCurse has shed all of its blood, but its evil\nforce is not weakened at all. My fellows can`t be released\nuntil Satan is killed. I lost SquamaBead, the key to open\nSatan Seal. That seal protects Satan from being hurt. The\nSquamaBead has fallen into hands of evil spirits. Can you\nfind it for me?",
                        "I will try my best.", "Sorry, I am busy.");

                    if (player.PkPoints <= 1000)
                    {
                        player.SetProgress(61, 0, 3);
                        player.Inform("You`ve completed the third stage of your tests!");

                        dialog.Say("The SquamaBead is probably in the hands of SerpentSpirit, HadesLord, DemonLord, FiendLord, FuryApe, DemonBat, \nFurySkeleton, FearlessBeast. Please try to get it for me.\n",
                            "OK.");
                    }
                    else
                    {
                        player.Inform("Error: Failed to meet condition.");
                    }
                }
                else if (player.GetProgress(61, 21) == 0)
                {
                    if (player.GetProgress(61, 2) != 0)
                    {
                        dialog.Say("The Binding Curse is not broken yet. Please go to kill the evil spirits as soon as possible.",
                            "I`ve got to go.");
                        return;
                    }

                    await dialog.Confirm("I`ve heard about your merits. Satan is imprisoned in the\ndepth of this place. I am the warden. I have 8 guards:\nAndrew, Peter, Philip, Timothy, Daphne, Victoria, Wayne,\nTheodore. They are killed and cursed by Satan. Their souls\ncan`t be released from suffering. Please kindly help them.",
                        "What can I do?", "I have no time.");

                    dialog.Require(player.PkPoints <= 1000);

                    await dialog.Confirm("The 8 curses are closely linked to form a BindingCurse. You\nmust break them in this order: Andrew, Peter, Philip,\nTimothy, Daphne, Victoria, Wayne, Theodore.\nThe BindingCurse has 21600 drops of blood. You must break\n the 8 curses repeatedly until the BindingCurse sheds all of its blood.\n\n",
                        "I see.", "Too troublesome.");

                    if (player.PkPoints <= 1000)
                    {
                        dialog.Say("If the BindingCurse has shed all of its blood and the curse can`t be removed, there is only one reason. You may come\nto me by then. I will tell you what to do.",
                            "OK.");
                    }
                    else
                    {
                        player.Inform("Error: Failed to meet condition.");
                    }
                }
                else if (player.GetProgress(61, 21) == 1)
                {
                    dialog.Say("The BindingCurse has shed 2400 drops of its blood. It still has 19200 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 2)
                {
                    dialog.Say("The BindingCurse has shed 4800 drops of its blood. It still has 16800 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 3)
                {
                    dialog.Say("The BindingCurse has shed 7200 drops of its blood. It still has 14400 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 4)
                {
                    dialog.Say("The BindingCurse has shed 9600 drops of its blood. It still has 12000 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 5)
                {
                    dialog.Say("The BindingCurse has shed 12000 drops of its blood. It still has 9600 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 6)
                {
                    dialog.Say("The BindingCurse has shed 14400 drops of its blood. It still has 7200 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 7)
                {
                    dialog.Say("The BindingCurse has shed 16800 drops of its blood. It still has 4800 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
                else if (player.GetProgress(61, 21) == 8)
                {
                    dialog.Say("The BindingCurse has shed 19200 drops of its blood. It still has 2400 drops of blood. Please try to make it shed \nmore blood. My fellows will be very grateful to you.",
                        "I will try my best.");
                }
            }
            else if (player.GetProgress(61, 0) == 4)
            {
                dialog.Say("You made it. Thank you very much for saving us. You`ve cleansed this hopeless abyss. Now the evil force of Satan \nhas gone away, this place will disappear very soon. All souls here will get reborn. You`ve completed your tests. \nYou may leave here to take second rebirth now.\n",
                    "Glad to hear that.");
            }
            else if (player.GetProgress(61, 0) == 3)
            {
                if (player.Has(722727))
                {
                    dialog.Say("It`s the right SquamaBead I`ve lost. You`ve got it! With this key of SatanSeal, you can go to kill Satan now. Satan\nwill take different forms after death. Be careful. You`ve passed so many tests. I believe you can destroy Satan.\n\n",
                        "I`ve got to kill Satan.");
                    return;
                }

                await dialog.Confirm("The BindingCurse has shed all of its blood, but its evil\nforce is not weakened at all. My fellows can`t be released\nuntil Satan is killed. I lost SquamaBead, the key to open\nSatan Seal. That seal protects Santa from being hurt. The\nSquamaBead has fallen into hands of evil spirits. Can you find it for me?\n",
                    "I will try my best.", "Sorry, I am busy.");

                if (player.PkPoints <= 1000)
                {
                    dialog.Say("The SquamaBead is probably in the hands of SerpentSpirit, HadesLord, DemonLord, FiendLord, FuryApe, DemonBat, \nFurySkeleton, FearlessBeast. Please try to get it for me.\n",
                        "OK.");
                }
                else
                {
                    player.Inform("Error: Failed to meet condition.");
                }
            }
        }
        else
        {
            player.Inform("Sorry, you are not a level 120+ reborn person or haven`t completed the second stage of your tests.");
        }
    }

    [Npc(3604)]
    public static async Task Bruce(Player player, Dialog dialog)
    {
        dialog.Avatar = 71;

        if (player.PkPoints > 1000)
        {
            return;
        }

        await dialog.Confirm("Another wronged person! What a poor guy! You should have been in the Heaven, unfortunately, you`ve been driven to \nthis terrible place where nobody will take care.\n",
            "Where am I now?");

        dialog.Require(player.PkPoints <= 1000);

        await dialog.Confirm("Here is not the fairyland or hell. This place is called Evil Abyss. If you want to get reborn and start a fresh \nlife, you need to go through a lot of tests.\n\n\n",
            "What tests?");

        if (player.PkPoints <= 1000)
        {
            dialog.Say("The journey to second rebirth is very long and dangerous. There are many vicious demons and fiends roaming around. \nI advise you think it over. If you have made up your mind, you may have a talk with my fellows. They will tell you what to do.\n\n\n",
                "I see.");
        }
        else
        {
            player.Inform("Error: Failed to meet condition.");
        }
    }

    [Npc(3605)]
    public static async Task EarthSeal(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            player.Inform("Sorry, you`re ineligible to use the EarthSeal.");
        }
        else if (player.Has(722723) && player.Has(722724) && player.Has(722725))
        {
            await dialog.Confirm("Are you sure you want to open the seal? If yes, you will summon a HillSpirit.", "Yes.", "No.");

            dialog.Require(player.PkPoints <= 1000);

            if (player.World.CountMonsters("HillSpirit", 1700) == 1)
            {
                player.Inform("This seal is already open.");
            }
            else if (player.Has(722723) && player.Has(722724) && player.Has(722725))
            {
                player.Take(722723);
                player.Take(722724);
                player.Take(722725);
                player.World.Spawn(3632, 1700, 447, 875);
                player.Inform("The EarthSeal is open. A HillSpirit has jumped out of the seal.");
                player.SetProgress(61, 14, 1);
            }
            else
            {
                player.Inform("You can`t open EarthSeal. Please bring a Moss, DreamGrass and SoulAroma to open it.");
            }
        }
        else
        {
            player.Inform("You can`t open EarthSeal. Please bring a Moss, DreamGrass and SoulAroma to open it.");
        }
    }

    [Npc(3606)]
    public static async Task WaterSeal(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            player.Inform("Sorry, you`re ineligible to use the WaterSeal.");
        }
        else if (player.Has(722723) && player.Has(722724) && player.Has(722725) && player.Has(722721))
        {
            await dialog.Confirm("The WaterSeal has a tooth-like groove. You can insert an EvilTooth into the groove to open this seal.",
                "Insert an EvilTooth.", "I`ve got to go.");

            dialog.Require(player.PkPoints <= 1000);

            if (player.World.CountMonsters("SwiftDevil", 1700) == 1)
            {
                player.Inform("This seal is already open.");
            }
            else if (player.Has(722723) && player.Has(722724) && player.Has(722725) && player.Has(722721))
            {
                player.Take(722723);
                player.Take(722724);
                player.Take(722725);
                player.Take(722721);
                player.World.Spawn(3633, 1700, 886, 443);
                player.Inform("A SwiftDevil has come out of the WaterSeal.");
                player.SetProgress(61, 15, 1);
            }
            else
            {
                player.Inform("You can`t open the WaterSeal. Please bring an EvilTooth, Moss, DreamGrass and SoulAroma to open it.");
            }
        }
        else
        {
            player.Inform("You can`t open the WaterSeal. Please bring an EvilTooth, Moss, DreamGrass and SoulAroma to open it.");
        }
    }

    [Npc(3607)]
    public static async Task FireSeal(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            player.Inform("Sorry, you`re ineligible to use the FireSeal.");
        }
        else if (player.Has(722723) && player.Has(722724) && player.Has(722725) && player.Has(722728))
        {
            await dialog.Confirm("If you want me to release a Banshee, you must bring an ImmortalStone, Moss, DreamGrass and SoulAroma. Have you got all ready?\n",
                "Of course.", "Not yet.");

            dialog.Require(player.PkPoints <= 1000);

            if (player.World.CountMonsters("Banshee", 1700) == 1)
            {
                player.Inform("This seal is already open.");
            }
            else if (player.Has(722723) && player.Has(722724) && player.Has(722725) && player.Has(722728))
            {
                player.Take(722723);
                player.Take(722724);
                player.Take(722725);
                player.Take(722728);
                player.World.Spawn(3634, 1700, 925, 934);
                player.Inform("A Banshee has come out of the FireSeal.");
                player.SetProgress(61, 16, 1);
            }
            else
            {
                player.Inform("You can`t open FireSeal. Please bring an ImmortalStone, Moss, DreamGrass and SoulAroma to open it.");
            }
        }
        else
        {
            player.Inform("You can`t open FireSeal. Please bring an ImmortalStone, Moss, DreamGrass and SoulAroma to open it.");
        }
    }

    [Npc(3608)]
    public static async Task SatanSeal(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) != 3)
        {
            player.Inform("Sorry, you`re ineligible to use the SatanSeal.");
            return;
        }

        if (!player.Has(722727))
        {
            player.Inform("You can`t open the SatanSeal. Please bring a SquamaBead to open it.");
            return;
        }

        await dialog.Confirm("Satan is imprisoned here. Only Squama Bead can open the seal to summon a Satan. Do you want to open the seal now?",
            "Yeah.", "I`ve got to go.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.World.CountMonsters("Satan", 1700) == 1 || player.World.CountMonsters("BeastSatan", 1700) == 1 ||
            player.World.CountMonsters("FurySatan", 1700) == 1)
        {
            player.Inform("The SatanSeal is already open.");
        }
        else if (player.Take(722727))
        {
            player.World.Spawn(3644, 1700, 337, 341);
            player.World.Announce(1700, $"{player.Name} has opened the SatanSeal! A Satan has come out of the seal.",
                TalkChannel.System);
        }
    }

    [Npc(3609)]
    public static async Task CleansingStove(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            player.Inform("Sorry, you`re ineligible to use the CleansingStove.");
        }
        else if (player.Has(722730))
        {
            await dialog.Confirm("There is floating light in the stove. You can`t see it clearly. What are you coming for?",
                "Cleanse my ImpureVigor.", "Just passing by.");

            dialog.Require(player.PkPoints <= 1000);

            if (player.World.CountMonsters("CleansingDevil", 1700) == 1)
            {
                player.Inform("This stove is already open.");
            }
            else if (player.Take(722730))
            {
                player.World.Spawn(3635, 1700, 717, 738);
                player.Inform("A CleansingDevil has come out of the stove.");
                player.SetProgress(61, 17, 1);
            }
        }
        else
        {
            player.Inform("You can`t open the CleansingStove. Please bring an ImpureVigor to open it.");
        }
    }

    [Npc(3615, 3616)]
    public static void AndrewCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Andrew`s soul is crying with pain.");
        }
    }

    [Npc(3617, 3618)]
    public static void PeterCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Peter`s soul is crying with pain.");
        }
    }

    [Npc(3619, 3620, 3621)]
    public static void PhilipCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Philip`s soul is crying with pain.");
        }
    }

    [Npc(3622, 3626, 3627)]
    public static void TimothyCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Timothy`s soul is crying with pain.");
        }
    }

    [Npc(3628, 3629)]
    public static void DaphneCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Daphne`s soul is crying with pain.");
        }
    }

    [Npc(3630, 3631, 3632)]
    public static void VictoriaCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Victoria`s soul is crying with pain.");
        }
    }

    [Npc(3633, 3634)]
    public static void WayneCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Wayne`s soul is crying with pain.");
        }
    }

    [Npc(3635, 3636, 3637)]
    public static void TheodoreCurse(Player player, Dialog dialog)
    {
        if (player.PkPoints <= 1000)
        {
            player.Inform("You feel there is evil atmosphere around you. You hear Theodore`s soul is crying with pain.");
        }
    }
}
