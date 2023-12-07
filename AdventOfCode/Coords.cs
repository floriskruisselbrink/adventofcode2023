namespace AdventOfCode;

public readonly record struct Coords(int X, int Y)
{
    public static Coords operator +(Coords a, Coords b) => new(a.X + b.X, a.Y + b.Y);
    public static Coords operator +(Coords a, (int x, int y) b) => new(a.X + b.x, a.Y + b.y);
    public static Coords operator -(Coords a, Coords b) => new(a.X - b.X, a.Y - b.Y);
    public static Coords operator -(Coords a, (int x, int y) b) => new(a.X - b.x, a.Y - b.y);

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
}
