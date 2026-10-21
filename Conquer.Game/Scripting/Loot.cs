namespace Conquer.Game.Scripting;

public static class Loot
{
    private static readonly ushort[][] Common = [[410, 420, 421, 430], [440, 450, 460, 480], [481, 490, 510, 530], [540, 560, 580, 561]];
    private static readonly ushort[][] Rare = [[410, 420, 421, 430, 440, 450, 460, 480], [481, 490, 510, 530, 540, 560, 580, 561]];
    private static readonly (int Chance, int OutOf)[] RareOdds = [(312, 10_000), (100, 10_000), (625, 1_000_000), (625, 10_000_000)];

    extension(Monster monster)
    {
        public void Drop(uint itemTypeId, Player owner, int chance, int outOf)
        {
            if (owner.Roll(outOf) < chance)
            {
                monster.Drop(itemTypeId, owner);
            }
        }

        public void DropOneOf(Player owner, params ReadOnlySpan<uint> itemTypeIds)
        {
            for (var i = 0; i < itemTypeIds.Length - 1; i++)
            {
                if (owner.Roll(itemTypeIds.Length - i) < 1)
                {
                    monster.Drop(itemTypeIds[i], owner);
                    return;
                }
            }

            monster.Drop(itemTypeIds[^1], owner);
        }

        public void DropEquipment(Player owner, uint series, params ReadOnlySpan<ushort[]> common)
        {
            var roll = owner.Roll(1, 8) - 1;

            foreach (var kind in common.IsEmpty ? Common[roll / 2] : common[roll / 2])
            {
                monster.Drop(kind * 1000u + series, owner);
            }

            roll = owner.Roll(1, 8) - 1;
            var quality = roll / 2;
            var (chance, outOf) = RareOdds[quality];

            foreach (var kind in Rare[roll % 2])
            {
                monster.Drop(kind * 1000u + series + 1 + (uint)quality, owner, chance, outOf);
            }
        }
    }
}
