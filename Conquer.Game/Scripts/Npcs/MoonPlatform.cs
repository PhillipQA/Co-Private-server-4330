namespace Conquer.Game.Scripts.Npcs;

public static class MoonPlatform
{
    [Npc(1500)]
    public static void Matchmaker(Player player, Dialog dialog)
    {
        dialog.Avatar = 116;

        dialog.Say("It`s a good place to watch the moon. You can usually come to enjoy the moon together. Have a good day!",
            "Thank you.");
    }
}
