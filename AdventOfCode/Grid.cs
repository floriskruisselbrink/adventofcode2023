namespace AdventOfCode;

public class Grid<T>
{
    private readonly IDictionary<(int, int), T> _grid = new Dictionary<(int, int), T>();

    public GridMember<T> this[int x, int y]
    {
        get { return _grid.TryGetValue((x, y), out var t) ? new GridMember<T>(x, y, t) : new GridMember<T>(x, y); }
        set { _grid[(x, y)] = value.Value; }
    }

    public IEnumerable<(int x, int y, T value)> AllMembers()
    {
        foreach (var t in _grid)
        {
            yield return (t.Key.Item1, t.Key.Item2, t.Value);
        }
    }

    public IEnumerable<GridMember<T>> Neighbours((int x, int y) location, bool includeDiagonal)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 || (x * y != 0 && !includeDiagonal))
                {
                    continue;
                }

                var m = this[location.x + x, location.y + y];
                if (m.Exists)
                {
                    yield return m;
                }
            }
        }
    }

    public (int minX, int minY, int maxX, int maxY) CalculateBounds()
    {
        var xRange = _grid.Keys.Select(k => k.Item1);
        var yRange = _grid.Keys.Select(k => k.Item2);

        return (xRange.Min(), yRange.Min(), xRange.Max(), yRange.Max());
    }
}

public record GridMember<T>(int X, int Y)
{
    public GridMember(int X, int Y, T value) : this(X, Y) { _value = value; Exists = true; }

    public bool Exists { get; init; } = false;

    private readonly T? _value = default;

    public T Value
    {
        get => Exists ? _value! : throw new NotSupportedException();
    }

    public static implicit operator GridMember<T>(T t) => new(0, 0, t);
    public static implicit operator T(GridMember<T> g) => g.Value;
}
