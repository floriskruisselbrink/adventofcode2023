using System.Text;

namespace AdventOfCode2023;

public class Day14 : BaseDay
{
    private readonly RockPlatform _input;

    public Day14()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var platform = new RockPlatform(_input);
        platform.TiltNorth();

        return new(platform.CalculateTotalLoad().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var platform = new RockPlatform(_input);

        var seenPlatforms = new Dictionary<string, (int cycle, int totalLoad)>();
        var cycleStart = -1;
        var cycleEnd = -1;
        var cycleLength = -1;
        var targetCycle = 1_000_000_000;
        for (var currentCycle = 1; currentCycle <= targetCycle; currentCycle++)
        {
            platform.TiltNorth();
            platform.TiltWest();
            platform.TiltSouth();
            platform.TiltEast();

            var map = platform.ToString();
            if (seenPlatforms.TryGetValue(map, out var seen))
            {
                if (cycleEnd == -1)
                {
                    cycleStart = seen.cycle;
                    cycleEnd = currentCycle;
                }
                else if (seen.cycle == cycleStart)
                {
                    cycleLength = currentCycle - cycleStart;
                    break;
                }
            }
            else
            {
                seenPlatforms[map] = (currentCycle, platform.CalculateTotalLoad());
            }
        }

        targetCycle = cycleStart + ((targetCycle - cycleEnd) % cycleLength);
        var load = seenPlatforms.First(x => x.Value.cycle == targetCycle).Value.totalLoad;
        return new(load.ToString());
    }

    private static RockPlatform ParseInput() => new RockPlatform(
        AocDownloader.GetInput(2023, 14).SplitIntoLines().ToList()
    );

    private class RockPlatform
    {
        private readonly HashSet<Coords> _squareRocks;
        private readonly HashSet<Coords> _roundRocks;

        public int Height { get; private set; }
        public int Width { get; private set; }

        public RockPlatform(RockPlatform other)
        {
            _squareRocks = new HashSet<Coords>(other._squareRocks);
            _roundRocks = new HashSet<Coords>(other._roundRocks);
            Height = other.Height;
            Width = other.Width;
        }

        public RockPlatform(IList<string> input)
        {
            _squareRocks = [];
            _roundRocks = [];

            foreach (var (line, y) in input.WithIndex())
            {
                foreach (var (ch, x) in line.WithIndex())
                {
                    switch (ch)
                    {
                        case '#':
                            _squareRocks.Add(new Coords(x, y));
                            break;
                        case 'O':
                            _roundRocks.Add(new Coords(x, y));
                            break;
                    }
                }
            }

            Height = input.Count;
            Width = input.First().Length;
        }

        public int CalculateTotalLoad() 
            => _roundRocks.Select(rock => Height - rock.Y).Sum();

        public void TiltNorth() => Tilt(
            rocks: _roundRocks.OrderBy(r => r.Y),
            getNextPosition: pos => pos.Up,
            endPositionReached: pos => pos.Y == 0
        );

        public void TiltWest() => Tilt(
            rocks: _roundRocks.OrderBy(r => r.X),
            getNextPosition: pos => pos.Left,
            endPositionReached: pos => pos.X == 0
        );

        public void TiltSouth() => Tilt(
            rocks: _roundRocks.OrderByDescending(r => r.Y),
            getNextPosition: pos => pos.Down,
            endPositionReached: pos => pos.Y == (Height - 1)
        );

        public void TiltEast() => Tilt(
            rocks: _roundRocks.OrderByDescending(r => r.X),
            getNextPosition: pos => pos.Right,
            endPositionReached: pos => pos.X == (Width - 1)
        );

        private void Tilt(IEnumerable<Coords> rocks, Func<Coords, Coords> getNextPosition, Func<Coords, bool> endPositionReached)
        {
            foreach (var rock in rocks)
            {
                var nextPosition = getNextPosition(rock);

                if (!endPositionReached(rock) && !_roundRocks.Contains(nextPosition) && !_squareRocks.Contains(nextPosition))
                {
                    _roundRocks.Remove(rock);

                    Coords thisPosition;
                    do
                    {
                        thisPosition = nextPosition;
                        nextPosition = getNextPosition(thisPosition);
                    }
                    while (!endPositionReached(thisPosition) && !_roundRocks.Contains(nextPosition) && !_squareRocks.Contains(nextPosition));

                    _roundRocks.Add(thisPosition);
                }
            }
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x <= Width; x++)
                {
                    if (_roundRocks.Contains(new Coords(x, y)))
                    {
                        sb.Append('O');
                    }
                    else if (_squareRocks.Contains(new Coords(x, y)))
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
