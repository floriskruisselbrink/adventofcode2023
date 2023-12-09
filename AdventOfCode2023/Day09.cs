namespace AdventOfCode2023;

public class Day09 : BaseDay
{
    private readonly IEnumerable<long[]> _input;

    public Day09()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        _input.Select(FindNextValue).Sum().ToString()
    );

    public override ValueTask<string> Solve_2() => new(
        _input.Select(FindPrevValue).Sum().ToString()
    );


    private long FindNextValue(long[] list)
    {
        var derivedList = new long[list.Length - 1];
        for (var i = 1; i < list.Length; i++)
        {
            derivedList[i-1] = list[i] - list[i - 1];
        }

        if (derivedList.All(x => x == 0))
        {
            return list.Last();
        }
        else
        {
            var nextValue = list.Last() + FindNextValue(derivedList);
            return nextValue;
        }
    }

    private long FindPrevValue(long[] list)
    {
        var derivedList = new long[list.Length - 1];
        for (var i = 1; i < list.Length; i++)
        {
            derivedList[i - 1] = list[i] - list[i - 1];
        }

        if (derivedList.All(x => x == 0))
        {
            return list.First();
        }
        else
        {
            var nextValue = list.First() - FindPrevValue(derivedList);
            return nextValue;
        }
    }

    private static IEnumerable<long[]> ParseInput()
    {
        return
            AocDownloader.GetInput(2023, 9)
            .SplitIntoLines()
            .Select(line => line.Split(' ').Select(long.Parse).ToArray())
            .ToList();
    }
}
