namespace Conquer.Game.Models;

public sealed class DiceTable(Npc npc)
{
    public const byte TableType = 19;

    private const int Kinds = 22;
    private const byte Small = 0;
    private const byte Big = 1;
    private const byte FirstTriple = 2;
    private const byte FirstTotal = 8;
    private const int SmallestBig = 11;
    private const uint MaxStake = 30_000;
    private const uint BigWin = 2 * MaxStake;

    private static readonly uint[] Chips = [100, 1_000, 10_000];
    private static readonly uint[] Odds = [2, 2, 216, 216, 216, 216, 216, 216, 72, 36, 21, 14, 10, 8, 8, 8, 8, 10, 14, 21, 36, 72];
    private static readonly TimeSpan Betting = TimeSpan.FromSeconds(50);
    private static readonly TimeSpan Rolling = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan Showing = TimeSpan.FromSeconds(8);

    private readonly List<Player> _players = [];
    private readonly Dictionary<Player, uint[]> _stakes = [];
    private Phase _phase;
    private DateTimeOffset _next;

    private enum Phase
    {
        Idle,
        Betting,
        Rolling,
        Showing
    }

    public void Join(Player player, DateTimeOffset now)
    {
        if (_players.Contains(player) || !player.CanSee(npc))
        {
            return;
        }

        player.Send(Named(NameAction.AddDicePlayer, player.Name));

        if (_phase == Phase.Idle)
        {
            _players.Add(player);
            Open(now);
            return;
        }

        player.Send(_phase == Phase.Betting ? Countdown(_next - now) : new MsgDice { Action = DiceAction.EndChip, NpcId = npc.Id });

        foreach (var other in _players)
        {
            player.Send(Named(NameAction.AddDicePlayer, other.Name));

            if (_stakes.TryGetValue(other, out var stakes))
            {
                player.Send(Bets(DiceAction.ChipIn, stakes));
            }
        }

        _players.Add(player);
    }

    public void Leave(Player player)
    {
        if (!_players.Remove(player))
        {
            return;
        }

        player.Send(Named(NameAction.RemoveDicePlayer, player.Name));

        if (_phase == Phase.Betting && Refund(player) is { } refunded)
        {
            Send(Bets(DiceAction.CancelChip, refunded));
        }

        Send(Named(NameAction.RemoveDicePlayer, player.Name));
    }

    public void PlaceBet(Player player, byte type, uint amount)
    {
        var stakes = _stakes.GetValueOrDefault(player) ?? new uint[Kinds];

        if (_phase != Phase.Betting || !_players.Contains(player) || !player.CanSee(npc) || type >= Kinds || !Chips.Contains(amount) ||
            Total(stakes) + amount > MaxStake || !player.Spend(amount))
        {
            return;
        }

        stakes[type] += amount;
        _stakes[player] = stakes;
        player.Send(new MsgDice { Action = DiceAction.ChipInConfirm, NpcId = npc.Id, Bets = [(type, amount)] });
        Send(new MsgDice { Action = DiceAction.ChipIn, NpcId = npc.Id, Bets = [(type, amount)] }, player);
    }

    public void CancelBets(Player player)
    {
        if (_phase == Phase.Betting && Refund(player) is { } refunded)
        {
            player.Send(Bets(DiceAction.CancelChipConfirm, refunded));
            Send(Bets(DiceAction.CancelChip, refunded), player);
        }
    }

    public void Tick(DateTimeOffset now)
    {
        foreach (var gone in _players.Where(player => player.Map != npc.Map).ToList())
        {
            if (gone.Map is null)
            {
                _players.Remove(gone);
                _stakes.Remove(gone);
            }
            else
            {
                Leave(gone);
            }
        }

        if (_phase == Phase.Idle || now < _next)
        {
            return;
        }

        switch (_phase)
        {
            case Phase.Betting:
                Send(new MsgDice { Action = DiceAction.EndChip, NpcId = npc.Id });
                (_phase, _next) = (Phase.Rolling, now + Rolling);
                break;

            case Phase.Rolling:
                Roll();
                (_phase, _next) = (Phase.Showing, now + Showing);
                break;

            case Phase.Showing when _players.Count > 0:
                Open(now);
                break;

            case Phase.Showing:
                _phase = Phase.Idle;
                break;
        }
    }

    private void Open(DateTimeOffset now)
    {
        _stakes.Clear();
        (_phase, _next) = (Phase.Betting, now + Betting);
        Send(Countdown(Betting));
    }

    private void Roll()
    {
        var random = npc.Map!.World.Random;
        var dice = (random.Next(1, 7), random.Next(1, 7), random.Next(1, 7));
        npc.Map.Broadcast(npc, MsgDice.Rolled(npc.Id, dice));

        foreach (var (player, stakes) in _stakes)
        {
            foreach (var (type, stake) in stakes.Index().Where(bet => bet.Item > 0 && Wins(bet.Index, dice)))
            {
                var winnings = stake * Odds[type];
                player.Earn(winnings);
                Send(new MsgName { Id = winnings, Action = NameAction.DiceBonus, Strings = [player.Name] });

                if (winnings > BigWin)
                {
                    npc.Map.Broadcast(npc, MsgTalk.System($"Beaming with joy, {player.Name} has won {winnings} silver at dice!"));
                }
            }
        }

        _stakes.Clear();
    }

    private static bool Wins(int type, (int, int, int) dice)
    {
        var (first, second, third) = dice;
        var total = first + second + third;

        return type switch
        {
            Small => total < SmallestBig,
            Big => total >= SmallestBig,
            < FirstTotal => first == second && second == third && first == type - FirstTriple + 1,
            _ => total == type - FirstTotal + 4
        };
    }

    private uint[]? Refund(Player player)
    {
        if (!_stakes.Remove(player, out var stakes))
        {
            return null;
        }

        player.Earn(Total(stakes));
        return stakes;
    }

    private static uint Total(uint[] stakes) => stakes.Aggregate(0u, (sum, stake) => sum + stake);

    private MsgDice Countdown(TimeSpan left) =>
        new() { Action = DiceAction.BeginChip, NpcId = npc.Id, Countdown = (byte)Math.Ceiling(left.TotalSeconds) };

    private MsgDice Bets(DiceAction action, uint[] stakes) => new()
    {
        Action = action,
        NpcId = npc.Id,
        Bets = [.. stakes.Index().Where(bet => bet.Item > 0).Select(bet => ((byte)bet.Index, bet.Item))]
    };

    private MsgName Named(NameAction action, string name) => new() { Id = npc.Id, Action = action, Strings = [name] };

    private void Send(IMessage message, Player? except = null) => _players.Where(player => player != except).Send(message);
}
