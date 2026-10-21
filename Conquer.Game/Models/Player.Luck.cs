namespace Conquer.Game.Models;

public partial class Player
{
    public const uint MaxLuckyTime = 2 * 60 * 60 * 1000;
    public const string LuckyGuy = "LuckyGuy";

    private const int PrayRange = 3;
    private const int PrayerRebirthLimit = 2;
    private const int LuckyStrikeOnMonsters = 3;
    private const int LuckyStrikeOnPlayers = 1;
    private const int LuckyEscapeChance = 2;
    private const int SpareChance = 10;
    private const int CasterRate = 3;
    private const int FlourishPerMille = 5;
    private const int FlourishRate = 10;
    private const int ChargePerMille = 5;
    private const int ChargeFloor = 50;
    private const int ChargeGap = 5;

    private static readonly TimeSpan PrayerIdle = TimeSpan.FromSeconds(3);

    private DateTimeOffset _lastTick;
    private (uint Map, ushort X, ushort Y) _prayedAt;
    private (uint Map, ushort X, ushort Y, DateTimeOffset Since) _standing;

    public uint LuckyTime { get; set; }
    public bool IsLucky => LuckyTime > 0;

    protected override bool LuckyStrike(Fighter target) => LuckyRoll(target is Monster ? LuckyStrikeOnMonsters : LuckyStrikeOnPlayers);

    protected override bool LuckyEscape() => LuckyRoll(LuckyEscapeChance);

    private bool LuckyRoll(int chance)
    {
        if (!IsLucky || World.Random.Next(100) >= chance)
        {
            return false;
        }

        ShowEffect(LuckyGuy);
        return true;
    }

    private int Flourish(int bonus)
    {
        if (!IsLucky || World.Random.Next(1000) >= FlourishPerMille)
        {
            return bonus;
        }

        Send(new MsgName { Id = Id, Action = NameAction.RoleEffect, Strings = [LuckyGuy] });
        Tell("You received 10x EXP from your attack, thanks to Lucky Time!", TalkChannel.System);
        return bonus * FlourishRate;
    }

    private void Charge(Fighter prey)
    {
        if (!IsLucky || Is(PlayerStatus.XpFull) || Xp < ChargeFloor || Level >= prey.Level + ChargeGap ||
            World.Random.Next(1000) >= ChargePerMille)
        {
            return;
        }

        SetXp(Full);
        Tell("Your XP power has been fully charged!", TalkChannel.System);
        ShowEffect(LuckyGuy);
    }

    private void Elapse(DateTimeOffset now)
    {
        var elapsed = _lastTick == default ? 0u : (uint)Math.Max(0, (now - _lastTick).TotalMilliseconds);
        _lastTick = now;

        ExperienceBoostTime -= Math.Min(elapsed, ExperienceBoostTime);
        Pray(elapsed, now);
    }

    private void Pray(uint elapsed, DateTimeOffset now)
    {
        var before = LuckyTime;

        if ((_standing.Map, _standing.X, _standing.Y) != (MapId, X, Y) || Target is not null)
        {
            _standing = (MapId, X, Y, now);
        }

        var still = _prayedAt == (MapId, X, Y) && Target is null && IsAlive;

        if (Is(PlayerStatus.CastingPray) && !still)
        {
            ClearEffect(PlayerStatus.CastingPray);
        }

        if (Is(PlayerStatus.Praying) && !(still && NearCaster()))
        {
            ClearEffect(PlayerStatus.Praying);
        }

        if (Is(PlayerStatus.CastingPray))
        {
            LuckyTime += elapsed * CasterRate;
        }
        else if (Is(PlayerStatus.Praying))
        {
            LuckyTime += elapsed;
        }
        else if (IsAlive && Rebirths < PrayerRebirthLimit && now - _standing.Since >= PrayerIdle && NearCaster())
        {
            ApplyEffect(PlayerStatus.Praying, 1, DateTimeOffset.MaxValue);
        }
        else
        {
            LuckyTime -= Math.Min(elapsed, LuckyTime);
        }

        LuckyTime = Math.Min(LuckyTime, MaxLuckyTime);

        if (LuckyTime / 1000 != before / 1000)
        {
            Update(AttributeType.LuckyTime, LuckyTime);
        }
    }

    protected override void OnEffectApplied(PlayerStatus status)
    {
        if (status is PlayerStatus.CastingPray or PlayerStatus.Praying)
        {
            _prayedAt = (MapId, X, Y);
        }

        if ((status & Frenzy) != 0)
        {
            _frenzySince = World.UtcNow;
        }
    }

    private bool NearCaster() =>
        Map?.InRange(this).OfType<Player>().Any(caster => caster.Is(PlayerStatus.CastingPray) && DistanceTo(caster) <= PrayRange) == true;
}
