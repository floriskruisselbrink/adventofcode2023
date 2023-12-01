using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023.Test;

public static class SolutionTests
{
    [TestCase(typeof(Day01), "54597", "54504")]
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
