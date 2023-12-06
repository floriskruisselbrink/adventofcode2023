namespace AdventOfCode;

public static class StringExtensions
{
    public static string[] SplitIntoLines(this string input)
    {
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] SplitIntoSections(this string input)
    {
        return input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }
}
