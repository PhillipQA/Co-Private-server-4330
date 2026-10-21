namespace Conquer.Game.Scripts.Npcs;

public static class GuildArea
{
    [Npc(815, 816, 817, 818)]
    public static async Task GuildConductor(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 3;

        switch (await dialog.Ask("Are you heading for the next teleporter? It is free for our members, and 1,000 silver for others.",
            "Please teleport me there.", "Buy Guild Teleporter.", "Just passing by."))
        {
            case 0:
                if (npc.OwnerId == (player.GuildId ?? 0))
                {
                    player.Teleport((uint)npc.Data0, (ushort)npc.Data1, (ushort)npc.Data2);
                }
                else if (player.Spend(1000))
                {
                    player.World.Guilds.GetValueOrDefault(npc.OwnerId)?.Deposit(1000);
                    player.Teleport((uint)npc.Data0, (ushort)npc.Data1, (ushort)npc.Data2);
                }
                else
                {
                    dialog.Say("Please take 1000 silvers to use guild teleporter.", "I see.");
                }
                break;

            case 1:
                var choice = await dialog.Ask("A Teleporter costs 100,000 silver. After you buy one and place it at a good location, you can be teleported there directly.",
                    "Buy Teleporter 1.", "Buy Teleporter 2.", "Buy Teleporter 3.", "Buy Teleporter 4.",
                    "I changed my mind.");

                if (choice > 3)
                {
                    return;
                }

                uint itemTypeId = choice switch { 0 => 720021, 1 => 720022, 2 => 720023, _ => 720024 };

                if (npc.OwnerId != (player.GuildId ?? 0) || player.ShownGuildRank != GuildRank.Leader)
                {
                    dialog.Say("Sorry, only the guild leader who won the guild war can buy a Guild Teleporter.",
                        "I see.");
                    return;
                }

                if ((player.Guild?.Fund ?? 0) < 100000)
                {
                    dialog.Say("Your guild fund is less than 100,000 silver.", "I see.");
                }
                else if (player.Give(itemTypeId) is not null && player.SpendGuildFund(100000))
                {
                    dialog.Say("You may go to find a good location to place your teleporter now.", "I see.");
                }
                break;
        }
    }

