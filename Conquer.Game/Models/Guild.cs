namespace Conquer.Game.Models;

public class Guild
{
    public uint Id { get; set; }
    public string Name { get; set; } = null!;
    public uint Fund { get; private set; }
    public uint? ParentGuildId { get; set; }
    public string Announcement { get; private set; } = "";
    public List<GuildAlly> Allies { get; set; } = [];
    public List<GuildEnemy> Enemies { get; set; } = [];

    public int MemberCount { get; set; }
    public string LeaderName { get; set; } = "";
    public MessageBoard Board { get; } = new();

    public bool IsAlliedWith(uint guildId) => Allies.Any(ally => ally.AllyGuildId == guildId);

    public bool IsEnemyOf(uint guildId) => Enemies.Any(enemy => enemy.EnemyGuildId == guildId);

    public bool CanDeposit(uint amount) => (ulong)Fund + amount <= uint.MaxValue;

    public void Deposit(uint amount) => Fund = (uint)Math.Min((ulong)Fund + amount, uint.MaxValue);

    public uint Withdraw(uint amount)
    {
        var taken = Math.Min(amount, Fund);
        Fund -= taken;
        return taken;
    }

    public bool Spend(uint amount)
    {
        if (Fund < amount)
        {
            return false;
        }

        Withdraw(amount);
        return true;
    }

    public void SetAnnouncement(string announcement) => Announcement = announcement;

    public Guild Snapshot() => (Guild)MemberwiseClone();
}
