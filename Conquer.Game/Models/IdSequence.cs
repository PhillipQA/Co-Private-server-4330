namespace Conquer.Game.Models;

public sealed class IdSequence(uint first, uint last, Func<uint, bool>? taken = null)
{
    private uint _offset;

    public void Resume(uint next) => _offset = next >= first && next <= last ? next - first : 0;

    public uint Next()
    {
        var id = first + _offset;

        for (var tries = last - first; tries > 0 && taken?.Invoke(id) == true; tries--)
        {
            id = After(id);
        }

        _offset = After(id) - first;
        return id;
    }

    private uint After(uint id) => id == last ? first : id + 1;
}
