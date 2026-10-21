namespace Conquer.Game.Scripts.Drops;

public static class CopperMine
{
    [Monster(51)]
    public static void RockMonster(Player player, Monster monster)
    {
        if (player.Roll(150000) < 1)
        {
            monster.Drop(ItemType.DragonBall, player);
        }
        else if (player.Roll(150000) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(107)]
    public static void RockMonsterL72(Player player, Monster monster) => monster.Drop(723007, player, 1, 240);
}
