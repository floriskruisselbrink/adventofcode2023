namespace AdventOfCode2023;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = AocDownloader.GetInput(2023, 1).SplitIntoLines().ToArray();
    }

    public override ValueTask<string> Solve_1() => new(
        _input.Select(CalibrationValue)
              .Sum()
              .ToString()
        );

    public override ValueTask<string> Solve_2() => new(
        _input.Select(ReplaceNumbers)
              .Select(CalibrationValue)
              .Sum()
              .ToString()
        );

    private string ReplaceNumbers(string input)
    {
        return input.Replace("one", "o1e")
                    .Replace("two", "t2o")
                    .Replace("three", "t3e")
                    .Replace("four", "f4r")
                    .Replace("five", "f5e")
                    .Replace("six", "s6x")
                    .Replace("seven", "s7n")
                    .Replace("eight", "e8t")
                    .Replace("nine", "n9e");
    }

    private int CalibrationValue(string input) => int.Parse(input.FindFirstDigit() + input.FindLastDigit());
}

internal static class Day01Extensions
{
    internal static string FindFirstDigit(this string input) => input.First(char.IsDigit).ToString();
    internal static string FindLastDigit(this string input) => input.Last(char.IsDigit).ToString();
}
