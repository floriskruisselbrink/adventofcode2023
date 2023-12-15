namespace AdventOfCode2023;

public class Day15 : BaseDay
{
    private readonly IList<string> _input;

    public Day15()
    {
        _input = ParseInput();
    }

    public override ValueTask<string> Solve_1() => new(
        _input.Select(Hash).Sum().ToString()
    );

    public override ValueTask<string> Solve_2()
    {
        var boxes = new Boxes();
        foreach (var instruction in _input)
        {
            boxes.ProcessInstruction(instruction);
        }

        return new(boxes.CalculateFocusingPower().ToString());
    }

    private readonly record struct FocusedLens(string Label, int FocalLength);

    private class Boxes
    {
        private readonly List<FocusedLens>[] _boxes = new List<FocusedLens>[256];

        public Boxes()
        {
            for (var i = 0; i < _boxes.Length; i++)
            {
                _boxes[i] = [];
            }
        }

        public long CalculateFocusingPower()
        {
            var power = 0L;

            foreach (var (box, i) in _boxes.WithIndex())
            {
                foreach (var (lens, j) in box.WithIndex())
                {
                    power += LensPower(i + 1, j + 1, lens.FocalLength);
                }
            }

            return power;
        }

        public void ProcessInstruction(string instruction)
        {
            if (instruction.Last() == '-')
            {
                RemoveLens(instruction[..^1]);
            }
            else
            {
                AddLens(instruction.Split('='));
            }
        }


        private void RemoveLens(string label)
        {
            _boxes[Hash(label)].RemoveAll(lens => lens.Label == label);
        }

        private void AddLens(string[] instruction)
        {
            var lens = new FocusedLens(instruction[0], int.Parse(instruction[1]));
         
            var hash = Hash(instruction[0]);
            var existing = _boxes[hash].FindIndex(lens => lens.Label == instruction[0]);
            if (existing >= 0)
            {
                _boxes[hash][existing] = lens; 
            }
            else
            {
                _boxes[hash].Add(lens);
            }
        }
    }

    private static int Hash(string input)
        => input.Aggregate(0, (hash, ch) => (hash + ch) * 17 % 256);

    private static int LensPower(int boxNumber, int slotNumber, int focalLength) 
        => boxNumber * slotNumber * focalLength;

    private static IList<string> ParseInput()
    {
        return AocDownloader.GetInput(2023, 15).ReadLine()!.Split(',').ToList();
    }
}
