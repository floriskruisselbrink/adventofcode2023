namespace AdventOfCode2023;

public class Day04 : BaseDay
{
    private record ScratchCard(int CardId, int MatchingNumbers);

    private readonly IEnumerable<ScratchCard> _input;

    public Day04()
    {
        _input = ParseInput().ToArray();
    }

    public override ValueTask<string> Solve_1() => new(_input
        .Select(card => (1 << card.MatchingNumbers) / 2)
        .Sum()
        .ToString()
    );

    public override ValueTask<string> Solve_2()
    {
        var countList = Enumerable.Repeat(1, _input.Count()).ToArray();

        foreach (var card in _input)
        {
            for (var i = 0; i < card.MatchingNumbers; i++)
            {
                countList[card.CardId + i] += countList[card.CardId - 1];
            }
        }

        return new(countList.Sum().ToString());
    }

    private IEnumerable<ScratchCard> ParseInput()
    {
        return File.ReadLines(InputFilePath).Select(line =>
        {
            var parts = line.Split(':', '|');
            var cardId = int.Parse(parts[0].Substring(parts[0].IndexOf(' ') + 1));
            var winningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            var myNumbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            var matchingNumbers = myNumbers.Intersect(winningNumbers).Count();
            return new ScratchCard(cardId, matchingNumbers);
        });
    }
}
