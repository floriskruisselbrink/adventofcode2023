namespace AdventOfCode2023;

public class Day02 : BaseDay
{
    private record Draw(int Red, int Blue, int Green);
    private record GameRecord(int GameId, Draw[] Draws);

    private readonly IEnumerable<GameRecord> _input;

    public Day02()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(_input
        .Where(IsPossibleGame)
        .Select(game => game.GameId)
        .Sum()
        .ToString()
    );

    public override ValueTask<string> Solve_2() => new(_input
        .Select(FindMinimumSetOfCubes)
        .Sum()
        .ToString()
    );

    private bool IsPossibleGame(GameRecord game) =>
        game.Draws.All((Func<Draw, bool>)(draw =>
            draw.Red <= 12 &&
            draw.Green <= 13 &&
            draw.Blue <= 14)
        );

    private int FindMinimumSetOfCubes(GameRecord game)
    {
        int minRed = 0, minGreen = 0, minBlue = 0;

        foreach (var draw in game.Draws)
        {
            minRed = Math.Max(minRed, draw.Red);
            minGreen = Math.Max(minGreen, draw.Green);
            minBlue = Math.Max(minBlue, draw.Blue);
        }
        return minRed * minGreen * minBlue;
    }

    private IEnumerable<GameRecord> ParseInput()
    {
        return File.ReadAllLines(InputFilePath).Select(line =>
        {
            var gameId = line[5..line.IndexOf(':')];

            var draws = line.Substring(line.IndexOf(':') + 1).Split("; ").Select(draw =>
            {
                int red = 0, blue = 0, green = 0;
                foreach (var item in draw.Split(", "))
                {
                    if (item.EndsWith("red"))
                    {
                        red += int.Parse(item.Remove(item.Length - 4));
                    }
                    else if (item.EndsWith("blue"))
                    {
                        blue += int.Parse(item.Remove(item.Length - 5));
                    }
                    else if (item.EndsWith("green"))
                    {
                        green += int.Parse(item.Remove(item.Length - 6));
                    }
                }
                return new Draw(red, blue, green);
            }).ToArray();

            return new GameRecord(int.Parse(gameId), draws);
        });
    }
}
