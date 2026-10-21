namespace Conquer.Game.Scripts.Drops;

public static class CentralPlain
{
    [Monster(1)]
    public static void Pheasant(Player player, Monster monster)
    {
        if (player.Profession <= Profession.Trojan || player.Profession <= Profession.Warrior)
        {
            if (player.Rebirths < 1)
            {
                if (player.Level < 5)
                {
                    if (player.Roll(20) < 3)
                    {
                        monster.DropOneOf(player, 410004, 420004, 430004, 440004, 450004, 460004, 480004, 481004, 490004, 510004, 530004, 540004,
                            560004, 580004, 561004);
                    }
                    else if (player.Roll(20) < 1)
                    {
                        monster.DropOneOf(player, 721287, 721288, 721273, 721274, 721275, 721276, 721277, 721278, 721279, 721280, 721281, 721282,
                            721283, 721284);

                        player.Inform("With a flash of light a weapon skill book comes out of the monster you`ve killed!");
                    }
                    else if (player.Roll(100) < 1)
                    {
                        monster.Drop(710001, player);
                    }
                }
                else if (player.Roll(100) < 1)
                {
                    monster.Drop(710001, player);
                }
            }
            else if (player.Roll(100) < 1)
            {
                monster.Drop(710001, player);
            }
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(710001, player);
        }
    }

    [Monster(2)]
    public static void Turtledove(Player player, Monster monster)
    {
        if (player.Profession <= Profession.Trojan)
        {
            if (player.Rebirths >= 1 || player.Level >= 10)
            {
                return;
            }

            if (player.Roll(25) < 1)
            {
                monster.DropOneOf(player, 410014, 420014, 430014, 440014, 450014, 460014, 480014, 481014, 490014, 510014, 530014, 540014, 560014,
                    580014, 561014);
            }
            else if (player.Roll(50) < 1)
            {
                monster.DropOneOf(player, 721287, 721288, 721273, 721274, 721275, 721276, 721277, 721278, 721279, 721280, 721281, 721282, 721283,
                    721284);

                player.Inform("With a flash of light a weapon skill book comes out of the monster you`ve killed!");
            }
        }
        else if (player.Profession <= Profession.Warrior && player.Rebirths < 1 && player.Level < 10)
        {
            if (player.Roll(25) < 1)
            {
                monster.DropOneOf(player, 410014, 420014, 430014, 440014, 450014, 460014, 480014, 481014, 490014, 510014, 530014, 540014, 560014,
                    580014, 561014);
            }
            else if (player.Roll(50) < 1)
            {
                monster.DropOneOf(player, 721287, 721288, 721273, 721274, 721275, 721276, 721277, 721278, 721279, 721280, 721281, 721282, 721283,
                    721284);

                player.Inform("With a flash of light a weapon skill book comes out of the monster you`ve killed!");
            }
        }
    }

    [Monster(3)]
    public static void Robin(Player player, Monster monster)
    {
        if (player.Profession <= Profession.Trojan)
        {
            if (player.Rebirths >= 1 || player.Level >= 15)
            {
                return;
            }

            if (player.Roll(100) < 1)
            {
                monster.DropOneOf(player, 410024, 420024, 430024, 440024, 450024, 460024, 480024, 481024, 490024, 510024, 530024, 540024, 560024,
                    580024, 561024);
            }
            else if (player.Roll(250) < 1)
            {
                monster.DropOneOf(player, 721287, 721288, 721273, 721274, 721275, 721276, 721277, 721278, 721279, 721280, 721281, 721282, 721283,
                    721284);

                player.Inform("With a flash of light a weapon skill book comes out of the monster you`ve killed!");
            }
        }
        else if (player.Profession <= Profession.Warrior && player.Rebirths < 1 && player.Level < 15)
        {
            if (player.Roll(100) < 1)
            {
                monster.DropOneOf(player, 410024, 420024, 430024, 440024, 450024, 460024, 480024, 481024, 490024, 510024, 530024, 540024, 560024,
                    580024, 561024);
            }
            else if (player.Roll(250) < 1)
            {
                monster.DropOneOf(player, 721287, 721288, 721273, 721274, 721275, 721276, 721277, 721278, 721279, 721280, 721281, 721282, 721283,
                    721284);

                player.Inform("With a flash of light a weapon skill book comes out of the monster you`ve killed!");
            }
        }
    }

