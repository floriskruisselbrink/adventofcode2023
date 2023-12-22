namespace AdventOfCode2023;

public class Day05 : BaseDay
{
    record Converter(Range<long> Source, long Delta)
    {
        internal Converter(long[] numbers) : this(new Range<long>(numbers[1], numbers[1] + numbers[2] - 1), numbers[0] - numbers[1]) { }
    }

    record Mapping(string Title, Converter[] Converters)
    {
        internal long Apply(long seed) => seed + (Converters.FirstOrDefault(c => c.Source.Contains(seed))?.Delta ?? 0);

        internal Range<long> Apply(Range<long> seedRange) => new(Apply(seedRange.Start), Apply(seedRange.End));

        internal IEnumerable<Range<long>> Cut(Range<long> seedRange)
        {
            var ranges = new List<Range<long>> { seedRange };
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
        var seedRanges = _seeds.Chunk(2).Select(s => new Range<long>(s[0], s[0] + s[1] - 1));

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
        var input = AocDownloader.GetInput(2023, 5).SplitIntoSections();

        var seeds = input.First()[7..].Split(' ').Select(long.Parse);
        var mappings = input.Select(ParseMapping);

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
