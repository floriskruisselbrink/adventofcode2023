namespace AdventOfCode2023;

public class Day07 : BaseDay
{
    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    private record class CamelHand(string Cards, long Bid, bool WithJokers): IComparable<CamelHand>
    {
        public CamelHand(string input, bool withJokers): this(input[0..5], int.Parse(input[6..]), withJokers) {}

        private readonly HandType _handType = CalculateHandType(Cards, WithJokers);
        private readonly string _sortKey = CalculateSortKey(Cards, WithJokers);

        public int CompareTo(CamelHand? other)
        {
            if (other is null)
            {
                return 1;
            }

            int result = _handType.CompareTo(other._handType);
            if (result == 0)
            {
                result = _sortKey.CompareTo(other._sortKey);
            }
            return result;
        }

        private static string CalculateSortKey(string cards, bool withJokers)
        {
            return cards
                .Replace('T', 'a')
                .Replace('J', withJokers ? '1' : 'b')
                .Replace('Q', 'c')
                .Replace('K', 'd')
                .Replace('A', 'e')
                ;
        }

        private static HandType CalculateHandType(string cards, bool withJokers)
        {
            var jokerCount = withJokers ? cards.Count(c => c == 'J') : 0;
            var groupCount = cards.Where(c => !withJokers || c != 'J').GroupBy(c => c);

            if (groupCount.Count() == 1 || jokerCount == 5)
            {
                return HandType.FiveOfAKind;
            }
            else if (groupCount.Any(g => g.Count() + jokerCount == 4))
            {
                return HandType.FourOfAKind;
            }
            else if (groupCount.Any(g => g.Count() == 3) && groupCount.Any(g => g.Count() == 2) ||
                     (jokerCount == 1 && groupCount.Count(g => g.Count() == 2) == 2))
            {
                return HandType.FullHouse;
            }
            else if (groupCount.Any(g => (g.Count() + jokerCount) == 3))
            {
                return HandType.ThreeOfAKind;
            }
            else if (groupCount.Count(g => g.Count() == 2) == 2)
            {
                return HandType.TwoPair;
            }
            else if (groupCount.Any(g => (g.Count() + jokerCount) == 2))
            {
                return HandType.OnePair;
            }
            else
            {
                return HandType.HighCard;
            }
        }
    }

    private readonly string[] _input;

    public Day07()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(Solve(false).ToString());
    public override ValueTask<string> Solve_2() => new(Solve(true).ToString());

    private long Solve(bool withJokers) => _input
        .Select(line => new CamelHand(line, withJokers))
        .Order()
        .Select((hand, i) => hand.Bid * (i + 1))
        .Sum();

    private string[] ParseInput() => File.ReadAllLines(InputFilePath);
}
