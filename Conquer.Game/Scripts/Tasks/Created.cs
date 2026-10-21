namespace Conquer.Game.Scripts.Tasks;

public static class Created
{
    [Task(402941)]
    public static async Task LeftGateDamagedWhenOpened(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 110;

        if (npc.OwnerId != (player.GuildId ?? 0) || player.ShownGuildRank != GuildRank.Leader)
        {
            return;
        }

        await dialog.Confirm("Would you like to repair the gate?", "Yes.", "No.");

        if ((player.Guild?.Fund ?? 0) < 300000 || !player.SpendGuildFund(300000))
        {
            player.Inform("Sorry, you can`t repair the gate because your guild fund is less than 300,000 silvers.");
            return;
        }

        if (npc.Data1 == 1)
        {
            if (npc.OwnerId != (player.GuildId ?? 0) || player.ShownGuildRank != GuildRank.Leader)
            {
                return;
            }

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
            if (await dialog.Ask("Would you like to close the gate?", "Yes.", "No.") == 0)
            {
                npc.SetLook(277);
            }
        }
    }

    [Task(402942)]
    public static async Task RightGateDamagedWhenOpened(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 110;

        if (npc.OwnerId != (player.GuildId ?? 0) || player.ShownGuildRank != GuildRank.Leader)
        {
            return;
        }

        await dialog.Confirm("Would you like to repair the gate?", "Yes.", "No.");

        if (player.ShownGuildRank < GuildRank.DeputyLeader)
        {
            return;
        }

        if (await dialog.Ask("Would you like to close the gate?", "Yes.", "No.") == 0)
        {
            npc.SetLook(277);
        }
    }

