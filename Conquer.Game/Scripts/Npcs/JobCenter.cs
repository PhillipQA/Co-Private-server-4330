namespace Conquer.Game.Scripts.Npcs;

public static class JobCenter
{
    [Npc(30)]
    public static async Task Taoist(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        switch (await dialog.Ask("After you learn spells, you may cast spells to kill enemies from a distance.",
            "Will you teach me?", "Learn advanced spells.", "Goodbye."))
        {
            case 0:
                if (!player.Knows(1001) &&
                    player.Profession is not (Profession.InternTaoist or Profession.Taoist or Profession.WaterTaoist or
                    Profession.WaterWizard or Profession.WaterMaster or Profession.WaterSaint or
                    Profession.FireTaoist or Profession.FireWizard or Profession.FireMaster or Profession.FireSaint))
                {
                    dialog.Avatar = 0;

                    dialog.Say("Sorry, only Taoists can learn these spells.", "I see. Thanks.");
                    return;
                }

                switch (await dialog.Ask("You are qualified to learn Thunder and cure. Which do you want to learn?",
                    "Thunder.", "Cure.", "Learn meditation.", "Learn fire."))
                {
                    case 0:
                        await Thunder();
                        break;

                    case 1:
                        await Cure();
                        break;

                    case 2:
                        await LearnMeditation();
                        break;

                    case 3:
                        await LearnFire();
                        break;
                }
                break;

            case 1:
                if (player.Level < 80 ||
                    player.Profession is not (Profession.FireWizard or Profession.FireMaster or Profession.FireSaint))
                {
                    dialog.Say("Sorry, you have to be a Fire Wizard over level 80 to learn that!", "I see.");
                    return;
                }

                if (player.Knows(1002))
                {
                    dialog.Say("You have learned Tornado.", "Thank you.");
                }
                else if (player.Knows(1001, 3))
                {
                    if (await dialog.Ask("Wow, you have grown in wisdom. Would like to learn a new spell?",
                        "Thanks.") == 0)
                    {
                        await Thanks();
                    }
                }
                else
                {
                    dialog.Say("If you want to learn Tornado, you should practice Fire to level 3.", "I see.");
                }
                break;
        }

        async Task Thunder()
        {
            if (player.Knows(1000))
            {
                dialog.Avatar = 6;

                dialog.Say("You have learned Thunder. Work hard so you can learn more spells.", "Thanks.");
            }
            else
            {
                player.Learn(1000);

                dialog.Avatar = 0;

                dialog.Say("You have learned the elementary spells. You must train them to become powerful!",
                    "Thanks, sir.");
            }
        }

        async Task Cure()
        {
            if (player.Knows(1005))
            {
                dialog.Avatar = 6;

                dialog.Say("You have learned Cure. Help others and you will grow strong.", "Thanks, sir.");
            }
            else
            {
                player.Learn(1005);

                dialog.Avatar = 0;

                dialog.Say("You have learned Cure. Help others and you will grow strong.", "Thanks, sir.");
            }
        }

        async Task LearnMeditation()
        {
            if (player.Level < 44)
            {
                dialog.Avatar = 0;

                dialog.Say("Sorry, only Water and Fire Taoists can learn this spell after they are level 44.",
                    "I See.");
            }
            else if (player.Knows(1195))
            {
                dialog.Avatar = 6;

                dialog.Say("Congrats! You have learned this spell.", "Thanks.");
            }
            else if (player.Profession is Profession.FireTaoist or Profession.FireWizard or Profession.FireMaster or
                Profession.FireSaint or Profession.WaterTaoist or Profession.WaterWizard or Profession.WaterMaster or
                Profession.WaterSaint)
            {
                player.Learn(1195);

                dialog.Avatar = 6;

                dialog.Say("Congrats! You have learned meditation.", "Thanks.");
            }
            else
            {
                dialog.Avatar = 0;

                dialog.Say("Sorry, only Fire and Water Taoist can learn this spell.", "I see.");
            }
        }

        async Task LearnFire()
        {
            if (player.Profession is Profession.FireTaoist or Profession.FireWizard or Profession.FireMaster)
            {
                if (!player.Knows(1000, 4))
                {
                    dialog.Avatar = 6;

                    dialog.Say("Sorry, only Fire Taoists can learn this spell after they practice Thunder to level 4.",
                        "I see.");
                    return;
                }

                dialog.Avatar = 6;

                if (await dialog.Ask("You can learn Fire now. Would you like me to teach you?", "Yes, please.") == 0)
                {
                    await YesPlease();
                }
            }
            else if (player.Profession == Profession.FireSaint && player.Knows(1000, 4))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask("You can learn Fire now. Would you like me to teach you?", "Yes, please.") == 0)
                {
                    await YesPlease();
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, only Fire Taoists can learn this spell after they practice Thunder to level 4.",
                    "I see.");
            }
        }

        async Task YesPlease()
        {
            player.Learn(1001);

            dialog.Avatar = 6;

            dialog.Say("You have learned Fire. Power comes from training.", "Thanks, sir.");
        }

        async Task Thanks()
        {
            player.Learn(1002);

            dialog.Avatar = 6;

            dialog.Say("You have learned Tornado. It is very powerful. Please make good use of it.", "Thanks, sir.");
        }
    }

    [Npc(400)]
    public static async Task ArcherGod(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        switch (await dialog.Ask("I am the famous Archer God. What can I do for you?", "Learn Archer skills.",
            "Get promoted."))
        {
            case 0:
                switch (await dialog.Ask("I am proficient in all Archer skills. What do you want to learn?",
                    "ElementaryFly[Level 15]", "Scatter[Level 23]", "RapidFire[Level 46]", "Improved Fly[Level 70]",
                    "ArrowRain[Level 70]", "Intensify[Level 71]", "Advanced Fly[Level 100]"))
                {
                    case 0:
                        if (player.Profession is not (Profession.Archer or Profession.EagleArcher or
                            Profession.TigerArcher or Profession.DragonArcher or Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, Only Archers can learn this skill after they reach level 15.", "I see.");
                            return;
                        }

                        if (player.Level < 15)
                        {
                            dialog.Say("Sorry, Only Archers can learn this skill after they reach level 15.", "I see.");
                        }
                        else if (player.Learn(8002))
                        {
                            dialog.Say("Congrats! You have learned Junior Fly. This spell can make you dodge the physical attack.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        break;

                    case 1:
                        if (player.Profession is not (Profession.Archer or Profession.EagleArcher or
                            Profession.TigerArcher or Profession.DragonArcher or Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, only Archers can learn this skill after they reach level 23.", "I see.");
                            return;
                        }

                        if (player.Level < 23)
                        {
                            dialog.Say("Sorry, only Archers can learn this skill after they reach level 23.", "I see.");
                        }
                        else if (player.Learn(8001))
                        {
                            dialog.Say("Congrats! You have learned Scatter.", "Thanks.");
                        }
                        else
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        break;

                    case 2:
                        if (player.Profession is not (Profession.EagleArcher or Profession.TigerArcher or
                            Profession.DragonArcher or Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, Only Eagle Archers can learn this spell after they reach level 46.",
                                "I see.");
                            return;
                        }

                        if (player.Level < 46)
                        {
                            dialog.Say("You can learn it after you reach level 46.", "I see.");
                        }
                        else if (player.Learn(8000))
                        {
                            dialog.Say("Congrats! You have learned Rapid Fire.", "Thanks.");
                        }
                        else
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        break;

                    case 3:
                        if (player.Profession is not (Profession.TigerArcher or Profession.DragonArcher or
                            Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, only Tiger Archers can learn this skill after they reach level 70.",
                                "I see.");
                            return;
                        }

                        if (player.Level < 70)
                        {
                            dialog.Say("Sorry, only Tiger Archers can learn this skill after they reach level 70.",
                                "I see.");
                        }
                        else if (player.Knows(8002, 0))
                        {
                            if (player.Learn(8003))
                            {
                                dialog.Say("Congrats! You have learned Improved Fly.", "Thanks.");
                            }
                            else
                            {
                                dialog.Say("You have learned this skill.", "I see.");
                            }
                        }
                        else if (player.Knows(8003))
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        else if (player.Knows(8004))
                        {
                            dialog.Say("Sorry, you have learned advanced Fly, you need not learn Improved Fly.",
                                "I see.");
                        }
                        else
                        {
                            dialog.Say("Sorry, please learn Junior Fly first.", "OK.");
                        }
                        break;

                    case 4:
                        if (player.Profession is not (Profession.TigerArcher or Profession.DragonArcher or
                            Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, only Tiger Archers can learn this skill after they reach level 70.",
                                "I see.");
                            return;
                        }

                        if (player.Level < 70)
                        {
                            dialog.Say("Sorry, only Tiger Archers can learn this skill after they reach level 70.",
                                "I see.");
                        }
                        else if (player.Learn(8030))
                        {
                            dialog.Say("Congrats! You have learned Arrow Rain.", "Thanks.");
                        }
                        else
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        break;

                    case 5:
                        if (player.Profession is not (Profession.TigerArcher or Profession.DragonArcher or
                            Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, only Tiger Archers can learn this skill after they reach level 71.",
                                "I see.");
                            return;
                        }

                        if (player.Level < 71)
                        {
                            return;
                        }

                        if (player.Learn(9000))
                        {
                            dialog.Say("Congrats! You have learned Intensify.", "Thanks.");
                        }
                        else
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        break;

                    case 6:
                        if (player.Profession is not (Profession.DragonArcher or Profession.ArcherMaster))
                        {
                            dialog.Say("Sorry, only Dragon Archers can learn this skill after they reach level 100.",
                                "I see.");
                            return;
                        }

                        if (player.Level < 100)
                        {
                            dialog.Say("Sorry, only Dragon Archers can learn this skill after they reach level 100.",
                                "I see.");
                        }
                        else if (player.Knows(8003, 1))
                        {
                            dialog.Say("You have learned this skill.", "I see.");
                        }
                        else if (player.Knows(8003, 0))
                        {
                            if (player.LevelUpMagic(8003))
                            {
                                dialog.Say("Congrats! You have learned Advanced Fly.", "Thanks.");
                            }
                            else
                            {
                                dialog.Say("You have learned this skill.", "I see.");
                            }
                        }
                        else if (player.Knows(8002))
                        {
                            dialog.Say("Sorry, please learn Improved Fly first.", "OK.");
                        }
                        else
                        {
                            dialog.Say("Sorry, please learn Junior Fly first.", "OK.");
                        }
                        break;
                }
                break;

            case 1:
                if (player.Profession == Profession.InternArcher)
                {
                    if (player.Level < 15)
                    {
                        dialog.Say("Sorry, you cannot get promoted before you reach level 15. Please train harder.",
                            "I see.");
                        return;
                    }

                    await dialog.Confirm("Archers have the advantage of fast speed and distance shoot. They can dodge physical attack while they are flying.",
                        "Get promoted.", "Let me think it over.");

                    if (player.Strength < 12 || player.Agility < 35 || player.Profession != Profession.InternArcher)
                    {
                        dialog.Say("Sorry, only Intern Archers can be promoted to Archer. Strength 12 and agility 35 are required. Please consult other people.",
                            "I see.");
                        return;
                    }

                    if (!player.SetProfession(Profession.Archer))
                    {
                        return;
                    }

                    player.Learn(8002);

                    if (player.Give(133305) is not null)
                    {
                        dialog.Say("Congrats! You are promoted to Archer and have learned flying. When your XP is full, system will notify you to use.",
                            "Thanks.");
                    }
                }
                else if (player.Profession == Profession.Archer)
                {
                    if (player.Level < 40)
                    {
                        dialog.Say("Sorry, you cannot get promoted before you reach level 40. Please train harder.",
                            "I see.");
                    }
                    else if (player.Strength >= 25 && player.Agility >= 90 && player.Profession == Profession.Archer)
                    {
                        await dialog.Confirm("If you give me 5 Euxenite Ores, I can promote you to Eagle Archer and teach you more skills.",
                            "Get promoted.", "Let me think it over.");

                        if (!player.Has(1072031, 5))
                        {
                            dialog.Say("Sorry, you do not have 5 Euxenite Ores. You may dig Euxenite Ores in mine cave.",
                                "I see.");
                            return;
                        }

                        if (player.Take(1072031, 5) && player.SetProfession(Profession.EagleArcher) &&
                            player.Give(500077, 1, item => item.Gem1 = (Gem)255))
                        {
                            dialog.Say("Congrats! You are promoted to Eagle Archer. I rewarded you a unique Horn Bow. Please train hard.",
                                "Thanks.");
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry, only Archer can be promoted to Eagle Archer. Strength 25 and agility 90 are required. Please train harder.",
                            "I see.");
                    }
                }
                else if (player.Profession == Profession.EagleArcher)
                {
                    await dialog.Confirm("Eagle Archers can be promoted to Tiger Archers.", "Get promoted.", "Let me think it over.");

                    await dialog.Confirm("Eagle Archer can be promoted to Tiger Archer after they reach level 70 and have an emerald.",
                        "Ready for promotion.", "I see.");

                    if (player.Level < 70)
                    {
                        dialog.Say("Sorry, you cannot get promoted before you reach level 70. Please train harder.",
                            "I see.");
                    }
                    else if (player.Strength >= 45 && player.Agility >= 150 &&
                        player.Profession == Profession.EagleArcher)
                    {
                        if (!player.Take(1080001))
                        {
                            dialog.Say("Sorry, an emerald is required. Hill Monsters may drop emeralds. Please come to get promoted after you have one.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.TigerArcher))
                        {
                            return;
                        }

                        if (player.Rebirths == 1)
                        {
                            dialog.Say("Congratulations! You have advanced to Tiger Archer. After rebirth, your stamina will counteract 30%  damage.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("Congrats! You are promoted to Tiger Archer. You may come to get promoted after you reach level 100.",
                                "Thanks.");
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry, only Eagle Archer can be promoted to Tiger Archer. Strength 45 and agility 150 are required. Please train harder.",
                            "I see.");
                    }
                }
                else if (player.Profession == Profession.TigerArcher)
                {
                    if (player.Level < 100 || player.Strength < 60 || player.Agility < 215)
                    {
                        dialog.Say("Sorry, only Tiger Archer can be promoted to Dragon Archer. Strength 60 and agility 215 are required. Please train harder.",
                            "I see.");
                    }
                    else
                    {
                        await dialog.Confirm("If you give me a meteor, I can promote you to Dragon Archer.", "Get promoted.");

                        dialog.Require(player.Has("Meteor"));

                        if (player.Rebirths != 0 && player.FreeSlots == 0)
                        {
                            dialog.Say("You have got reborn. I will send you a gift. Please prepare a slot in your inventory for that.",
                                "Ok. Wait a minute.");
                            return;
                        }

                        if (!player.SetProfession(Profession.DragonArcher) || !player.Take(ItemType.Meteor) ||
                            player.Give(700031) is null)
                        {
                            return;
                        }

                        if (player.Rebirths == 0)
                        {
                            dialog.Say("Congratulations! You are promoted to Dragon Archer. I reward you an Rainbow Gem.",
                                "Thanks.");
                        }
                        else if (player.Give(133377, 1, item => item.Gem1 = (Gem)255))
                        {
                            dialog.Say("You are Dragon Archer. Because you have been reborn, I will give you a RainbowGem and a unique socketed equipment.",
                                "Thanks.");
                        }
                    }
                }
                else if (player.Profession == Profession.DragonArcher)
                {
                    if (player.Strength < 68 || player.Agility < 235)
                    {
                        dialog.Say("Sorry, only Dragon Archers can be promoted to Archer Kings after they are level 110, agility 235 and strength 68.",
                            "I see.");
                    }
                    else
                    {
                        await dialog.Confirm("If you give me a Moon Box, I can promote you to Archer King.", "Get promoted.");

                        if (!player.Take("MoonBox"))
                        {
                            dialog.Say("Sorry, you do not have Moon Box. You may obtain one from Eight Diagram Tactics quest. Come to get promoted after you get one.",
                                "I see.");
                            return;
                        }

                        if (player.SetProfession(Profession.ArcherMaster) &&
                            player.Give(ItemType.DragonBall) is not null)
                        {
                            dialog.Say("Congrats! You are promoted to Archer King. I reward you a Dragon Ball.",
                                "Thanks.");
                        }
                    }
                }
                else if (player.Profession == Profession.ArcherMaster)
                {
                    dialog.Say("Congrats! You are promoted to Archer King. Please train hard. I believe you will be a great Archer King.",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, you are not an Archer. I only promote and teach Archers.", "I see.");
                }
                break;
        }
    }

    [Npc(10000)]
    public static async Task TaoistMoon(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Profession == Profession.InternTaoist)
        {
            if (player.Level < 15)
            {
                dialog.Say("Sorry, you must be level 15 to be promoted. Go seek wisdom in battle.", "I see. Thanks.");
                return;
            }

            await dialog.Confirm("Taoist spells make them strong in a group or alone.", "Get promoted.", "Let me think it over.");

            if (player.Spirit < 25 || player.Agility < 10 || player.Profession != Profession.InternTaoist)
            {
                dialog.Say("Sorry! Only intern Taoist can become a Taoist after their Spirit is 25, Agility 10 .",
                    "I see.");
                return;
            }

            if (!player.SetProfession(Profession.Taoist))
            {
                return;
            }

            player.Learn(1010);

            if (player.Give(134303) is not null)
            {
                dialog.Say("Congrats! As a Taoist you have lightning. When your XP is ready you can use it.",
                    "I see. Thanks.");
            }
        }
        else if (player.Profession == Profession.Taoist)
        {
            if (player.Level < 40)
            {
                if (await dialog.Ask("You must attain level 40 to be fire or water Taoist.", "I see.",
                    "Learn XP Skill.") == 1)
                {
                    await LearnXPSkill();
                }
            }
            else if (player.Spirit >= 80 && player.Agility >= 25 && player.Profession == Profession.Taoist)
            {
                switch (await dialog.Ask("Taoist can be promoted to be Fire or Water Taoist. Which do you prefer?",
                    "Fire Taoist", "Water Taoist.", "Learn XP Skills.", "Let me think it over."))
                {
                    case 0:
                        if (player.SetProfession(Profession.FireTaoist))
                        {
                            player.Give(421075);

                            dialog.Say("Congrats! You are promoted to Fire Taoist. Please train hard and come to get promoted after you reach level 70.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        if (player.SetProfession(Profession.WaterTaoist))
                        {
                            player.Give(421075);
                            player.Learn(1050);

                            dialog.Say("Congrats! You are Water Taoist now and have learned revive. When your XP is full, you may cast revive to resurrect a ghost.",
                                "Thanks.");
                        }
                        break;

                    case 2:
                        await LearnXPSkill();
                        break;
                }
            }
            else
            {
                dialog.Say("Sorry, only Taoists can be promoted to Fire or Water Taoists after they are spirit 80, agility 25.",
                    "I see.");
            }
        }
        else if (player.Profession == Profession.FireTaoist)
        {
            switch (await dialog.Ask("Fire Taoists can be promoted to Fire Wizards. If you have not learned XP skills, you can learn it now.",
                "Get promoted again.", "Learn magic.", "Learn XP Skill.", "Let me think it over."))
            {
                case 0:
                    await dialog.Confirm("Fire Taoists can be promoted to Fire Wizard after they reach level 70, for a mere emerald.",
                        "Ready for promotion.", "I see.");

                    if (player.Level < 70)
                    {
                        if (await dialog.Ask("Sorry, your level is too low. Please train harder and come to get promoted after you reach level 70.",
                            "I see.", "Learn XP Skill.") == 1)
                        {
                            await LearnXPSkill();
                        }
                    }
                    else if (player.Spirit >= 140 && player.Agility >= 45 && player.Profession == Profession.FireTaoist)
                    {
                        if (!player.Take(1080001))
                        {
                            dialog.Say("Sorry, an emerald is required for promotion. Hill Monsters may drop emeralds. Please come to get promoted after you have one.",
                                "I see.");
                            return;
                        }

                        if (player.SetProfession(Profession.FireWizard))
                        {
                            player.Give(134365);

                            dialog.Say("Congratulations! You have advanced to Fire Wizard. After rebirth, your stamina will counteract 30%  damage.",
                                "I see.");
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry, only Fire Taoists can be promoted to Fire Wizards after their Spirit is 140, and Agility 45.",
                            "I see.");
                    }
                    break;

                case 1:
                    await LearnMagic();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.FireWizard)
        {
            if (player.Level >= 100 && player.Agility >= 60 && player.Spirit >= 205)
            {
                switch (await dialog.Ask("If you give me a meteor, I can promote you to Fire Master.", "Get promoted.",
                    "Learn magic.", "Learn XP Skill.", "Let me think it over."))
                {
                    case 0:
                        if (player.Rebirths != 0 && player.FreeSlots == 0)
                        {
                            dialog.Say("You have got reborn. I will send you a gift. Please prepare a slot in your inventory for that.",
                                "Ok. Wait a minute.");
                            return;
                        }

                        if (!player.Take(ItemType.Meteor))
                        {
                            dialog.Say("Sorry, you do not have a meteor. Please come to get promoted after you have one.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.FireMaster) || player.Give(700031) is null)
                        {
                            return;
                        }

                        if (player.Rebirths == 0)
                        {
                            dialog.Say("Congrats! You are promoted to Fire Master. I reward you an Rainbow Gem.",
                                "Thanks.");
                        }
                        else if (player.Give(134387, 1, item => item.Gem1 = (Gem)255))
                        {
                            dialog.Say("You are Fire Master. Because you have been reborn, I will give you a RainbowGem and a unique socketed equipment.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnMagic();
                        break;

                    case 2:
                        await LearnXPSkill();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Only Fire Wizards that reach level 100, spirit 205, agility 60 can be Fire Masters!",
                "I see.", "Learn magic.", "Learn XP Skill.", "Let me think it over."))
            {
                case 1:
                    await LearnMagic();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.FireMaster)
        {
            if (player.Level >= 110 && player.Agility >= 65 && player.Spirit >= 225)
            {
                switch (await dialog.Ask("If you give me a Moon Box, I can promote you to Fire Saint.", "Get promoted.",
                    "Learn magic.", "Learn XP Skill.", "Let me think it over."))
                {
                    case 0:
                        if (!player.Take("MoonBox"))
                        {
                            dialog.Say("Sorry, you do not have a Moon Box. You may obtain a Moon Box from Eight Diagram Tactics quest.",
                                "I see.");
                            return;
                        }

                        if (player.SetProfession(Profession.FireSaint) && player.Give(ItemType.DragonBall) is not null)
                        {
                            dialog.Say("Congrats! You are promoted to Fire Saint. I reward you a Dragon Ball.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnMagic();
                        break;

                    case 2:
                        await LearnXPSkill();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Sorry, you cannot be promoted to Fire Saint before you are level 110, agility 65 and spirit 225.",
                "I see.", "Learn magic.", "Learn XP Skill.", "Let me think it over."))
            {
                case 1:
                    await LearnMagic();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.FireSaint)
        {
            switch (await dialog.Ask("You have been a Fire Saint. Please train harder. I believe you will be a great Saint.",
                "Thanks.", "Learn magic.", "Learn XP Skill.", "Let me think it over."))
            {
                case 1:
                    await LearnMagic();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.WaterTaoist)
        {
            switch (await dialog.Ask("Water Taoist can be promoted to Water Wizard. Do you want to get promoted? If you have not learned XP skill, you may learn now.",
                "Get promoted.", "Learn Water spells.", "Learn XP Skill.", "Let me think it over."))
            {
                case 0:
                    await dialog.Confirm("After Water Taoists reach level 70, they can be promoted to Water Wizard. An emerald will be charged for promotion.",
                        "Ready for promotion.", "I see.");

                    if (player.Level < 70)
                    {
                        if (await dialog.Ask("Sorry, you cannot get promoted before you reach level 70. Please train harder.",
                            "I see.", "Learn XP skills.") == 1)
                        {
                            await LearnXPSkill();
                        }
                    }
                    else if (player.Spirit >= 140 && player.Agility >= 45 &&
                        player.Profession == Profession.WaterTaoist)
                    {
                        if (!player.Take(1080001))
                        {
                            dialog.Say("Sorry, an emerald is required for promotion. Hill Monsters may drop emeralds. Please come to get promoted after you have one.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.WaterWizard))
                        {
                            return;
                        }

                        player.Give(134365);

                        if (player.Rebirths == 1)
                        {
                            dialog.Say("Congratulations! You have advanced to Water Wizard. After rebirth, your stamina will counteract 30%  damage.",
                                "I see.");
                        }
                        else
                        {
                            dialog.Say("Congrats! You are promoted to Water Wizard. Please train hard and come to get promoted after you reach level 100.",
                                "I see.");
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry, only Water Taoist can be promoted to Water Wizard. Spirit 140 and agility 45 are required. Please train harder.",
                            "I see.");
                    }
                    break;

                case 1:
                    await LearnWaterSpells();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.WaterWizard)
        {
            if (player.Level >= 100 && player.Spirit >= 205 && player.Agility >= 60)
            {
                switch (await dialog.Ask("If you give me a meteor, I can promote you to Water Master.", "Get promoted.",
                    "Learn Water spells.", "Learn XP Skill.", "Let me think it over."))
                {
                    case 0:
                        if (player.Rebirths != 0 && player.FreeSlots == 0)
                        {
                            dialog.Say("You have got reborn. I will send you a gift. Please prepare a slot in your inventory for that.",
                                "Ok. Wait a minute.");
                            return;
                        }

                        if (!player.Take(ItemType.Meteor))
                        {
                            dialog.Say("Sorry, you do not have a meteor. You may come to get promoted after you have a meteor.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.WaterMaster) || player.Give(700031) is null)
                        {
                            return;
                        }

                        if (player.Rebirths == 0)
                        {
                            dialog.Say("Congratulations! You are promoted to Water Master. I reward you a Rainbow Gem.",
                                "Thanks.");
                        }
                        else if (player.Give(134387, 1, item => item.Gem1 = (Gem)255))
                        {
                            dialog.Say("You are Water Master. Because you have been reborn, I will give you a RainbowGem and a unique socketed equipment.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnWaterSpells();
                        break;

                    case 2:
                        await LearnXPSkill();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Sorry, you can be promoted to Water Master only after you reach level 100, spirit 205, agility 60. Please train harder.",
                "I see.", "Learn Water spells.", "Learn XP Skill.", "Let me think it over."))
            {
                case 1:
                    await LearnWaterSpells();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.WaterMaster)
        {
            if (player.Level >= 110 && player.Agility >= 65 && player.Spirit >= 225)
            {
                switch (await dialog.Ask("If you give me a Moon Box, I can promote you to Water Saint.",
                    "Get promoted.", "Learn Water spells.", "Learn XP Skill.", "Let me think it over."))
                {
                    case 0:
                        if (!player.Take("MoonBox") || !player.SetProfession(Profession.WaterSaint))
                        {
                            dialog.Say("Sorry, you do not have a Moon Box. You may obtain a Moon Box from Eight Diagram Tactics quest.",
                                "I see.");
                            return;
                        }

                        if (player.Give(ItemType.DragonBall) is not null)
                        {
                            dialog.Say("Congrats! You are promoted to Water Saint. I reward you a Dragon Ball.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnWaterSpells();
                        break;

                    case 2:
                        await LearnXPSkill();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Sorry, you cannot be promoted to Water Saint before you are level 110, aglity 65, and spirit 225.",
                "I see.", "Learn Water spells.", "Learn XP Skill.", "Let me think it over."))
            {
                case 1:
                    await LearnWaterSpells();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else if (player.Profession == Profession.WaterSaint)
        {
            switch (await dialog.Ask("You have been a Water Saint. Please train harder. I believe you will be a great Saint.",
                "Thanks.", "Learn Water spells.", "Learn XP Skill.", "Let me think it over."))
            {
                case 1:
                    await LearnWaterSpells();
                    break;

                case 2:
                    await LearnXPSkill();
                    break;
            }
        }
        else
        {
            dialog.Say("Sorry. You are not a Taoist. Please consult others.", "I see.");
        }

        async Task LearnXPSkill()
        {
            switch (await dialog.Ask("Lighting can hit enemies within a certain range. Water Taoists can learn Water Elf to disguise themselves. Which do you prefer?",
                "Lightning.", "Volcano.", "Water Elf.", "Learn XP Revive.", "I do not want to learn."))
            {
                case 0:
                    if (player.Knows(1010))
                    {
                        dialog.Say("You have learned this skill.", "Thanks.");
                    }
                    else
                    {
                        player.Learn(1010);

                        dialog.Say("Congrats! You have learned lightning. System will notify you to use when your XP is full.",
                            "Thanks.");
                    }
                    break;

                case 1:
                    if (player.Level < 40)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach level 40.", "I see.");
                    }
                    else
                    {
                        player.Learn(1125);

                        dialog.Say("Congrats! You have learned Volcano.", "Thanks.");
                    }
                    break;

                case 2:
                    if (player.Profession is not (Profession.WaterTaoist or Profession.WaterWizard or
                        Profession.WaterMaster or Profession.WaterSaint))
                    {
                        dialog.Say("Only Water Taoist can learn Water Elf.", "I see.");
                        return;
                    }

                    if (player.Level < 50)
                    {
                        dialog.Say("Sorry, you cannot learn Water Elf before you are level 50. Please train harder.",
                            "I see.");
                    }
                    else if (player.Knows(1280))
                    {
                        dialog.Say("You have learned this spell before.", "I see.");
                    }
                    else
                    {
                        player.Learn(1280);

                        dialog.Say("Congrats! You have learned Water Elf.", "Thanks.");
                    }
                    break;

                case 3:
                    if (player.Profession is not (Profession.WaterTaoist or Profession.WaterWizard or
                        Profession.WaterMaster or Profession.WaterSaint))
                    {
                        dialog.Say("Only Water Taoist above level 40 can learn Revive.", "I See.");
                        return;
                    }

                    if (player.Level < 40)
                    {
                        dialog.Say("Only Water Taoist above level 40 can learn Revive.", "I See.");
                    }
                    else
                    {
                        player.Learn(1050);

                        dialog.Say("You have learned Revive.", "Thanks.");
                    }
                    break;
            }
        }

        async Task LearnMagic()
        {
            switch (await dialog.Ask("What spell do you want to learn?", "Fire Ring.", "Fire Circle.", "FireMeteor."))
            {
                case 0:
                    if (player.Level < 50)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach level 50.", "I see.");
                    }
                    else
                    {
                        player.Learn(1150);

                        dialog.Say("Congrats! You have learned Fire Ring.", "Thanks.");
                    }
                    break;

                case 1:
                    if (player.Level < 65)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach level 65.", "I see.");
                    }
                    else
                    {
                        player.Learn(1120);

                        dialog.Say("Congrats! You have learned Fire Circle.", "Thanks.");
                    }
                    break;

                case 2:
                    if (player.Level < 52)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach level 52.", "I see.");
                    }
                    else
                    {
                        player.Learn(1180);

                        dialog.Say("Congrats! You have learned Fire Meteor.", "Thanks.");
                    }
                    break;
            }
        }

        async Task LearnWaterSpells()
        {
            var choice = await dialog.Ask("Water Taoist can learn many spells. You must reach the required level before you learn. What would you like to learn?",
                "Healing Rain[level 40]", "Invisibility[level 60]", "Star of Accuracy[level 45]",
                "Magic Shield[level 50]", "Stigma[level 55]", "Pray[Level 70]", "Advanced Cure[Level 81]",
                "Nectar[level 94]");

            switch (choice)
            {
                case <= 4 or 6 or 7:
                    int value = choice switch { 0 => 40, 1 => 60, 2 => 45, 3 => 50, 4 => 55, 6 => 81, _ => 94 };
                    ushort magicType = choice switch { 0 => 1055, 1 => 1075, 2 => 1085, 3 => 1090, 4 => 1095, 6 => 1175,
                        _ => 1170 };

                    if (player.Level < value)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach the required level. Please train harder.",
                            "I see.");
                    }
                    else if (player.Knows(magicType))
                    {
                        dialog.Say("You have learned this spell.", "I see.");
                    }
                    else
                    {
                        player.Learn(magicType);

                        dialog.Say("Congrats! You have learned the spell. The power of water exists everywhere.",
                            "Thanks.");
                    }
                    break;

                case 5:
                    if (player.Level < 70)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach the required level. Please train harder.",
                            "I see.");
                    }
                    else if (player.Knows(1100))
                    {
                        dialog.Say("You have learned this spell.", "I see.");
                    }
                    else
                    {
                        player.Learn(1100);

                        dialog.Say("Congrats! You have learned this spell.", "Thanks.");
                    }
                    break;
            }
        }
    }

    [Npc(10001)]
    public static async Task WarriorGod(Player player, Dialog dialog)
    {
        dialog.Avatar = 8;

        if (player.Profession == Profession.InternWarrior)
        {
            if (player.Level < 15)
            {
                dialog.Say("Sorry, you cannot get promoted before you reach level 15. Please train harder.", "I see.");
                return;
            }

            await dialog.Confirm("Warriors are good at attack and defense. Their physical defense is the strongest. Only warriors can wear shields and helmets.",
                "Get promoted.", "Let me think it over.");

            if (player.Strength < 28 || player.Agility < 10 || player.Vitality < 10 ||
                player.Profession != Profession.InternWarrior)
            {
                dialog.Say("Sorry, Intern Warriors can be promoted to Warriors after their Strength is 28, Agility is 10 and Vitality is 10.",
                    "I see.");
                return;
            }

            if (!player.SetProfession(Profession.Warrior))
            {
                return;
            }

            player.Learn(1015);
            player.Learn(1020);
            player.Learn(1025);
            player.Learn(1040);

            if (player.Give(131303) is not null)
            {
                dialog.Say("Congrats! You are promoted to Warrior and have learned XP skills. System will notify you when your XP is full.",
                    "Thanks.");
            }
        }
        else if (player.Profession == Profession.Warrior)
        {
            if (player.Level < 40)
            {
                switch (await dialog.Ask("Sorry, you cannot get promoted before you reach level 40. Please train harder.",
                    "I see.", "Learn XP Skill.", "Learn weapon skills."))
                {
                    case 1:
                        await LearnXPSkill();
                        break;

                    case 2:
                        if (player.IsSkilled(410, 5))
                        {
                            if (player.Learn(1045))
                            {
                                if (player.IsSkilled(420, 5))
                                {
                                    player.Learn(1046);

                                    dialog.Say("Congrats! You have learned fast blade and scent sword.", "Thanks.");
                                }
                                else
                                {
                                    dialog.Say("Congrats! You have learned fast blade.", "Thanks.");
                                }
                            }
                            else if (player.IsSkilled(420, 5))
                            {
                                player.Learn(1046);

                                dialog.Say("Congrats! You have learned scent sword.", "Thanks.");
                            }
                            else
                            {
                                dialog.Say("I can teach you fast blade and scent sword. Please come to learn after your blade or sword reaches level 5.",
                                    "Ok.");
                            }
                        }
                        else if (player.IsSkilled(420, 5))
                        {
                            player.Learn(1046);

                            dialog.Say("Congrats! You have learned scent sword.", "Thanks.");
                        }
                        else
                        {
                            dialog.Say("I can teach you fast blade and scent sword. Please come to learn after your blade or sword reaches level 5.",
                                "Ok.");
                        }
                        break;
                }
            }
            else if (player.Strength >= 80 && player.Agility >= 25 && player.Profession == Profession.Warrior)
            {
                switch (await dialog.Ask("Warrior can be promoted to Brass Warrior. Are you here for promotion? If you haven`t learned XP skills, you can learn them now.",
                    "Get promoted.", "Learn XP Skills.", "Let me think it over."))
                {
                    case 0:
                        if (player.SetProfession(Profession.BrassWarrior) && player.Give(900305) is not null)
                        {
                            dialog.Say("Congrats! You are promoted to Brass Warrior. Please train hard and come to get promoted after you reach level 70.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnXPSkill();
                        break;
                }
            }
            else
            {
                dialog.Say("Sorry, only Warrior can be promoted to Brass Warrior after their Strength is 80 and Agility 25.",
                    "I see.");
            }
        }
        else if (player.Profession == Profession.BrassWarrior)
        {
            switch (await dialog.Ask("Brass Warriors can be promoted to Silver Warriors. If you have not learned XP skills, you can learn now.",
                "Get promoted again.", "Learn XP skills.", "Learn Dash.", "Let me think it over."))
            {
                case 0:
                    await dialog.Confirm("After Brass Warriors reach level 70, they can be promoted to Silver Warriors for the mere price of an emerald.",
                        "Ready for promotion.", "I see.");

                    if (player.Level < 70)
                    {
                        if (await dialog.Ask("Sorry, you cannot get promoted before you reach level 70. Please train harder.",
                            "I see.", "Learn XP skills.") == 1)
                        {
                            await LearnXPSkill();
                        }
                    }
                    else if (player.Strength >= 140 && player.Agility >= 45 &&
                        player.Profession == Profession.BrassWarrior)
                    {
                        if (!player.Take(1080001))
                        {
                            dialog.Say("Sorry, an emerald is required for promotion. Hill Monsters may drop emeralds. Please come to get promoted after you have one.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.SilverWarrior))
                        {
                            return;
                        }

                        player.Give(131365);

                        if (player.Rebirths == 1)
                        {
                            dialog.Say("Congratulations! You have advanced to Silver Warrior. After rebirth, your stamina will counteract 30%  damage.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("Congrats! You have been promoted to Silver Warrior. Please train hard and come to get promoted after you reach level 100.",
                                "Thanks.");
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry, only Brass Warriors can be promoted to Silver Warriors. Strength 140 and Agility 45 are required. Please train harder.",
                            "I see.");
                    }
                    break;

                case 1:
                    await LearnXPSkill();
                    break;

                case 2:
                    await LearnDash();
                    break;
            }
        }
        else if (player.Profession == Profession.SilverWarrior)
        {
            if (player.Level >= 100 && player.Strength >= 205 && player.Agility >= 60)
            {
                switch (await dialog.Ask("If you give me a meteor, I can promote you to Gold Warrior.", "Get promoted.",
                    "Learn XP skills.", "Learn Dash.", "Let me think it over."))
                {
                    case 0:
                        if (player.Rebirths != 0 && player.FreeSlots == 0)
                        {
                            dialog.Say("You have got reborn. I will send you a gift. Please prepare a slot in your inventory for that.",
                                "Ok. Wait a minute.");
                            return;
                        }

                        if (!player.Take(ItemType.Meteor))
                        {
                            dialog.Say("Sorry, you do not have a meteor. You may come to get promoted after you have a meteor.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.GoldWarrior) || player.Give(700031) is null)
                        {
                            return;
                        }

                        if (player.Rebirths == 0)
                        {
                            dialog.Say("Congratulations! You are promoted to Gold Warrior. I reward you a Rainbow Gem.",
                                "Thanks.");
                        }
                        else if (player.Give(131387, 1, item => item.Gem1 = (Gem)255))
                        {
                            dialog.Say("You are Gold Warrior. Because you have been reborn, I will give you a RainbowGem and a unique socketed equipment.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnXPSkill();
                        break;

                    case 2:
                        await LearnDash();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("You can be promoted to Gold Warrior after you are level 100, strength 205, agility 60.\nSorry, you do not meet the requirements. Please train harder and come to get promoted later.",
                "OK.", "Learn XP skills.", "Learn Dash.", "Let me think it over."))
            {
                case 1:
                    await LearnXPSkill();
                    break;

                case 2:
                    await LearnDash();
                    break;
            }
        }
        else if (player.Profession == Profession.GoldWarrior)
        {
            if (player.Level >= 110 && player.Strength >= 225 && player.Agility >= 65)
            {
                switch (await dialog.Ask("If you give me a Moon Box, I may promote you to Warrior King.",
                    "Get promoted.", "Learn XP skills.", "Learn Dash.", "Let me think it over."))
                {
                    case 0:
                        if (!player.Take("MoonBox"))
                        {
                            dialog.Say("Sorry, you do not have a Moon Box. You may obtain a Moon Box from Eight Diagram Tactics quest.",
                                "I see.");
                            return;
                        }

                        if (player.SetProfession(Profession.WarriorMaster) &&
                            player.Give(ItemType.DragonBall) is not null)
                        {
                            dialog.Say("Congrats! You are promoted to Warrior King. I reward you a Dragon Ball.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnXPSkill();
                        break;

                    case 2:
                        await LearnDash();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Sorry, you cannot be promoted to Warrior King before you are level 110, strength 225, and agility 65.",
                "I see.", "Learn XP skills.", "Learn Dash.", "Let me think it over."))
            {
                case 1:
                    await LearnXPSkill();
                    break;

                case 2:
                    await LearnDash();
                    break;
            }
        }
        else if (player.Profession == Profession.WarriorMaster)
        {
            switch (await dialog.Ask("You have been a Warrior King. Please train harder. I believe you will be a great King.",
                "Thanks.", "Learn XP skills.", "Learn Dash.", "Let me think it over."))
            {
                case 1:
                    await LearnXPSkill();
                    break;

                case 2:
                    await LearnDash();
                    break;
            }
        }
        else
        {
            dialog.Say("Sorry. You are not a warrior. Please consult others.", "I see.");
        }

        async Task LearnXPSkill()
        {
            await dialog.Confirm("Accuracy triples hit rate, Shield triples defense, Superman decuples attack, and Roar adds XP to the teammates.",
                "I want to learn.", "I do not care.");

            if (player.Knows(1040))
            {
                dialog.Say("You have learned two skills.", "Thanks.");
            }
            else
            {
                player.Learn(1015);
                player.Learn(1020);
                player.Learn(1025);
                player.Learn(1040);

                dialog.Say("Congrats! You have learned Accuracy, Shield, Superman and Defense. System will notify you when your XP is full.",
                    "Thanks.");
            }
        }

        async Task LearnDash()
        {
            if (player.Level < 63)
            {
                dialog.Say("Only Warriors can learn this skill after they reach level 63.", "Thanks.");
            }
            else
            {
                player.Learn(1051);

                dialog.Say("Congrats! You have learned Dash.", "I see.");
            }
        }
    }

    [Npc(10022)]
    public static async Task TrojanStar(Player player, Dialog dialog)
    {
        dialog.Avatar = 5;

        if (player.Profession == Profession.InternTrojan)
        {
            if (player.Level < 15)
            {
                dialog.Say("Sorry, you cannot get promoted before you reach level 15. Please train harder.", "I see.");
                return;
            }

            await dialog.Confirm("Trojan has more HP than any other class. Only Trojan can use two handed weapons which will give much more damage to enemies.",
                "Get promoted.", "Let me think it over.");

            if (player.Strength < 28 || player.Agility < 10 || player.Vitality < 10 ||
                player.Profession != Profession.InternTrojan)
            {
                dialog.Say("Sorry, Intern Trojans can be promoted to Trojans after their Strength is 28, Agility is 10 and Vitality is 10.",
                    "I see.");
                return;
            }

            if (player.SetProfession(Profession.Trojan))
            {
                player.Learn(1015);

                dialog.Say("Congrats! You are promoted to Trojan and have learned XP skills, system will notify you to use XP skills when your XP is full.",
                    "Thanks.");
            }
        }
        else if (player.Profession == Profession.Trojan)
        {
            if (player.Level < 40)
            {
                switch (await dialog.Ask("Sorry, you cannot get promoted before you are level 40. Please train harder.",
                    "I see.", "Learn skills.", "Learn XP skills."))
                {
                    case 1:
                        await LearnSkills();
                        break;

                    case 2:
                        await LearnXPSkills();
                        break;
                }
            }
            else if (player.Strength < 60 || player.Agility < 25 || player.Vitality < 25)
            {
                dialog.Say("To be promoted to Veteran Trojan, you should be strength 60, agility 25 and stamina 25. Please train harder.",
                    "I see.");
            }
            else
            {
                switch (await dialog.Ask("Trojan can be promoted to Veteran Trojan. Do you want to get promoted? If you have not learned XP skills, you may learn now.",
                    "Get promoted.", "Learn skills.", "Learn XP skills.", "Let me think it over."))
                {
                    case 0:
                        if (player.SetProfession(Profession.VeteranTrojan))
                        {
                            player.Give(410075);

                            dialog.Say("Congrats! You are promoted to Veteran Trojan. Please train hard and come to get promoted after you reach level 70.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnSkills();
                        break;

                    case 2:
                        await LearnXPSkills();
                        break;
                }
            }
        }
        else if (player.Profession == Profession.VeteranTrojan)
        {
            switch (await dialog.Ask("Veteran Trojan can be promoted to Tiger Trojan. Do you wanna get promoted? If you have not learned XP skill, you may learn now.",
                "Get promoted.", "Learn skills.", "Learn XP skills.", "Let me think it over."))
            {
                case 0:
                    await dialog.Confirm("After Veteran Trojans reach level 70, they can be promoted to Tiger Trojan. An emerald will be charged for promotion.",
                        "Ready for promotion.", "I see.");

                    if (player.Level < 70)
                    {
                        switch (await dialog.Ask("Sorry, you cannot get promoted before you reach level 70. Please train harder.",
                            "I see.", "Learn skills.", "Learn XP skills."))
                        {
                            case 1:
                                await LearnSkills();
                                break;

                            case 2:
                                await LearnXPSkills();
                                break;
                        }
                    }
                    else if (player.Strength >= 110 && player.Agility >= 42 && player.Vitality >= 45 &&
                        player.Profession == Profession.VeteranTrojan)
                    {
                        if (!player.Take(1080001))
                        {
                            dialog.Say("Sorry, an emerald is required for promotion. Hill Monsters may drop emeralds. Please come to get promoted after you have one.",
                                "I see.");
                            return;
                        }

                        if (!player.SetProfession(Profession.TigerTrojan))
                        {
                            return;
                        }

                        player.Give(130365);

                        if (player.Rebirths == 1)
                        {
                            dialog.Say("Congratulations! You have advanced to Tiger Trojan. After rebirth, your stamina will counteract 30%  damage.",
                                "Thanks.");
                        }
                        else
                        {
                            dialog.Say("Congrats! You are promoted to Tiger Trojan. Please train hard and come to get promoted after you reach level 100.",
                                "Thanks.");
                        }
                    }
                    else
                    {
                        dialog.Say("Sorry, only Veteran Trojan can be promoted to Tiger Trojan. Strength 110, agility 42 and stamina 45 are required.",
                            "I see.");
                    }
                    break;

                case 1:
                    await LearnSkills();
                    break;

                case 2:
                    await LearnXPSkills();
                    break;
            }
        }
        else if (player.Profession == Profession.TigerTrojan)
        {
            if (player.Level < 100 || player.Strength < 155 || player.Agility < 60 || player.Vitality < 92)
            {
                switch (await dialog.Ask("You can be promoted to DragonTrojan after you reach level 100, strength 155, agility 60 and stamina 92. Please train harder.",
                    "Learn skills.", "Learn XP skills.", "Let me think it over."))
                {
                    case 0:
                        await LearnSkills();
                        break;

                    case 1:
                        await LearnXPSkills();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("If you give me a meteor, I can promote you to Dragon Trojan.", "Get promoted.",
                "Learn skills.", "Learn XP skills.", "Let me think it over."))
            {
                case 0:
                    if (player.Rebirths != 0 && player.FreeSlots == 0)
                    {
                        dialog.Say("You have got reborn. I will send you a gift. Please prepare a slot in your inventory for that.",
                            "Ok. Wait a minute.");
                        return;
                    }

                    if (!player.Take(ItemType.Meteor))
                    {
                        dialog.Say("Sorry, you do not have a meteor. You may come to get promoted after you have a meteor.",
                            "I see.");
                        return;
                    }

                    if (!player.SetProfession(Profession.DragonTrojan) || player.Give(700031) is null)
                    {
                        return;
                    }

                    if (player.Rebirths == 0)
                    {
                        dialog.Say("Congratulations! You are promoted to Dragon Trojan. I reward you a Rainbow Gem.",
                            "Thanks.");
                    }
                    else if (player.Give(130487, 1, item => item.Gem1 = (Gem)255))
                    {
                        dialog.Say("You are Dragon Trojan. Because you have been reborn, I will give you a RainbowGem and a unique socketed equipment.",
                            "Thanks.");
                    }
                    break;

                case 1:
                    await LearnSkills();
                    break;

                case 2:
                    await LearnXPSkills();
                    break;
            }
        }
        else if (player.Profession == Profession.DragonTrojan)
        {
            if (player.Level >= 110 && player.Strength >= 170 && player.Agility >= 65 && player.Vitality >= 100)
            {
                switch (await dialog.Ask("If you give me a Moon Box, I can promote you to Trojan King.",
                    "Get promoted.", "Learn skills.", "Learn XP skills.", "Let me think it over."))
                {
                    case 0:
                        if (!player.Take("MoonBox"))
                        {
                            dialog.Say("Sorry, you do not have a Moon Box. You may obtain a Moon Box from Eight Diagram Tactics quest.",
                                "I see.");
                            return;
                        }

                        if (player.SetProfession(Profession.TrojanMaster) &&
                            player.Give(ItemType.DragonBall) is not null)
                        {
                            dialog.Say("Congrats! You are promoted to Trojan King. I reward you a Dragon Ball.",
                                "Thanks.");
                        }
                        break;

                    case 1:
                        await LearnSkills();
                        break;

                    case 2:
                        await LearnXPSkills();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("Sorry, you cannot be promoted to Trojan King before you are level 110, strength 170, agility 65, and stamina 100.",
                "I see.", "Learn skills.", "Learn XP skills.", "Let me think it over."))
            {
                case 1:
                    await LearnSkills();
                    break;

                case 2:
                    await LearnXPSkills();
                    break;
            }
        }
        else if (player.Profession == Profession.TrojanMaster)
        {
            switch (await dialog.Ask("You have been a Trojan King. Please train harder. I believe you will be a great King.",
                "Thanks.", "Learn skills.", "Learn XP skills.", "Let me think it over."))
            {
                case 1:
                    await LearnSkills();
                    break;

                case 2:
                    await LearnXPSkills();
                    break;
            }
        }
        else
        {
            dialog.Say("Sorry, you are not Trojan. please ask other people for help.", "I see.");
        }

        async Task LearnSkills()
        {
            switch (await dialog.Ask("You may learn Hercules. This spell is very powerful. You may cast it to kill enemies easily.",
                "Learn Hercules.", "SpiritualHealing[level 40]", "I changed my mind."))
            {
                case 0:
                    if (player.Level < 40)
                    {
                        dialog.Say("Sorry, only Trojans can learn this skill after they reach level 40.", "I see.");
                    }
                    else
                    {
                        player.Learn(1115);

                        dialog.Say("Congrats! You have learned Hercules. Please practice it hard.", "Thanks.");
                    }
                    break;

                case 1:
                    if (player.Level < 40)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you reach the required level.", "I see.");
                    }
                    else
                    {
                        player.Learn(1190);

                        dialog.Say("Congrats! You have learned spiritual healing. Please practice it hard.", "Thanks.");
                    }
                    break;
            }
        }

        async Task LearnXPSkills()
        {
            switch (await dialog.Ask("You can learn accuracy, cyclone and robot. Accuracy triples hit rate, cyclone boosts speed. Robot is used to disguise as robot.",
                "Learn accuracy and cyclone.", "Learn Robot.", "I do not want to learn."))
            {
                case 0:
                    player.Learn(1015);
                    player.Learn(1110);

                    dialog.Say("Congrats! You have learned Accuracy and Cyclone. System will notify you to use when your XP is full.",
                        "Thanks.");
                    break;

                case 1:
                    if (player.Level < 40)
                    {
                        dialog.Say("Sorry, you cannot learn this spell before you are level 40.", "I see.",
                            "I do not want to learn.");
                    }
                    else if (player.Knows(1270))
                    {
                        dialog.Say("You have learned this spell before.", "I see.", "I do not want to learn.");
                    }
                    else
                    {
                        player.Learn(1270);

                        dialog.Say("Congrats! You have learned Robot. When your XP is full, you may cast it to disguise as a Robot.",
                            "Thanks.", "I do not want to learn.");
                    }
                    break;
            }
        }
    }

    [Npc(35500)]
    public static async Task MightyTao(Player player, Dialog dialog)
    {
        dialog.Avatar = 56;

        await dialog.Confirm("Sigh, it is a pity that no one can inherit my skill.", "What kind of skill?", "I am just passing by.");

        if (player.Rebirths == 0)
        {
            dialog.Say("You are intelligent. But I can`t pass the skill to you for you have not been reborn yet.",
                "It is a pity.");
            return;
        }

        switch (await dialog.Ask("You have been reborn successfully. I have a spell to summon the guards or the monsters to your aid. Which one do you prefer?",
            "SummonGuard[level 15]", "SummonMob[level 40]", "Let me think it over."))
        {
            case 0:
                if (player.Knows(4000))
                {
                    dialog.Say("You have mastered the spell of SummonGuard. Please put in more efforts.", "Thanks.");
                }
                else if (player.Has(1072031))
                {
                    await dialog.Confirm("Since you have got EuxeniteOre, I will teach you SummonGuard. Please put in more efforts.", "Thanks.");

                    if (player.Take(1072031))
                    {
                        player.Learn(4000);
                    }
                }
                else
                {
                    dialog.Say("If you want to learn SummonGuard, you should get a EuxeniteOre.", "Thanks.");
                }
                break;

            case 1:
                if (player.Level < 40)
                {
                    dialog.Say("You have  reborn, but your level is not high enough to master SummonMob.", "Thanks.");
                    return;
                }

                switch (await dialog.Ask("Each profession has its own skills. please choose a suitable skill according to your profession.",
                    "Warrior Magic", "Trojan Magic", "Archer Magic", "Water Taoist Magic", "Fire Taoist Magic",
                    "Let me see."))
                {
                    case 0:
                        await WarriorMagic();
                        break;

                    case 1:
                        await TrojanMagic();
                        break;

                    case 2:
                        await ArcherMagic();
                        break;

                    case 3:
                        await WaterTaoistMagic();
                        break;

                    case 4:
                        await FireTaoistMagic();
                        break;
                }
                break;
        }

        async Task WarriorMagic()
        {
            if (player.Profession is not (Profession.InternWarrior or Profession.Warrior or Profession.BrassWarrior or
                Profession.SilverWarrior or Profession.GoldWarrior or Profession.WarriorMaster))
            {
                switch (await dialog.Ask("Sorry, you are not a warrior.", "Warrior Magic", "Trojan Magic",
                    "Archer Magic", "Water Taoist Magic", "Fire Taoist Magic", "Let me see."))
                {
                    case 0:
                        await WarriorMagic();
                        break;

                    case 1:
                        await TrojanMagic();
                        break;

                    case 2:
                        await ArcherMagic();
                        break;

                    case 3:
                        await WaterTaoistMagic();
                        break;

                    case 4:
                        await FireTaoistMagic();
                        break;
                }

                return;
            }

            if (player.Knows(4060))
            {
                dialog.Say("You have mastered FireEvil, you should take more practice and carry it forward.",
                    "Thanks.");
            }
            else if (player.HasAny(1072050, 1072059))
            {
                if (await dialog.Ask("Since you have got GoldOre, I will teach you FireEvil for Warrior. You should carry it forward.",
                    "Thanks.") == 0)
                {
                    await Thanks();
                }
            }
            else
            {
                dialog.Say("If you want to learn HeavenEvil, you should get a GoldOre of any rate. This is our rule.",
                    "Thanks.");
            }
        }

        async Task TrojanMagic()
        {
            if (player.Profession is not (Profession.InternTrojan or Profession.Trojan or Profession.VeteranTrojan or
                Profession.TigerTrojan or Profession.DragonTrojan or Profession.TrojanMaster))
            {
                switch (await dialog.Ask("Each profession has its own skill. You are not a Trojon, how can you learn Trojan`s skill?",
                    "Warrior Magic", "Trojan Magic", "Archer Magic", "Water Taoist Magic", "Fire Taoist Magic",
                    "Let me see."))
                {
                    case 0:
                        await WarriorMagic();
                        break;

                    case 1:
                        await TrojanMagic();
                        break;

                    case 2:
                        await ArcherMagic();
                        break;

                    case 3:
                        await WaterTaoistMagic();
                        break;

                    case 4:
                        await FireTaoistMagic();
                        break;
                }

                return;
            }

            if (player.Knows(4050))
            {
                dialog.Say("You have mastered BloodyBat, you should take more practice and carry it forward.",
                    "Thanks.");
            }
            else if (player.HasAny(1072050, 1072059))
            {
                if (await dialog.Ask("You have got GoldOre. I will teach you BloodyBat Magic for Trojan. You should carry it forward.",
                    "Thanks.") == 0)
                {
                    await Thanks11();
                }
            }
            else
            {
                dialog.Say("If you want to learn HeavenEvil, you should get a GoldOre of any rate. This is our rule.",
                    "Thanks.");
            }
        }

        async Task ArcherMagic()
        {
            if (player.Profession is not (Profession.InternArcher or Profession.Archer or Profession.EagleArcher or
                Profession.TigerArcher or Profession.DragonArcher or Profession.ArcherMaster))
            {
                switch (await dialog.Ask("Each profession has its magic. You are not Archer, how can you learn Archer's Magic?",
                    "Warrior Magic", "Trojan Magic", "Archer Magic", "Water Taoist Magic", "Fire Taoist Magic",
                    "Let me see."))
                {
                    case 0:
                        await WarriorMagic();
                        break;

                    case 1:
                        await TrojanMagic();
                        break;

                    case 2:
                        await ArcherMagic();
                        break;

                    case 3:
                        await WaterTaoistMagic();
                        break;

                    case 4:
                        await FireTaoistMagic();
                        break;
                }

                return;
            }

            if (player.Knows(4070))
            {
                dialog.Say("You have mastered Skeleton. You should take more practice.", "Thanks.");
            }
            else if (player.HasAny(1072050, 1072059))
            {
                if (await dialog.Ask("Since you have got a GoldOre, I will teach you Skeleton. You should take more practice and carry it forward.",
                    "Thanks.") == 0)
                {
                    await Thanks12();
                }
            }
            else
            {
                dialog.Say("If you want to learn HeavenEvil Magic, you should get a GoldOre of any rate. This is our rule.",
                    "Thanks.");
            }
        }

        async Task WaterTaoistMagic()
        {
            if (player.Profession is not (Profession.InternTaoist or Profession.Taoist or Profession.WaterTaoist or
                Profession.WaterWizard or Profession.WaterMaster or Profession.WaterSaint))
            {
                switch (await dialog.Ask("Each profession has its magic. You are not Water Taoist, how can you learn Water Taoist's Magic?",
                    "Warrior Magic", "Trojan Magic", "Archer Magic", "Water Taoist Magic", "Fire Taoist Magic",
                    "Let me see."))
                {
                    case 0:
                        await WarriorMagic();
                        break;

                    case 1:
                        await TrojanMagic();
                        break;

                    case 2:
                        await ArcherMagic();
                        break;

                    case 3:
                        await WaterTaoistMagic();
                        break;

                    case 4:
                        await FireTaoistMagic();
                        break;
                }

                return;
            }

            if (player.Knows(4020))
            {
                dialog.Say("You have mastered BatBoss, you should take more practice and carry it forward.", "Thanks.");
            }
            else if (player.HasAny(1072050, 1072059))
            {
                if (await dialog.Ask("Since you have got a GoldOre, I will teach you BatBoss for Water Taoist. You should carry it forward.",
                    "Thanks.") == 0)
                {
                    await Thanks13();
                }
            }
            else
            {
                dialog.Say("If you want to learn HeavenEvil Magic, you should get a GoldOre of any rate. This is our rule.",
                    "Thanks.");
            }
        }

        async Task FireTaoistMagic()
        {
            if (player.Profession is not (Profession.InternTaoist or Profession.Taoist or Profession.FireTaoist or
                Profession.FireWizard or Profession.FireMaster or Profession.FireSaint))
            {
                switch (await dialog.Ask("Only Fire Taoist can learn SummonBat.", "Warrior Magic", "Trojan Magic",
                    "Archer Magic", "Water Taoist Magic", "Fire Taoist Magic", "Let me see."))
                {
                    case 0:
                        await WarriorMagic();
                        break;

                    case 1:
                        await TrojanMagic();
                        break;

                    case 2:
                        await ArcherMagic();
                        break;

                    case 3:
                        await WaterTaoistMagic();
                        break;

                    case 4:
                        await FireTaoistMagic();
                        break;
                }

                return;
            }

            if (player.Knows(4020))
            {
                dialog.Say("You have mastered the skill of SummonBat, you must do more practice to strengthen it.",
                    "Thanks.");
            }
            else if (player.HasAny(1072050, 1072059))
            {
                if (await dialog.Ask("Since you have got GoldOre, I will teach you SummonBat. You should do more practice and carry forward it.",
                    "Thanks.") == 0)
                {
                    await Thanks14();
                }
            }
            else
            {
                dialog.Say("If you want to learn HeavenEvil Magic, you should get a GoldOre of any rate. This is our rule.",
                    "Thanks.");
            }
        }

        async Task Thanks()
        {
            if (player.Take(1072050, 1072059, 1))
            {
                player.Learn(4060);
            }
        }

        async Task Thanks11()
        {
            if (player.Take(1072050, 1072059, 1))
            {
                player.Learn(4050);
            }
        }

        async Task Thanks12()
        {
            if (player.Take(1072050, 1072059, 1))
            {
                player.Learn(4070);
            }
        }

        async Task Thanks13()
        {
            if (player.Take(1072050, 1072059, 1))
            {
                player.Learn(4020);
            }
        }

        async Task Thanks14()
        {
            if (player.Take(1072050, 1072059, 1))
            {
                player.Learn(4010);
            }
        }
    }
}
