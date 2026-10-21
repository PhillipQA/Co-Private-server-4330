namespace Conquer.Game.Scripts.Drops;

public static class HellGate
{
    [Monster(5045, 5046)]
    public static void UndeadSoldierAndPhantom(Player player, Monster monster)
    {
        if (player.Profession <= Profession.TrojanMaster || player.Profession <= Profession.WarriorMaster)
        {
            monster.Drop(723085, player, 1, 60);
        }
        else if (player.Profession <= Profession.ArcherMaster)
        {
            monster.Drop(723085, player, 1, 100);
        }
        else if (player.Profession <= Profession.WaterSaint)
        {
            monster.Drop(723085, player, 1, 50);
        }
        else if (player.Roll(80) < 1)
        {
            monster.Drop(723085, player);
        }
    }

    [Monster(5049, 5050)]
    public static void UndeadSpearmanAndRevenant(Player player, Monster monster)
    {
        if (player.Profession <= Profession.TrojanMaster || player.Profession <= Profession.WarriorMaster)
        {
            monster.Drop(723085, player, 2, 75);
        }
        else if (player.Profession <= Profession.ArcherMaster)
        {
            monster.Drop(723085, player, 2, 125);
        }
        else if (player.Profession <= Profession.WaterSaint)
        {
            monster.Drop(723085, player, 4, 125);
        }
        else if (player.Roll(50) < 1)
        {
            monster.Drop(723085, player);
        }
    }

    [Monster(5051)]
    public static void Eidolon(Player player, Monster monster)
    {
        if (player.Profession <= Profession.TrojanMaster || player.Profession <= Profession.WarriorMaster ||
            player.Profession <= Profession.ArcherMaster || player.Profession <= Profession.WaterSaint)
        {
            monster.Drop(723085, player, 1, 10);
        }
        else if (player.Roll(10) < 1)
        {
            monster.Drop(723085, player);
        }
    }
}
