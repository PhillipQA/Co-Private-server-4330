namespace Conquer.Game.Scripts.Npcs;

public static class Death
{
    [Npc(600016)]
    public static async Task GuardianGod(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (await dialog.Ask("I have watched this tactic for many years and seen many challengers. Do you have full confidence that you will succeed?",
            "What tactic is this?", "Of course.") == 0)
        {
            dialog.Say("This is Death Tactic. If you do not have the tokens for other 6 tactics, you cannot leave here until you die.",
                "How terrible!");
        }
    }

    [Npc(600017)]
    public static async Task Ghost(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("I have been trapped here for many years. When can I see a person break through the tactics?",
            "How can I conquer?", "What nonsense!");

        if (!player.Has(721015))
        {
            dialog.Say("You are so weak. I am afraid you will die from the tactics.", "I must prove my ability.");
            return;
        }

        await dialog.Confirm("If you do not have the tokens for the tactics other than Death and life, you cannot leave here until you die.",
            "I have all tokens.", "What a bad luck!");

        if (!player.Has(721010) || !player.Has(721011) || !player.Has(721012) || !player.Has(721013) ||
            !player.Has(721014) || !player.Has(721015))
        {
            dialog.Say("Sorry, you do not have the said tokens. I am afraid you cannot leave here until you die.",
                "God bless me.");
            return;
        }

        await dialog.Confirm("Great! Would you like me to make a Soul Jade from your tokens so that you can enter Life Tactic to claim your prize?",
            "Yes, Please.", "No, thanks.");

        if (player.Take(721010) && player.Take(721011) && player.Take(721012) && player.Take(721013) &&
            player.Take(721014) && player.Take(721015) && player.Teleport(1050, 211, 164, force: true))
        {
            player.Give(721072);
        }
    }
}
