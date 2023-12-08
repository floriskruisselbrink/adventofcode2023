namespace AdventOfCode2023;

public class Day06 : BaseDay
{
    private record struct Race(long Time, long Distance)
    {
        internal readonly long Attempt(long pressedTime)
        {
            return (Time - pressedTime) * pressedTime;
        }
    }

    private readonly Race[] _input1;
    private readonly Race _input2;

    public Day06()
    {
        (_input1, _input2) = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        _input1.Select(CountRecordBeatOptions).Product().ToString()
    );

    public override ValueTask<string> Solve_2() => new(
        CountRecordBeatOptions(_input2).ToString()
    );

    private long CountRecordBeatOptions(Race race)
    {
        long result = 0;
        for (var i = 0; i < race.Time; i++)
        {
            if (race.Attempt(i) > race.Distance)
            {
                result++;
            }
        }
        return result;
    }

    private (Race[] part1, Race part2) ParseInput()
    {
        var input = AocDownloader.GetInput(2023, 6).SplitIntoLines().ToArray();
        var times1 = input[0][10..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);
        var distances1 = input[1][10..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

        var time2 = long.Parse(input[0][10..].Replace(" ", ""));
        var distance2 = long.Parse(input[1][10..].Replace(" ", ""));

        return (times1.Zip(distances1, (t, d) => new Race(t, d)).ToArray(), new Race(time2, distance2));
    }
}
