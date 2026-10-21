namespace Conquer.Game.Scripts.Events;

public static class Events
{
    [Minute(2030000)]
    public static void OpenPkTournamentEntry(World world)
    {
        if (world.Now.IsMonthly(1, new(20, 30), 1, new(20, 32)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(20, 30), DayOfWeek.Saturday, new(20, 32)))
        {
            world.MoveNpc(31, 1002, 452, 292);
            world.MoveNpc(10021, 5000, 28, 85);
        }
    }

    [Minute(2030001)]
    public static void ClosePkTournamentEntry(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 10), 1, new(21, 12)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 10), DayOfWeek.Saturday, new(21, 12)))
        {
            world.MoveNpc(31, 5000, 28, 82);
        }
    }

    [Minute(2030002)]
    public static void BeginAndEndPkTournament(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 10), 1, new(21, 12)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 10), DayOfWeek.Saturday, new(21, 12)))
        {
            world.MoveNpc(32, 1528, 106, 115);
            world.MoveNpc(145, 1508, 106, 115);
            world.MoveNpc(146, 1518, 106, 115);
        }
        else if (world.Now.IsMonthly(1, new(23, 57), 1, new(23, 59)) ||
            world.Now.IsWeekly(DayOfWeek.Sunday, new(0, 1), DayOfWeek.Sunday, new(0, 3)))
        {
            world.MoveNpc(32, 5000, 32, 78);
            world.MoveNpc(10021, 1002, 459, 291);
        }
    }

    [Minute(2030005)]
    public static void AnnounceJailPardon(World world)
    {
        if (world.Now.IsHourly(0, 2) || world.Now.IsHourly(30, 32))
        {
            world.Announce(6001,
                "It is time for general pardon. You may leave now. Behave yourself after you are out of the jail.",
                TalkChannel.System);
        }
    }

    [Minute(2030006)]
    public static void ScheduleDruggist(World world)
    {
        if (world.Now.IsHourly(1, 2) || world.Now.IsHourly(21, 22) || world.Now.IsHourly(41, 42))
        {
            world.MoveNpc(30087, 1002, 385, 371);
        }
        else if (world.Now.IsHourly(4, 5) || world.Now.IsHourly(24, 25) || world.Now.IsHourly(44, 45))
        {
            world.MoveNpc(30087, 5000, 52, 56);
        }
    }

    [Minute(2030007)]
    public static void ScheduleEmilyInApeCity(World world)
    {
        if (world.Now.IsDaily(new(23, 0), new(23, 1)))
        {
            switch (world.Roll(1, 4))
            {
                case 1:
                    world.MoveNpc(600067, 1020, 565, 730);
                    break;

                case 2:
                    world.MoveNpc(600067, 1020, 455, 460);
                    break;

                case 3:
                    world.MoveNpc(600067, 1020, 54, 333);
                    break;

                case 4:
                    world.MoveNpc(600067, 1020, 600, 500);
                    break;
            }
        }
        else if (world.Now.IsDaily(new(0, 58), new(0, 59)))
        {
            world.MoveNpc(600067, 5000, 27, 77);
        }
    }

    [Minute(2030008)]
    public static void ScheduleTientsinInDreamland(World world)
    {
        if (world.Now.IsDaily(new(13, 0), new(13, 1)) || world.Now.IsDaily(new(14, 0), new(14, 1)))
        {
            world.MoveNpc(30122, 1012, 67, 65);
        }
        else if (world.Now.IsDaily(new(17, 58), new(17, 59)))
        {
            world.MoveNpc(30122, 5000, 58, 59);
        }
    }

    [Minute(2030010)]
    public static void BroadcastGmTips(World world)
    {
        if (world.Now.IsHourly(0, 1))
        {
            world.Announce("GMs have [GM] or [PM] at the end of their names. Don`t trust anyone claiming to be a GM but not having [GM]/[PM] in their names",
                TalkChannel.Gm);
        }
        else if (world.Now.IsHourly(20, 21))
        {
            world.Announce("File account cases at \"Account Issue\" under \"Support\". File dragonball purchase problems at \"Purchase Problems\" under \"Support\"",
                TalkChannel.Gm);
        }
        else if (world.Now.IsHourly(40, 41))
        {
            world.Announce("Refer Conquer Online to your friends: http://account.conqueronline.com/enzf/referrals/referrals.php",
                TalkChannel.Gm);
        }
        else if (world.Now.IsHourly(50, 51))
        {
            world.Announce("Help UnknownMan at Market (212,204) to level up quickly.", TalkChannel.Gm);
        }
        else if (world.Now.IsHourly(55, 56))
        {
            world.Announce("www.conqueronline.com is the only official website of CO, which is also the only website where you can purchase Dragonballs.",
                TalkChannel.Gm);
        }
    }

    [Minute(2030013)]
    public static void SpawnGuildBeast(World world)
    {
        var beastGuard = world.FindNpc(819)!;

        if (world.Now.IsDaily(new(21, 0), new(21, 0)))
        {
            if (beastGuard.OwnerId == 0)
            {
                return;
            }

            if (beastGuard.Data2 != 1)
            {
                beastGuard.Data0 = 1;
                world.Spawn(3120, 1038, 86, 78);
            }
        }
        else if (world.Now.IsDaily(new(21, 59), new(22, 0)) && world.CountMonsters("GuildBeast", 1038) == 1)
        {
            world.RemoveMonsters(1038, 3120);
            beastGuard.Data0 = 3;
        }
    }

    [Minute(2030014)]
    public static void AnnounceGuildBeast(World world)
    {
        if (world.Now.IsDaily(new(20, 57), new(20, 58)) || world.Now.IsDaily(new(20, 58), new(20, 59)) ||
            world.Now.IsDaily(new(20, 59), new(21, 0)))
        {
            world.Announce("The Guild Beast will come to the Guild Map(86,78) at 21:00.", TalkChannel.Gm);
        }
    }

    [Minute(2030015, 2030016)]
    public static void EmptyTimer(World world)
    {
    }

    [Minute(2030018)]
    public static void CallLowBracketToNextRoundOldMaps(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)) ||
            world.Now.IsMonthly(1, new(21, 28), 1, new(21, 29)) || world.Now.IsMonthly(1, new(21, 29), 1, new(21, 30)))
        {
            world.Announce(1080,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1080,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1080,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1080,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1080,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 28)) ||
            world.Now.IsMonthly(1, new(22, 28), 1, new(22, 29)) || world.Now.IsMonthly(1, new(22, 29), 1, new(22, 30)))
        {
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 28), DayOfWeek.Saturday, new(21, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 29), DayOfWeek.Saturday, new(21, 30)))
        {
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 28), DayOfWeek.Saturday, new(22, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 29), DayOfWeek.Saturday, new(22, 30)))
        {
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
    }

    [Minute(2030019)]
    public static void CallMidBracketToNextRoundOldMaps(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)) ||
            world.Now.IsMonthly(1, new(21, 28), 1, new(21, 29)) || world.Now.IsMonthly(1, new(21, 29), 1, new(21, 30)))
        {
            world.Announce(1081,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1081,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1081,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1081,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1081,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 29)) ||
            world.Now.IsMonthly(1, new(22, 28), 1, new(22, 29)) || world.Now.IsMonthly(1, new(22, 29), 1, new(22, 30)))
        {
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 28), DayOfWeek.Saturday, new(21, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 29), DayOfWeek.Saturday, new(21, 30)))
        {
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 28), DayOfWeek.Saturday, new(22, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 29), DayOfWeek.Saturday, new(22, 30)))
        {
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
    }

    [Minute(2030020)]
    public static void CallHighBracketToNextRoundOldMaps(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)) ||
            world.Now.IsMonthly(1, new(21, 28), 1, new(21, 29)) || world.Now.IsMonthly(1, new(21, 29), 1, new(21, 30)))
        {
            world.Announce(1018,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1018,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1018,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1018,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1018,
                "Please find ArenaGuard(240,154) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 29)) ||
            world.Now.IsMonthly(1, new(22, 28), 1, new(22, 29)) || world.Now.IsMonthly(1, new(22, 29), 1, new(22, 30)))
        {
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 28), DayOfWeek.Saturday, new(21, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 29), DayOfWeek.Saturday, new(21, 30)))
        {
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 28), DayOfWeek.Saturday, new(22, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 29), DayOfWeek.Saturday, new(22, 30)))
        {
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
    }

    [Minute(2030021)]
    public static void ShowPkTournamentArenaGuards(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)))
        {
            world.MoveNpc(20060, 1508, 105, 126);
            world.MoveNpc(20090, 1518, 105, 126);
            world.MoveNpc(20120, 1528, 105, 126);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 28)))
        {
            world.MoveNpc(20060, 1090, 162, 133);
            world.MoveNpc(20090, 1092, 162, 133);
            world.MoveNpc(20120, 1094, 162, 133);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)))
        {
            world.MoveNpc(20060, 1508, 105, 126);
            world.MoveNpc(20090, 1518, 105, 126);
            world.MoveNpc(20120, 1528, 105, 126);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)))
        {
            world.MoveNpc(20060, 1090, 162, 133);
            world.MoveNpc(20090, 1092, 162, 133);
            world.MoveNpc(20120, 1094, 162, 133);
        }
    }

    [Minute(2030022)]
    public static void HidePkTournamentArenaGuards(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 30), 1, new(21, 31)) ||
            world.Now.IsMonthly(1, new(22, 30), 1, new(22, 31)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 30), DayOfWeek.Saturday, new(21, 31)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 30), DayOfWeek.Saturday, new(22, 31)))
        {
            world.MoveNpc(20060, 5000, 30, 78);
            world.MoveNpc(20090, 5000, 32, 78);
            world.MoveNpc(20120, 5000, 34, 78);
        }
    }

    [Minute(2030024)]
    public static void MovePkTournamentGenerals(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 26), 1, new(21, 27)))
        {
            world.MoveNpc(145, 5000, 35, 78);
            world.MoveNpc(146, 5000, 37, 78);
            world.MoveNpc(32, 5000, 32, 78);
        }
        else if (world.Now.IsMonthly(1, new(21, 30), 1, new(21, 31)))
        {
            world.MoveNpc(145, 1090, 167, 133);
            world.MoveNpc(146, 1092, 167, 133);
            world.MoveNpc(32, 1094, 167, 133);
        }
        else if (world.Now.IsMonthly(1, new(22, 26), 1, new(22, 27)))
        {
            world.MoveNpc(145, 5000, 35, 78);
            world.MoveNpc(146, 5000, 37, 78);
            world.MoveNpc(32, 5000, 32, 78);
        }
        else if (world.Now.IsMonthly(1, new(22, 30), 1, new(22, 31)))
        {
            world.MoveNpc(145, 1091, 47, 33);
            world.MoveNpc(146, 1093, 47, 33);
            world.MoveNpc(32, 1095, 47, 33);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 26), DayOfWeek.Saturday, new(21, 27)))
        {
            world.MoveNpc(145, 5000, 35, 78);
            world.MoveNpc(146, 5000, 37, 78);
            world.MoveNpc(32, 5000, 32, 78);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 30), DayOfWeek.Saturday, new(21, 31)))
        {
            world.MoveNpc(145, 1090, 167, 133);
            world.MoveNpc(146, 1092, 167, 133);
            world.MoveNpc(32, 1094, 167, 133);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 26), DayOfWeek.Saturday, new(22, 27)))
        {
            world.MoveNpc(145, 5000, 35, 78);
            world.MoveNpc(146, 5000, 37, 78);
            world.MoveNpc(32, 5000, 32, 78);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 30), DayOfWeek.Saturday, new(22, 31)))
        {
            world.MoveNpc(145, 1091, 47, 33);
            world.MoveNpc(146, 1093, 47, 33);
            world.MoveNpc(32, 1095, 47, 33);
        }
    }

    [Minute(2030030)]
    public static void ScheduleRedPacketSeller(World world)
    {
        if (world.Now.IsBetween(new(2004, 1, 22, 0, 0, 0), new(2004, 1, 22, 0, 10, 0)))
        {
            world.MoveNpc(7212, 1002, 424, 341);
        }
        else if (world.Now.IsBetween(new(2004, 1, 30, 0, 0, 0), new(2004, 1, 30, 0, 10, 0)))
        {
            world.MoveNpc(7212, 5000, 110, 60);
        }
    }

    [Minute(2030031)]
    public static void ScatterSquamasInTwinCityAndPhoenixCastle(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1002, 445, 231, 0);
            world.CreateTrap(51, 11, 1002, 387, 369, 0);
            world.CreateTrap(51, 11, 1002, 939, 570, 0);
            world.CreateTrap(51, 11, 1002, 850, 651, 0);
            world.CreateTrap(51, 11, 1011, 75, 416, 0);
            world.CreateTrap(51, 11, 1011, 134, 283, 0);
            world.CreateTrap(51, 11, 1011, 288, 122, 0);
            world.CreateTrap(51, 11, 1011, 381, 119, 0);
            world.CreateTrap(51, 11, 1011, 490, 171, 0);
            world.CreateTrap(51, 11, 1011, 423, 60, 0);
        }
    }

    [Minute(2030032)]
    public static void ScatterSquamasInPhoenixCastleAndCaves(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1011, 610, 328, 0);
            world.CreateTrap(51, 11, 1011, 752, 425, 0);
            world.CreateTrap(51, 11, 1011, 800, 478, 0);
            world.CreateTrap(51, 11, 1011, 528, 757, 0);
            world.CreateTrap(51, 11, 1011, 653, 837, 0);
            world.CreateTrap(51, 11, 1011, 893, 541, 0);
            world.CreateTrap(51, 11, 1011, 158, 241, 0);
            world.CreateTrap(51, 11, 1011, 222, 232, 0);
            world.CreateTrap(51, 11, 1013, 27, 35, 0);
            world.CreateTrap(51, 11, 1014, 116, 116, 0);
        }
    }

    [Minute(2030033)]
    public static void ScatterSquamasInNewForestAndApeCity(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1016, 51, 41, 0);
            world.CreateTrap(51, 11, 1076, 149, 411, 0);
            world.CreateTrap(51, 11, 1020, 238, 162, 0);
            world.CreateTrap(51, 11, 1076, 272, 335, 0);
            world.CreateTrap(51, 11, 1076, 692, 593, 0);
            world.CreateTrap(51, 11, 1076, 312, 200, 0);
            world.CreateTrap(51, 11, 1076, 907, 683, 0);
            world.CreateTrap(51, 11, 1020, 561, 664, 0);
            world.CreateTrap(51, 11, 1020, 811, 647, 0);
            world.CreateTrap(51, 11, 1020, 716, 406, 0);
        }
    }

    [Minute(2030034)]
    public static void ScatterSquamasInApeCityAndNewCanyon(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1020, 196, 425, 0);
            world.CreateTrap(51, 11, 1020, 482, 596, 0);
            world.CreateTrap(51, 11, 1020, 296, 350, 0);
            world.CreateTrap(51, 11, 1020, 537, 513, 0);
            world.CreateTrap(51, 11, 1020, 543, 480, 0);
            world.CreateTrap(51, 11, 1075, 125, 364, 0);
            world.CreateTrap(51, 11, 1075, 205, 502, 0);
            world.CreateTrap(51, 11, 1075, 346, 678, 0);
            world.CreateTrap(51, 11, 1075, 655, 339, 0);
            world.CreateTrap(51, 11, 1075, 789, 589, 0);
        }
    }

    [Minute(2030035)]
    public static void ScatterSquamasInDesertCity(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1000, 514, 591, 0);
            world.CreateTrap(51, 11, 1000, 506, 603, 0);
            world.CreateTrap(51, 11, 1000, 639, 525, 0);
            world.CreateTrap(51, 11, 1000, 804, 601, 0);
            world.CreateTrap(51, 11, 1000, 718, 758, 0);
            world.CreateTrap(51, 11, 1000, 529, 334, 0);
            world.CreateTrap(51, 11, 1000, 304, 129, 0);
            world.CreateTrap(51, 11, 1000, 223, 226, 0);
            world.CreateTrap(51, 11, 1000, 259, 458, 0);
            world.CreateTrap(51, 11, 1000, 97, 298, 0);
        }
    }

    [Minute(2030036)]
    public static void ScatterSquamasInNewDesertAndBirdIsland(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1077, 233, 383, 0);
            world.CreateTrap(51, 11, 1077, 383, 264, 0);
            world.CreateTrap(51, 11, 1077, 459, 328, 0);
            world.CreateTrap(51, 11, 1077, 558, 663, 0);
            world.CreateTrap(51, 11, 1077, 806, 530, 0);
            world.CreateTrap(51, 11, 1015, 686, 565, 0);
            world.CreateTrap(51, 11, 1015, 856, 576, 0);
            world.CreateTrap(51, 11, 1015, 774, 779, 0);
            world.CreateTrap(51, 11, 1015, 555, 779, 0);
            world.CreateTrap(51, 11, 1015, 574, 584, 0);
        }
    }

    [Minute(2030037)]
    public static void ScatterSquamasInBirdIslandAndMysticCastle(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1015, 375, 436, 0);
            world.CreateTrap(51, 11, 1015, 280, 90, 0);
            world.CreateTrap(51, 11, 1015, 174, 190, 0);
            world.CreateTrap(51, 11, 1015, 209, 419, 0);
            world.CreateTrap(51, 11, 1015, 313, 546, 0);
            world.CreateTrap(51, 11, 1015, 686, 960, 0);
            world.CreateTrap(51, 11, 1001, 351, 610, 0);
            world.CreateTrap(51, 11, 1001, 348, 553, 0);
            world.CreateTrap(51, 11, 1001, 323, 438, 0);
            world.CreateTrap(51, 11, 1001, 222, 370, 0);
        }
    }

    [Minute(2030038)]
    public static void ScatterSquamasInMysticCastleAndMarket(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1001, 143, 316, 0);
            world.CreateTrap(51, 11, 1001, 90, 280, 0);
            world.CreateTrap(51, 11, 1001, 358, 248, 0);
            world.CreateTrap(51, 11, 1001, 320, 168, 0);
            world.CreateTrap(51, 11, 1001, 529, 202, 0);
            world.CreateTrap(51, 11, 1001, 492, 508, 0);
            world.CreateTrap(51, 11, 1036, 213, 310, 0);
            world.CreateTrap(51, 11, 1036, 98, 200, 0);
            world.CreateTrap(51, 11, 1036, 71, 73, 0);
            world.CreateTrap(51, 11, 1036, 160, 101, 0);
        }
    }

    [Minute(2030039)]
    public static void ScatterSquamasInGlobeMapsAndMysticCave(World world)
    {
        if (world.Now.IsDaily(new(20, 0), new(20, 0)))
        {
            world.CreateTrap(51, 11, 1036, 226, 158, 0);
            world.CreateTrap(51, 11, 1213, 422, 252, 0);
            world.CreateTrap(51, 11, 1214, 569, 453, 0);
            world.CreateTrap(51, 11, 1214, 612, 1106, 0);
            world.CreateTrap(51, 11, 1214, 1012, 1147, 0);
            world.CreateTrap(51, 11, 1215, 590, 497, 0);
            world.CreateTrap(51, 11, 1216, 398, 524, 0);
            world.CreateTrap(51, 11, 1217, 547, 493, 0);
            world.CreateTrap(51, 11, 1300, 314, 593, 0);
            world.CreateTrap(51, 11, 1300, 513, 346, 0);
        }
    }

    [Minute(2030057)]
    public static void CallLowBracketToNextRound(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)) ||
            world.Now.IsMonthly(1, new(21, 28), 1, new(21, 29)) || world.Now.IsMonthly(1, new(21, 29), 1, new(21, 30)))
        {
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 28)) ||
            world.Now.IsMonthly(1, new(22, 28), 1, new(22, 29)) || world.Now.IsMonthly(1, new(22, 29), 1, new(22, 30)))
        {
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 28), DayOfWeek.Saturday, new(21, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 29), DayOfWeek.Saturday, new(21, 30)))
        {
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1508,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 28), DayOfWeek.Saturday, new(22, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 29), DayOfWeek.Saturday, new(22, 30)))
        {
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1090,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
    }

    [Minute(2030058)]
    public static void CallMidBracketToNextRound(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)) ||
            world.Now.IsMonthly(1, new(21, 28), 1, new(21, 29)) || world.Now.IsMonthly(1, new(21, 29), 1, new(21, 30)))
        {
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 29)) ||
            world.Now.IsMonthly(1, new(22, 28), 1, new(22, 29)) || world.Now.IsMonthly(1, new(22, 29), 1, new(22, 30)))
        {
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 28), DayOfWeek.Saturday, new(21, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 29), DayOfWeek.Saturday, new(21, 30)))
        {
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1518,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 28), DayOfWeek.Saturday, new(22, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 29), DayOfWeek.Saturday, new(22, 30)))
        {
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1092,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
    }

    [Minute(2030059)]
    public static void CallHighBracketToNextRound(World world)
    {
        if (world.Now.IsMonthly(1, new(21, 27), 1, new(21, 28)) ||
            world.Now.IsMonthly(1, new(21, 28), 1, new(21, 29)) || world.Now.IsMonthly(1, new(21, 29), 1, new(21, 30)))
        {
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsMonthly(1, new(22, 27), 1, new(22, 29)) ||
            world.Now.IsMonthly(1, new(22, 28), 1, new(22, 29)) || world.Now.IsMonthly(1, new(22, 29), 1, new(22, 30)))
        {
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 27), DayOfWeek.Saturday, new(21, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 28), DayOfWeek.Saturday, new(21, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(21, 29), DayOfWeek.Saturday, new(21, 30)))
        {
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1528,
                "Please find ArenaGuard(105,126) to enter the next PK map before 21:30. Or you will lose by default.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 27), DayOfWeek.Saturday, new(22, 28)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 28), DayOfWeek.Saturday, new(22, 29)) ||
            world.Now.IsWeekly(DayOfWeek.Saturday, new(22, 29), DayOfWeek.Saturday, new(22, 30)))
        {
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
            world.Announce(1094,
                "Please find ArenaGuard(162,133) to enter the next PK map before 22:30. Or you will lose by default.",
                TalkChannel.System);
        }
    }

    [Minute(2030060)]
    public static void OpenDisCity(World world)
    {
        if (world.Now.IsWeekly(DayOfWeek.Monday, new(20, 0), DayOfWeek.Monday, new(20, 0)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(12, 0), DayOfWeek.Tuesday, new(12, 0)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(20, 0), DayOfWeek.Wednesday, new(20, 0)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(12, 0), DayOfWeek.Thursday, new(12, 0)))
        {
            world.DeleteNpcs(4021, 2);
            world.DeleteNpcs(4022, 2);
            world.DeleteNpcs(4025, 2);

            var firstSaint = new DynaNpc
            {
                Name = "SolarSaint",
                Type = 2,
                Sort = 1,
                Look = 1310,
                MapId = 4021,
                X = 253,
                Y = 57,
                Task0 = 5020150
            };

            var secondSaint = new DynaNpc
            {
                Name = "SolarSaint",
                Type = 2,
                Sort = 1,
                Look = 1314,
                MapId = 4022,
                X = 215,
                Y = 344,
                Task0 = 5020250
            };

            if (world.CreateNpc(firstSaint) && world.CreateNpc(secondSaint))
            {
                world.RemoveMonsters(4025, 5058);
                world.RemoveMonsters(4025, 5059);
                world.Spawn(5058, 4025, 156, 152);
                world.Spawn(5058, 4025, 149, 152);
                world.Spawn(5058, 4025, 142, 152);
                world.Spawn(5058, 4025, 135, 152);
                world.Announce("The quest starts now, hurry up! Go to Ape City, and find SolarSaint to enter Dis City.");
            }
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(19, 55), DayOfWeek.Monday, new(19, 55)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(11, 55), DayOfWeek.Tuesday, new(11, 55)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(19, 55), DayOfWeek.Wednesday, new(19, 55)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(11, 55), DayOfWeek.Thursday, new(11, 55)))
        {
            world.Announce("Five minutes left. You must find SolarSaint to enter Dis City in five minutes.");
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(20, 2), DayOfWeek.Monday, new(20, 2)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(12, 2), DayOfWeek.Tuesday, new(12, 2)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(20, 2), DayOfWeek.Wednesday, new(20, 2)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(12, 2), DayOfWeek.Thursday, new(12, 2)))
        {
            world.Announce("Only three minutes left. You must find SolarSaint to enter Dis City in three minutes.");
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(19, 58), DayOfWeek.Monday, new(19, 58)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(11, 58), DayOfWeek.Tuesday, new(11, 58)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(19, 58), DayOfWeek.Wednesday, new(19, 58)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(11, 58), DayOfWeek.Thursday, new(11, 58)))
        {
            world.Announce("Only two minutes left. You must find SolarSaint to enter Dis City in two minutes.");
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(20, 4), DayOfWeek.Monday, new(20, 4)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(12, 4), DayOfWeek.Tuesday, new(12, 4)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(20, 4), DayOfWeek.Wednesday, new(20, 4)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(12, 4), DayOfWeek.Thursday, new(12, 4)))
        {
            world.Announce("Only one minute left. You must find SolarSaint to enter Dis City in one minute.");
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(20, 5), DayOfWeek.Monday, new(20, 5)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(12, 5), DayOfWeek.Tuesday, new(12, 5)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(20, 5), DayOfWeek.Wednesday, new(20, 5)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(12, 5), DayOfWeek.Thursday, new(12, 5)))
        {
            world.MoveNpc(3215, 5000, 104, 111);
            world.Announce("You`ve run out of time. Those who can`t enter Dis City this time may come next time.");
        }
    }

    [Minute(2030061)]
    public static void CloseDisCity(World world)
    {
        if (world.Now.IsWeekly(DayOfWeek.Monday, new(21, 10), DayOfWeek.Monday, new(21, 11)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Tuesday, new(13, 10), DayOfWeek.Tuesday, new(13, 11)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Wednesday, new(21, 10), DayOfWeek.Wednesday, new(21, 11)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Thursday, new(13, 10), DayOfWeek.Thursday, new(13, 11)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(21, 14), DayOfWeek.Monday, new(21, 15)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Tuesday, new(13, 14), DayOfWeek.Tuesday, new(13, 15)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Wednesday, new(21, 14), DayOfWeek.Wednesday, new(21, 15)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Thursday, new(13, 14), DayOfWeek.Thursday, new(13, 15)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(21, 19), DayOfWeek.Monday, new(21, 20)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Tuesday, new(13, 19), DayOfWeek.Tuesday, new(13, 20)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Wednesday, new(21, 19), DayOfWeek.Wednesday, new(21, 20)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(13, 19), DayOfWeek.Thursday, new(13, 20)) ||
            world.Now.IsWeekly(DayOfWeek.Monday, new(21, 24), DayOfWeek.Monday, new(21, 25)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Tuesday, new(13, 24), DayOfWeek.Tuesday, new(13, 25)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Wednesday, new(21, 24), DayOfWeek.Wednesday, new(21, 25)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 21:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Thursday, new(13, 24), DayOfWeek.Thursday, new(13, 25)))
        {
            world.Announce(4025, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4024, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4023, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4022, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
            world.Announce(4021, "Time is running short. SolarSaint will lead you to leave at 13:30.",
                TalkChannel.System);
        }
        else if (world.Now.IsWeekly(DayOfWeek.Monday, new(21, 30), DayOfWeek.Monday, new(21, 30)) ||
            world.Now.IsWeekly(DayOfWeek.Tuesday, new(13, 30), DayOfWeek.Tuesday, new(13, 30)) ||
            world.Now.IsWeekly(DayOfWeek.Wednesday, new(21, 30), DayOfWeek.Wednesday, new(21, 30)) ||
            world.Now.IsWeekly(DayOfWeek.Thursday, new(13, 30), DayOfWeek.Thursday, new(13, 30)))
        {
            world.DeleteNpcs(4021, 2);
            world.DeleteNpcs(4022, 2);
            world.DeleteNpcs(4025, 2);

            world.Maps[4025].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)world.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            world.Maps[4021].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)world.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            world.Maps[4022].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)world.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            world.Maps[4023].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)world.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            world.Maps[4024].Each(-1, (player, dialog) =>
            {
                player.SetRegister(4, (uint)world.Roll(32768));
                player.SetRegister(4, player.GetRegister(4) % 5);

                dialog.Later(player.GetRegister(4), () =>
                {
                    player.CountScore(false);
                    player.Teleport(1020, [(561, 496), (540, 486), (518, 490), (533, 514), (549, 512), (554, 528), (578, 567),
                        (555, 568)], force: true);
                });
            });

            world.MoveNpc(3215, 1020, 532, 480);
            world.Announce("The quest is over. Thanks a lot for your participation.", TalkChannel.Gm);
        }
    }

    [Minute(2030071)]
    public static void SpawnGanoderma(World world)
    {
        if (!world.Now.IsHourly(15, 15))
        {
            return;
        }

        if (world.CountMonsters("GanodermaL72", 1011) == 0)
        {
            world.Spawn(3130, 1011, 659, 772);
            world.Announce("Warning! Ganoderma has appeared in the forest.", TalkChannel.Gm);
        }
        else if (world.CountMonsters("GanodermaL70", 1011) == 0)
        {
            world.Spawn(3131, 1011, 662, 775);
            world.Announce("Warning! Ganoderma has appeared in the forest.", TalkChannel.Gm);
        }
        else if (world.CountMonsters("GanodermaL74", 1011) == 0)
        {
            world.Spawn(3132, 1011, 662, 777);
            world.Announce("Warning! Ganoderma has appeared in the forest.", TalkChannel.Gm);
        }
        else if (world.CountMonsters("GanodermaL76", 1011) == 0)
        {
            world.Spawn(3133, 1011, 665, 779);
        }
        else
        {
            world.Announce("Warning! Ganoderma has appeared in the forest.", TalkChannel.Gm);
        }
    }

    [Minute(2030072)]
    public static void SpawnTitan(World world)
    {
        if (!world.Now.IsHourly(16, 16))
        {
            return;
        }

        if (world.CountMonsters("TitanL77", 1020) == 0)
        {
            world.Spawn(3134, 1020, 396, 588);
            world.Announce("Warning! Titan has appeared in the canyon.", TalkChannel.Gm);
        }
        else if (world.CountMonsters("TitanL75", 1020) == 0)
        {
            world.Spawn(3135, 1020, 398, 590);
            world.Announce("Warning! Titan has appeared in the canyon.", TalkChannel.Gm);
        }
        else if (world.CountMonsters("TitanL79", 1020) == 0)
        {
            world.Spawn(3136, 1020, 399, 593);
            world.Announce("Warning! Titan has appeared in the canyon.", TalkChannel.Gm);
        }
        else if (world.CountMonsters("TitanL81", 1020) == 0)
        {
            world.Spawn(3137, 1020, 401, 592);
        }
        else
        {
            world.Announce("Warning! Titan has appeared in the canyon.", TalkChannel.Gm);
        }
    }

    [Minute(2030096)]
    public static void RotateMonthlyLuna2006(World world)
    {
        if (world.Now.IsBetween(new(2006, 3, 29, 0, 0, 0), new(2006, 4, 27, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3640, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 4, 28, 0, 0, 0), new(2006, 5, 26, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3641, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 5, 27, 0, 0, 0), new(2006, 6, 25, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3642, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 6, 26, 0, 0, 0), new(2006, 7, 24, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3643, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 7, 25, 0, 0, 0), new(2006, 8, 23, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3644, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 8, 24, 0, 0, 0), new(2006, 9, 21, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3645, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 9, 22, 0, 0, 0), new(2006, 10, 21, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3646, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 10, 22, 0, 0, 0), new(2006, 11, 20, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3647, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 11, 21, 0, 0, 0), new(2006, 12, 19, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3648, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2006, 12, 20, 0, 0, 0), new(2007, 1, 18, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3649, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 1, 19, 0, 0, 0), new(2007, 2, 17, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3640, 5000, 39, 90);
            world.MoveNpc(3641, 5000, 40, 90);
            world.MoveNpc(3642, 5000, 41, 90);
            world.MoveNpc(3643, 5000, 42, 90);
            world.MoveNpc(3644, 5000, 43, 90);
            world.MoveNpc(3645, 5000, 44, 90);
            world.MoveNpc(3646, 5000, 45, 90);
            world.MoveNpc(3647, 5000, 46, 90);
            world.MoveNpc(3648, 5000, 47, 90);
            world.MoveNpc(3649, 5000, 48, 90);
            world.MoveNpc(3650, 5000, 49, 90);
            world.MoveNpc(3650, 1002, 372, 407);
        }
    }

    [Minute(2030097)]
    public static void RotateMonthlyLuna2007(World world)
    {
        if (world.Now.IsBetween(new(2007, 2, 18, 0, 0, 0), new(2007, 3, 18, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3650, 5000, 37, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 40, 90);
            world.MoveNpc(3668, 5000, 41, 90);
            world.MoveNpc(3669, 5000, 42, 90);
            world.MoveNpc(3670, 5000, 43, 90);
            world.MoveNpc(3671, 5000, 44, 90);
            world.MoveNpc(3672, 5000, 45, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3666, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 3, 19, 0, 0, 0), new(2007, 4, 16, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3667, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 4, 17, 0, 0, 0), new(2007, 5, 16, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3668, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 5, 17, 0, 0, 0), new(2007, 6, 14, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3669, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 6, 15, 0, 0, 0), new(2007, 7, 13, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3670, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 7, 14, 0, 0, 0), new(2007, 8, 12, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3671, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 8, 13, 0, 0, 0), new(2007, 9, 10, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3672, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 9, 11, 0, 0, 0), new(2007, 10, 10, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);

            if (world.MoveNpc(3673, 5000, 46, 90))
            {
                world.MoveNpc(3674, 5000, 47, 90);

                if (world.MoveNpc(3675, 5000, 48, 90))
                {
                    world.MoveNpc(3676, 5000, 49, 90);
                    world.MoveNpc(3673, 1002, 372, 407);
                }
                else
                {
                    world.MoveNpc(3666, 5000, 39, 90);
                    world.MoveNpc(3667, 5000, 39, 90);
                    world.MoveNpc(3668, 5000, 39, 90);
                    world.MoveNpc(3669, 5000, 39, 90);
                    world.MoveNpc(3670, 5000, 39, 90);
                    world.MoveNpc(3671, 5000, 39, 90);
                    world.MoveNpc(3672, 5000, 39, 90);
                    world.MoveNpc(3673, 5000, 46, 90);
                    world.MoveNpc(3674, 5000, 47, 90);
                    world.MoveNpc(3675, 5000, 48, 90);
                    world.MoveNpc(3676, 5000, 49, 90);
                    world.MoveNpc(3672, 1002, 372, 407);
                }
            }
            else if (world.Now.IsBetween(new(2007, 8, 13, 0, 0, 0), new(2007, 9, 10, 23, 59, 0)))
            {
                world.MoveNpc(30154, 5000, 38, 90);
                world.MoveNpc(3666, 5000, 39, 90);
                world.MoveNpc(3667, 5000, 39, 90);
                world.MoveNpc(3668, 5000, 39, 90);
                world.MoveNpc(3669, 5000, 39, 90);
                world.MoveNpc(3670, 5000, 39, 90);
                world.MoveNpc(3671, 5000, 39, 90);
                world.MoveNpc(3672, 5000, 39, 90);
                world.MoveNpc(3673, 5000, 46, 90);
                world.MoveNpc(3674, 5000, 47, 90);
                world.MoveNpc(3675, 5000, 48, 90);
                world.MoveNpc(3676, 5000, 49, 90);
                world.MoveNpc(3672, 1002, 372, 407);
            }
            else if (world.Now.IsBetween(new(2007, 9, 11, 0, 0, 0), new(2007, 10, 10, 23, 59, 0)))
            {
                world.MoveNpc(30154, 5000, 38, 90);
                world.MoveNpc(3666, 5000, 39, 90);
                world.MoveNpc(3667, 5000, 39, 90);
                world.MoveNpc(3668, 5000, 39, 90);
                world.MoveNpc(3669, 5000, 39, 90);
                world.MoveNpc(3670, 5000, 39, 90);
                world.MoveNpc(3671, 5000, 39, 90);
                world.MoveNpc(3672, 5000, 39, 90);

                if (world.MoveNpc(3673, 5000, 46, 90))
                {
                    world.MoveNpc(3674, 5000, 47, 90);

                    if (world.MoveNpc(3675, 5000, 48, 90))
                    {
                        world.MoveNpc(3676, 5000, 49, 90);
                        world.MoveNpc(3673, 1002, 372, 407);
                    }
                    else
                    {
                        world.MoveNpc(3666, 5000, 39, 90);
                        world.MoveNpc(3667, 5000, 39, 90);
                        world.MoveNpc(3668, 5000, 39, 90);
                    }
                }
                else if (world.Now.IsBetween(new(2007, 8, 13, 0, 0, 0), new(2007, 9, 10, 23, 59, 0)))
                {
                    world.MoveNpc(30154, 5000, 38, 90);
                    world.MoveNpc(3666, 5000, 39, 90);
                    world.MoveNpc(3667, 5000, 39, 90);
                    world.MoveNpc(3668, 5000, 39, 90);
                }
                else if (world.Now.IsBetween(new(2007, 9, 11, 0, 0, 0), new(2007, 10, 10, 23, 59, 0)))
                {
                    world.MoveNpc(30154, 5000, 38, 90);
                    world.MoveNpc(3666, 5000, 39, 90);
                    world.MoveNpc(3667, 5000, 39, 90);
                }
                else if (world.Now.IsBetween(new(2007, 10, 11, 0, 0, 0), new(2007, 11, 9, 23, 59, 0)))
                {
                    world.MoveNpc(30154, 5000, 38, 90);
                    world.MoveNpc(3666, 5000, 39, 90);
                }
                else if (world.Now.IsBetween(new(2007, 11, 10, 0, 0, 0), new(2007, 12, 9, 23, 59, 0)))
                {
                    world.MoveNpc(30154, 5000, 38, 90);
                }
            }
            else if (world.Now.IsBetween(new(2007, 10, 11, 0, 0, 0), new(2007, 11, 9, 23, 59, 0)))
            {
                world.MoveNpc(30154, 5000, 38, 90);
                world.MoveNpc(3666, 5000, 39, 90);
                world.MoveNpc(3667, 5000, 39, 90);
                world.MoveNpc(3668, 5000, 39, 90);
                world.MoveNpc(3669, 5000, 39, 90);
                world.MoveNpc(3670, 5000, 39, 90);
                world.MoveNpc(3671, 5000, 39, 90);
                world.MoveNpc(3672, 5000, 39, 90);
                world.MoveNpc(3673, 5000, 46, 90);
                world.MoveNpc(3674, 5000, 47, 90);
                world.MoveNpc(3675, 5000, 48, 90);
                world.MoveNpc(3676, 5000, 49, 90);
                world.MoveNpc(3674, 1002, 372, 407);
            }
            else if (world.Now.IsBetween(new(2007, 11, 10, 0, 0, 0), new(2007, 12, 9, 23, 59, 0)))
            {
                world.MoveNpc(30154, 5000, 38, 90);
                world.MoveNpc(3666, 5000, 39, 90);
                world.MoveNpc(3667, 5000, 39, 90);
                world.MoveNpc(3668, 5000, 39, 90);
                world.MoveNpc(3669, 5000, 39, 90);
                world.MoveNpc(3670, 5000, 39, 90);
                world.MoveNpc(3671, 5000, 39, 90);
                world.MoveNpc(3672, 5000, 39, 90);
                world.MoveNpc(3673, 5000, 46, 90);
                world.MoveNpc(3674, 5000, 47, 90);
                world.MoveNpc(3675, 5000, 48, 90);
                world.MoveNpc(3676, 5000, 49, 90);
            }
            else if (world.Now.IsBetween(new(2007, 12, 10, 0, 0, 0), new(2008, 1, 7, 23, 59, 0)))
            {
                world.MoveNpc(30154, 5000, 38, 90);
                world.MoveNpc(3666, 5000, 39, 90);
                world.MoveNpc(3667, 5000, 39, 90);
                world.MoveNpc(3668, 5000, 39, 90);
                world.MoveNpc(3669, 5000, 39, 90);
                world.MoveNpc(3670, 5000, 39, 90);
                world.MoveNpc(3671, 5000, 39, 90);
                world.MoveNpc(3672, 5000, 39, 90);
                world.MoveNpc(3673, 5000, 46, 90);
                world.MoveNpc(3674, 5000, 47, 90);
                world.MoveNpc(3675, 5000, 48, 90);
            }
            else if (world.Now.IsBetween(new(2008, 1, 8, 0, 0, 0), new(2008, 2, 6, 23, 59, 0)))
            {
                world.MoveNpc(30154, 5000, 38, 90);
                world.MoveNpc(3666, 5000, 39, 90);
                world.MoveNpc(3667, 5000, 39, 90);
                world.MoveNpc(3668, 5000, 39, 90);
                world.MoveNpc(3669, 5000, 39, 90);
                world.MoveNpc(3670, 5000, 39, 90);
                world.MoveNpc(3671, 5000, 39, 90);
                world.MoveNpc(3672, 5000, 39, 90);
                world.MoveNpc(3673, 5000, 46, 90);
                world.MoveNpc(3674, 5000, 47, 90);
            }
        }
        else if (world.Now.IsBetween(new(2007, 10, 11, 0, 0, 0), new(2007, 11, 9, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3674, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 11, 10, 0, 0, 0), new(2007, 12, 9, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3675, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2007, 12, 10, 0, 0, 0), new(2008, 1, 7, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3676, 1002, 372, 407);
        }
        else if (world.Now.IsBetween(new(2008, 1, 8, 0, 0, 0), new(2008, 2, 6, 23, 59, 0)))
        {
            world.MoveNpc(30154, 5000, 38, 90);
            world.MoveNpc(3666, 5000, 39, 90);
            world.MoveNpc(3667, 5000, 39, 90);
            world.MoveNpc(3668, 5000, 39, 90);
            world.MoveNpc(3669, 5000, 39, 90);
            world.MoveNpc(3670, 5000, 39, 90);
            world.MoveNpc(3671, 5000, 39, 90);
            world.MoveNpc(3672, 5000, 39, 90);
            world.MoveNpc(3673, 5000, 46, 90);
            world.MoveNpc(3674, 5000, 47, 90);
            world.MoveNpc(3675, 5000, 48, 90);
            world.MoveNpc(3676, 5000, 49, 90);
            world.MoveNpc(3677, 1002, 372, 407);
        }
    }
}
