﻿namespace AdventOfCode;

public class Grid<T>
{
    private readonly Dictionary<Coords, T> _grid = [];

    public GridMember<T> this[int x, int y]
    {
        get { return _grid.TryGetValue(new Coords(x, y), out var t) ? new GridMember<T>(x, y, t) : new GridMember<T>(x, y); }
        set { _grid[new Coords(x, y)] = value.Value; }
    }

    public GridMember<T> this[Coords location]
    {
        get { return this[location.X, location.Y]; }
        set { this[location.X, location.Y] = value; }
    }

    public int Count => _grid.Count;

    public IEnumerable<(Coords location, T value)> AllMembers()
    {
        foreach (var t in _grid)
        {
            yield return (t.Key, t.Value);
        }
    }

    public IEnumerable<GridMember<T>> Neighbours(Coords location, bool includeDiagonal)
    {
        var neighbours = includeDiagonal ? location.Adjacents : location.Neighbors;
        foreach (var neighbour in neighbours)
        {
            var m = this[neighbour];
            if (m.Exists)
            {
                yield return m;
            }
        }
    }

    public (int min, int max) MinMaxX()
    {
        var xRange = _grid.Keys.Select(k => k.X);
        return (xRange.Min(), xRange.Max());
    }

    public (int min, int max) MinMaxY()
    {
        var yRange = _grid.Keys.Select(k => k.Y);
        return (yRange.Min(), yRange.Max());
    }

    public (Coords min, Coords max) CalculateBounds()
    {
        var xRange = MinMaxX();
        var yRange = MinMaxY();

        return (new Coords(xRange.min, yRange.min), new Coords(xRange.max, yRange.max));
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
