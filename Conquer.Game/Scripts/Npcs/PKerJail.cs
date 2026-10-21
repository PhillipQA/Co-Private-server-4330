namespace Conquer.Game.Scripts.Npcs;

public static class PKerJail
{
    [Npc(42)]
    public static async Task Warden(Player player, Dialog dialog)
    {
        dialog.Avatar = 2;

        switch (await dialog.Ask("What can I do for you?", "Let me out, please.", "Lend me a hoe, ok?",
            "Just passing by."))
        {
            case 0:
                if (player.PkPoints < 100)
                {
                    if (await dialog.Ask("You do not look like an evildoer. You may leave now.", "Thanks.") == 0)
                    {
                        player.Teleport(1002, 518, 356, force: true);
                    }
                }
                else if (player.PkPoints < 500)
                {
                    await dialog.Confirm("Sorry, you cannot leave here. I shall not release you until you are in red name or give me two gold ores.",
                        "Here are gold ores.");

                    if (!player.HasAny(1072050, 1072059, 2))
                    {
                        dialog.Say("Sorry, you do not have the required gold ores. How dare you cheat me?",
                            "I dare not.");
                        return;
                    }

                    await dialog.Confirm("Since you have given me two gold ores, you may leave now.", "Thanks, sir.");

                    if (player.Take(1072050, 1072059, 2))
                    {
                        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(1000, 567, 764), (1002, 738, 784),
                            (1002, 414, 8), (1020, 455, 167), (1000, 274, 436), (1015, 674, 785), (1015, 166, 189),
                            (1011, 382, 752)]);
                        player.Teleport(mapId, x, y, force: true);
                    }
                }
                else if (player.PkPoints < 1000)
                {
                    await dialog.Confirm("You have killed too many people. I shall not let you out until your PK points are lower than 100 or give me 3 gold ores.",
                        "Here are 3 gold ores.");

                    if (!player.HasAny(1072050, 1072059, 3))
                    {
                        dialog.Say("Sorry, you do not have the required gold ores. How dare you cheat me?",
                            "I dare not.");
                        return;
                    }

                    await dialog.Confirm("Since you have given me 3 gold ores, you may leave now.", "Thanks, sir.");

                    if (player.Take(1072050, 1072059, 3))
                    {
                        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(1000, 567, 764), (1002, 738, 784),
                            (1002, 414, 8), (1020, 455, 167), (1000, 274, 436), (1015, 674, 785), (1015, 166, 189),
                            (1011, 382, 752)]);
                        player.Teleport(mapId, x, y, force: true);
                    }
                }
                else if (player.PkPoints < 3000)
                {
                    await dialog.Confirm("You have killed too many people. I shall not let you out until your PK points are lower than 100 or give me 4 gold ores.",
                        "Here are 4 gold ores.");

                    if (!player.HasAny(1072050, 1072059, 4))
                    {
                        dialog.Say("Sorry, you do not have the required gold ores. How dare you cheat me?",
                            "I dare not.");
                        return;
                    }

                    await dialog.Confirm("Since you have given me 4 gold ores, you may leave now.", "Thanks, sir.");

                    if (player.Take(1072050, 1072059, 4))
                    {
                        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(1000, 567, 764), (1002, 738, 784),
                            (1002, 414, 8), (1020, 455, 167), (1000, 274, 436), (1015, 674, 785), (1015, 166, 189),
                            (1011, 382, 752)]);
                        player.Teleport(mapId, x, y, force: true);
                    }
                }
                else if (player.PkPoints < 10000)
                {
                    await dialog.Confirm("You have killed too many people. I shall not let you out until your PK points are lower than 100 or give me 5 gold ores.",
                        "Here are 5 gold ores.");

                    if (!player.HasAny(1072050, 1072059, 5))
                    {
                        dialog.Say("Sorry, you do not have the required gold ores. How dare you cheat me?",
                            "I dare not.");
                        return;
                    }

                    await dialog.Confirm("Since you have given me 5 gold ores, you may leave now.", "Thanks, sir.");

                    if (player.Take(1072050, 1072059, 5))
                    {
                        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(1000, 567, 764), (1002, 738, 784),
                            (1002, 414, 8), (1020, 455, 167), (1000, 274, 436), (1015, 674, 785), (1015, 166, 189),
                            (1011, 382, 752)]);
                        player.Teleport(mapId, x, y, force: true);
                    }
                }
                else
                {
                    await dialog.Confirm("You have killed too many people. I shall not let you out until your PK points are lower than 100 or give me 6 gold ores.",
                        "Here are 6 gold ores.");

                    if (!player.HasAny(1072050, 1072059, 6))
                    {
                        dialog.Say("Sorry, you do not have the required gold ores. How dare you cheat me?",
                            "I dare not.");
                        return;
                    }

                    await dialog.Confirm("Since you have given me 6 gold ores, you may leave now.", "Thanks, sir.");

                    if (player.Take(1072050, 1072059, 6))
                    {
                        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(1000, 567, 764), (1002, 738, 784),
                            (1002, 414, 8), (1020, 455, 167), (1000, 274, 436), (1015, 674, 785), (1015, 166, 189),
                            (1011, 382, 752)]);
                        player.Teleport(mapId, x, y, force: true);
                    }
                }
                break;

            case 1:
                if (await dialog.Ask("Take this hoe. Work hard. If you can get gold ores and give two to me, I may let you out.",
                    "Thanks, sir.") == 0)
                {
                    player.Give(562001);
                }
                break;
        }
    }
}
