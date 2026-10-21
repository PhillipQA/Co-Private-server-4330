namespace Conquer.Game.Scripts.Npcs;

public static class Maze2
{
    [Npc(30069)]
    public static async Task ProtectingKid(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has("DragonLock"))
        {
            await dialog.Confirm("You don`t take Lock and I will send you back to Maze.", "Alright.");

            dialog.Require(player.Has("DragonLock"));

            if (player.Has(721112))
            {
                if (player.Roll(2) >= 1)
                {
                    player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                        (259, 355)], force: true, record: (1020, 566, 565));
                    return;
                }

                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else if (player.Roll(2) < 1)
            {
                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else
            {
                player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                    (242, 376)], force: true, record: (1020, 566, 565));
            }
        }
        else if (player.Has("PhoenixLock"))
        {
            await dialog.Confirm("You do not have the correct token, I will send you back to the maze.", "Alright.");

            dialog.Require(player.Has("PhoenixLock"));

            if (player.Has(721111))
            {
                if (player.Roll(2) >= 1)
                {
                    player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                        (242, 376)], force: true, record: (1020, 566, 565));
                    return;
                }

                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else if (player.Roll(2) < 1)
            {
                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else
            {
                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
        }
        else if (player.Has("CloudLock"))
        {
            await dialog.Confirm("We are protecting kids. Only one of us will send you to claim the prize. While the other one will send you back to the maze.",
                "?????");

            dialog.Require(player.Has("CloudLock"));

            if (player.Roll(2) < 1)
            {
                player.World.MoveNpc(30069, 1061, 512, 322);
                player.World.MoveNpc(30092, 1061, 510, 324);
            }

            if (!player.Take(721111))
            {
                return;
            }

            if (player.Teleport(1041, [(323, 334), (343, 361), (365, 307), (387, 364), (399, 337), (405, 324), (382, 273),
                (357, 337)], force: true, record: (1020, 566, 565)))
            {
                dialog.Say("The monsters in the prize claiming maze are very ferocious. Be careful.", "Thanks.");
            }
        }
        else
        {
            if (await dialog.Ask("Since you do not bring the maze lock, I have to teleport you out of the maze. I hope you can get the big prize next time.",
                "I hope so.") == 0)
            {
                player.Teleport(1020, 566, 565, force: true);
            }
        }
    }

    [Npc(30092)]
    public static async Task ProtectingKidDecoy(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has("PhoenixLock"))
        {
            await dialog.Confirm("You do not have the correct token, I will send you back to the maze.", "Alright.");

            dialog.Require(player.Has("PhoenixLock"));

            if (player.Has(721111))
            {
                if (player.Roll(2) >= 1)
                {
                    player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                        (242, 376)], force: true, record: (1020, 566, 565));
                    return;
                }

                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else if (player.Roll(2) < 1)
            {
                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else
            {
                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
        }
        else if (player.Has("DragonLock"))
        {
            await dialog.Confirm("You don`t take Lock and I will send you back to Maze.", "Alright.");

            dialog.Require(player.Has("DragonLock"));

            if (player.Has(721112))
            {
                if (player.Roll(2) >= 1)
                {
                    player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                        (259, 355)], force: true, record: (1020, 566, 565));
                    return;
                }

                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else if (player.Roll(2) < 1)
            {
                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else
            {
                player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                    (242, 376)], force: true, record: (1020, 566, 565));
            }
        }
        else if (player.Has("CloudLock"))
        {
            await dialog.Confirm("Well done. Only I can teleport you to claim the prize, that guy standing by me will teleport you back. Do you trust me?",
                "Yes, I do.", "You are playing tricks.");

            dialog.Require(player.Has("CloudLock"));

            if (player.Roll(2) < 1)
            {
                player.World.MoveNpc(30092, 1061, 510, 324);
                player.World.MoveNpc(30069, 1061, 512, 322);
            }

            if (player.Roll(2) >= 1)
            {
                player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                    (242, 376)], force: true, record: (1020, 566, 565));
                return;
            }

            player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                (259, 355)], force: true, record: (1020, 566, 565));
        }
        else
        {
            if (await dialog.Ask("Since you do not bring the maze lock, I have to teleport you out of the maze. I hope you can get the big prize next time.",
                "I hope so.") == 0)
            {
                player.Teleport(1020, 566, 565, force: true);
            }
        }
    }

    [Npc(30124)]
    public static async Task Lauren(Player player, Dialog dialog)
    {
        dialog.Avatar = 154;

        if (!player.Has(721153))
        {
            dialog.Say("I wish I could get out of here soon.", "Good luck.");
            return;
        }

        await dialog.Confirm("Thank goodness. I have not seen a single person for long. Did you come all the way to rescue me?", "Yes, I did.");

        await dialog.Confirm("But WaterRing sticks to me and I can`t take it off.", "I can do it.");

        await dialog.Confirm("Oh, you got the seal. Give it to me please and take away WaterRing at once. I want to be free.", "Ok. One moment.");

        if (player.Take(721153))
        {
            player.Give(721150);
        }
    }

    [Npc(30127)]
    public static async Task CellGuard(Player player, Dialog dialog)
    {
        dialog.Avatar = 53;

        if (await dialog.Ask("Do you want to leave?", "Yes.", "I`d like to stay here.") == 0)
        {
            player.Teleport(1061, 548, 297, force: true);
        }
    }
}
