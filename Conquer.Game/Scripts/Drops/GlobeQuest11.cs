namespace Conquer.Game.Scripts.Drops;

public static class GlobeQuest11
{
    [Monster(8417, 8418)]
    public static void EvilGhostAndNightDevil(Player player, Monster monster)
    {
        if (player.Roll(300) < 1)
        {
            if (player.Roll(2) < 1)
            {
                monster.Drop(player.Pick<uint>(150097, 160097, 120097, 131847, 111347, 134447, 133627, 114947), player);
            }
            else
            {
                monster.Drop(player.Pick<uint>(900717, 151097, 130647, 118547, 117357, 113827, 121097, 152087), player);
            }
        }
        else if (player.Roll(100) < 1)
        {
            if (player.Roll(2) < 1)
            {
                monster.Drop(player.Pick<uint>(150096, 160096, 120096, 131846, 111346, 134446, 133626, 114946), player);
            }
            else
            {
                monster.Drop(player.Pick<uint>(900716, 151096, 130646, 118546, 117356, 113826, 121096, 152086), player);
            }
        }
    }
}
