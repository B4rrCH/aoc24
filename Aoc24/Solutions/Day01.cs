using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day01(TextReader input) : SolutionBase<int, int>, IConstructFromReader<Day01>
{
    public static Day01 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var lefts = new List<int>();
        var rights = new List<int>();

        await foreach (var line in input.ReadLinesAsync())
        {
            var (left, right) = line.AsSpan().ParseLine();
            lefts.Add(left);
            rights.Add(right);
        }

        return lefts.Order().Zip(rights.Order(), (l, r) => l - r).Sum(Math.Abs);
    }

    public override async Task<int> Part2()
    {
        var counts = new Dictionary<int, (int Left, int Right)>();

        await foreach (var line in input.ReadLinesAsync())
        {
            var (left, right) = line.AsSpan().ParseLine();
            counts.Update(left, counter => counter with { Left = counter.Left + 1 });
            counts.Update(right, counter => counter with { Right = counter.Right + 1 });
        }

        return counts.Sum(kvp => kvp.Key * kvp.Value.Left * kvp.Value.Right);
    }
}

file static class Day01Extensions
{
    public static void Update<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TValue, TValue> updateValue)
        where TKey : notnull
        where TValue : new()
    {
        if (dictionary.TryGetValue(key, out var value) is false)
        {
            value = new TValue();
        }
        dictionary[key] = updateValue(value);
    }

    public static (int Left, int Right) ParseLine(this ReadOnlySpan<char> line)
    {
        const StringSplitOptions splitOptions =
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

        Span<Range> destination = stackalloc Range[2];

        if (line.Split(destination, ' ', splitOptions) is 2
            && int.TryParse(line[destination[0]], out var left)
            && int.TryParse(line[destination[1]], out var right))
        {
            return (left, right);
        }

        throw new InvalidOperationException("Invalid input");
    }
}
