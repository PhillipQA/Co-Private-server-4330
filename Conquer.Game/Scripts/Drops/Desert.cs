namespace Conquer.Game.Scripts.Drops;

public static class Desert
{
    [Monster(14, 16, 17)]
    public static void SandRockAndBladeMonsters(Player player, Monster monster)
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

    [Monster(15)]
    public static void HillMonster(Player player, Monster monster)
    {
        if (player.Roll(180) >= 1)
        {
            return;
        }

        monster.Drop(1080001, player);

        if (player.Roll(10000) < 38)
        {
            monster.Drop(410133, player);
            monster.Drop(410134, player);
            monster.Drop(420133, player);
            monster.Drop(420134, player);
        }

        monster.Drop(410136, player, 69, 1000000);
        monster.Drop(420136, player, 69, 1000000);
        monster.Drop(410137, player, 14, 10000000);
        monster.Drop(420137, player, 14, 10000000);

        if (player.Roll(10000) < 38)
        {
            monster.Drop(410143, player);
            monster.Drop(410144, player);
            monster.Drop(420143, player);
            monster.Drop(420144, player);
        }

        monster.Drop(410146, player, 69, 1000000);
        monster.Drop(420146, player, 69, 1000000);
        monster.Drop(410147, player, 14, 10000000);

        if (player.Roll(10000000) < 14)
        {
            monster.Drop(420147, player);
        }
        else if (player.Roll(180) < 1)
        {
            monster.Drop(1080001, player);

            if (player.Roll(10000) < 38)
            {
                monster.Drop(410133, player);
                monster.Drop(410134, player);
                monster.Drop(420133, player);
                monster.Drop(420134, player);
            }

            monster.Drop(410136, player, 69, 1000000);
            monster.Drop(420136, player, 69, 1000000);
            monster.Drop(410137, player, 14, 10000000);
            monster.Drop(420137, player, 14, 10000000);

            if (player.Roll(10000) < 38)
            {
                monster.Drop(410143, player);
                monster.Drop(410144, player);
                monster.Drop(420143, player);
                monster.Drop(420144, player);
            }

            monster.Drop(410146, player, 69, 1000000);
            monster.Drop(420146, player, 69, 1000000);
            monster.Drop(410147, player, 14, 10000000);

            if (player.Roll(10000000) < 14)
            {
                monster.Drop(420147, player);
            }
            else if (player.Roll(180) < 1)
            {
                monster.Drop(1080001, player);

                if (player.Roll(10000) < 38)
                {
                    monster.Drop(410133, player);
                    monster.Drop(410134, player);
                    monster.Drop(420133, player);
                    monster.Drop(420134, player);
                }

                monster.Drop(410136, player, 69, 1000000);
                monster.Drop(420136, player, 69, 1000000);
                monster.Drop(410137, player, 14, 10000000);
                monster.Drop(420137, player, 14, 10000000);

                if (player.Roll(10000) < 38)
                {
                    monster.Drop(410143, player);
                    monster.Drop(410144, player);
                    monster.Drop(420143, player);
                    monster.Drop(420144, player);
                }

                monster.Drop(410146, player, 69, 1000000);
            }
        }
    }

    [Monster(32)]
    public static void SnowApe(Player player, Monster monster) =>
        monster.DropEquipment(player, 115, [410, 420, 500, 430], [440, 450, 460, 480], [481, 490, 510, 530], [540, 560, 580, 561]);

    [Monster(33)]
    public static void CrazySnake(Player player, Monster monster) => monster.DropEquipment(player, 125);

    [Monster(34)]
    public static void SandDevil(Player player, Monster monster) => monster.DropEquipment(player, 135);

    [Monster(35)]
    public static void MountMonster(Player player, Monster monster) => monster.DropEquipment(player, 145);

    [Monster(36)]
    public static void StoneMonster(Player player, Monster monster) => monster.DropEquipment(player, 155);

    [Monster(37)]
    public static void BladeMonster(Player player, Monster monster) => monster.DropEquipment(player, 165);

    [Monster(76)]
    public static void BladeGhostL83(Player player, Monster monster) => monster.Drop(721254, player, 1, 30);

    [Monster(3033)]
    public static void StoneBandit(Player player, Monster monster) => monster.Drop(721128, player, 1, 30);

    [Monster(6011)]
    public static void Ghost(Player player, Monster monster) => monster.Drop(723004, player);

    [Monster(8109, 8110, 8111, 8112)]
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

    [Monster(8209, 8210, 8211, 8212)]
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

    [Monster(8309, 8310, 8311, 8312)]
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
