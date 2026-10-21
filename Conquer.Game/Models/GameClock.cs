namespace Conquer.Game.Models;

public sealed class GameClock : TimeProvider
{
    public override TimeZoneInfo LocalTimeZone { get; } =
        TimeZoneInfo.CreateCustomTimeZone("Conquer", TimeSpan.FromHours(-8), "Conquer", "Conquer");
}
