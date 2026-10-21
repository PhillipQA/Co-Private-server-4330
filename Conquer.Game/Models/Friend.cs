namespace Conquer.Game.Models;

public record Friend
{
    public uint PlayerId { get; set; }
    public uint FriendPlayerId { get; set; }
    public string Name { get; set; } = null!;
}
