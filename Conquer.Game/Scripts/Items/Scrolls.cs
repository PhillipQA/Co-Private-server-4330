namespace Conquer.Game.Scripts.Items;

public static class Scrolls
{
    [Item(1060020)]
    public static void TwinCityGate(Player player) => player.Teleport(1002, 429, 378, record: (1002, 429, 378));

    [Item(1060021)]
    public static void DesertCityGate(Player player) => player.Teleport(1000, 500, 650, record: (1000, 500, 650));

    [Item(1060022)]
    public static void ApeCityGate(Player player) => player.Teleport(1020, 565, 562, record: (1020, 565, 562));

    [Item(1060023)]
    public static void CastleGate(Player player) => player.Teleport(1011, 188, 264, record: (1011, 188, 264));

    [Item(1060024)]
    public static void BirdIslandGate(Player player) => player.Teleport(1015, 717, 571, record: (1015, 717, 571));

    [Item(1060025)]
    public static void WindSpellSouthwestOfTwinCity(Player player) => player.Teleport(1002, 411, 706, record: (1002, 411, 706));

    [Item(1060026)]
    public static void WindSpellNorthwestOfTwinCity(Player player) => player.Teleport(1002, 98, 325, record: (1002, 98, 325));

    [Item(1060027)]
    public static void WindSpellSoutheastOfTwinCity(Player player) => player.Teleport(1002, 796, 469, record: (1002, 796, 469));

    [Item(1060028)]
    public static void WindSpellSouthOfPhoenixCastle(Player player) => player.Teleport(1011, 540, 776, record: (1011, 540, 776));

    [Item(1060029)]
    public static void WindSpellSoutheastOfPhoenixCastle(Player player) => player.Teleport(1011, 737, 455, record: (1011, 737, 455));

    [Item(1060030)]
    public static void BlackDye(Player player) => player.DyeHair(3);

    [Item(1060031)]
    public static void WindSpellSoutheastOfApeCity(Player player) => player.Teleport(1020, 827, 605, record: (1020, 827, 605));

    [Item(1060032)]
    public static void WindSpellWestOfApeCity(Player player) => player.Teleport(1020, 497, 736, record: (1020, 497, 736));

    [Item(1060033)]
    public static void WindSpellNorthwestOfApeCity(Player player) => player.Teleport(1020, 108, 399, record: (1020, 108, 399));

    [Item(1060034)]
    public static void WindSpellNortheastOfDesertCity(Player player) => player.Teleport(1000, 230, 210, record: (1000, 230, 210));

    [Item(1060035)]
    public static void WindSpellEastOfDesertCity(Player player) => player.Teleport(1000, 794, 553, record: (1000, 794, 553));

    [Item(1060037)]
    public static void WindSpellToAncientCity(Player player) => player.Teleport(1001, 470, 366, record: (1001, 470, 366));

    [Item(1060038)]
    public static void WindSpellWestOfPhoenixCastle(Player player) => player.Teleport(1011, 64, 422, record: (1011, 64, 422));

    [Item(1060040)]
    public static void VioletDye(Player player) => player.DyeHair(9);

    [Item(1060050)]
    public static void BlueDye(Player player) => player.DyeHair(8);

    [Item(1060060)]
    public static void GreenDye(Player player) => player.DyeHair(7);

    [Item(1060070)]
    public static void BrownDye(Player player) => player.DyeHair(6);

    [Item(1060080)]
    public static void RedDye(Player player) => player.DyeHair(5);

    [Item(1060090)]
    public static void WhiteDye(Player player) => player.DyeHair(4);

    [Item(1060100)]
    public static void BombScroll(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.Level >= 80 &&
            player.Profession is Profession.FireWizard or Profession.FireMaster or Profession.FireSaint)
        {
            player.Learn(1160);

            dialog.Say("You have learned Bomb.", "Thanks.");
        }
        else
        {
            dialog.Say("Sorry, only Fire Wizards can learn this spell after they reach level 80.", "I see.");
        }
    }

    [Item(1060101)]
    public static void FireOfHell(Player player, Dialog dialog)
    {
        dialog.Avatar = 0;

        if (player.Level >= 82 &&
            player.Profession is Profession.FireWizard or Profession.FireMaster or Profession.FireSaint)
        {
            player.Learn(1165);

            dialog.Say("You have learned Fire of Hell.", "Thanks.");
        }
        else
        {
            dialog.Say("Sorry, only Fire Wizards can learn this spell after they reach level 82.", "I see.");
        }
    }

    [Item(1060102)]
    public static void StoneCityGate(Player player) => player.Teleport(1213, 448, 272);
}
