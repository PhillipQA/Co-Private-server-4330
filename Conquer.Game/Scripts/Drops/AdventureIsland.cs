namespace Conquer.Game.Scripts.Drops;

public static class AdventureIsland
{
    [Monster(3102)]
    public static void SnakeKing(Player player, Monster monster)
    {
        if (player.Roll(96) < 1)
        {
            monster.Drop(160076, player);
        }
        else if (player.Roll(96) < 1)
        {
            monster.Drop(ItemType.DragonBall, player);
        }
        else if (player.Roll(1000) < 1)
        {
            monster.Drop(player.Pick<uint>(700013, 700023, 700033, 700043, 700053, 700063, 700003, 700023), player);
        }
        else if (player.Roll(300) < 1)
        {
            monster.Drop(player.Pick<uint>(700022, 700012, 700022, 700032, 700042, 700052, 700062, 700002), player);
        }
        else
        {
            if (player.Roll(8) < 1)
            {
                monster.Drop(ItemType.Meteor, player);
                monster.Drop(ItemType.Meteor, player);
                monster.Drop(ItemType.Meteor, player);
                monster.Drop(ItemType.Meteor, player);
                monster.Drop(ItemType.Meteor, player);
            }

            monster.Drop(ItemType.Meteor, player);
        }
    }
}
