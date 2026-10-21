namespace Conquer.Game.Models;

public partial class Player
{
    public Player Snapshot()
    {
        var snapshot = (Player)MemberwiseClone();
        snapshot.Client = null!;
        snapshot.World = null!;
        snapshot.Map = null;
        snapshot.Dialog = null;
        snapshot.Target = null;
        snapshot.Items = [..Items.Select(item => item with { })];
        snapshot.Magics = [..Magics.Select(magic => magic with { })];
        snapshot.WeaponSkills = [..WeaponSkills.Select(skill => skill with { })];
        snapshot.Quests = [..Quests.Select(quest => quest with { })];
        snapshot.Friends = [..Friends.Select(friend => friend with { })];
        snapshot.Enemies = [..Enemies.Select(enemy => enemy with { })];
        snapshot.Prizes = [..Prizes.Select(prize => prize with { })];
        return snapshot;
    }

    public IEnumerable<object> Rows() => [this, ..Items, ..Magics, ..WeaponSkills, ..Quests, ..Friends, ..Enemies, ..Prizes];
}
