namespace Conquer.Game.Messages;

public record MsgPlayer : IMessage
{
    private const int StatueLifeScale = 100;

    public MsgPlayer()
    {
    }

    public MsgPlayer(Player player)
    {
        Id = player.Id;
        Look = player.Look;
        Status = (uint)player.Status;
        OwnerId = player.GuildId is { } guildId ? (uint)player.ShownGuildRank << 24 | guildId : 0;
        Head = player.ItemAt(ItemPosition.Head)?.ItemTypeId ?? 0;
        Armor = player.ItemAt(ItemPosition.Armor)?.ItemTypeId ?? 0;
        Right = player.ItemAt(ItemPosition.Right)?.ItemTypeId ?? 0;
        Left = player.ItemAt(ItemPosition.Left)?.ItemTypeId ?? 0;
        Health = (ushort)player.Health;
        Level = player.Level;
        X = player.X;
        Y = player.Y;
        Hair = player.Hair;
        Direction = player.Direction;
        Pose = (byte)player.Pose;
        Rebirths = player.Rebirths;
        Strings = [player.Name, player.Spouse];
    }

    public MsgPlayer(Monster monster)
    {
        Id = monster.Id;
        Look = monster.MonsterType.Look;
        Status = (uint)monster.Status;
        Health = (ushort)monster.Health;
        Level = monster.Level;
        X = monster.X;
        Y = monster.Y;
        Direction = monster.Direction;
        Strings = [monster.MonsterType.Name];
    }

    public MsgPlayer(DynaNpc npc)
    {
        var statue = npc.Statue!;
        Id = npc.Id;
        Look = statue.Look;
        OwnerId = (uint)statue.Rank << 24 | npc.OwnerId;
        Head = statue.Head;
        Armor = statue.Armor;
        Right = statue.Right;
        Left = statue.Left;
        Health = (ushort)(npc.Health / StatueLifeScale);
        Level = statue.Frame;
        X = npc.X;
        Y = npc.Y;
        Hair = statue.Hair;
        Direction = (Direction)(npc.Look % 10);
        Pose = statue.Pose;
        Strings = [npc.Name];
    }

    public uint Id { get; set; }
    public uint Look { get; set; }
    public uint Status { get; set; }
    public uint OwnerId { get; set; }
    public uint Head { get; set; }
    public uint Armor { get; set; }
    public uint Right { get; set; }
    public uint Left { get; set; }
    public ushort Health { get; set; }
    public ushort Level { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort Hair { get; set; }
    public Direction Direction { get; set; }
    public byte Pose { get; set; }
    public byte Rebirths { get; set; }
    public List<string> Strings { get; set; } = [];

    public ushort Size => (ushort)(53 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgPlayer;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Look = ReadUInt32LittleEndian(buffer[8..]);
        Status = ReadUInt32LittleEndian(buffer[12..]);
        OwnerId = ReadUInt32LittleEndian(buffer[16..]);
        Head = ReadUInt32LittleEndian(buffer[20..]);
        Armor = ReadUInt32LittleEndian(buffer[24..]);
        Right = ReadUInt32LittleEndian(buffer[28..]);
        Left = ReadUInt32LittleEndian(buffer[32..]);
        Health = ReadUInt16LittleEndian(buffer[40..]);
        Level = ReadUInt16LittleEndian(buffer[42..]);
        X = ReadUInt16LittleEndian(buffer[44..]);
        Y = ReadUInt16LittleEndian(buffer[46..]);
        Hair = ReadUInt16LittleEndian(buffer[48..]);
        Direction = (Direction)buffer[50];
        Pose = buffer[51];
        Rebirths = buffer[52];
        Strings = buffer[53..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Look);
        WriteUInt32LittleEndian(buffer[12..], Status);
        WriteUInt32LittleEndian(buffer[16..], OwnerId);
        WriteUInt32LittleEndian(buffer[20..], Head);
        WriteUInt32LittleEndian(buffer[24..], Armor);
        WriteUInt32LittleEndian(buffer[28..], Right);
        WriteUInt32LittleEndian(buffer[32..], Left);
        WriteUInt16LittleEndian(buffer[40..], Health);
        WriteUInt16LittleEndian(buffer[42..], Level);
        WriteUInt16LittleEndian(buffer[44..], X);
        WriteUInt16LittleEndian(buffer[46..], Y);
        WriteUInt16LittleEndian(buffer[48..], Hair);
        buffer[50] = (byte)Direction;
        buffer[51] = Pose;
        buffer[52] = Rebirths;
        buffer[53..].WriteStrings(Strings);
    }
}