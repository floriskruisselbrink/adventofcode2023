namespace AdventOfCode2023;

public class Day11 : BaseDay
{
    private readonly record struct Galaxy(long X, long Y)
    {
        public long DistanceTo(Galaxy other) => Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
    }

    private readonly List<Galaxy> _galaxies;

    public Day11()
    {
        _galaxies = ParseInput().ToList();
    }

    public override ValueTask<string> Solve_1() => new(Solve(2).ToString());

    public override ValueTask<string> Solve_2() => new(Solve(1000000).ToString()); // 593821824796 is too high

    private long Solve(long expansion)
    {
        var (emptyRows, emptyCols) = FindEmptyRowsCols();
        var expandedGalaxy = ExpandGalaxy(_galaxies, expansion, emptyRows, emptyCols);

        var allPairs = AllGalaxyPairs(expandedGalaxy.ToArray());
        var lengthSum = allPairs.Select(pair => pair.Item1.DistanceTo(pair.Item2)).Sum();

        return lengthSum;
    }
    private (IEnumerable<int> rows, IEnumerable<int> cols) FindEmptyRowsCols()
    {
        var maxX = Convert.ToInt32(_galaxies.Max(g => g.X));
        var maxY = Convert.ToInt32(_galaxies.Max(g => g.X));

        var occupiedCols = _galaxies.Select(g => g.X);
        var occupiedRows = _galaxies.Select(g => g.Y);

        var emptyRows = Enumerable.Range(0, maxY).Where(y => !occupiedRows.Contains(y));
        var emptyCols = Enumerable.Range(0, maxX).Where(x => !occupiedCols.Contains(x));
        return (emptyRows, emptyCols);
    }

    private List<Galaxy> ExpandGalaxy(List<Galaxy> input, long expansion, IEnumerable<int> emptyRows, IEnumerable<int> emptyCols)
    {
        return input.Select(galaxy =>
        {
            var newX = galaxy.X + (expansion-1) * emptyCols.Count(x => x < galaxy.X);
            var newY = galaxy.Y + (expansion-1) * emptyRows.Count(y => y < galaxy.Y);
            return new Galaxy(newX, newY);
        }).ToList();
    }

    private IEnumerable<(Galaxy, Galaxy)> AllGalaxyPairs(Galaxy[] galaxies)
    {
        for (int i = 0; i < galaxies.Length - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Length; j++)
            {
                yield return (galaxies[i], galaxies[j]);
            }
        }
    }

    private static IEnumerable<Galaxy> ParseInput()
    {
        var input = AocDownloader.GetInput(2023, 11).SplitIntoLines().ToArray();
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] == '#')
                {
                    yield return new Galaxy(x, y);
                }
            }
        }
    }
}
