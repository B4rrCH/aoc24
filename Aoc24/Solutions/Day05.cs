using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day05(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day05>
{
    public static Day05 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var orders = await this.ParseOrders();

        return await reader.ReadLinesAsync()
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .SumAsync(pages =>
            {
                foreach (var (index, page) in pages.Index())
                {
                    if (orders[page].Intersect(pages[..index]).Any())
                    {
                        return 0;
                    }
                }

                return pages[pages.Length / 2];
            });
    }

    public override async Task<int> Part2()
    {
        var orders = await this.ParseOrders();
        var comparer = Comparer<int>.Create((a, b) => orders[a].Contains(b) ? 1 : orders[b].Contains(a) ? -1 : 0);

        return await reader.ReadLinesAsync()
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .SumAsync(pages =>
            {
                foreach (var (index, page) in pages.Index())
                {
                    if (orders[page].Intersect(pages[..index]).Any())
                    {
                        return pages.Order(comparer).ElementAt(pages.Length / 2);
                    }
                }

                return 0;
            });
    }

    private ValueTask<ILookup<int, int>> ParseOrders() =>
        reader.ReadLinesAsync().TakeWhile(s => s.Contains('|'))
            .Select(line =>
            {
                var index = line.IndexOf('|');
                return (
                    Before: int.Parse(line.AsSpan(..index)),
                    After: int.Parse(line.AsSpan((index+1)..)));
            })
            .ToLookupAsync(pair => pair.Before, pair => pair.After);
}
