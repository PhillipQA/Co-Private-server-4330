namespace Conquer.Game.Models;

public partial class Player
{
    public bool HasTask(uint task) => (Tasks & Flag(task)) != 0;

    public void SetTask(uint task, bool active) => Tasks = active ? Tasks | Flag(task) : Tasks & ~Flag(task);

    private static ulong Flag(uint task) =>
        task < 64 ? 1ul << (int)task : throw new ArgumentOutOfRangeException(nameof(task), task, "There are only 64 task flags.");

    public int GetProgress(uint quest, uint slot) => Progress(quest, slot)?.Value ?? 0;

    public void SetProgress(uint quest, uint slot, int value) => Track(quest, slot).Value = value;

    public void Stamp(uint quest, uint slot) => Track(quest, slot).Timestamp = World.Now;

    public int SecondsSince(uint quest, uint slot) =>
        Progress(quest, slot)?.Timestamp is { } stamp ? (int)(World.Now - stamp).TotalSeconds : int.MaxValue;

    private Quest? Progress(uint quest, uint slot) => Quests.Find(entry => entry.QuestId == quest && entry.Slot == slot);

    private Quest Track(uint quest, uint slot)
    {
        if (Progress(quest, slot) is { } entry)
        {
            return entry;
        }

        entry = new() { PlayerId = Id, QuestId = quest, Slot = slot };
        Quests.Add(entry);
        return entry;
    }
}
