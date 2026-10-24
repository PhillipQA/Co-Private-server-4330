namespace Conquer.Game.Messages;

public static class ItemBlock
{
    public static Item Read(ReadOnlySpan<byte> block) => new()
    {
        ItemTypeId = ReadUInt32LittleEndian(block),
        Durability = ReadUInt16LittleEndian(block[4..]),
        MaxDurability = ReadUInt16LittleEndian(block[6..]),
        Ident = block[9],
        Position = (ItemPosition)block[10],
        Gem1 = (Gem)block[11],
        Gem2 = (Gem)block[12],
        Magic1 = block[13],
        Magic2 = block[14],
        Plus = block[15],
        Bless = block[16],
        Enchant = block[17],
        Restrain = ReadUInt32LittleEndian(block[20..])
    };

    public static void Write(Span<byte> block, Item item, byte action)
    {
        WriteUInt32LittleEndian(block, LegendaryLook.ShownTypeId(item));
        WriteUInt16LittleEndian(block[4..], item.Durability);
        WriteUInt16LittleEndian(block[6..], item.MaxDurability);
        block[8] = action;
        block[9] = item.Ident;
        block[10] = (byte)item.Position;
        block[11] = (byte)item.Gem1;
        block[12] = (byte)item.Gem2;
        block[13] = item.Magic1;
        block[14] = item.Magic2;
        block[15] = item.Plus;
        block[16] = item.Bless;
        block[17] = item.Enchant;
        WriteUInt32LittleEndian(block[20..], item.Restrain);
    }
}
