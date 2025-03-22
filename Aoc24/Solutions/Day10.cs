using System.Runtime.InteropServices;
using Aoc24.IO;
using Microsoft.Extensions.ObjectPool;

namespace Aoc24.Solutions;

public class Day10(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day10>
{
    private static readonly ObjectPool<HashSet<(int X, int Y)>> PositionPool = HashSetPool.Create<(int X, int Y)>();

    private static readonly ObjectPool<Dictionary<(int X, int Y), int>> CountPool =
        DictionaryPool.Create<(int X, int Y), int>();

    public static Day10 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var grid = await reader.ReadTo2DArrayAsync();

        return grid.Indexes()
                .AsParallel()
                .Where(t => grid[t.X, t.Y] is '0')
                .Select(start => CountSummits(grid, start))
                .Sum();

    }

    private static int CountSummits(char[,] grid, (int X, int Y) start)
    {
        var previousPositions = PositionPool.Get();
        previousPositions.Add(start);
        for (var nextHeight = '1'; nextHeight <= '9'; nextHeight++)
        {
            // ReSharper disable once AccessToModifiedClosure
            var nextPositions = previousPositions
                .SelectMany(Neighbours)
                .Where(p => grid[p.X, p.Y] == nextHeight)
                .ToHashSet(PositionPool);
            PositionPool.Return(previousPositions);
            previousPositions = nextPositions;
        }

        var result = previousPositions.Count;
        PositionPool.Return(previousPositions);
        return result;

        IEnumerable<(int X, int Y)> Neighbours((int, int) point)
        {
            if (point.Item1 - 1 >= 0)
                yield return (point.Item1 - 1, point.Item2);

            if (point.Item2 - 1 >= 0)
                yield return (point.Item1, point.Item2 - 1);

            if (point.Item1 + 1 < grid.GetLength(0))
                yield return (point.Item1 + 1, point.Item2);

            if (point.Item2 + 1 < grid.GetLength(1))
                yield return (point.Item1, point.Item2 + 1);
        }
    }

    public override async Task<int> Part2()
    {
        var grid = await reader.ReadTo2DArrayAsync();

        return grid.Indexes()
            .AsParallel()
            .Where(t => grid[t.X, t.Y] is '0')
            .Select(start => CountPaths(grid, start))
            .Sum();
    }

    private static int CountPaths(char[,] grid, (int X, int Y) start)
    {
        var previous = CountPool.Get();
        previous.Add(start, 1);
        for (var nextHeight = '1'; nextHeight <= '9'; nextHeight++)
        {
            var next = CountPool.Get();
            foreach (var (position, count) in previous)
            {
                foreach (var neighbour in Neighbours(position))
                {
                    if (grid[neighbour.X, neighbour.Y] == nextHeight)
                    {
                        CollectionsMarshal.GetValueRefOrAddDefault(next, neighbour, out _) += count;
                    }
                }
            }

            CountPool.Return(previous);
            previous = next;
        }

        var result = previous.Values.Sum();
        CountPool.Return(previous);
        return result;

        IEnumerable<(int X, int Y)> Neighbours((int, int) point)
        {
            if (point.Item1 - 1 >= 0)
                yield return (point.Item1 - 1, point.Item2);

            if (point.Item2 - 1 >= 0)
                yield return (point.Item1, point.Item2 - 1);

            if (point.Item1 + 1 < grid.GetLength(0))
                yield return (point.Item1 + 1, point.Item2);

            if (point.Item2 + 1 < grid.GetLength(1))
                yield return (point.Item1, point.Item2 + 1);
        }
    }
}