    [Npc(819)]
    public static async Task BeastGuard(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 110;

        var beastGuard = player.World.FindNpc(819)!;

        if (beastGuard.OwnerId == 0)
        {
            if (player.Map!.OwnerId != (player.GuildId ?? 0))
            {
                dialog.Say("I am the Beast Guard. Once your guild wins the guild war, I shall lead the Guild Beast to serve you.",
                    "Oh, I see.");
                return;
            }

            if (player.ShownGuildRank != GuildRank.Leader)
            {
                dialog.Say("Your guild occupies the guild map. If the leader spends 500,000 guild fund, I shall lead the Guild Beast to serve your guild.",
                    "I will inform my leader.");
                return;
            }

            if (await dialog.Ask("Your guild has occupied the guild map. If you spend 500,000 guild fund, I shall lead the Guild Beast to serve your guild.",
                "All are ready.") == 0)
            {
                await AllAreReady();
            }
        }
        else if (beastGuard.Data1 == beastGuard.Map!.OwnerId)
        {
            if (beastGuard.Data2 == 1)
            {
                if (player.Map!.OwnerId != (player.GuildId ?? 0))
                {
                    dialog.Say("I am the Beast Guard. Once your guild wins the guild war, I shall lead the Guild Beast to serve you.",
                        "Oh, I see.");
                    return;
                }

                if (player.ShownGuildRank != GuildRank.Leader)
                {
                    dialog.Say("Your guild occupies the guild map. If the leader spends 500,000 guild fund, I shall lead the Guild Beast to serve your guild.",
                        "I will inform my leader.");
                    return;
                }

                if (await dialog.Ask("Your guild has occupied the guild map. If you spend 500,000 guild fund, I shall lead the Guild Beast to serve your guild.",
                    "All are ready.") == 0)
                {
                    await AllAreReady();
                }
            }
            else if (player.World.Now.IsDaily(new(21, 0), new(21, 59)))
            {
                if (beastGuard.Data0 == 2)
                {
                    dialog.Say("This beast has been killed. You may try tomorrow.", "It is ok.");
                }
                else if (beastGuard.Data0 == 1)
                {
                    if (player.World.CountMonsters("GuildBeast", 1038) == 1)
                    {
                        dialog.Say("Once you kill the Guild Beast before 22:00, you can get a Dragon Ball. And it only drops one Dragon Ball a day.",
                            "I see.");
                        return;
                    }

                    beastGuard.Data0 = 0;

                    if (await dialog.Ask("I let a Guild Beast out at 21:00. Everybody can fight it. Once you kill it before 22:00, you can get a dragon ball.",
                        "What if nobody kills it?") == 0)
                    {
                        await AskWhatIfNobodyKillsIt();
                    }
                }
                else
                {
                    if (await dialog.Ask("I let a Guild Beast out at 21:00. Everybody can fight it. Once you kill it before 22:00, you can get a dragon ball.",
                        "What if nobody kills it?") == 0)
                    {
                        await AskWhatIfNobodyKillsIt();
                    }
                }
            }
            else if (player.World.Now.IsDaily(new(22, 0), new(23, 59)))
            {
                if (beastGuard.Data0 == 3)
                {
                    if (npc.OwnerId != (player.GuildId ?? 0))
                    {
                        dialog.Say("Only the Guild leader or Deputy Guild Leader who has won the guild war can get a Dragon Ball from me.",
                            "I see.");
                        return;
                    }

                    if (player.ShownGuildRank < GuildRank.DeputyLeader)
                    {
                        dialog.Say("Only the Guild leader or Deputy Guild Leader who has won the guild war can get a Dragon Ball from me.",
                            "I see.");
                    }
                    else
                    {
                        if (await dialog.Ask("Well done. You have protected the beast. Please take this dragon Ball.",
                            "Thanks.") == 0)
                        {
                            await Thanks();
                        }
                    }
                }
                else if (beastGuard.Data0 == 2)
                {
                    dialog.Say("This beast has been killed. You may try tomorrow.", "It is ok.");
                }
                else if (beastGuard.Data0 == 4)
                {
                    if (npc.OwnerId == (player.GuildId ?? 0))
                    {
                        dialog.Say("Sorry, you are late. Your guild member has claimed the dragon ball today.",
                            "What a bad luck.");
                    }
                    else
                    {
                        if (await dialog.Ask("I let a Guild Beast out at 21:00. Everybody can fight it. Once you kill it before 22:00, you can get a dragon ball.",
                            "What if nobody kills it?") == 0)
                        {
                            await AskWhatIfNobodyKillsIt();
                        }
                    }
                }
                else if (beastGuard.Data0 == 0)
                {
                    if (await dialog.Ask("I let a Guild Beast out at 21:00. Everybody can fight it. Once you kill it before 22:00, you can get a dragon ball.",
                        "What if nobody kills it?") == 0)
                    {
                        await AskWhatIfNobodyKillsIt();
                    }
                }
                else
                {
                    beastGuard.Data0 = 0;

                    if (await dialog.Ask("I let a Guild Beast out at 21:00. Everybody can fight it. Once you kill it before 22:00, you can get a dragon ball.",
                        "What if nobody kills it?") == 0)
                    {
                        await AskWhatIfNobodyKillsIt();
                    }
                }
            }
            else if (beastGuard.Data0 == 3)
            {
                if (npc.OwnerId != (player.GuildId ?? 0))
                {
                    dialog.Say("Only the Guild leader or Deputy Guild Leader who has won the guild war can get a Dragon Ball from me.",
                        "I see.");
                    return;
                }

                if (player.ShownGuildRank < GuildRank.DeputyLeader)
                {
                    dialog.Say("Only the Guild leader or Deputy Guild Leader who has won the guild war can get a Dragon Ball from me.",
                        "I see.");
                }
                else
                {
                    if (await dialog.Ask("Well done. You have protected the beast. Please take this dragon Ball.",
                        "Thanks.") == 0)
                    {
                        await Thanks();
                    }
                }
            }
            else
            {
                beastGuard.Data0 = 0;

                if (await dialog.Ask("I let a Guild Beast out at 21:00. Everybody can fight it. Once you kill it before 22:00, you can get a dragon ball.",
                    "What if nobody kills it?") == 0)
                {
                    await AskWhatIfNobodyKillsIt();
                }
            }
        }
        else if (player.Map!.OwnerId == (player.GuildId ?? 0))
        {
            if (player.ShownGuildRank != GuildRank.Leader)
            {
                dialog.Say("Your guild occupies the guild map. If the leader spends 500,000 guild fund, I shall lead the Guild Beast to serve your guild.",
                    "I will inform my leader.");
                return;
            }

            if (await dialog.Ask("Your guild has occupied the guild map. If you spend 500,000 guild fund, I shall lead the Guild Beast to serve your guild.",
                "All are ready.") == 0)
            {
                await AllAreReady();
            }
        }
        else
        {
            dialog.Say("I am the Beast Guard. Once your guild wins the guild war, I shall lead the Guild Beast to serve you.",
                "Oh, I see.");
        }

        async Task AllAreReady()
        {
            if (player.Map!.OwnerId != (player.GuildId ?? 0))
            {
                player.Inform("The Guild Map is taken over by the other guild. Failed to purchase the Guild Beast.");
                return;
            }

            if ((player.Guild?.Fund ?? 0) >= 500000 && player.SpendGuildFund(500000))
            {
                beastGuard.OwnerId = (player.GuildId ?? 0);
                beastGuard.Data1 = (int)beastGuard.Map!.OwnerId;
                beastGuard.Data2 = 0;

                dialog.Say("I shall lead the Guild Beast to serve you before other guild occupies the guild map.",
                    "It is great.");
            }
            else
            {
                dialog.Say("Your guild fund is less than 500,000 silver.", "I shall come later.");
            }
        }

        async Task AskWhatIfNobodyKillsIt()
        {
            dialog.Say("In that case, the guild leader or deputy leader who has won the guild war can get a Dragon Ball. First come, First served.",
                "I see.");
        }

        async Task Thanks()
        {
            if (beastGuard.Data0 != 3)
            {
                dialog.Say("Sorry, you are late. Your guild member has claimed the dragon ball today.",
                    "What a bad luck.");
                return;
            }

            if (beastGuard.Data2 == 1)
            {
                dialog.Say("Sorry, the owner of the guild map has been changed. Please repurchase a Beast Guard.",
                    "Alright.");
            }
            else if (npc.OwnerId == (player.GuildId ?? 0))
            {
                if (player.ShownGuildRank < GuildRank.DeputyLeader)
                {
                    dialog.Say("Only the Guild leader or Deputy Guild Leader who has won the guild war can get a Dragon Ball from me.",
                        "I see.");
                }
                else if (player.FreeSlots > 0)
                {
                    beastGuard.Data0 = 4;
                    player.Give(ItemType.DragonBall);
                }
                else
                {
                    dialog.Say("Please prepare a vacuum in your inventory.", "Alright.");
                }
            }
            else
            {
                dialog.Say("Only the Guild leader or Deputy Guild Leader who has won the guild war can get a Dragon Ball from me.",
                    "I see.");
            }
        }
    }

