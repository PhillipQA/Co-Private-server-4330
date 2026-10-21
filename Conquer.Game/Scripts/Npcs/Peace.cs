namespace Conquer.Game.Scripts.Npcs;

public static class Peace
{
    [Npc(600004)]
    public static async Task GuardianGod(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("I have watched this tactic for many years and seen many challengers. Do you have full confidence that you will succeed?",
            "What tactic is this?", "Of course.");

        if (player.Has(721010))
        {
            dialog.Say("Since you have got a token, I am glad to tell you that this is Peace Tactic. You may ask Ghost to teleport you to Maggie.",
                "Thanks.");
        }
        else
        {
            dialog.Say("It is a secret. I cannot tell you before you have a token for this tactic.", "I see.");
        }
    }

    [Npc(600005)]
    public static async Task Ghost(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("I have been trapped here for many years. Many challengers have come but failed. When can I see a person conquer the tactics?",
            "How can I conquer?", "What nonsense!");

        if (!player.Has(721010))
        {
            dialog.Say("You are so weak. I am afraid you will die from this tactic.", "I must prove my ability.");
            return;
        }

        await dialog.Confirm("Wow! You have a token for Peace Tactic. My only wish is to see a person break through this tactic. I hope you can succeed.",
            "Why not leave here?");

        await dialog.Confirm("I do not want to get revived until I see a person break through the tactics. Perhaps you can make my dreams come true.",
            "How can I break through?");

        await dialog.Confirm("According to my experience, the key tactic is Death, but I do not know how it works. You may try the other tactics first.",
            "How can I leave?");

        await dialog.Confirm("Although I cannot help you break through this tactic, but I can help you leave here. I hope you can conquer the tactics soon.",
            "I must try my best.");

        if (await dialog.Ask("If you are ready to leave this tactic, I am going to teleport you out right now.",
            "I am ready.", "I shall leave later.") == 0)
        {
            player.Teleport(1042, 23, 24, force: true);
        }
    }
}
