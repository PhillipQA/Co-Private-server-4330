namespace Conquer.Game.Messages;

public enum WalkMode
{
    Walk,
    Run
}

public record MsgWalk : IMessage
{
    public uint Id { get; set; }
    public Direction Direction { get; set; }
    public WalkMode Mode { get; set; }
    public ushort Size => 12;
    public MessageType Type => MessageType.MsgWalk;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Id = ReadUInt32LittleEndian(buffer[4..]);
        Direction = (Direction)buffer[8];
        Mode = (WalkMode)buffer[9];
    }

    public void Write(Span<byte> buffer)
    {
        WriteUInt32LittleEndian(buffer[4..], Id);
        buffer[8] = (byte)Direction;
        buffer[9] = (byte)Mode;
    }

    public void Handle(Player player)
    {
        var direction = (Direction)((byte)Direction % 8);

        if (player.Pet is { } pet && pet.Id == Id)
        {
            pet.Step(direction, this);
        }
        else if (player.Id == Id)
        {
            player.StopHunt("Autohunt stopped because you moved.");
            player.Walk(direction, this);
        }
        else if (!World.IsPetId(Id))
        {
            player.Client.Close();
        }
    }
}