namespace Conquer.Game.Scripts.Items;

public static class Packs
{
    private static readonly Dictionary<uint, (string Name, byte Type, uint Look, int Limit)> Furniture = new()
    {
        [721177] = ("WallLamp", 25, 10, 8),
        [721178] = ("LowBookshelf", 25, 21, 9),
        [721179] = ("LowCabinet", 25, 37, 9),
        [721180] = ("LongBookshelf", 25, 41, 9),
        [721181] = ("LowDesk", 25, 51, 9),
        [721182] = ("RosewoodCabinet", 25, 61, 9),
        [721183] = ("LongCabinet", 25, 71, 9),
        [721184] = ("FoldingScreen", 25, 81, 9),
        [721185] = ("Dresser", 25, 91, 9),
        [721186] = ("BasinRack", 25, 101, 9),
        [721187] = ("Chair", 25, 111, 9),
        [721188] = ("LowTable", 25, 121, 9),
        [721222] = ("Lamp", 101, 8300, 9),
        [721223] = ("Zither", 101, 8310, 9),
        [721224] = ("Harp", 101, 8320, 9),
        [721225] = ("GreenPearl", 25, 201, 9),
        [721226] = ("Fountain", 25, 210, 9),
        [721227] = ("Swing", 25, 221, 9),
        [721228] = ("Windmill", 25, 231, 9),
        [721229] = ("Hobbyhorse", 25, 131, 9),
        [721230] = ("Rockery", 25, 140, 9),
        [721231] = ("StoneStatue", 25, 151, 9),
        [721232] = ("StoneLion", 25, 161, 9),
        [721233] = ("BlackDragon", 25, 171, 9),
        [721234] = ("BronzeTurtle", 25, 181, 9),
        [721235] = ("Phoenix", 25, 191, 9)
    };

    [Item(700102, 720027)]
    public static void MeteorScroll(Player player, Dialog dialog)
    {
        dialog.Avatar = 29;

        if (player.FreeSlots >= 10)
        {
            if (player.Take(720027) && player.Give(ItemType.Meteor, 10))
            {
                player.Inform("You open the MeteorScroll and get 10 meteors.");
            }
        }
        else
        {
            dialog.Say("Please prepare 10 slots in your inventory before you open it.", "Ok, thanks.");
        }
    }

    [Item(700103, 720028)]
    public static void DBScroll(Player player, Dialog dialog)
    {
        dialog.Avatar = 29;

        if (player.FreeSlots >= 10)
        {
            if (player.Take(720028) && player.Give(ItemType.DragonBall, 10))
            {
                player.Inform("You open the DragonScroll and get 10 Dragonballs.");
            }
        }
        else
        {
            dialog.Say("Please prepare 10 slots in your inventory before you open it.", "Ok, thanks.");
        }
    }

