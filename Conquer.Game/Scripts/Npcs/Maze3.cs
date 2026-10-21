namespace Conquer.Game.Scripts.Npcs;

public static class Maze3
{
    [Npc(30073)]
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
        else if (player.Has("CloudLock"))
        {
            await dialog.Confirm("You don`t take Lock with you and I will send you back to Maze.", "Alright.");

            dialog.Require(player.Has("CloudLock"));

            if (player.Has(721110))
            {
                if (player.Roll(2) >= 1)
                {
                    player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                        (242, 376)], force: true, record: (1020, 566, 565));
                    return;
                }

                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else if (player.Roll(2) < 1)
            {
                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
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
            await dialog.Confirm("We are protecting kids. Only one of us will send you to claim the prize. While the other one will send you back to the maze.",
                "It is ok now.");

            dialog.Require(player.Has("PhoenixLock"));

            if (player.Roll(2) < 1)
            {
                player.World.MoveNpc(30073, 1062, 512, 322);
                player.World.MoveNpc(30093, 1062, 510, 324);
            }

            if (!player.Take(721112))
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

    [Npc(30093)]
    public static async Task ProtectingKidDecoy(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Has("CloudLock"))
        {
            await dialog.Confirm("You don`t take Lock with you and I will send you back to Maze.", "Alright.");

            dialog.Require(player.Has("CloudLock"));

            if (player.Has(721110))
            {
                if (player.Roll(2) >= 1)
                {
                    player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                        (242, 376)], force: true, record: (1020, 566, 565));
                    return;
                }

                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else if (player.Roll(2) < 1)
            {
                player.Teleport(1060, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
            }
            else
            {
                player.Teleport(1062, [(318, 393), (360, 418), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356),
                    (242, 376)], force: true, record: (1020, 566, 565));
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
        else if (player.Has("PhoenixLock"))
        {
            await dialog.Confirm("Well done. Only I can teleport you to claim the prize, that guy standing by me will teleport you back. Do you trust me?",
                "Yes, I do.", "You are playing tricks.");

            dialog.Require(player.Has("PhoenixLock"));

            if (player.Roll(2) < 1)
            {
                player.World.MoveNpc(30093, 1062, 510, 324);
                player.World.MoveNpc(30073, 1062, 512, 322);
            }

            if (player.Roll(2) >= 1)
            {
                player.Teleport(1061, [(318, 393), (360, 418), (310, 381), (311, 432), (283, 342), (283, 356), (242, 376),
                    (259, 355)], force: true, record: (1020, 566, 565));
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
}
