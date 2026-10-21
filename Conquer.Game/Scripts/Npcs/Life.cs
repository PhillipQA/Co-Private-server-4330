namespace Conquer.Game.Scripts.Npcs;

public static class Life
{
    [Npc(600018, 600019, 600020, 600021, 600022, 600023, 600024, 600025, 600026, 600027, 600028, 600029, 600030, 600031,
        600032, 600033, 600034, 600035, 600036, 600037, 600038, 600039, 600040, 600041, 600042)]
    public static async Task LonelyGhost(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        await dialog.Confirm("We have been trapped here for many years. Few challengers have a chance to arrive here. You are a real conqueror.",
            "Who are you?");

        await dialog.Confirm("We were from an old Kingdom. Our rivals made the mystic tactics. We failed to break through them and died.", "I see.");

        await dialog.Confirm("I am trapped here and cannot get revived. Since you have passed the tactics, would you like to help me get reborn?",
            "How can I help you?");

        switch (await dialog.Ask("We cannot get reborn unless someone would like to take us out. One person can take only one ghost at a time. Will you help me?",
            "No problem.", "Sorry, I am very busy."))
        {
            case 0:
                await NoProblem();
                break;

            case 1:
                switch (await dialog.Ask("You only need to spare a little time. I want to get reborn very much. Please kindly have a mercy for me.",
                    "I shall help you.", "I want to leave here."))
                {
                    case 0:
                        await NoProblem();
                        break;

                    case 1:
                        await dialog.Confirm("Since you do not want to help me, I cannot force you. If you are ready to leave here, I shall be glad to teleport you out.",
                            "I am ready.");

                        if (player.Take(721072))
                        {
                            player.Teleport(1042, 23, 24, force: true);
                        }
                        break;
                }
                break;
        }

        async Task NoProblem()
        {
            switch (await dialog.Ask("You can choose three Meteors or the MoonBox. But what you will get from MoonBox depends on your luck. Which one do you prefer?",
                "Three meteors.", "MoonBox.", "Let me think it over."))
            {
                case 0:
                    if (player.FreeSlots >= 3)
                    {
                        if (player.Take(721072) && player.Give(ItemType.Meteor, 3))
                        {
                            player.Teleport(1042, 23, 24, force: true);
                        }
                    }
                    else
                    {
                        dialog.Say("Please prepare three vacancy in your Bag.\nI see. 0", []);
                    }
                    break;

                case 1:
                    if (player.Take(721072) && player.Teleport(1042, 23, 24, force: true))
                    {
                        player.Give(721090);
                    }
                    break;
            }
        }
    }
}
