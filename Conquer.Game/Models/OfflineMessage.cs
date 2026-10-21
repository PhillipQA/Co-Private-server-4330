namespace Conquer.Game.Models;

public record OfflineMessage
{
    public const string Stamp = "yyyyMMddHHmmss";
    public const int MaxLength = 255;

    public static readonly TimeSpan Lifetime = TimeSpan.FromDays(31);

    public uint Id { get; set; }
    public uint PlayerId { get; set; }
    public string Sender { get; set; } = null!;
    public DateTime SentAt { get; set; }
    public string Words { get; set; } = null!;
}
