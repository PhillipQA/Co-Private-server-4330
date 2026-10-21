using System.Buffers;

namespace Conquer.Game.Models;

public partial class Player : Fighter
{
    public const int MaxNameLength = 15;
    private const byte BranchRankDrop = 20;

    public static readonly (uint Map, ushort X, ushort Y) LoginPoint = (1002, 430, 380);
    public static readonly (uint Map, ushort X, ushort Y) BirthPoint = (1010, 60, 108);

    private static readonly SearchValues<char> ForbiddenNameCharacters = SearchValues.Create(" ;,/\\=%@'\"[]");

    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public uint Model { get; set; }
    public ushort Avatar { get; set; }
    public ushort Hair { get; set; }
    public uint Money { get; set; }
    public uint StorageMoney { get; set; }
    public uint StoragePin { get; set; }
    public ulong Experience { get; set; }
    public ushort Strength { get; set; }
    public ushort Agility { get; set; }
    public ushort Vitality { get; set; }
    public ushort Spirit { get; set; }
    public ushort AttributePoints { get; set; }
    public uint Virtue { get; set; }
    public ushort Mana { get; set; }
    public PkMode PkMode { get; set; }
    public short PkPoints { get; set; }
    public Profession Profession { get; set; }
    public byte Rebirths { get; set; }
    public Profession? PreviousProfession { get; set; }
    public string Spouse { get; set; } = Unmarried;
    public uint HomeId { get; set; }
    public List<Item> Items { get; set; } = null!;
    public List<WeaponSkill> WeaponSkills { get; set; } = null!;
    public List<Magic> Magics { get; set; } = null!;
    public List<Friend> Friends { get; set; } = null!;
    public List<Enemy> Enemies { get; set; } = null!;
    public List<Prize> Prizes { get; set; } = [];
    public List<Quest> Quests { get; set; } = null!;
    public ulong Tasks { get; set; }
    public ushort ExperienceBoost { get; set; }
    public uint ExperienceBoostTime { get; set; }
    public ushort RebirthBonus { get; set; }
    public int KoRecord { get; set; }
    public uint? GuildId { get; set; }
    public GuildRank GuildRank { get; set; }
    public uint GuildDonation { get; set; }
    public uint RecordMapId { get; set; } = LoginPoint.Map;
    public ushort RecordX { get; set; } = LoginPoint.X;
    public ushort RecordY { get; set; } = LoginPoint.Y;

    public GameClient Client { get; set; } = null!;
    public World World { get; internal set; } = null!;
    public Dialog? Dialog { get; internal set; }
    public Team? Team { get; internal set; }
    public Trade? Trade { get; internal set; }
    public Booth? Booth { get; internal set; }
    public uint TradeRequest { get; internal set; }
    public uint GuildApplication { get; internal set; }
    public uint GuildInvitation { get; internal set; }
    public uint FriendRequest { get; internal set; }

    public override IMessage Spawn => new MsgPlayer(this);
    public uint Look => (uint)(Model + Avatar * 10000) + (_ghost ? IsFemale ? FemaleGhost : MaleGhost : Form?.Look * FormLook ?? 0);
    public Guild? Guild => GuildId is { } guildId ? World.Guilds.GetValueOrDefault(guildId) : null;
    public GuildRank ShownGuildRank => GuildRank > GuildRank.Member && Guild?.ParentGuildId is not null ? GuildRank - BranchRankDrop : GuildRank;
    public bool IsFemale => Model / 1000 == 2;
    public bool IsMarried => Spouse != Unmarried;
    public bool IsGm => Name.Contains("[GM]") || Name.Contains("[PM]");
    public bool IsWanted => PkPoints >= 100;
    public bool IsWaterTaoist => Profession is >= Profession.WaterTaoist and <= Profession.WaterSaint;

    public static bool HasAllowedCharacters(string name) =>
        !name.Any(char.IsControl) && name.AsSpan().IndexOfAny(ForbiddenNameCharacters) < 0;

    public void Send(IMessage message) => Client.Write(message);

    public void Tell(string text, TalkChannel channel = TalkChannel.Talk) => Send(new MsgTalk(channel, "SYSTEM", Name, "", text));

    public void OpenDialog(uint dialog) => Send(Act(ActionType.OpenDialog, dialog));

    public void Post(ClientCommand command) => Send(Act(ActionType.PostCommand, (uint)command));

    public void CountScore(bool on) => Send(Act(ActionType.ScoreCounter, on ? 1u : 0));

    public void AddScore(uint points) => Send(Act(ActionType.AddScore, points));

    public void SetGuild(uint? guildId, GuildRank rank, uint donation)
    {
        GuildId = guildId;
        GuildRank = rank;
        GuildDonation = donation;
        GuildApplication = 0;
        GuildInvitation = 0;
        Map?.Broadcast(this, Spawn);
    }

    public bool SpendGuildFund(uint amount) => Guild?.Spend(amount) == true;

    public int Roll(int sides) => World.Roll(sides);

    public int Roll(int first, int last) => World.Roll(first, last);

    public T Pick<T>(params ReadOnlySpan<T> choices) => World.Pick(choices);

    private void Update(AttributeType type, uint value) => Send(new MsgUserAttrib(Id, type, value));
}
