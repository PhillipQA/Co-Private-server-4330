namespace Conquer.Game.Scripts.Npcs;

public static class Legendsmith
{
    public const uint NpcId = 10777;

    [Npc(NpcId)]
    public static async Task Talk(Player player, Dialog dialog)
    {
        var choice = await dialog.Ask(
            "I am the Legendsmith. With the right materials I can forge Super equipment into Legendary gear. " +
            "Every kind of Legendary item awakens its own special power.",
            "Forge my gear.", "Make a Super Dragon Ball.", "Make a Legend Orb.", "Which of my gear is Legendary?",
            "How does this work?", "Reforge attributes.", "Goodbye.");

        switch (choice)
        {
            case 0:
                await Forge();
                break;

            case 1:
                if (player.MakeSuperDragonBall())
                {
                    dialog.Say("Here is your Super Dragon Ball. Use it to try forging, or combine three into a Legend Orb.", "Thanks.");
                }
                else
                {
                    dialog.Say($"A Super Dragon Ball takes {Player.DragonBallsPerSuper} Dragon Balls. Come back when you have them.", "I see.");
                }
                break;

            case 2:
                if (player.MakeLegendOrb())
                {
                    dialog.Say("A Legend Orb! Forging with it never fails.", "Thanks.");
                }
                else
                {
                    dialog.Say($"A Legend Orb takes {Player.SupersPerOrb} Super Dragon Balls and {Player.ShardsPerOrb} Dragon Shards. " +
                               "Dragon Shards are dropped by bosses.", "I see.");
                }
                break;

            case 3:
                var legendary = player.LegendaryGear();
                player.ShowLegendaryGear(player);
                dialog.Say(legendary.Count == 0
                    ? "None of the gear you are wearing is Legendary yet."
                    : $"Legendary gear you are wearing: {string.Join(", ", legendary.Select(player.NameOf))}. " +
                      "I have written their powers in your chat. Type /legend any time to see them again.", "Thanks.");
                break;

            case 4:
                if (await dialog.Ask($"Only Super equipment can be forged. A Super Dragon Ball ({Player.DragonBallsPerSuper} Dragon Balls) " +
                           $"has a {Player.SuperForgeChance}% chance and is used up either way. A Legend Orb " +
                           $"({Player.SupersPerOrb} Super Dragon Balls + {Player.ShardsPerOrb} Dragon Shards) always succeeds. " +
                           "Wear the item you want forged when you come to me.", "What powers does Legendary gear have?", "I see.") == 0)
                {
                    dialog.Say("Each kind of item has its own power: weapons make foes bleed, rings drain life, boots turn aside a " +
                               "player's blow, shields blunt magic, and every class's armor and headgear has a power of its own. " +
                               "On top of that, every Legendary item rolls two random attributes, and a lucky roll is Perfect or even Godly. " +
                               $"I can reforge them for {Player.ReforgeShards} Dragon Shards. Type /legend to read what yours do.", "Amazing!");
                }
                break;

            case 5:
                await Reforge();
                break;
        }

        async Task Reforge()
        {
            var gear = player.LegendaryGear();

            if (gear.Count == 0)
            {
                dialog.Say("You are not wearing any Legendary gear. Wear the item whose attributes you want reforged.", "I see.");
                return;
            }

            var pick = await dialog.Ask($"Every Legendary item carries two random attributes. For {Player.ReforgeShards} Dragon Shards " +
                                        "I will reforge both of them; its Legendary power stays the same. Which item?",
                [..gear.Select(player.NameOf), "Never mind."]);

            if (pick < 0 || pick >= gear.Count)
            {
                return;
            }

            switch (player.Reforge(gear[pick]))
            {
                case Player.ReforgeResult.Reforged:
                    dialog.Say($"Done. Your {player.NameOf(gear[pick])} now has: {string.Join(", ", Player.AffixesOf(gear[pick]))}.", "Thank you!");
                    break;

                case Player.ReforgeResult.NoShards:
                    dialog.Say($"Reforging takes {Player.ReforgeShards} Dragon Shards. Bosses drop them.", "I see.");
                    break;

                default:
                    dialog.Say("That item is not Legendary.", "I see.");
                    break;
            }
        }

        async Task Forge()
        {
            var gear = player.ForgeableGear();

            if (gear.Count == 0)
            {
                dialog.Say("You are not wearing any Super equipment that can still be forged.", "I see.");
                return;
            }

            var pick = await dialog.Ask("Which item should I forge?", [..gear.Select(player.NameOf), "Never mind."]);

            if (pick < 0 || pick >= gear.Count)
            {
                return;
            }

            var item = gear[pick];
            var material = await dialog.Ask($"How should I forge your {player.NameOf(item)}?",
                $"Super Dragon Ball ({Player.SuperForgeChance}% chance).", "Legend Orb (always succeeds).", "Never mind.") switch
            {
                0 => Player.SuperDragonBall,
                1 => Player.LegendOrb,
                _ => 0u
            };

            if (material == 0)
            {
                return;
            }

            switch (player.Forge(item, material))
            {
                case Player.ForgeResult.Succeeded:
                    dialog.Say($"It is done. Your {player.NameOf(item)} is now Legendary!", "Thank you!");
                    break;

                case Player.ForgeResult.Failed:
                    dialog.Say("The forging failed and the Super Dragon Ball was consumed. Better luck next time.", "Oh well.");
                    break;

                case Player.ForgeResult.NoMaterial:
                    dialog.Say("You don't have that material with you.", "I see.");
                    break;

                default:
                    dialog.Say("That item can't be forged anymore.", "I see.");
                    break;
            }
        }
    }
}
