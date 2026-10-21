namespace Conquer.Game.Messages;

public enum DialogType
{
    Text = 1,
    Link = 2,
    Edit = 3,
    Picture = 4,
    ListLine = 5,
    Create = 100,
    Answer = 101,
    TaskId = 102,
    UpdateNotice = 112
}

public record MsgTaskDialog : IMessage
{
    private const uint ExpelTask = 31100;

    public uint Id { get; set; }
    public ushort Data { get; set; }
    public byte Option { get; set; }
    public DialogType DialogType { get; set; }
    public List<string> Strings { get; set; } = [];
    public ushort Size => (ushort)(12 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgTaskDialog;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Data = ReadUInt16LittleEndian(buffer[8..]);
        Option = buffer[10];
        DialogType = (DialogType)buffer[11];
        Strings = buffer[12..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        WriteUInt16LittleEndian(buffer[8..], Data);
        buffer[10] = Option;
        buffer[11] = (byte)DialogType;
        buffer[12..].WriteStrings(Strings);
    }

    public async Task HandleAsync(Player player, GuildService guilds)
    {
        if (!player.IsAlive)
        {
            player.Tell("You are already dead.", TalkChannel.System);
            return;
        }

        var text = Strings.FirstOrDefault() ?? "";

        if (DialogType == DialogType.Answer)
        {
            player.Dialog?.Answer(Option, text);
        }
        else if (DialogType == DialogType.TaskId && Id == ExpelTask)
        {
            await guilds.KickAsync(player, text);
        }
    }
}