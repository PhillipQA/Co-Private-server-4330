namespace Conquer.Game.Scripts.Npcs;

public static class NPCJail
{
    private static readonly (ushort X, ushort Y)[] ArenaSpots = [(126, 146), (98, 139), (92, 165), (150, 112), (115, 115), (90, 85), (113, 73), (141, 85)];

    private static readonly (TimeOnly First, TimeOnly Last)[] TournamentRounds =
        [(new(21, 10), new(21, 27)), (new(21, 30), new(22, 27)), (new(22, 30), new(23, 59))];

    private static readonly uint[] MoonPlatforms = [1100, 1101, 1102, 1103, 1104, 1105, 1106, 1107, 1108, 1109];

    private static readonly Dictionary<uint, (DateOnly First, DateOnly Last)[]> MoonPlatformNights = new()
    {
        [3640] =
        [
            (new(2006, 3, 29), new(2006, 3, 31)), (new(2006, 4, 1), new(2006, 4, 3)), (new(2006, 4, 4), new(2006, 4, 6)),
            (new(2006, 4, 7), new(2006, 4, 9)), (new(2006, 4, 10), new(2006, 4, 12)), (new(2006, 4, 13), new(2006, 4, 15)),
            (new(2006, 4, 16), new(2006, 4, 18)), (new(2006, 4, 19), new(2006, 4, 21)), (new(2006, 4, 22), new(2006, 4, 24)),
            (new(2006, 4, 25), new(2006, 4, 27))
        ],
        [3641] =
        [
            (new(2006, 4, 28), new(2006, 4, 30)), (new(2006, 5, 1), new(2006, 5, 3)), (new(2006, 5, 4), new(2006, 5, 6)),
            (new(2006, 5, 7), new(2006, 5, 9)), (new(2006, 5, 10), new(2006, 5, 12)), (new(2006, 5, 13), new(2006, 5, 15)),
            (new(2006, 5, 16), new(2006, 5, 18)), (new(2006, 5, 19), new(2006, 5, 21)), (new(2006, 5, 22), new(2006, 5, 24)),
            (new(2006, 5, 25), new(2006, 5, 26))
        ],
        [3642] =
        [
            (new(2006, 5, 27), new(2006, 5, 29)), (new(2006, 5, 30), new(2006, 6, 1)), (new(2006, 6, 2), new(2006, 6, 4)),
            (new(2006, 6, 5), new(2006, 6, 7)), (new(2006, 6, 8), new(2006, 6, 10)), (new(2006, 6, 11), new(2006, 6, 13)),
            (new(2006, 6, 14), new(2006, 6, 16)), (new(2006, 6, 17), new(2006, 6, 19)), (new(2006, 6, 20), new(2006, 6, 22)),
            (new(2006, 6, 23), new(2006, 6, 25))
        ],
        [3643] =
        [
            (new(2006, 6, 26), new(2006, 6, 28)), (new(2006, 6, 29), new(2006, 7, 1)), (new(2006, 7, 2), new(2006, 7, 4)),
            (new(2006, 7, 5), new(2006, 7, 7)), (new(2006, 7, 8), new(2006, 7, 10)), (new(2006, 7, 11), new(2006, 7, 13)),
            (new(2006, 7, 14), new(2006, 7, 16)), (new(2006, 7, 17), new(2006, 7, 19)), (new(2006, 7, 20), new(2006, 7, 22)),
            (new(2006, 7, 23), new(2006, 7, 24))
        ],
        [3644] =
        [
            (new(2006, 7, 25), new(2006, 7, 27)), (new(2006, 7, 28), new(2006, 7, 30)), (new(2006, 7, 31), new(2006, 8, 2)),
            (new(2006, 8, 3), new(2006, 8, 5)), (new(2006, 8, 6), new(2006, 8, 8)), (new(2006, 8, 9), new(2006, 8, 11)),
            (new(2006, 8, 12), new(2006, 8, 14)), (new(2006, 8, 15), new(2006, 8, 17)), (new(2006, 8, 18), new(2006, 8, 20)),
            (new(2006, 8, 21), new(2006, 8, 23))
        ],
        [3645] =
        [
            (new(2006, 8, 24), new(2006, 8, 26)), (new(2006, 8, 27), new(2006, 8, 29)), (new(2006, 8, 30), new(2006, 9, 1)),
            (new(2006, 9, 2), new(2006, 9, 4)), (new(2006, 9, 5), new(2006, 9, 7)), (new(2006, 9, 8), new(2006, 9, 10)),
            (new(2006, 9, 11), new(2006, 9, 13)), (new(2006, 9, 14), new(2006, 9, 16)), (new(2006, 9, 17), new(2006, 9, 19)),
            (new(2006, 9, 20), new(2006, 9, 21))
        ],
        [3646] =
        [
            (new(2006, 9, 22), new(2006, 9, 24)), (new(2006, 9, 25), new(2006, 9, 27)), (new(2006, 9, 28), new(2006, 9, 30)),
            (new(2006, 10, 1), new(2006, 10, 3)), (new(2006, 10, 4), new(2006, 10, 6)), (new(2006, 10, 7), new(2006, 10, 9)),
            (new(2006, 10, 10), new(2006, 10, 12)), (new(2006, 10, 13), new(2006, 10, 15)), (new(2006, 10, 16), new(2006, 10, 18)),
            (new(2006, 10, 19), new(2006, 10, 21))
        ],
        [3647] =
        [
            (new(2006, 10, 22), new(2006, 10, 24)), (new(2006, 10, 25), new(2006, 10, 27)), (new(2006, 10, 28), new(2006, 10, 30)),
            (new(2006, 10, 31), new(2006, 11, 2)), (new(2006, 11, 3), new(2006, 11, 5)), (new(2006, 11, 6), new(2006, 11, 8)),
            (new(2006, 11, 9), new(2006, 11, 11)), (new(2006, 11, 12), new(2006, 11, 14)), (new(2006, 11, 15), new(2006, 11, 16)),
            (new(2006, 11, 18), new(2006, 11, 20))
        ],
        [3648] =
        [
            (new(2006, 11, 21), new(2006, 11, 23)), (new(2006, 11, 24), new(2006, 11, 26)), (new(2006, 11, 27), new(2006, 11, 29)),
            (new(2006, 11, 30), new(2006, 12, 2)), (new(2006, 12, 3), new(2006, 12, 5)), (new(2006, 12, 6), new(2006, 12, 8)),
            (new(2006, 12, 9), new(2006, 12, 11)), (new(2006, 12, 12), new(2006, 12, 14)), (new(2006, 12, 15), new(2006, 12, 17)),
            (new(2006, 12, 18), new(2006, 12, 19))
        ],
        [3649] =
        [
            (new(2006, 12, 20), new(2006, 12, 22)), (new(2006, 12, 23), new(2006, 12, 25)), (new(2006, 12, 26), new(2006, 12, 28)),
            (new(2006, 12, 29), new(2006, 12, 31)), (new(2007, 1, 1), new(2007, 1, 3)), (new(2007, 1, 4), new(2007, 1, 6)),
            (new(2007, 1, 7), new(2007, 1, 9)), (new(2007, 1, 10), new(2007, 1, 12)), (new(2007, 1, 13), new(2007, 1, 15)),
            (new(2007, 1, 16), new(2007, 1, 18))
        ],
        [3650] =
        [
            (new(2007, 1, 19), new(2007, 1, 21)), (new(2007, 1, 22), new(2007, 1, 24)), (new(2007, 1, 25), new(2007, 1, 27)),
            (new(2007, 1, 28), new(2007, 1, 30)), (new(2007, 1, 31), new(2007, 2, 2)), (new(2007, 2, 3), new(2007, 2, 5)),
            (new(2007, 2, 6), new(2007, 2, 8)), (new(2007, 2, 9), new(2007, 2, 11)), (new(2007, 2, 12), new(2007, 2, 14)),
            (new(2007, 2, 15), new(2007, 2, 17))
        ],
        [3666] =
        [
            (new(2007, 2, 18), new(2007, 2, 20)), (new(2007, 2, 21), new(2007, 2, 23)), (new(2007, 2, 24), new(2007, 2, 26)),
            (new(2007, 2, 27), new(2007, 3, 1)), (new(2007, 3, 2), new(2007, 3, 4)), (new(2007, 3, 5), new(2007, 3, 7)),
            (new(2007, 3, 8), new(2007, 3, 10)), (new(2007, 3, 11), new(2007, 3, 13)), (new(2007, 3, 14), new(2007, 3, 16)),
            (new(2007, 3, 17), new(2007, 3, 18))
        ],
        [3667] =
        [
            (new(2007, 3, 19), new(2007, 3, 21)), (new(2007, 3, 22), new(2007, 3, 24)), (new(2007, 3, 25), new(2007, 3, 27)),
            (new(2007, 3, 28), new(2007, 3, 30)), (new(2007, 3, 31), new(2007, 4, 2)), (new(2007, 4, 3), new(2007, 4, 5)),
            (new(2007, 4, 6), new(2007, 4, 8)), (new(2007, 4, 9), new(2007, 4, 11)), (new(2007, 4, 12), new(2007, 4, 14)),
            (new(2007, 4, 15), new(2007, 4, 16))
        ],
        [3668] =
        [
            (new(2007, 4, 17), new(2007, 4, 19)), (new(2007, 4, 20), new(2007, 4, 22)), (new(2007, 4, 23), new(2007, 4, 25)),
            (new(2007, 4, 26), new(2007, 4, 28)), (new(2007, 4, 29), new(2007, 5, 1)), (new(2007, 5, 2), new(2007, 5, 4)),
            (new(2007, 5, 5), new(2007, 5, 7)), (new(2007, 5, 8), new(2007, 5, 10)), (new(2007, 5, 11), new(2007, 5, 13)),
            (new(2007, 5, 14), new(2007, 5, 16))
        ],
        [3669] =
        [
            (new(2007, 5, 17), new(2007, 5, 19)), (new(2007, 5, 20), new(2007, 5, 22)), (new(2007, 5, 23), new(2007, 5, 25)),
            (new(2007, 5, 26), new(2007, 5, 28)), (new(2007, 5, 29), new(2007, 5, 31)), (new(2007, 6, 1), new(2007, 6, 3)),
            (new(2007, 6, 4), new(2007, 6, 6)), (new(2007, 6, 7), new(2007, 6, 9)), (new(2007, 6, 10), new(2007, 6, 12)),
            (new(2007, 6, 13), new(2007, 6, 14))
        ],
        [3670] =
        [
            (new(2007, 6, 15), new(2007, 6, 17)), (new(2007, 6, 18), new(2007, 6, 20)), (new(2007, 6, 21), new(2007, 6, 23)),
            (new(2007, 6, 24), new(2007, 6, 26)), (new(2007, 6, 27), new(2007, 6, 29)), (new(2007, 6, 30), new(2007, 7, 2)),
            (new(2007, 7, 3), new(2007, 7, 5)), (new(2007, 7, 6), new(2007, 7, 8)), (new(2007, 7, 9), new(2007, 7, 11)),
            (new(2007, 7, 12), new(2007, 7, 13))
        ],
        [3671] =
        [
            (new(2007, 7, 14), new(2007, 7, 16)), (new(2007, 7, 17), new(2007, 7, 19)), (new(2007, 7, 20), new(2007, 7, 22)),
            (new(2007, 7, 23), new(2007, 7, 25)), (new(2007, 7, 26), new(2007, 7, 28)), (new(2007, 7, 29), new(2007, 7, 31)),
            (new(2007, 8, 1), new(2007, 8, 3)), (new(2007, 8, 4), new(2007, 8, 6)), (new(2007, 8, 7), new(2007, 8, 9)),
            (new(2007, 8, 10), new(2007, 8, 12))
        ],
        [3672] =
        [
            (new(2007, 8, 13), new(2007, 8, 15)), (new(2007, 8, 16), new(2007, 8, 18)), (new(2007, 8, 19), new(2007, 8, 21)),
            (new(2007, 8, 22), new(2007, 8, 24)), (new(2007, 8, 25), new(2007, 8, 27)), (new(2007, 8, 28), new(2007, 8, 30)),
            (new(2007, 8, 31), new(2007, 9, 2)), (new(2007, 9, 3), new(2007, 9, 5)), (new(2007, 9, 6), new(2007, 9, 8)),
            (new(2007, 9, 9), new(2007, 9, 10))
        ],
        [3673] =
        [
            (new(2007, 9, 11), new(2007, 9, 13)), (new(2007, 9, 14), new(2007, 9, 16)), (new(2007, 9, 17), new(2007, 9, 19)),
            (new(2007, 9, 20), new(2007, 9, 22)), (new(2007, 9, 23), new(2007, 9, 25)), (new(2007, 9, 26), new(2007, 9, 28)),
            (new(2007, 9, 29), new(2007, 10, 1)), (new(2007, 10, 2), new(2007, 10, 4)), (new(2007, 10, 5), new(2007, 10, 7)),
            (new(2007, 10, 8), new(2007, 10, 10))
        ],
        [3674] =
        [
            (new(2007, 10, 11), new(2007, 10, 13)), (new(2007, 10, 14), new(2007, 10, 16)), (new(2007, 10, 17), new(2007, 10, 19)),
            (new(2007, 10, 20), new(2007, 10, 22)), (new(2007, 10, 23), new(2007, 10, 25)), (new(2007, 10, 26), new(2007, 10, 28)),
            (new(2007, 10, 29), new(2007, 10, 31)), (new(2007, 11, 1), new(2007, 11, 3)), (new(2007, 11, 4), new(2007, 11, 6)),
            (new(2007, 11, 7), new(2007, 11, 9))
        ],
        [3675] =
        [
            (new(2007, 11, 10), new(2007, 11, 12)), (new(2007, 11, 13), new(2007, 11, 15)), (new(2007, 11, 16), new(2007, 11, 18)),
            (new(2007, 11, 19), new(2007, 11, 21)), (new(2007, 11, 22), new(2007, 11, 24)), (new(2007, 11, 25), new(2007, 11, 27)),
            (new(2007, 11, 28), new(2007, 11, 30)), (new(2007, 12, 1), new(2007, 12, 3)), (new(2007, 12, 4), new(2007, 12, 6)),
            (new(2007, 12, 7), new(2007, 12, 9))
        ],
        [3676] =
        [
            (new(2007, 12, 10), new(2007, 12, 12)), (new(2007, 12, 13), new(2007, 12, 15)), (new(2007, 12, 16), new(2007, 12, 18)),
            (new(2007, 12, 19), new(2007, 12, 21)), (new(2007, 12, 22), new(2007, 12, 24)), (new(2007, 12, 25), new(2007, 12, 27)),
            (new(2007, 12, 28), new(2007, 12, 30)), (new(2007, 12, 31), new(2008, 1, 2)), (new(2008, 1, 3), new(2008, 1, 5)),
            (new(2008, 1, 6), new(2008, 1, 7))
        ],
        [3677] =
        [
            (new(2008, 1, 8), new(2008, 1, 10)), (new(2008, 1, 11), new(2008, 1, 13)), (new(2008, 1, 14), new(2008, 1, 16)),
            (new(2008, 1, 17), new(2008, 1, 19)), (new(2008, 1, 20), new(2008, 1, 22)), (new(2008, 1, 23), new(2008, 1, 25)),
            (new(2008, 1, 26), new(2008, 1, 28)), (new(2008, 1, 29), new(2008, 1, 31)), (new(2008, 2, 1), new(2008, 2, 3)),
            (new(2008, 2, 4), new(2008, 2, 6))
        ]
    };

