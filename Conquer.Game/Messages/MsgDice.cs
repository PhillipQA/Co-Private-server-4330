namespace Conquer.Game.Messages;

public enum DiceAction : byte
{
    ChipIn = 0,
    ChipInConfirm = 1,
    CancelChip = 2,
    CancelChipConfirm = 3,
    BeginChip = 4,
    EndChip = 5,
    Dice = 6
}

public record MsgDice : IMessage
{
    public DiceAction Action { get; set; }
    public byte Countdown { get; set; }
    public uint NpcId { get; set; }
    public List<(byte Type, uint Amount)> Bets { get; set; } = [];
    public ushort Size => (ushort)(12 + Bets.Count * 8);
    public MessageType Type => MessageType.MsgDice;

    public static MsgDice Rolled(uint npcId, (int, int, int) dice) => new()
    {
        Action = DiceAction.Dice,
        NpcId = npcId,
        Bets = [(0, (uint)(dice.Item1 | dice.Item2 << 8 | dice.Item3 << 16))]
    };

    public void Read(ReadOnlySpan<byte> buffer)
    {
        Action = (DiceAction)buffer[4];
        NpcId = ReadUInt32LittleEndian(buffer[8..]);

        if (Action == DiceAction.BeginChip)
        {
            Countdown = buffer[5];
            return;
        }

        for (var i = 0; i < buffer[5]; i++)
        {
            Bets.Add((buffer[12 + i * 8], ReadUInt32LittleEndian(buffer[(16 + i * 8)..])));
        }
    }

    public void Write(Span<byte> buffer)
    {
        buffer[4] = (byte)Action;
        buffer[5] = Action == DiceAction.BeginChip ? Countdown : (byte)Bets.Count;
        WriteUInt32LittleEndian(buffer[8..], NpcId);

        foreach (var (i, (type, amount)) in Bets.Index())
        {
            buffer[12 + i * 8] = type;
            WriteUInt32LittleEndian(buffer[(16 + i * 8)..], amount);
        }
    }

    public void Handle(Player player, Map map)
    {
        if (map.Find<Npc>(NpcId)?.Dice is not { } table || Bets is not [var (type, amount)])
        {
            return;
        }

        switch (Action)
        {
            case DiceAction.ChipIn:
                table.PlaceBet(player, type, amount);
                break;

            case DiceAction.CancelChip:
                table.CancelBets(player);
                break;
        }
    }
}