    [Item(720010)]
    public static void Amrita(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1000030, 3))
            {
                player.Take(720010);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720011)]
    public static void Panacea(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1002000, 3))
            {
                player.Take(720011);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720012)]
    public static void Ginseng(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1002010, 3))
            {
                player.Take(720012);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720013)]
    public static void Vanilla(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1002020, 3))
            {
                player.Take(720013);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720014)]
    public static void RecoveryPill(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1001030, 3))
            {
                player.Take(720014);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720015)]
    public static void SoulPill(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1001040, 3))
            {
                player.Take(720015);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720016)]
    public static void RefreshingPill(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1002030, 3))
            {
                player.Take(720016);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720017)]
    public static void ChantPill(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots >= 3)
        {
            if (player.Give(1002040, 3))
            {
                player.Take(720017);
            }
        }
        else
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "I see.");
        }
    }

    [Item(720020)]
    public static async Task StatueScroll(Player player, Item item, Dialog dialog)
    {
        await dialog.PickSpot(9, 1130, region: 3);

        if (dialog.Place(new() { Name = "Statue", Type = 9, Sort = 35, Look = 1130, OwnerType = 2, MaxHealth = 60000 },
            region: 3))
        {
            player.Take(item);
        }
    }

    [Item(720021, 720022, 720023, 720024)]
    public static async Task GuildTeleporter(Player player, Item item, Dialog dialog)
    {
        dialog.Avatar = 3;

        if (player.Map!.Info.Is(MapFlags.ChangeMapDisable))
        {
            dialog.Say("Sorry, you cannot place your Teleporter here.", "I see.");
            return;
        }

        var number = item.ItemTypeId - 720020;
        var look = 1447 + number * 10;
        var conductor = 814 + number;
        await dialog.PickSpot(15, look);

        var teleporter = new DynaNpc
        {
            Name = $"GuildTeleporter{number}",
            Type = 15,
            Sort = 1,
            Look = look,
            OwnerType = 2,
            LinkId = conductor,
            Task0 = 150100
        };

        if (dialog.Place(teleporter) && player.Take(item.ItemTypeId))
        {
            teleporter.OwnerId = 0;
            player.World.Mark(conductor, player);
        }
    }

    [Item(720030)]
    public static void Firework(Player player, Item item)
    {
        if (player.Stamina < 100)
        {
            player.Inform("Sorry, you can not use firework until you have full stamina.");
        }
        else
        {
            player.AddStamina(-100);
            player.Fireworks();
            player.Take(item);
        }
    }

    [Item(720031)]
    public static void EndlessLove(Player player, Item item)
    {
        if (player.Stamina < 100)
        {
            player.Inform("Sorry, you can not use firework until you have full stamina.");
        }
        else
        {
            player.AddStamina(-100);
            player.ShowEffect("firework-2love");
            player.Take(item);
        }
    }

    [Item(720032)]
    public static void MyWish(Player player, Item item)
    {
        if (player.Stamina < 100)
        {
            player.Inform("Sorry, you can not use firework until you have full stamina.");
        }
        else
        {
            player.AddStamina(-100);
            player.ShowEffect("firework-like");
            player.Take(item);
        }
    }

    [Item(720360)]
    public static void RoastTurkey(Player player)
    {
        player.AddHealth(player.Stats.MaxHealth);
        player.AddMana(player.Stats.MaxMana);

        if (player.GetProgress(10, 5) != 19)
        {
            player.AddProgress(10, 5, 1);
            player.Inform("You have eaten up a delicious roast turkey, which recovers your HP and MP.");
            player.Take(720360);
            return;
        }

        if (player.Transform(8040, 0, 22, 60))
        {
            player.World.Announce("Roast turkey is delicious, but you should not have eaten up so much. Now you have been turned into a turkey!",
                TalkChannel.Gm);
            player.SetProgress(10, 5, 0);
            player.Take(720360);
        }
    }

    [Item(720361)]
    public static void PumpkinsBag(Player player, Dialog dialog)
    {
        if (player.GetRegister(5) == 1)
        {
            player.Inform("You can not open the bag now, for it is only allowed to be opened every three minutes.");
        }
        else if (player.FreeSlots > 0)
        {
            player.ShowEffect("n-arrow");
            player.Alert($"{player.Name} opened Pumpkin`s bag and got a gift.");

            switch (player.Roll(1, 8))
            {
                case 1:
                    if (player.Give(720368) is null || !player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 2:
                    if (player.Give(720363) is null || !player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 3:
                    if (player.Give(720364) is null || !player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 4:
                    if (player.Give(720365) is null || !player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 5:
                    if (player.Give(720366) is null || !player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 6:
                    if (player.Give(720367) is null)
                    {
                        return;
                    }

                    player.World.Announce(1002, $"{player.Name} smiles with a bunch of roses in hand.",
                        TalkChannel.System);
                    player.World.Announce(1011, $"{player.Name} smiles with a bunch of roses in hand.",
                        TalkChannel.System);
                    player.World.Announce(1020, $"{player.Name} smiles with a bunch of roses in hand.",
                        TalkChannel.System);
                    player.World.Announce(1015, $"{player.Name} smiles with a bunch of roses in hand.",
                        TalkChannel.System);
                    player.World.Announce(1000, $"{player.Name} smiles with a bunch of roses in hand.",
                        TalkChannel.System);

                    if (!player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 7:
                    if (player.Give(720362) is null || !player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;

                case 8:
                    player.World.Announce(1002,
                        $"{player.Name} is attacked by Pumpkin`s cake, and in extreme panic now!", TalkChannel.System);
                    player.World.Announce(1011,
                        $"{player.Name} is attacked by Pumpkin`s cake, and in extreme panic now!", TalkChannel.System);
                    player.World.Announce(1020,
                        $"{player.Name} is attacked by Pumpkin`s cake, and in extreme panic now!", TalkChannel.System);
                    player.World.Announce(1015,
                        $"{player.Name} is attacked by Pumpkin`s cake, and in extreme panic now!", TalkChannel.System);
                    player.World.Announce(1000,
                        $"{player.Name} is attacked by Pumpkin`s cake, and in extreme panic now!", TalkChannel.System);
                    player.Inform("A cake flies from the bag, hitting your face. While Berk is laughing coldly.");

                    if (!player.Take(720361))
                    {
                        return;
                    }

                    player.SetRegister(5, 1);

                    dialog.Later(180, () =>
                    {
                        player.SetRegister(5, 0);
                    });
                    break;
            }
        }
        else
        {
            player.Inform("Please prepare slots before you open the bag.");
        }
    }

    [Item(720362)]
    public static void FatPumpkin(Player player)
    {
        if (player.Take(720362))
        {
            player.Inform("It looks delicious, but actually nothing special. In a word, never judge only by appearance.");
        }
    }

    [Item(720363)]
    public static void Spinach(Player player)
    {
        if (player.Take(720363))
        {
            player.AddHealth(player.Stats.MaxHealth);
            player.AddMana(player.Stats.MaxMana);
            player.Inform("You have recovered your HP and mana by eating spinach.");
        }
    }

    [Item(720364)]
    public static void BeanStalk(Player player)
    {
        if (player.Take(720364))
        {
            player.AddHealth(player.Stats.MaxHealth);
            player.AddMana(player.Stats.MaxMana);
            player.Inform("Your HP and mana have been recovered after eating Bean");
        }
    }

    [Item(720365)]
    public static void Shampoo(Player player)
    {
        if (player.Take(720365))
        {
            player.SetHair((ushort)(player.Hair - player.Hair % 100 + 0));
            player.World.Announce(1002, $"{player.Name} becomes bald after using the unknown Shampoo.",
                TalkChannel.System);
            player.World.Announce(1011, $"{player.Name} becomes bald after using the unknown Shampoo.",
                TalkChannel.System);
            player.World.Announce(1020, $"{player.Name} becomes bald after using the unknown Shampoo.",
                TalkChannel.System);
            player.World.Announce(1015, $"{player.Name} becomes bald after using the unknown Shampoo.",
                TalkChannel.System);
            player.World.Announce(1000, $"{player.Name} becomes bald after using the unknown Shampoo.",
                TalkChannel.System);
        }
    }

    [Item(720366)]
    public static void LoveLetter(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (!player.Take(720366))
        {
            return;
        }

        player.ShowEffect("firework-2love");

        switch (player.Roll(1, 8))
        {
            case 1 or 4 or 7:
                dialog.Say("It is difficult to love a person, but it is much more difficult for a pumpkin. I have no idea whether I should\ngive up or wait for your love. I am a common pumpkin, who never draw others` attention. When I was a little flower, I\ndreamed of romantic love. Once, I fell in love with an industrious bee, but was deeply hurt. Then I gave all my\nlove to the gentle woolly bear but was bitten all around, only to see him flying away with a beautiful butterfly.\nHowever, on that happy day, you came to me, and came into my life. You aroused deep love in my heart, but left\nsoon with my heart-piercing emotion. I miss you so much. Days and nights, I dream that one day, you walk towards\nme, waving your hand, with attracting smile while I walk towards you with my arms wide opened, then we hug\ntogether, tightly, and happily. It is only a daydream, I know you will not come back.",
                    []);
                break;

            case 2 or 5 or 8:
                dialog.Say("Love is a hard pursuit. You run after me behind, while I escape ahead. Different person has different feeling,\ndifferent comfort varying from person to person. People court me for abstract desire. And I just run and escape.\nUntil one day when I encountered with you. I know I will not wait in vain. I find you at last, my love. I am\nso excited and eager to walk together with you all the way through my life. I am satisfied but sorrow, for I\nchoose to escape from my deep love. I could not stand being rejected any more. I am so coward that I do not\ndare to imagine that you, my lover, will cook me into pumpkin soup. Nevertheless, I know I will not be lonely\n any longer. Because, you are there. My heart will go wherever you go.",
                    []);
                break;

            case 3 or 6:
                dialog.Say("Love is beautiful, wonderful and sweet feeling. When I met you at the first sight, I fell in love with you. You are so\nconspicuous and attracting in crowds. Your beauty and your elegance choked me. My heart beat fast. I can not move my\neyes from you. Oh, my love, who brings you to me? You are my angel sent by God. I pray with your sweet words in\nmy ear. I am indulged in it and mad at it, in spite of my knowledge that it is just lying. I am willing to give all\nI have to you, my whole heart and all my love. Do you know my heart smiles at thinking of you, like a blooming\npumpkin flower Even in cold days, I feel like a warm and delighting day in spring. You leave cheerfully, and\nsatisfied with things you want in your hand, without turning back. I will never regret. But my heart is broken.\nIt is being pierced by a overwhelming pain.",
                    []);
                break;
        }

        player.World.Announce(1002, $"{player.Name} has received a love letter from Pumpkin.", TalkChannel.System);
        player.World.Announce(1011, $"{player.Name} has received a love letter from Pumpkin.", TalkChannel.System);
        player.World.Announce(1020, $"{player.Name} has received a love letter from Pumpkin.", TalkChannel.System);
        player.World.Announce(1015, $"{player.Name} has received a love letter from Pumpkin.", TalkChannel.System);
        player.World.Announce(1000, $"{player.Name} has received a love letter from Pumpkin.", TalkChannel.System);
    }

    [Item(720368)]
    public static void NaughtyPumpkin(Player player)
    {
        if (!player.Take(720368))
        {
            return;
        }

        if (player.Roll(10) < 1)
        {
            if (player.Give(720362) is not null)
            {
                player.World.Announce(1002, $"At last, {player.Name} gives love to Pumpkin.", TalkChannel.System);
                player.World.Announce(1011, $"At last, {player.Name} gives love to Pumpkin.", TalkChannel.System);
                player.World.Announce(1020, $"At last, {player.Name} gives love to Pumpkin.", TalkChannel.System);
                player.World.Announce(1015, $"At last, {player.Name} gives love to Pumpkin.", TalkChannel.System);
                player.World.Announce(1000, $"At last, {player.Name} gives love to Pumpkin.", TalkChannel.System);
            }
        }
        else if (player.Transform(8043, 0, 31, 60))
        {
            player.World.Announce(1002, $"{player.Name} was turned into a Roaring Ape after eating Naughty Pumpkin.",
                TalkChannel.System);
            player.World.Announce(1011, $"{player.Name} was turned into a Roaring Ape after eating Naughty Pumpkin.",
                TalkChannel.System);
            player.World.Announce(1020, $"{player.Name} was turned into a Roaring Ape after eating Naughty Pumpkin.",
                TalkChannel.System);
            player.World.Announce(1015, $"{player.Name} was turned into a Roaring Ape after eating Naughty Pumpkin.",
                TalkChannel.System);
            player.World.Announce(1000, $"{player.Name} was turned into a Roaring Ape after eating Naughty Pumpkin.",
                TalkChannel.System);
        }
    }

    [Item(720369)]
    public static void ThanksgivingBag(Player player)
    {
        if (player.FreeSlots < 5)
        {
            player.Inform("There is no enough space in your bag, please leave five slots.");
            return;
        }

        if (!player.Take(720369))
        {
            return;
        }

        if (player.Roll(200) < 1)
        {
            if (player.Give(137370) is not null)
            {
                player.World.Announce(1002, $"Congratulations! {player.Name} has got a Dark Robe!", TalkChannel.System);
                player.World.Announce(1011, $"Congratulations! {player.Name} has got a Dark Robe!", TalkChannel.System);
                player.World.Announce(1020, $"Congratulations! {player.Name} has got a Dark Robe!", TalkChannel.System);
                player.World.Announce(1015, $"Congratulations! {player.Name} has got a Dark Robe!", TalkChannel.System);
                player.World.Announce(1000, $"Congratulations! {player.Name} has got a Dark Robe!", TalkChannel.System);
            }
        }
        else if (player.Give(720371, 4))
        {
            player.Give(720371);
        }
    }

    [Item(720370)]
    public static void PremiumRoastTurkey(Player player)
    {
        if (player.Take(720370))
        {
            player.AddHealth(player.Stats.MaxHealth);
            player.AddMana(player.Stats.MaxMana);
            player.Transform(8040, 0, 22, 60);
        }
    }

    [Item(720371)]
    public static void EvilPumpkin(Player player)
    {
        if (!player.Take(720371))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case 1 or 5:
                player.Transform(8041, 0, 3560, 60);
                player.World.Announce(1002, $"{player.Name} has turned into a Stone Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1011, $"{player.Name} has turned into a Stone Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1020, $"{player.Name} has turned into a Stone Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1015, $"{player.Name} has turned into a Stone Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1000, $"{player.Name} has turned into a Stone Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                break;

            case 2 or 6:
                player.Transform(8042, 0, 3563, 60);
                player.World.Announce(1002, $"{player.Name} has turned into a Fire Snake after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1011, $"{player.Name} has turned into a Fire Snake after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1020, $"{player.Name} has turned into a Fire Snake after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1015, $"{player.Name} has turned into a Fire Snake after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1000, $"{player.Name} has turned into a Fire Snake after eating Evil Pumpkin.",
                    TalkChannel.System);
                break;

            case 3 or 7:
                player.Transform(8044, 0, 3564, 60);
                player.World.Announce(1002, $"{player.Name} has turned into a Sand Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1011, $"{player.Name} has turned into a Sand Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1020, $"{player.Name} has turned into a Sand Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1015, $"{player.Name} has turned into a Sand Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1000, $"{player.Name} has turned into a Sand Monster after eating Evil Pumpkin.",
                    TalkChannel.System);
                break;

            case 4 or 8:
                player.Transform(8043, 0, 31, 60);
                player.World.Announce(1002, $"{player.Name} has turned into a Roaring Ape after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1011, $"{player.Name} has turned into a Roaring Ape after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1020, $"{player.Name} has turned into a Roaring Ape after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1015, $"{player.Name} has turned into a Roaring Ape after eating Evil Pumpkin.",
                    TalkChannel.System);
                player.World.Announce(1000, $"{player.Name} has turned into a Roaring Ape after eating Evil Pumpkin.",
                    TalkChannel.System);
                break;
        }
    }

    [Item(720372)]
    public static void ChristmasBagForNine(Player player)
    {
        if (!player.Has(721501) || !player.Has(720378) || !player.Has(720379) || !player.Has(720380) ||
            !player.Has(720381) || !player.Has(720382) || !player.Has(720383) || !player.Has(720384))
        {
            player.Inform("You don`t have enough gifts to fill the Christmas bag.");
            return;
        }

        if (player.Take(721501) && player.Take(720378) && player.Take(720379) && player.Take(720380) &&
            player.Take(720381) && player.Take(720382) && player.Take(720383) && player.Take(720384) &&
            player.Take(720372) && player.Give(720375) is not null)
        {
            player.Inform("Congratulations! You`ve got a Gift Bag!");
        }
    }

    [Item(720373)]
    public static void ChristmasBagForEight(Player player)
    {
        if (!player.Has(721501) || !player.Has(720378) || !player.Has(720379) || !player.Has(720380) ||
            !player.Has(720381) || !player.Has(720382) || !player.Has(720383) || !player.Has(720384))
        {
            player.Inform("You don`t have enough gifts to fill the Christmas bag.");
            return;
        }

        if (player.Take(721501) && player.Take(720378) && player.Take(720379) && player.Take(720380) &&
            player.Take(720381) && player.Take(720382) && player.Take(720383) && player.Take(720384) &&
            player.Take(720373) && player.Give(720376) is not null)
        {
            player.Inform("Congratulations! You`ve got a Gift Bag!");
        }
    }

    [Item(720374)]
    public static void GiftBagOfTen(Player player)
    {
        if (player.FreeSlots < 8)
        {
            player.Inform("You don`t have enough free space in your inventory.");
            return;
        }

        if (player.Take(720374) && player.Give(721501) is not null && player.Give(720378) is not null &&
            player.Give(720379) is not null && player.Give(720380) is not null && player.Give(720381) is not null &&
            player.Give(720382) is not null && player.Give(720383) is not null && player.Give(720384) is not null &&
            player.Give(720377) is not null)
        {
            player.Inform("You`ve opened a Gift Bag and got a lot of gifts.");
        }
    }

    [Item(720375)]
    public static void GiftBagOfNine(Player player)
    {
        if (player.FreeSlots < 8)
        {
            player.Inform("You don`t have enough free space in your inventory.");
            return;
        }

        if (player.Take(720375) && player.Give(721501) is not null && player.Give(720378) is not null &&
            player.Give(720379) is not null && player.Give(720380) is not null && player.Give(720381) is not null &&
            player.Give(720382) is not null && player.Give(720383) is not null && player.Give(720384) is not null &&
            player.Give(720372) is not null)
        {
            player.Inform("You`ve opened a Gift Bag and got a lot of gifts.");
        }
    }

    [Item(720376)]
    public static void GiftBagOfEight(Player player)
    {
        if (player.FreeSlots < 8)
        {
            player.Inform("You don`t have enough free space in your inventory.");
            return;
        }

        if (player.Take(720376) && player.Give(721501) is not null && player.Give(720378) is not null &&
            player.Give(720379) is not null && player.Give(720380) is not null && player.Give(720381) is not null &&
            player.Give(720382) is not null && player.Give(720383) is not null && player.Give(720384) is not null &&
            player.Give(720373) is not null)
        {
            player.Inform("You`ve opened a Gift Bag and got a lot of gifts.");
        }
    }

    [Item(720377)]
    public static void ChristmasBagForTen(Player player)
    {
        if (!player.Has(721501) || !player.Has(720378) || !player.Has(720379) || !player.Has(720380) ||
            !player.Has(720381) || !player.Has(720382) || !player.Has(720383) || !player.Has(720384))
        {
            player.Inform("You don`t have enough gifts to fill the Christmas bag.");
            return;
        }

        if (player.Take(721501) && player.Take(720378) && player.Take(720379) && player.Take(720380) &&
            player.Take(720381) && player.Take(720382) && player.Take(720383) && player.Take(720384) &&
            player.Take(720377) && player.Give(720374) is not null)
        {
            player.Inform("Congratulations! You`ve got a Gift Bag!");
        }
    }

    [Item(720378)]
    public static void ChristmasCandy(Player player)
    {
        player.AddMana(player.Stats.MaxMana);

        if (player.Take(720378))
        {
            player.Inform("Your MP has been recovered!");
        }
    }

    [Item(720379)]
    public static void Egg(Player player)
    {
        if (player.GetProgress(13, 6) == 0)
        {
            player.AddProgress(13, 6, 1);
            player.AddXp(15);

            if (player.Take(720379))
            {
                player.Stamp(13, 6);
                player.Inform("Your XP has been increased by 15 points. Please wait 2 minutes to use another one.");
                player.ShowEffect("zf2-e295");
            }
        }
        else if (player.SecondsSince(13, 6) >= 120)
        {
            player.AddXp(15);

            if (player.Take(720379))
            {
                player.Stamp(13, 6);
                player.Inform("Your XP has been increased by 15 points. Please wait 2 minutes to use another one.");
                player.ShowEffect("zf2-e295");
            }
        }
        else
        {
            player.Inform("You can`t use this item at present.");
        }
    }

    [Item(720380)]
    public static void ColorfulEgg(Player player)
    {
        if (player.Roll(2) < 1)
        {
            player.ShowEffect("zf2-e192");
        }
        else
        {
            player.ShowEffect("zf2-e355");
        }

        player.Take(720380);
    }

    [Item(720381)]
    public static void Fruit(Player player)
    {
        player.AddHealth(3000);

        if (player.Take(720381))
        {
            player.Inform("Your HP has been recovered!");
        }
    }

    [Item(720382)]
    public static void Bible(Player player)
    {
        if (player.PkPoints < 1)
        {
            player.Inform("You have zero PK points. You can`t use this item.");
        }
        else if (player.GetProgress(13, 7) == 0)
        {
            player.AddProgress(13, 7, 1);
            player.Inform("Your PK points has been reduced by 1 point. Please wait 2 minutes to use another one.");
            player.Stamp(13, 7);

            if (player.Take(720382))
            {
                player.ShowEffect("zf2-e121");
                player.SetPkPoints(player.PkPoints - 1);
            }
        }
        else if (player.SecondsSince(13, 7) >= 120)
        {
            player.Inform("Your PK points has been reduced by 1 point. Please wait 2 minutes to use another one.");
            player.Stamp(13, 7);

            if (player.Take(720382))
            {
                player.ShowEffect("zf2-e121");
                player.SetPkPoints(player.PkPoints - 1);
            }
        }
        else
        {
            player.Inform("You can`t use this item at present.");
        }
    }

    [Item(720383)]
    public static void LifeFlower(Player player)
    {
        if (player.GetProgress(13, 8) == 0)
        {
            player.AddProgress(13, 8, 1);
            player.AddMana(player.Stats.MaxMana);
            player.AddHealth(player.Stats.MaxHealth);

            if (player.Take(720383))
            {
                player.Inform("Your HP and MP have been recovered. Please wait 2 minutes to use another one.");
                player.ShowEffect("zf2-e129");
                player.Stamp(13, 8);
            }
        }
        else if (player.SecondsSince(13, 8) >= 120)
        {
            player.AddMana(player.Stats.MaxMana);
            player.AddHealth(player.Stats.MaxHealth);

            if (player.Take(720383))
            {
                player.Inform("Your HP and MP have been recovered. Please wait 2 minutes to use another one.");
                player.ShowEffect("zf2-e129");
                player.Stamp(13, 8);
            }
        }
        else
        {
            player.Inform("You can`t use this item at present.");
        }
    }

    [Item(720384)]
    public static void MagicBean(Player player)
    {
        player.AddHealth(2000);

        if (player.Take(720384))
        {
            player.Inform("Your HP has been recovered.");
        }
    }

    [Item(720386)]
    public static void CoarseStocking(Player player)
    {
        if (!player.World.Now.IsYearly(12, 25, new(6, 0), 12, 25, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 26, new(0, 0), 12, 26, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 27, new(0, 0), 12, 27, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 28, new(0, 0), 12, 28, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 29, new(0, 0), 12, 29, new(23, 59)))
        {
            player.Alert("There is no gift in your stocking.");
            return;
        }

        if (player.Take(720386) && player.Give(720390) is not null)
        {
            player.Alert("You`ve got a gift from your stocking.");
        }
    }

    [Item(720387)]
    public static void WeavingStocking(Player player)
    {
        if (!player.World.Now.IsYearly(12, 26, new(6, 0), 12, 26, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 27, new(0, 0), 12, 27, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 28, new(0, 0), 12, 28, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 29, new(0, 0), 12, 29, new(23, 59)))
        {
            player.Alert("There is no gift in your stocking.");
            return;
        }

        if (player.Take(720387) && player.Give(720390) is not null)
        {
            player.Alert("You`ve got a gift from your stocking.");
        }
    }

    [Item(720388)]
    public static void CottonStocking(Player player)
    {
        if (!player.World.Now.IsYearly(12, 27, new(6, 0), 12, 27, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 28, new(0, 0), 12, 28, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 29, new(0, 0), 12, 29, new(23, 59)))
        {
            player.Alert("There is no gift in your stocking.");
            return;
        }

        if (player.Take(720388) && player.Give(720390) is not null)
        {
            player.Alert("You`ve got a gift from your stocking.");
        }
    }

    [Item(720389)]
    public static void WoolStocking(Player player)
    {
        if (!player.World.Now.IsYearly(12, 28, new(6, 0), 12, 28, new(23, 59)) &&
            !player.World.Now.IsYearly(12, 29, new(0, 0), 12, 29, new(23, 59)))
        {
            player.Alert("There is no gift in your stocking.");
            return;
        }

        if (player.Take(720389) && player.Give(720390) is not null)
        {
            player.Alert("You`ve got a gift from your stocking.");
        }
    }

    [Item(720390)]
    public static void GiftStocking(Player player)
    {
        if (player.Roll(10) < 9)
        {
            if (player.Take(720390))
            {
                player.SetRegister(5, player.ExperienceForTime(300));
                player.GainExperience(player.GetRegister(5));
                player.Inform("Congratulations! You`ve got bonous exp.");
            }
        }
        else if (player.Take(720390))
        {
            player.SetRegister(5, player.ExperienceForTime(600));
            player.GainExperience(player.GetRegister(5));
            player.Inform("Congratulations! You`ve got bonous exp.");
        }
    }

    [Item(720391)]
    public static async Task ChristmasWreath(Player player, Dialog dialog)
    {
        if (!player.Map!.Info.Is(MapFlags.House))
        {
            player.Tell("You can`t place a Christmas Wreath out of your house.");
            return;
        }

        if (player.Map!.OwnerId != player.Id)
        {
            player.Tell("You can`t place a Christmas Wreath in other people`s house.");
            return;
        }

        await dialog.PickSpot(25, 570);

        if (dialog.Place(new() { Name = "ChristmasWreath", Type = 25, Sort = 32, Look = 570, OwnerType = 1, Base = 1 }) &&
            player.Take(720391))
        {
            player.Inform("Congratulations! You`ve placed a Christmas Wreath in your house.");
        }
    }

    [Item(720392)]
    public static async Task ChristmasTree(Player player, Dialog dialog)
    {
        if (!player.Map!.Info.Is(MapFlags.House))
        {
            player.Tell("You can`t place a Christmas Tree out of your house.");
            return;
        }

        if (player.Map!.OwnerId != player.Id)
        {
            player.Tell("You can`t place a Christmas Tree in other people`s house.");
            return;
        }

        await dialog.PickSpot(25, 540);

        if (dialog.Place(new() { Name = "ChristmasTree", Type = 25, Sort = 32, Look = 540, OwnerType = 1, Base = 1 }) &&
            player.Take(720392))
        {
            player.Inform("Congratulations! You`ve placed a Christmas Tree in your house.");
        }
    }

    [Item(720393)]
    public static void ExpPill(Player player)
    {
        if (player.Take(720393))
        {
            player.Inform("From now on, you can get triple experience for the next two hours.");
            player.BoostExperience(300, 7200);
        }
    }

    [Item(720394)]
    public static void ExpAmrita(Player player)
    {
        if (player.Take(720394))
        {
            player.Inform("From now on, you can get quintuple experience for the next two hours.");
            player.BoostExperience(500, 7200);
        }
    }

    [Item(721020)]
    public static void MoonBoxUniqueDeerskinBoots(Player player)
    {
        if (player.Give(160037) is not null)
        {
            player.Take(721020);
        }
    }

    [Item(721021)]
    public static void MoonBoxSocketedRefinedDeerskinBoots(Player player)
    {
        if (player.Give(160036, 1,
            item => { item.Durability = 2099; item.MaxDurability = 2099; item.Gem1 = (Gem)255; }))
        {
            player.Take(721021);
        }
    }

    [Item(721022)]
    public static void MoonBoxRefinedSoftBoots(Player player)
    {
        if (player.Give(160056) is not null)
        {
            player.Take(721022);
        }
    }

    [Item(721023)]
    public static void MoonBoxUniqueSoftBoots(Player player)
    {
        if (player.Give(160057) is not null)
        {
            player.Take(721023);
        }
    }

    [Item(721024)]
    public static void MoonBoxRefinedThreadNecklace(Player player)
    {
        if (player.Give(120046) is not null)
        {
            player.Take(721024);
        }
    }

    [Item(721025)]
    public static void MoonBoxUniqueThreadNecklace(Player player)
    {
        if (player.Give(120047) is not null)
        {
            player.Take(721025);
        }
    }

    [Item(721030)]
    public static void MoonBoxUniqueMaskBag(Player player)
    {
        if (player.Give(121047) is not null)
        {
            player.Take(721030);
        }
    }

    [Item(721031)]
    public static void MoonBoxSocketedRefinedMaskBag(Player player)
    {
        if (player.Give(121046, 1,
            item => { item.Durability = 3099; item.MaxDurability = 3099; item.Gem1 = (Gem)255; }))
        {
            player.Take(721031);
        }
    }

    [Item(721032)]
    public static void MoonBoxRefinedPurpleBag(Player player)
    {
        if (player.Give(121066) is not null)
        {
            player.Take(721032);
        }
    }

    [Item(721033)]
    public static void MoonBoxUniquePurpleBag(Player player)
    {
        if (player.Give(121067) is not null)
        {
            player.Take(721033);
        }
    }

    [Item(721034)]
    public static void MoonBoxRefinedSilverRing(Player player)
    {
        if (player.Give(150036) is not null)
        {
            player.Take(721034);
        }
    }

    [Item(721035)]
    public static void MoonBoxUniqueSilverRing(Player player)
    {
        if (player.Give(150037) is not null)
        {
            player.Take(721035);
        }
    }

    [Item(721040)]
    public static void MoonBoxUniquePureBracelet(Player player)
    {
        if (player.Give(152047) is not null)
        {
            player.Take(721040);
        }
    }

    [Item(721041)]
    public static void MoonBoxSocketedRefinedPureBracelet(Player player)
    {
        if (player.Give(152046, 1,
            item => { item.Durability = 2599; item.MaxDurability = 2599; item.Gem1 = (Gem)255; }))
        {
            player.Take(721041);
        }
    }

    [Item(721042)]
    public static void MoonBoxRefinedBrassBracelet(Player player)
    {
        if (player.Give(152066) is not null)
        {
            player.Take(721042);
        }
    }

    [Item(721043)]
    public static void MoonBoxUniqueBrassBracelet(Player player)
    {
        if (player.Give(152067) is not null)
        {
            player.Take(721043);
        }
    }

    [Item(721051)]
    public static void MoonBoxMeteor(Player player)
    {
        if (player.Give(ItemType.Meteor) is not null)
        {
            player.Take(721051);
        }
    }

    [Item(721052)]
    public static void MoonBoxRefinedDeerskinBoots(Player player)
    {
        if (player.Give(160036) is not null)
        {
            player.Take(721052);
        }
    }

    [Item(721053)]
    public static void MoonBoxSocketedRefinedThreadNecklace(Player player)
    {
        if (player.Give(120046, 1,
            item => { item.Durability = 3099; item.MaxDurability = 3099; item.Gem1 = (Gem)255; }))
        {
            player.Take(721053);
        }
    }

    [Item(721054)]
    public static void MoonBoxRefinedJadeNecklace(Player player)
    {
        if (player.Give(120066) is not null)
        {
            player.Take(721054);
        }
    }

    [Item(721055)]
    public static void MoonBoxUniqueJadeNecklace(Player player)
    {
        if (player.Give(120067) is not null)
        {
            player.Take(721055);
        }
    }

    [Item(721061)]
    public static void MoonBoxRefinedMaskBag(Player player)
    {
        if (player.Give(121046) is not null)
        {
            player.Take(721061);
        }
    }

    [Item(721062)]
    public static void MoonBoxSocketedRefinedSilverRing(Player player)
    {
        if (player.Give(150036, 1,
            item => { item.Durability = 2599; item.MaxDurability = 2599; item.Gem1 = (Gem)255; }))
        {
            player.Take(721062);
        }
    }

    [Item(721063)]
    public static void MoonBoxRefinedGoldRing(Player player)
    {
        if (player.Give(150056) is not null)
        {
            player.Take(721063);
        }
    }

    [Item(721064)]
    public static void MoonBoxUniqueGoldRing(Player player)
    {
        if (player.Give(150057) is not null)
        {
            player.Take(721064);
        }
    }

    [Item(721065)]
    public static void MoonBoxRefinedPureBracelet(Player player)
    {
        if (player.Give(152046) is not null)
        {
            player.Take(721065);
        }
    }

    [Item(721080)]
    public static void MoonBoxElitePeachBracelet(Player player)
    {
        if (player.Give(152018) is not null)
        {
            player.Take(721080);
        }
    }

    [Item(721081)]
    public static void MoonBoxEliteDeerskinBoots(Player player)
    {
        if (player.Give(160038) is not null)
        {
            player.Take(721081);
        }
    }

    [Item(721082)]
    public static void MoonBoxEliteHeartNecklace(Player player)
    {
        if (player.Give(120028) is not null)
        {
            player.Take(721082);
        }
    }

    [Item(721083)]
    public static void MoonBoxEliteFlowerBag(Player player)
    {
        if (player.Give(121028) is not null)
        {
            player.Take(721083);
        }
    }

    [Item(721084)]
    public static void MoonBoxEliteSilverRing(Player player)
    {
        if (player.Give(150038) is not null)
        {
            player.Take(721084);
        }
    }

    [Item(721090)]
    public static void MoonBoxRandomPrize(Player player, Dialog dialog)
    {
        dialog.Avatar = 9;

        if (player.FreeSlots < 3)
        {
            dialog.Say("Please leave 3 free slots in your inventory first.", "Okay.");
            return;
        }

        if (player.Roll(100) < 30)
        {
            if (player.Give(ItemType.Meteor, 3))
            {
                player.Take(721090);
            }
        }
        else if (player.Roll(100) < 58)
        {
            var itemTypeId = player.Pick<uint>(160036, 120046, 150036, 152046, 121046);

            if (player.Give(itemTypeId) is not null && player.Give(ItemType.Meteor) is not null)
            {
                player.Take(721090);
            }
        }
        else if (player.Roll(100) < 63)
        {
            var itemTypeId = player.Pick<uint>(160057, 120067, 150057, 152067, 121067);

            if (player.Give(itemTypeId) is not null)
            {
                player.Take(721090);
            }
        }
        else if (player.Roll(100) < 95)
        {
            var itemTypeId = player.Pick<uint>(160058, 120068, 150058, 152068, 121068);

            if (player.Give(itemTypeId) is not null)
            {
                player.Take(721090);
            }
        }
        else
        {
            var itemTypeId = player.Pick<uint>(160036, 120046, 121046, 150036, 152046);

            if (player.Give(itemTypeId, 1,
                item => { item.Durability = 2099; item.MaxDurability = 2099; item.Gem1 = (Gem)255; }))
            {
                player.Take(721090);
            }
        }
    }

    [Item(721160)]
    public static void MoonCake1(Player player)
    {
        player.AddHealth(800);
        player.AddMana(800);
        player.Take(721160);
    }

    [Item(721163)]
    public static void MoonCake2(Player player)
    {
        player.AddHealth(player.Stats.MaxHealth);
        player.Take(721163);
    }

    [Item(721164)]
    public static void MoonCake3(Player player)
    {
        player.AddMana(player.Stats.MaxMana);
        player.Take(721164);
    }

    [Item(721165)]
    public static void HoneyWine(Player player)
    {
        player.AddHealth(player.Stats.MaxHealth);
        player.AddMana(player.Stats.MaxMana);
        player.Take(721165);
    }

    [Item(721177, 721178, 721179, 721180, 721181, 721182, 721183, 721184, 721185, 721186, 721187, 721188, 721222, 721223, 721224, 721225, 721226,
        721227, 721228, 721229, 721230, 721231, 721232, 721233, 721234, 721235)]
    public static async Task PlaceFurniture(Player player, Item item, Dialog dialog)
    {
        var (name, type, look, limit) = Furniture[item.ItemTypeId];

        dialog.Avatar = 92;

        if (!player.Map!.Info.Is(MapFlags.House))
        {
            player.Tell("You cannot place your furniture outside your house.");
            return;
        }

        if (player.Map!.OwnerId != player.Id)
        {
            player.Tell("This house is not yours. You cannot place your furniture in this house.");
            return;
        }

        if (player.Home!.Level < 2)
        {
            dialog.Say("Sorry, only second-class house can be furnished with furniture.", "I see.", "Just passing by.");
        }
        else if (player.Home!.Level == 2)
        {
            if (player.Map!.CountNpcs(25) >= limit)
            {
                player.Tell("A second-class house can be furnished with 8 pieces of furniture.");
                return;
            }

            await dialog.PickSpot(type, look);

            if (dialog.Place(new() { Name = name, Type = type, Sort = 32, Look = look, OwnerType = 1, Base = 1 }))
            {
                player.Take(item.ItemTypeId);
            }
        }
    }

    [Item(721189)]
    public static async Task ItemBox(Player player, Dialog dialog)
    {
        if (!player.Map!.Info.Is(MapFlags.House))
        {
            player.Tell("You cannot place your furniture outside your house.");
            return;
        }

        if (player.Map!.OwnerId != player.Id)
        {
            player.Tell("This house is not yours. You cannot place your furniture in this house.");
            return;
        }

        if (player.Home!.Level < 2)
        {
            if (player.Map!.CountNpcs(4) >= 1)
            {
                player.Tell("You can put only one ItemBox in a level-one house.");
                return;
            }

            await dialog.PickSpot(4, 8200);

            var box = new DynaNpc
            {
                Name = "ItemBox",
                Type = 4,
                Sort = 33,
                Look = 8200,
                OwnerType = 1,
                Task0 = 8003010,
                Data3 = 20
            };

            if (dialog.Place(box))
            {
                player.Take(721189);
            }
        }
        else if (player.Home!.Level == 2)
        {
            if (player.Map!.CountNpcs(4) >= 2)
            {
                player.Tell("You can put only 2 Item Boxes in a second-class house.");
                return;
            }

            await dialog.PickSpot(4, 8200);

            var box = new DynaNpc
            {
                Name = "ItemBox",
                Type = 4,
                Sort = 33,
                Look = 8200,
                OwnerType = 1,
                Task0 = 8003010,
                Data3 = 20
            };

            if (dialog.Place(box))
            {
                player.Take(721189);
            }
        }
    }

    [Item(721271)]
    public static void HappyRabbit(Player player)
    {
        if (player.Take(721271))
        {
            player.ShowEffect("relive1");
            player.Transform(3320, 0, 3320, 1200);
        }
    }

    [Item(721273)]
    public static void LonghammerSkill(Player player, Item item, Dialog dialog)
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

    [Item(721274)]
    public static void HalbertSkill(Player player, Item item, Dialog dialog)
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

    [Item(721275)]
    public static void SpearSkill(Player player, Item item, Dialog dialog)
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

    [Item(721276)]
    public static void DaggerSkill(Player player, Item item, Dialog dialog)
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

    [Item(721277)]
    public static void WandSkill(Player player, Item item, Dialog dialog)
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

    [Item(721278)]
    public static void GlaiveSkill(Player player, Item item, Dialog dialog)
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

    [Item(721279)]
    public static void PoleaxeSkill(Player player, Item item, Dialog dialog)
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

    [Item(721280)]
    public static void HookSkill(Player player, Item item, Dialog dialog)
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

    [Item(721281)]
    public static void AxeSkill(Player player, Item item, Dialog dialog)
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

    [Item(721282)]
    public static void ClubSkill(Player player, Item item, Dialog dialog)
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

    [Item(721283)]
    public static void ScepterSkill(Player player, Item item, Dialog dialog)
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

    [Item(721284)]
    public static void WhipSkill(Player player, Item item, Dialog dialog)
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

    [Item(721285)]
    public static void MagicBox(Player player)
    {
        if (!player.Take(721285))
        {
            return;
        }

        if (player.Roll(100) < 3)
        {
            player.Give(721090);
        }
        else if (player.Roll(97) < 3)
        {
            player.Give(725016);
        }
        else if (player.Roll(94) < 7)
        {
            player.Give(722840);
        }
        else if (player.Roll(87) < 9)
        {
            player.Give(player.Pick<uint>(150310, 150320, 150320, 150310, 150310, 150320, 150310, 150320));
        }
        else if (player.Roll(78) < 15)
        {
            player.Give(422020);
        }
        else if (player.Roll(63) < 18)
        {
            player.Give(422010);
        }
        else if (player.Roll(45) < 20)
        {
            player.Give(422000);
        }
        else
        {
            player.Give(1002050);
        }
    }

    [Item(721287)]
    public static void SwordSkill(Player player, Item item, Dialog dialog)
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

    [Item(721288)]
    public static void HammerSkill(Player player, Item item, Dialog dialog)
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

    [Item(721500)]
    public static void ChristmasGiftBag(Player player)
    {
        if (player.Roll(10) < 9)
        {
            if (player.Take(721500) && player.Give(721501) is not null)
            {
                player.Fireworks();
                player.Alert($"Wish {player.Name} a Merry Christmas and Happy New Year.");
            }
        }
        else if (player.Roll(5) < 4)
        {
            if (player.Take(721500) && player.Give(115300) is not null)
            {
                player.Fireworks();
                player.Alert($"Wish {player.Name} a Merry Christmas and Happy New Year.");
            }
        }
        else if (player.Take(721500) && player.Give(115310) is not null)
        {
            player.Fireworks();
            player.Alert($"Wish {player.Name} a Merry Christmas and Happy New Year.");
        }
    }

    [Item(721501)]
    public static void CandyCane(Player player)
    {
        player.AddHealth(1000);
        player.AddMana(1000);
        player.Take(721501);
    }

    [Item(721505)]
    public static void RedPacket1(Player player)
    {
        if (player.Earn(5000) && player.Take(721505))
        {
            player.Inform($"{player.Name} received a 5000-silver Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721506)]
    public static void RedPacket2(Player player)
    {
        if (player.Earn(10000) && player.Take(721506))
        {
            player.Inform($"{player.Name} received a 10000-silver Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721507)]
    public static void RedPacket3(Player player)
    {
        if (player.Earn(50000) && player.Take(721507))
        {
            player.Inform($"{player.Name} received a 50000-silver Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721508)]
    public static void RedPacket4(Player player)
    {
        if (player.Earn(100000) && player.Take(721508))
        {
            player.Inform($"{player.Name} received a 100000-silver Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721509)]
    public static void RedPacket5(Player player)
    {
        if (player.Earn(1000000) && player.Take(721509))
        {
            player.Alert($"{player.Name} received a 1000000-silver Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721510)]
    public static void MeteorPack(Player player)
    {
        if (player.Give(ItemType.Meteor) is not null && player.Take(721510))
        {
            player.Alert($"{player.Name} received a Meteor Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721511)]
    public static void DragonPack(Player player)
    {
        if (player.Give(ItemType.DragonBall) is not null && player.Take(721511))
        {
            player.Alert($"{player.Name} received a Dragon Ball Red Packet from his/her friends. They wish {player.Name} all the best!");
            player.ShowEffect("zf2-e105");
        }
    }

    [Item(721540)]
    public static void AncestorBox(Player player)
    {
        if (player.FreeSlots < 4)
        {
            player.Inform("Please prepare at least four slots in your inventory.");
            return;
        }

        if (!player.Take(721540))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case 1:
                if (player.Give(ItemType.Meteor, 2))
                {
                    player.Give(ItemType.Meteor);
                }
                break;

            case 2:
                if (player.Give(ItemType.Meteor, 4))
                {
                    player.Give(ItemType.Meteor);
                }
                break;

            case 3:
                player.Give(ItemType.DragonBall);
                break;

            case 4:
                player.Give(700031);
                break;

            case 5:
                player.Give(700001);
                break;

            case 6:
                player.Give(700011);
                break;

            case 7:
                player.Give(700062);
                break;

            case 8:
                player.Give(700022);
                break;
        }
    }

    [Item(721541)]
    public static void SunBox(Player player)
    {
        if (player.FreeSlots < 4)
        {
            player.Inform("Please prepare at least four slots in your inventory.");
            return;
        }

        if (!player.Take(721541))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case <= 7:
                if (player.Give(ItemType.Meteor, 5))
                {
                    player.Inform("Congratulations! You received five meteors.");
                }
                break;

            case 8:
                switch (player.Roll(1, 8))
                {
                    case 1:
                        if (player.Give(ItemType.Meteor, 5))
                        {
                            player.Inform("Congratulations! You received five meteors.");
                        }
                        break;

                    case 2:
                        if (player.Give(150076, 1,
                            item => { item.Durability = 3099; item.MaxDurability = 3099; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You got a ring with one socket.");
                        }
                        break;

                    case 3:
                        if (player.Give(152086, 1,
                            item => { item.Durability = 3099; item.MaxDurability = 3099; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a GoldBracelet with one socket.");
                        }
                        break;

                    case 4:
                        if (player.Give(160076, 1,
                            item => { item.Durability = 3099; item.MaxDurability = 3099; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a EagleBoots with one socket.");
                        }
                        break;

                    case 5:
                        if (player.Give(120086, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a CrystalNecklace with one socket.");
                        }
                        break;

                    case 6:
                        if (player.Give(900506, 1,
                            item => { item.Durability = 3899; item.MaxDurability = 3899; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a SoftShield with one socket.");
                        }
                        break;

                    case 7:
                        if (player.Give(121086, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a BlueBag with one socket.");
                        }
                        break;

                    case 8:
                        if (player.Give(113916, 1,
                            item => { item.Durability = 4198; item.MaxDurability = 4198; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a CatHat with one socket.");
                        }
                        break;
                }
                break;
        }
    }

    [Item(721542)]
    public static void MoonBox(Player player)
    {
        if (!player.Take(721542))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case <= 7:
                if (player.Give(700062) is not null)
                {
                    player.Inform("Congratulations! You received a refine MoonGem.");
                }
                break;

            case 8:
                switch (player.Roll(1, 8))
                {
                    case 1:
                        if (player.Give(700062) is not null)
                        {
                            player.Inform("Congratulations! You received a refine MoonGem.");
                        }
                        break;

                    case 2:
                        if (player.Give(131436, 1,
                            item => { item.Durability = 3899; item.MaxDurability = 3899; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a SteelArmor with one socket.");
                        }
                        break;

                    case 3:
                        if (player.Give(133626, 1,
                            item => { item.Durability = 3799; item.MaxDurability = 3799; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a WolfCoat with one socket.");
                        }
                        break;

                    case 4:
                        if (player.Give(130836, 1,
                            item => { item.Durability = 3899; item.MaxDurability = 3899; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a receivedhicPlate with one socket.");
                        }
                        break;

                    case 5:
                        if (player.Give(134536, 1,
                            item => { item.Durability = 3998; item.MaxDurability = 3998; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a StarRobe with one socket.");
                        }
                        break;

                    case 6:
                        if (player.Give(111346, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a SyeniticHelmet with one socket.");
                        }
                        break;

                    case 7:
                        if (player.Give(114946, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a CloudCap with one socket.");
                        }
                        break;

                    case 8:
                        if (player.Give(117346, 1,
                            item => { item.Durability = 3698; item.MaxDurability = 3698; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a TasselEarring with one socket.");
                        }
                        break;
                }
                break;
        }
    }

    [Item(721543)]
    public static void StarBox(Player player)
    {
        if (!player.Take(721543))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case <= 7:
                if (player.Give(700032) is not null)
                {
                    player.Inform("Congratulations! You received a refined RainbowGem.");
                }
                break;

            case 8:
                switch (player.Roll(1, 8))
                {
                    case 1:
                        if (player.Give(700032) is not null)
                        {
                            player.Inform("Congratulations! You received a refined RainbowGem.");
                        }
                        break;

                    case 2:
                        if (player.Give(150097, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received an IvoryRing with one socket.");
                        }
                        break;

                    case 3:
                        if (player.Give(152107, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a VioletBracelet with one socket.");
                        }
                        break;

                    case 4:
                        if (player.Give(160097, 1,
                            item => { item.Durability = 4098; item.MaxDurability = 4098; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a pair of LightBoots with one socket.");
                        }
                        break;

                    case 5:
                        if (player.Give(120097, 1,
                            item => { item.Durability = 3999; item.MaxDurability = 3999; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a GoldNecklace with one socket.");
                        }
                        break;

                    case 6:
                        if (player.Give(900527, 1,
                            item => { item.Durability = 3599; item.MaxDurability = 3599; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received an OctagonalShield with one socket.");
                        }
                        break;

                    case 7:
                        if (player.Give(121097, 1,
                            item => { item.Durability = 3999; item.MaxDurability = 3999; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a BeanBag with one socket.");
                        }
                        break;

                    case 8:
                        if (player.Give(113927, 1,
                            item => { item.Durability = 4198; item.MaxDurability = 4198; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a JackalHat with one socket.");
                        }
                        break;
                }
                break;
        }
    }

    [Item(721544)]
    public static void CloudBox(Player player)
    {
        if (!player.Take(721544))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case <= 7:
                if (player.Give(700022) is not null)
                {
                    player.Inform("Congratulations! You received a refined FuryGem.");
                }
                break;

            case 8:
                switch (player.Roll(1, 8))
                {
                    case 1:
                        if (player.Give(700022) is not null)
                        {
                            player.Inform("Congratulations! You received a refined FuryGem.");
                        }
                        break;

                    case 2:
                        if (player.Give(131457, 1,
                            item => { item.Durability = 4098; item.MaxDurability = 4098; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a ChainedArmor with one socket.");
                        }
                        break;

                    case 3:
                        if (player.Give(133637, 1,
                            item => { item.Durability = 3998; item.MaxDurability = 3998; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a LeopardCoat with one socket.");
                        }
                        break;

                    case 4:
                        if (player.Give(130857, 1,
                            item => { item.Durability = 4098; item.MaxDurability = 4098; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a DemonArmor with one socket.");
                        }
                        break;

                    case 5:
                        if (player.Give(134457, 1,
                            item => { item.Durability = 4098; item.MaxDurability = 4098; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a PowerGown with one socket.");
                        }
                        break;

                    case 6:
                        if (player.Give(111357, 1,
                            item => { item.Durability = 3999; item.MaxDurability = 3999; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a PearlHelmet with one socket.");
                        }
                        break;

                    case 7:
                        if (player.Give(114957, 1,
                            item => { item.Durability = 3999; item.MaxDurability = 3999; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received a LotusCap with one socket.");
                        }
                        break;

                    case 8:
                        if (player.Give(117357, 1,
                            item => { item.Durability = 4098; item.MaxDurability = 4098; item.Gem1 = (Gem)255; }))
                        {
                            player.Inform("Congratulations! You received an ThreadEarring with one socket.");
                        }
                        break;
                }
                break;
        }
    }

    [Item(721545)]
    public static void SashTokenGiftPack(Player player)
    {
        if (player.FreeSlots < 2)
        {
            player.Inform("There is not enough room in your inventory. Please check your free slots.");
            return;
        }

        if (player.Take(721545) && player.Give(722840) is not null)
        {
            player.Give(ItemType.Meteor);
        }
    }

    [Item(722012)]
    public static void MysticBoxHousing(Player player)
    {
        if (player.Roll(5) < 3)
        {
            if (player.Give(721173) is not null)
            {
                player.Take(722012);
            }
        }
        else if (player.Roll(25) < 3)
        {
            if (player.Give(721170) is not null)
            {
                player.Take(722012);
            }
        }
        else if (player.Earn(50000))
        {
            player.ShowEffect("money");
            player.Take(722012);
        }
    }

    [Item(722013)]
    public static void MysticBoxPearlRing(Player player)
    {
        if (player.Roll(100) < 5)
        {
            if (player.Give(150137) is not null)
            {
                player.Take(722013);
            }
        }
        else if (player.Roll(100) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.Take(722013);
            }
        }
        else if (player.Give(ItemType.Meteor) is not null)
        {
            player.Take(722013);
        }
    }

    [Item(722014)]
    public static void MysticBoxBronzeClub(Player player)
    {
        if (player.Roll(100) < 5)
        {
            if (player.Give(480137) is not null)
            {
                player.Take(722014);
            }
        }
        else if (player.Roll(100) < 1)
        {
            if (player.Give(121127) is not null)
            {
                player.Take(722014);
            }
        }
        else if (player.Earn(30000))
        {
            player.ShowEffect("money");
            player.Take(722014);
        }
    }

    [Item(722015)]
    public static void MysticBoxSilver(Player player)
    {
        if (player.Roll(5) < 1)
        {
            if (player.Give(ItemType.Meteor) is not null)
            {
                player.Take(722015);
            }
        }
        else if (player.Roll(50) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.Take(722015);
            }
        }
        else if (player.Roll(100) < 5)
        {
            switch (player.Roll(1, 8))
            {
                case 1:
                    if (player.Give(700001) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 2:
                    if (player.Give(700011) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 3:
                    if (player.Give(700021) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 4:
                    if (player.Give(700031) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 5:
                    if (player.Give(700041) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 6:
                    if (player.Give(700051) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 7:
                    if (player.Give(700061) is not null)
                    {
                        player.Take(722015);
                    }
                    break;

                case 8:
                    if (player.Earn(60000))
                    {
                        player.ShowEffect("money");
                        player.Take(722015);
                    }
                    break;
            }
        }
        else if (player.Earn(60000))
        {
            player.ShowEffect("money");
            player.Take(722015);
        }
    }

    [Item(722016)]
    public static void MysticBoxMeteor(Player player)
    {
        if (player.Roll(100) < 1)
        {
            if (player.Give(150137) is not null)
            {
                player.Take(722016);
            }
        }
        else if (player.Roll(100) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.Take(722016);
            }
        }
        else if (player.Give(ItemType.Meteor) is not null)
        {
            player.Take(722016);
        }
    }

    [Item(722017)]
    public static void MysticBoxGems(Player player)
    {
        if (player.Roll(5) < 1)
        {
            if (player.Give(ItemType.Meteor) is not null)
            {
                player.Take(722017);
            }
        }
        else if (player.Roll(50) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.Take(722017);
            }
        }
        else if (player.Roll(500) < 1)
        {
            if (player.Earn(50000))
            {
                player.ShowEffect("money");
                player.Take(722017);
            }
        }
        else
        {
            switch (player.Roll(1, 8))
            {
                case 1:
                    if (player.Give(700002) is not null)
                    {
                        player.Take(722017);
                    }
                    break;

                case 2:
                    if (player.Give(700012) is not null)
                    {
                        player.Take(722017);
                    }
                    break;

                case 3:
                    if (player.Give(700022) is not null)
                    {
                        player.Take(722017);
                    }
                    break;

                case 4:
                    if (player.Give(700032) is not null)
                    {
                        player.Take(722017);
                    }
                    break;

                case 5:
                    if (player.Give(700042) is not null)
                    {
                        player.Take(722017);
                    }
                    break;

                case 6:
                    if (player.Give(700052) is not null)
                    {
                        player.Take(722017);
                    }
                    break;

                case 7:
                    if (player.Give(700062) is not null)
                    {
                        player.Take(722017);
                    }
                    break;
            }
        }
    }

    [Item(722018)]
    public static void MysticBoxAmbergrisBag(Player player)
    {
        if (player.Roll(50) < 1)
        {
            if (player.Give(ItemType.Meteor) is not null)
            {
                player.Take(722018);
            }
        }
        else if (player.Roll(100) < 1)
        {
            if (player.Give(121127) is not null)
            {
                player.Take(722018);
            }
        }
        else if (player.Earn(30000))
        {
            player.ShowEffect("money");
            player.Take(722018);
        }
    }

    [Item(722034)]
    public static void EggRope(Player player) => player.Inform("The magic of EggRope has gone with the Easter. You can not use it now.");

    [Item(722037)]
    public static void MagicEgg(Player player)
    {
        if (!player.Take(722037))
        {
            return;
        }

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                player.ShowEffect("bombarrow4");
                player.Transform(8056, 0, 3706, 30);
                break;

            case 3 or 4:
                player.ShowEffect("bombarrow1");
                player.Transform(8057, 0, 3707, 30);
                break;

            case 5 or 6:
                player.ShowEffect("bombarrow6");
                player.Transform(8058, 0, 3708, 30);
                break;

            case 7 or 8:
                player.ShowEffect("bombarrow7");
                player.Transform(8059, 0, 3709, 30);
                break;
        }
    }

    [Item(722167)]
    public static void MagicMirror(Player player, Dialog dialog)
    {
        if (!player.World.Now.IsBetween(new(2006, 10, 28, 0, 0, 0), new(2006, 10, 31, 23, 59, 0)))
        {
            player.Inform("The halloween has passed and there is no ghost any more.");
            return;
        }

        if (player.GetProgress(23, 1) != 0)
        {
            player.Inform("You cannot use the MagicMirror twice in 10 minutes, please try later.");
            return;
        }

        if (player.MapId == 1002)
        {
            if (player.X <= 398)
            {
                player.Inform("The MagicMirror did not respond, you may try the other places.");
            }
            else if (player.X <= 430)
            {
                if (player.X <= 412)
                {
                    if (player.Y <= 394)
                    {
                        if (player.X <= 408)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                        else if (player.X <= 420)
                        {
                            if (player.Y <= 374)
                            {
                                if (player.X <= 418 || player.X > 430 || player.Y <= 322 || player.Y > 334)
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                    return;
                                }

                                player.World.MoveNpc(3870, 1002, 424, 328);
                                ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                            }
                            else if (player.Y <= 386)
                            {
                                player.World.MoveNpc(3869, 1002, 414, 380);
                                ShowGhost(player, dialog, player.World.MoveNpc(3941, 1002, 415, 376));
                            }
                            else if (player.X > 418 && player.X <= 430 && player.Y > 322 && player.Y <= 334)
                            {
                                player.World.MoveNpc(3870, 1002, 424, 328);
                                ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                            }
                            else
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                        }
                        else if (player.X > 418 && player.X <= 430 && player.Y > 322 && player.Y <= 334)
                        {
                            player.World.MoveNpc(3870, 1002, 424, 328);
                            ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.Y <= 406)
                    {
                        player.World.MoveNpc(3868, 1002, 404, 400);
                        ShowGhost(player, dialog, player.World.MoveNpc(3938, 1002, 405, 395));
                    }
                    else if (player.X <= 408)
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                    else if (player.X <= 420)
                    {
                        if (player.Y <= 374)
                        {
                            if (player.X <= 418 || player.X > 430 || player.Y <= 322 || player.Y > 334)
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                                return;
                            }

                            player.World.MoveNpc(3870, 1002, 424, 328);
                            ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                        }
                        else if (player.Y <= 386)
                        {
                            player.World.MoveNpc(3869, 1002, 414, 380);
                            ShowGhost(player, dialog, player.World.MoveNpc(3941, 1002, 415, 376));
                        }
                        else if (player.X > 418 && player.X <= 430 && player.Y > 322 && player.Y <= 334)
                        {
                            player.World.MoveNpc(3870, 1002, 424, 328);
                            ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.X > 418 && player.X <= 430 && player.Y > 322 && player.Y <= 334)
                    {
                        player.World.MoveNpc(3870, 1002, 424, 328);
                        ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                    }
                    else
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                }
                else if (player.X <= 408)
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
                else if (player.X <= 420)
                {
                    if (player.Y <= 374)
                    {
                        if (player.X <= 418 || player.X > 430 || player.Y <= 322 || player.Y > 334)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                            return;
                        }

                        player.World.MoveNpc(3870, 1002, 424, 328);
                        ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                    }
                    else if (player.Y <= 386)
                    {
                        player.World.MoveNpc(3869, 1002, 414, 380);
                        ShowGhost(player, dialog, player.World.MoveNpc(3941, 1002, 415, 376));
                    }
                    else if (player.X > 418 && player.X <= 430 && player.Y > 322 && player.Y <= 334)
                    {
                        player.World.MoveNpc(3870, 1002, 424, 328);
                        ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                    }
                    else
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                }
                else if (player.X > 418 && player.X <= 430 && player.Y > 322 && player.Y <= 334)
                {
                    player.World.MoveNpc(3870, 1002, 424, 328);
                    ShowGhost(player, dialog, player.World.MoveNpc(3936, 1002, 426, 331));
                }
                else
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
            }
            else if (player.X <= 434)
            {
                player.Inform("The MagicMirror did not respond, you may try the other places.");
            }
            else if (player.X <= 468)
            {
                if (player.X <= 446)
                {
                    if (player.Y <= 343)
                    {
                        if (player.X <= 442)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                        else if (player.X <= 454)
                        {
                            if (player.Y <= 327)
                            {
                                if (player.X <= 452)
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                }
                                else if (player.X <= 464)
                                {
                                    if (player.Y <= 360)
                                    {
                                        if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                                        {
                                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                                            return;
                                        }

                                        player.World.MoveNpc(3920, 1002, 462, 341);
                                        ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                    }
                                    else if (player.Y <= 372)
                                    {
                                        player.World.MoveNpc(3919, 1002, 458, 366);
                                        ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                                    }
                                    else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                                    {
                                        player.World.MoveNpc(3920, 1002, 462, 341);
                                        ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                    }
                                    else
                                    {
                                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                                    }
                                }
                                else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                                {
                                    player.World.MoveNpc(3920, 1002, 462, 341);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                }
                                else
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                }
                            }
                            else if (player.Y <= 339)
                            {
                                player.World.MoveNpc(3918, 1002, 448, 333);
                                ShowGhost(player, dialog, player.World.MoveNpc(3942, 1002, 444, 331));
                            }
                            else if (player.X <= 452)
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                            else if (player.X <= 464)
                            {
                                if (player.Y <= 360)
                                {
                                    if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                                    {
                                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                                        return;
                                    }

                                    player.World.MoveNpc(3920, 1002, 462, 341);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                }
                                else if (player.Y <= 372)
                                {
                                    player.World.MoveNpc(3919, 1002, 458, 366);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                                }
                                else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                                {
                                    player.World.MoveNpc(3920, 1002, 462, 341);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                }
                                else
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                }
                            }
                            else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                            {
                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                        }
                        else if (player.X <= 452)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                        else if (player.X <= 464)
                        {
                            if (player.Y <= 360)
                            {
                                if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                    return;
                                }

                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else if (player.Y <= 372)
                            {
                                player.World.MoveNpc(3919, 1002, 458, 366);
                                ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                            }
                            else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                            {
                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                        }
                        else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                        {
                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.Y <= 355)
                    {
                        player.World.MoveNpc(3917, 1002, 440, 349);
                        ShowGhost(player, dialog, player.World.MoveNpc(3939, 1002, 444, 349));
                    }
                    else if (player.X <= 442)
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                    else if (player.X <= 454)
                    {
                        if (player.Y <= 327)
                        {
                            if (player.X <= 452)
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                            else if (player.X <= 464)
                            {
                                if (player.Y <= 360)
                                {
                                    if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                                    {
                                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                                        return;
                                    }

                                    player.World.MoveNpc(3920, 1002, 462, 341);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                }
                                else if (player.Y <= 372)
                                {
                                    player.World.MoveNpc(3919, 1002, 458, 366);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                                }
                                else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                                {
                                    player.World.MoveNpc(3920, 1002, 462, 341);
                                    ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                                }
                                else
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                }
                            }
                            else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                            {
                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                        }
                        else if (player.Y <= 339)
                        {
                            player.World.MoveNpc(3918, 1002, 448, 333);
                            ShowGhost(player, dialog, player.World.MoveNpc(3942, 1002, 444, 331));
                        }
                        else if (player.X <= 452)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                        else if (player.X <= 464)
                        {
                            if (player.Y <= 360)
                            {
                                if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                    return;
                                }

                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else if (player.Y <= 372)
                            {
                                player.World.MoveNpc(3919, 1002, 458, 366);
                                ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                            }
                            else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                            {
                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                        }
                        else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                        {
                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.X <= 452)
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                    else if (player.X <= 464)
                    {
                        if (player.Y <= 360)
                        {
                            if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                                return;
                            }

                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else if (player.Y <= 372)
                        {
                            player.World.MoveNpc(3919, 1002, 458, 366);
                            ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                        }
                        else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                        {
                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                    {
                        player.World.MoveNpc(3920, 1002, 462, 341);
                        ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                    }
                    else
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                }
                else if (player.X <= 442)
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
                else if (player.X <= 454)
                {
                    if (player.Y <= 327)
                    {
                        if (player.X <= 452)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                        else if (player.X <= 464)
                        {
                            if (player.Y <= 360)
                            {
                                if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                                {
                                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                                    return;
                                }

                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else if (player.Y <= 372)
                            {
                                player.World.MoveNpc(3919, 1002, 458, 366);
                                ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                            }
                            else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                            {
                                player.World.MoveNpc(3920, 1002, 462, 341);
                                ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                            }
                            else
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                            }
                        }
                        else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                        {
                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.Y <= 339)
                    {
                        player.World.MoveNpc(3918, 1002, 448, 333);
                        ShowGhost(player, dialog, player.World.MoveNpc(3942, 1002, 444, 331));
                    }
                    else if (player.X <= 452)
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                    else if (player.X <= 464)
                    {
                        if (player.Y <= 360)
                        {
                            if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                            {
                                player.Inform("The MagicMirror did not respond, you may try the other places.");
                                return;
                            }

                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else if (player.Y <= 372)
                        {
                            player.World.MoveNpc(3919, 1002, 458, 366);
                            ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                        }
                        else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                        {
                            player.World.MoveNpc(3920, 1002, 462, 341);
                            ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                        }
                        else
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                        }
                    }
                    else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                    {
                        player.World.MoveNpc(3920, 1002, 462, 341);
                        ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                    }
                    else
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                }
                else if (player.X <= 452)
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
                else if (player.X <= 464)
                {
                    if (player.Y <= 360)
                    {
                        if (player.X <= 456 || player.X > 468 || player.Y <= 335 || player.Y > 347)
                        {
                            player.Inform("The MagicMirror did not respond, you may try the other places.");
                            return;
                        }

                        player.World.MoveNpc(3920, 1002, 462, 341);
                        ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                    }
                    else if (player.Y <= 372)
                    {
                        player.World.MoveNpc(3919, 1002, 458, 366);
                        ShowGhost(player, dialog, player.World.MoveNpc(3937, 1002, 464, 366));
                    }
                    else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                    {
                        player.World.MoveNpc(3920, 1002, 462, 341);
                        ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                    }
                    else
                    {
                        player.Inform("The MagicMirror did not respond, you may try the other places.");
                    }
                }
                else if (player.X > 456 && player.X <= 468 && player.Y > 335 && player.Y <= 347)
                {
                    player.World.MoveNpc(3920, 1002, 462, 341);
                    ShowGhost(player, dialog, player.World.MoveNpc(3943, 1002, 466, 341));
                }
                else
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
            }
            else if (player.X <= 481)
            {
                if (player.Y <= 325)
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
                else if (player.Y <= 337)
                {
                    player.World.MoveNpc(3921, 1002, 475, 331);
                    ShowGhost(player, dialog, player.World.MoveNpc(3944, 1002, 480, 335));
                }
                else if (player.Y > 380 && player.Y <= 392)
                {
                    player.World.MoveNpc(3922, 1002, 475, 386);
                    ShowGhost(player, dialog, player.World.MoveNpc(3940, 1002, 473, 390));
                }
                else
                {
                    player.Inform("The MagicMirror did not respond, you may try the other places.");
                }
            }
            else
            {
                player.Inform("The MagicMirror did not respond, you may try the other places.");
            }
        }
        else if (player.MapId == 1511 && player.X > 57 && player.X <= 69 && player.Y > 32 && player.Y <= 44)
        {
            player.World.MoveNpc(3923, 1511, 63, 38);
            ShowGhost(player, dialog, player.World.MoveNpc(3935, 1511, 60, 36));
        }
        else
        {
            player.Inform("The MagicMirror did not respond, you may try the other places.");
        }
    }

    private static void ShowGhost(Player player, Dialog dialog, bool shown)
    {
        if (!shown)
        {
            player.Inform("There is weird power preventing you from using the MagicMirror.");
            return;
        }

        player.AddProgress(23, 1, 1);
        dialog.Later(600, () => player.SetProgress(23, 1, 0));

        player.Inform("After you held the MagicMirror, white mist wreathed, a weird form emerges from the mist.");
        player.ShowEffect("veil");

        if (player.GetProgress(23, 0) == 1)
        {
            player.SetProgress(23, 0, 2);
            player.Inform("You have found ghosts, now you can go back to report it to CaptainCheung.");
        }
    }

    [Item(722171)]
    public static void MagicDust(Player player)
    {
        if (!player.Take(722171))
        {
            return;
        }

        player.Inform("After the MagicDust being poured out, a ghost stood in flying dust, amazedly.");

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                player.ShowEffect("ghost");
                break;

            case 3 or 4:
                player.ShowEffect("BloodGhost-4");
                break;

            case 5 or 6:
                player.ShowEffect("zf2-e215");
                break;

            case 7 or 8:
                player.ShowEffect("posionbomb");
                break;
        }

        var (magicType, monsterTypeId) = player.Pick<(ushort, uint)>([(8096, 4032), (8097, 4033), (8098, 4034),
            (8099, 4035), (8100, 4036), (8101, 22), (8102, 5055), (8103, 5051)]);
        player.Transform(magicType, 0, monsterTypeId, 300);
    }

    [Item(722172)]
    public static void PhantomBag(Player player)
    {
        if (!player.Take(722172))
        {
            return;
        }

        player.Inform("You opened a PhantomBag and disguised into a ghost. Everyone saw you got scared and ran away.");

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                player.ShowEffect("ghost");
                break;

            case 3 or 4:
                player.ShowEffect("BloodGhost-4");
                break;

            case 5 or 6:
                player.ShowEffect("zf2-e215");
                break;

            case 7 or 8:
                player.ShowEffect("posionbomb");
                break;
        }

        var (magicType, monsterTypeId) = player.Pick<(ushort, uint)>([(8096, 4032), (8097, 4033), (8098, 4034),
            (8099, 4035), (8100, 4036), (8101, 22), (8102, 5055), (8103, 5051)]);
        player.Transform(magicType, 0, monsterTypeId, 300);
    }

    [Item(722173)]
    public static void PretenderCandy(Player player)
    {
        if (!player.Take(722173))
        {
            return;
        }

        player.Inform("You surprisedly find your body is disappearing after eating a PretenderCandy. At last, you disguised into a horrible ghost.");

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                player.ShowEffect("ghost");
                break;

            case 3 or 4:
                player.ShowEffect("BloodGhost-4");
                break;

            case 5 or 6:
                player.ShowEffect("zf2-e215");
                break;

            case 7 or 8:
                player.ShowEffect("posionbomb");
                break;
        }

        var (magicType, monsterTypeId) = player.Pick<(ushort, uint)>([(8096, 4032), (8097, 4033), (8098, 4034),
            (8099, 4035), (8100, 4036), (8101, 22), (8102, 5055), (8103, 5051)]);
        player.Transform(magicType, 0, monsterTypeId, 300);
    }

    [Item(722174)]
    public static void ScreamFruit(Player player)
    {
        if (!player.Take(722174))
        {
            return;
        }

        player.Inform("You disguised into a weird ghost as soon as you put a ScreamFruit into your mouse.");

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                player.ShowEffect("ghost");
                break;

            case 3 or 4:
                player.ShowEffect("BloodGhost-4");
                break;

            case 5 or 6:
                player.ShowEffect("zf2-e215");
                break;

            case 7 or 8:
                player.ShowEffect("posionbomb");
                break;
        }

        var (magicType, monsterTypeId) = player.Pick<(ushort, uint)>([(8096, 4032), (8097, 4033), (8098, 4034),
            (8099, 4035), (8100, 4036), (8101, 22), (8102, 5055), (8103, 5051)]);
        player.Transform(magicType, 0, monsterTypeId, 300);
    }

    [Item(722175)]
    public static void PhantomAmulet(Player player)
    {
        if (!player.Take(722175))
        {
            return;
        }

        player.Inform("Srolling down to the end of the PhantomAmulet, you find yourself have already disguised into a ghost.");

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                player.ShowEffect("ghost");
                break;

            case 3 or 4:
                player.ShowEffect("BloodGhost-4");
                break;

            case 5 or 6:
                player.ShowEffect("zf2-e215");
                break;

            case 7 or 8:
                player.ShowEffect("posionbomb");
                break;
        }

        var (magicType, monsterTypeId) = player.Pick<(ushort, uint)>([(8096, 4032), (8097, 4033), (8098, 4034),
            (8099, 4035), (8100, 4036), (8101, 22), (8102, 5055), (8103, 5051)]);
        player.Transform(magicType, 0, monsterTypeId, 300);
    }

    [Item(722176)]
    public static void Pumpkin(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        switch (player.Roll(1, 8))
        {
            case 1:
                dialog.Say("Take me to CarverStar in Twin City. I have missed him for a year since I saw him last Halloween.\n\n",
                    "I will bring you to see him.");
                break;

            case 2:
                dialog.Say("I hear there is a pumpkin killer who has killed a great many pumpkins in Twin City. If I see him, I will kill him.\n\n",
                    "A pumpkin...kill a human? I highly doubt.");
                break;

            case 3:
                dialog.Say("Hey hey, you, stop! How can you hand me to the pumpkin killer? Do you know who am I? I`m a pumpkin, a great\npumpkin, a great pumpkin who will change history!\n",
                    "Change history? Human history or pumpkin history?");
                break;

            case 4:
                dialog.Say("Oh, my beautiful lover, don`t go! Don`t leave me! Don`t you see my heart is broken?\n\n",
                    "Wow, what a spoony pumpkin!");
                break;

            case 5:
                dialog.Say("I hear there are some people always bully we poor pumpkins around Twin City. I hope you are not like those pumpkin\nbully.\n\n",
                    "I`m not like them. I just like eating pumpkins in fact.");
                break;

            case 6:
                dialog.Say("Don`t talk to me like that. I suggest you respect me and speak with me politely. If you dare to offend me again,\nI will certainly give you a lesson.\n",
                    "How frightened should I be? Lol.");
                break;

            case 7:
                dialog.Say("Wearing a mask carved by a pumpkin, you can think like a pumpkin and observe the world from a pumpkin`s view. Then\nyou will see a great deal of golden...Pumpkins.\n",
                    "A pumpkin is a pumpkin.");
                break;

            case 8:
                dialog.Say("Listen to me. I will do anything for you if you don`t send me to Carver. You know, I`m the only talking pumpkin in\nthe world. We can run a circus. Do you like my idea?\n",
                    "I have no interest.");
                break;
        }
    }

    [Item(722177)]
    public static void GrimacePumpkin(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        switch (player.Roll(1, 8))
        {
            case 1 or 2:
                dialog.Say("It is too cold with a holed coat. It`s all your fault. Why did you make so many holes into my coat?\n\n",
                    "You will get used to it.");
                break;

            case 3 or 4:
                dialog.Say("How ugly! Who carved it? I can`t believe it. You know, I was a very handsome pumpkin before. But now...\n\n",
                    "Poor handsome pumpkin...");
                break;

            case 5 or 6:
                dialog.Say("Look at this face! I`m afraid people will think Carver and I are twins. I guess Carver was thinking of his own\nface when he carved my face.\n",
                    "I don`t know for sure.");
                break;

            case 7:
                dialog.Say("I must say the strangest people in the world are pumpkin carvers.\n\n",
                    "What will be stranger than a talking pumpkin?");
                break;

            case 8:
                dialog.Say("Do you know why people all wear masks made of pumpkins? Because they are too ugly!\n\n",
                    "Is that so?");
                break;
        }
    }

    [Item(722178)]
    public static void SurpriseBox(Player player)
    {
        if (player.FreeSlots < 5)
        {
            player.Inform("Please prepare six slots before you open a SurpriseBox.");
            return;
        }

        if (player.Take(722178) && player.Give(722179) is not null && player.Give(722180) is not null &&
            player.Give(722181) is not null && player.Give(722182) is not null && player.Give(722183) is not null &&
            player.Give(722184) is not null)
        {
            player.Inform("Congratulations! You have opened a SurpriseBox and get a lot of presents.");
        }
    }

    [Item(722179)]
    public static void Lollipop(Player player)
    {
        if (!player.Take(722179))
        {
            return;
        }

        player.ShowEffect("relivel");

        if (player.Transform(8096, 0, 4032, 300))
        {
            player.Inform("You transformed into a ghost after you ate up the PretenderCandy.");
        }
    }

    [Item(722180)]
    public static void GhostHat(Player player)
    {
        if (!player.Take(722180))
        {
            return;
        }

        player.ShowEffect("relivel");

        if (player.Transform(8097, 0, 4033, 300))
        {
            player.Inform("The GhostHat suddenly flew over your head, and then you disguised into a ghost.");
        }
    }

    [Item(722181)]
    public static void PhantomHat(Player player)
    {
        if (!player.Take(722181))
        {
            return;
        }

        player.ShowEffect("relivel");

        if (player.Transform(8098, 0, 4034, 300))
        {
            player.Inform("A ghost-shaped shadow from the PhantomHat cast on you, so you appeared to be a ghost.");
        }
    }

    [Item(722182)]
    public static void HalloweenRose(Player player)
    {
        if (!player.Take(722182))
        {
            return;
        }

        player.ShowEffect("relivel");

        if (player.Transform(8099, 0, 4035, 300))
        {
            player.Inform("You showered your body with rose peatals and the fragrance filled the air. All of a sudden, you became a ghost!");
        }
    }

    [Item(722183)]
    public static void SkeletonMask(Player player)
    {
        if (!player.Take(722183))
        {
            return;
        }

        player.ShowEffect("relivel");

        if (player.Transform(8100, 0, 4036, 300))
        {
            player.Inform("You put on the SkeletonMask when nobody cared you, then a ghost appeared in the street suddenly.");
        }
    }

    [Item(722184)]
    public static void SugarPumpkin(Player player)
    {
        if (!player.Take(722184))
        {
            return;
        }

        player.ShowEffect("relivel");

        if (player.Transform(8101, 0, 22, 300))
        {
            player.Inform("Although the pumpkin tasted weird, you managed to eat up it. Fortunately it really made you disguise into a ghost.");
        }
    }

    [Item(722358)]
    public static void VioletBox(Player player)
    {
        if (!player.Take(722358))
        {
            return;
        }

        if (player.Roll(500) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.World.Announce($"Congratulations! {player.Name} opened a VioletBox and received a DragonBall.",
                    TalkChannel.Gm);
            }
        }
        else if (player.Earn(5000))
        {
            player.Inform("Congratulations! You opened the VioletBox and received 5,000 silvers.");
        }
    }

    [Item(722550)]
    public static void XPAmulet(Player player)
    {
        if (player.Take(722550))
        {
            player.AddXp(10000);
        }
    }

    [Item(722559)]
    public static async Task GradeTwoItemBox(Player player, Dialog dialog)
    {
        if (!player.Map!.Info.Is(MapFlags.House))
        {
            player.Tell("You cannot place your furniture outside your house.");
            return;
        }

        if (player.Map!.OwnerId != player.Id)
        {
            player.Tell("This house is not yours. You cannot place your furniture in this house.");
            return;
        }

        if (player.Home!.Level < 2)
        {
            player.Tell("You can`t put this kind of ItemBox in a Grade One House.");
        }
        else if (player.Home!.Level == 2)
        {
            if (player.Map!.CountNpcs("ItemBox") >= 3)
            {
                player.Tell("You can only put one ItemBox of this kind in a Grade Two House.");
                return;
            }

            await dialog.PickSpot(3, 8200);

            var box = new DynaNpc
            {
                Name = "ItemBox2",
                Type = 4,
                Sort = 33,
                Look = 8200,
                OwnerType = 1,
                Task0 = 8003010,
                Data3 = 20
            };

            if (dialog.Place(box))
            {
                player.Take(722559);
            }
        }
    }

    [Item(722735)]
    public static void RedStalk(Player player)
    {
        if (!player.Has(722735) || !player.Has(722734) || !player.Has(722733, 6))
        {
            player.Inform("If you want a RedFlower, please get 6 RedPetals, a RedPistil and RedStalk ready, then click the RedStalk.");
            return;
        }

        if (player.Take(722735) && player.Take(722734) && player.Take(722733, 6) &&
            player.Give(722732) is not null)
        {
            player.Inform("You`ve got a RedFlower!");
        }
    }

    [Item(722739)]
    public static void WhiteStalk(Player player)
    {
        if (!player.Has(722738) || !player.Has(722739) || !player.Has(722737, 6))
        {
            player.Inform("If you want a WhiteFlower, please get 6 WhitePetals, a WhitePistil and WhiteStalk ready, then click the WhiteStalk.");
            return;
        }

        if (player.Take(722738) && player.Take(722739) && player.Take(722737, 6) &&
            player.Give(722736) is not null)
        {
            player.Inform("You`ve got a WhiteFlower!");
        }
    }

    [Item(722812)]
    public static void TreasureMap(Player player, Dialog dialog)
    {
        dialog.Avatar = 80;

        dialog.Say("The Queen Mother of the West`s young attendants took her Jiuhuan and Jiuxiao Stones to play with. Bored, they tossed them\naway, and ThunderApe and HillMonster took them and fled Kunlun. ThunderApe now lives in the canyon, HillMonster in the desert.",
            "I see. I`ll find them and take the two stones back.");
    }

    [Item(722822)]
    public static void MagicLamp(Player player)
    {
        if (player.FreeSlots < 2)
        {
            player.Inform("Please prepare two slots in your inventory for the prizes.");
            return;
        }

        if (!player.Take(722822))
        {
            return;
        }

        if (player.Roll(50) < 1)
        {
            if (player.Give(117306, 1,
                item => { item.Durability = 3998; item.MaxDurability = 3998; item.Gem1 = (Gem)255; }))
            {
                player.World.Announce($"Congratulations! {player.Name} opend the MagicLamp and got a socketed refined earring.",
                    TalkChannel.Gm);
            }
        }
        else if (player.Roll(50) < 1)
        {
            if (player.Give(160016, 1,
                item => { item.Durability = 1599; item.MaxDurability = 1599; item.Gem1 = (Gem)255; }))
            {
                player.World.Announce($"Congratulations! {player.Name} open the MagicLamp and get a pair of socketed refined OxhideBoots.",
                    TalkChannel.Gm);
            }
        }
        else if (player.Roll(20) < 1)
        {
            if (player.Give(ItemType.DragonBall) is not null)
            {
                player.World.Announce($"Congratulations! {player.Name} open the MagicLamp and got a Dragon Ball.",
                    TalkChannel.Gm);
            }
        }
        else if (player.Give(ItemType.Meteor, 3))
        {
            player.Inform("Congratulations! You gained three meteors.");
        }
    }

    [Item(722826)]
    public static void NutMooncake(Player player)
    {
        if (player.Take(722826))
        {
            player.AddHealth(player.Stats.MaxHealth);
        }
    }

    [Item(722827)]
    public static void BeanMooncake(Player player)
    {
        if (player.Take(722827))
        {
            player.AddMana(player.Stats.MaxMana);
        }
    }

    [Item(722828)]
    public static void YolkMooncake(Player player)
    {
        if (player.Take(722828))
        {
            player.AddHealth(player.Stats.MaxHealth);
            player.AddMana(player.Stats.MaxMana);
        }
    }

    [Item(722830)]
    public static void Turkey(Player player)
    {
        if (!player.Take(722830))
        {
            return;
        }

        player.ShowEffect("firework-like");

        if (player.Roll(4) >= 3)
        {
            player.Inform("You will gain double experience points for the coming 40 minutes.");
            player.BoostExperience(200, 2400);
            return;
        }

        if (player.Give(720017) is not null)
        {
            player.Inform("Congratulations! You gained a ChantPill.");
        }
    }

    [Item(722831)]
    public static void PrizeFruit(Player player)
    {
        if (!player.Take(722831))
        {
            return;
        }

        player.ShowEffect("firework-2love");

        if (player.Roll(2) < 1)
        {
            if (player.Give(720017) is not null)
            {
                player.Inform("Congratulations! You gained a ChantPill.");
            }
        }
        else if (player.Give(ItemType.Meteor) is not null)
        {
            player.Inform("Congratulations! You gained a meteor.");
        }
    }

    [Item(722860)]
    public static void LoveScroll(Player player)
    {
        if (player.Take(722860) && player.Give(722871) is not null && player.Teleport(1015, 717, 571))
        {
            player.Record(1015, 717, 571);
        }
    }

    [Item(722867)]
    public static void ShopList(Player player, Dialog dialog)
    {
        dialog.Avatar = 67;

        dialog.Say("Seven dyes, one piece per color. And remember to buy a stancher too. Don`t take the wrong things.",
            "Santa Claus");
    }

    [Item(722869)]
    public static void ChristmasPack(Player player)
    {
        if (!player.Take(722869))
        {
            return;
        }

        player.ShowEffect("relive1");

        switch (player.Roll(1, 8))
        {
            case 1:
                if (player.Give(1000000) is not null)
                {
                    player.Inform("Congratulations! You got a Stancher.");
                    player.Alert($"{player.Name}, Merry Christmas!");
                }
                break;

            case 2:
                switch (player.Roll(1, 8))
                {
                    case 1:
                        if (player.Give(1060030) is not null)
                        {
                            player.Inform("Congratulations! You got a BlackDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 2:
                        if (player.Give(1060040) is not null)
                        {
                            player.Inform("Congratulations! You got a PurpleDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 3:
                        if (player.Give(1060050) is not null)
                        {
                            player.Inform("Congratulations! You got a BlueDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 4:
                        if (player.Give(1060060) is not null)
                        {
                            player.Inform("Congratulations! You got a GreenDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 5:
                        if (player.Give(1060070) is not null)
                        {
                            player.Inform("Congratulations! You got a BrownDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 6:
                        if (player.Give(1060080) is not null)
                        {
                            player.Inform("Congratulations! You got a RedDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 7 or 8:
                        if (player.Give(1060090) is not null)
                        {
                            player.Inform("Congratulations! You got a WhiteDye.");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;
                }
                break;

            case >= 3 and <= 7:
                if (player.Give(721501) is not null)
                {
                    player.Inform("Congratulations! You got a ChristmasCandy.");
                    player.Alert($"{player.Name}, Merry Christmas!");
                }
                break;

            case 8:
                switch (player.Roll(1, 8))
                {
                    case 1:
                        player.ShowEffect("firework-like");
                        player.Alert($"{player.Name}, Merry Christmas!");
                        break;

                    case 2:
                        player.ShowEffect("firework-2love");
                        player.Alert($"{player.Name}, Merry Christmas!");
                        break;

                    case 3:
                        if (player.Give(132318) is not null)
                        {
                            player.Inform("Congratulations! You got an elite Dress!");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 4:
                        if (player.Give(132808) is not null)
                        {
                            player.Inform("Congratulations! You got an elite Coat!");
                            player.Alert($"{player.Name}, Merry Christmas!");
                        }
                        break;

                    case 5:
                        player.Fireworks();
                        player.Alert($"{player.Name}, Merry Christmas!");
                        break;

                    case 6:
                        player.ShowEffect("phoenix");
                        player.Alert($"{player.Name}, Merry Christmas!");
                        break;

                    case 7:
                        player.ShowEffect("fighter");
                        player.Alert($"{player.Name}, Merry Christmas!");
                        break;

                    case 8:
                        switch (player.Roll(1, 8))
                        {
                            case 1:
                                if (player.Give(562001, 1,
                                    item => { item.Durability = 1099; item.MaxDurability = 1099; item.Gem1 = (Gem)255; item.Gem2 = (Gem)255; }))
                                {
                                    player.World.Announce($"Merry Christmas! Congratulate {player.Name} on getting a PickAxe with two sockets from ChristmasPack!",
                                        TalkChannel.Gm);
                                }
                                break;

                            case >= 2:
                                if (player.Give(ItemType.Meteor) is not null)
                                {
                                    player.World.Announce($"Merry Christmas! Congratulate {player.Name} on getting a Meteor from ChristmasPack!",
                                        TalkChannel.Gm);
                                }
                                break;
                        }
                        break;
                }
                break;
        }
    }

    [Item(722903)]
    public static void RedPacket(Player player)
    {
        if (player.Take(722903) && player.Earn(68888))
        {
            player.Inform("Congratulations! You got 68888 silvers.");
        }
    }

    [Item(722904)]
    public static void GoldBullion(Player player)
    {
        if (player.Take(722904) && player.Earn(88888))
        {
            player.Inform("Congratulations! You got 88888 silvers.");
        }
    }

    [Item(722916)]
    public static void NewbiePackage(Player player)
    {
        if (player.FreeSlots < 9)
        {
            player.Inform("Please leave at least 9 free slots in your inventory before you open the gift.");
            return;
        }

        if (player.Take(722916) && player.Earn(5888) && player.Give(422000, 3) && player.Give(1060020, 5) &&
            player.Fireworks("Conquer!"))
        {
            player.Inform($"Hero {player.Name} has set out on the long road of Conquer. Enjoy the loyalty and romance of the martial world!");
        }
    }

    [Item(723017)]
    public static void ExpPotion(Player player)
    {
        if (player.Take(723017))
        {
            player.Inform("You will gain double experience points for the coming 60 minutes.");
            player.BoostExperience(200, 3600);
        }
    }

    [Item(723052)]
    public static void ValentineTurkey(Player player)
    {
        if (player.Take(723052))
        {
            player.AddHealth(player.Stats.MaxHealth);
        }
    }

    [Item(723071)]
    public static async Task MagicGate(Player player, Dialog dialog)
    {
        await dialog.PickSpot(2, 1417);

        var stone = new DynaNpc
        {
            Name = "TeleportStone",
            Type = 2,
            Sort = 1,
            Look = 1417,
            LinkId = 7521755,
            Task0 = 7521755
        };

        if (dialog.Place(stone))
        {
            player.Take(723071);
        }
    }

    [Item(723249)]
    public static void BunnyMiracleFlower(Player player)
    {
        if (!player.Take(723249))
        {
            return;
        }

        if (!player.IsFemale)
        {
            player.ShowEffect("relive1");
            player.Transform(3310, 0, 3310, 30);
        }
        else
        {
            player.ShowEffect("relive1");
            player.Transform(3312, 0, 3312, 30);
        }
    }

    [Item(723250)]
    public static void EggMiracleFlower(Player player)
    {
        if (player.Take(723250))
        {
            player.ShowEffect("relive1");
            player.Transform(3311, 0, 3311, 30);
        }
    }

    [Item(723251)]
    public static void GoldIngots(Player player)
    {
        if (player.Take(723251) && player.Earn(666))
        {
            player.Inform("Congratulations! You`ve got 666 silver.");
        }
    }

    [Item(723252)]
    public static void GoldBars(Player player)
    {
        if (player.Take(723252) && player.Earn(888))
        {
            player.Inform("Congratulations! You`ve got 888 silver.");
        }
    }

    [Item(723253)]
    public static void GreatGoldBars(Player player)
    {
        if (player.Take(723253) && player.Earn(999))
        {
            player.Inform("Congratulations! You`ve got 999 silver.");
        }
    }

    [Item(1090000)]
    public static void Silver(Player player, Dialog dialog)
    {
        dialog.Avatar = 6;

        if (player.Level < 15)
        {
            dialog.Say("Sorry, you must be level 15 to be promoted. Go seek wisdom in battle.", "I see. Thanks.");
        }
        else
        {
            dialog.Say("Taoist spells make them strong in a group or alone.", "Get promoted.", "Let me think it over.");
        }
    }
}
