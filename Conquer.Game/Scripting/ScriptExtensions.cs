namespace Conquer.Game.Scripting;

public static class ScriptExtensions
{
    extension(Player player)
    {
        public void Inform(string text) => player.Tell(text, TalkChannel.System);

        public void Alert(string text) => player.Tell(text, TalkChannel.Gm);

        public void AddProgress(uint quest, uint slot, int amount) => player.SetProgress(quest, slot, player.GetProgress(quest, slot) + amount);

        public bool Teleport(uint mapId, ushort x, ushort y, (uint Map, ushort X, ushort Y) record, bool force = false)
        {
            if (!player.Teleport(mapId, x, y, force))
            {
                return false;
            }

            player.Record(record.Map, record.X, record.Y);
            return true;
        }

        public bool Teleport(uint mapId, ReadOnlySpan<(ushort X, ushort Y)> spots, bool force = false)
        {
            var (x, y) = player.Pick(spots);
            return player.Teleport(mapId, x, y, force);
        }

        public bool Teleport(uint mapId, ReadOnlySpan<(ushort X, ushort Y)> spots, (uint Map, ushort X, ushort Y) record, bool force = false)
        {
            var (x, y) = player.Pick(spots);
            return player.Teleport(mapId, x, y, record, force);
        }
    }
}
