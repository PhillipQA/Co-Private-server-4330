namespace Conquer.Game.Scripts.Drops;

public static class LeftCloister
{
    [Monster(5055)]
    public static void Wraith(Player player, Monster monster)
    {
        if (player.World.CountMonsters("Wraith", 4023) < 12)
        {
            player.World.Announce(4025, "Knights from left flank of HellCloister have broken into BattleFormation!",
                TalkChannel.System);
            player.World.Announce(4024, "Knights from left flank of HellCloister have broken into BattleFormation!",
                TalkChannel.System);

            player.World.Maps[4023].Each(-1, (player, dialog) =>
            {
                player.Inform("Be careful that BattleFormation is a PK map! Once  you log off, you`ll start from Canyon again.");

                if (player.Teleport(4025, [(148, 276), (154, 276), (154, 270), (148, 270), (148, 276), (154, 276), (154, 270), (148, 270)]))
                {
                    dialog.Avatar = 67;

                    dialog.Say("Now you have reached BattleFormation eventually. Kill all the Syrens then UltimatePluto will come out.\nkill him to get DarkHorn. And I will use it to destroy BattleFormation.",
                        "I`ll watch out.");
                }
            });
        }
        else if (player.Roll(1200) < 1)
        {
            monster.Drop(725016, player);
        }
    }
}
