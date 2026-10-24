namespace Conquer.Game.Messages;

public enum PackageAction
{
    QueryList = 0,
    CheckIn = 1,
    CheckOut = 2,
    QueryList2 = 3
}

public record MsgPackage : IMessage
{
    public uint Id { get; set; }
    public PackageAction Action { get; set; }
    public byte PackageType { get; set; }
    public uint ItemId { get; set; }
    public List<Item> Items { get; set; } = [];

    public ushort Size => (ushort)(16 + Items.Count * 20);
    public MessageType Type => MessageType.MsgPackage;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Action = (PackageAction)buffer[8];
        PackageType = buffer[9];
        ItemId = ReadUInt32LittleEndian(buffer[12..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        buffer[8] = (byte)Action;
        buffer[9] = PackageType;
        WriteUInt32LittleEndian(buffer[12..], Items.Count > 0 ? (uint)Items.Count : ItemId);

        foreach (var (i, item) in Items.Index())
        {
            var entry = buffer[(16 + i * 20)..];
            WriteUInt32LittleEndian(entry, item.Id);
            WriteUInt32LittleEndian(entry[4..], LegendaryLook.ShownTypeId(item));
            entry[8] = item.Ident;
            entry[9] = (byte)item.Gem1;
            entry[10] = (byte)item.Gem2;
            entry[11] = item.Magic1;
            entry[12] = item.Magic2;
            entry[13] = item.Plus;
            WriteUInt16LittleEndian(entry[14..], item.Bless);
            WriteUInt16LittleEndian(entry[16..], item.Enchant);
            WriteUInt16LittleEndian(entry[18..], (ushort)item.Restrain);
        }
    }

    public async Task HandleAsync(Player player, Map map, GameStore store)
    {
        switch (map.Find<Entity>(Id))
        {
            case Npc { IsStorage: true } warehouse when player.CanSee(warehouse) && player.CanOpenStorage:
                var stored = Action switch
                {
                    PackageAction.QueryList => true,
                    PackageAction.CheckIn => player.FindItem(ItemId) is { } item && player.Store(item, Id),
                    PackageAction.CheckOut => player.StoredIn(Id).FirstOrDefault(item => item.Id == ItemId) is { } item && player.Withdraw(item),
                    _ => false
                };

                if (stored)
                {
                    List(player.StoredIn(Id));
                }
                break;

            case DynaNpc { IsTrunk: true } box when player.CanSee(box):
                var boxed = Action switch
                {
                    PackageAction.QueryList => true,
                    PackageAction.CheckIn => player.FindItem(ItemId) is { } item && player.Store(item, box),
                    PackageAction.CheckOut => await MayEmpty(box) && box.Contents.FirstOrDefault(item => item.Id == ItemId) is { } item &&
                                              player.Withdraw(item, box),
                    _ => false
                };

                if (boxed)
                {
                    List(box.Contents);
                }
                break;
        }

        void List(IEnumerable<Item> items) => player.Send(new MsgPackage { Id = Id, PackageType = PackageType, Items = [..items] });

        async Task<bool> MayEmpty(DynaNpc box) => box.IsOwnedBy(player) || player.IsMarried && await store.HomeOfAsync(player.Spouse) == box.MapId;
    }
}
