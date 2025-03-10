using Aoc24;
using Aoc24.Solutions;
using Spectre.Console;

IEnumerable<(string, Func<Task<PartResult>>, Func<Task<PartResult>>)> solvers =
[
    Solvers<Day01>(),
    Solvers<Day02>(),
    Solvers<Day03>(),
    Solvers<Day04>(),
    Solvers<Day05>(),
    Solvers<Day06>(),
    Solvers<Day07>(),
    Solvers<Day08>(),
    Solvers<Day09>(),
];

var table = new Table()
    .AddColumns(
        "Name",
        "Part1",
        "[[ms]]",
        "Part2",
        "[[ms]]"
    ).MinimalBorder();

var progress = new Progress(AnsiConsole.Console) { RefreshRate = TimeSpan.FromMilliseconds(10) }
    .Columns(
        new TaskDescriptionColumn(),
        new ProgressBarColumn(),
        new PercentageColumn(),
        new SpinnerColumn(Spinner.Known.Aesthetic));

await progress.StartAsync(
    async ctx =>
    {
        foreach (var (name, part1, part2) in solvers)
        {
            var task = ctx.AddTask(name, true, 2);
            ctx.Refresh();
            var result1 = await part1();
            task.Value = 1;
            ctx.Refresh();
            var result2 = await part2();
            task.Value = 2;
            ctx.Refresh();

            table.AddRow(
                name,
                result1.Result,
                $"{result1.TimeTaken.TotalMilliseconds:0}",
                result2.Result,
                $"{result2.TimeTaken.TotalMilliseconds:0}");
        }
    });

AnsiConsole.Write(table);

return;

static (string, Func<Task<PartResult>>, Func<Task<PartResult>>) Solvers<TSolution>()
    where TSolution : SolutionBase, IConstructFromReader<TSolution>
{
    return (typeof(TSolution).Name, Solution.RunPart1<TSolution>, Solution.RunPart2<TSolution>);
}
