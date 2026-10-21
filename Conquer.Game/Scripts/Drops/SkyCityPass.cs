namespace Conquer.Game.Scripts.Drops;

public static class SkyCityPass
{
    [Monster(3000)]
    public static void OddMonster(Player player, Monster monster) => monster.Drop(721100, player, 1, 17);

    [Monster(3001)]
    public static void NorthMonster(Player player, Monster monster) => monster.Drop(721100, player, 1, 25);

    [Monster(3002)]
    public static void SouthMonster(Player player, Monster monster) => monster.Drop(721100, player, 1, 31);

    [Monster(3003)]
    public static void WestMonster(Player player, Monster monster) => monster.Drop(721100, player, 1, 36);

    [Monster(3004)]
    public static void EastMonster(Player player, Monster monster) => monster.Drop(721100, player, 1, 42);

    [Monster(3005)]
    public static void FuryGhost(Player player, Monster monster) => monster.Drop(721101, player, 1, 42);

    [Monster(3006)]
    public static void CrazyGhost(Player player, Monster monster) => monster.Drop(721101, player, 1, 36);

    [Monster(3007)]
    public static void RoaringGhost(Player player, Monster monster) => monster.Drop(721101, player, 1, 31);

    [Monster(3008)]
    public static void ScaryGhost(Player player, Monster monster) => monster.Drop(721101, player, 1, 25);

    [Monster(3009)]
    public static void BloodGhost(Player player, Monster monster) => monster.Drop(721101, player, 1, 17);

    [Monster(3010)]
    public static void IronBird(Player player, Monster monster) => monster.Drop(721102, player, 1, 42);

    [Monster(3011)]
    public static void CopperBird(Player player, Monster monster) => monster.Drop(721102, player, 1, 36);

    [Monster(3012)]
    public static void SilverBird(Player player, Monster monster) => monster.Drop(721102, player, 1, 31);

    [Monster(3013)]
    public static void GoldenBird(Player player, Monster monster) => monster.Drop(721102, player, 1, 25);

    [Monster(3014)]
    public static void FastBird(Player player, Monster monster) => monster.Drop(721102, player, 1, 17);

    [Monster(3015)]
    public static void BrutalBandit(Player player, Monster monster) => monster.Drop(721103, player, 1, 42);

    [Monster(3016)]
    public static void WildBandit(Player player, Monster monster) => monster.Drop(721103, player, 1, 36);

    [Monster(3017)]
    public static void CruelBandit(Player player, Monster monster) => monster.Drop(721103, player, 1, 31);

    [Monster(3018)]
    public static void SingleEye(Player player, Monster monster) => monster.Drop(721103, player, 1, 25);

    [Monster(3019)]
    public static void TyrantBandit(Player player, Monster monster) => monster.Drop(721103, player, 1, 17);

    [Monster(3020)]
    public static void AberrantEvil(Player player, Monster monster) => monster.Drop(721108, player, 1, 69);

    [Monster(3021)]
    public static void CecropsBoss(Player player, Monster monster) => monster.Drop(721107, player, 1, 50);

    [Monster(3022)]
    public static void Minotaur(Player player, Monster monster) => monster.Drop(721106, player, 1, 41);

    [Monster(3023)]
    public static void Jumpy(Player player, Monster monster) => monster.Drop(721105, player, 1, 36);

    [Monster(3024)]
    public static void EvilMonkey(Player player, Monster monster) => monster.Drop(721104, player, 1, 33);
}
