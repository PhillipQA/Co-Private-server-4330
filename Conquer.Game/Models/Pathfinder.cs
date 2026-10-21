namespace Conquer.Game.Models;

public static class Pathfinder
{
    private const int MaxSteps = 64;
    private const int MaxClimb = 25;

    public static List<Direction> Find(Map map, (int X, int Y) from, (int X, int Y) to, int escape = 0)
    {
        if (from == to)
        {
            return [];
        }

        var leaving = escape > 0;
        var open = new List<Node> { new(from.X, from.Y, Cost(from, to), null, default, 0) };
        var seen = new HashSet<(int, int)> { from };
        var (best, bestDistance) = ((Node?)null, Distance(from, to));

        for (var closed = 0; open.Count > 0 && closed < MaxSteps; closed++)
        {
            var index = Pick(open, leaving);
            var node = open[index];
            open.RemoveAt(index);

            if (node.Steps >= MaxSteps)
            {
                continue;
            }

            for (var direction = 0; direction < 8; direction++)
            {
                var step = ((Direction)direction).Step();
                var (x, y) = (node.X + step.X, node.Y + step.Y);

                if (!map.IsWalkable(x, y) || Math.Abs(map.Cell(node.X, node.Y).Altitude - map.Cell(x, y).Altitude) > MaxClimb || !seen.Add((x, y)))
                {
                    continue;
                }

                var next = new Node(x, y, Cost((x, y), to), node, (Direction)direction, node.Steps + 1);
                open.Add(next);
                var distance = Distance((x, y), to);

                if (leaving ? distance > bestDistance : distance < bestDistance)
                {
                    (best, bestDistance) = (next, distance);

                    if (leaving && distance >= escape * escape)
                    {
                        return Path(next);
                    }
                }

                if (!leaving && (x, y) == to)
                {
                    return Path(next);
                }
            }
        }

        return best is null ? [] : Path(best);
    }

    private static int Pick(List<Node> open, bool leaving)
    {
        var index = 0;

        for (var i = 1; i < open.Count; i++)
        {
            if (leaving ? open[i].Cost > open[index].Cost : open[i].Cost < open[index].Cost)
            {
                index = i;
            }
        }

        return index;
    }

    private static List<Direction> Path(Node node)
    {
        var path = new List<Direction>();

        for (var step = node; step.Parent is not null; step = step.Parent)
        {
            path.Add(step.Direction);
        }

        path.Reverse();
        return path;
    }

    private static int Cost((int X, int Y) at, (int X, int Y) to)
    {
        var (dx, dy) = (Math.Abs(to.X - at.X), Math.Abs(to.Y - at.Y));
        return 10 * Math.Max(dx, dy) + 6 * Math.Min(dx, dy);
    }

    private static int Distance((int X, int Y) a, (int X, int Y) b) => (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);

    private sealed record Node(int X, int Y, int Cost, Node? Parent, Direction Direction, int Steps);
}
