using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day01 : BaseDay
    {
        private readonly string[] _input;

        public Day01()
        {
            _input = File.ReadAllLines(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            var sum = _input.Select(line => int.Parse(FindFirstDigit(line).ToString() + FindLastDigit(line).ToString()))
                            .Sum();
            return new(sum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            var sum = _input.Select(ReplaceNumbers)
                            .Select(line => int.Parse(FindFirstDigit(line).ToString() + FindLastDigit(line).ToString()))
                            .Sum();
            return new(sum.ToString());
        }

        private string ReplaceNumbers(string input)
        {
            return input.Replace("one", "o1e")
                        .Replace("two", "t2o")
                        .Replace("three", "t3e")
                        .Replace("four", "f4r")
                        .Replace("five", "f5e")
                        .Replace("six", "s6x")
                        .Replace("seven", "s7n")
                        .Replace("eight", "e8t")
                        .Replace("nine", "n9e");
        }

        private char FindFirstDigit(IEnumerable<char> input)
        {
            return input.SkipWhile(c => !char.IsDigit(c)).First();
        }

        private char FindLastDigit(IEnumerable<char> input)
        {
            return FindFirstDigit(input.Reverse());
        }
    }
}
