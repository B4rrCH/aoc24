using Aoc24.IO;

namespace Aoc24;

public interface IConstructFromReader<out TSelf> where TSelf : IConstructFromReader<TSelf>
{
    static abstract TSelf Construct(TextReader reader);
}

public static class Solution
{
    public static async Task Run<TSolution>()
        where TSolution : SolutionBase, IConstructFromReader<TSolution>
    {
        var fileName = Path.GetFullPath(Path.Combine("Data", typeof(TSolution).Name));
        Console.WriteLine($"Running {typeof(TSolution).Name} on {fileName}");

        using (var reader = FileExtensions.OpenAsyncText(fileName))
        {
            var i = TSolution.Construct(reader);
            await i.RunPart1();
        }

        using (var reader = FileExtensions.OpenAsyncText(fileName))
        {
            var i = TSolution.Construct(reader);
            await i.RunPart2();
        }
    }
}

public abstract class SolutionBase
{
    public abstract Task RunPart1();
    public abstract Task RunPart2();
}

public abstract class SolutionBase<TResult1, TResult2> : SolutionBase
{
    public sealed override async Task RunPart1() =>
        Console.WriteLine($"\t{nameof(this.Part1)}: {await this.Part1()}");

    public sealed override async Task RunPart2() =>
        Console.WriteLine($"\t{nameof(this.Part2)}: {await this.Part2()}");
    public abstract Task<TResult1> Part1();
    public abstract Task<TResult2> Part2();
}
