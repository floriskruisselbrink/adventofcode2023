using System.Numerics;

namespace AdventOfCode;

// end is inclusief!
public readonly record struct Range<T>(T Start, T End)
    where T : struct, INumber<T>
{
    public T Length => T.One + End - Start;
}
