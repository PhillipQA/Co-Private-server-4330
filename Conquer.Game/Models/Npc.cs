namespace Conquer.Game.Models;

public class Npc : Entity
{
    public const byte ShopType = 1;

    private const byte StorageType = 3;

    public override IMessage Spawn => new MsgNpcInfo(this);
    public bool IsShop => Type == ShopType;
    public bool IsStorage => Type == StorageType;
    public string Name { get; set; } = null!;
    public byte Type { get; set; }
    public uint Look { get; set; }
    public byte Base { get; set; }
    public byte Sort { get; set; }
    public DiceTable? Dice { get; set; }
}