namespace Conquer.Game.Scripts.Drops;

public static class Canyon
{
    [Monster(10)]
    public static void Macaque(Player player, Monster monster) => monster.Drop(722808, player, 1, 80);

    [Monster(11)]
    public static void GiantApe(Player player, Monster monster)
    {
        if (player.World.Now.IsWeekly(DayOfWeek.Friday, new(12, 0), DayOfWeek.Saturday, new(15, 0)) &&
            player.Roll(50) < 1)
        {
            monster.Drop(722917, player);
        }
    }

    [Monster(13)]
    public static void Snakeman(Player player, Monster monster)
    {
        if (player.Roll(100000) < 1)
        {
            monster.Drop(1060100, player);
        }
        else if (player.Roll(100000) < 1)
        {
            monster.Drop(1060101, player);
        }
    }

    [Monster(30)]
    public static void SmartMonkey(Player player, Monster monster) =>
        monster.DropEquipment(player, 95, [410, 420, 500, 430], [440, 450, 460, 480], [481, 490, 510, 530], [540, 560, 580, 561]);

    [Monster(31)]
    public static void RoaringApe(Player player, Monster monster) => monster.DropEquipment(player, 105);

    [Monster(69)]
    public static void MacaqueL48(Player player, Monster monster)
    {
        if (player.World.Now.IsBetween(new(2006, 1, 1, 0, 0, 0), new(2006, 2, 9, 20, 0, 0)) && player.Roll(3) < 1)
        {
            monster.Drop(720465, player);
        }
    }

    [Monster(3050)]
    public static void HeresySnakeman(Player player, Monster monster) => monster.Drop(721124, player, 1, 30);

    [Monster(3134, 3135, 3136, 3137)]
    public static void Titan(Player player, Monster monster) => monster.Drop(723017, player, 1, 10);

    [Monster(8105)]
    public static void MonkeyMessenger(Player player, Monster monster)
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

    [Monster(8106, 8107, 8108)]
    public static void Messengers(Player player, Monster monster)
    {
        if (player.Roll(100) < 4)
        {
            monster.Drop(player.Pick<uint>(150070, 160070, 410070, 420070, 421070, 430070, 440070, 440070), player);
        }
        else if (player.Roll(100) < 3)
        {
            monster.Drop(player.Pick<uint>(900300, 130440, 131340, 134340, 900300, 130440, 131340, 134340), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8205)]
    public static void MonkeyAide(Player player, Monster monster)
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

    [Monster(8206, 8207, 8208)]
    public static void Aides(Player player, Monster monster)
    {
        if (player.Roll(100) < 1)
        {
            monster.Drop(player.Pick<uint>(450070, 460070, 480070, 481070, 490070, 510070, 530070, 530070), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(player.Pick<uint>(540070, 560070, 151070, 120080, 540070, 560070, 151070, 120080), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8305)]
    public static void MonkeyKing(Player player, Monster monster)
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

    [Monster(8306, 8307, 8308)]
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
                case 1 or 3:
                    monster.Drop(ItemType.Meteor, player);
                    monster.Drop(ItemType.Meteor, player);
                    break;

                case 2 or 4:
                    monster.Drop(ItemType.Meteor, player);
                    break;
            }

            monster.Drop(ItemType.Meteor, player);
        }
        else if (player.Roll(35000) < 1)
        {
            monster.Drop(ItemType.DragonBall, player);
        }
    }
}
