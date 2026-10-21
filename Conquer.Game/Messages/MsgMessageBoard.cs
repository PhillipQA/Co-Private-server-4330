namespace Conquer.Game.Messages;

public enum MessageBoardAction : byte
{
    Delete = 1,
    GetList = 2,
    List = 3,
    GetWords = 4
}

public record MsgMessageBoard : IMessage
{
    public ushort Index { get; set; }
    public TalkChannel Channel { get; set; }
    public MessageBoardAction Action { get; set; }
    public List<string> Strings { get; set; } = [];
    public ushort Size => (ushort)(9 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgMessageBoard;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Index = ReadUInt16LittleEndian(buffer[4..]);
        Channel = (TalkChannel)ReadUInt16LittleEndian(buffer[6..]);
        Action = (MessageBoardAction)buffer[8];
        Strings = buffer[9..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt16LittleEndian(buffer[4..], Index);
        WriteUInt16LittleEndian(buffer[6..], (ushort)Channel);
        buffer[8] = (byte)Action;
        buffer[9..].WriteStrings(Strings);
    }

    public void Handle(Player player)
    {
        if (MessageBoard.For(Channel, player) is not { } boards)
        {
            return;
        }

        switch (Action)
        {
            case MessageBoardAction.GetList:
                player.Send(this with { Action = MessageBoardAction.List, Strings = boards.Page(Channel, Index) });
                break;

            case MessageBoardAction.GetWords when Strings is [var author]:
                if (boards.Read(Channel, author) is { } words)
                {
                    player.Send(new MsgTalk(Channel, author, player.Name, "", words));
                }
                break;

            case MessageBoardAction.Delete when Strings is [var author] && author == player.Name:
                boards.Erase(Channel, author);
                break;
        }
    }
}