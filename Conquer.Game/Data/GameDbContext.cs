using Microsoft.EntityFrameworkCore;

namespace Conquer.Game.Data;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<Bounty> Bounties => Set<Bounty>();
    public DbSet<DynaNpc> DynaNpcs => Set<DynaNpc>();
    public DbSet<Enemy> Enemies => Set<Enemy>();
    public DbSet<Friend> Friends => Set<Friend>();
    public DbSet<Generator> Generators => Set<Generator>();
    public DbSet<Guild> Guilds => Set<Guild>();
    public DbSet<GuildAlly> GuildAllies => Set<GuildAlly>();
    public DbSet<GuildEnemy> GuildEnemies => Set<GuildEnemy>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<ItemPlus> ItemPluses => Set<ItemPlus>();
    public DbSet<ItemType> ItemTypes => Set<ItemType>();
    public DbSet<OfflineMessage> OfflineMessages => Set<OfflineMessage>();
    public DbSet<LevelExp> LevelExps => Set<LevelExp>();
    public DbSet<Magic> Magics => Set<Magic>();
    public DbSet<MagicType> MagicTypes => Set<MagicType>();
    public DbSet<MapInfo> Maps => Set<MapInfo>();
    public DbSet<MonsterType> MonsterTypes => Set<MonsterType>();
    public DbSet<Npc> Npcs => Set<Npc>();
    public DbSet<Passway> Passways => Set<Passway>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PointAllot> PointAllots => Set<PointAllot>();
    public DbSet<Prize> Prizes => Set<Prize>();
    public DbSet<Portal> Portals => Set<Portal>();
    public DbSet<Quest> Quests => Set<Quest>();
    public DbSet<Rebirth> Rebirths => Set<Rebirth>();
    public DbSet<Region> Regions => Set<Region>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();
    public DbSet<Trap> Traps => Set<Trap>();
    public DbSet<TrapType> TrapTypes => Set<TrapType>();
    public DbSet<WeaponSkill> WeaponSkills => Set<WeaponSkill>();
    public DbSet<WeaponSkillExp> WeaponSkillExps => Set<WeaponSkillExp>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Fighter>();
        modelBuilder.Ignore<Monster>();
        modelBuilder.Ignore<GameClient>();
        modelBuilder.Ignore<Map>();
        modelBuilder.Ignore<Team>();
        modelBuilder.Ignore<Trade>();
        modelBuilder.Ignore<Booth>();
        modelBuilder.Ignore<DiceTable>();
        modelBuilder.Ignore<World>();
        modelBuilder.Ignore<Dialog>();

        modelBuilder.Entity<Player>(player =>
        {
            player
                .Ignore(e => e.Inventory).Ignore(e => e.Carried)
                .Ignore(e => e.Stats).Ignore(e => e.Stamina).Ignore(e => e.Xp).Ignore(e => e.Pose).Ignore(e => e.NextAttack)
                .Ignore(e => e.Kills).Ignore(e => e.Form).Ignore(e => e.KnownMagics).Ignore(e => e.KnownWeaponSkills)
                .Ignore(e => e.TradeRequest).Ignore(e => e.GuildApplication).Ignore(e => e.GuildInvitation).Ignore(e => e.FriendRequest)
                .Ignore(e => e.Hunting).Ignore(e => e.Pursuing).Ignore(e => e.Status).Ignore(e => e.ProposedTo)
                .Ignore(e => e.LuckyTime);

            player.Property(e => e.Id).UseIdentityColumn(1000001);
            player.Property(e => e.Name).HasMaxLength(Player.MaxNameLength);
            player.Property(e => e.UserId).HasMaxLength(450);
            player.Property(e => e.Tasks).HasConversion<long>();
            player.HasIndex(e => e.Name).IsUnique();
            player.HasIndex(e => e.UserId).IsUnique();
            player.HasOne<Guild>().WithMany().HasForeignKey(e => e.GuildId);
        });

        modelBuilder.Entity<DynaNpc>()
            .Ignore(e => e.Health).Ignore(e => e.Direction).Ignore(e => e.Status).Ignore(e => e.Stats)
            .Ignore(e => e.NextAttack).Ignore(e => e.Kills).Ignore(e => e.Contents)
            .OwnsOne(e => e.Statue);

        modelBuilder.Entity<Magic>()
            .HasKey(e => new { e.PlayerId, e.Type });

        modelBuilder.Entity<WeaponSkill>()
            .HasKey(e => new { e.PlayerId, e.Type });

        modelBuilder.Entity<Quest>()
            .HasKey(e => new { e.PlayerId, e.QuestId, e.Slot });

        modelBuilder.Entity<Friend>()
            .HasKey(e => new { e.PlayerId, e.FriendPlayerId });

        modelBuilder.Entity<Enemy>()
            .HasKey(e => new { e.PlayerId, e.EnemyPlayerId });

        modelBuilder.Entity<Guild>(guild =>
        {
            guild.Ignore(e => e.MemberCount).Ignore(e => e.LeaderName);
            guild.Property(e => e.Name).HasMaxLength(GuildService.MaxNameLength);
            guild.Property(e => e.Announcement).HasMaxLength(GuildService.MaxAnnouncementLength);
            guild.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<GuildAlly>()
            .HasIndex(e => new { e.GuildId, e.AllyGuildId }).IsUnique();

        modelBuilder.Entity<GuildEnemy>()
            .HasIndex(e => new { e.GuildId, e.EnemyGuildId }).IsUnique();

        modelBuilder.Entity<OfflineMessage>(message =>
        {
            message.HasOne<Player>().WithMany().HasForeignKey(e => e.PlayerId);
            message.Property(e => e.Sender).HasMaxLength(Player.MaxNameLength);
            message.Property(e => e.Words).HasMaxLength(OfflineMessage.MaxLength);
        });

        modelBuilder.Entity<Bounty>(bounty =>
        {
            bounty.Property(e => e.Target).HasMaxLength(Player.MaxNameLength);
            bounty.Property(e => e.Payer).HasMaxLength(Player.MaxNameLength);
        });

        foreach (var type in new[] { typeof(Bounty), typeof(DynaNpc), typeof(Item), typeof(MapInfo), typeof(Rebirth), typeof(Region), typeof(Trap), typeof(TrapType) })
        {
            modelBuilder.Entity(type).Property("Id").ValueGeneratedNever();
        }

        modelBuilder.Entity<LevelExp>()
            .HasKey(e => e.Level);

        modelBuilder.Entity<WeaponSkillExp>()
            .HasKey(e => e.Level);
    }
}