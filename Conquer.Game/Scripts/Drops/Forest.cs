namespace Conquer.Game.Scripts.Drops;

public static class Forest
{
    [Monster(6)]
    public static void WingedSnake(Player player, Monster monster)
    {
        if (!player.World.Now.IsBetween(new(2006, 1, 1, 0, 0, 0), new(2006, 2, 9, 20, 0, 0)))
        {
            return;
        }

        if (player.Roll(10) < 1)
        {
            monster.Drop(720459, player);
        }
        else if (player.Roll(10) < 1)
        {
            monster.Drop(720464, player);
        }
    }

    [Monster(7)]
    public static void Bandit(Player player, Monster monster) => monster.Drop(721263, player, 1, 1000);

    [Monster(8)]
    public static void Ratling(Player player, Monster monster)
    {
        if (player.World.Now.IsWeekly(DayOfWeek.Friday, new(12, 0), DayOfWeek.Saturday, new(15, 0)) &&
            player.Roll(300) < 1)
        {
            monster.Drop(722907, player);
        }
    }

    [Monster(9)]
    public static void FireSpirit(Player player, Monster monster) => monster.Drop(722807, player, 1, 70);

    [Monster(26)]
    public static void FireSnake(Player player, Monster monster) =>
        monster.DropEquipment(player, 55, [410, 420, 500, 430], [440, 450, 460, 480], [481, 490, 510, 530], [540, 560, 580, 561]);

    [Monster(27)]
    public static void CateranBoss(Player player, Monster monster) => monster.DropEquipment(player, 65);

    [Monster(28)]
    public static void BlueElf(Player player, Monster monster) =>
        monster.DropEquipment(player, 75, [410, 420, 421, 430], [440, 450, 460, 480], [481, 490, 510, 530], [530]);

    [Monster(29)]
    public static void ElfKing(Player player, Monster monster) => monster.DropEquipment(player, 85);

    [Monster(3031)]
    public static void Cateran(Player player, Monster monster) => monster.Drop(721117, player, 1, 50);

    [Monster(3032)]
    public static void PoisonousRat(Player player, Monster monster) => monster.Drop(721120, player, 1, 30);

    [Monster(3130, 3131, 3132, 3133)]
    public static void Ganoderma(Player player, Monster monster) => monster.Drop(723017, player, 1, 10);

    [Monster(6010)]
    public static void LonelyTyrant(Player player, Monster monster) => monster.Drop(723002, player);

    [Monster(8101, 8102, 8103, 8104)]
    public static void Messengers(Player player, Monster monster)
    {
        if (player.Roll(100) < 5)
        {
            monster.Drop(player.Pick<uint>(160050, 150050, 410050, 420050, 430050, 440050, 450050, 450050), player);
        }
        else if (player.Roll(100) < 5)
        {
            monster.Drop(player.Pick<uint>(460050, 130320, 131320, 134320, 460050, 130320, 131320, 134320), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8201, 8202, 8203, 8204)]
    public static void Aides(Player player, Monster monster)
    {
        if (player.Roll(100) < 2)
        {
            monster.Drop(player.Pick<uint>(460050, 480050, 481050, 490050, 510050, 530050, 540050, 540050), player);
        }
        else if (player.Roll(100) < 2)
        {
            monster.Drop(player.Pick<uint>(460050, 130320, 131320, 134320, 460050, 130320, 131320, 134320), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8301, 8302, 8303, 8304)]
    public static void Kings(Player player, Monster monster)
    {
        if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
        else if (player.Roll(100) < 1)
        {
            switch (player.Roll(1, 8))
            {
                case 1 or 4:
                    monster.Drop(ItemType.Meteor, player);
                    monster.Drop(ItemType.Meteor, player);
                    break;

                case 2 or 3 or 5 or 7 or 8:
                    monster.Drop(ItemType.Meteor, player);
                    break;

                case 6:
                    monster.Drop(ItemType.Meteor, player, 1, 100);
                    break;
            }
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }
}
