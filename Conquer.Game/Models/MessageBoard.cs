namespace Conquer.Game.Models;

public sealed class MessageBoard
{
    private const int PageSize = 10;
    private const int TitleLength = 44;
    private const int Capacity = 200;

    private readonly Dictionary<TalkChannel, List<Post>> _boards = [];

    public static bool Holds(TalkChannel channel) => channel is >= TalkChannel.MsgTrade and <= TalkChannel.MsgSystem;

    public static MessageBoard? For(TalkChannel channel, Player player) =>
        channel == TalkChannel.MsgGuild ? player.Guild?.Board : player.World.Board;

    public void Write(TalkChannel channel, string author, string words, DateTimeOffset now)
    {
        if (!Holds(channel) || string.IsNullOrWhiteSpace(words))
        {
            return;
        }

        var board = Board(channel);

        Erase(channel, author);
        board.Add(new(author, words, now.ToString(OfflineMessage.Stamp)));

        if (board.Count > Capacity)
        {
            board.RemoveRange(0, board.Count - Capacity);
        }
    }

    public bool Erase(TalkChannel channel, string author) =>
        Holds(channel) && Board(channel).RemoveAll(post => post.Author == author) > 0;

    public string? Read(TalkChannel channel, string author) =>
        Holds(channel) ? Board(channel).FirstOrDefault(post => post.Author == author)?.Words : null;

    public List<string> Page(TalkChannel channel, int index) =>
        Holds(channel)
            ? [.. Board(channel).AsEnumerable().Reverse().Skip(index).Take(PageSize)
                .SelectMany(post => (string[])[post.Author, post.Words[..Math.Min(post.Words.Length, TitleLength)], post.Date])]
            : [];

    private List<Post> Board(TalkChannel channel) => _boards.TryGetValue(channel, out var board) ? board : _boards[channel] = [];

    private sealed record Post(string Author, string Words, string Date);
}
