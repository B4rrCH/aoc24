using System.Numerics;
using System.Runtime.InteropServices;

namespace Aoc24.Solutions;

public class Day11(TextReader reader) : SolutionBase<ulong, ulong>, IConstructFromReader<Day11>
{
    private static readonly int MaxUlongDigits = ulong.MaxValue.ToString().Length;
    public static Day11 Construct(TextReader reader) => new(reader);

    public override async Task<ulong> Part1()
    {
        var input = await reader.ReadToEndAsync();
        var stones = SplitAndParse(input);
        return Blink(stones, 25);
    }

    public override async Task<ulong> Part2()
    {
        var input = await reader.ReadToEndAsync();
        var stones = SplitAndParse(input);
        return Blink(stones, 75);
    }

    private static IEnumerable<ulong> SplitAndParse(string input)
    {
        var start = 0;

        while (start < input.Length)
        {
            if (input.AsSpan(start).IndexOf(' ') is var digits and > 0)
            {
                yield return ulong.Parse(input.AsSpan(start)[..digits]);
                start += digits + 1;
            }
            else
            {
                yield return ulong.Parse(input.AsSpan(start));
                yield break;
            }
        }
    }

    private static ulong Blink(IEnumerable<ulong> stones, int blinkCount)
    {
        var counter = new Dictionary<ulong,ulong>();

        foreach (var stone in stones)
        {
            counter.IncrementBy(stone, 1ul);
        }

        for (var i = 0; i < blinkCount; ++i)
        {
            counter = counter.Aggregate(
                new Dictionary<ulong, ulong>(),
                (newCounter, pair) => Blink(newCounter, pair.Key, pair.Value));
        }

        return counter.Values.Sum();
    }

    private static Dictionary<ulong, ulong> Blink(Dictionary<ulong, ulong> newCounter, ulong stone, ulong count)
    {
        if (stone == 0)
        {
            newCounter.IncrementBy(1ul, count);
            return newCounter;
        }

        Span<char> buffer = stackalloc char[MaxUlongDigits];
        if (stone.TryFormat(buffer, out var charsWritten)
            && charsWritten % 2 == 0)
        {
            newCounter.IncrementBy(ulong.Parse(buffer[..(charsWritten / 2)]), count);
            newCounter.IncrementBy(ulong.Parse(buffer[(charsWritten / 2)..]), count);
            return newCounter;
        }

        newCounter.IncrementBy(2024ul * stone, count);
        return newCounter;
    }
}

file static class Extensions
{
    public static T Sum<T>(this IEnumerable<T> source)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T> =>
        source.Aggregate(T.AdditiveIdentity, (a, b) => checked(a + b));

    public static void IncrementBy<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue increment)
        where TKey : notnull
        where TValue : struct, IAdditionOperators<TValue, TValue, TValue> =>
        CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out _) += increment;
}
