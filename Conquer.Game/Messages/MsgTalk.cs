using System.Drawing;

namespace Conquer.Game.Messages;

public enum TalkChannel : ushort
{
    Normal = 2000,
    Private = 2001,
    Action = 2002,
    Team = 2003,
    Guild = 2004,
    System = 2005,
    Family = 2006,
    Talk = 2007,
    Yelp = 2008,
    Friend = 2009,
    Global = 2010,
    Gm = 2011,
    Whisper = 2012,
    Ghost = 2013,
    Serve = 2014,
    Register = 2100,
    Entrance = 2101,
    Shop = 2102,
    PetTalk = 2103,
    BoothSlogan = 2104,
    WebPage = 2105,
    NewMessage = 2106,
    Task = 2107,
    GuildWarFirst = 2108,
    GuildWarNext = 2109,
    OfflineMessage = 2110,
    GuildBulletin = 2111,
    MessageBox = 2112,
    Reject = 2113,
    GuildTenet = 2114,
    MsgTrade = 2201,
    MsgFriend = 2202,
    MsgTeam = 2203,
    MsgGuild = 2204,
    MsgOther = 2205,
    MsgSystem = 2206,
    Broadcast = 2500
}

[Flags]
public enum TalkStyles : ushort
{
    Normal = 0b_0000,
    Scroll = 0b_0001,
    Flash = 0b_0010,
    Blast = 0b_1000
}

public record MsgTalk : IMessage
{
    public MsgTalk()
    {
    }

    public MsgTalk(TalkChannel channel, params List<string> strings)
    {
        Channel = channel;
        Strings = strings;
    }

    public static MsgTalk System(string text, TalkChannel channel = TalkChannel.System) => new(channel, "SYSTEM", "ALLUSERS", "", text);

    public Color Color { get; set; } = Color.White;
    public TalkChannel Channel { get; set; }
    public TalkStyles Styles { get; set; }
    public uint Timestamp { get; set; } = (uint)Environment.TickCount;
    public List<string> Strings { get; set; } = null!;

    public ushort Size => (ushort)(16 + StringPack.Size(Strings));
    public MessageType Type => MessageType.MsgTalk;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Color = Color.FromArgb(ReadInt32LittleEndian(buffer[4..]));
        Channel = (TalkChannel)ReadUInt16LittleEndian(buffer[8..]);
        Styles = (TalkStyles)ReadUInt16LittleEndian(buffer[10..]);
        Timestamp = ReadUInt32LittleEndian(buffer[12..]);
        Strings = buffer[16..].ReadStrings();
    }

    public void Write(Span<byte> buffer)
    {
        WriteInt32LittleEndian(buffer[4..], Color.FromArgb(0, Color).ToArgb());
        WriteUInt16LittleEndian(buffer[8..], (ushort)Channel);
        WriteUInt16LittleEndian(buffer[10..], (ushort)Styles);
        WriteUInt32LittleEndian(buffer[12..], Timestamp);
        buffer[16..].WriteStrings(Strings);
    }

    public async Task HandleAsync(Player player, Map map, World world, CommandService commands, GuildService guilds, GameStore store)
    {
        if (Strings is not [_, var hearer, var emotion, var words, ..])
        {
            return;
        }

        if (words.StartsWith('/'))
        {
            // Some clients append junk bytes after the typed text; commands are plain ASCII, so stop at the first
            // character that isn't.
            var command = new string(words.TakeWhile(c => c is >= ' ' and <= '~').ToArray()).TrimEnd();

            if (player.IsGm || CommandService.IsPublic(command))
            {
                await commands.HandleAsync(player, command);
            }

            return;
        }

        var spoken = this with { Strings = [player.Name, hearer, emotion, words] };

        if (Channel == TalkChannel.Private)
        {
            if (world.Find(hearer) is { } listener)
            {
                listener.Send(spoken);
            }
            else
            {
                player.Tell($"{hearer} is not online.");
            }

            return;
        }

        if (MessageBoard.Holds(Channel))
        {
            MessageBoard.For(Channel, player)?.Write(Channel, player.Name, words, world.UtcNow);
            return;
        }

        if (Channel == TalkChannel.OfflineMessage)
        {
            if (words.Length <= OfflineMessage.MaxLength && player.TryLeaveMessage(hearer, world.UtcNow) is { } friend)
            {
                await store.SaveOfflineMessageAsync(new OfflineMessage
                {
                    PlayerId = friend.FriendPlayerId,
                    Sender = player.Name,
                    SentAt = world.Now,
                    Words = words
                });
            }

            return;
        }

        if (Channel == TalkChannel.BoothSlogan)
        {
            player.Booth?.Advertise(words);
            return;
        }

        if (Channel == TalkChannel.GuildBulletin)
        {
            guilds.SetAnnouncement(player, words);
            return;
        }

        Audience(player, hearer, map, world).Send(spoken);
    }

    private IEnumerable<Player> Audience(Player player, string hearer, Map map, World world) => Channel switch
    {
        TalkChannel.Serve when player.IsGm => world.Find(hearer) is { } asker ? [asker] : [],
        TalkChannel.Serve => world.Players.Values.Where(other => other.IsGm),
        TalkChannel.Normal => map.InRange(player).OfType<Player>(),
        TalkChannel.Ghost => map.InRange(player).OfType<Player>().Where(other => !other.IsAlive || other.IsWaterTaoist),
        TalkChannel.Team => player.Team?.Members.Where(member => member != player) ?? [],
        TalkChannel.Guild when player.Guild is { } guild => world.FamilyMembers(guild).Where(member => member != player),
        TalkChannel.Friend => player.OnlineFriends,
        _ => []
    };
}