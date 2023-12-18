namespace AdventOfCode2023;

public class Day18 : BaseDay
{
    private readonly string[] _input;

    public Day18()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var (trench, perimeter) = ParseInstructions1();
        return new(CountTotalArea(trench, perimeter).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var (trench, perimeter) = ParseInstructions2();
        return new(CountTotalArea(trench, perimeter).ToString());
    }

    private (List<Coords>, int) ParseInstructions1()
    {
        var trench = new List<Coords>();
        var location = new Coords(0, 0);
        var perimeter = 0;
        trench.Add(location);

        foreach (var instruction in _input.Select(line => line.Split(' ')))
        {
            var direction = instruction[0] switch
            {
                "R" => Direction.East,
                "D" => Direction.South,
                "L" => Direction.West,
                "U" => Direction.North,
                _ => throw new ArgumentException($"Unknown instruction {instruction[0]}")
            };
            var distance = int.Parse(instruction[1]);

            perimeter += distance;
            location = location.Plus(direction, distance);
            trench.Add(location);
        }

        return (trench, perimeter);
    }

    private (List<Coords>, int) ParseInstructions2()
    {
        var trench = new List<Coords>();
        var location = new Coords(0, 0);
        var perimeter = 0;
        trench.Add(location);

        foreach (var instruction in _input.Select(line => line.Split(' ')[2]))
        {
            var direction = instruction[^2] switch
            {
                '0' => Direction.East,
                '1' => Direction.South,
                '2' => Direction.West,
                '3' => Direction.North,
                _ => throw new ArgumentException($"Unknown instruction {instruction[0]}")
            };
            var distance = int.Parse(instruction[2..7], System.Globalization.NumberStyles.HexNumber);

            perimeter += distance;
            location = location.Plus(direction, distance);
            trench.Add(location);
        }

        return (trench, perimeter);
    }

    private static long CountTotalArea(List<Coords> trench, int perimeter)
        => CountInsideArea(trench) + perimeter / 2 + 1;

    private static long CountInsideArea(List<Coords> trench)
    {
        return Math.Abs(trench.Zip(trench.Skip(1), (a, b) =>
            1L * a.X * b.Y - 1L * b.X * a.Y
        ).Sum()) / 2;
    }

    private static readonly string s_TestInput = """
        R 6 (#70c710)
        D 5 (#0dc571)
        L 2 (#5713f0)
        D 2 (#d2c081)
        R 2 (#59c680)
        D 2 (#411b91)
        L 5 (#8ceee2)
        U 2 (#caa173)
        L 1 (#1b58a2)
        U 2 (#caa171)
        R 2 (#7807d2)
        U 3 (#a77fa3)
        L 2 (#015232)
        U 2 (#7a21e3)
        """;

    private static string[] ParseInput() => 
            AocDownloader.GetInput(2023, 18)
            //s_TestInput
            .SplitIntoLines()
            .ToArray();
}
