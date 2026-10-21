namespace Conquer.Game.Scripts.Npcs;

public static class PassPortal
{
    [Npc(600003)]
    public static async Task Maggie(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.Has("SoulJade"))
        {
            if (await dialog.Ask("With your Soul Jade, you can enter the Life Tactic. Would you like to enter this tactic now?",
                "Yeah.", "No, thanks.") == 0)
            {
                player.Teleport(1050, 211, 164);
            }

            return;
        }

        switch (await dialog.Ask("What are you here for? Please do not go ahead, or you will enter a very dangerous tactic.",
            "Such tactics do exist?", "I decide to try.", "I had better leave here."))
        {
            case 0:
                await dialog.Confirm("The tactics are most changeful. I once thought highly of myself and died from the damned tactics.",
                    "How can I break through?", "I do not believe it.");

                await dialog.Confirm("I have studied it for many years, but fail to work it out. It is similar to the said Eight-Diagram Tactics. Do you want to try?",
                    "Yeah. I like challenge.", "No. I changed my mind.");

                if (await dialog.Ask("Once you enter the tactics, it is very difficult to leave. Please think it over. I do not want you to die from them.",
                    "I made up my mind.", "I changed my mind.") == 0)
                {
                    await IDecideToTry();
                }
                break;

            case 1:
                await IDecideToTry();
                break;
        }

        async Task IDecideToTry()
        {
            await dialog.Confirm("Since you like challenge, I shall teleport you there. Be careful and remember to pick up the token dropped by the monsters.",
                "Okay.");

            switch (player.Roll(1, 8))
            {
                case 1:
                    player.Teleport(1043, 211, 164);
                    break;

                case 2:
                    player.Teleport(1044, 211, 164);
                    break;

                case 3:
                    player.Teleport(1045, 211, 164);
                    break;

                case 4:
                    player.Teleport(1046, 211, 164);
                    break;

                case 5:
                    player.Teleport(1047, 211, 164);
                    break;

                case 6:
                    player.Teleport(1048, 211, 164);
                    break;

                case 7:
                    player.Teleport(1049, 211, 164);
                    break;

                case 8:
                    player.Teleport(player.Pick<uint>(1043, 1044, 1045, 1046, 1047, 1048, 1049), 211, 164);
                    break;
            }
        }
    }
}
