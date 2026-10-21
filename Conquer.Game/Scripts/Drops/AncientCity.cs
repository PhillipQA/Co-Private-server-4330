namespace Conquer.Game.Scripts.Drops;

public static class AncientCity
{
    [Monster(20, 80)]
    public static void TombBat(Player player, Monster monster)
    {
        if (player.World.Now.IsBetween(new(2006, 1, 7, 0, 0, 0), new(2006, 2, 9, 20, 0, 0)) && player.Roll(3) < 1)
        {
            monster.Drop(720501, player);
        }
    }

    [Monster(57)]
    public static void BullMonster(Player player, Monster monster)
    {
        if (player.World.Now.IsBetween(new(2006, 1, 1, 0, 0, 0), new(2006, 2, 9, 20, 0, 0)) && player.Roll(3) < 1)
        {
            monster.Drop(720467, player);
        }
    }

    [Monster(58, 88)]
    public static void RedAndBloodyDevil(Player player, Monster monster) => monster.Drop(723030, player, 1, 800);

    [Monster(89)]
    public static void DevilKing(Player player, Monster monster) => monster.Drop(723030, player, 1, 250);

    [Monster(91)]
    public static void BloodyKing(Player player, Monster monster) => monster.DropEquipment(player, 215);

    [Monster(92, 93)]
    public static void BullAndGreenDevil(Player player, Monster monster) => monster.DropEquipment(player, 225);

    [Monster(8116, 8117, 8118, 8119)]
    public static void Messengers(Player player, Monster monster)
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

    [Monster(8216, 8217, 8218, 8219)]
    public static void Aides(Player player, Monster monster)
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

    [Monster(8316, 8317, 8318, 8319)]
    public static void Kings(Player player, Monster monster)
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
