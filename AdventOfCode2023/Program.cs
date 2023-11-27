if (args.Length == 0)
{
    await Solver.SolveLast(opt => opt.ClearConsole = false);
}
else if (args.Length == 1 && args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase))
{
    await Solver.SolveAll(opt =>
    {
        opt.ShowConstructorElapsedTime = true;
        opt.ShowTotalElapsedTimePerDay = true;
    });
}
else
{
    var indices = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);

    await Solver.Solve(indices.Where(i => i < uint.MaxValue));
}