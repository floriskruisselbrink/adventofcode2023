namespace AdventOfCode2023;

public class Day16 : BaseDay
{
    private readonly string[] _input;

    public Day16()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        new Solver(_input).Solve(new Beam(new Coords(0, 0), Direction.East)).ToString()
    );

    public override ValueTask<string> Solve_2()
    {
        var solver = new Solver(_input);
        var bestResult = 0L;

        var maxStackSize = 1024 * 1024 * 64;
        Thread thread = new Thread(() => {
            for (var y = 0; y < solver.MaxY; y++)
            {
                bestResult = Math.Max(bestResult, solver.Solve(new Beam(new Coords(0, y), Direction.East)));
                bestResult = Math.Max(bestResult, solver.Solve(new Beam(new Coords(solver.MaxX - 1, y), Direction.West)));
            }
            for (var x = 0; x < solver.MaxX; x++)
            {
                bestResult = Math.Max(bestResult, solver.Solve(new Beam(new Coords(x, 0), Direction.South)));
                bestResult = Math.Max(bestResult, solver.Solve(new Beam(new Coords(x, solver.MaxY - 1), Direction.North)));
            }
        }, maxStackSize);
        thread.Start();
        thread.Join();

        return new(bestResult.ToString());
    }

    private static readonly string s_TestInput = """
        .|...\....
        |.-.\.....
        .....|-...
        ........|.
        ..........
        .........\
        ..../.\\..
        .-.-/..|..
        .|....-|.\
        ..//.|....
        """;

    private static string[] ParseInput()
    {
        return 
            AocDownloader.GetInput(2023, 16)
            //s_TestInput
            .SplitIntoLines().ToArray();
    }

    private record class Beam(Coords Location, Direction Direction)
    {
        public Beam Next() => new(Location + Direction, Direction);
        public Beam Next(Direction newDirection) => new(Location + newDirection, newDirection);

        public Beam Rotate(char mirror) => (mirror, Direction) switch
        {
            ('/', Direction.East) => Next(Direction.North),
            ('/', Direction.West) => Next(Direction.South),
            ('/', Direction.North) => Next(Direction.East),
            ('/', Direction.South) => Next(Direction.West),
            ('\\', Direction.East) => Next(Direction.South),
            ('\\', Direction.West) => Next(Direction.North),
            ('\\', Direction.North) => Next(Direction.West),
            ('\\', Direction.South) => Next(Direction.East),
            _ => throw new ArgumentException($"Unknown mirror {mirror} or direction {Direction}")
        };
    }

    private class Solver
    {
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        private readonly string[] _grid;

        public Solver(string[] grid) 
        { 
            _grid = grid;
            MaxX = _grid[0].Length;
            MaxY = _grid.Length;
        }

        public int Solve(Beam startingBeam)
        {
            var cache = new HashSet<Beam>();

            TraceBeam(startingBeam, cache);

            return cache.GroupBy(b => b.Location).Count();
        }

        private void TraceBeam(Beam beam, HashSet<Beam> cache)
        {
            if (!InRange(beam.Location) || !cache.Add(beam))
            {
                return;
            }

            var tile = _grid[beam.Location.Y][beam.Location.X];
            switch (tile)
            {
                case '.':
                    TraceBeam(beam.Next(), cache);
                    break;

                case '-':
                    if (beam.Direction.IsVertical())
                    {
                        TraceBeam(beam.Next(Direction.East), cache);
                        TraceBeam(beam.Next(Direction.West), cache);
                    }
                    else
                    {
                        TraceBeam(beam.Next(), cache);
                    }
                    break;

                case '|':
                    if (beam.Direction.IsHorizontal())
                    {
                        TraceBeam(beam.Next(Direction.North), cache);
                        TraceBeam(beam.Next(Direction.South), cache);
                    }
                    else
                    {
                        TraceBeam(beam.Next(), cache);
                    }
                    break;

                case '/':
                case '\\':
                    TraceBeam(beam.Rotate(tile), cache);
                    break;

                default:
                    throw new ArgumentException($"Unknown tile {tile} found at {beam.Location}");
            }
        }

        private bool InRange(Coords location) => (location.X >= 0 && location.Y >= 0 && location.X < MaxX && location.Y < MaxY);
    }
}
