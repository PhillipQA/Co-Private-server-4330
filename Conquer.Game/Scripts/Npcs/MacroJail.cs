namespace Conquer.Game.Scripts.Npcs;

public static class MacroJail
{
    [Npc(6002)]
    public static async Task Warden(Player player, Dialog dialog)
    {
        if (player.GetProgress(6, 2) != 1)
        {
            dialog.Avatar = 2;

            await dialog.Confirm("Do you know it is jail that you are in now?", "Have I done something wrong?");

            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("Certainly. If you use any kind of bot, edit the game in any way, or you scam in game, you will be sent here.",
                "I see. And how can I leave here?");

            dialog.Require(player.PkPoints <= 1000);

            await dialog.Confirm("You must pay two DragonBalls for your first time being caught as punishment, four DragonBalls for your second\ntime, and ten DragonBalls for your third time.\nIf you are caught botting more than three times, you will have no chance to get released.\n",
                "There is no alternative for me.");

            dialog.Require(player.PkPoints <= 1000);

            dialog.Later(1, async () =>
            {
                if (player.GetProgress(6, 1) == 0)
                {
                    dialog.Avatar = 2;

                    await dialog.Confirm("I will charge you two DragonBalls to set you free since it is your first time. I do hope this is also your last time.",
                        "OK.");

                    if (!player.Has(ItemType.DragonBall, 2))
                    {
                        dialog.Say("Sorry, you do not have two DragonBalls, so I cannot release you.", "OK, I see.");
                        return;
                    }

                    await dialog.Confirm("I will let you out this time. If you are caught again, you must pay 4 DragonBalls for your guilty. Don`t let me see you again!",
                        "I will NEVER bot again!");

                    if (player.Take(ItemType.DragonBall, 2))
                    {
                        player.AddProgress(6, 1, 1);
                        player.Teleport(1002, 429, 378, force: true);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have two DragonBalls, so I cannot release you.", "OK, I see.");
                    }
                }
                else if (player.GetProgress(6, 1) == 1)
                {
                    dialog.Avatar = 2;

                    await dialog.Confirm("It`s your second time being caught botting. You must pay four DragonBalls to get released.", "OK.");

                    if (!player.Has(ItemType.DragonBall, 4))
                    {
                        dialog.Say("Sorry, you do not have four DragonBalls, so I cannot release you.", "OK, I see.");
                        return;
                    }

                    await dialog.Confirm("I will let you out this time. Remember if you are caught botting next time, you must pay 10 DragonBalls for your guilty.",
                        "I will NEVER bot again!");

                    if (player.Take(ItemType.DragonBall, 4))
                    {
                        player.AddProgress(6, 1, 1);
                        player.Teleport(1002, 429, 378, force: true);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have four DragonBalls, so I cannot release you.", "OK, I see.");
                    }
                }
                else if (player.GetProgress(6, 1) == 2)
                {
                    dialog.Avatar = 2;

                    await dialog.Confirm("It is your third time being caught botting. I will not let you out unless you hand in 10 DragonBalls.",
                        "OK.");

                    if (!player.Has(ItemType.DragonBall, 10))
                    {
                        dialog.Say("Sorry, you do not have four DragonBalls, so I cannot release you.",
                            "Ah, it`s so regrettable.");
                        return;
                    }

                    await dialog.Confirm("It`s your last chance! If you are caught botting next time, nobody can set you free and you will have to stay in the\nBotjail forever. My friend, let`s play the game fairly and keep far away from bots!",
                        "I will NEVER bot again!");

                    if (player.Take(ItemType.DragonBall, 10))
                    {
                        player.AddProgress(6, 1, 1);
                        player.Teleport(1002, 429, 378, force: true);
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have four DragonBalls, so I cannot release you.",
                            "Ah, it`s so regrettable.");
                    }
                }
                else if (player.GetProgress(6, 1) >= 3)
                {
                    dialog.Avatar = 2;

                    dialog.Say("It is unforgivable you are caught botting more than three times. Since you cannot play the game fair, just stay here forever!\n",
                        "Ah, it`s so regrettable.");
                }
            });

            return;
        }

        dialog.Avatar = 57;

        await dialog.Confirm("Your were sent here to take a visit by the PrisonOffcer. Are you going to leave now?",
            "Yes, let me out please.", "Let me think it about.");

        await dialog.Confirm("OK, good luck. And see you later.", "Thank you. Byebye.");

        if (player.Teleport(1002, 434, 380, force: true))
        {
            player.SetProgress(6, 2, 0);
        }
    }
}
