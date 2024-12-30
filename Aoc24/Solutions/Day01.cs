using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day01(TextReader input)
{
    public async Task<int> Part1()
    {
        var lefts = new List<int>();
        var rights = new List<int>();

        await foreach (var line in input.ReadLinesAsync())
        {
            var (left, right) = ParseLine(line);
            lefts.Add(left);
            rights.Add(right);
        }

        return lefts.Order().Zip(rights.Order(), (l, r) => l - r).Sum(Math.Abs);
    }

    public async Task<int> Part2()
    {
        var lefts = new Dictionary<int, int>();
        var rights = new Dictionary<int, int>();

        await foreach (var line in input.ReadLinesAsync())
        {
            var (left, right) = ParseLine(line);
            lefts[left] = lefts.GetValueOrDefault(left, 0) + 1;
            rights[right] = rights.GetValueOrDefault(right, 0) + 1;
        }

        var sum = 0;

        foreach (var (number, leftCount) in lefts)
        {
            sum += number * leftCount * rights.GetValueOrDefault(number, 0);
        }

        return sum;
    }

    private static (int Left, int Right) ParseLine(ReadOnlySpan<char> line)
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
