using System.Collections.Immutable;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day02(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day02>
{
    public static Day02 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        return await reader.ReadLinesAsync()
            .Select(s => Report.Parse(s))
            .CountAsync(IsSafe, CancellationToken.None);


    }

    private static bool IsSafe(Report report)
    {
        var pairwiseTypes = report.Levels.Pairwise((a, b) => b - a)
            .Select(difference => difference switch
            {
                >= 1 and <= 3 => ReportType.Increasing,
                >= -3 and <= -1  => ReportType.Decreasing,
                _ => ReportType.Unsafe,
            })
            .ToHashSet();

        if (pairwiseTypes.Count > 1)
        {
            return false;
        }

        return pairwiseTypes.FirstOrDefault() is not ReportType.Unsafe;
    }

    public override async Task<int> Part2()
    {
        return await reader.ReadLinesAsync()
            .Select(s => Report.Parse(s))
            .CountAsync(IsSafeish, CancellationToken.None);

    }

    private static bool IsSafeish(Report report) =>
        IsSafe(report)
        || Enumerable.Range(0, report.Levels.Count)
            .Select(i => new Report(report.Levels.RemoveAt(i)))
            .Any(IsSafe);

    private enum ReportType
    {
        Unsafe,
        Increasing,
        Decreasing,
    }

    private readonly record struct Report(ImmutableList<int> Levels)
    {
        public static Report Parse(ReadOnlySpan<char> line)
        {
            Span<Range> numberLocations = stackalloc Range[line.Count(' ') + 1];
            var count = line.Split(
                numberLocations,
                ' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var levels = ImmutableList.CreateBuilder<int>();
            for (var i = 0; i < count; i++)
            {
                levels.Add(int.Parse(line[numberLocations[i]]));
            }

            return new Report(levels.ToImmutable());
        }
    }
}

file static class EnumerableExtensions
{
    public static IEnumerable<TResult> Pairwise<T, TResult>(this IEnumerable<T> source, Func<T, T, TResult> selector)
    {
        using var enumerator = source.GetEnumerator();
        if (enumerator.MoveNext() is false)
        {
            yield break;
        }

        var previous = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return selector(previous, enumerator.Current);
            previous = enumerator.Current;
        }
    }
}


