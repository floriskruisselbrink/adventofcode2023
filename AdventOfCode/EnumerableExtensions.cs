using System.Numerics;

namespace AdventOfCode;

public static class EnumerableExtensions
{
    public static T Product<T>(this IEnumerable<T> list) where T : INumber<T>
    {
        return list.Aggregate(T.One, (a, b) => a * b);
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        => source.Select((item, index) => (item, index));
}
