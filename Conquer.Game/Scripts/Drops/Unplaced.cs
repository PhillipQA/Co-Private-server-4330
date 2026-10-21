namespace Conquer.Game.Scripts.Drops;

public static class Unplaced
{
    [Monster(100)]
    public static void Skeleton(Player player, Monster monster) => monster.Drop(480066, player);

    [Monster(101)]
    public static void GhostWarrior(Player player, Monster monster) => monster.Drop(480046, player);

    [Monster(102)]
    public static void SkeletonKing(Player player, Monster monster) => monster.Drop(480086, player);

    [Monster(103)]
    public static void GhostAres(Player player, Monster monster) => monster.Drop(480116, player);

    [Monster(5059)]
    public static void UltimatePluto(Player player, Monster monster)
    {
        monster.Drop(790001, player);
        player.World.DeleteNpcs(4025, 2);

        var saint = new DynaNpc
        {
            Name = "SolarSaint",
            Type = 2,
            Sort = 1,
            Look = 1310,
            MapId = 4025,
            X = 147,
            Y = 27,
            Task0 = 5020650
        };

        if (!player.World.CreateNpc(saint))
        {
            return;
        }

        if (player.ShownGuildRank < GuildRank.Leader)
        {
            player.World.Announce(4025,
                $"{player.Name} of {(player.Guild?.Name ?? "None")} has defeated UltimatePluto! Please give DarkHorn to SolarSaint[147,27].",
                TalkChannel.System);
            player.World.Announce($"{player.Name} of {(player.Guild?.Name ?? "None")} has defeated UltimatePluto!",
                TalkChannel.Gm);
        }
        else
        {
            player.World.Announce(4025,
                $"{player.Name} has defeated UltimatePluto! Please give DarkHorn to SolarSaint[147,27].",
                TalkChannel.System);
            player.World.Announce($"{player.Name} has defeated UltimatePluto!", TalkChannel.Gm);
        }
    }
}
