using System.Text;

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

    public static IEnumerable<string> SplitIntoLines(this TextReader input)
    {
        string? line;
        while ((line = input.ReadLine()) != null)
        {
            yield return line;
        }
    }

    public static IEnumerable<string> SplitIntoSections(this TextReader input)
    {
        string? line;
        StringBuilder section = new();
        while ((line = input.ReadLine()) != null)
        {
            if (line == string.Empty)
            {
                yield return section.ToString();
                section.Clear();
            }
            else
            {
                section.AppendLine(line);
            }
        }

        yield return section.ToString();
    }
}
