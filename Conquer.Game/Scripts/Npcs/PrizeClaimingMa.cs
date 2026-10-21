namespace Conquer.Game.Scripts.Npcs;

public static class PrizeClaimingMa
{
    [Npc(30061)]
    public static async Task GuardingKid(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        await dialog.Confirm("You are great to make it so far. Only one of us will send you to claim the prize. Please believe me.",
            "Alright. I believe you.", "Let me think it over.");

        if (player.Roll(2) < 1)
        {
            player.World.MoveNpc(30061, 1041, 512, 322);
        }

        if (player.Teleport(1041, 449, 247, force: true))
        {
            dialog.Say("It will not be easy to claim the prize. The monsters ahead are tougher. Be careful.",
                "Thanks for reminding.");
        }
    }

    [Npc(30062, 30063, 30064)]
    public static async Task GuardingKidExit(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (await dialog.Ask("You have done a good job to make it so far. I will send you out. Wish you good luck next time.",
            "Well. I have no choice.") == 0)
        {
            player.Teleport(1020, 566, 565, force: true);
        }
    }

    [Npc(30077)]
    public static async Task KindTaoist(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        await dialog.Confirm("You are great to make it finally. I will send you a gift. Hope you like it.", "Thanks a lot.");

        if (player.Roll(1000) < 1)
        {
            player.Give(150016, 1,
                item => { item.Durability = 1698; item.MaxDurability = 1698; item.Gem1 = (Gem)255; });

            if (player.Teleport(1020, 566, 565, force: true))
            {
                var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126), (309, 126),
                    (309, 126), (293, 92), (253, 79)]);
                player.World.MoveNpc(30077, 1041, x, y);
            }
        }
        else if (player.Roll(1000) < 1)
        {
            player.Give(ItemType.DragonBall);

            if (player.Teleport(1020, 566, 565, force: true))
            {
                var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126), (309, 126),
                    (309, 126), (293, 92), (253, 79)]);
                player.World.MoveNpc(30077, 1041, x, y);
            }
        }
        else if (player.Roll(100) < 30)
        {
            if (player.Give(ItemType.Meteor) is not null)
            {
                if (player.Give(ItemType.Meteor) is not null)
                {
                    player.Give(ItemType.Meteor);
                }

                if (player.Teleport(1020, 566, 565, force: true))
                {
                    var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                        (309, 126), (309, 126), (293, 92), (253, 79)]);
                    player.World.MoveNpc(30077, 1041, x, y);
                }
            }
            else if (player.Teleport(1020, 566, 565, force: true))
            {
                var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126), (309, 126),
                    (309, 126), (293, 92), (253, 79)]);
                player.World.MoveNpc(30077, 1041, x, y);
            }
        }
        else
        {
            switch (player.Roll(1, 8))
            {
                case 1 or 7:
                    if (player.Earn(1000) && player.Teleport(1020, 566, 565, force: true))
                    {
                        var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                            (309, 126), (309, 126), (293, 92), (253, 79)]);
                        player.World.MoveNpc(30077, 1041, x, y);
                    }
                    break;

                case 2:
                    if (player.Earn(3000) && player.Teleport(1020, 566, 565, force: true))
                    {
                        var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                            (309, 126), (309, 126), (293, 92), (253, 79)]);
                        player.World.MoveNpc(30077, 1041, x, y);
                    }
                    break;

                case 3 or 4:
                    player.Give(121066);

                    if (player.Teleport(1020, 566, 565, force: true))
                    {
                        var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                            (309, 126), (309, 126), (293, 92), (253, 79)]);
                        player.World.MoveNpc(30077, 1041, x, y);
                    }
                    break;

                case 5:
                    if (player.Earn(5000) && player.Teleport(1020, 566, 565, force: true))
                    {
                        var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                            (309, 126), (309, 126), (293, 92), (253, 79)]);
                        player.World.MoveNpc(30077, 1041, x, y);
                    }
                    break;

                case 6:
                    if (player.Earn(8000) && player.Teleport(1020, 566, 565, force: true))
                    {
                        var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                            (309, 126), (309, 126), (293, 92), (253, 79)]);
                        player.World.MoveNpc(30077, 1041, x, y);
                    }
                    break;

                case 8:
                    if (player.Earn(10000) && player.Teleport(1020, 566, 565, force: true))
                    {
                        var (x, y) = player.Pick<(ushort, ushort)>([(301, 151), (301, 151), (266, 126), (266, 126),
                            (309, 126), (309, 126), (293, 92), (253, 79)]);
                        player.World.MoveNpc(30077, 1041, x, y);
                    }
                    break;
            }
        }
    }

    [Npc(30090)]
    public static async Task GuardingKidDecoy(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        await dialog.Confirm("You have great to make it so far. Only one of us will send you to claim the prize. Please believe me.",
            "Alright. I believe you.", "Let me think it over.");

        if (player.Roll(2) >= 1)
        {
            if (player.Teleport(1041, [(323, 334), (343, 361), (365, 307), (387, 364), (399, 337), (405, 324), (382, 273),
                (357, 337)], force: true, record: (1020, 566, 565)))
            {
                dialog.Say("The monsters in the prize claiming maze are very ferocious. Be careful.", "Thanks.");
            }

            return;
        }

        player.World.MoveNpc(30090, 1041, 510, 324);

        if (player.Teleport(1041, [(323, 334), (343, 361), (365, 307), (387, 364), (399, 337), (405, 324), (382, 273), (357, 337)], force: true))
        {
            dialog.Say("Aha, you are cheated. I never teleport you to the prize maze. You are teleported back to another maze. Please try again. Good l",
                "What a bad luck.");
        }
    }
}
