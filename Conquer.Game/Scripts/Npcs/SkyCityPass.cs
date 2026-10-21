namespace Conquer.Game.Scripts.Npcs;

public static class SkyCityPass
{
    [Npc(30001)]
    public static async Task GlumMonsterLevelUpTo90(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level >= 91)
        {
            dialog.Say("This floor is for those who are under level 90.", "I see.");
            return;
        }

        await dialog.Confirm("This is 1st pass. I may teleport you to the next pass directly, or you will be teleported to the battle stage. Are you ready?",
            "Yeah.", "Let me think it over.");

        if (player.Roll(7) >= 1)
        {
            player.Teleport(1040, 469, 488);
            return;
        }

        if (player.Teleport(1040, 544, 331))
        {
            dialog.Say("Congratulations! You have passed the first test. I am pleased to teleport you to the next pass.",
                "Thanks.");
        }
    }

    [Npc(30002)]
    public static async Task GlumMonsterLevel80To100(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 80 || player.Level >= 101)
        {
            dialog.Say("Sorry, you cannot challenge this floor if you are not between level 80 and 100.", "I see.");
            return;
        }

        await dialog.Confirm("This is 1st pass. I may teleport you to the next pass directly, or you will be teleported to the battle stage. Are you ready?",
            "Yeah.", "Let me think it over.");

        if (player.Roll(6) >= 1)
        {
            player.Teleport(1040, 445, 512);
            return;
        }

        if (player.Teleport(1040, 544, 331))
        {
            dialog.Say("Congratulations! You have passed the first test. I am pleased to teleport you to the next pass.",
                "Thanks.");
        }
    }

    [Npc(30003)]
    public static async Task GlumMonsterLevel90To110(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 90 || player.Level >= 111)
        {
            dialog.Say("If you want to challenge this pass, you must be between level 90 and 110.", "I see.");
            return;
        }

        await dialog.Confirm("This is the first floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yeah.", "Let me think it over.");

        if (player.Roll(100) >= 20)
        {
            player.Teleport(1040, 421, 536);
            return;
        }

        if (player.Teleport(1040, 544, 331))
        {
            dialog.Say("Congratulations! You have passed the first test. I am pleased to teleport you to the next pass.",
                "Thanks.");
        }
    }

    [Npc(30004)]
    public static async Task GlumMonsterLevel100Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 100)
        {
            dialog.Say("If you want to challenge this pass, you must be above level 100.", "I see.");
            return;
        }

        await dialog.Confirm("This is the first floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
            "Yeah.", "Let me think it over.");

        if (player.Roll(100) >= 25)
        {
            player.Teleport(1040, 397, 560);
            return;
        }

        if (player.Teleport(1040, 544, 331))
        {
            dialog.Say("Congratulations! You have passed the first test. I am pleased to teleport you to the next pass.",
                "Thanks.");
        }
    }

    [Npc(30005)]
    public static async Task GlumMonsterLevel110Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 110)
        {
            dialog.Say("If you want to challenge this pass, you must be above level 110.", "I see.");
            return;
        }

        await dialog.Confirm("This is the first floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
            "Yeah.", "Let me think it over.");

        if (player.Roll(3) >= 1)
        {
            player.Teleport(1040, 373, 584);
            return;
        }

        if (player.Teleport(1040, 544, 331))
        {
            dialog.Say("Congratulations! You have passed the first test. I am pleased to teleport you to the next pass.",
                "Thanks.");
        }
    }

    [Npc(30006, 30007, 30008, 30009, 30010)]
    public static async Task StageGuardFloor1(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721100))
        {
            dialog.Say("Only you fight in the stage and get a Pass Token, can I send you back to challenge.", "I see.");
            return;
        }

        await dialog.Confirm("Since you get the token, I will teleport you to the starting point to continue the challenge. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        dialog.Require(player.Has("PassToken1"));

        if (player.Take(721100))
        {
            player.Teleport(1040, 595, 383);
        }
    }

    [Npc(30011)]
    public static async Task GreenSnakeLevelUpTo90(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level >= 91)
        {
            dialog.Say("This floor is for those who are under level 90.", "I see.");
            return;
        }

        await dialog.Confirm("This is the second floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        if (player.Roll(7) >= 1)
        {
            player.Teleport(1040, 421, 440);
            return;
        }

        if (player.Teleport(1040, 492, 281))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30012)]
    public static async Task GreenSnakeLevel80To100(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 80 || player.Level >= 101)
        {
            dialog.Say("If you want to challenge this floor, you must be between level 80 and 100.", "I see.");
            return;
        }

        await dialog.Confirm("This is the second floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        if (player.Roll(6) >= 1)
        {
            player.Teleport(1040, 397, 464);
            return;
        }

        if (player.Teleport(1040, 492, 281))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30013)]
    public static async Task GreenSnakeLevel90To110(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 90 || player.Level >= 111)
        {
            dialog.Say("This floor is for those who are between level 90 and 110.", "I see.");
            return;
        }

        await dialog.Confirm("This is the second floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        if (player.Roll(5) >= 1)
        {
            player.Teleport(1040, 373, 488);
            return;
        }

        if (player.Teleport(1040, 492, 281))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30014)]
    public static async Task GreenSnakeLevel100Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 100)
        {
            dialog.Say("Please come after your level is above 100.", "I see.");
            return;
        }

        await dialog.Confirm("This is the second floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
            "Yes, I am.", "Wait a moment.");

        if (player.Roll(4) >= 1)
        {
            player.Teleport(1040, 349, 512);
            return;
        }

        if (player.Teleport(1040, 492, 281))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30015)]
    public static async Task GreenSnakeLevel110Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 110)
        {
            dialog.Say("This floor is for those who are above level 110.", "I see.");
            return;
        }

        await dialog.Confirm("This is the second floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
            "Yes, I am.", "Wait a moment.");

        if (player.Roll(3) >= 1)
        {
            player.Teleport(1040, 325, 536);
            return;
        }

        if (player.Teleport(1040, 492, 281))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30016, 30017, 30018, 30019, 30020)]
    public static async Task StageGuardFloor2(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721101))
        {
            dialog.Say("Only you fight in the stage and get a Pass Token, can I send you back to challenge.", "I see.");
            return;
        }

        await dialog.Confirm("With this pass token, you may challenge the sky pass again. Would you like me to teleport you back?",
            "Yes, please.", "Wait a moment.");

        dialog.Require(player.Has("PassToken2"));

        if (player.Take(721101))
        {
            player.Teleport(1040, 544, 331);
        }
    }

    [Npc(30021)]
    public static async Task EvilHawkLevel100Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has(721154))
        {
            await dialog.Confirm("Oh, you were sent by Tientsin. Congratulations! This is the third floor. I will give you the PassLetter.",
                "Thanks.");

            if (!player.Take(721154) || player.Give(721155) is null)
            {
                return;
            }

            if (await dialog.Ask("You deserve it. Will you stay to challenge the Sky Pass or will you leave?",
                "I will continue the SkyPass.", "I will leave here.") == 1)
            {
                player.Teleport(1012, 170, 158);
            }
        }
        else if (player.Level < 100)
        {
            dialog.Say("This floor is for those who are above level 100.", "I see.");
        }
        else
        {
            await dialog.Confirm("This is the third floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
                "Yes, I am.", "Wait a moment.");

            if (player.Roll(100) >= 25)
            {
                player.Teleport(1040, 301, 464);
                return;
            }

            if (player.Teleport(1040, 436, 224))
            {
                dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
            }
        }
    }

    [Npc(30022)]
    public static async Task EvilHawkLevel110Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has(721154))
        {
            await dialog.Confirm("Oh, you were sent by Tientsin. Congratulations! This is the third floor. I will give you the PassLetter.",
                "Thanks.");

            if (!player.Take(721154) || player.Give(721155) is null)
            {
                return;
            }

            if (await dialog.Ask("You deserve it. Will you stay to challenge the Sky Pass or will you leave?",
                "I will continue the SkyPass.", "I will leave here.") == 1)
            {
                player.Teleport(1012, 170, 158);
            }
        }
        else if (player.Level < 110)
        {
            dialog.Say("Please come after your level is above 110.", "I see.");
        }
        else
        {
            await dialog.Confirm("This is the third floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
                "Yes, I am.", "Wait a moment.");

            if (player.Roll(3) >= 1)
            {
                player.Teleport(1040, 277, 488);
                return;
            }

            if (player.Teleport(1040, 436, 224))
            {
                dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
            }
        }
    }

    [Npc(30023)]
    public static async Task EvilHawkLevel90To110(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has(721154))
        {
            await dialog.Confirm("Oh, you were sent by Tientsin. Congratulations! This is the third floor. I will give you the PassLetter.",
                "Thanks.");

            if (!player.Take(721154) || player.Give(721155) is null)
            {
                return;
            }

            if (await dialog.Ask("You deserve it. Will you stay to challenge the Sky Pass or will you leave?",
                "I will continue the SkyPass.", "I will leave here.") == 1)
            {
                player.Teleport(1012, 170, 158);
            }
        }
        else if (player.Level >= 90 && player.Level < 111)
        {
            await dialog.Confirm("This is the third floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
                "Yes. I am.", "Wait a moment.");

            if (player.Roll(100) >= 20)
            {
                player.Teleport(1040, 325, 440);
                return;
            }

            if (player.Teleport(1040, 436, 224))
            {
                dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
            }
        }
        else
        {
            dialog.Say("This floor is for those who are between level 90 and 110.", "I see.");
        }
    }

    [Npc(30024)]
    public static async Task EvilHawkLevel80To100(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has(721154))
        {
            await dialog.Confirm("Oh, you were sent by Tientsin. Congratulations! This is the third floor. I will give you the PassLetter.",
                "Thanks.");

            if (!player.Take(721154) || player.Give(721155) is null)
            {
                return;
            }

            if (await dialog.Ask("You deserve it. Will you stay to challenge the Sky Pass or will you leave?",
                "I will continue the SkyPass.", "I will leave here.") == 1)
            {
                player.Teleport(1012, 170, 158);
            }
        }
        else if (player.Level < 80 || player.Level < 101)
        {
            if (await dialog.Ask("This is the third floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
                "Yes. I am.", "Wait a moment.") == 0)
            {
                await YesIAm();
            }
        }
        else
        {
            dialog.Say("This floor is for those who are between level 80 and 100.", "I see.");
        }

        async Task YesIAm()
        {
            if (player.Roll(6) >= 1)
            {
                player.Teleport(1040, 349, 416);
                return;
            }

            if (player.Teleport(1040, 436, 224))
            {
                dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
            }
        }
    }

    [Npc(30025)]
    public static async Task EvilHawkLevelUpTo90(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has(721154))
        {
            await dialog.Confirm("Oh, you were sent by Tientsin. Congratulations! This is the third floor. I will give you the PassLetter.",
                "Thanks.");

            if (!player.Take(721154) || player.Give(721155) is null)
            {
                return;
            }

            if (await dialog.Ask("You deserve it. Will you stay to challenge the Sky Pass or will you leave?",
                "I will continue the SkyPass.", "I will leave here.") == 1)
            {
                player.Teleport(1012, 170, 158);
            }
        }
        else if (player.Level < 91)
        {
            await dialog.Confirm("This is the third floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
                "Yes. I am.", "Wait a moment.");

            if (player.Roll(7) >= 1)
            {
                player.Teleport(1040, 373, 392);
                return;
            }

            if (player.Teleport(1040, 436, 224))
            {
                dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
            }
        }
        else
        {
            dialog.Say("This floor is for those who are under level 90.", "I see.");
        }
    }

    [Npc(30026, 30027, 30028, 30029, 30030)]
    public static async Task StageGuardFloor3(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721102))
        {
            dialog.Say("Only you fight in the stage and get a Pass Token, can I send you back to challenge.", "I see.");
            return;
        }

        await dialog.Confirm("With this pass token, you may challenge the sky pass again. Would you like me to teleport you back?",
            "Yes, please.", "Wait a moment.");

        dialog.Require(player.Has("PassToken3"));

        if (player.Take(721102))
        {
            player.Teleport(1040, 492, 281);
        }
    }

    [Npc(30031)]
    public static async Task MonsterGeneralLevel110Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 110)
        {
            dialog.Say("This floor is for those who are above level 110.", "I see.");
            return;
        }

        await dialog.Confirm("This is the fourth floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
            "Yes, I am.", "Wait a moment.");

        if (player.Roll(3) >= 1)
        {
            player.Teleport(1040, 229, 440);
            return;
        }

        if (player.Teleport(1040, 394, 181))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30032)]
    public static async Task MonsterGeneralLevel80To100(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 80 || player.Level >= 101)
        {
            dialog.Say("If you want to challenge this floor, you must be between level 80 and 100.", "I see.");
            return;
        }

        await dialog.Confirm("This is the fourth floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        if (player.Roll(6) >= 1)
        {
            player.Teleport(1040, 300, 367);
            return;
        }

        if (player.Teleport(1040, 394, 181))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30033)]
    public static async Task MonsterGeneralLevelUpTo90(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level >= 91)
        {
            dialog.Say("This floor is for those who are under level 90.", "I see.");
            return;
        }

        await dialog.Confirm("This is the fourth floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        if (player.Roll(7) >= 1)
        {
            player.Teleport(1040, 324, 343);
            return;
        }

        if (player.Teleport(1040, 394, 181))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30034)]
    public static async Task MonsterGeneralLevel90To110(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 90 || player.Level >= 111)
        {
            dialog.Say("This floor is for those who are between level 90 and 110.", "I see.");
            return;
        }

        await dialog.Confirm("This is the fourth floor. I may teleport you to the next floor directly, or to the battle stage. Are you ready?",
            "Yes. I am.", "Wait a moment.");

        if (player.Roll(100) >= 20)
        {
            player.Teleport(1040, 276, 391);
            return;
        }

        if (player.Teleport(1040, 394, 181))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30035)]
    public static async Task MonsterGeneralLevel100Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 100)
        {
            dialog.Say("This floor is for those who are above level 100.", "I see.");
            return;
        }

        await dialog.Confirm("This is the fourth floor. I may teleport you to the next pass directly, or to the battle stage. Are you ready?",
            "Yes, I am.", "Wait a moment.");

        if (player.Roll(100) >= 25)
        {
            player.Teleport(1040, 252, 415);
            return;
        }

        if (player.Teleport(1040, 394, 181))
        {
            dialog.Say("Congratulations! You passed! I will send you to the next floor.", "Thanks.");
        }
    }

    [Npc(30036, 30037, 30038, 30039, 30040)]
    public static async Task StageGuardFloor4(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721103))
        {
            dialog.Say("Only you fight in the stage and get a Pass Token, can I send you back to challenge.", "I see.");
            return;
        }

        await dialog.Confirm("With this pass token, you may challenge the sky pass again. Would you like me to teleport you back?",
            "Yes, please.", "Wait a moment.");

        dialog.Require(player.Has("PassToken4"));

        if (player.Take(721103))
        {
            player.Teleport(1040, 436, 224);
        }
    }

    [Npc(30043)]
    public static async Task TerminalGuardLevel110Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 110)
        {
            dialog.Say("This floor is for those who are above level 110.", "I see.");
        }
        else
        {
            await dialog.Confirm("Welcome! This is the last floor. You are lucky to make it so far. It is tougher ahead. Do you want a try?",
                "Sure!");

            if (await dialog.Ask("But you have to fight in the stage for a Pass Token. The guard may send you to claim the prize or send you back to me.",
                "Ok, let`s get started.", "Wait a moment.") == 0)
            {
                player.Teleport(1040, 180, 391);
            }
        }
    }

    [Npc(30044)]
    public static async Task TerminalGuardLevel100Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 100)
        {
            dialog.Say("This floor is for those who are above level 100.", "I see.");
        }
        else
        {
            await dialog.Confirm("Welcome! This is the last floor. You are lucky to make it so far. It is tougher ahead. Do you want a try?",
                "Sure!");

            if (await dialog.Ask("But you have to fight in the stage for a Pass Token. The guard may send you to claim the prize or send you back to me.",
                "Ok, let`s get started.", "Wait a moment.") == 0)
            {
                player.Teleport(1040, 204, 367);
            }
        }
    }

    [Npc(30045)]
    public static async Task TerminalGuardLevel90To110(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 90 || player.Level >= 111)
        {
            dialog.Say("This floor is for those who are between level 90 and 110.", "I see.");
            return;
        }

        await dialog.Confirm("You have a good luck. This is the last pass. You must prove your ability before you get the big prize. Are you ready?",
            "Sure.");

        if (await dialog.Ask("But you have to fight in the stage for a Pass Token. The guard may send you to claim the prize or send you back to me.",
            "Go ahead.", "Wait a moment.") == 0)
        {
            player.Teleport(1040, 228, 343);
        }
    }

    [Npc(30046)]
    public static async Task FinalStageGuardLevelUpTo90(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721104))
        {
            dialog.Say("Keep up with the good job! I may send you to claim the prizes if you obtain the Pass Token.",
                "I will try my best.");
            return;
        }

        if (player.Roll(6) < 1)
        {
            await dialog.Confirm("Congrats! You have passed the sky pass. I am pleased to teleport you to claim the prize.", "Thanks.");

            dialog.Require(player.Has("PassTokenL80"));

            if (player.Take(721104) && player.Give(721109) is not null)
            {
                player.Teleport(1040, 192, 250);
            }
        }
        else if (player.Take(721104) && player.Teleport(1040, 394, 181))
        {
            dialog.Say("I am so sorry that you will be sent back to challenge again.", "What a bad luck.");
        }
    }

    [Npc(30047)]
    public static async Task FinalStageGuardLevel80To100(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721105))
        {
            dialog.Say("Keep up with the good job! I may send you to claim the prizes if you obtain the Pass Token.",
                "I will try my best.");
            return;
        }

        if (player.Roll(5) < 1)
        {
            await dialog.Confirm("Congrats! You have passed the sky pass. I am pleased to teleport you to claim the prize.", "Thanks.");

            dialog.Require(player.Has("PassTokenL90"));

            if (player.Take(721105) && player.Give(721109) is not null)
            {
                player.Teleport(1040, 192, 250);
            }
        }
        else if (player.Take(721105) && player.Teleport(1040, 394, 181))
        {
            dialog.Say("I am so sorry that you will be sent back to challenge again.", "What a bad luck.");
        }
    }

    [Npc(30048)]
    public static async Task FinalStageGuardLevel90To110(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721106))
        {
            dialog.Say("Keep up with the good job! I may send you to claim the prizes if you obtain the Pass Token.",
                "I will try my best.");
            return;
        }

        if (player.Roll(4) < 1)
        {
            await dialog.Confirm("Congrats! You have passed the sky pass. I am pleased to teleport you to claim the prize.", "Thanks.");

            dialog.Require(player.Has("PassTokenL100"));

            if (player.Take(721106) && player.Give(721109) is not null)
            {
                player.Teleport(1040, 192, 250);
            }
        }
        else if (player.Take(721106) && player.Teleport(1040, 394, 181))
        {
            dialog.Say("I am so sorry that you will be sent back to challenge again.", "What a bad luck.");
        }
    }

    [Npc(30049)]
    public static async Task FinalStageGuardLevel100Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721107))
        {
            dialog.Say("Keep up with the good job! I may send you to claim the prizes if you obtain the Pass Token.",
                "I will try my best.");
            return;
        }

        if (player.Roll(3) < 1)
        {
            await dialog.Confirm("Congrats! You have passed the sky pass. I am pleased to teleport you to claim the prize.", "Thanks.");

            dialog.Require(player.Has("PassTokenL110"));

            if (player.Take(721107) && player.Give(721109) is not null)
            {
                player.Teleport(1040, 192, 250);
            }
        }
        else if (player.Take(721107) && player.Teleport(1040, 394, 181))
        {
            dialog.Say("I am so sorry that you will be sent back to challenge again.", "What a bad luck.");
        }
    }

    [Npc(30050)]
    public static async Task FinalStageGuardLevel110Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has(721108))
        {
            dialog.Say("Keep up with the good job! I may send you to claim the prizes if you obtain the Pass Token.",
                "I will try my best.");
            return;
        }

        if (player.Roll(2) < 1)
        {
            await dialog.Confirm("Congratulations! You passed the Sky Pass successfully. Now I will teleport you to claim the prize.", "Thanks.");

            dialog.Require(player.Has("PassTokenL120"));

            if (player.Take(721108) && player.Give(721109) is not null)
            {
                player.Teleport(1040, 192, 250);
            }
        }
        else if (player.Take(721108) && player.Teleport(1040, 394, 181))
        {
            dialog.Say("I am so sorry that you will be sent back to challenge again.", "What a bad luck.");
        }
    }

    [Npc(30051)]
    public static async Task TerminalGuardLevel80To100(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level < 80 || player.Level >= 101)
        {
            dialog.Say("If you want to challenge this floor, you must be between level 80 and 100.", "I see.");
            return;
        }

        await dialog.Confirm("You have a good luck. This is the last pass. You must prove your ability before you get the big prize. Are you ready?",
            "Sure.");

        if (await dialog.Ask("But you have to fight in the stage for a Pass Token. The guard may send you to claim the prize or send you back to me.",
            "Go ahead.", "Wait a moment.") == 0)
        {
            player.Teleport(1040, 252, 319);
        }
    }

    [Npc(30052)]
    public static async Task TerminalGuardLevelUpTo90(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Level >= 91)
        {
            dialog.Say("This floor is for those who are under level 90.", "I see.");
            return;
        }

        await dialog.Confirm("You have a good luck. This is the last pass. You must prove your ability before you get the big prize. Are you ready?",
            "Sure.");

        if (await dialog.Ask("But you have to fight in the stage for a Pass Token. The guard may send you to claim the prize or send you back to me.",
            "Go ahead.", "Wait a moment.") == 0)
        {
            player.Teleport(1040, 277, 296);
        }
    }

    [Npc(30054)]
    public static async Task GodCloud(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (!player.Has("SkyPrizeToken"))
        {
            return;
        }

        await dialog.Confirm("Well done! You have passed the 5 tough floors in one time. You have showed your great competence and perseverance.",
            "Thanks.");

        dialog.Require(player.Has("SkyPrizeToken"));

        switch (await dialog.Ask("There are two Treasure Boxes here, 4 Meteors in the first box, 10 Meteors or some money in the second one. Which do you like?",
            "Treasure Box 1", "Treasure Box 2"))
        {
            case 0:
                dialog.Require(player.Has("SkyPrizeToken"));

                if (!player.Take(721109) || player.Give(ItemType.Meteor) is null)
                {
                    return;
                }

                if (player.Give(ItemType.Meteor, 2))
                {
                    player.Give(ItemType.Meteor);
                }

                player.Teleport(1002, 430, 380);
                break;

            case 1:
                dialog.Require(player.Has("SkyPrizeToken"));

                if (player.Roll(100) < 20)
                {
                    if (!player.Take(721109))
                    {
                        return;
                    }

                    if (player.Give(ItemType.Meteor, 9))
                    {
                        player.Give(ItemType.Meteor);
                    }

                    player.Teleport(1002, 430, 380);
                    return;
                }

                switch (player.Roll(1, 8))
                {
                    case 1 or 2:
                        if (player.Take(721109) && player.Earn(1000))
                        {
                            player.Teleport(1002, 430, 380);
                        }
                        break;

                    case >= 3 and <= 6:
                        if (player.Take(721109) && player.Give(ItemType.Meteor) is not null)
                        {
                            if (player.Give(ItemType.Meteor, 2))
                            {
                                player.Give(ItemType.Meteor);
                            }

                            player.Teleport(1002, 430, 380);
                        }
                        break;

                    case 7 or 8:
                        if (player.Take(721109) && player.Earn(10000))
                        {
                            player.Teleport(1002, 430, 380);
                        }
                        break;
                }

                if (!player.Take(721109) || player.Give(ItemType.Meteor) is null)
                {
                    return;
                }

                if (player.Give(ItemType.Meteor, 2))
                {
                    player.Give(ItemType.Meteor);
                }

                player.Teleport(1002, 430, 380);
                break;
        }
    }
}
