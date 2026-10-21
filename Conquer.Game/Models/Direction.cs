namespace Conquer.Game.Models;

public enum Direction
{
    South,
    SouthWest,
    West,
    NorthWest,
    North,
    NorthEast,
    East,
    SouthEast
}

public static class Directions
{
    private static readonly (int X, int Y)[] Steps = [(0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1)];

    public static (int X, int Y) Step(this Direction direction) => Steps[(int)direction % Steps.Length];

    public static Direction Toward(int dx, int dy) => (Direction)Array.IndexOf(Steps, (Math.Sign(dx), Math.Sign(dy)));
}
