namespace Conquer.Game.Scripts.Drops;

public static class EvilAbyss
{
    [Monster(3600, 3604, 3608, 3612, 3616, 3620, 3624, 3628)]
    public static void FirstRank(Player player, Monster monster)
    {
        DropFlowerPart(player, monster);
        SinkHill(player, monster, 1, 250);
    }

    [Monster(3601, 3605, 3609, 3613, 3617, 3621, 3625, 3629)]
    public static void SecondRank(Player player, Monster monster)
    {
        DropFlowerPart(player, monster);
        SinkHill(player, monster, 6, 225);
    }

    [Monster(3602, 3606, 3610, 3614, 3618, 3622, 3626, 3630)]
    public static void ThirdRank(Player player, Monster monster)
    {
        DropFlowerPart(player, monster);
        SinkHill(player, monster, 20, 60);
    }

    [Monster(3603, 3607, 3611, 3615, 3619, 3623, 3627, 3631)]
    public static void Lords(Player player, Monster monster)
    {
        DropFlowerPart(player, monster);
        SinkHill(player, monster, 500, 6, lord: true);
    }

    private static void DropFlowerPart(Player player, Monster monster)
    {
        if (player.Roll(10) < 1)
        {
            monster.Drop(player.Pick<uint>(722733, 722733, 722734, 722735, 722737, 722737, 722738, 722739), player);
        }
    }

    private static void SinkHill(Player player, Monster monster, int meters, int odds, bool lord = false)
    {
        if (player.Rebirths != 1 || player.Level < 120)
        {
            return;
        }

        var stage = player.GetProgress(61, 0);

        if (stage == 1)
        {
            if (player.GetProgress(61, 1) >= 40000)
            {
                return;
            }

            if (player.GetProgress(61, 1) >= 40000 - meters)
            {
                player.Alert("All souls of the dead have been released from suffering. The hill of grievance has been razed to the ground!");
            }
            else
            {
                player.Inform($"The hill of grievance has sunk for {meters} {(meters == 1 ? "meter" : "meters")}");
            }

            player.AddProgress(61, 1, meters);
            player.SetProgress(61, 11, 1);
        }
        else if (lord && stage == 3)
        {
            monster.Drop(722727, player);
            player.Alert("The shining Squama Bead has appeared in the evil dark mist.");
        }
        else if (stage < 1)
        {
            if (player.Roll(odds) < 1)
            {
                monster.Drop(722723, player);
            }
            else if (player.Roll(odds - 1) < 1)
            {
                monster.Drop(722724, player);
            }
            else if (player.Roll(odds - 2) < 1)
            {
                monster.Drop(722725, player);
            }
        }
    }

    [Monster(3632)]
    public static void HillSpirit(Player player, Monster monster)
    {
        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            return;
        }

        if (player.GetProgress(61, 14) == 1)
        {
            player.SetProgress(61, 14, 0);
            monster.Drop(722722, player);
        }
    }

    [Monster(3633)]
    public static void SwiftDevil(Player player, Monster monster)
    {
        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            return;
        }

        if (player.GetProgress(61, 15) == 1)
        {
            player.SetProgress(61, 15, 0);
            monster.Drop(722726, player);
        }
    }

    [Monster(3634)]
    public static void Banshee(Player player, Monster monster)
    {
        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            return;
        }

        if (player.GetProgress(61, 16) == 1)
        {
            player.SetProgress(61, 16, 0);
            monster.Drop(722729, player);
        }
    }

    [Monster(3635)]
    public static void CleansingDevil(Player player, Monster monster)
    {
        if (player.Rebirths is 2 or 0 || player.Level < 120 || player.GetProgress(61, 0) >= 1)
        {
            return;
        }

        if (player.GetProgress(61, 17) == 1)
        {
            player.SetProgress(61, 17, 0);
            monster.Drop(722731, player);
        }
    }

    [Monster(3636)]
    public static void Andrew(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The force of AndrewCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3637)]
    public static void Peter(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The force of PeterCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3638)]
    public static void Philip(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The force of PhilipCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3639)]
    public static void Timothy(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The force of TimothyCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3640)]
    public static void Daphne(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The force of DaphneCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3641)]
    public static void Victoria(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The force of VictoriaCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3642)]
    public static void Wayne(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 21) == 9)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The force of WayneCurse has disappeared!");
            player.AddProgress(61, 2, 1);
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 7)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Theodore.");
        }
    }

    [Monster(3643)]
    public static void Theodore(Player player, Monster monster)
    {
        if (player.GetProgress(61, 0) != 2)
        {
            return;
        }

        if (player.GetProgress(61, 2) == 7)
        {
            if (player.GetProgress(61, 21) <= 7)
            {
                player.Alert("You`ve broken 8 curses. The BindingCurse has shed a lot of its blood. The evil force has temporarily disappeared.");
                player.SetProgress(61, 2, 0);
                player.AddProgress(61, 21, 1);
                return;
            }

            if (player.GetProgress(61, 21) == 8)
            {
                player.SetProgress(61, 2, 0);
                player.AddProgress(61, 21, 1);
                player.Alert("The BindingCurse has been broken into pieces. It has shed all of its blood, but it is still possessed by the evil force.");
            }
        }
        else if (player.GetProgress(61, 21) == 9)
        {
        }
        else if (player.GetProgress(61, 2) == 0)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Andrew.");
        }
        else if (player.GetProgress(61, 2) == 1)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Peter.");
        }
        else if (player.GetProgress(61, 2) == 2)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Philip.");
        }
        else if (player.GetProgress(61, 2) == 3)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Timothy.");
        }
        else if (player.GetProgress(61, 2) == 4)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Daphne.");
        }
        else if (player.GetProgress(61, 2) == 5)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Victoria.");
        }
        else if (player.GetProgress(61, 2) == 6)
        {
            player.Alert("The BindingCurse has been weakened, but there are no signs of fading away. It seems that you should go to kill Wayne.");
        }
    }

    [Monster(3644)]
    public static void Satan(Player player, Monster monster)
    {
        player.World.Spawn(3645, 1700, 337, 341);
        player.Inform("Satan has taken the form of BeastSatan.");
    }

    [Monster(3645)]
    public static void BeastSatan(Player player, Monster monster)
    {
        player.World.Spawn(3646, 1700, 337, 341);
        player.Inform("Satan has taken the form of FurySatan.");
    }

    [Monster(3646)]
    public static void FurySatan(Player player, Monster monster)
    {
        if (player.Rebirths is 2 or 0 || player.Level < 120)
        {
            return;
        }

        if (player.GetProgress(61, 0) == 3)
        {
            player.SetProgress(61, 0, 4);
            player.Inform("Congratulations! You`ve killed Satan and completed all tests.");
            player.World.Announce(1700,
                $"Satan has done all kinds of evil in EvilAbyss. It is finally killed by {player.Name}!",
                TalkChannel.System);
        }
    }
}
