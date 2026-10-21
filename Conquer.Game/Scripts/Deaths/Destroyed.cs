namespace Conquer.Game.Scripts.Deaths;

public static class Destroyed
{
    [Destruction(150000)]
    public static void Pole(Player player, DynaNpc npc)
    {
        var beastGuard = player.World.FindNpc(819)!;

        npc.SetHealth(20000000);
        npc.Map!.War!.ResetOwner();
        player.Map!.War!.Stop();

        var leftGate = new DynaNpc
        {
            Name = "Close left gate",
            Type = 26,
            Sort = 21,
            Look = 241,
            OwnerType = 2,
            OwnerId = player.Map!.OwnerId,
            MapId = 1038,
            X = 165,
            Y = 210,
            MaxHealth = 10000000,
            Base = 24,
            LinkId = 5030100,
            Task0 = 5030101
        };

        var rightGate = new DynaNpc
        {
            Name = "Close right gat",
            Type = 26,
            Sort = 21,
            Look = 277,
            OwnerType = 2,
            OwnerId = player.Map!.OwnerId,
            MapId = 1038,
            X = 223,
            Y = 179,
            MaxHealth = 10000000,
            Base = 27,
            LinkId = 5030102,
            Task0 = 5030103
        };

        if (player.World.DeleteNpcs(1038, 26) && player.World.CreateNpc(leftGate) && player.World.CreateNpc(rightGate) &&
            beastGuard.Data1 != beastGuard.Map!.OwnerId)
        {
            beastGuard.Data2 = 1;
        }
    }

    [Destruction(511500)]
    public static void AncientDevil(Player player, DynaNpc npc)
    {
        if (player.World.DeleteNpc(npc) && player.World.DropItem(ItemType.Meteor, 1052, 216, 208) &&
            player.World.DropItem(ItemType.Meteor, 1052, 218, 205) &&
            player.World.DropItem(ItemType.Meteor, 1052, 220, 210) &&
            player.World.DropItem(ItemType.Meteor, 1052, 222, 218) &&
            player.World.DropItem(ItemType.Meteor, 1052, 220, 208) && player.Roll(8) < 1)
        {
            player.World.DropItem(ItemType.DragonBall, 1052, 218, 208);
        }
    }

    [Destruction(5030100)]
    public static void LeftGate(Player player, DynaNpc npc)
    {
        npc.Data1 = 1;
        npc.SetLook(251);
        npc.SetHealth(100);
    }

    [Destruction(5030102)]
    public static void RightGate(Player player, DynaNpc npc)
    {
        npc.Data1 = 1;
        npc.SetLook(287);
        npc.SetHealth(100);
    }
}
