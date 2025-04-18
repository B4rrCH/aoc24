using Aoc24;
using Aoc24.Solutions;

IEnumerable<(string, Func<Task<PartResult>>, Func<Task<PartResult>>)> solvers =
[
    Solver<Day01>(),
    Solver<Day02>(),
    Solver<Day03>(),
    Solver<Day04>(),
    Solver<Day05>(),
    Solver<Day06>(),
    Solver<Day07>(),
    Solver<Day08>(),
    Solver<Day09>(),
    Solver<Day10>(),
    Solver<Day11>(),
    Solver<Day12>(),
    Solver<Day13>(),
    Solver<Day14>(),
    Solver<Day15>(),
    Solver<Day16>(),
    Solver<Day17>(),
];

foreach (var (name, part1, part2) in solvers)
{
    var longest = ulong.MaxValue.ToString().Length;
    Console.WriteLine(name);
    var result1 = await part1();
    Console.WriteLine(
        $$"""    Part1: {0,{{longest}}} ({1:0} ms)""",
        result1.Result,
        result1.TimeTaken.TotalMilliseconds);
    var result2 = await part2();
    Console.WriteLine(
        $$"""    Part2: {0,{{longest}}} ({1:0} ms)""",
        result2.Result,
        result2.TimeTaken.TotalMilliseconds);
}

return;

static (string, Func<Task<PartResult>>, Func<Task<PartResult>>) Solver<TSolution>()
    where TSolution : SolutionBase, IConstructFromReader<TSolution>
{
    return (typeof(TSolution).Name, Solution.RunPart1<TSolution>, Solution.RunPart2<TSolution>);
}
