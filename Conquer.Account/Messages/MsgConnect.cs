namespace Conquer.Account.Messages;

public record MsgConnect : IMessage
{
    public uint Id { get; set; }
    public uint Data { get; set; }
    public string? Info { get; set; }
    public ushort Size => 28;
    public MessageType Type => MessageType.MsgConnect;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt32LittleEndian(buffer[8..]);
        Info = buffer[12..28].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt32LittleEndian(buffer[8..], Data);
        buffer[12..28].WriteString(Info);
    }

    public Task HandleAsync() => Task.CompletedTask;
}