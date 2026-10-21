using System.Text;

namespace Conquer.Game.Messages;

public record MsgTick : IMessage
{
    private const uint ShortNameCheck = 0x9D4B5703;
    private const uint NameCheckKey = 0x9823;

    public MsgTick()
    {
    }

    public MsgTick(Player player, Random random)
    {
        Id = player.Id;
        Junk = [Next(), Next(), Next(), Next()];

        uint Next() => (uint)random.Next(0x1FFFFFFF, 0x6FFFFFFF);
    }

    public uint Id { get; set; }
    public uint Timestamp { get; set; }
    public uint[] Junk { get; set; } = new uint[4];
    public uint Check { get; set; }

    public ushort Size => 32;
    public MessageType Type => MessageType.MsgTick;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Timestamp = ReadUInt32LittleEndian(buffer[8..]);
        Check = ReadUInt32LittleEndian(buffer[28..]);
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Timestamp);

        foreach (var (i, junk) in Junk.Index())
        {
            WriteUInt32LittleEndian(buffer[(12 + i * 4)..], junk);
        }

        WriteUInt32LittleEndian(buffer[28..], Check);
    }

    public void Handle(GameClient client, Player player, World world, ILogger<MsgTick> logger)
    {
        if (Id != player.Id || Check != CheckFor(player.Name))
        {
            logger.LogWarning("{Player} failed the tick check.", player.Name);
            client.Close();
            return;
        }

        client.Answer(world.UtcNow);
    }

    private static uint CheckFor(string name) =>
        name.Length < 4 ? ShortNameCheck : ReadUInt16LittleEndian(Encoding.Latin1.GetBytes(name)) ^ NameCheckKey;
}
