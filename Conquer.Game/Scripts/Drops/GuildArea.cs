namespace Conquer.Game.Scripts.Drops;

public static class GuildArea
{
    [Monster(3120)]
    public static void GuildBeast(Player player, Monster monster)
    {
        player.World.FindNpc(819)!.Data0 = 2;
        monster.Drop(ItemType.DragonBall, player);
    }
}
