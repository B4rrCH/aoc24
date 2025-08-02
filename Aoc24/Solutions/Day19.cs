using System.Collections.Frozen;
using Aoc24.IO;

namespace Aoc24.Solutions;

using StringSet = FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>>;

public class Day19(TextReader reader) : SolutionBase<int, long>, IConstructFromReader<Day19>
{
    public static Day19 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var towels = await this.ParseTowels();
        var patterns = await reader.ReadLinesAsync().ToListAsync();
        return patterns.AsParallel().Count(pattern => Part2(pattern, towels) > 0);
    }

    public override async Task<long> Part2()
    {
        var towels = await this.ParseTowels();
        var patterns = await reader.ReadLinesAsync().ToListAsync();
        return patterns.AsParallel().Sum(pattern => Part2(pattern, towels));
    }

    private static long Part2(string pattern, StringSet towels)
    {
        Span<long> nrWaysUpTo = stackalloc long[pattern.Length + 1];
        nrWaysUpTo[0] = 1;

        for (var towelEnd = 1; towelEnd <= pattern.Length; towelEnd++)
        {
            for (var towelStart = 0; towelStart < towelEnd; towelStart++)
            {
                if (nrWaysUpTo[towelStart] > 0 && towels.Contains(pattern.AsSpan(towelStart, towelEnd - towelStart)))
                {
                    nrWaysUpTo[towelEnd] += nrWaysUpTo[towelStart];
                }
            }
        }

        return nrWaysUpTo[pattern.Length];
    }

    private async Task<StringSet> ParseTowels()
    {
        var line = await reader.ReadLineAsync();
        _ = await reader.ReadLineAsync();

        var list = new HashSet<string>();

        foreach (var range in line.AsSpan().Split(','))
        {
            list.Add(line.AsSpan(range).Trim().ToString());
        }

        return list.ToFrozenSet().GetAlternateLookup<ReadOnlySpan<char>>();
    }
}
