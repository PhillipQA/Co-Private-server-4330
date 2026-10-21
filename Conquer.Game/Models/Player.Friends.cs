namespace Conquer.Game.Models;

public partial class Player
{
    private const int MaxFriends = 50;

    private static readonly TimeSpan OfflineMessageInterval = TimeSpan.FromMinutes(1);

    private readonly Dictionary<uint, DateTimeOffset> _wordsLeft = [];

    internal IEnumerable<Player> OnlineFriends => Friends.Select(friend => World.Players.GetValueOrDefault(friend.FriendPlayerId)).OfType<Player>();
    private IEnumerable<Player> Haters => World.Players.Values.Where(other => other.Enemies.Any(enemy => enemy.EnemyPlayerId == Id));

    public bool IsFriend(uint playerId) => Friends.Any(friend => friend.FriendPlayerId == playerId);

    public bool AddFriend(Player other)
    {
        if (IsFriend(other.Id) || Friends.Count >= MaxFriends || other.Friends.Count >= MaxFriends)
        {
            return false;
        }

        Accept(this, other);
        Accept(other, this);
        return true;

        static void Accept(Player one, Player two)
        {
            one.Friends.Add(new() { PlayerId = one.Id, FriendPlayerId = two.Id, Name = two.Name });
            one.Send(new MsgFriend(two.Id, FriendAction.FriendAccept, true, two.Name));
        }
    }

    public void RemoveFriend(uint playerId)
    {
        Friends.RemoveAll(friend => friend.FriendPlayerId == playerId);
        Send(new MsgFriend(playerId, FriendAction.FriendRemove, false, ""));
    }

    public void AddEnemy(Player other)
    {
        if (Enemies.Any(enemy => enemy.EnemyPlayerId == other.Id))
        {
            return;
        }

        Enemies.Add(new() { PlayerId = Id, EnemyPlayerId = other.Id, Name = other.Name });
        Send(new MsgFriend(other.Id, FriendAction.EnemyAdd, World.Players.ContainsKey(other.Id), other.Name));
    }

    public void RemoveEnemy(uint playerId)
    {
        Enemies.RemoveAll(enemy => enemy.EnemyPlayerId == playerId);
        Send(new MsgFriend(playerId, FriendAction.EnemyRemove, false, ""));
    }

    public Friend? TryLeaveMessage(string name, DateTimeOffset now)
    {
        if (Friends.FirstOrDefault(friend => friend.Name == name) is not { } friend || World.Players.ContainsKey(friend.FriendPlayerId))
        {
            return null;
        }

        if (_wordsLeft.TryGetValue(friend.FriendPlayerId, out var last) && now - last < OfflineMessageInterval)
        {
            Tell($"Failed to leave message. Please leave message to a friend every {OfflineMessageInterval.TotalMinutes} minutes.");
            return null;
        }

        _wordsLeft[friend.FriendPlayerId] = now;
        Tell("Message sent successfully. The target will receive it as soon as he/she logs into the game.");
        return friend;
    }

    public void OnDeleting()
    {
        foreach (var friend in OnlineFriends)
        {
            friend.RemoveFriend(Id);
        }

        foreach (var hater in Haters)
        {
            hater.RemoveEnemy(Id);
        }

        if (IsMarried && World.Find(Spouse) is { } spouse)
        {
            spouse.SetSpouse(Unmarried);
        }

        World.Bounties.RemoveAll(Name);
    }

    public void NotifyFriends(bool online)
    {
        OnlineFriends.Send(new MsgFriend(Id, online ? FriendAction.FriendOnline : FriendAction.FriendOffline, online, Name));
        Haters.Send(new MsgFriend(Id, online ? FriendAction.EnemyOnline : FriendAction.EnemyOffline, online, Name));
    }
}
