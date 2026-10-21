namespace Conquer.Game.Models;

public class MapItem : Entity
{
    private const uint LongestLife = 180;
    private const uint ShortestLife = 60;
    private const uint WorthPerSecond = 8;

    private static readonly TimeSpan OwnerTime = TimeSpan.FromSeconds(30);

    private static readonly (uint Amount, uint ItemTypeId)[] Piles =
        [(10, 1090000), (100, 1090010), (1000, 1090020), (2000, 1091000), (5000, 1091010), (uint.MaxValue, 1091020)];

    public Item? Item { get; init; }
    public uint Money { get; init; }
    public uint OwnerId { get; init; }
    public uint? Price { get; init; }
    public DateTimeOffset Dropped { get; set; }

    public uint ItemTypeId => Item?.ItemTypeId ?? Piles.First(pile => Money <= pile.Amount).ItemTypeId;
    public override IMessage Spawn => new MsgMapItem(this, MapItemAction.Create);
    public override IMessage Despawn => new MsgMapItem(this, MapItemAction.Delete);

    public TimeSpan Lifetime => TimeSpan.FromSeconds(Item is null
        ? OwnerId == 0 ? LifeFor(Money) : LongestLife
        : Price is { } price ? LifeFor(price) : LongestLife);

    public bool IsExpired(DateTimeOffset now) => now >= Dropped + Lifetime;

    private static uint LifeFor(uint worth) => Math.Min(worth / WorthPerSecond + ShortestLife, LongestLife);

    public bool IsReservedFrom(Player player, DateTimeOffset now) =>
        OwnerId != 0 && OwnerId != player.Id && now < Dropped + OwnerTime && !SharedWith(player) && !IsMateOf(player);

    private bool IsMateOf(Player player) => player.IsMarried && player.World.Players.GetValueOrDefault(OwnerId)?.Name == player.Spouse;

    private bool SharedWith(Player player) =>
        player.Team is { } team && team.Has(OwnerId) && ItemTypeId is not (ItemType.DragonBall or ItemType.Meteor) &&
        (Item is null ? team.SharesMoney : team.SharesItems);
}
