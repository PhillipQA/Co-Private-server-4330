namespace Conquer.Game.Scripts.Drops;

public static class GlobeQuest10
{
    [Monster(8415)]
    public static void MeteorDove(Player player, Monster monster)
    {
        monster.Drop(ItemType.Meteor, player);
        monster.Drop(ItemType.Meteor, player);
        monster.Drop(ItemType.Meteor, player);
        monster.Drop(ItemType.Meteor, player);
        monster.Drop(ItemType.Meteor, player);
    }

    [Monster(8416)]
    public static void LesserMeteorDove(Player player, Monster monster) => monster.Drop(ItemType.Meteor, player, 1, 150);
}
