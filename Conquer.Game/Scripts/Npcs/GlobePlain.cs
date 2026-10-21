namespace Conquer.Game.Scripts.Npcs;

public static class GlobePlain
{
    [Npc(300655)]
    public static async Task OldExplorer(Player player, Dialog dialog)
    {
        dialog.Avatar = 12;

        if (await dialog.Ask("This is the way to the meteor and adventure area. The main monsters in adventure area are ferocious but may drop nice\nitems. In the two areas, special monsters have the same appearance and name as normal ones, but they may drop\nmeteors or DragonBalls.You can`t use teleport scrolls there. You may ask the boatman on the in-island to help you leave there.",
            "I want to have a try.", "Thanks.") == 0)
        {
            player.Record(1210, 1039, 718);
            player.Teleport(1210, 1039, 718, force: true);
        }
    }
}
