namespace Conquer.Game.Scripts.Npcs;

public static class Market
{
    [Npc(45)]
    public static async Task MarketController(Player player, Dialog dialog)
    {
        dialog.Avatar = 156;

        if (await dialog.Ask("Do you want to leave the market? I can teleport you for free.", "Yeah. Thanks.",
            "No, I shall stay here.") == 0)
        {
            player.Recall();
        }
    }

    [Npc(47)]
    public static async Task PrizeNpc(Player player, Dialog dialog)
    {
        dialog.Avatar = 10;

        await dialog.Confirm("Are you coming for the prize? You can come to claim your prize if the system notifies you that you have got prize.",
            "Claim my prize.", "Just passing by.");

        if (player.FreeSlots < 10)
        {
            dialog.Say("Please leave 10 slots in your inventory.", "I see.");
            return;
        }

        if (player.Level < 50)
        {
            dialog.Say("You cannot claim Dragaonball unless you are level 50 or higher.", "Oh, I see.");
        }
        else if (player.ClaimPrize())
        {
            dialog.Say("Congrats! You have got your prize.", "Thanks.");
        }
        else
        {
            dialog.Say("Sorry. You do not have any prize to claim.", "Oh, I see.");
        }
    }

    [Npc(390)]
    public static async Task LoveStone(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        await dialog.Confirm("The fate brings lovers together. I hope that all can get married and live happily with their lover. What can I do for you?\n\n\n",
            "I want to get married.", "Nothing special.");

        await dialog.Confirm("The fate brings lovers together. I hope that all can get married and live happily with their lover. What can I do for you?",
            "I want to get married.");

        await dialog.Confirm("Marriage is the promise forever. Are you sure that you want to spend the rest of your life with your lover?",
            "I want to get married.", "I prefer being single.");

        dialog.Require(!player.IsMarried);

        if (player.Level < 20)
        {
            dialog.Say("Sorry, you cannot get married before you are level 20 or above.", "I see.");
            return;
        }

        await dialog.Confirm("Are you ready to propose?", "Yeah. I am ready.", "Let me think it over.");

        if (!player.IsMarried)
        {
            player.Post(ClientCommand.Propose);

            dialog.Say("Please click on your lover to propose.", "OK.");
        }
        else
        {
            player.Inform("Error: Failed to meet condition.");
        }
    }

    [Npc(3381)]
    public static async Task SurgeonMiracle(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        if (player.PkPoints > 1000)
        {
            return;
        }

        await dialog.Confirm("Do you want to have your size changed so that you can become much more adorable? Now here is a precious chance for you.",
            "I want to change my size.", "I don`t want to change.");

        dialog.Require(player.PkPoints <= 1000);

        await dialog.Confirm("If you pay me one dragonball, I can have your size changed. You will become more attractive and start a fresh life. \nBy the way, to avoid some unexpected things, make sure you are not in any disguise form.\n",
            "Here is a dragonball.", "I have no dragonball.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.Look >= 2000000000)
        {
            player.Inform("Please restore human appearance before you change your body size.");
            return;
        }

        if (!player.Has(ItemType.DragonBall))
        {
            dialog.Say("Sorry, there is no dragonball in your inventory. I can`t have your size changed.\n", "I see.");
            return;
        }

        player.Take(ItemType.DragonBall);

        if (player.Body == 1)
        {
            player.SetBody(2);
        }
        else if (player.Body == 2)
        {
            player.SetBody(1);
        }
        else if (player.Body == 3)
        {
            player.SetBody(4);
        }
        else
        {
            player.SetBody(3);
        }

        dialog.Say("Well, have a good look at yourself. Are you satisfied with your new appearance? You can start a fresh life now.",
            "Yeah. Thank you very much.");
    }

    [Npc(5004)]
    public static async Task MillionaireLee(Player player, Dialog dialog)
    {
        dialog.Avatar = 29;

        await dialog.Confirm("Hunting meteors and dragonballs is an exciting thing. However, they also pile up in your inventories.",
            "Agreed. How do you deal with it?", "I am poor and do not have the problem.");

        switch (await dialog.Ask("I can pack dragonballs and meteors for you. Give me 10 meteors or 10 dragonballs, I will make it to a MeteorScroll\nor a DragonScroll that occupies only one slot. Just right click on it, it will return into 10 meteors or 10 dragonballs again.",
            "Cool. Please pack my meteors.", "Cool. Please pack my dragonballs."))
        {
            case 0:
                if (player.Has(ItemType.Meteor, 10))
                {
                    if (player.Take(ItemType.Meteor, 10) && player.Give(720027) is not null)
                    {
                        player.Inform("Your meteors have been packed successfully.");
                    }
                }
                else
                {
                    dialog.Say("Sorry, you do not have 10 meteors.", "OK, I know.");
                }
                break;

            case 1:
                if (player.Has(ItemType.DragonBall, 10))
                {
                    if (player.Take(ItemType.DragonBall) && player.Take(ItemType.DragonBall) &&
                        player.Take(ItemType.DragonBall) && player.Take(ItemType.DragonBall) &&
                        player.Take(ItemType.DragonBall) && player.Take(ItemType.DragonBall) &&
                        player.Take(ItemType.DragonBall) && player.Take(ItemType.DragonBall) &&
                        player.Take(ItemType.DragonBall) && player.Take(ItemType.DragonBall) &&
                        player.Give(720028) is not null)
                    {
                        player.Inform("Your dragonballs have been packed successfully.");
                    }
                }
                else
                {
                    dialog.Say("Sorry, you do not have 10 dragonballs.", "OK, I know.");
                }
                break;
        }
    }

    [Npc(6003)]
    public static async Task PrisonOfficer(Player player, Dialog dialog)
    {
        dialog.Avatar = 57;

        await dialog.Confirm("Welcome, Welcome to our BotJail travel tour. If you are level 14 or below, I will show you to the BotJail to \ntake a look around for free. Do you wanna go?\n",
            "Yes, please send me there.", "No thanks.");

        if (player.Level < 15)
        {
            if (player.Teleport(6002, 29, 72))
            {
                player.AddProgress(6, 2, 1);
            }
        }
        else
        {
            dialog.Say("Sorry, you are level 15 or above, so I can not send you to the botjail.", "Oh, what a pity!");
        }
    }

    [Npc(7050)]
    public static async Task WeaponMaster(Player player, Dialog dialog)
    {
        dialog.Avatar = 28;

        switch (await dialog.Ask("MagicArtisan is very trustworthy, he can always upgrade the level and the quality of your equipment without failure.\nHowever, he is helpless in upgrading the high-level equipment. I can do what he can`t do. What are you going to upgrade?",
            "Can you tell me more?", "Upgrade my weapon.", "Necklace/Bag.", "Heavey Ring, Bracelet.", "Boots.",
            "Armor/Coat/Robe.", "Helmet/Cap/Hat/Coronet.", "Just passing by."))
        {
            case 0:
                dialog.Say("When the required level of an equipment is above 110, Magic Artisan will be unable to upgrade its level. So I am here\nto make it possible. Whatever to be upgraded, I will charge one Dragon Ball.",
                    "I see.");
                break;

            case 1:
                await dialog.Confirm("It requires a Dragon Ball. Are you ready for that?", "Yeah.", "Not yet.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragon Ball.", "I see.");
                    return;
                }

                if (!player.Wears(ItemPosition.Right))
                {
                    dialog.Say("Please wear your equipment first.", "I see.");
                    return;
                }

                if (!player.UpgradeLevel(ItemPosition.Right, ultra: true))
                {
                    dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                    return;
                }

                player.Take(ItemType.DragonBall);

                dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                    "Thanks.");
                break;

            case 2:
                await dialog.Confirm("It requires a Dragon Ball. Are you ready for that?", "Yes. I am.", "Not yet.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragon Ball.", "I see.");
                    return;
                }

