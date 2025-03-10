using Aoc24.IO;

namespace Aoc24;

public readonly record struct PartResult(string Result, TimeSpan TimeTaken);

public static class Solution
{
    public static async Task<PartResult> RunPart1<TSolution>()
        where TSolution : SolutionBase, IConstructFromReader<TSolution>
    {
        var fullPath = Path.GetFullPath(Path.Combine("Data", typeof(TSolution).Name));
        using var reader = FileExtensions.OpenAsyncText(fullPath);
        return await TSolution.Construct(reader).RunPart1();
    }

    public static async Task<PartResult> RunPart2<TSolution>()
        where TSolution : SolutionBase, IConstructFromReader<TSolution>
    {
        var fullPath = Path.GetFullPath(Path.Combine("Data", typeof(TSolution).Name));
        using var reader = FileExtensions.OpenAsyncText(fullPath);
        return await TSolution.Construct(reader).RunPart2();
    }
}