    [Monster(4)]
    public static void Apparition(Player player, Monster monster)
    {
        if (player.Profession <= Profession.Trojan)
        {
            if (player.Rebirths >= 1 || player.Level >= 20 || player.Roll(100) >= 1)
            {
                return;
            }

            monster.DropOneOf(player, 410034, 420034, 430034, 440034, 450034, 460034, 480034, 481034, 490034, 510034, 530034, 540034, 560034, 580034,
                561034);
        }
        else if (player.Profession <= Profession.Warrior && player.Rebirths < 1 && player.Level < 20 &&
            player.Roll(100) < 1)
        {
            monster.DropOneOf(player, 410034, 420034, 430034, 440034, 450034, 460034, 480034, 481034, 490034, 510034, 530034, 540034, 560034, 580034,
                561034);
        }
    }

    [Monster(21)]
    public static void FlyingRooster(Player player, Monster monster)
    {
        switch (player.Roll(1, 8))
        {
            case 1 or 2 or 7 or 8:
                monster.Drop(410005, player);
                monster.Drop(420005, player);
                monster.Drop(421005, player);
                monster.Drop(500005, player);
                break;

            case 3 or 4:
                monster.Drop(420005, player);
                monster.Drop(420005, player);
                monster.Drop(410005, player);
                monster.Drop(410005, player);
                break;

            case 5 or 6:
                monster.Drop(500005, player);
                monster.Drop(490005, player);
                monster.Drop(500005, player);
                monster.Drop(500005, player);
                break;
        }

        switch (player.Roll(1, 8))
        {
            case 1:
                monster.Drop(410006, player, 312, 10000);
                monster.Drop(420006, player, 312, 10000);
                monster.Drop(421006, player, 312, 10000);
                monster.Drop(430006, player, 312, 10000);
                monster.Drop(440006, player, 312, 10000);
                monster.Drop(450006, player, 312, 10000);
                monster.Drop(460006, player, 312, 10000);
                monster.Drop(480006, player, 312, 10000);
                break;

            case 2:
                monster.Drop(481006, player, 312, 10000);
                monster.Drop(490006, player, 312, 10000);
                monster.Drop(510006, player, 312, 10000);
                monster.Drop(530006, player, 312, 10000);
                monster.Drop(540006, player, 312, 10000);
                monster.Drop(560006, player, 312, 10000);
                monster.Drop(580006, player, 312, 10000);
                monster.Drop(561006, player, 312, 10000);
                break;

            case 3:
                monster.Drop(410007, player, 100, 10000);
                monster.Drop(420007, player, 100, 10000);
                monster.Drop(421007, player, 100, 10000);
                monster.Drop(430007, player, 100, 10000);
                monster.Drop(440007, player, 100, 10000);
                monster.Drop(450007, player, 100, 10000);
                monster.Drop(460007, player, 100, 10000);
                monster.Drop(480007, player, 100, 10000);
                break;

            case 4:
                monster.Drop(481007, player, 100, 10000);
                monster.Drop(490007, player, 100, 10000);
                monster.Drop(410007, player, 100, 10000);
                monster.Drop(530007, player, 100, 10000);
                monster.Drop(540007, player, 100, 10000);
                monster.Drop(560007, player, 100, 10000);
                monster.Drop(580007, player, 100, 10000);
                monster.Drop(561007, player, 100, 10000);
                break;

            case 5:
                monster.Drop(410008, player, 625, 1000000);
                monster.Drop(420008, player, 625, 1000000);
                monster.Drop(421008, player, 625, 1000000);
                monster.Drop(430008, player, 625, 1000000);
                monster.Drop(440008, player, 625, 1000000);
                monster.Drop(450008, player, 625, 1000000);
                monster.Drop(460008, player, 625, 1000000);
                monster.Drop(480008, player, 625, 1000000);
                break;

            case 6:
                monster.Drop(481008, player, 625, 1000000);
                monster.Drop(490008, player, 625, 1000000);
                monster.Drop(510008, player, 625, 1000000);
                monster.Drop(530008, player, 625, 1000000);
                monster.Drop(540008, player, 625, 1000000);
                monster.Drop(560008, player, 625, 1000000);
                monster.Drop(580008, player, 625, 1000000);
                monster.Drop(561008, player, 625, 1000000);
                break;

            case 7:
                monster.Drop(410009, player, 625, 10000000);
                monster.Drop(420009, player, 625, 10000000);
                monster.Drop(421009, player, 625, 10000000);
                monster.Drop(430009, player, 625, 10000000);
                monster.Drop(440009, player, 625, 10000000);
                monster.Drop(450009, player, 625, 10000000);
                monster.Drop(460009, player, 625, 10000000);
                monster.Drop(480009, player, 625, 10000000);
                break;

            case 8:
                monster.Drop(481009, player, 625, 10000000);
                monster.Drop(490009, player, 625, 10000000);
                monster.Drop(510009, player, 625, 10000000);
                monster.Drop(530009, player, 625, 10000000);
                monster.Drop(540009, player, 625, 10000000);
                monster.Drop(560009, player, 625, 10000000);
                monster.Drop(580009, player, 625, 10000000);
                monster.Drop(561009, player, 625, 10000000);
                break;
        }
    }