    [Npc(31)]
    public static async Task GeneralBravery(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (!player.Has("PKTicket"))
        {
            if (await dialog.Ask("PK Tournament is held on every Saturday and the first day of every month. The winner will get a big prize. Do you want to try?",
                "Yeah.", "Just passing by.") == 0)
            {
                dialog.Say("The admission ticket is required for the PK Tournament. You may get some tickets from the pheasants outside of this city.",
                    "Ok, I am gonna get some.");
            }

            return;
        }

        await dialog.Confirm("You have the admission ticket. Are you here for the PK Tournament?", "Yes. Let me in.", "Just passing by.");

        if (player.Level < 61)
        {
            if (player.Teleport(1508, ArenaSpots))
            {
                player.Take(710001);
                return;
            }
        }
        else if (player.Level < 91)
        {
            if (player.Teleport(1518, ArenaSpots))
            {
                player.Take(710001);
                return;
            }
        }

        if (player.Teleport(1528, ArenaSpots))
        {
            player.Take(710001);
        }
    }

    [Npc(32)]
    public static async Task GeneralWinnerLevel91Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (Tournament(player.World.Now, 1528, 1094, 1095) is not (var arena, var monthly))
        {
            return;
        }

        if (player.World.CountAlivePlayers(arena) != 1)
        {
            if (await dialog.Ask("If you defeat all the challengers in the arena, you can come to claim your prize within the tournament period.",
                "Oh, I see.", "Please send me out.") == 1)
            {
                await LeaveArena(player, dialog);
            }

            return;
        }

