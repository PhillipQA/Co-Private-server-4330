namespace Conquer.Game.Models;

public partial class Player
{
    private const int JumpRange = 17;
    private const int MaxDislocation = 8;
    private const int WallRise = 210;
    private const int BrokenAltitude = 1000;

    public int LedTeamSize => Team?.Leader == this ? Team.Members.Count : 0;

    private Map? RevivalMap => World.Maps.GetValueOrDefault(Map?.Info.RevivalMapId ?? 0);

    public void Arrive(Map map, DateTimeOffset now)
    {
        RecalculateStats();
        map.Add(this, X, Y);
        Protect(now);
        SetPkPoints(PkPoints);
        Update(AttributeType.Stamina, Stamina);

        if (World.KoBoard.RankOf(Id) is > 0 and var rank)
        {
            Update(AttributeType.TimeAdd, rank);
        }
    }

    public bool Teleport(uint mapId, ushort x, ushort y, bool force = false)
    {
        if (Map is null || !force && Map.Info.Is(MapFlags.TeleportDisable) || !World.Maps.TryGetValue(mapId, out var destination))
        {
            return false;
        }

        Map.Transfer(this, destination, x, y);

        if (destination.Info.Is(MapFlags.TeamDisable))
        {
            Team?.Remove(this);
        }

        return true;
    }

    public bool Teleport(Map map, bool force = false) => Teleport(map.Id, map.PortalX, map.PortalY, force);

    public bool TeleportTeam(uint mapId, ushort x, ushort y)
    {
        if (LedTeamSize < 2)
        {
            return false;
        }

        foreach (var member in Team!.Members.Where(member => member != this && member.IsAlive && Sees(member)).ToList())
        {
            member.Teleport(mapId, x, y);
        }

        return Teleport(mapId, x, y);
    }

    public bool GoToRevivalPoint() => RevivalMap is { } revival && Teleport(revival);

    public void Record(uint mapId, ushort x, ushort y) => (RecordMapId, RecordX, RecordY) = (mapId, x, y);

    public bool Recall(bool force = false) => Teleport(RecordMapId, RecordX, RecordY, force);

    public void Depart()
    {
        if (IsAlive && Map is { } map && !map.Info.Is(MapFlags.RecordDisable))
        {
            Record(map.Id, X, Y);
        }
    }

    public void Resume()
    {
        if (!IsAlive || World.Maps.GetValueOrDefault(MapId)?.Info.Is(MapFlags.RecordDisable) != false)
        {
            (MapId, X, Y) = (RecordMapId, RecordX, RecordY);
            Health = Math.Max(Health, 1);
        }
    }

    public void KickBack() => Send(Act(ActionType.KickBack));

    public bool Walk(Direction direction, MsgWalk step)
    {
        Direction = direction;
        Halt();

        if (step.Mode == WalkMode.Run || _casting?.Spell is { Sort: MagicSort.Buffing, Status: MagicStatus.Intensify })
        {
            AbortCast();
        }

        Booth?.Close();
        EndProtection();

        var (dx, dy) = direction.Step();

        return Map is not null && Map.Move(this, (ushort)(X + dx), (ushort)(Y + dy), step);
    }

    public bool SyncPosition(ushort x, ushort y)
    {
        if ((x, y) == (0, 0) || (x, y) == (X, Y))
        {
            return true;
        }

        if (Map is null || DistanceTo(x, y) >= MaxDislocation)
        {
            return false;
        }

        if (!Map.IsWalkable(x, y))
        {
            return true;
        }

        Booth?.Close();
        Pose = 0;
        _mining = false;
        Map.Broadcast(this, new MsgAction
        {
            Id = Id,
            X = x,
            Y = y,
            DataX = x,
            DataY = y,
            Direction = (ushort)Direction,
            Action = ActionType.Synchro
        }, self: false);
        Map.Shift(this, x, y);
        return true;
    }

    public bool Jump(ushort x, ushort y, IMessage because)
    {
        if (Map is null)
        {
            return false;
        }

        if (!IsAlive || DistanceTo(x, y) > JumpRange || Vaults(x, y))
        {
            KickBack();
            return false;
        }

        if ((x, y) != (X, Y))
        {
            Direction = Directions.Toward(x - X, y - Y);
        }

        Interrupt();
        Booth?.Close();
        EndProtection();
        return Map.Move(this, x, y, because);
    }

    private bool Vaults(ushort x, ushort y)
    {
        if (!Map!.IsWalkable(x, y))
        {
            return false;
        }

        var rise = Map.Cell(x, y).Altitude - Map.Cell(X, Y).Altitude;

        return rise is > WallRise and < BrokenAltitude;
    }
}