    [Monster(22)]
    public static void BlueCooer(Player player, Monster monster)
    {
        switch (player.Roll(1, 8))
        {
            case <= 4:
                monster.Drop(410015, player);
                monster.Drop(420015, player);
                monster.Drop(421015, player);
                break;

            case 5 or 6:
                monster.Drop(500015, player);
                monster.Drop(490015, player);
                monster.Drop(500015, player);
                break;

            case 7 or 8:
                monster.Drop(500015, player);
                monster.Drop(500015, player);
                monster.Drop(500015, player);
                break;
        }

        monster.Drop(500015, player);

        switch (player.Roll(1, 8))
        {
            case 1:
                monster.Drop(410016, player, 312, 10000);
                monster.Drop(420016, player, 312, 10000);
                monster.Drop(421016, player, 312, 10000);
                monster.Drop(430016, player, 312, 10000);
                monster.Drop(440016, player, 312, 10000);
                monster.Drop(450016, player, 312, 10000);
                monster.Drop(500016, player, 312, 10000);
                monster.Drop(480026, player, 312, 10000);
                break;

            case 2:
                monster.Drop(410016, player, 312, 10000);
                monster.Drop(490016, player, 312, 10000);
                monster.Drop(510016, player, 312, 10000);
                monster.Drop(530016, player, 312, 10000);
                monster.Drop(410016, player, 312, 10000);
                monster.Drop(560016, player, 312, 10000);
                monster.Drop(580016, player, 312, 10000);
                monster.Drop(561016, player, 312, 10000);
                break;

            case 3:
                monster.Drop(410017, player, 100, 10000);
                monster.Drop(420017, player, 100, 10000);
                monster.Drop(421017, player, 100, 10000);
                monster.Drop(430017, player, 100, 10000);
                monster.Drop(440117, player, 100, 10000);
                monster.Drop(450017, player, 100, 10000);
                monster.Drop(460017, player, 100, 10000);
                monster.Drop(480017, player, 100, 10000);
                break;

            case 4:
                monster.Drop(481017, player, 100, 10000);
                monster.Drop(490017, player, 100, 10000);
                monster.Drop(500017, player, 100, 10000);
                monster.Drop(530017, player, 100, 10000);
                monster.Drop(540017, player, 100, 10000);
                monster.Drop(560017, player, 100, 10000);
                monster.Drop(580017, player, 100, 10000);
                monster.Drop(561017, player, 100, 10000);
                break;

            case 5:
                monster.Drop(410018, player, 625, 1000000);
                monster.Drop(420018, player, 625, 1000000);
                monster.Drop(421018, player, 625, 1000000);
                monster.Drop(430018, player, 625, 1000000);
                monster.Drop(440018, player, 625, 1000000);
                monster.Drop(450018, player, 625, 1000000);
                monster.Drop(460018, player, 625, 1000000);
                monster.Drop(480018, player, 625, 1000000);
                break;

            case 6:
                monster.Drop(481018, player, 625, 1000000);
                monster.Drop(490018, player, 625, 1000000);
                monster.Drop(510018, player, 625, 1000000);
                monster.Drop(530018, player, 625, 1000000);
                monster.Drop(540018, player, 625, 1000000);
                monster.Drop(560018, player, 625, 1000000);
                monster.Drop(580018, player, 625, 1000000);
                monster.Drop(561018, player, 625, 1000000);
                break;

            case 7:
                monster.Drop(410019, player, 625, 10000000);
                monster.Drop(420019, player, 625, 10000000);
                monster.Drop(421019, player, 625, 10000000);
                monster.Drop(430019, player, 625, 10000000);
                monster.Drop(440019, player, 625, 10000000);
                monster.Drop(450019, player, 625, 10000000);
                monster.Drop(460019, player, 625, 10000000);
                monster.Drop(480019, player, 625, 10000000);
                break;

            case 8:
                monster.Drop(481019, player, 625, 10000000);
                monster.Drop(490019, player, 625, 10000000);
                monster.Drop(510019, player, 625, 10000000);
                monster.Drop(530019, player, 625, 10000000);
                monster.Drop(540019, player, 625, 10000000);
                monster.Drop(560019, player, 625, 10000000);
                monster.Drop(580019, player, 625, 10000000);
                monster.Drop(561019, player, 625, 10000000);
                break;
        }
    }

