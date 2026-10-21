namespace Conquer.Game.Scripts.Npcs;

public static class FurnitureStore
{
    private static readonly Dictionary<uint, (string Name, uint Price, uint ItemTypeId)> Pieces = new()
    {
        [3113] = ("Lamp", 8500, 721177),
        [3114] = ("LowShelf", 8000, 721178),
        [3115] = ("Cabinet", 8000, 721179),
        [3116] = ("BombeChest", 8000, 721181),
        [3117] = ("RosewoodCabinet", 8500, 721182),
        [3118] = ("HighCabinet", 8000, 721183),
        [3119] = ("FoldingScreen", 12000, 721184),
        [3120] = ("Dresser", 12000, 721185),
        [3121] = ("BasinRack", 6000, 721186),
        [3122] = ("Chair", 9000, 721187),
        [3123] = ("EndTable", 9000, 721188),
        [3125] = ("HighShelf", 8000, 721180),
        [3126] = ("Lamp", 9000, 721222),
        [3127] = ("Zither", 12000, 721223),
        [3128] = ("Harp", 12000, 721224),
        [3129] = ("GreenPearl", 15000, 721225),
        [3130] = ("Fountain", 20000, 721226),
        [3131] = ("Swing", 15000, 721227),
        [3132] = ("Windmill", 18000, 721228),
        [3133] = ("Hobbyhorse", 12000, 721229),
        [3134] = ("Rockery", 15000, 721230),
        [3135] = ("StoneStatue", 15000, 721231),
        [3136] = ("StoneLion", 15000, 721232),
        [3137] = ("BlackDragon", 20000, 721233),
        [3138] = ("BronzeTurtle", 18000, 721234),
        [3139] = ("Phoenix", 20000, 721235)
    };

    [Npc(3113, 3114, 3115, 3116, 3117, 3118, 3119, 3120, 3121, 3122, 3123, 3125, 3126, 3127, 3128, 3129, 3130, 3131, 3132, 3133, 3134, 3135, 3136,
        3137, 3138, 3139)]
    public static async Task Furniture(Player player, Entity npc, Dialog dialog)
    {
        var (name, price, itemTypeId) = Pieces[npc.Id];

        dialog.Avatar = 188;

        if (player.PkPoints > 1000)
        {
            return;
        }

        await dialog.Confirm($"Do you want this {name}? It costs only {price} silvers.", "Yes.", "No.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.Money < price)
        {
            dialog.Say($"You do not have {price} silvers with you.", "I am sorry.");
        }
        else if (player.FreeSlots > 0)
        {
            if (player.Spend(price) && player.Give(itemTypeId) is not null)
            {
                player.Inform($"You got a {name}");
            }
        }
        else
        {
            dialog.Say("Please prepare one slot in your inventory.", "Alright.");
        }
    }

    [Npc(3124)]
    public static async Task ItemBox(Player player, Dialog dialog)
    {
        dialog.Avatar = 188;

        if (player.PkPoints > 1000)
        {
            return;
        }

        await dialog.Confirm("Do you want this ItemBox? It is expensive, and costs 100000 silvers.", "Yes.", "No.");

        dialog.Require(player.PkPoints <= 1000);

        if (player.Money < 100000)
        {
            dialog.Say("You do not have 100000 silvers with you.", "I am sorry.");
        }
        else if (player.FreeSlots > 0)
        {
            if (player.Spend(100000) && player.Give(721189) is not null)
            {
                player.Inform("You got an ItemBox");
            }
        }
        else
        {
            dialog.Say("Please prepare one slot in your inventory.", "Alright.");
        }
    }
}
