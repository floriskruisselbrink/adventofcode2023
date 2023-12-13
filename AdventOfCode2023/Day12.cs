namespace AdventOfCode2023
{
    public class Day12 : BaseDay
    {
        private readonly IList<SpringSet> _input;

        public Day12()
        {
            _input = ParseInput();
        }

        public override ValueTask<string> Solve_1() => new(
            _input.Select(s => s.Solve()).Sum().ToString()
        );

        public override ValueTask<string> Solve_2() => new(
            _input.Select((s, i) => s.UnFold().Solve()).Sum().ToString()
        );

        ~Day12()
        {
            SpringSet.ClearCache();
        }

        private const string TestInput = """
            ???.### 1,1,3
            .??..??...?##. 1,1,3
            ?#?#?#?#?#?#?#? 1,3,1,6
            ????.#...#... 4,1,1
            ????.######..#####. 1,6,5
            ?###???????? 3,2,1
            """;

        private IList<SpringSet> ParseInput() =>
            AocDownloader.GetInput(2023, 12).SplitIntoLines()
            //TestInput.SplitIntoLines()
                .Select(line => line.Split(' '))
                .Select(line => new SpringSet(line[0], line[1].Split(',').Select(int.Parse).ToArray()))
                .ToList();

        private record class SpringSet(string Layout, int[] Groups)
        {
            private static Dictionary<SpringSet, long> s_cache = [];

            public static void ClearCache() => s_cache.Clear();

            public SpringSet UnFold() => new(
                string.Join('?', Enumerable.Repeat(Layout, 5)), 
                Enumerable.Repeat(Groups, 5).SelectMany(g => g).ToArray()
            );

            public long Solve()
            {
                if(s_cache.TryGetValue(this, out var cachedResult))
                {
                    return cachedResult;
                }

                var result = 0L;

                if (Groups.Length == 0)
                {
                    result = Layout.Any(s => s == '#') ? 0 : 1;
                    s_cache[this] = result;
                    return result;
                }
                
                if (Layout.Length == 0)
                {
                    s_cache[this] = 0;
                    return 0;
                }

                result = SolveMultipleGroups();
                s_cache[this] = result;
                return result;
            }

            private long SolveMultipleGroups()
            {
                var nextCharacter = Layout.First();
                var nextGroup = Groups.First();

                long Dot()
                {
                    return new SpringSet(Layout[1..], Groups).Solve();
                }

                long Pound()
                {
                    if (Layout.Length < nextGroup)
                    {
                        return 0;
                    }

                    var groupPattern = Layout[..nextGroup].Replace('?', '#');
                    if (groupPattern.Any(s => s != '#') || groupPattern.Length != nextGroup)
                    {
                        return 0;
                    }

                    if (Layout.Length == nextGroup)
                    {
                        return Groups.Length == 1 ? 1 : 0;
                    }

                    if (Layout[nextGroup] == '.' || Layout[nextGroup] == '?')
                    {
                        return new SpringSet(Layout[(nextGroup + 1)..], Groups[1..]).Solve();
                    }

                    return 0;
                }

                return nextCharacter switch
                {
                    '.' => Dot(),
                    '#' => Pound(),
                    '?' => Dot() + Pound(),
                    _ => throw new NotSupportedException()
                };
            }

            public override int GetHashCode()
            {
                var hash = new HashCode();
                hash.Add(Layout);
                foreach (var group in Groups)
                {
                    hash.Add(group);
                }
                return hash.ToHashCode();
            }

            public virtual bool Equals(SpringSet? other)
            {
                if (other == null) return false;
                return Layout == other.Layout && Groups.SequenceEqual(other.Groups);
            }
        }
    }
}
