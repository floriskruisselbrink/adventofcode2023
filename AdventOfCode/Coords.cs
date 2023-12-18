namespace AdventOfCode;

public enum Direction
{
    North,
    East,
    South,
    West
};

public static class DirectionExtensions
{
    public static bool IsVertical(this Direction direction) => (direction == Direction.North || direction == Direction.South);
    public static bool IsHorizontal(this Direction direction) => (direction == Direction.East || direction == Direction.West);
}

public readonly record struct Coords(int X, int Y)
{
    
    public static Coords operator +(Coords a, Coords b) => new(a.X + b.X, a.Y + b.Y);
    public static Coords operator +(Coords a, (int x, int y) b) => new(a.X + b.x, a.Y + b.y);
    public static Coords operator -(Coords a, Coords b) => new(a.X - b.X, a.Y - b.Y);
    public static Coords operator -(Coords a, (int x, int y) b) => new(a.X - b.x, a.Y - b.y);

    public static Coords operator +(Coords coord, Direction direction) => coord.Plus(direction, 1);
    public static Coords operator -(Coords coord, Direction direction) => coord.Plus(direction, -1);

    public static Coords OffsetLeft => new(-1, 0);
    public static Coords OffsetUp => new(0, -1);
    public static Coords OffsetRight => new(1, 0);
    public static Coords OffsetDown => new(0, 1);
    public static Coords OffsetUpLeft => new(-1, -1);
    public static Coords OffsetUpRight => new(1, -1);
    public static Coords OffsetDownLeft => new(-1, 1);
    public static Coords OffsetDownRight => new(1, 1);

    public Coords Left => this + OffsetLeft;
    public Coords Up => this + OffsetUp;
    public Coords Right => this + OffsetRight;
    public Coords Down => this + OffsetDown;
    public Coords UpLeft => this + OffsetUpLeft;
    public Coords UpRight => this + OffsetUpRight;
    public Coords DownLeft => this + OffsetDownLeft;
    public Coords DownRight => this + OffsetDownRight;
    /// <summary>Horizontal & Vertical</summary>
    public Coords[] Neighbors => new[] { Left, Up, Right, Down };
    /// <summary>Horizontal, Vertical & Diagonal</summary>
    public Coords[] Adjacents => new[] { Left, UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft };
    /// <summary>Manhattan Distance</summary>
    public int DistanceTo(Coords target) => Math.Abs(target.X - X) + Math.Abs(target.Y - Y);

    public Coords Plus(Direction direction, int amount = 1) => direction switch
    {
        Direction.North => new(X, Y - amount),
        Direction.East => new(X + amount, Y),
        Direction.South => new(X, Y + amount),
        Direction.West => new(X - amount, Y),
        _ => throw new ArgumentException($"Unknown direction ${direction}"),
    };

    public override string? ToString()
    {
        return $"({X},{Y})";
    }

    public override int GetHashCode()
    {
        return X * 49 + Y;
    }

    public bool Equals(Coords other)
    {
        return X == other.X && Y == other.Y;
    }
}