        var prize = monthly
            ? await dialog.Ask("Congratulations! You are the winner of the PK War. You will be awarded 2 million or a super gem. What do you prefer?",
                "2000000 silver.", "Super Phoenix Gem.", "Super Dragon Gem.")
            : await dialog.Ask("Congrats. You are the champion of the PK tournament. What would you like to claim, 500,000 silver or a Dragon Ball?",
                "500,000 silver.", "A Dragon Ball.");

        if (ClaimPrize(player, monthly, prize))
        {
            player.World.MoveNpc(32, 5000, 32, 78);
        }
    }

    [Npc(145)]
    public static async Task GeneralWinnerLevelUpTo60(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (Tournament(player.World.Now, 1508, 1090, 1091) is not (var arena, var monthly))
        {
            return;
        }

        if (player.World.CountAlivePlayers(arena) != 1)
        {
            if (await dialog.Ask("When you defeat all the challengers in the arena, you can come to claim your prize within the tournament period.",
                "Oh, I see.", "Please send me out.") == 1)
            {
                await LeaveArena(player, dialog);
            }

            return;
        }

        var prize = monthly
            ? await dialog.Ask("Congratulations! You are the winner of the PK War. You will be awarded 2 million or a super gem. What do you prefer?",
                "2,000,000 silver", "Super Phoenix Gem", "Super Dragon Gem")
            : await dialog.Ask("Congrats! You are the champion of the PK tournament today. What prize would you like, 500,000 silver or a dragon ball?",
                "500,000 silver.", "A Dragon Ball.");

        if (ClaimPrize(player, monthly, prize))
        {
            player.World.MoveNpc(145, 5000, 37, 78);
        }
    }

    [Npc(146)]
    public static async Task GeneralWinnerLevel61To90(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (Tournament(player.World.Now, 1518, 1092, 1093) is not (var arena, var monthly))
        {
            return;
        }

        if (player.World.CountAlivePlayers(arena) != 1)
        {
            if (await dialog.Ask("When you defeat all the challengers in the arena, you can come to claim your prize within the tournament period.",
                "Oh, I see.", "Please send me out.") == 1)
            {
                await LeaveArena(player, dialog);
            }

            return;
        }

        var prize = monthly
            ? await dialog.Ask("Congratulations! You are the winner of the PK War. You will be awarded 2 million or a super gem. What do you prefer?",
                "2,000,000 silver.", "Super Phoenix Gem", "Super Dragon Gem")
            : await dialog.Ask("Congrats! You are the champion of the PK tournament today. What prize would you like, 500,000 silver or a dragon ball?",
                "500,000 silver", "A Dragon Ball");

        if (ClaimPrize(player, monthly, prize))
        {
            player.World.MoveNpc(146, 5000, 35, 78);
        }
    }

    private static (uint Arena, bool Monthly)? Tournament(DateTime now, params ReadOnlySpan<uint> arenas)
    {
        for (var round = 0; round < TournamentRounds.Length; round++)
        {
            var (first, last) = TournamentRounds[round];

            if (now.IsMonthly(1, first, 1, last))
            {
                return (arenas[round], true);
            }

            if (now.IsWeekly(DayOfWeek.Saturday, first, DayOfWeek.Saturday, last))
            {
                return (arenas[round], false);
            }
        }

        return null;
    }

    private static async Task LeaveArena(Player player, Dialog dialog)
    {
        if (await dialog.Ask("Are you sure to quit the PK war?", "Yes.") == 0)
        {
            player.Teleport(1002, 430, 380);
        }
    }

    private static bool ClaimPrize(Player player, bool monthly, int prize)
    {
        var claimed = (monthly, prize) switch
        {
            (true, 0) => player.Earn(2000000),
            (true, 1) => player.Give(700003) is not null,
            (true, 2) => player.Give(700013) is not null,
            (false, 0) => player.Earn(500000),
            (false, 1) => player.Give(ItemType.DragonBall) is not null,
            _ => false
        };

        if (claimed)
        {
            player.World.Announce(1002, $"Congratulations! {player.Name} wins this {(monthly ? "month" : "week")}`s PK Tournament.",
                TalkChannel.System);
            player.Teleport(1002, 430, 380);
        }

        return claimed;
    }

    [Npc(3640, 3641, 3642, 3643, 3644, 3645, 3646, 3647, 3648, 3649, 3650, 3666, 3667, 3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677)]
    public static async Task Luna(Player player, Entity npc, Dialog dialog)
    {
        dialog.Avatar = 116;

        var now = player.World.Now;
        var platform = Array.FindIndex(MoonPlatformNights[npc.Id],
            nights => now.IsBetween(nights.First.ToDateTime(TimeOnly.MinValue), nights.Last.ToDateTime(new(23, 59))));

        if (platform < 0)
        {
            return;
        }

        await dialog.Confirm("What can I do for you?", "Enter Moon Platform.", "Just passing by.");

        await dialog.Confirm("You may enjoy the bright moon and twinkle stars on Moon Platform. I can teleport you there for a fee of 300 silver.",
            "Teleport me there.", "I changed my mind.");

        if (player.Spend(300))
        {
            player.Teleport(MoonPlatforms[platform], 187, 162, record: (1002, 439, 389));
        }
        else
        {
            dialog.Say("Sorry, you do not have 300 silver.", "I see.");
        }
    }

    [Npc(7212)]
    public static async Task RedPacketSeller(Player player, Dialog dialog)
    {
        dialog.Avatar = 175;

        switch (await dialog.Ask("Happy Spring Festival! Send a Red Packet to your friends and wish them good fortune in the new year.\nWhich Red Packet would you like?",
            "A 5000-silver Red Packet.", "A 10000-silver Red Packet.", "A 50000-silver Red Packet.",
            "A 100000-silver Red Packet.", "A 1000000-silver Red Packet.", "A Meteor Red Packet.",
            "A Dragon Ball Red Packet.", "Just passing by."))
        {
            case 0:
                if (player.Money < 5000)
                {
                    dialog.Say("Sorry, you do not have 5000 silvers.", "I see.");
                }
                else if (player.Give(721505) is not null)
                {
                    player.Spend(5000);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                break;

            case 1:
                if (player.Money < 10000)
                {
                    dialog.Say("Sorry, you do not have 10000 silvers.", "I see.");
                }
                else if (player.Give(721506) is not null)
                {
                    player.Spend(10000);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                break;

            case 2:
                if (player.Money < 50000)
                {
                    dialog.Say("Sorry, you do not have 50000 silvers.", "I see.");
                }
                else if (player.Give(721507) is not null)
                {
                    player.Spend(50000);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                break;

            case 3:
                if (player.Money < 100000)
                {
                    dialog.Say("Sorry, you do not have 100000 silvers.", "I see.");
                }
                else if (player.Give(721508) is not null)
                {
                    player.Spend(100000);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                break;

            case 4:
                if (player.Money < 1000000)
                {
                    dialog.Say("Sorry, you do not have 1000000 silvers.", "I see.");
                }
                else if (player.Give(721509) is not null)
                {
                    player.Spend(1000000);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                break;

            case 5:
                if (player.Take(ItemType.Meteor))
                {
                    player.Give(721510);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, the Meteor Red Packet costs a Meteor.", "I see.");
                }
                break;

            case 6:
                if (player.Take(ItemType.DragonBall))
                {
                    player.Give(721511);

                    dialog.Say("Here is your Red Packet. Give it to a friend and wish them all the best in the new year!",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, the Dragon Ball Red Packet costs a Dragon Ball.", "I see.");
                }
                break;
        }
    }

    [Npc(10061)]
    public static async Task CaptainFang(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        await dialog.Confirm("The kindhearted people and the evil people coexist in the world. It is our responsibility to punish those evil people.\n I have set up a board to recruit the talented kindhearted people. Please join us if you have the ambition to kill the \n evil and uphold the justice. If you hunt down the wanted people, they will be put into the jail. You will have the\n opportunity to become one of our members if you arrest many of them.",
            "Check the wanted list.", "Just passing by.");

        var cursor = 0u;
        var answer = await dialog.Ask($"Board   Wanted Target    Guild   Level   PK points\n------------------------------\n{player.ListFugitives(ref cursor, 5)}\n------------------------------",
            "I accept board number:", 7, "Next", "Close");

        switch (answer.Option)
        {
            case 0:
                await Next();
                break;

            case 2:
                await AcceptBoard(answer.Input);
                break;
        }

        async Task Next()
        {
            var answer2 = await dialog.Ask($"Board   Wanted Target    Guild   Level   PK points\n------------------------------\n{player.ListFugitives(ref cursor, 5)}\n------------------------------",
                "I accept board number:", 7, "Next", "Close");

            switch (answer2.Option)
            {
                case 0:
                    await Next();
                    break;

                case 2:
                    await AcceptBoard(answer2.Input);
                    break;
            }
        }

        async Task AcceptBoard(string input0)
        {
            if (player.PkPoints >= 1)
            {
                dialog.Say("Well", "I see.");
                return;
            }

            if (player.PursueFugitive(input0))
            {
                dialog.Say("If you accept two or more offers, you can take only the last. Your offer will be canceled if you log out before it is finished.",
                    "I see.");
            }
            else
            {
                dialog.Say("Sorry, you have entered the wrong board number.", "I see.");
            }
        }
    }

    [Npc(20060)]
    public static async Task ArenaGuardLevelUpTo60(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.World.Now.IsDaily(new(21, 27), new(21, 30)))
        {
            if (await dialog.Ask("Are you heading for the next PK map?", "Yes.", "Wait a minute.") == 0)
            {
                player.Teleport(1090, [(101, 75), (80, 63), (54, 53), (101, 75), (104, 113), (80, 63), (54, 53), (104, 113)]);
            }
        }
        else if (player.World.Now.IsDaily(new(22, 27), new(22, 30)))
        {
            if (await dialog.Ask("Are you heading for the next PK map?", "Yes.", "Wait a minute.") == 0)
            {
                player.Teleport(1091, [(31, 30), (31, 30), (32, 23), (32, 23), (31, 30), (31, 30), (32, 23), (32, 23)]);
            }
        }
    }

    [Npc(20090)]
    public static async Task ArenaGuardLevel61To90(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.World.Now.IsDaily(new(21, 27), new(21, 30)))
        {
            if (await dialog.Ask("Are you heading for the next PK map?", "Yes.", "Wait a minute.") == 0)
            {
                player.Teleport(1092, [(101, 75), (80, 63), (54, 53), (101, 75), (104, 113), (80, 63), (54, 53), (104, 113)]);
            }
        }
        else if (player.World.Now.IsDaily(new(22, 27), new(22, 30)))
        {
            if (await dialog.Ask("Are you heading for the next PK map?", "Yes.", "Wait a minute.") == 0)
            {
                player.Teleport(1093, [(31, 30), (31, 30), (32, 23), (32, 23), (31, 30), (31, 30), (32, 23), (32, 23)]);
            }
        }
    }

    [Npc(20120)]
    public static async Task ArenaGuardLevel91Plus(Player player, Dialog dialog)
    {
        dialog.Avatar = 7;

        if (player.World.Now.IsDaily(new(21, 27), new(21, 30)))
        {
            if (await dialog.Ask("Are you heading for the next PK map?", "Yes.", "Wait a minute.") == 0)
            {
                player.Teleport(1094, [(101, 75), (80, 63), (54, 53), (101, 75), (104, 113), (80, 63), (54, 53), (104, 113)]);
            }
        }
        else if (player.World.Now.IsDaily(new(22, 27), new(22, 30)))
        {
            if (await dialog.Ask("Are you heading for the next PK map?", "Yes.", "Wait a minute.") == 0)
            {
                player.Teleport(1095, [(31, 30), (31, 30), (32, 23), (32, 23), (31, 30), (31, 30), (32, 23), (32, 23)]);
            }
        }
    }

    [Npc(30087)]
    public static async Task Druggist(Player player, Dialog dialog)
    {
        dialog.Avatar = 2;

        if (player.Has("Bandage"))
        {
            dialog.Say("You have got Bandage, please go away and don`t bother me.", "Alright.");
        }
        else if (player.Has("ShopList"))
        {
            await dialog.Confirm("Three Ginsengs are needed to refine Invigorant.", "Here you are.", "I will get it ready.");

            dialog.Require(player.Has("ShopList"));

            if (player.Has(1002010, 3))
            {
                if (player.Take(721118) && player.Take(1002010, 3))
                {
                    player.Give(721119);
                }
            }
            else
            {
                dialog.Say("You don`t have enough Ginsengs. Please buy more.", "You are so troublesome.");
            }
        }
        else
        {
            dialog.Say("I should concentrate on refining drugs. Please leave me alone if you do not have anything important to do here.",
                "Alright.");
        }
    }

    [Npc(30122)]
    public static async Task Tientsin(Player player, Dialog dialog)
    {
        dialog.Avatar = 61;

        if (player.Has(721156))
        {
            dialog.Say("JailGuard is nearby. Try your luck to find it out.", "Thanks. I will.");
        }
        else if (player.Has(721155))
        {
            await dialog.Confirm("You did pass the 2nd floor. I will keep my words. Take the passport and find Jail Guard to get into Violet Jail.",
                "Thanks.");

            if (player.Take(721155))
            {
                player.Give(721156);
            }
        }
        else if (player.Has(721154))
        {
            dialog.Say("You haven`t passed the second floor of Skypass. So you can`t enter the Violet Jail.",
                "I will get the passport soon.");
        }
        else if (player.Has(721153))
        {
            await dialog.Confirm("Hey. Are you sent by DevineArtisan and Hades to get WaterRing? They have tried many times in vain.",
                "I might have a try as well.", "Alright, I give up.");

            if (await dialog.Ask("Good. If you want to get the Passport from me, pass the sceond floor of SkyPass first!",
                "No problem.", "Ok. Wait a moment.") == 0)
            {
                player.Give(721154);
            }
        }
        else
        {
            dialog.Say("What brings you here?", "Just pass by.");
        }
    }

    [Npc(600067)]
    public static async Task Emily(Player player, Dialog dialog)
    {
        dialog.Avatar = 134;

        if (player.IsFemale)
        {
            switch (await dialog.Ask("It is dangerous that you go by yourself. Are you lonely? Let us have a chat, and then you will not feel lonely.",
                "That is great.", "I doubt it."))
            {
                case 0:
                    await AgreeToEscort();
                    break;

                case 1:
                    await DoubtHer();
                    break;
            }

            return;
        }

        switch (await dialog.Ask("I am on my way home alone. But the path is steep and dangerous animals are lurking ahead. Could you please escort me home?",
            "No problom.", "It is doubtful that a young lady walks alone at midnight."))
        {
            case 0:
                await AgreeToEscort();
                break;

            case 1:
                await DoubtHer();
                break;
        }

        async Task AgreeToEscort()
        {
            player.AddHealth(-100);
            player.AddHealth(1);

            dialog.Say("LOL! You fell for my small tricks easily.", "Help me.");
        }

        async Task DoubtHer()
        {
            if (player.Level < 60)
            {
                dialog.Say("Do not bother me, newbie, or I will take you down.", "It is ok.");
            }
            else if (player.Has(723010))
            {
                if (await dialog.Ask("Oh, you are sent by Old General. You are too weak to oppose us.\nSince you select to enter the cave, I will sent you there. Let`s see how you handle yourself against my boss. Do you have the g",
                    "I would like to have a try.", "No. I will give up.") == 0)
                {
                    await EnterSnakeDen();
                }
            }
            else
            {
                await dialog.Confirm("Yeah. You are right. I advise you mind your own business.", "Everybody can say so.", "Ok. I got to go.");

                if (await dialog.Ask("You do get some guts. I will send you to my boss`s. Hope you won`t regret on what you said.",
                    "I would like to have a try.", "No. I will give up.") == 0)
                {
                    await EnterSnakeDen();
                }
            }
        }

        async Task EnterSnakeDen()
        {
            player.Teleport(1070, 194, 185);
        }
    }
}
