namespace AdventOfCode2023;

public class Day03 : BaseDay
{
    private readonly Grid<char> _input;

    public Day03()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
            FindPartNumbers(_ => true)
                .Select(p => p.number)
                .Sum()
                .ToString()
            );

    public override ValueTask<string> Solve_2() => new(
            FindPartNumbers(c => c == '*')
                .GroupBy(part => (part.x, part.y))
                .Where(group => group.Count() == 2)
                .Select(group => group.First().number * group.Last().number)  
                .Sum()
                .ToString()
            );

    private IEnumerable<(int x, int y, int number)> FindPartNumbers(Func<char, bool> symbolTest)
    {
        var bounds = _input.CalculateBounds();

        for (int y = bounds.minY; y <= bounds.maxY; y++)
        {
            int currentNumber = 0;
            bool currentAdjacent = false;
            int currentX = -1;
            int currentY = -1;
            for (int x = bounds.minX; x <= bounds.maxX; x++)
            {
                if (!_input[x, y].Exists)
                {
                    if (currentAdjacent && currentNumber > 0)
                    {
                        yield return (currentX, currentY, currentNumber);
                    }
                    currentNumber = 0;
                    currentAdjacent = false;
                }
                else if (char.IsDigit(_input[x, y]))
                {
                    currentNumber = currentNumber * 10 + (_input[x, y] - '0');
                    foreach (var neighbour in _input.Neighbours(new Coords(x, y), true))
                    {
                        if (!char.IsDigit(neighbour.Value) && symbolTest(neighbour.Value))
                        {
                            currentAdjacent = true;
                            currentX = neighbour.X;
                            currentY = neighbour.Y;
                        }
                    }
                }
                else if (currentAdjacent && currentNumber > 0)
                {
                    yield return (currentX, currentY, currentNumber);
                    currentNumber = 0;
                    currentAdjacent = false;
                }
            }

            if (currentAdjacent && currentNumber > 0)
            {
                yield return (currentX, currentY, currentNumber);
            }
        }
    }
    private Grid<char> ParseInput()
    {
        var lines = File.ReadAllLines(InputFilePath);
        var grid = new Grid<char>();

        int y = 0;
        foreach (var line in lines)
        {
            int x = 0;
            foreach (char ch in line)
            {
                if (ch != '.')
                {
                    grid[x, y] = ch;
                }
                x++;
            }
            y++;
        }

        return grid;
    }
}
