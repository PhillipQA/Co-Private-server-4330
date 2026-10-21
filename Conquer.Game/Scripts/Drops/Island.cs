namespace Conquer.Game.Scripts.Drops;

public static class Island
{
    [Monster(18)]
    public static void Birdman(Player player, Monster monster)
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

    [Monster(38)]
    public static void BirdKing(Player player, Monster monster) => monster.DropEquipment(player, 175);

    [Monster(39)]
    public static void SilverHawk(Player player, Monster monster) => monster.DropEquipment(player, 185);

    [Monster(40)]
    public static void Robber(Player player, Monster monster) => monster.DropEquipment(player, 195);

    [Monster(90)]
    public static void FlyingBat(Player player, Monster monster) => monster.DropEquipment(player, 205);

    [Monster(8113, 8114)]
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

    [Monster(8115)]
    public static void RobberMessenger(Player player, Monster monster)
    {
        if (player.Roll(100) < 3)
        {
            monster.Drop(player.Pick<uint>(150090, 160090, 410090, 420090, 430090, 440090, 450090, 450090), player);
        }
        else if (player.Roll(100) < 2)
        {
            monster.Drop(player.Pick<uint>(111350, 114650, 117350, 118350, 120090, 121090, 151090, 151090), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8213, 8214)]
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

    [Monster(8215)]
    public static void RobberAide(Player player, Monster monster)
    {
        if (player.Roll(100) < 1)
        {
            monster.Drop(player.Pick<uint>(460090, 480090, 481090, 490090, 510090, 530090, 540090, 540090), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(player.Pick<uint>(560090, 561090, 580090, 111350, 114350, 117350, 118350, 118350), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8313, 8314)]
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

    [Monster(8315)]
    public static void RobberKing(Player player, Monster monster)
    {
        if (player.Roll(100) < 2)
        {
            monster.Drop(ItemType.Meteor, player);
        }
        else if (player.Roll(100) < 2)
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
        else if (player.Roll(25000) < 1)
        {
            monster.Drop(ItemType.DragonBall, player);
        }
    }
}
