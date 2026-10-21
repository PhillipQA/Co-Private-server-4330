namespace Conquer.Game.Messages;

public record MsgAllot : IMessage
{
    public byte Strength { get; set; }
    public byte Agility { get; set; }
    public byte Vitality { get; set; }
    public byte Spirit { get; set; }
    public ushort Size => 8;
    public MessageType Type => MessageType.MsgAllot;

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Strength = buffer[4];
        Agility = buffer[5];
        Vitality = buffer[6];
        Spirit = buffer[7];
    }

    public void Write(Span<byte> buffer)
    {
        buffer[4] = Strength;
        buffer[5] = Agility;
        buffer[6] = Vitality;
        buffer[7] = Spirit;
    }

    public void Handle(Player player)
    {
        var points = Strength + Agility + Vitality + Spirit;

        if (points > player.AttributePoints)
        {
            return;
        }

        player.Strength += Strength;
        player.Agility += Agility;
        player.Vitality += Vitality;
        player.Spirit += Spirit;
        player.AttributePoints -= (ushort)points;
        player.RecalculateStats();
    }
}