    [Npc(7000)]
    public static async Task GuildKeeper(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (await dialog.Ask("Are you going to leave here?", "Yes.", "No. Wait a moment.") == 0)
        {
            player.Teleport(1002, 430, 380);
        }
    }

    [Npc(208854)]
    public static async Task Pole(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 3;

        await dialog.Confirm("What can I do for you?", "Heal this Stone Pillar.", "Just passing by.");

        if (npc.OwnerId != (player.GuildId ?? 0))
        {
            dialog.Say("Sorry. You can`t repair the pole. Only the leader of the guild taking the Guild House can use guild fund to repair it.",
                "Oh, I see.");
            return;
        }

        if (player.ShownGuildRank != GuildRank.Leader)
        {
            dialog.Say("Sorry, only guild leader can spend guild fund in healing this pillar.", "I see.");
            return;
        }

        var input = await dialog.Input("How much would you like to spend in healing this pillar?",
            "I spend", 10, "Let me think it over.");
        npc.Data0 = (int)(int.TryParse(input, out var accepted) ? accepted : 0);

        dialog.Say("Thanks for your fund. This stone pillar will be healed automatically until your fund is used up or its HP is full.",
            "Ok.");
    }

    [Npc(208855)]
    public static async Task LeftGate(Player player, DynaNpc npc, Dialog dialog)
    {
        if (npc.Look == 241)
        {
            if (npc.OwnerId == (player.GuildId ?? 0))
            {
                dialog.Avatar = 110;

                switch (await dialog.Ask("Would you like to enter the Guild House?", "Yes.", "Open the gate.",
                    "Just passing by."))
                {
                    case 0:
                        player.Teleport(1038, 159, 183);
                        break;

                    case 1:
                        if (player.ShownGuildRank < GuildRank.DeputyLeader)
                        {
                            dialog.Say("Sorry, only the guild leader and deputy guild leader can open the gate.",
                                "I see.");
                        }
                        else
                        {
                            npc.SetLook(251);
                        }
                        break;
                }
            }
            else if (player.World.Now.IsWeekly(DayOfWeek.Friday, new(14, 0), DayOfWeek.Friday, new(23, 59)) ||
                player.World.Now.IsWeekly(DayOfWeek.Saturday, new(0, 0), DayOfWeek.Saturday, new(20, 0)))
            {
                if (!player.Has(721261))
                {
                    return;
                }

                dialog.Avatar = 67;

                if (await dialog.Ask("It`s dangerous to use Bomb. And you will lose your life too. Are you sure you want to attack with Bomb?",
                    "Yeah. Go ahead.", "No. I give up.") == 0)
                {
                    await YeahGoAhead();
                }
            }
            else if (player.Has(721261))
            {
                player.Inform("It`s not guild war time. You are strongly recommended not to use Bomb.");
            }
        }
        else if (npc.Look == 251)
        {
            if (npc.Data1 == 1)
            {
                if (npc.OwnerId != (player.GuildId ?? 0) || player.ShownGuildRank != GuildRank.Leader)
                {
                    return;
                }

                dialog.Avatar = 110;

                await dialog.Confirm("Would you like to repair the damaged gate?", "Yes.", "No.");

                if ((player.Guild?.Fund ?? 0) < 300000)
                {
                    player.Inform("Sorry, you can`t repair the gate because your guild fund is less than 300,000 silvers.");
                }
                else if (player.SpendGuildFund(300000) && player.World.DeleteNpc(npc))
                {
                    player.World.CreateNpc(new()
                    {
                        Name = "Close left gate",
                        Type = 26,
                        Sort = 21,
                        Look = 241,
                        OwnerType = 2,
                        OwnerId = player.Map!.OwnerId,
                        MapId = 1038,
                        X = 165,
                        Y = 210,
                        MaxHealth = 10000000,
                        Base = 24,
                        LinkId = 5030100,
                        Task0 = 5030101
                    });
                }
            }
            else if (npc.OwnerId == (player.GuildId ?? 0) && player.ShownGuildRank >= GuildRank.DeputyLeader)
            {
                dialog.Avatar = 110;

                if (await dialog.Ask("Would you like to close the gate?", "Yes.", "No.") == 0)
                {
                    npc.SetLook(241);
                }
            }
        }

        async Task YeahGoAhead()
        {
            if (npc.Look != 241)
            {
                player.Inform("The gate is open. You don`t need to use a Bomb.");
                return;
            }

            if (!player.Has(721261))
            {
                player.Inform("You don`t have a Bomb to attack the gate with.");
                return;
            }

            player.Take(721261);
            player.ShowEffect("firemagic");
            player.ShowEffect("bombarrow");
            player.Sacrifice(1002, 430, 380);
            player.World.Maps[1038].ShowEffect(165, 211, "firemagic");
            player.World.Maps[1038].ShowEffect(165, 211, "bombarrow");
            npc.Data0 = (int)npc.Health;

            if (npc.Data0 >= 3000000)
            {
                npc.Data0 += -2000000;
                npc.SetHealth(npc.Data0);

                if ((int)player.ShownGuildRank < 101)
                {
                    player.World.Announce($"{player.Name} from {(player.Guild?.Name ?? "None")} detonated the Bomb and killed herself/himself. But the left gate was damaged severely.",
                        TalkChannel.Gm);
                }
                else
                {
                    player.World.Announce($"{player.Name} detonated the Bomb and killed herself/himself. But the left gate was damaged severely.",
                        TalkChannel.Gm);
                }

                return;
            }

            npc.Data1 = 1;
            npc.SetLook(251);
            npc.SetHealth(100);

            if ((int)player.ShownGuildRank < 101)
            {
                player.World.Announce($"{player.Name} from {(player.Guild?.Name ?? "None")} detonated the Bomb and killed herself/himself. But the left gate was blown up!",
                    TalkChannel.Gm);
            }
            else
            {
                player.World.Announce($"{player.Name} detonated the Bomb and killed herself/himself. But the left gate was blown up!",
                    TalkChannel.Gm);
            }
        }
    }

