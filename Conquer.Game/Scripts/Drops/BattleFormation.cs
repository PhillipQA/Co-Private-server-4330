namespace Conquer.Game.Scripts.Drops;

public static class BattleFormation
{
    [Monster(5058)]
    public static void Syren(Player player, Monster monster)
    {
        if (player.World.CountMonsters("Syren", 4025) >= 1 || player.World.CountMonsters("UltimatePluto", 4025) >= 1)
        {
            return;
        }

        player.World.Spawn(5059, 4025, 156, 145);
        player.World.Announce(4025, "I`m imperishable! What is waiting for all of you is nothing but death!",
            TalkChannel.System);

        player.World.Maps[4025].Each(-1, (player, dialog) =>
        {
            player.PlaySound("sound/shake.wav");

            dialog.Later(5, () =>
            {
                player.PlaySound("sound/laugh.wav");
            });
        });
    }
}