    [Task(5020150)]
    public static async Task SolarSaintAtHellGate(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (!player.Has(723085, 5))
        {
            dialog.Say("I need five SoulStones in order to help you.", "I see.");
            return;
        }

        await dialog.Confirm("With these SoulStones I can help you enter the HellGate shielded from poisonous fogs. Are you ready?",
            "I`m ready.", "Wait a moment.");

        dialog.Require(player.PkPoints <= 1000);

        if (!player.Has(723085, 5))
        {
            dialog.Say("I need five SoulStones in order to help you.", "I see.");
            return;
        }

        player.Take(723085, 5);
        player.SetRegister(3, 0);
        player.CountScore(true);

        if (!player.Teleport(4022, 217, 342))
        {
            return;
        }

        npc.Data0 += 1;

        dialog.Later(1, () =>
        {
            if (player.Profession <= Profession.TrojanMaster)
            {
                dialog.Say("You`ll pass if you kill 800 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
            }
            else if (player.Profession <= Profession.WarriorMaster)
            {
                dialog.Say("You`ll pass if you kill 900 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me",
                    "I see.");
            }
            else if (player.Profession <= Profession.ArcherMaster)
            {
                dialog.Say("You`ll pass if you kill 1300 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
            }
            else if (player.Profession <= Profession.WaterSaint)
            {
                dialog.Say("You`ll pass if you kill 600 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
            }
            else
            {
                dialog.Say("You`ll pass if you kill 1000 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
            }
        });

        if (npc.Data0 == 60)
        {
            npc.Data0 = 0;

            if (!player.World.DeleteNpc(npc))
            {
                return;
            }

            player.World.Maps[4021].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)player.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            player.World.Announce("To protect the unrelated persons, I`ll send those who can`t enter HellGate back.",
                TalkChannel.Gm);
        }
        else if (npc.Data0 == 1)
        {
            if (player.ShownGuildRank < GuildRank.Leader)
            {
                player.World.Announce($"{player.Name} of {(player.Guild?.Name ?? "None")} has fought through HellGate and entered HellHall!",
                    TalkChannel.Gm);
            }
            else
            {
                player.World.Announce($"{player.Name} takes lead in entering HellGate by virtue of SoulStones!",
                    TalkChannel.Gm);
            }
        }
        else if (player.ShownGuildRank < GuildRank.Leader)
        {
            player.World.Announce(4021,
                $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has fought through HellGate and entered HellHall!",
                TalkChannel.System);
            player.World.Announce(4022,
                $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has fought through HellGate and entered HellHall!",
                TalkChannel.System);
        }
        else
        {
            player.World.Announce(4021,
                $"No.{npc.Data0} Knight {player.Name} has fought through HellGate and entered HellHall!",
                TalkChannel.System);
            player.World.Announce(4022,
                $"No.{npc.Data0} Knight {player.Name} has fought through HellGate and entered HellHall!",
                TalkChannel.System);
        }
    }

    [Task(5020250)]
    public static async Task SolarSaintInHellHall(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Profession <= Profession.TrojanMaster)
        {
            if (player.GetRegister(3) < 800)
            {
                dialog.Say("You`ll pass if you kill 800 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
                return;
            }

            switch (await dialog.Ask("It`s time to enter HellCloister. Which flank will you take?", "The left.",
                "The right.", "Wait a moment."))
            {
                case 0:
                    await EnterLeftFlank();
                    break;

                case 1:
                    await EnterRightFlank();
                    break;
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            if (player.GetRegister(3) < 900)
            {
                dialog.Say("You`ll pass if you kill 900 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me",
                    "I see.");
                return;
            }

            switch (await dialog.Ask("It`s time to enter HellCloister. Which flank will you take?", "The left.",
                "The right.", "Wait a moment."))
            {
                case 0:
                    await EnterLeftFlank();
                    break;

                case 1:
                    await EnterRightFlank();
                    break;
            }
        }
        else if (player.Profession <= Profession.ArcherMaster)
        {
            if (player.GetRegister(3) < 1300)
            {
                dialog.Say("You`ll pass if you kill 1300 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
                return;
            }

            switch (await dialog.Ask("It`s time to enter HellCloister. Which flank will you take?", "The left.",
                "The right.", "Wait a moment."))
            {
                case 0:
                    await EnterLeftFlank();
                    break;

                case 1:
                    await EnterRightFlank();
                    break;
            }
        }
        else if (player.Profession <= Profession.WaterSaint)
        {
            if (player.GetRegister(3) < 600)
            {
                dialog.Say("You`ll pass if you kill 600 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                    "I see.");
                return;
            }

            switch (await dialog.Ask("It`s time to enter HellCloister. Which flank will you take?", "The left.",
                "The right.", "Wait a moment."))
            {
                case 0:
                    await EnterLeftFlank();
                    break;

                case 1:
                    await EnterRightFlank();
                    break;
            }
        }
        else if (player.GetRegister(3) >= 1000)
        {
            switch (await dialog.Ask("It`s time to enter HellCloister. Which flank will you take?", "The left.",
                "The right.", "Wait a moment."))
            {
                case 0:
                    await EnterLeftFlank();
                    break;

                case 1:
                    await EnterRightFlank();
                    break;
            }
        }
        else
        {
            dialog.Say("You`ll pass if you kill 1000 monsters in HellHall. Hurry up! Only 30 persons can get to HellCloister with me.",
                "I see.");
        }

        async Task EnterLeftFlank()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (npc.Data1 >= 15)
            {
                dialog.Say("We have enough people on the left flank, perhaps you may go to the right flank.", "OK.");
            }
            else if (player.FreeSlots > 0)
            {
                player.SetRegister(3, 0);

                if (player.Give(723087) is null)
                {
                    return;
                }

                npc.Data0 += 1;
                npc.Data1 += 1;

                if (npc.Data0 == 1)
                {
                    if (npc.Data1 != 1)
                    {
                        return;
                    }

                    if ((int)player.ShownGuildRank < 101)
                    {
                        if (!player.Teleport(4023, 297, 656))
                        {
                            return;
                        }

                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"{player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                    else if (player.Teleport(4023, 297, 656))
                    {
                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"Knight {player.Name} has entered the left flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                }
                else if (npc.Data0 >= 30)
                {
                    if ((int)player.ShownGuildRank < 101)
                    {
                        player.World.Announce(4022,
                            $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                        player.World.Announce(4023,
                            $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                    }
                    else
                    {
                        player.World.Announce(4022,
                            $"No.{npc.Data0} Knight {player.Name} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                        player.World.Announce(4023,
                            $"No.{npc.Data0} Knight {player.Name} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                    }

                    if (!player.Teleport(4023, 297, 656))
                    {
                        return;
                    }

                    dialog.Later(1, async () =>
                    {
                        player.CountScore(false);

                        if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                            "Thanks a lot.") == 0)
                        {
                            await ExplainBattleFormation();
                        }
                    });

                    npc.Data0 = 0;

                    if (!player.World.DeleteNpc(npc))
                    {
                        return;
                    }

                    player.World.Maps[4022].Each(-1, (player, dialog) =>
                    {
                        player.SetRegister(4, (uint)player.Roll(32768));
                        player.SetRegister(4, player.GetRegister(4) % 5);

                        dialog.Later(player.GetRegister(4), () =>
                        {
                            player.CountScore(false);
                            player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                                (555, 568)], force: true);
                        });
                    });

                    player.World.Maps[4021].Each(-1, (player, dialog) =>
                    {
                        player.SetRegister(4, (uint)player.Roll(32768));
                        player.SetRegister(4, player.GetRegister(4) % 5);

                        dialog.Later(player.GetRegister(4), () =>
                        {
                            player.CountScore(false);
                            player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                                (555, 568)], force: true);
                        });
                    });

                    player.World.Announce("Time is up! SolarSaint will send those who have not entered HellCloister back.",
                        TalkChannel.Gm);
                }
                else if (npc.Data1 == 1)
                {
                    if ((int)player.ShownGuildRank < 101)
                    {
                        if (!player.Teleport(4023, 297, 656))
                        {
                            return;
                        }

                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"{player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                    else if (player.Teleport(4023, 297, 656))
                    {
                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"{player.Name} has entered the left flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                }
                else if ((int)player.ShownGuildRank < 101)
                {
                    if (!player.Teleport(4023, 297, 656))
                    {
                        return;
                    }

                    dialog.Later(1, async () =>
                    {
                        player.CountScore(false);

                        if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                            "Thanks a lot.") == 0)
                        {
                            await ExplainBattleFormation();
                        }
                    });

                    player.World.Announce(4022,
                        $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                        TalkChannel.System);
                    player.World.Announce(4023,
                        $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                        TalkChannel.System);
                }
                else if (player.Teleport(4023, 297, 656))
                {
                    dialog.Later(1, async () =>
                    {
                        player.CountScore(false);

                        if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                            "Thanks a lot.") == 0)
                        {
                            await ExplainBattleFormation();
                        }
                    });

                    player.World.Announce(4022,
                        $"No.{npc.Data0} Knight {player.Name} has entered the left flank of HellCloister!",
                        TalkChannel.System);
                    player.World.Announce(4023,
                        $"No.{npc.Data1} Knight {player.Name} has entered the left flank of HellCloister!",
                        TalkChannel.System);
                }
            }
            else
            {
                dialog.Say("I`ll give you a LuckyAmulet. Please prepare a slot in your inventory for it.", "OK.");
            }
        }

        async Task EnterRightFlank()
        {
            dialog.Require(player.PkPoints <= 1000);

            if (npc.Data2 >= 15)
            {
                dialog.Say("We have enough people on the right flank. Perhaps you may go left.", "OK.");
            }
            else if (player.FreeSlots > 0)
            {
                player.SetRegister(3, 0);

                if (player.Give(723087) is null)
                {
                    return;
                }

                npc.Data0 += 1;
                npc.Data2 += 1;

                if (npc.Data0 == 1)
                {
                    if (npc.Data2 != 1)
                    {
                        return;
                    }

                    if ((int)player.ShownGuildRank < 101)
                    {
                        if (!player.Teleport(4024, 297, 656))
                        {
                            return;
                        }

                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"{player.Name} of {(player.Guild?.Name ?? "None")} has entered the right flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                    else if (player.Teleport(4024, 297, 656))
                    {
                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"{player.Name} has entered the right flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                }
                else if (npc.Data0 >= 30)
                {
                    if ((int)player.ShownGuildRank < 101)
                    {
                        player.World.Announce(4022,
                            $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                        player.World.Announce(4024,
                            $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                    }
                    else
                    {
                        player.World.Announce(4022,
                            $"No.{npc.Data0} Knight {player.Name} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                        player.World.Announce(4024,
                            $"No.{npc.Data0} Knight {player.Name} has entered the left flank of HellCloister!",
                            TalkChannel.System);
                    }

                    if (!player.Teleport(4024, 297, 656))
                    {
                        return;
                    }

                    dialog.Later(1, async () =>
                    {
                        player.CountScore(false);

                        if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                            "Thanks a lot.") == 0)
                        {
                            await ExplainBattleFormation();
                        }
                    });

                    npc.Data0 = 0;

                    if (!player.World.DeleteNpc(npc))
                    {
                        return;
                    }

                    player.World.Maps[4022].Each(-1, (player, dialog) =>
                    {
                        player.SetRegister(4, (uint)player.Roll(32768));
                        player.SetRegister(4, player.GetRegister(4) % 5);

                        dialog.Later(player.GetRegister(4), () =>
                        {
                            player.CountScore(false);
                            player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                                (555, 568)], force: true);
                        });
                    });

                    player.World.Maps[4021].Each(-1, (player, dialog) =>
                    {
                        player.SetRegister(4, (uint)player.Roll(32768));
                        player.SetRegister(4, player.GetRegister(4) % 5);

                        dialog.Later(player.GetRegister(4), () =>
                        {
                            player.CountScore(false);
                            player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                                (555, 568)], force: true);
                        });
                    });

                    player.World.Announce("Time is up! SolarSaint will send those who have not entered HellCloister back.",
                        TalkChannel.Gm);
                }
                else if (npc.Data2 == 1)
                {
                    if ((int)player.ShownGuildRank < 101)
                    {
                        if (!player.Teleport(4024, 297, 656))
                        {
                            return;
                        }

                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });
                    }
                    else if (player.Teleport(4024, 297, 656))
                    {
                        dialog.Later(1, async () =>
                        {
                            player.CountScore(false);

                            if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                                "Thanks a lot.") == 0)
                            {
                                await ExplainBattleFormation();
                            }
                        });

                        player.World.Announce($"{player.Name} has entered the right flank of HellCloister!",
                            TalkChannel.Gm);
                    }
                }
                else if ((int)player.ShownGuildRank < 101)
                {
                    if (!player.Teleport(4024, 297, 656))
                    {
                        return;
                    }

                    dialog.Later(1, async () =>
                    {
                        player.CountScore(false);

                        if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                            "Thanks a lot.") == 0)
                        {
                            await ExplainBattleFormation();
                        }
                    });

                    player.World.Announce(4022,
                        $"No.{npc.Data0} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the right flank of HellCloister!",
                        TalkChannel.System);
                    player.World.Announce(4024,
                        $"No.{npc.Data2} Knight {player.Name} of {(player.Guild?.Name ?? "None")} has entered the right flank of HellCloister!",
                        TalkChannel.System);
                }
                else if (player.Teleport(4024, 297, 656))
                {
                    dialog.Later(1, async () =>
                    {
                        player.CountScore(false);

                        if (await dialog.Ask("You must work together with others in the same map to kill all the Wraiths in HellCloister. I will give you a\nLuckyAmulet to protect you.",
                            "Thanks a lot.") == 0)
                        {
                            await ExplainBattleFormation();
                        }
                    });

                    player.World.Announce(4022,
                        $"No.{npc.Data0} Knight {player.Name} has entered the right flank of HellCloister!",
                        TalkChannel.System);
                    player.World.Announce(4024,
                        $"No.{npc.Data2} Knight {player.Name} has entered the right flank of HellCloister!",
                        TalkChannel.System);
                }
            }
            else
            {
                dialog.Say("I`ll give you a LuckyAmulet. Please prepare a slot in your inventory for it.", "OK.");
            }
        }

        async Task ExplainBattleFormation()
        {
            if (player.PkPoints <= 1000)
            {
                dialog.Say("After you kill out the Wraiths successfully, I`ll send you into BattleFormation. Please prepare for it timely.",
                    "I see.");
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }
        }
    }

    [Task(5020650)]
    public static async Task SolarSaintInBattleFormation(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (player.Has(790001))
        {
            await dialog.Confirm("Luckily we succeeded! Now I present you StarSword. Do remember to visit me with it someday, and I`ll reward you.",
                "Thank you.");

            dialog.Require(player.PkPoints <= 1000);

            if (!player.Take(790001) || player.Give(723088) is null || !player.Teleport(1020, 566, 565, force: true))
            {
                return;
            }

            player.World.Maps[4025].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)player.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            player.World.Maps[4023].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)player.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            player.World.Maps[4024].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)player.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            player.World.Maps[4022].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)player.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            player.World.Maps[4021].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)player.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            player.World.RemoveGenerated(4021, 2);
            player.World.RemoveGenerated(4022, 2);
            player.World.RemoveGenerated(4023, 2);
            player.World.RemoveGenerated(4024, 2);
            player.World.RemoveGenerated(4025, 2);
            player.World.MoveNpc(3215, 1020, 532, 480);

            if ((int)player.ShownGuildRank < 101)
            {
                player.World.Announce($"SolarSaint has destroyed BattleFormation by virtue of {player.Name} of {(player.Guild?.Name ?? "None")} and leads everybody back safely!",
                    TalkChannel.Gm);
            }
            else
            {
                player.World.Announce($"SolarSaint has destroyed BattleFormation by virtue of {player.Name} and leads everybody back safely!",
                    TalkChannel.Gm);
            }

            return;
        }

        await dialog.Confirm("Without DarkHorn I can`t destroy BattleFormation. Here is very dangerous, you`d better leave right away.",
            "I`d leave now.", "Wait a moment.");

        if (player.PkPoints <= 1000)
        {
            player.Teleport(1020, 566, 565, force: true);
        }
        else
        {
            player.Inform("Error: Failed to meet condition.");
        }
    }

    [Task(5030101)]
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
                    await BombLeftGate();
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

        async Task BombLeftGate()
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

    [Task(5030103)]
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
                    await BombRightGate();
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

        async Task BombRightGate()
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

    [Task(8003010)]
    public static void ItemBox(Player player, Dialog dialog) => player.OpenDialog(4);

    [Task(9501100)]
    public static async Task PrincessDi(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 120;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (!player.Has(722883))
        {
            await dialog.Confirm("Thank you for risking your life to save me, but these bandits are ruthless. Please go quickly before they find you too.",
                "Sorry, I can`t help you.");

            if (player.PkPoints <= 1000)
            {
                player.World.DeleteNpc(npc);
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }

            return;
        }

        await dialog.Confirm("I knew at a glance that a hero like you could rescue me, and so you have. Thank you! Here is my jade pendant. Take it\nto the authorities and claim a big reward!",
            "Thank you, Princess.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.Take(722883) && player.Give(722881) is not null && player.World.DeleteNpc(npc))
        {
            player.Inform("Congratulations! You received the Di family`s jade pendant.");
        }
    }

    [Task(9501200)]
    public static async Task YoungMasterWen(Player player, DynaNpc npc, Dialog dialog)
    {
        dialog.Avatar = 29;

        if (player.PkPoints > 1000)
        {
            return;
        }

        if (!player.Has(722883))
        {
            await dialog.Confirm("Thank you for taking such a risk, but these bandits are ruthless and I won`t drag you down with me. Please leave now!",
                "Sorry, I am too weak to save you.");

            if (player.PkPoints <= 1000)
            {
                player.World.DeleteNpc(npc);
            }
            else
            {
                player.Inform("Error: Failed to meet condition.");
            }

            return;
        }

        await dialog.Confirm("You braved the bandits to save me, and I will never forget it. Here is the Wen family token. Take it to the authorities!",
            "Thank you, young master.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.Take(722883) && player.Give(722882) is not null && player.World.DeleteNpc(npc))
        {
            player.Inform("Congratulations! You received the Wen family`s token.");
        }
    }
}
