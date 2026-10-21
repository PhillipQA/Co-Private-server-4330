namespace Conquer.Game.Scripts.Npcs;

public static class Legendsmith
{
    public const uint NpcId = 10777;

    [Npc(NpcId)]
    public static async Task Talk(Player player, Dialog dialog)
    {
        switch (await dialog.Ask(
            "I am the Legendsmith. With the right materials I can forge Super equipment into Legendary gear, " +
            $"which raises that item's attack, defense, magic and HP by {Player.LegendaryBonusPercent}%.",
            "Forge my gear.", "Make a Super Dragon Ball.", "Make a Legend Orb.", "Which of my gear is Legendary?",
            "How does this work?", "Goodbye."))
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
                dialog.Say(legendary.Count == 0
                    ? "None of the gear you are wearing is Legendary yet."
                    : $"Legendary gear you are wearing: {string.Join(", ", legendary.Select(player.NameOf))}.", "Thanks.");
                break;

            case 4:
                dialog.Say($"Only Super equipment can be forged. A Super Dragon Ball ({Player.DragonBallsPerSuper} Dragon Balls) " +
                           $"has a {Player.SuperForgeChance}% chance and is used up either way. A Legend Orb " +
                           $"({Player.SupersPerOrb} Super Dragon Balls + {Player.ShardsPerOrb} Dragon Shards) always succeeds. " +
                           "Wear the item you want forged when you come to me.", "I see.");
                break;
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