    [Monster(23)]
    public static void GameBird(Player player, Monster monster) => monster.DropEquipment(player, 25);

    [Monster(24)]
    public static void WarGhost(Player player, Monster monster) =>
        monster.DropEquipment(player, 35, [410, 420, 410, 430], [440, 450, 460, 480], [481, 490, 510, 530], [540, 560, 580, 561]);

    [Monster(25)]
    public static void BattleGhost(Player player, Monster monster) => monster.DropEquipment(player, 45);

    [Monster(64)]
    public static void HeavyGhostL23(Player player, Monster monster) => monster.Drop(725016, player, 1, 150000);

    [Monster(8100)]
    public static void HumanMessenger(Player player, Monster monster)
    {
        if (player.Roll(100) < 5)
        {
            monster.Drop(player.Pick<uint>(160050, 150050, 410050, 420050, 430050, 440050, 450050, 450050), player);
        }
        else if (player.Roll(100) < 5)
        {
            monster.Drop(player.Pick<uint>(460050, 130320, 131320, 134320, 460050, 130320, 131320, 134320), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8200)]
    public static void HumanAide(Player player, Monster monster)
    {
        if (player.Roll(100) < 2)
        {
            monster.Drop(player.Pick<uint>(460050, 480050, 481050, 490050, 510050, 530050, 540050, 540050), player);
        }
        else if (player.Roll(100) < 2)
        {
            monster.Drop(player.Pick<uint>(460050, 130320, 131320, 134320, 460050, 130320, 131320, 134320), player);
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }

    [Monster(8300)]
    public static void GhostKing(Player player, Monster monster)
    {
        if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
        else if (player.Roll(100) < 1)
        {
            switch (player.Roll(1, 8))
            {
                case 1 or 4:
                    monster.Drop(ItemType.Meteor, player);
                    monster.Drop(ItemType.Meteor, player);
                    break;

                case 2 or 3 or 5 or 7 or 8:
                    monster.Drop(ItemType.Meteor, player);
                    break;

                case 6:
                    monster.Drop(ItemType.Meteor, player, 1, 100);
                    break;
            }
        }
        else if (player.Roll(100) < 1)
        {
            monster.Drop(ItemType.Meteor, player);
        }
    }
}
