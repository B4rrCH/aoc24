using System.Runtime.InteropServices;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day08(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day08>
{
    public static Day08 Construct(TextReader reader) => new(reader);
    public override async Task<int> Part1()
    {
        var map = await ParseMap(reader.ReadLinesAsync());
        return map.Antennas.Values
            .SelectMany(antennas =>
                antennas.Pairs().SelectMany(p => GetAntinodesPart1(p.First, p.Second, map)))
            .Distinct()
            .Count();
    }

    public override async Task<int> Part2()
    {
        var map = await ParseMap(reader.ReadLinesAsync());
        return map.Antennas.Values
            .SelectMany(antennas =>
                antennas.Pairs().SelectMany(p => GetAntinodesPart2(p.First, p.Second, map)))
            .Distinct()
            .Count();
    }

    private static IEnumerable<Position> GetAntinodesPart1(Position a, Position b, Map map)
    {
        var offset = a - b;
        a += offset;
        b -= offset;

        if (map.IsInBounds(a))
        {
            yield return a;
        }

        if (map.IsInBounds(b))
        {
            yield return b;
        }
    }
    private static IEnumerable<Position> GetAntinodesPart2(Position a, Position b, Map map)
    {
        var offset = a - b;

        while (map.IsInBounds(a))
        {
            yield return a;
            a += offset;
        }

        while (map.IsInBounds(b))
        {
            yield return b;
            b -= offset;
        }
    }

    private static async Task<Map> ParseMap(IAsyncEnumerable<string> lines)
    {
        var width = 0;
        var height = 0;
        var antennas = new Dictionary<char, List<Position>>();
        await foreach (var line in lines)
        {
            width = Math.Max(width, line.Length);
            foreach (var (frequency, y) in FindAntennas(line.AsMemory()))
            {
                ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(antennas, frequency, out _);
                var list = value ??= [];
                list.Add(new Position(height, y));
            }
            ++height;
        }

        return new Map(antennas, height, width);
    }

    private static IEnumerable<(char Frequency, int Y)> FindAntennas(ReadOnlyMemory<char> line)
    {
        var y = 0;
        while (line.Span.IndexOfAnyExcept('.') is var index and >= 0)
        {
            yield return (line.Span[index], y + index);
            y += index + 1;
            line = line[(index + 1)..];
        }
    }

    private readonly record struct Map(
        IReadOnlyDictionary<char, List<Position>> Antennas,
        int Height,
        int Width)
    {
        public bool IsInBounds(Position position) =>
            0 <= position.X && position.X < this.Height && 0 <= position.Y && position.Y < this.Width;
    }

    private readonly record struct Position(int X, int Y)
    {
        public static Offset operator -(Position a, Position b) => new (a.X - b.X, a.Y - b.Y);
        public static Position operator +(Position a, Offset b) => new(a.X + b.Dx, a.Y + b.Dy);
        public static Position operator -(Position a, Offset b) => new(a.X - b.Dx, a.Y - b.Dy);
        public readonly record struct Offset(int Dx, int Dy);
    }
}

file static class ListExtensions
{
    public static IEnumerable<(T First, T Second)> Pairs<T>(this List<T> source)
    {
        using var enumerator1 = source.GetEnumerator();
        while (enumerator1.MoveNext())
        {
            using var enumerator2 = source.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(enumerator1.Current, enumerator2.Current) is false)
                {
                    yield return (enumerator1.Current, enumerator2.Current);
                }
            }
        }
    }
}