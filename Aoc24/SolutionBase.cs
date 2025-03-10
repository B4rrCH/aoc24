using System.Diagnostics;

namespace Aoc24;

public interface IConstructFromReader<out TSelf> where TSelf : IConstructFromReader<TSelf>
{
    static abstract TSelf Construct(TextReader reader);
}

public abstract class SolutionBase
{
    public abstract Task<PartResult> RunPart1();
    public abstract Task<PartResult> RunPart2();
}

public abstract class SolutionBase<TResult1, TResult2> : SolutionBase
{
    public sealed override async Task<PartResult> RunPart1()
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await this.Part1();
        stopwatch.Stop();
        return new PartResult($"{result}", stopwatch.Elapsed);
    }

    public sealed override async Task<PartResult> RunPart2()
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await this.Part2();
        stopwatch.Stop();
        return new PartResult($"{result}", stopwatch.Elapsed);
    }

    public abstract Task<TResult1> Part1();
    public abstract Task<TResult2> Part2();
}
