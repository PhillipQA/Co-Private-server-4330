
namespace Conquer.Game.Messages;

public enum FriendAction : byte
{
    FriendApply = 10,
    FriendAccept = 11,
    FriendOnline = 12,
    FriendOffline = 13,
    FriendRemove = 14,
    GetInfo = 15,
    EnemyOnline = 16,
    EnemyOffline = 17,
    EnemyRemove = 18,
    EnemyAdd = 19
}

public record MsgFriend : IMessage
{
    public MsgFriend()
    {
    }

    public MsgFriend(uint id, FriendAction action, bool isOnline, string name)
    {
        Id = id;
        Action = action;
        IsOnline = isOnline;
        Name = name;
    }

    public uint Id { get; set; }
    public FriendAction Action { get; set; }
    public bool IsOnline { get; set; }
    public string Name { get; set; } = null!;
    public ushort Size => 28;
    public MessageType Type => MessageType.MsgFriend;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Action = (FriendAction)buffer[8];
        IsOnline = buffer[9] != 0;
        Name = buffer[12..28].ReadString();
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        buffer[8] = (byte)Action;
        buffer[9] = (byte)(IsOnline ? 1 : 0);
        buffer[12..28].WriteString(Name);
    }

    public async Task HandleAsync(Player player, World world, GameStore store)
    {
        var other = world.Players.GetValueOrDefault(Id);

        switch (Action)
        {
            case FriendAction.FriendApply when other is not null && other != player && !player.IsFriend(Id):
                if (other.FriendRequest == player.Id)
                {
                    player.FriendRequest = 0;
                    other.FriendRequest = 0;

                    if (player.AddFriend(other))
                    {
                        Proclaim($"{player.Name} and {other.Name} are friends from now on!");
                    }
                    break;
                }

                player.FriendRequest = Id;
                other.Send(new MsgFriend(player.Id, FriendAction.FriendApply, true, player.Name));
                other.Tell($"{player.Name} wishes to make friends with you!", TalkChannel.System);
                player.Tell("Request of making friends has been sent out.", TalkChannel.System);
                break;

            case FriendAction.FriendRemove when player.Friends.FirstOrDefault(friend => friend.FriendPlayerId == Id) is { } broken:
                Proclaim($"{player.Name} broke up friendship with {broken.Name}.");
                player.RemoveFriend(Id);

                if (other is null)
                {
                    await store.RemoveFriendAsync(Id, player.Id);
                }
                else
                {
                    other.RemoveFriend(player.Id);
                }
                break;

            case FriendAction.EnemyRemove:
                player.RemoveEnemy(Id);
                break;

            case FriendAction.GetInfo when other is not null:
                player.Send(new MsgFriend(Id, FriendAction.FriendOnline, true, other.Name));
                break;
        }

        void Proclaim(string text) => player.Map?.Broadcast(player, MsgTalk.System(text));
    }
}