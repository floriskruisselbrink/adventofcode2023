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
            using var input = new StreamReader(InputFileDirPath + "\\05.txt");
            
            var seeds = input.ReadLine()!.Split(' ').Skip(1).Select(long.Parse);

            input.ReadLine();

            return (seeds.ToArray(), ParseMappings(input).ToArray());
        }

        private IEnumerable<Mapping> ParseMappings(StreamReader input)
        {
            string? line;
            string title = "";
            List<string> mappings = [];
            while ((line = input.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    yield return CreateMapping(title, mappings);
                    title = "";
                    mappings = [];
                    continue;
                }

                if (mappings.Count == 0)
                {
                    title = line;
                    line = input.ReadLine();
                }

                mappings.Add(line!);
            }

            yield return CreateMapping(title, mappings); 
        }

        private Mapping CreateMapping(string title, List<string> mappings)
        {
            var m = mappings.Select(s => new Converter(s.Split(' ').Select(long.Parse).ToArray()));
            return new Mapping(title, m.ToArray());
        }
    }
}
