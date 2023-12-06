namespace AdventOfCode;

public static class EnumerableExtensions
{
    public static long Product(this IEnumerable<long> list)
    {
        return list.Aggregate(1L, (a, b) => a * b);
    }
}
