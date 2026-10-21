namespace Conquer.Game.Scripts.Npcs;

public static class GlobeDesert
{
    [Npc(3000)]
    public static async Task DoctorHolt(Player player, Dialog dialog)
    {
        dialog.Avatar = 15;

        if (player.Has(722809))
        {
            if (!player.Has(722807) || !player.Has(722808))
            {
                if (await dialog.Ask("SaviorPill is rare and invaluable. You must have racked your brains to get it.",
                    "Yeah. I know that you need it badly.", "It is no sweat at all.") == 0)
                {
                    await HandOverSaviorPill();
                }

                return;
            }

            await dialog.Confirm("With those ingredients, I will be able to make the medicine to control the plague. Could you send them to me?",
                "Sure. That is what I am coming for.", "I`d rather keep SaviorPill to myself.");

            await dialog.Confirm("Thanks to your kindness, the villagers will survive the plague. Please take this meteor as my token of thankfulness.",
                "Thanks a lot.");

            if (player.Take(722807) && player.Take(722808) && player.Take(722809) &&
                player.Give(ItemType.Meteor) is not null)
            {
                player.Inform("Congratulations! You have received a meteor.");
                var (mapId, x, y) = player.Pick<(uint, ushort, ushort)>([(1000, 496, 578), (1000, 302, 433),
                    (1213, 474, 255), (1000, 53, 283), (1213, 442, 258), (1000, 496, 578), (1213, 474, 255),
                    (1213, 442, 258)]);
                player.World.MoveNpc(3000, mapId, x, y);
            }
        }
        else if (player.Has(722817))
        {
            if (player.Has(722818))
            {
                if (player.Has(722819))
                {
                    dialog.Say("Haven`t you found out the Antique Dealer?", "It won`t be long.");
                }
                else if (player.Has(722807))
                {
                    if (!player.Has(722808))
                    {
                        dialog.Say("FireSpiritFang and Aweto must be used together as the ingredients.", "Oh, I see.");
                        return;
                    }

                    if (await dialog.Ask("You are marvelous to get those two items in such a short notice.",
                        "I am concerned with the villagers too.") == 0)
                    {
                        await ShowConcern();
                    }
                }
                else if (player.Has(722808))
                {
                    dialog.Say("FireSpiritFang and Aweto must be used together as the ingredients.", "Oh, I see.");
                }
                else
                {
                    if (await dialog.Ask("May I stop you for a minute? It gets on my nerves. I really need your help.",
                        "Take it easy. I am glad to be of help.",
                        "I am sorry, I`ve got something important to do.") == 0)
                    {
                        await OfferHelp();
                    }
                }
            }
            else if (player.Has(722807))
            {
                if (!player.Has(722808))
                {
                    dialog.Say("FireSpiritFang and Aweto must be used together as the ingredients.", "Oh, I see.");
                    return;
                }

                if (await dialog.Ask("You are marvelous to get those two items in such a short notice.",
                    "I am concerned with the villagers too.") == 0)
                {
                    await ShowConcern();
                }
            }
            else if (player.Has(722808))
            {
                dialog.Say("FireSpiritFang and Aweto must be used together as the ingredients.", "Oh, I see.");
            }
            else
            {
                if (await dialog.Ask("May I stop you for a minute? It gets on my nerves. I really need your help.",
                    "Take it easy. I am glad to be of help.", "I am sorry, I`ve got something important to do.") == 0)
                {
                    await OfferHelp();
                }
            }
        }
        else if (player.Has(722807))
        {
            if (!player.Has(722808))
            {
                dialog.Say("FireSpiritFang and Aweto must be used together as the ingredients.", "Oh, I see.");
                return;
            }

            if (await dialog.Ask("You are marvelous to get those two items in such a short notice.",
                "I am concerned with the villagers too.") == 0)
            {
                await ShowConcern();
            }
        }
        else if (player.Has(722808))
        {
            dialog.Say("FireSpiritFang and Aweto must be used together as the ingredients.", "Oh, I see.");
        }
        else
        {
            if (await dialog.Ask("May I stop you for a minute? It gets on my nerves. I really need your help.",
                "Take it easy. I am glad to be of help.", "I am sorry, I`ve got something important to do.") == 0)
            {
                await OfferHelp();
            }
        }

        async Task HandOverSaviorPill()
        {
            if (await dialog.Ask("But having SaviorPill is not enough. I need FireSpiritFang and Aweto, together with it, to make medicine to cure the villagers.",
                "Don`t worry. I will fetch them for you.") == 0)
            {
                dialog.Say("You are really kind. I believe the villagers will recover from the diseases soon with your persistent help.",
                    "It is my pleasure.");
            }
        }

        async Task ShowConcern()
        {
            switch (await dialog.Ask("Thanks. Those two items can relieve the illness although they can not cure it. SaviorPill is an indispensable ingredient.",
                "How to get it?", "I will set off to get it right now."))
            {
                case 0:
                    await dialog.Confirm("I know that the Antique Dealer in Bird Island has it. He is fond of collecting all kinds of curios.",
                        "Oh, but I do not have any curio.");

                    if (await dialog.Ask("Do not worry. I have some. But, you know? I do not have time to pay him a visit.",
                        "I can run an errand for you.") == 0)
                    {
                        await RunErrand();
                    }
                    break;

                case 1:
                    await RunErrand();
                    break;
            }
        }

        async Task OfferHelp()
        {
            await dialog.Confirm("Plague is prevailing in the Desert. I am frustrated after seeing so many people being tortured by the disease.",
                "Have you come up with any solution?", "I am sorry to hear that.");

            await dialog.Confirm("Yeah, I worked out a formula to make the remedy for the disease after I have studied a great deal of books and\ndone a lot of experiments. What bothers me is that three ingredients are necessary: FireSpiritFang, Aweto and SaviorPill.",
                "How to get those ingredients?", "Are you sure of the effect?");

            if (await dialog.Ask("You can get FireSpiritFang from the FireSpirit in the forest, and Aweto from the Macaque in the canyon.\nAs to SaviorPill, it is a rather rare panacea.",
                "How to get it?", "Oh, I see.") == 0)
            {
                dialog.Say("I heard that an Antique Dealer in Bird Island has it. And he will probably give it away if you send him some rare\ncurios.I have TankFrestco, KingSword and AmberCup. If you bring me the FireSpiritFang and Aweto, I will entrust them to you.",
                    "Ok. I will go to find them.");
            }
        }

        async Task RunErrand()
        {
            if (await dialog.Ask("That is great! Here are my antiques. The Antique Dealer will be interested and give you the pill.",
                "Alright. I will go right now.") != 0)
            {
                return;
            }

            if (!player.Take(722807) || !player.Take(722808))
            {
                return;
            }

            switch (player.Roll(1, 8))
            {
                case 1 or 4 or 7:
                    if (player.Give(722817) is not null)
                    {
                        player.Inform("Congratulations! You have received a TangFresco.");
                    }
                    break;

                case 2 or 5 or 8:
                    if (player.Give(722818) is not null)
                    {
                        player.Inform("Congratulations! You have received a King Sword.");
                    }
                    break;

                case 3 or 6:
                    if (player.Give(722819) is not null)
                    {
                        player.Inform("Congratulations! You have received an Amber Cup.");
                    }
                    break;
            }
        }
    }
}
