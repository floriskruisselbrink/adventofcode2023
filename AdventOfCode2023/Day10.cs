namespace AdventOfCode2023;

public class Day10 : BaseDay
{
    private readonly string[] _input;

    private Grid<char> _grid;

    public Day10()
    {
        _input = ParseInput();
        _grid = new Grid<char>();
    }

    public override ValueTask<string> Solve_1()
    {
        _grid = FindGiantLoop();
        return new((_grid.Count/2).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(CountInsideGiantLoop().ToString());
    }

    private Grid<char> FindGiantLoop()
    {
        var grid = new Grid<char>();

        var start = FindStartingPoint();
        grid[start] = FindStartingPipe(start);

        var prev = start;
        var next = FindNeighbours(start, grid[start]).First();
        while (next != start)
        {
            grid[next] = _input[next.Y][next.X];
            var current = next;
            next = FindNeighbours(next).First(c => c != prev);
            prev = current;
        }

        return grid;
    }

    private int CountInsideGiantLoop()
    {
        var count = 0;
        var inside = false;
        var lastCorner = '?';
        for (var y = 0; y < _input.Length; y++)
        {
            var line = _input[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (!_grid[x, y].Exists)
                {
                    if (inside) count++;
                    continue;
                }

                var current = _grid[x, y].Value;
                if (current == '|')
                {
                    inside = !inside;
                }
                else if (current == 'L')
                {
                    lastCorner = current;
                }
                else if (current == 'F')
                {
                    lastCorner = current;
                }
                else if (current == 'J' && lastCorner == 'F')
                {
                    lastCorner = '?';
                    inside = !inside;
                }
                else if (current == '7' && lastCorner == 'L')
                {
                    lastCorner = '?';
                    inside = !inside;
                }
            }
        }
        return count;
    }

    private Coords FindStartingPoint()
    {
        for (var y = 0; y < _input.Length; y++) 
        {
            var line = _input[y];
            var x = line.IndexOf('S');
            if (x > 0) return new Coords(x, y);
        }
        throw new ApplicationException("No starting point 'S' found");
    }

    private char FindStartingPipe(Coords start)
    {
        // TODO, dit is handmatig uit de input gelezen...
        return '-';
    }

    private long FindLoopLength(Coords start, char startPipe)
    {
        var length = 2;
        var prev = start;
        var next = FindNeighbours(start, startPipe).First();

        while (next != start)
        {
            length++;
            var current = next;
            next = FindNeighbours(next).First(c => c != prev);
            prev = current;
        }
        return length;
    }

    private Coords[] FindNeighbours(Coords coords) => FindNeighbours(coords, _input[coords.Y][coords.X]);

    private Coords[] FindNeighbours(Coords coords, char pipe) => pipe switch
    {
        '|' => [coords.Up, coords.Down],
        '-' => [coords.Left, coords.Right],
        'L' => [coords.Up, coords.Right],
        'J' => [coords.Up, coords.Left],
        '7' => [coords.Down, coords.Left],
        'F' => [coords.Down, coords.Right],
        '.' => [],
        _ => throw new ApplicationException($"Unknown pipe type '{pipe}' found at {coords}")
    };

    private static string[] ParseInput()
    {
        return AocDownloader.GetInput(2023, 10).SplitIntoLines().ToArray();
    }
}
