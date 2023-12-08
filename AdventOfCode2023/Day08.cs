namespace AdventOfCode2023;

public class Day08 : BaseDay
{
    private readonly string _instructions;
    private readonly Dictionary<string, (string, string)> _map;

    public Day08()
    {
        (_instructions, _map) = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        FindSteps("AAA", node => node == "ZZZ").ToString()
    );

    public override ValueTask<string> Solve_2() => new(
        _map.Keys.Where(node => node.EndsWith('A'))
                 .Select(start => FindSteps(start, node => node.EndsWith('Z')))
                 .LeastCommonMultiple()
                 .ToString()
    );

    private long FindSteps(string from, Predicate<string> finished)
    {
        var current = from;
        var steps = 0L;
        var instructionIndex = 0;
        while (!finished(current))
        {
            current = NextNode(current, instructionIndex);
            steps++;
            instructionIndex++;
            if (instructionIndex >= _instructions.Length) instructionIndex = 0;
        }

        return steps;
    }
    private string NextNode(string current, int instructionIndex) => _instructions[instructionIndex] switch
    {
        'L' => _map[current].Item1,
        'R' => _map[current].Item2,
        _ => throw new ArgumentOutOfRangeException(nameof(instructionIndex), "Verwacht alleen L/R")
    };

    private static (string, Dictionary<string, (string, string)>) ParseInput()
    {
        var input = AocDownloader.GetInput(2023, 8).SplitIntoLines();
        var instructions = input.First();
        var maps = input.Skip(1).Select(line => (line[0..3], (line[7..10], line[12..15]))).ToDictionary();
        return (instructions, maps);
    }
}
