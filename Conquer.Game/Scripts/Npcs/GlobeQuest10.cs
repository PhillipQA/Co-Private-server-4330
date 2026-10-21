namespace Conquer.Game.Scripts.Npcs;

public static class GlobeQuest10
{
    [Npc(300652)]
    public static async Task MineSupervisor(Player player, Dialog dialog)
    {
        dialog.Avatar = 12;

        await dialog.Confirm("Many kinds of Gems are buried in this mine, like Dragon, Phoenix, Rainbow and Kylin Gems. Will you enter this mine?",
            "Yeah.", "Just passing by.");

        if (player.Level < 40)
        {
            dialog.Say("You cannot enter this mine before you are level 40 or above.", "I see.");
        }
        else
        {
            player.Teleport(1218, 29, 70, force: true);
        }
    }
}
