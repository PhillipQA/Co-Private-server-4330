namespace Conquer.Game.Models;

// The 4330 client takes an item's name and description from its own itemtype.dat, so every copy of an item shows
// the same name. To make Legendary items show "L-<name>" and their powers in the tooltip, the client has a second
// "display" entry for every Super equipment item, and the server sends that display ID instead of the real one for
// Legendary items. The server itself always keeps the real item ID.
//
// The display ID is the Super ID with its last digit 9 changed to 1 (a quality that no equipment uses), except for
// the few items where that ID is already taken. tools/make_legendary_client.py builds the matching client entries
// with the same rule, so keep the two in sync.
public static class LegendaryLook
{
    private static readonly Dictionary<uint, uint> Exceptions = new()
    {
        [410309] = 410300,
        [421309] = 421302,
        [500309] = 500302
    };

    public static bool IsEquipmentId(uint itemTypeId) => (itemTypeId / 1000) switch
    {
        >= 111 and <= 160 => true,
        >= 400 and <= 599 => true,
        900 => true,
        _ => false
    };

    public static uint? DisplayIdOf(uint itemTypeId) =>
        itemTypeId % 10 != 9 || !IsEquipmentId(itemTypeId) ? null
        : Exceptions.TryGetValue(itemTypeId, out var exception) ? exception
        : itemTypeId - 8;

    // The item type ID to send to the client for this item.
    public static uint ShownTypeId(Item item) =>
        item.Legendary && DisplayIdOf(item.ItemTypeId) is { } shown ? shown : item.ItemTypeId;
}
