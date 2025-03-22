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
];

foreach (var (name, part1, part2) in solvers)
{
    Console.WriteLine(name);
    var result1 = await part1();
    Console.WriteLine($"    Part1: {result1.Result,15} ({result1.TimeTaken.TotalMilliseconds:0} ms)");
    var result2 = await part2();
    Console.WriteLine($"    Part2: {result2.Result,15} ({result2.TimeTaken.TotalMilliseconds:0} ms)");
}

return;

static (string, Func<Task<PartResult>>, Func<Task<PartResult>>) Solver<TSolution>()
    where TSolution : SolutionBase, IConstructFromReader<TSolution>
{
    return (typeof(TSolution).Name, Solution.RunPart1<TSolution>, Solution.RunPart2<TSolution>);
}
