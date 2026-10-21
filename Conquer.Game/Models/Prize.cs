namespace Conquer.Game.Models;

public record Prize
{
    public uint Id { get; set; }
    public uint PlayerId { get; set; }
    public uint ItemTypeId { get; set; }
}
