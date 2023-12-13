namespace AdventOfCode2023;

public class Day13 : BaseDay
{
    private readonly List<MirrorValley> _input;

    public Day13()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        _input.Select(p => p.CalculateMirror()).Sum().ToString()
    );

    public override ValueTask<string> Solve_2() => new(
        _input.Select(p => p.CalculateSmudgedMirror()).Sum().ToString()
    );

    private class MirrorValley
    {
        private readonly Pattern _horizontal;
        private readonly Pattern _vertical;

        public MirrorValley(string input)
        {
            var lines = input.SplitIntoLines();
            _vertical = new Pattern(lines);
            _horizontal = new Pattern(RotatePattern(lines));
        }

        private MirrorValley(Pattern horizontal, Pattern vertical)
        {
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public long CalculateMirror() => FindMirrors().Single();

        public long CalculateSmudgedMirror()
        {
            var originalMirror = CalculateMirror();

            foreach (var variation in AllVariations())
            {
                var variationMirrors = variation.FindMirrors().Where(m => m != originalMirror).ToList();
                if (variationMirrors.Count == 1)
                {
                    return variationMirrors.Single();
                }
            }
            return 0;
        }

        private IEnumerable<int> FindMirrors()
        {
            var mirrors = new List<int>();
            mirrors.AddRange(_vertical.FindReflections().Select(m => 100 * m));
            mirrors.AddRange(_horizontal.FindReflections());
            return mirrors.Where(m => m > 0);
        }

        private IEnumerable<MirrorValley> AllVariations()
        {
            for (var x = 0; x < _vertical.Width; x++)
            {
                for (var y = 0; y < _vertical.Height; y++)
                {
                    yield return new MirrorValley(_horizontal.WithSmudge(y, x), _vertical.WithSmudge(x, y));
                }
            }
        }

        private static string[] RotatePattern(string[] lines)
        {
            string[] result = new string[lines[0].Length];

            for (var x = 0; x < lines[0].Length; x++)
            {
                result[x] = new string(lines.Select(line => line[x]).ToArray());
            }
            return result;
        }
    }

    private class Pattern
    {
        private readonly string[] _pattern;

        public int Width => _pattern[0].Length;
        public int Height => _pattern.Length;

        public Pattern(string[] pattern)
        {
            _pattern = pattern;
        }

        public Pattern WithSmudge(int x, int y)
        {
            var fixedLine = _pattern[y].ToCharArray();
            fixedLine[x] = (fixedLine[x] == '.') ? '#' : '.';

            return new Pattern(
                _pattern.Select((line, i) => (y == i) ? new string(fixedLine) : line)
                        .ToArray()
            );
        }

        public IEnumerable<int> FindReflections()
        {
            for (var mirror = 1; mirror < _pattern.Length; mirror++)
            {
                if (ReflectsAround(mirror))
                {
                    yield return mirror;
                }
            }
        }

        private bool ReflectsAround(int mirror)
        {
            var above = mirror - 1;
            var below = mirror;
            while (above >= 0 && below < _pattern.Length)
            {
                if (_pattern[above] != _pattern[below])
                {
                    return false;
                }

                above--;
                below++;
            }

            return true;
        }

        public override string? ToString()
        {
            return string.Join(Environment.NewLine, _pattern);
        }
    }

    private static readonly string s_testInput = """
        #.##..##.
        ..#.##.#.
        ##......#
        ##......#
        ..#.##.#.
        ..##..##.
        #.#.##.#.

        #...##..#
        #....#..#
        ..##..###
        #####.##.
        #####.##.
        ..##..###
        #....#..#
        """;

    private static List<MirrorValley> ParseInput() => 
        AocDownloader.GetInput(2023, 13)
        //s_testInput
            .SplitIntoSections()
            .Select(pattern => new MirrorValley(pattern))
            .ToList();
}
