namespace Conquer.Game.Scripts.Drops;

public static class HellHall
{
    [Monster(5053)]
    public static void Temptress(Player player, Monster monster)
    {
        monster.Drop(1050002, player, 1, 30);

        player.SetRegister(3, player.GetRegister(3) + 1);
        player.AddScore(1);

        if (player.Profession <= Profession.TrojanMaster)
        {
            if (player.GetRegister(3) >= 800)
            {
                player.Inform("You`ve killed 800 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            if (player.GetRegister(3) >= 900)
            {
                player.Inform("You`ve killed 900 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.ArcherMaster)
        {
            if (player.GetRegister(3) >= 1300)
            {
                player.Inform("You`ve killed 1300 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.WaterSaint)
        {
            if (player.GetRegister(3) >= 600)
            {
                player.Inform("You`ve killed 600 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.GetRegister(3) >= 1000)
        {
            player.Inform("You`ve killed 1000 devils, you may find SolarSaint[215,344] to go to the next level.");
        }
    }

    [Monster(5054, 5057)]
    public static void CenticoreAndNaga(Player player, Monster monster)
    {
        player.SetRegister(3, player.GetRegister(3) + 1);
        player.AddScore(1);

        if (player.Profession <= Profession.TrojanMaster)
        {
            if (player.GetRegister(3) >= 800)
            {
                player.Inform("You`ve killed 800 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            if (player.GetRegister(3) >= 900)
            {
                player.Inform("You`ve killed 900 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.ArcherMaster)
        {
            if (player.GetRegister(3) >= 1300)
            {
                player.Inform("You`ve killed 1300 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.WaterSaint)
        {
            if (player.GetRegister(3) >= 600)
            {
                player.Inform("You`ve killed 600 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.GetRegister(3) >= 1000)
        {
            player.Inform("You`ve killed 1000 devils, you may find SolarSaint[215,344] to go to the next level.");
        }
    }

    [Monster(5056)]
    public static void HellTroll(Player player, Monster monster)
    {
        player.SetRegister(3, player.GetRegister(3) + 3);
        player.AddScore(1);
        player.AddScore(1);
        player.AddScore(1);

        if (player.Profession <= Profession.TrojanMaster)
        {
            if (player.GetRegister(3) >= 800)
            {
                player.Inform("You`ve killed 800 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            if (player.GetRegister(3) >= 900)
            {
                player.Inform("You`ve killed 900 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.ArcherMaster)
        {
            if (player.GetRegister(3) >= 1300)
            {
                player.Inform("You`ve killed 1300 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.Profession <= Profession.WaterSaint)
        {
            if (player.GetRegister(3) >= 600)
            {
                player.Inform("You`ve killed 600 devils, you may find SolarSaint[215,344] to go to the next level.");
            }
        }
        else if (player.GetRegister(3) >= 1000)
        {
            player.Inform("You`ve killed 1000 devils, you may find SolarSaint[215,344] to go to the next level.");
        }
    }
}
