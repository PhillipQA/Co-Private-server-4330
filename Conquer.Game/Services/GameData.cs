using System.Collections.Frozen;
using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Services;

public class GameData(IDbContextFactory<GameDbContext> dbContextFactory)
{
    public FrozenDictionary<uint, ItemType> ItemTypes { get; private set; } = null!;
    public FrozenDictionary<(uint ItemTypeId, byte Level), ItemPlus> ItemPluses { get; private set; } = null!;
    public FrozenDictionary<(ushort Type, ushort Level), MagicType> MagicTypes { get; private set; } = null!;
    public FrozenDictionary<uint, MonsterType> MonsterTypes { get; private set; } = null!;
    public FrozenDictionary<(byte Profession, byte Level), PointAllot> PointAllots { get; private set; } = null!;
    public FrozenDictionary<byte, LevelExp> LevelExps { get; private set; } = null!;
    public FrozenDictionary<(byte From, byte To, byte Rebirths), Rebirth> Rebirths { get; private set; } = null!;
    public FrozenDictionary<byte, WeaponSkillExp> WeaponSkillExps { get; private set; } = null!;
    public FrozenDictionary<ushort, TrapType> TrapTypes { get; private set; } = null!;
    public ILookup<uint, uint> Shops { get; private set; } = null!;

    public async Task LoadAsync()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();

        ItemTypes = (await db.ItemTypes.ToListAsync()).ToFrozenDictionary(itemType => itemType.Id);
        ItemPluses = (await db.ItemPluses.ToListAsync()).ToFrozenDictionary(plus => (plus.ItemTypeId, plus.Level));
        MagicTypes = (await db.MagicTypes.ToListAsync()).ToFrozenDictionary(magicType => (magicType.Type, magicType.Level));
        MonsterTypes = (await db.MonsterTypes.ToListAsync()).ToFrozenDictionary(monsterType => monsterType.Id);
        PointAllots = (await db.PointAllots.ToListAsync()).ToFrozenDictionary(allot => (allot.Profession, allot.Level));
        LevelExps = (await db.LevelExps.ToListAsync()).ToFrozenDictionary(levelExp => levelExp.Level);
        Rebirths = (await db.Rebirths.ToListAsync()).ToFrozenDictionary(rebirth => (rebirth.RequiredProfession, rebirth.NewProfession, rebirth.Rebirths));
        WeaponSkillExps = (await db.WeaponSkillExps.ToListAsync()).ToFrozenDictionary(exp => exp.Level);
        TrapTypes = (await db.TrapTypes.ToListAsync()).ToFrozenDictionary(trapType => trapType.Id);
        Shops = (await db.ShopItems.ToListAsync()).ToLookup(shopItem => shopItem.NpcId, shopItem => shopItem.ItemTypeId);
    }
}
