using System.Numerics;

namespace AdventOfCode;

// end is inclusief!
public readonly record struct Range<T>(T Start, T End)
    where T : struct, INumber<T>
{
    public readonly bool IsEmpty => Start > End;
    public readonly bool Contains(T value) => value >= Start && value <= End;
    public readonly T Length => T.One + End - Start;

    public readonly IEnumerable<Range<T>> CutWith(Range<T> interval)
    {
        var before = new Range<T>(Min(Start, interval.Start), Min(End, interval.Start - T.One));
        var inside = new Range<T>(Max(Start, interval.Start), Min(End, interval.End));
        var after = new Range<T>(Max(Start, interval.End + T.One), Max(End, interval.End));
        return new List<Range<T>> { before, inside, after }.Where(r => !r.IsEmpty);
    }

    private static T Min(T a, T b) => a < b ? a : b;
    private static T Max(T a, T b) => a > b ? a : b;
}
