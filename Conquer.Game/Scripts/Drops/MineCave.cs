namespace Conquer.Game.Scripts.Drops;

public static class MineCave
{
    [Monster(59)]
    public static void RockMonsterL15(Player player, Monster monster)
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
}
