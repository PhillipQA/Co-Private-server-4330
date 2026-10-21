namespace Conquer.Game.Scripts.Npcs;

public static class IronMine
{
    [Npc(1613)]
    public static void BlueMouse(Player player, Dialog dialog)
    {
        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(2006, 98, 143), (2002, 75, 100), (2004, 120, 149), (2011, 66, 75), (2009, 120, 149),
            (2007, 81, 81), (2005, 119, 166), (2000, 71, 100)]);

        player.World.MoveNpc(1613, mapId, x, y);
        Catch(player, dialog);
    }

    [Npc(1612)]
    public static void BlueMouseOnFloor3B(Player player, Dialog dialog)
    {
        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(2000, 66, 75), (2001, 66, 75), (2002, 98, 103), (2004, 98, 103), (2005, 66, 75),
            (2006, 120, 149), (2007, 66, 75), (2011, 83, 132)]);

        player.World.MoveNpc(1612, mapId, x, y);
        Catch(player, dialog);
    }

    [Npc(1617)]
    public static void BlueMouseOnFloor3C(Player player, Dialog dialog)
    {
        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(2000, 98, 143), (2001, 75, 100), (2002, 111, 134), (2006, 66, 75), (2005, 98, 103),
            (2007, 120, 149), (2009, 111, 134), (2011, 98, 103)]);

        player.World.MoveNpc(1617, mapId, x, y);
        Catch(player, dialog);
    }

    [Npc(1614)]
    public static void BlueMouseOnFloor4A(Player player, Dialog dialog)
    {
        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(2000, 81, 81), (2001, 98, 143), (2002, 83, 132), (2004, 111, 134), (2005, 83, 132),
            (2007, 98, 103), (2009, 83, 132), (2011, 111, 134)]);

        player.World.MoveNpc(1614, mapId, x, y);
        Catch(player, dialog);
    }

    [Npc(1615)]
    public static void BlueMouseOnFloor4F(Player player, Dialog dialog)
    {
        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(2000, 111, 134), (2001, 83, 132), (2002, 120, 149), (2004, 75, 100),
            (2005, 111, 134), (2006, 75, 100), (2009, 71, 100), (2007, 98, 143)]);

        player.World.MoveNpc(1615, mapId, x, y);
        Catch(player, dialog);
    }

    [Npc(1616)]
    public static void BlueMouseOnFloor4D(Player player, Dialog dialog)
    {
        var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(2000, 120, 149), (2001, 119, 166), (2002, 119, 166), (2004, 66, 75),
            (2005, 71, 100), (2006, 83, 132), (2007, 111, 134), (2011, 119, 166)]);

        player.World.MoveNpc(1616, mapId, x, y);
        Catch(player, dialog);
    }

    private static void Catch(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.Has(722511))
        {
            if (player.Roll(10) < 7)
            {
                dialog.Say("Oh, my god! Narrowly escaped! Bye, guy!", "Damn!");
            }
            else if (player.FreeSlots >= 2)
            {
                player.Take(722511);

                switch (player.Roll(1, 8))
                {
                    case <= 5:
                        if (player.Give(722514) is not null)
                        {
                            player.Inform("Congratulations! You got RoyalSword");

                            dialog.Say("Let me go! I will give you something.", "Go away.");
                        }
                        break;

                    case 6 or 7:
                        if (player.Give(722514) is not null && player.Give(722513) is not null)
                        {
                            player.Inform("Congratulations! You got RoyalSword and PinetumPicture");

                            dialog.Say("Let me go! I will give you something.", "Go away.");
                        }
                        break;

                    case 8:
                        switch (player.Roll(1, 8))
                        {
                            case <= 7:
                                if (player.Give(722514) is not null && player.Give(722513) is not null &&
                                    player.Give(722512) is not null)
                                {
                                    player.Inform("Congratulations! You got RoyalSword, PinetumPicture and AsterNecklace");

                                    dialog.Say("Let me go! I will give you something.", "Go away.");
                                }
                                break;

                            case 8:
                                if (player.Give(722515) is not null)
                                {
                                    player.Inform("Congratulations! You got SecretCommand");

                                    dialog.Say("Let me go! I will give you something.", "Go away.");
                                }
                                break;
                        }
                        break;
                }
            }
            else
            {
                player.Inform("You do not have enough space in your bag.");
            }
        }
        else if (player.Has(722510))
        {
            if (player.Roll(10) < 7)
            {
                dialog.Say("Oh, my god! Narrowly escaped! Bye, guy!", "Damn!");
            }
            else if (player.FreeSlots >= 2)
            {
                player.Take(722510);

                switch (player.Roll(1, 8))
                {
                    case <= 4:
                        if (player.Give(722512) is not null)
                        {
                            player.Inform("Congratulations! You got AsterNecklace");

                            dialog.Say("Let me go! I will give you something.", "Go away.");
                        }
                        break;

                    case >= 5 and <= 7:
                        if (player.Give(722513) is not null)
                        {
                            player.Inform("Congratulations! You got PinetumPicture");

                            dialog.Say("Let me go! I will give you something.", "Go away.");
                        }
                        break;

                    case 8:
                        switch (player.Roll(1, 8))
                        {
                            case <= 7:
                                if (player.Give(722512) is not null && player.Give(722513) is not null)
                                {
                                    player.Inform("Congratulations! You got AsterNecklace and PinetumPicture");

                                    dialog.Say("Let me go! I will give you something.", "Go away.");
                                }
                                break;

                            case 8:
                                if (player.Give(722514) is not null)
                                {
                                    player.Inform("Congratulations! You got RoyalSword");

                                    dialog.Say("Let me go! I will give you something.", "Go away.");
                                }
                                break;
                        }
                        break;
                }
            }
            else
            {
                player.Inform("Congratulations! You got");
            }
        }
        else
        {
            dialog.Say("You should at least bring a needle to catch me. Goodbye!", "Damn!");
        }
    }
}
