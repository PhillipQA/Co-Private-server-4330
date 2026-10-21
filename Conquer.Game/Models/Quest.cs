namespace Conquer.Game.Models;

public record Quest
{
    public uint PlayerId { get; set; }
    public uint QuestId { get; set; }
    public uint Slot { get; set; }
    public int Value { get; set; }
    public DateTime? Timestamp { get; set; }
}
