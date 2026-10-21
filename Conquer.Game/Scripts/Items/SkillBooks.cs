namespace Conquer.Game.Scripts.Items;

public static class SkillBooks
{
    [Item(725000)]
    public static void Thunder(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Spirit < 20)
        {
            dialog.Say("Sorry, you cannot learn this spell before your Spirit is 20. Please train harder.", "I see.");
        }
        else if (player.Knows(1000))
        {
            dialog.Say("You have learned Lightning Bolt.", "I see.");
        }
        else
        {
            player.Learn(1000);

            dialog.Say("You have learned Thunder.", "I see.");
            player.Take(item);
        }
    }

    [Item(725001)]
    public static void Fire(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Spirit < 80)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are spirit 80. Please train harder.", "I see.");
        }
        else if (player.Knows(1001))
        {
            dialog.Say("You have learned Astral Fire bolt.", "I see.");
        }
        else if (player.Knows(1000, 4))
        {
            player.Learn(1001);

            dialog.Say("You have learned Fire.", "I see.");
            player.Take(item);
        }
        else
        {
            dialog.Say("Sorry, you cannot learn this spell before you practice Thunder to level 4.", "I see.");
        }
    }

    [Item(725002)]
    public static void Tornado(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Spirit < 160)
        {
            dialog.Say("Sorry, you cannot learn this spell before your Spirit is 160. Please train harder.", "I see.");
        }
        else if (player.Knows(1002))
        {
            dialog.Say("You have learned Celestial Bolt.", "I see.");
        }
        else if (player.Knows(1001, 3))
        {
            player.Learn(1002);

            dialog.Say("You have learned Tornado.", "I see.");
            player.Take(item);
        }
        else
        {
            dialog.Say("Sorry, you cannot learn this spell before you practice Fire to level 3.", "I see.");
        }
    }

    [Item(725003)]
    public static void Cure(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Spirit < 30)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are spirit 30. Please train harder.", "I see.");
        }
        else if (player.Knows(1005))
        {
            dialog.Say("You have learned Cure.", "I see.");
        }
        else
        {
            player.Learn(1005);

            dialog.Say("You have learned Cure.", "I see.");
            player.Take(item);
        }
    }

    [Item(725004)]
    public static void Lightning(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Spirit < 25)
        {
            dialog.Say("Sorry, you cannot learn this spell before your Spirit is 25. Please train harder.", "I see.");
        }
        else if (player.Knows(1010))
        {
            dialog.Say("You have learned Thunder Strike.", "I see.");
        }
        else
        {
            player.Learn(1010);

            dialog.Say("You have learned Lighting.", "I see.");
            player.Take(item);
        }
    }

    [Item(725005)]
    public static void FastBlade(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (!player.IsSkilled(410, 5))
        {
            dialog.Say("Sorry, you cannot learn this skill before you practice your blade to level 5. Please train harder.",
                "I see.");
            return;
        }

        if (player.Knows(1045))
        {
            dialog.Say("You have learned Fast Attack.", "I see.");
        }
        else
        {
            player.Learn(1045);

            dialog.Say("You have learned Fast Blade.", "I see.");
            player.Take(item);
        }
    }

    [Item(725010)]
    public static void ScentSword(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (!player.IsSkilled(420, 5))
        {
            dialog.Say("Sorry, you cannot learn this skill before you practice your sword to level 5. Please train harder.",
                "I see.");
            return;
        }

        if (player.Knows(1046))
        {
            dialog.Say("You have learned Scent Sword.", "I see.");
        }
        else
        {
            player.Learn(1046);

            dialog.Say("You have learned Scent Sword.", "I see.");
            player.Take(item);
        }
    }

    [Item(725011)]
    public static void WideStrike(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(1250))
        {
            dialog.Say("You have learned Wide Strike.", "I see.");
        }
        else
        {
            player.Learn(1250);

            dialog.Say("You`ve comprehended the elements of WideStrike. As you gain more experiences in using glaive, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725012)]
    public static void SpeedGun(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(1260))
        {
            dialog.Say("You have learned Speed Shot.", "I see.");
        }
        else
        {
            player.Learn(1260);

            dialog.Say("You`ve comprehended the elements of SpeedGun. As you gain more experiences in using spear, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725013)]
    public static void Penetration(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(1290))
        {
            dialog.Say("You have learned Penetration.", "I see.");
        }
        else
        {
            player.Learn(1290);

            dialog.Say("You`ve comprehended the elements of Penetration. As you gain more experiences in using dagger, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725014)]
    public static void Halt(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(1300))
        {
            dialog.Say("You have learned Halt.", "I see.");
        }
        else
        {
            player.Learn(1300);

            dialog.Say("You`ve comprehended the elements of Halt. As you gain more experiences in using longhammer, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725015)]
    public static void DivineHare(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Profession is not (Profession.WaterTaoist or Profession.WaterWizard or Profession.WaterMaster or
            Profession.WaterSaint))
        {
            dialog.Say("Only Water Taoists can learn this spell after they reach level 54.", "I see.");
            return;
        }

        if (player.Level < 54)
        {
            dialog.Say("Only Water Taoists can learn this spell after they reach level 54.", "I see.");
        }
        else if (player.Knows(1350))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1350);

            dialog.Say("You have learned Divine Hare.", "I see.");
            player.Take(item);
        }
    }

    [Item(725016)]
    public static void NightDevil(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 70)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 70.", "I see.");
        }
        else if (player.Knows(1360))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1360);

            dialog.Say("You have learned Night Devil.", "I see.");
            player.Take(item);
        }
    }

    [Item(725018)]
    public static void Dance2(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1380))
        {
            dialog.Say("I have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1380);

            dialog.Say("You have learned Dance 2.", "I see.");
            player.Take(item);
        }
    }

    [Item(725019)]
    public static void Dance3(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1385))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1385);

            dialog.Say("You have learned Dance 3.", "I see.");
            player.Take(item);
        }
    }

    [Item(725020)]
    public static void Dance4(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1390))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1390);

            dialog.Say("You have learned Dance 4.", "I see.");
            player.Take(item);
        }
    }

    [Item(725021)]
    public static void Dance5(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1395))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1395);

            dialog.Say("You have learned Dance 5.", "I see.");
            player.Take(item);
        }
    }

    [Item(725022)]
    public static void Dance6(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1400))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1400);

            dialog.Say("You have learned Dance 6.", "I see.");
            player.Take(item);
        }
    }

    [Item(725023)]
    public static void Dance7(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1405))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1405);

            dialog.Say("You have learned Dance 7.", "I see.");
            player.Take(item);
        }
    }

    [Item(725024)]
    public static void Dance8(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40)
        {
            dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.");
        }
        else if (player.Knows(1410))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1410);

            dialog.Say("You have learned Dance 8.", "I see.");
            player.Take(item);
        }
    }

    [Item(725025)]
    public static void FlyingMoon(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 40 ||
            player.Profession is not (Profession.BrassWarrior or Profession.SilverWarrior or Profession.GoldWarrior or
            Profession.WarriorMaster))
        {
            dialog.Say("Sorry, only Warriors can learn this skill after they are level 40.", "I see.");
            return;
        }

        if (player.Knows(1320))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(1320);

            dialog.Say("You have learned Flying Moon.", "I see.");
            player.Take(item);
        }
    }

    [Item(725026)]
    public static void Snow(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(5010))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(5010);

            dialog.Say("You`ve comprehended the elements of Snow. As you gain more experiences in using wand, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725027)]
    public static void StrandedMonster(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(5020))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(5020);

            dialog.Say("You`ve comprehended the elements of StrandedMonster. As you gain more experiences in using halberd, it`ll be more and more powe",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725028)]
    public static void SpeedLightning(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Level < 70 ||
            player.Profession is not (Profession.FireWizard or Profession.FireMaster or Profession.FireSaint or
            Profession.WaterWizard or Profession.WaterMaster or Profession.WaterSaint))
        {
            dialog.Say("Sorry, only Water or Fire Wizards can learn this spell after they are level 70.", "I see.");
            return;
        }

        if (player.Knows(5001))
        {
            dialog.Say("You have learned this spell.", "I see.");
        }
        else
        {
            player.Learn(5001);

            dialog.Say("You have learned Speed Lightning.", "I see.");
            player.Take(item);
        }
    }

    [Item(725029)]
    public static void Phoenix(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(5030))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(5030);

            dialog.Say("You`ve comprehended the elements of Phoenix. As you gain more experiences in using sword, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725030)]
    public static void Boom(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(5040))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(5040);

            dialog.Say("You`ve comprehended the elements of Boom. As you gain more experiences in using hammer, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725031)]
    public static void Boreas(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(5050))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(5050);

            dialog.Say("You`ve comprehended the elements of Boreas. As you gain more experiences in using poleaxe, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725040)]
    public static void Seizer(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(7000))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(7000);

            dialog.Say("You`ve comprehended the elements of Seizer. As you gain more experiences in using hook, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725041)]
    public static void Earthquake(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(7010))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(7010);

            dialog.Say("You`ve comprehended the elements of Earthquake. As you gain more experiences in using axe, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725042)]
    public static void Rage(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(7020))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(7020);

            dialog.Say("You`ve comprehended the elements of Rage. As you gain more experiences in using club, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725043)]
    public static void Celestial(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(7030))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(7030);

            dialog.Say("You`ve comprehended the elements of Celestial. As you gain more experiences in using scepter, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725044)]
    public static void Roamer(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Knows(7040))
        {
            dialog.Say("You have learned this skill.", "I see.");
        }
        else
        {
            player.Learn(7040);

            dialog.Say("You`ve comprehended the elements of Roamer. As you gain more experiences in using whip, it`ll be more and more powerful.",
                "I see.");
            player.Take(item);
        }
    }

    [Item(725050)]
    public static void HotJuly(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        if (player.Roll(100) < 80)
        {
            if (player.FreeSlots >= 5)
            {
                if (player.Take(725050))
                {
                    player.Give(ItemType.Meteor);
                }
            }
            else
            {
                dialog.Say("Please leave 5 free slots in your inventory to claim the prize.", "I see.");
            }

            return;
        }

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                if (player.Roll(5) < 1)
                {
                    if (player.FreeSlots >= 5)
                    {
                        if (player.Take(725050) && player.Give(ItemType.DragonBall) is not null)
                        {
                            player.World.Announce($"{player.Name} opened a HotJuly and, in a stroke of great luck, a DragonBall fell out! {player.Name} is dancing with joy.",
                                TalkChannel.Gm);
                        }
                    }
                    else
                    {
                        dialog.Say("Please leave 5 free slots in your inventory to claim the prize.", "I see.");
                    }
                }
                else if (player.FreeSlots >= 5)
                {
                    if (player.Take(725050) && player.Give(ItemType.Meteor, 5))
                    {
                        player.World.Announce($"{player.Name} opened a HotJuly and, in a stroke of great luck, 5 Meteors fell out! {player.Name} is dancing with joy.",
                            TalkChannel.Gm);
                    }
                }
                else
                {
                    dialog.Say("Please leave 5 free slots in your inventory to claim the prize.", "I see.");
                }
                break;

            case >= 3:
                if (player.Roll(150) < 1)
                {
                    if (player.FreeSlots >= 5)
                    {
                        if (player.Take(725050) && player.Give(ItemType.DragonBall, 3))
                        {
                            player.World.Announce($"{player.Name} opened a HotJuly and, in a stroke of great luck, 3 DragonBalls fell out! {player.Name} is dancing with joy.",
                                TalkChannel.Gm);
                        }
                    }
                    else
                    {
                        dialog.Say("Please leave 5 free slots in your inventory to claim the prize.", "I see.");
                    }
                }
                else if (player.FreeSlots >= 5)
                {
                    if (player.Take(725050) && player.Give(ItemType.Meteor, 3))
                    {
                        player.World.Announce($"{player.Name} opened a HotJuly and, in a stroke of great luck, 3 Meteors fell out! {player.Name} is dancing with joy.",
                            TalkChannel.Gm);
                    }
                }
                else
                {
                    dialog.Say("Please leave 5 free slots in your inventory to claim the prize.", "I see.");
                }
                break;
        }
    }
}
