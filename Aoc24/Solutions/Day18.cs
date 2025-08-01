using System.Diagnostics;
using System.Numerics;
using Aoc24.IO;
using Point = (int X, int Y);

namespace Aoc24.Solutions;

public class Day18(TextReader reader) : SolutionBase<int, string>, IConstructFromReader<Day18>
{
    public static Day18 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var map = new Map(71, 71);
        await foreach (var (x, y) in reader.ReadLinesAsync()
                           .Select(line =>
                           {
                               var span = line.AsSpan();
                               var comma = span.IndexOf(',');
                               return (int.Parse(span[(comma + 1)..]), int.Parse(span[..comma]));
                           })
                           .Take(1024))
        {
            map[x, y] = -2;
        }

        return map.Find<Map, int>((0, 0), (70, 70));
    }

    public override async Task<string> Part2()
    {
        var map = new Map(71, 71);

        var allBlocks = await reader.ReadLinesAsync()
            .Select(line =>
            {
                var span = line.AsSpan();
                var comma = span.IndexOf(',');
                return (X: int.Parse(span[(comma + 1)..]), Y: int.Parse(span[..comma]));
            })
            .ToArrayAsync();

        var lower = 0;
        var upper = allBlocks.Length;

        var middle = upper / 2;
        while (lower < upper)
        {
            map.Reset();

            foreach (var (x, y) in allBlocks.AsSpan(0, middle))
            {
                map[x, y] = Dijkstra.Blocked;
            }

            if (map.Find<Map, int>((0, 0), (70, 70)) >= 0)
            {
                lower = middle + 1;
                middle = lower/ 2 + upper / 2 + lower % 2 * (upper % 2);
            }
            else
            {
                upper = middle;
                middle = lower/ 2 + upper / 2 + lower % 2 * (upper % 2);
            }
        }

        var lastBlock = allBlocks[middle - 1];
        return $"{lastBlock.Y},{lastBlock.X}";
    }
}

internal static class Dijkstra
{
    public static TNumber Find<TMap, TNumber>(this TMap map, Point start, Point end)
        where TMap : ITwoDimensional<TNumber>
        where TNumber : INumber<TNumber>
    {
        map[start.X, start.Y] = TNumber.Zero;
        var queue = new Queue<Point>([start]);

        while (queue.TryDequeue(out var current))
        {
            Debug.Assert(
                map[current.X, current.Y] >= TNumber.Zero,
                $"Expected map[{current.X}, {current.Y}] >= 0 but was {map[current.X, current.Y]}");
            if (current == end)
            {
                return map[current.X, current.Y];
            }
            var distance = map[current.X, current.Y] + TNumber.One;

            map.AddIfUnseen(queue, (current.X, current.Y + 1), distance);
            map.AddIfUnseen(queue, (current.X + 1, current.Y), distance);
            map.AddIfUnseen(queue, (current.X, current.Y - 1), distance);
            map.AddIfUnseen(queue, (current.X - 1, current.Y), distance);
        }

        return map[end.X, end.Y];
    }

    private static void AddIfUnseen<TMap, TNumber>(this TMap map, Queue<Point> queue, Point point, TNumber distance)
        where TMap : ITwoDimensional<TNumber>
        where TNumber : INumber<TNumber>
    {
        if (map.IsUnseen<TMap, TNumber>(point) is false)
        {
            return;
        }

        queue.Enqueue(point);
        map[point.X, point.Y] = distance;
    }

    private static bool IsUnseen<TMap, TNumber>(this TMap map, Point point)
        where TMap : ITwoDimensional<TNumber>
        where TNumber : INumber<TNumber> =>
        0 <= point.X
        && point.X < map.Height
        && 0 <= point.Y
        && point.Y < map.Width
        && map[point.X, point.Y] == -TNumber.One;

    public const int Blocked = -2;
}

internal interface ITwoDimensional<T>
{
    int Width { get; }
    int Height { get; }
    T this[int x, int y]  { get; set; }

}

internal class Map : ITwoDimensional<int>
{
    private readonly int[,] storage;

    public int Width { get; }
    public int Height { get; }

    public int this[int x, int y]
    {
        get => this.storage[x, y];
        set => this.storage[x, y] = value;
    }

    public Map(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        this.storage = new int[width, height];
        this.Reset();
    }

    public void Reset() => this.storage.AsSpan().Fill(-1);
}