using Aoc24.IO;

namespace Aoc24.Solutions;

using Position = (int X, int Y);

public class Day04(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day04>
{
    public static Day04 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var lines = await reader.ReadLinesAsync().ToArrayAsync();
        return Part1Candidates(lines)
            .Count(positions =>
                lines[positions.Item1.X][positions.Item1.Y] == 'X'
                && lines[positions.Item2.X][positions.Item2.Y] == 'M'
                && lines[positions.Item3.X][positions.Item3.Y] == 'A'
                && lines[positions.Item4.X][positions.Item4.Y] == 'S');
    }

    public override async Task<int> Part2() =>
        await reader.ReadLinesAsync().Chunk3()
            .Select(threeLines =>
            {
                var (first, second, third) = threeLines;
                var sum = 0;

                for (var i = 1; i + 1 < second.Length; ++i)
                {
                    if (second[i] is 'A'
                        && (first[i - 1], third[i + 1]) is ('M', 'S') or ('S', 'M')
                        && (first[i + 1], third[i - 1]) is ('M', 'S') or ('S', 'M'))
                    {
                        ++sum;
                    }
                }

                return sum;
            })
            .SumAsync();

    private static IEnumerable<(Position, Position, Position, Position)> Part1Candidates(string[] grid)
    {
        return Enumerable.Range(0, grid.Length).SelectMany(x => Enumerable.Range(0, grid[0].Length).Select(y => (x, y)))
            .SelectMany(start => Part1CandidatesStartingAt(grid.Length, grid[0].Length, start));

        static IEnumerable<(Position, Position, Position, Position)> Part1CandidatesStartingAt(
            int maxX, int maxY, Position start)
        {
            var (x, y) = start;

            if (x + 3 < maxX)
            {
                yield return (start, (x + 1, y), (x + 2, y), (x + 3, y));

                if (y + 3 < maxY)
                {
                    yield return (start, (x + 1, y + 1), (x + 2, y + 2), (x + 3, y + 3));
                }

                if (0 <= y - 3)
                {
                    yield return (start, (x + 1, y - 1), (x + 2, y - 2), (x + 3, y - 3));
                }

            }

            if (0 <= x - 3)
            {
                yield return (start, (x - 1, y), (x - 2, y), (x - 3, y));

                if (y + 3 < maxY)
                {
                    yield return (start, (x - 1, y + 1), (x - 2, y + 2), (x - 3, y + 3));
                }

                if (0 <= y - 3)
                {
                    yield return (start, (x - 1, y - 1), (x - 2, y - 2), (x - 3, y - 3));
                }
            }

            if (y + 3 < maxY)
            {
                yield return (start, (x, y + 1), (x, y + 2), (x, y + 3));
            }

            if (0 <= y - 3)
            {
                yield return (start, (x, y - 1), (x, y - 2), (x, y - 3));
            }
        }
    }


}

file static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<(T, T, T)> Chunk3<T>(
        this IAsyncEnumerable<T> source)
    {
        await using var enumerator = source.GetAsyncEnumerator();

        if (await enumerator.MoveNextAsync() is false)
        {
            yield break;
        }
        var first = enumerator.Current;

        if (await enumerator.MoveNextAsync() is false)
        {
            yield break;
        }
        var second = enumerator.Current;

        while (await enumerator.MoveNextAsync())
        {
            var third = enumerator.Current;
            yield return (first, second, third);
            (first, second) = (second, third);
        }
    }
}