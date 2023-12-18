using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Test;

public static class SolutionTests
{
    [TestCase(typeof(Day01), "54597", "54504")]
    [TestCase(typeof(Day02), "2278", "67953")]
    [TestCase(typeof(Day03), "533775", "78236071")]
    [TestCase(typeof(Day04), "22193", "5625994")]
    [TestCase(typeof(Day05), "218513636", "81956384")]
    [TestCase(typeof(Day06), "303600", "23654842")]
    [TestCase(typeof(Day07), "249390788", "248750248")]
    [TestCase(typeof(Day08), "13771", "13129439557681")]
    [TestCase(typeof(Day09), "1884768153", "1031")]
    [TestCase(typeof(Day10), "7107", "281")]
    [TestCase(typeof(Day11), "9418609", "593821230983")]
    [TestCase(typeof(Day12), "6827", "1537505634471")]
    [TestCase(typeof(Day13), "27505", "22906")]
    // [TestCase(typeof(Day14), "107053", "88371")]
    [TestCase(typeof(Day15), "511343", "294474")]
    // [TestCase(typeof(Day16), "8249", "8444")]
    public static void Test(Type type, string solution1, string solution2)
    {
        if (Activator.CreateInstance(type) is BaseProblem instance)
        {
            Assert.Multiple(async () =>
            {
                Assert.That(await instance.Solve_1(), Is.EqualTo(solution1));
                Assert.That(await instance.Solve_2(), Is.EqualTo(solution2));
            });
        }
        else
        {
            Assert.Fail($"{type} is not a BaseProblem");
        }
    }
}
