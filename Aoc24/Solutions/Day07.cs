using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day07(TextReader reader) : SolutionBase<long, long>, IConstructFromReader<Day07>
{
    public static Day07 Construct(TextReader reader) => new(reader);

    public override async Task<long> Part1()
    {
        var lines = await reader.ReadLinesAsync().ToArrayAsync();
        return lines.AsParallel().Sum(line => CountSatisfiableResults(line, 2));
    }

    public override async Task<long> Part2()
    {
        var lines = await reader.ReadLinesAsync().ToArrayAsync();
        return lines.AsParallel().Sum(line => CountSatisfiableResults(line, 3));
    }

    private static long CountSatisfiableResults(ReadOnlySpan<char> line, int numberOfOps)
    {
        if (line.IndexOf(':') is not (var colon and >= 0))
        {
            throw new ArgumentException("Invalid line, does not contain ':'");
        }

        var target = long.Parse(line[..colon]);

        var arguments = line[(colon + 2)..];
        Span<long> numbers = stackalloc long[arguments.Count(' ') + 1];
        var i = 0;
        foreach (var range in arguments.Split(' '))
        {
            numbers[i++] = long.Parse(arguments[range]);
        }

        return CanSatisfy(target, numbers, numberOfOps) ? target : 0L;
    }

    private static bool CanSatisfy(long target, ReadOnlySpan<long> numbers, int numberOfOps)
    {
        var limit = 1;
        for (var i = 0; i < numbers.Length - 1; ++i)
        {
            limit *= numberOfOps;
        }

        for (var ops = 0; ops < limit; ++ops)
        {
            if (FoldOperations(numbers, numberOfOps, ops) == target)
            {
                return true;
            }
        }
        return false;
    }

    private static long FoldOperations(ReadOnlySpan<long> numbers, int numberOfOps, int ops)
    {
        var actual = numbers[0];

        for (var j = 0; j < numbers.Length - 1; ++j)
        {
            actual = (ops % numberOfOps) switch
            {
                0 => actual + numbers[j + 1],
                1 => actual * numbers[j + 1],
                _ => Concat(actual, numbers[j + 1]),
            };
            ops /= numberOfOps;
        }

        return actual;
    }

    private static long Concat(long a, long b)
    {
        var bCopy = b;
        while (bCopy != 0)
        {
            a *= 10;
            bCopy /= 10;
        }
        return a + b;
    }
}
