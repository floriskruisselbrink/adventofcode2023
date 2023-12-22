using System.Numerics;

namespace AdventOfCode;

public static class EnumerableExtensions
{
    public static T Product<T>(this IEnumerable<T> source) where T : INumber<T>
    {
        return source.Aggregate(T.One, (a, b) => a * b);
    }

    public static TResult Product<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) 
        where TResult : INumber<TResult>
    {
        return source.Select(selector).Product();
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        => source.Select((item, index) => (item, index));
}
