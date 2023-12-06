namespace AdventOfCode2023
{
    public class Day05 : BaseDay
    {
        record struct LongRange(long Start, long End)
        {
            internal readonly bool IsEmpty => Start > End;
            internal readonly bool Contains(long value) => value >= Start && value <= End;
            
            internal readonly IEnumerable<LongRange> CutWith(LongRange interval)
            {
                var before = new LongRange(Math.Min(Start, interval.Start), Math.Min(End, interval.Start - 1));
                var inside = new LongRange(Math.Max(Start, interval.Start), Math.Min(End, interval.End));
                var after = new LongRange(Math.Max(Start, interval.End + 1), Math.Max(End, interval.End));
                var result = new List<LongRange> { before, inside, after }.Where(r => !r.IsEmpty);
                return result;
            }
        }

        record Converter(LongRange Source, long Delta)
        {
            internal Converter(long[] numbers) : this(new LongRange(numbers[1], numbers[1] + numbers[2] - 1), numbers[0] - numbers[1]) { }
        }

        record Mapping(string Title, Converter[] Converters)
        {
            internal long Apply(long seed) => seed + (Converters.FirstOrDefault(c => c.Source.Contains(seed))?.Delta ?? 0);

            internal LongRange Apply(LongRange seedRange) => new(Apply(seedRange.Start), Apply(seedRange.End));

            internal IEnumerable<LongRange> Cut(LongRange seedRange)
            {
                var ranges = new List<LongRange> { seedRange };
                foreach (var converter in Converters)
                {
                    for (int i = ranges.Count -1 ; i >= 0; i--)
                    {
                        var range = ranges[i];
                        var cutRanges = range.CutWith(converter.Source);

                        ranges.RemoveAt(i);
                        ranges.AddRange(cutRanges);
                    }
                }

                return ranges;
            }
        }

        private readonly long[] _seeds;
        private readonly Mapping[] _mappings;

        public Day05()
        {
            (_seeds, _mappings) = ParseInput();
        }

        public override ValueTask<string> Solve_1()
        {
            IEnumerable<long> seeds = _seeds;

            foreach (var mapping in _mappings)
            {
                seeds = seeds.Select(mapping.Apply);
            }

            return new(seeds.Min().ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var seedRanges = _seeds.Chunk(2).Select(s => new LongRange(s[0], s[0] + s[1] - 1));

            foreach (var mapping in _mappings)
            {
                seedRanges = seedRanges
                    .SelectMany(mapping.Cut)
                    .Select(mapping.Apply);
            }

            return new(seedRanges.Select(s => s.Start).Min().ToString());
        }

        private (long[], Mapping[]) ParseInput()
        {
            var input = File.ReadAllText(InputFilePath).SplitIntoSections();

            var seeds = input[0][7..].Split(' ').Select(long.Parse);
            var mappings = input.Skip(1).Select(ParseMapping);

            return (seeds.ToArray(), mappings.ToArray());
        }

        private Mapping ParseMapping(string input)
        {
            var lines = input.SplitIntoLines();
            return new Mapping(
                lines.First(),
                lines.Skip(1)
                    .Select(line => line.Split(' ').Select(long.Parse))
                    .Select(numbers => new Converter(numbers.ToArray()))
                    .ToArray()
            );
        }
    }
}