    [Npc(208856)]
    public static async Task RightGate(Player player, DynaNpc npc, Dialog dialog)
    {
        if (npc.Look == 277)
        {
            if (npc.OwnerId == (player.GuildId ?? 0))
            {
                dialog.Avatar = 110;

                switch (await dialog.Ask("Would you like to enter the Guild House?", "Yes.", "Open the gate.",
                    "Just passing by."))
                {
                    case 0:
                        player.Teleport(1038, 192, 173);
                        break;

                    case 1:
                        if (player.ShownGuildRank < GuildRank.DeputyLeader)
                        {
                            dialog.Say("Sorry, only the guild leader and deputy guild leader can open the gate.",
                                "I see.");
                        }
                        else
                        {
                            npc.SetLook(287);
                        }
                        break;
                }
            }
            else if (player.World.Now.IsWeekly(DayOfWeek.Friday, new(14, 0), DayOfWeek.Friday, new(23, 59)) ||
                player.World.Now.IsWeekly(DayOfWeek.Saturday, new(0, 0), DayOfWeek.Saturday, new(20, 0)))
            {
                if (!player.Has(721261))
                {
                    return;
                }

                dialog.Avatar = 67;

                if (await dialog.Ask("It`s dangerous to use Bomb. And you will lose your life too. Are you sure you want to attack with Bomb?",
                    "Yes. Go ahead.", "No. I give up.") == 0)
                {
                    await YesGoAhead();
                }
            }
            else if (player.Has(721261))
            {
                player.Inform("It`s not guild war time. You are strongly recommended not to use Bomb.");
            }
        }
        else if (npc.Look == 287)
        {
            if (npc.Data1 == 1)
            {
                if (npc.OwnerId != (player.GuildId ?? 0) || player.ShownGuildRank != GuildRank.Leader)
                {
                    return;
                }

                dialog.Avatar = 110;

                await dialog.Confirm("Would you like to repair the damaged gate?", "Yes.", "No.");

                if ((player.Guild?.Fund ?? 0) < 300000)
                {
                    player.Inform("Sorry, you can`t repair the gate because your guild fund is less than 300,000 silvers.");
                }
                else if (player.SpendGuildFund(300000) && player.World.DeleteNpc(npc))
                {
                    player.World.CreateNpc(new()
                    {
                        Name = "Close right gat",
                        Type = 26,
                        Sort = 21,
                        Look = 277,
                        OwnerType = 2,
                        OwnerId = player.Map!.OwnerId,
                        MapId = 1038,
                        X = 223,
                        Y = 179,
                        MaxHealth = 10000000,
                        Base = 27,
                        LinkId = 5030102,
                        Task0 = 5030103
                    });
                }
            }
            else if (npc.OwnerId == (player.GuildId ?? 0) && player.ShownGuildRank >= GuildRank.DeputyLeader)
            {
                dialog.Avatar = 110;

                if (await dialog.Ask("Would you like to close the gate?", "Yes.", "No.") == 0)
                {
                    npc.SetLook(277);
                }
            }
        }

        async Task YesGoAhead()
        {
            if (npc.Look != 277)
            {
                player.Inform("The gate has been open. You don`t need to use Bomb.");
                return;
            }

            if (!player.Has(721261))
            {
                player.Inform("You don`t have the Bomb to attack the gate.");
                return;
            }

            player.Take(721261);
            player.ShowEffect("firemagic");
            player.ShowEffect("bombarrow");
            player.Sacrifice(1002, 430, 380);
            player.World.Maps[1038].ShowEffect(224, 179, "firemagic");
            player.World.Maps[1038].ShowEffect(224, 179, "bombarrow");
            npc.Data0 = (int)npc.Health;

            if (npc.Data0 >= 3000000)
            {
                npc.Data0 += -2000000;
                npc.SetHealth(npc.Data0);

                if ((int)player.ShownGuildRank < 101)
                {
                    player.World.Announce($"{player.Name} from {(player.Guild?.Name ?? "None")} detonated the Bomb and killed herself/himself. But the right gate was damaged severely.",
                        TalkChannel.Gm);
                }
                else
                {
                    player.World.Announce($"{player.Name} detonated the Bomb and killed herself/himself. But the right gate was damaged severely.",
                        TalkChannel.Gm);
                }

                return;
            }

            npc.Data1 = 1;
            npc.SetLook(287);
            npc.SetHealth(100);

            if ((int)player.ShownGuildRank < 101)
            {
                player.World.Announce($"{player.Name} from {(player.Guild?.Name ?? "None")} detonated the Bomb and killed herself/himself. But the right gate was blown up!",
                    TalkChannel.Gm);
            }
            else
            {
                player.World.Announce($"{player.Name} detonated the Bomb and killed herself/himself. But the right gate was blown up!",
                    TalkChannel.Gm);
            }
        }
    }
}