                if (!player.Wears(ItemPosition.Necklace))
                {
                    dialog.Say("Please wear your equipment first.", "I see.");
                    return;
                }

                if (player.Worn(ItemPosition.Necklace)?.Level == 24)
                {
                    dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                }
                else if (player.UpgradeLevel(ItemPosition.Necklace, ultra: true))
                {
                    player.Take(ItemType.DragonBall);

                    dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                }
                break;

            case 3:
                await dialog.Confirm("It requires a Dragon Ball. Are you ready for that?", "Yes. I am.", "Not yet.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragon Ball.", "I see.");
                    return;
                }

                if (!player.Wears(ItemPosition.Ring))
                {
                    dialog.Say("Please wear your equipment first.", "I see.");
                    return;
                }

                if (player.Worn(ItemPosition.Ring)?.Subtype == 150)
                {
                    if (player.Worn(ItemPosition.Ring)?.Level == 24)
                    {
                        dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                    }
                    else if (player.UpgradeLevel(ItemPosition.Ring, ultra: true))
                    {
                        player.Take(ItemType.DragonBall);

                        dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                            "Thanks.");
                    }
                    else
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                    }
                }
                else if (player.Worn(ItemPosition.Ring)?.Subtype == 152)
                {
                    if (player.Worn(ItemPosition.Ring)?.Level == 25)
                    {
                        dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                    }
                    else if (player.UpgradeLevel(ItemPosition.Ring, ultra: true))
                    {
                        player.Take(ItemType.DragonBall);

                        dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                            "Thanks.");
                    }
                    else
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                    }
                }
                else if (player.Worn(ItemPosition.Ring)?.Subtype == 151)
                {
                    if (player.Worn(ItemPosition.Ring)?.Level == 24)
                    {
                        dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                    }
                    else if (player.UpgradeLevel(ItemPosition.Ring, ultra: true))
                    {
                        player.Take(ItemType.DragonBall);

                        dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                            "Thanks.");
                    }
                    else
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                    }
                }
                break;

            case 4:
                await dialog.Confirm("It requires a Dragon Ball. Are you ready for that?", "Yes. I am.", "Not yet.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragon Ball.", "I see.");
                    return;
                }

                if (!player.Wears(ItemPosition.Boots))
                {
                    dialog.Say("Please wear your equipment first.", "I see.");
                    return;
                }

                if (player.Worn(ItemPosition.Boots)?.Level == 24)
                {
                    dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                }
                else if (player.UpgradeLevel(ItemPosition.Boots, ultra: true))
                {
                    player.Take(ItemType.DragonBall);

                    dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                        "Thanks.");
                }
                else
                {
                    dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                }
                break;

            case 5:
                await dialog.Confirm("I will charge one DragonBall to level up it. Are you ready?", "Yes, please.", "Please wait a moment.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragon Ball.", "Oh. I see.");
                    return;
                }

                if (!player.Wears(ItemPosition.Armor))
                {
                    dialog.Say("Please wear your equipment first.", "I see.");
                    return;
                }

                if (player.Worn(ItemPosition.Armor)?.Subtype == 137)
                {
                    dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                }
                else if (player.Worn(ItemPosition.Armor)?.Level >= 9)
                {
                    if (!player.UpgradeLevel(ItemPosition.Armor, ultra: true))
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                        return;
                    }

                    player.Take(ItemType.DragonBall);

                    dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                        "Thanks. I will.");
                }
                else
                {
                    dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                }
                break;

            case 6:
                await dialog.Confirm("I will charge one DragonBall to level up it. Are you ready?", "Yes, please.", "Please wait a moment.");

                if (!player.Has(ItemType.DragonBall))
                {
                    dialog.Say("Sorry, you do not have a Dragon Ball.", "Oh, I see.");
                    return;
                }

                if (!player.Wears(ItemPosition.Head))
                {
                    dialog.Say("Please wear your equipment first.", "I see.");
                    return;
                }

                if (player.Worn(ItemPosition.Head)?.Subtype is 115 or 116)
                {
                    dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                }
                else if (player.Worn(ItemPosition.Head)?.Level >= 9)
                {
                    if (!player.UpgradeLevel(ItemPosition.Head, ultra: true))
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                        return;
                    }

                    player.Take(ItemType.DragonBall);

                    dialog.Say("Your equipment has been upgraded. It is very difficult to own a good weapon. Please use it with care.",
                        "Thanks. I will.");
                }
                else
                {
                    dialog.Say("Sorry, I can`t help to upgrade it.", "I see.");
                }
                break;
        }
    }

    [Npc(10062)]
    public static async Task MagicArtisan(Player player, Dialog dialog)
    {
        if (player.Profession >= Profession.InternTaoist)
        {
            dialog.Avatar = 6;

            switch (await dialog.Ask("Artisan Wind is skilled at upgrading. It is a pity that he sometimes fails to upgrade the items for people. I am his\nteacher and have retired for many years. I am back again to help those people who want to upgrade their items badly.\nAlthough I charge more meteors or DragonBalls, I can guarantee satisfaction to you. What are you going to upgrade?\n",
                "Upgrade the quality.", "Upgrade the level."))
            {
                case 0:
                    switch (await dialog.Ask("What item do you want to upgrade?", "Cap and Earring", "Bag", "Gown",
                        "Sword", "Bracelet", "Boots", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring();
                            break;

                        case 1:
                            await Bag();
                            break;

                        case 2:
                            await Gown();
                            break;

                        case 3:
                            await Sword();
                            break;

                        case 4:
                            await Bracelet();
                            break;

                        case 5:
                            await Boots();
                            break;
                    }
                    break;

                case 1:
                    switch (await dialog.Ask("What item do you want to upgrade?", "Cap and Earring", "Bag/Necklace",
                        "Gown", "Sword", "Bracelet", "Boot", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring15();
                            break;

                        case 1:
                            await BagNecklace();
                            break;

                        case 2:
                            await Gown17();
                            break;

                        case 3:
                            await Sword18();
                            break;

                        case 4:
                            await Bracelet19();
                            break;

                        case 5:
                            await Boot();
                            break;
                    }
                    break;
            }
        }
        else if (player.Profession <= Profession.Warrior)
        {
            dialog.Avatar = 6;

            switch (await dialog.Ask("Artisan Wind is skilled at upgrading. It is a pity that he sometimes fails to upgrade the items for people. I am his\nteacher and have retired for many years. I am back again to help those people who want to upgrade their items badly.\nAlthough I charge more meteors or DragonBalls, I can guarantee satisfaction to you. What are you going to upgrade?\n",
                "Upgrade the quality", "Upgrade the level"))
            {
                case 0:
                    switch (await dialog.Ask("What item do you want to upgrade?", "Coronet, Earring", "Necklace",
                        "Armor", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring();
                            break;

                        case 1:
                            await Bag();
                            break;

                        case 2:
                            await Gown();
                            break;

                        case 3:
                            await Sword();
                            break;

                        case 4:
                            await Bracelet();
                            break;

                        case 5:
                            await Boots();
                            break;
                    }
                    break;

                case 1:
                    switch (await dialog.Ask("What item do you want to upgrade?", "Coronet, Earring", "Necklace",
                        "Armor", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring15();
                            break;

                        case 1:
                            await BagNecklace();
                            break;

                        case 2:
                            await Gown17();
                            break;

                        case 3:
                            await Sword18();
                            break;

                        case 4:
                            await Bracelet19();
                            break;

                        case 5:
                            await Boot();
                            break;
                    }
                    break;
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            dialog.Avatar = 6;

            switch (await dialog.Ask("Artisan Wind is skilled at upgrading. It is a pity that he sometimes fails to upgrade the items for people. I am his\nteacher and have retired for years. I come back again to help those people who want to upgrade their items badly.\nAlthough I charge more meteors or DragonBalls, I can guarantee satisfaction to you. What are you going to upgrade?\n",
                "Upgrade the quality", "Upgrade the level"))
            {
                case 0:
                    switch (await dialog.Ask("What items do you want to upgrade?", "Helmet, Earring", "Necklace",
                        "Armor", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Shield", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring();
                            break;

                        case 1:
                            await Bag();
                            break;

                        case 2:
                            await Gown();
                            break;

                        case 3:
                            await Sword();
                            break;

                        case 4:
                            await Bracelet();
                            break;

                        case 5:
                            await Boots();
                            break;

                        case 6:
                            if (!player.Wears(ItemPosition.Left))
                            {
                                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                                return;
                            }

                            if (player.Worn(ItemPosition.Left)?.Quality == 9)
                            {
                                dialog.Say("Sorry, this item is a super one. It has reached the highest quality.",
                                    "I see.");
                            }
                            else if (player.IsRepaired(ItemPosition.Left))
                            {
                                await dialog.Confirm($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Left)} dragonballs.",
                                    "OK.", "I changed my mind.");

                                if (player.UpgradeQuality(ItemPosition.Left))
                                {
                                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.",
                                        "Thanks.");
                                }
                                else
                                {
                                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                                        "I see.");
                                }
                            }
                            else
                            {
                                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.",
                                    "I see.");
                            }
                            break;
                    }
                    break;

                case 1:
                    switch (await dialog.Ask("What items do you want to upgrade?", "Helmet, Earring", "Necklace",
                        "Armor", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Shield", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring15();
                            break;

                        case 1:
                            await BagNecklace();
                            break;

                        case 2:
                            await Gown17();
                            break;

                        case 3:
                            await Sword18();
                            break;

                        case 4:
                            await Bracelet19();
                            break;

                        case 5:
                            await Boot();
                            break;

                        case 6:
                            if (!player.Wears(ItemPosition.Left))
                            {
                                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                                return;
                            }

                            if (player.Worn(ItemPosition.Left)?.Level == 9)
                            {
                                dialog.Say("I am very sorry. This equipment consumes a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMast",
                                    "I see.");
                            }
                            else if (player.IsRepaired(ItemPosition.Left))
                            {
                                if (player.Worn(ItemPosition.Left)?.Level == 8 &&
                                    player.Worn(ItemPosition.Left)?.Quality == 9)
                                {
                                    dialog.Avatar = 28;

                                    await dialog.Confirm("Sorry, too many meteors are required to upgrade this item. So I will charge you two DragonBalls instead. Do you want to upgrade",
                                        "Yes. I wanna upgrade it.", "Just passing by.");

                                    if (!player.Has(ItemType.DragonBall, 2))
                                    {
                                        dialog.Say("Sorry, you do not have two dragon balls.", "Oh. I see.");
                                        return;
                                    }

                                    if (!player.UpgradeLevel(ItemPosition.Left, ultra: true))
                                    {
                                        dialog.Say("The required level of the equipment has exceeded your current level.",
                                            "I see.");
                                        return;
                                    }

                                    player.Take(ItemType.DragonBall, 2);

                                    dialog.Say("Done! Here you are. It is not easy to get a good gear. Please take good care of it.",
                                        "Thanks. I will.");
                                    return;
                                }

                                if (await dialog.Ask($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Left)} meteors or meteor tears.",
                                    "OK.", "I changed my mind.") == 0)
                                {
                                    await UpgradeShieldLevel();
                                }
                            }
                            else
                            {
                                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.",
                                    "I see.");
                            }
                            break;
                    }
                    break;
            }
        }
        else
        {
            dialog.Avatar = 6;

            switch (await dialog.Ask("Artisan Wind is skilled at upgrading. It is a pity that he sometimes fails to upgrade the items for people. I am his\nteacher and have retired for many years. I am back again to help those people who want to upgrade their items badly.\nAlthough I charge more meteors or DragonBalls, I can guarantee satisfaction to you. What are you going to upgrade?\n",
                "Upgrade the quality", "Upgrade the level"))
            {
                case 0:
                    switch (await dialog.Ask("What item do you want to upgrade?", "Hat, Earring", "Necklace", "coat",
                        "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring();
                            break;

                        case 1:
                            await Bag();
                            break;

                        case 2:
                            await Gown();
                            break;

                        case 3:
                            await Sword();
                            break;

                        case 4:
                            await Bracelet();
                            break;

                        case 5:
                            await Boots();
                            break;
                    }
                    break;

                case 1:
                    switch (await dialog.Ask("What item do you want to upgrade?", "Hat, Earring", "Necklace", "Coat",
                        "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Just passing by."))
                    {
                        case 0:
                            await CapAndEarring15();
                            break;

                        case 1:
                            await BagNecklace();
                            break;

                        case 2:
                            await Gown17();
                            break;

                        case 3:
                            await Sword18();
                            break;

                        case 4:
                            await Bracelet19();
                            break;

                        case 5:
                            await Boot();
                            break;
                    }
                    break;
            }
        }

        async Task CapAndEarring()
        {
            if (!player.Wears(ItemPosition.Head))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Head)?.Quality == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item is a super one. It has reached the highest quality.", "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Head))
            {
                dialog.Avatar = 6;

                await dialog.Confirm($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Head)} DragonBalls.",
                    "OK.", "I changed my mind.");

                if (player.UpgradeQuality(ItemPosition.Head))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Bag()
        {
            if (!player.Wears(ItemPosition.Necklace))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Necklace)?.Quality == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item is a super one. It has reached the highest quality.", "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Necklace))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Necklace)} dragonballs.",
                    "OK.", "I changed my mind.") != 0)
                {
                    return;
                }

                if (player.UpgradeQuality(ItemPosition.Necklace))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Gown()
        {
            if (!player.Wears(ItemPosition.Armor))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Armor)?.Quality == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, I cannot upgrade this item. It has reached the highest level.", "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Armor))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Armor)} dragonballs.",
                    "OK.", "I changed my mind.") != 0)
                {
                    return;
                }

                if (player.UpgradeQuality(ItemPosition.Armor))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Sword()
        {
            if (!player.Wears(ItemPosition.Right))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Right)?.Quality == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item is a super one. It has reached the highest quality.", "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Right))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Right)} dragonballs.",
                    "OK.", "I changed my mind.") != 0)
                {
                    return;
                }

                if (player.UpgradeQuality(ItemPosition.Right))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Bracelet()
        {
            if (!player.Wears(ItemPosition.Ring))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Ring)?.Quality == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, I cannot upgrade this item. It has reached the highest level.", "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Ring))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Ring)} dragonballs.",
                    "OK.", "I changed my mind.") != 0)
                {
                    return;
                }

                if (player.UpgradeQuality(ItemPosition.Ring))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Boots()
        {
            if (!player.Wears(ItemPosition.Boots))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Boots)?.Quality == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item is a super one. It has reached the highest quality.", "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Boots))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.QualityCost(ItemPosition.Boots)} dragonballs.",
                    "OK.", "I changed my mind.") != 0)
                {
                    return;
                }

                if (player.UpgradeQuality(ItemPosition.Boots))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task CapAndEarring15()
        {
            if (!player.Wears(ItemPosition.Head))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Head)?.Level == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("I am very sorry. This equipment consumes a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMast",
                    "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Head))
            {
                if (player.Worn(ItemPosition.Head)?.Level == 8 && player.Worn(ItemPosition.Head)?.Quality == 9)
                {
                    dialog.Avatar = 28;

                    await dialog.Confirm("Sorry, too many meteors are required to upgrade it. So I will charge you two DragonBalls instead. Do you want to upgrade it?",
                        "Yes. I wanna upgrade it.", "Just passing by.");

                    if (!player.Has(ItemType.DragonBall, 2))
                    {
                        dialog.Say("Sorry, you do not have two dragon balls.", "Oh. I see.");
                        return;
                    }

                    if (!player.UpgradeLevel(ItemPosition.Head, ultra: true))
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                        return;
                    }

                    player.Take(ItemType.DragonBall, 2);

                    dialog.Say("Done! Here you are. It is not easy to get a good gear. Please take good care of it.",
                        "Thanks. I will.");
                    return;
                }

                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Head)} meteors or meteor tears.",
                    "OK.", "I changed my mind.") == 0)
                {
                    await UpgradeHeadgearLevel();
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task BagNecklace()
        {
            if (!player.Wears(ItemPosition.Necklace))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Necklace)?.Level == 21)
            {
                dialog.Avatar = 6;

                dialog.Say("I am very sorry. This equipment consumes a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMast",
                    "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Necklace))
            {
                dialog.Avatar = 6;

                await dialog.Confirm($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Necklace)} meteors or meteor tears.",
                    "OK.", "I changed my mind.");

                if (player.UpgradeLevel(ItemPosition.Necklace))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Gown17()
        {
            if (!player.Wears(ItemPosition.Armor))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Armor)?.Level == 9)
            {
                dialog.Avatar = 6;

                dialog.Say("I am sorry. This equipment consumes a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMaster.",
                    "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Armor))
            {
                if (player.Worn(ItemPosition.Armor)?.Level == 8 && player.Worn(ItemPosition.Armor)?.Quality == 9)
                {
                    dialog.Avatar = 28;

                    await dialog.Confirm("Sorry, too many meteors are required to upgrade this item. So I will charge you two DragonBalls instead. Do you want to upgrade",
                        "Yes. I wanna upgrade it.", "Just passing by.");

                    if (!player.Has(ItemType.DragonBall, 2))
                    {
                        dialog.Say("Sorry, you do not have two dragon balls.", "Oh. I see.");
                        return;
                    }

                    if (!player.UpgradeLevel(ItemPosition.Armor, ultra: true))
                    {
                        dialog.Say("The required level of the equipment has exceeded your current level.", "I see.");
                        return;
                    }

                    player.Take(ItemType.DragonBall, 2);

                    dialog.Say("Done! Here you are. It is not easy to get a good gear. Please take good care of it.",
                        "Thanks. I will.");
                    return;
                }

                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Armor)} meteors or meteor tears.",
                    "OK.", "I changed my mind.") == 0)
                {
                    await UpgradeArmorLevel();
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Sword18()
        {
            if (!player.Wears(ItemPosition.Right))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Right)?.Level == 22)
            {
                dialog.Avatar = 6;

                dialog.Say("I am very sorry. This equipment needs a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMaster.",
                    "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Right))
            {
                dialog.Avatar = 6;

                await dialog.Confirm($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Right)} meteors or meteor tears.",
                    "OK.", "I changed my mind.");

                if (player.UpgradeLevel(ItemPosition.Right))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Bracelet19()
        {
            if (!player.Wears(ItemPosition.Ring))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Ring)?.Level == 22)
            {
                dialog.Avatar = 6;

                dialog.Say("I am very sorry. This equipment consumes a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMast",
                    "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Ring))
            {
                dialog.Avatar = 6;

                await dialog.Confirm($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Ring)} meteors or meteor tears.",
                    "OK.", "I changed my mind.");

                if (player.UpgradeLevel(ItemPosition.Ring))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task Boot()
        {
            if (!player.Wears(ItemPosition.Boots))
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, please wear the item you want to upgrade first.", "I see.");
                return;
            }

            if (player.Worn(ItemPosition.Boots)?.Level == 22)
            {
                dialog.Avatar = 6;

                dialog.Say("I am very sorry. This equipment consumes a lot of meteors to upgrade. You`d better try your luck with ArtisanWind or WeaponMast",
                    "I see.");
            }
            else if (player.IsRepaired(ItemPosition.Boots))
            {
                dialog.Avatar = 6;

                if (await dialog.Ask($"To upgrade this item, you should give me {player.LevelCost(ItemPosition.Boots)} meteors or meteor tears.",
                    "OK.", "I changed my mind.") != 0)
                {
                    return;
                }

                if (player.UpgradeLevel(ItemPosition.Boots))
                {
                    dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
                }
                else
                {
                    dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                        "I see.");
                }
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, this item has been damaged. Please get it repaired first.", "I see.");
            }
        }

        async Task UpgradeHeadgearLevel()
        {
            if (player.UpgradeLevel(ItemPosition.Head))
            {
                dialog.Avatar = 6;

                dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                    "I see.");
            }
        }

        async Task UpgradeArmorLevel()
        {
            if (player.UpgradeLevel(ItemPosition.Armor))
            {
                dialog.Avatar = 6;

                dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                    "I see.");
            }
        }

        async Task UpgradeShieldLevel()
        {
            if (player.UpgradeLevel(ItemPosition.Left))
            {
                dialog.Avatar = 6;

                dialog.Say("Your item has been upgraded. Please kindly check. I never fail you.", "Thanks.");
            }
            else
            {
                dialog.Avatar = 6;

                dialog.Say("Sorry, I cannot upgrade this item. You do not have the required stuff or your level is too low to wear if it is upgraded.",
                    "I see.");
            }
        }
    }

    [Npc(10065)]
    public static async Task GodlyArtisan(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Profession >= Profession.InternTaoist)
        {
            switch (await dialog.Ask("Magic Artisan and I are classmates. Our teacher liked us very much and taught us his unique skills. Magic Artisan\nis adept in upgrading the items, while I am proficient in repairing the max durability. Our teacher told us that\nwe should help others with all our hearts. So I am here to help those people who want their items repaired badly.\nWhat items do you want me to repair?",
                "Cap, Earring", "Bag, necklace", "Robe", "Sword", "Bracelet", "Boots"))
            {
                case 0:
                    await CapEarring();
                    break;

                case 1:
                    await BagNecklace();
                    break;

                case 2:
                    await Robe();
                    break;

                case 3:
                    await Sword();
                    break;

                case 4:
                    await Bracelet();
                    break;

                case 5:
                    await Boots();
                    break;
            }
        }
        else if (player.Profession <= Profession.Warrior)
        {
            switch (await dialog.Ask("Magic Artisan and I are classmates. Our teacher liked us very much and has taught us his unique skills.\nMagic Artisan is adept in upgrading the items, while I am proficient in repairing the max durability.\nOur teacher told us that we should try to help others. I am here to help those people who need their items repaired badly.\nWhat items do you want me to repair?",
                "Coronet, Earring", "Necklace", "Armor", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots"))
            {
                case 0:
                    await CapEarring();
                    break;

                case 1:
                    await BagNecklace();
                    break;

                case 2:
                    await Robe();
                    break;

                case 3:
                    await Sword();
                    break;

                case 4:
                    await Bracelet();
                    break;

                case 5:
                    await Boots();
                    break;
            }
        }
        else if (player.Profession <= Profession.WarriorMaster)
        {
            switch (await dialog.Ask("Magic Artisan and I are classmates. Our teacher liked us very much and has taught us his unique skills.\nMagic Artisan is adept in upgrading the items, while I am proficient in repairing the max durability.\nOur teacher told us that we should try to help others. I am here to help those people who need their items repaired badly.\nWhat items do you want me to repair?",
                "Helmet, Earring", "Necklace", "Armor", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots", "Shield"))
            {
                case 0:
                    await CapEarring();
                    break;

                case 1:
                    await BagNecklace();
                    break;

                case 2:
                    await Robe();
                    break;

                case 3:
                    await Sword();
                    break;

                case 4:
                    await Bracelet();
                    break;

                case 5:
                    await Boots();
                    break;

                case 6:
                    if (!player.Wears(ItemPosition.Left))
                    {
                        dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                        return;
                    }

                    if (!player.IsRepaired(ItemPosition.Left))
                    {
                        dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                            "I see.");
                        return;
                    }

                    if (player.RestoreCost(ItemPosition.Left) == 0)
                    {
                        dialog.Say("The max durability of this item is in good condition.", "I see.");
                        return;
                    }

                    await dialog.Confirm($"To repair this item, you should give me {player.RestoreCost(ItemPosition.Left)} silver.",
                        "OK.", "I changed my mind.");

                    if (player.RestoreMaxDurability(ItemPosition.Left))
                    {
                        dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                            "Thanks.");
                    }
                    else
                    {
                        dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
                    }
                    break;
            }
        }
        else
        {
            switch (await dialog.Ask("Magic Artisan and I are classmates. Our teacher liked us very much and has taught us his unique skills.\nMagic Artisan is adept in upgrading the items, while I am proficient in repairing the max durability.\nOur teacher told us that we should try to help others. I am here to help those people who need their items repaired badly.\nWhat items do you want me to repair?",
                "Hat, Earring", "Necklace", "Coat", "Weapon", "Ring/Heavy Ring/Bracelet", "Boots"))
            {
                case 0:
                    await CapEarring();
                    break;

                case 1:
                    await BagNecklace();
                    break;

                case 2:
                    await Robe();
                    break;

                case 3:
                    await Sword();
                    break;

                case 4:
                    await Bracelet();
                    break;

                case 5:
                    await Boots();
                    break;
            }
        }

        async Task CapEarring()
        {
            if (!player.Wears(ItemPosition.Head))
            {
                dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                return;
            }

            if (!player.IsRepaired(ItemPosition.Head))
            {
                dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                    "I see.");
                return;
            }

            if (player.RestoreCost(ItemPosition.Head) == 0)
            {
                dialog.Say("The max durability of this item is in good condition.", "I see.");
                return;
            }

            await dialog.Confirm($"To get this item repaired, you should give me {player.RestoreCost(ItemPosition.Head)} silver.",
                "OK.", "I changed my mind.");

            if (player.RestoreMaxDurability(ItemPosition.Head))
            {
                dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                    "Thanks.");
            }
            else
            {
                dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
            }
        }

        async Task BagNecklace()
        {
            if (!player.Wears(ItemPosition.Necklace))
            {
                dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                return;
            }

            if (!player.IsRepaired(ItemPosition.Necklace))
            {
                dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                    "I see.");
                return;
            }

            if (player.RestoreCost(ItemPosition.Necklace) == 0)
            {
                dialog.Say("The max durability of this item is in good condition.", "I see.");
                return;
            }

            await dialog.Confirm($"To repair this item, you should give me {player.RestoreCost(ItemPosition.Necklace)} silver.",
                "OK.", "I changed my mind.");

            if (player.RestoreMaxDurability(ItemPosition.Necklace))
            {
                dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                    "Thanks.");
            }
            else
            {
                dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
            }
        }

        async Task Robe()
        {
            if (!player.Wears(ItemPosition.Armor))
            {
                dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                return;
            }

            if (!player.IsRepaired(ItemPosition.Armor))
            {
                dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                    "I see.");
                return;
            }

            if (player.RestoreCost(ItemPosition.Armor) == 0)
            {
                dialog.Say("The max durability of this item is in good condition.", "I see.");
                return;
            }

            if (await dialog.Ask($"To repair this item, you should give me {player.RestoreCost(ItemPosition.Armor)} silver.",
                "OK.", "I changed my mind.") != 0)
            {
                return;
            }

            if (player.RestoreMaxDurability(ItemPosition.Armor))
            {
                dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                    "Thanks.");
            }
            else
            {
                dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
            }
        }

        async Task Sword()
        {
            if (!player.Wears(ItemPosition.Right))
            {
                dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                return;
            }

            if (!player.IsRepaired(ItemPosition.Right))
            {
                dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                    "I see.");
                return;
            }

            if (player.RestoreCost(ItemPosition.Right) == 0)
            {
                dialog.Say("The max durability of this item is in good condition.", "I see.");
                return;
            }

            if (await dialog.Ask($"To repair this item, you should give me {player.RestoreCost(ItemPosition.Right)} silver.",
                "OK.", "I changed my mind.") != 0)
            {
                return;
            }

            if (player.RestoreMaxDurability(ItemPosition.Right))
            {
                dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                    "Thanks.");
            }
            else
            {
                dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
            }
        }

        async Task Bracelet()
        {
            if (!player.Wears(ItemPosition.Ring))
            {
                dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                return;
            }

            if (!player.IsRepaired(ItemPosition.Ring))
            {
                dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                    "I see.");
                return;
            }

            if (player.RestoreCost(ItemPosition.Ring) == 0)
            {
                dialog.Say("The max durability of this item is in good condition.", "I see.");
                return;
            }

            if (await dialog.Ask($"To repair this item, you should give me {player.RestoreCost(ItemPosition.Ring)} silver.",
                "OK.", "I changed my mind.") != 0)
            {
                return;
            }

            if (player.RestoreMaxDurability(ItemPosition.Ring))
            {
                dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                    "Thanks.");
            }
            else
            {
                dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
            }
        }

        async Task Boots()
        {
            if (!player.Wears(ItemPosition.Boots))
            {
                dialog.Say("Sorry, you do not wear this item. Please equip first.", "I see.");
                return;
            }

            if (!player.IsRepaired(ItemPosition.Boots))
            {
                dialog.Say("Sorry, please ask a shopkeeper to repair this item first. I only repair the max durability.",
                    "I see.");
                return;
            }

            if (player.RestoreCost(ItemPosition.Boots) == 0)
            {
                dialog.Say("The max durability of this item is in good condition.", "I see.");
                return;
            }

            if (await dialog.Ask($"To repair this item, you should give me {player.RestoreCost(ItemPosition.Boots)} silver.",
                "OK.", "I changed my mind.") != 0)
            {
                return;
            }

            if (player.RestoreMaxDurability(ItemPosition.Boots))
            {
                dialog.Say("Your item has been repaired. Please kindly check and take good care of your items.",
                    "Thanks.");
            }
            else
            {
                dialog.Say("Sorry, you do not have the required money. I cannot repair it for you.", "I see.");
            }
        }
    }

    [Npc(30156)]
    public static async Task HouseAdmin(Player player, Dialog dialog, GameStore store)
    {
        dialog.Avatar = 92;

        if (player.HomeId == 0)
        {
            if (!player.IsMarried)
            {
                if (await dialog.Ask("What can I do for you?", "Buy a house.", "Just passing by.") == 0)
                {
                    await BuyAHouse();
                }

                return;
            }

            if (player.IsFemale)
            {
                switch (await dialog.Ask("What can do for you?", "Buy a house.", "Visit my husband`s house.",
                    "Just passing by."))
                {
                    case 0:
                        await BuyAHouse();
                        break;

                    case 1:
                        await VisitSpouseHouse();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("What can I do for you?", "Buy a house.", "Visit my wife`s house.",
                "Just passing by."))
            {
                case 0:
                    await BuyAHouse();
                    break;

                case 1:
                    await VisitSpouseHouse();
                    break;
            }
        }
        else if (player.IsMarried)
        {
            if (player.IsFemale)
            {
                switch (await dialog.Ask("What can I do for you?", "Go home.", "Upgrade my house.",
                    "Visit my husband`s house.", "Just passing by."))
                {
                    case 0:
                        await GoHome();
                        break;

                    case 1:
                        await UpgradeMyHouse();
                        break;

                    case 2:
                        await VisitSpouseHouse();
                        break;
                }

                return;
            }

            switch (await dialog.Ask("What can I do for you?", "Go home.", "Upgrade my house.",
                "Visit my wife`s house.", "Just passing by."))
            {
                case 0:
                    await GoHome();
                    break;

                case 1:
                    await UpgradeMyHouse();
                    break;

                case 2:
                    await VisitSpouseHouse();
                    break;
            }
        }
        else
        {
            switch (await dialog.Ask("What can I do for you?", "Go home.", "Upgrade my house.", "Just passing by."))
            {
                case 0:
                    await GoHome();
                    break;

                case 1:
                    await UpgradeMyHouse();
                    break;
            }
        }

        async Task BuyAHouse()
        {
            if (!player.Has(721170))
            {
                dialog.Say("Once you have a House Permit, you may come to apply for a house.", "I see.");
                return;
            }

            if (player.Money < 300000)
            {
                dialog.Say("If you give me a House Permit and 300,000 silver, I shall sell a house to you.", "I see.");
                return;
            }

            await dialog.Confirm("Do you want to buy a house? Once you pay 300,000 silver, you will own a house.",
                "Here is my money.", "Too expensive.");

            if (!player.Spend(300000))
            {
                dialog.Say("If you give me a House Permit and 300,000 silver, I shall sell a house to you.", "I see.");
                return;
            }

            if (!player.Take(721170) || !player.CreateHome("House", 1098, 1038, 35, 35, 1002, 0, 1))
            {
                return;
            }

            var box = new DynaNpc
            {
                Name = "ItemBox",
                Type = 4,
                Sort = 33,
                Look = 8200,
                OwnerType = 1,
                OwnerId = player.Id,
                MapId = player.HomeId,
                X = 21,
                Y = 39,
                Task0 = 8003010,
                Data3 = 20
            };

            if (player.World.CreateNpc(box))
            {
                dialog.Say("Congrats! You own a house now. I can teleport you to your house. You can team others up to let them visit your house.",
                    "I see. Thanks.");
            }
        }

        async Task VisitSpouseHouse()
        {
            if (!player.Visit(await store.HomeOfAsync(player.Spouse)))
            {
                dialog.Say("Your spouse does not own a house.", "I see.");
            }
        }

        async Task GoHome()
        {
            player.GoHome();
        }

        async Task UpgradeMyHouse()
        {
            if (player.Home!.Level < 2)
            {
                await dialog.Confirm("To upgrade your house to second-class, you should have an Upgrade Certificate and 500,000 silver.",
                    "All are ready.", "I shall come later.", "Just passing by.");

                if (!player.Has(721174))
                {
                    dialog.Say("Sorry, you do not have an Upgrade Certificate. The House Agent is issuing the Upgrade certificates in Twin City.",
                        "I see.", "Just passing by.");
                    return;
                }

                if (player.Money < 500000)
                {
                    dialog.Say("Sorry, you do not have 500,000 silver.", "I see.", "Just passing by.");
                    return;
                }

                player.Take(721174);
                player.Spend(500000);
                player.Home!.Level = 2;
                player.Home!.Redesign(1099);
                player.Home!.PortalX = 51;
                player.Home!.PortalY = 80;

                dialog.Say("Congrats. Your house has been upgraded to second-class. As we need two days to repair your house, please go home then.",
                    "Thank you.", "Just passing by.");
            }
            else if (player.Home!.Level == 2)
            {
                dialog.Say("Your house is of second-class. You need not upgrade it.", "I see.", "Just passing by.");
            }
            else
            {
                dialog.Say("Your house is better than second-class. You need not upgrade it.", "I see.",
                    "Just passing by.");
            }
        }
    }

    [Npc(30158)]
    public static async Task Craftsman(Player player, Dialog dialog)
    {
        dialog.Avatar = 92;

        await dialog.Confirm("What can I do for you?", "Do you have timber.", "Just passing by.");

        await dialog.Confirm("We are rebuilding the market and have stored a lot of timber.", "Can you give me some?", "You have wasted too much.");

        await dialog.Confirm("We are in urgent need of ores. You may exchange five ores for a piece of timber.",
            "Here are five ores.", "I shall come later.");

        if (player.Take(1072010, 1072059, 5))
        {
            player.Give(721171);
        }
        else
        {
            dialog.Say("Sorry, you do not have five ores.", "I see.");
        }
    }

    [Npc(30159)]
    public static async Task Carpenter(Player player, Dialog dialog)
    {
        dialog.Avatar = 92;

        await dialog.Confirm("I am here to issue all kinds of timber Vouchers. Are you here for the Vouchers?", "Yeah.", "Just passing by.");

        switch (await dialog.Ask("I can issue a Rosewood Voucher for 10 pieces of timber, and a Timber Voucher for 10 Rosewood Vouchers. What do you want?",
            "Rosewood Voucher.", "Timber Voucher.", "I changed my mind."))
        {
            case 0:
                if (player.Take(721171, 10))
                {
                    player.Give(721172);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 10 pieces of timber. I heard that Craftsman is supplying timber.",
                        "I see.");
                }
                break;

            case 1:
                if (player.Take(721172, 10))
                {
                    player.Give(721173);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 10 Rosewood Vouchers.", "I see.");
                }
                break;
        }
    }

    [Npc(30160)]
    public static async Task Stoneman(Player player, Dialog dialog)
    {
        dialog.Avatar = 28;

        switch (await dialog.Ask("I am issuing all kinds of ore vouchers. What Vouchers do you want?",
            "Copper Ore Voucher", "Ore Voucher"))
        {
            case 0:
                await dialog.Confirm("To obtain a Copper Ore Voucher, 12 Copper Ores will be charged. Are you ready for that?",
                    "Yeah. Here you are.", "No. I shall come later.");

                if (player.Take(1072020, 1072029, 12))
                {
                    player.Give(721175);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 12 Copper Ores.", "I see.");
                }
                break;

            case 1:
                await dialog.Confirm("To obtain an Ore Voucher, 10 Copper Ore Vouchers are required. Are you ready for that?",
                    "Yeah. Here you are.", "No. I shall come later.");

                if (player.Take(721175, 10))
                {
                    player.Give(721176);
                }
                else
                {
                    dialog.Say("Sorry, you do not have 10 Copper Ore Vouchers.", "I see.");
                }
                break;
        }
    }

    [Npc(35015)]
    public static async Task Ethereal(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        switch (await dialog.Ask("Tortoise Gem is an ancient rarity. It can not only reduce the damages taken, but also upgrade the blessed equipment.\nIf we craft it with blessed equipment, its power will be brought into full play. Only I know the recipe.",
            "I want to upgrade my blessed equipment.", "How can I upgrade my blessed attribute?",
            "What`s the use of this oven?"))
        {
            case 0:
                switch (await dialog.Ask("Please select the blessed equipment you want to upgrade.", "Headwear",
                    "Necklace", "Ring, Heavy ring, Bracelet", "Right-handed weapon", "Armor, Coat, Robe", "Shield.",
                    "Boots", "Let me think it over."))
                {
                    case 0:
                        if (!player.Wears(ItemPosition.Head))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Head))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Head)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Head)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Head))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Head))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Head)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Head, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Head)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Head))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Head))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Head)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Head, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Head)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Head))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Head))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Head)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Head, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;

                    case 1:
                        if (!player.Wears(ItemPosition.Necklace))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Necklace))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Necklace)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Necklace)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Necklace))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Necklace))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Necklace)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Necklace, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Necklace)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Necklace))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Necklace))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Necklace)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Necklace, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Necklace)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Necklace))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Necklace))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Necklace)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Necklace, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;

                    case 2:
                        if (!player.Wears(ItemPosition.Ring))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Ring))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Ring)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Ring)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Ring))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Ring))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Ring)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Ring, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Ring)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Ring))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Ring))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Ring)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Ring, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Ring)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Ring))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Ring))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Ring)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Ring, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;

                    case 3:
                        if (!player.Wears(ItemPosition.Right))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Right))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Right)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Right)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Right))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Right))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Right)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Right, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Right)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Right))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Right))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Right)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Right, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Right)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Right))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Right))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Right)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Right, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;

                    case 4:
                        if (!player.Wears(ItemPosition.Armor))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Armor))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Armor)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Armor)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Armor))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Armor))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Armor)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Armor, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Armor)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Armor))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Armor))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Armor)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Armor, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Armor)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Armor))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Armor))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Armor)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Armor, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;

                    case 5:
                        if (!player.Wears(ItemPosition.Left))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Left))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Left)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Left)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Left))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Left))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Left)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Left, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Left)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Left))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Left))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Left)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Left, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Left)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Left))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Left))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Left)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Left, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;

                    case 6:
                        if (!player.Wears(ItemPosition.Boots))
                        {
                            dialog.Say("Sorry, you have no such equipment.", "I see.");
                            return;
                        }

                        if (!player.IsRepaired(ItemPosition.Boots))
                        {
                            dialog.Say("Please repair your equipment first.", "I see.");
                            return;
                        }

                        if (player.ItemAt(ItemPosition.Boots)?.Bless == 7)
                        {
                            dialog.Say("Your blessed equipment has reached the highest level. It can`t be upgraded any more.",
                                "I see. Thanks.");
                        }
                        else if (player.ItemAt(ItemPosition.Boots)?.Bless == 5)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 5 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Boots))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Boots))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Boots)?.Bless != 5)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 5))
                            {
                                if (player.Take(700073, 5) && player.Bless(ItemPosition.Boots, 7))
                                {
                                    player.ShowEffect("Aegis4");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Boots)?.Bless == 3)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have 3 super Tortoise Gems.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Boots))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Boots))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Boots)?.Bless != 3)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073, 3))
                            {
                                if (player.Take(700073, 3) && player.Bless(ItemPosition.Boots, 5))
                                {
                                    player.ShowEffect("Aegis3");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else if (player.ItemAt(ItemPosition.Boots)?.Bless == 1)
                        {
                            await dialog.Confirm("If you want to upgrade your blessed attribute to the next level, you must have a super Tortoise Gem.",
                                "All are ready.", "Let me think it over.");

                            if (!player.Wears(ItemPosition.Boots))
                            {
                                dialog.Say("Sorry, you have no such equipment.", "I see.");
                                return;
                            }

                            if (!player.IsRepaired(ItemPosition.Boots))
                            {
                                dialog.Say("Please repair your equipment first.", "I see.");
                                return;
                            }

                            if (player.ItemAt(ItemPosition.Boots)?.Bless != 1)
                            {
                                player.Inform("Please select the correct blessed equipment to upgrade.");
                                return;
                            }

                            if (player.Has(700073))
                            {
                                if (player.Take(700073) && player.Bless(ItemPosition.Boots, 3))
                                {
                                    player.ShowEffect("Aegis2");
                                    player.Inform("Congratulations! Your blessed attribute has been successfully upgraded.");
                                }
                            }
                            else
                            {
                                dialog.Say("You don`t have the required items. I can`t help you.", "I see.");
                            }
                        }
                        else
                        {
                            dialog.Say("Only blessed equipment can be upgraded.", "I see. Thanks.");
                        }
                        break;
                }
                break;

            case 1:
                if (await dialog.Ask("Only blessed equipment can be upgraded. Super Tortoise Gems are required. After blessed attribute is upgraded, more\ndamages taken will be reduced. When blessed attribute reaches level 4 (the highest), the damage taken will be reduced by 7%.",
                    "How many super Tortoise Gems are required?") == 0)
                {
                    dialog.Say("If you want to upgrade your blessed attribute to level 2, you must have level-1 blessed equipment and a Super \nTortoise Gem. After it is upgraded to level 2, the damage taken will be reduced by 3%.\nIf you want to upgrade your blessed attribute to level 3, you must have level-2 blessed equipment and 3 Super \nTortoise Gems. After it is upgraded to level 3, the damage taken will be reduced by 5%.\nIf you want to upgrade your blessed attribute to level 4, you must have level-3 blessed equipment and 5 Super \nTortoise Gems. After it is upgraded to level 4, the damage taken will be reduced by 7%.",
                        "I see. Thanks.");
                }
                break;

            case 2:
                await dialog.Confirm("We use this magic oven to enchant your weapon with a gem, the enchanted weapon will increase your Maximum HP.",
                    "That`s marvelous.");

                switch (await dialog.Ask("The enchanted weapon can increase up to 255 HP. The higher quality gem will enchant higher HP. The enchanted effects\nalso vary with the type of the gem. Your luck is a very important factor, too.",
                    "Can my weapon be enchanted repeatedly?", "What gems are required?"))
                {
                    case 0:
                        dialog.Say("Sure. You can re-enchant your weapon. If the new attribute is higher, it will replace the old one, otherwise, it won`t replace.",
                            "I see. Thanks.");
                        break;

                    case 1:
                        dialog.Say("They can be Dragon, Phoenix, Tortoise, Rainbow, Moon, Violet, Fury, or Kylin. The enchanted effects vary with the gem quality.",
                            "I see. Thanks.");
                        break;
                }
                break;
        }
    }

    [Npc(35016)]
    public static void WuxingOven(Player player, Dialog dialog) => player.Post(ClientCommand.Enchant);

    [Npc(35501)]
    public static void ArtisanRow(Player player, Dialog dialog) => player.OpenDialog(1);

    [Npc(300000)]
    public static async Task Shelby(Player player, Dialog dialog)
    {
        dialog.Avatar = 29;

        switch (await dialog.Ask("We hope all can help each other. If you power level the newbies, we may reward you DragonBalls or Meteors. Are you interested?",
            "Tell me more details.", "Check my virtue points.", "Claim Prize.", "Just passing by."))
        {
            case 0:
                await dialog.Confirm("If you are above level 70 and try to power level the newbies (at least 20 levels lower than you), you may gain virtue points.",
                    "What are the virtue points?");

                switch (await dialog.Ask("The more newbies you power level, the more virtue points you gain. I shall give you a good reward for a certain virtue points.",
                    "How can I gain virtue points?", "What prize can I expect?"))
                {
                    case 0:
                        dialog.Say("Once the newbies are one level up, the team captain can gain virtue points accordingly.",
                            "I see.");
                        break;

                    case 1:
                        dialog.Say("I shall reward you a DragonBall for 270'000 virtue points or a meteor for 5000 virtue points.",
                            "I see.");
                        break;
                }
                break;

            case 1:
                dialog.Say($"You currently have {player.Virtue} virtue points.", "Thanks.");
                break;

            case 2:
                var choice = await dialog.Ask("What prize do you prefer?", "Meteor", "DragonBall.",
                    "Let me think it over");

                if (choice > 1)
                {
                    return;
                }

                uint amount = choice switch { 0 => 5000, _ => 270000 };
                uint itemTypeId = choice switch { 0 => ItemType.Meteor, _ => ItemType.DragonBall };

                if (player.Virtue < amount)
                {
                    dialog.Say("Sorry, you do not have the required virtue points.", "I see.");
                }
                else
                {
                    player.Give(itemTypeId);
                    player.SpendVirtue(amount);
                }
                break;
        }
    }

    [Npc(350050)]
    public static async Task CelestialTao(Player player, Dialog dialog)
    {
        dialog.Avatar = 29;

        await dialog.Confirm("Reborn level 70+ players can redistribute their attribute points at the cost of a dragonball.",
            "I will reallot my points.", "Let me think it over.");

        if (player.Rebirths == 0)
        {
            dialog.Say("You cannot reallot your ability points unless you are reborn and above level 70.", "I see.");
        }
        else if (player.Level < 70)
        {
            dialog.Say("Although you are reborn, you have not reached level 70. Please put in more efforts.", "I see.");
        }
        else if (player.Has(ItemType.DragonBall))
        {
            if (player.GoToRevivalPoint() && player.Take(ItemType.DragonBall))
            {
                dialog.Say("Congratulations! You can reallot your ability points.", "Thanks.");
            }
        }
        else
        {
            dialog.Say("Sorry, you do not have a dragonball.", "I see.");
        }
    }

    [Npc(600055)]
    public static async Task Starlit(Player player, Dialog dialog, GameStore store)
    {
        dialog.Avatar = 6;

        if (await dialog.Ask("One star in the sky represents a person. When a star falls, one person will disappear. Then I will make up a new one.",
            "You are great.", "Can you give me a star?") != 1)
        {
            return;
        }

        if (await dialog.Ask("Every star represents a person on the earth. How can I give it to you? If you have nothing special, please do not disturb me.",
            "I am leaving.", "What are you busy with?") != 1)
        {
            return;
        }

        await dialog.Confirm("Couples rely on each other as their stars in the sky do. When they do not get along well with each other, I shall...",
            "What will you do?");

        await dialog.Confirm("I will make their stars move apart until they can no longer shine in each other. Then they will break up finally.",
            "Why are you so cruel?", "I see.");

        await dialog.Confirm("I do not take them apart unless they have suffered a lot from their marriage and decide to end up their relationship.",
            "Can you help me divorce?", "I see.");

        await dialog.Confirm("If your marriage left you nothing but regret and pain, then bring a meteor and a meteor tear, I can help you out.",
            "Why are they needed?", "I love my spouse.");

        await dialog.Confirm("A star stays in the sky, then the person can revive. If it falls down as a meteor, he/she cannot, and I will get it back.",
            "What is Meteor Tear for?");

        await dialog.Confirm("The true love will move a meteor to tears, thus Meteor Tear is formed. I do not take Meteor Tear for myself.",
            "For whom then?");

        if (await dialog.Ask("I shall give it to your spouse as comfort. I hope he/she will know the world is still full of love, and live happily.",
            "I see.", "Get divorced.") != 1)
        {
            return;
        }

        if (!player.IsMarried)
        {
            dialog.Say("You are still single. Do not play such joke on me.", "Just a joke.");
            return;
        }

        await dialog.Confirm("The lovers should care for each other. Do not draw a conclusion in haste. You had better think it over.",
            "I have made up my mind.", "Let me think it over.");

        await dialog.Confirm("Are you sure that you want to leave your lover?", "Yeah.", "I changed my mind.");

        await dialog.Confirm("Since you have made up your mind, I shall help you and take away your Meteor Tear and Meteor.",
            "Thanks.", "I changed my mind.");

        if (!player.Has(ItemType.Meteor) || !player.Has(1088002))
        {
            dialog.Say("Sorry, you do not have a meteor and meteor tear.", "I see.");
            return;
        }

        player.Take(ItemType.Meteor);
        player.Take(1088002);

        dialog.Say("You divorced your spouse. I hope you can think it over before you get married next time. Do not come for divorce any more.",
            "Thanks.");

        if (player.Divorce() is (var spouse, { } tear))
        {
            await store.DivorceAsync(spouse, tear);
        }
    }
}
