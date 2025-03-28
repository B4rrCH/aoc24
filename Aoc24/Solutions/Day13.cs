using System.Buffers;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day13(TextReader reader) : SolutionBase<long, long>, IConstructFromReader<Day13>
{
    private static readonly SearchValues<char> Numbers = SearchValues.Create("0123456789");

    public static Day13 Construct(TextReader reader) => new(reader);

    public override async Task<long> Part1() =>
        await this.ParseMachines()
            .Select(MinimumCostToWin)
            .SumAsync();

    public override async Task<long> Part2()
    {
        const long offset = 10000000000000;
        var prizeOffset = new Point(offset, offset);
        return await this.ParseMachines()
            .Select(m => m with { Prize = m.Prize + prizeOffset })
            .Select(MinimumCostToWin)
            .SumAsync();
    }


    private static long MinimumCostToWin(Machine machine)
    {
        // This is a linear optimization problem of the form:
        // minimize     f(u,v) = 3u + v     for u, v in Z
        // subject to   prize = ua + vb
        // Unless a and b are collinear, there can only be at most one solution,
        // We see that the restriction can be re-written as
        // (a, b) (u, v)^T = prize and hence (u,v)^T = (a, b)^1 prize

        var ((ax, ay), (bx, by), (px, py)) = machine;
        var discriminant = ax * by - ay * bx;

        if (discriminant is 0L)
        {
            if (ax * py - ay * px is not 0L)
            {
                // a and b are collinear, but prize is not
                // There is no way to get to the prize
                return 0L;
            }

            // I don't want to solve linear non-homogeneous diophantine equations.
            // Apparently this is also fine for the examples and provided input from AOC 2024.
            // To do implement this, use Bézout's identity and the extended GCD.
            throw new NotSupportedException(
                "A, B and the Prize are collinear, but solving diophantine equations is not implemented.");
        }

        var (u, remainder) = long.DivRem(by * px - bx * py, discriminant);
        if (u < 0L || remainder is not 0L)
        {
            // There his no natural number solution, so we cannot reach the prize
            return 0L;
        }

        (var v, remainder) = long.DivRem(ax * py - ay * px, discriminant);
        if (v < 0L || remainder is not 0L)
        {
            // There his no natural number solution, so we cannot reach the prize
            return 0L;
        }

        return u + u + u + v;
    }

    private async IAsyncEnumerable<Machine> ParseMachines()
    {
        await using var enumerator = reader.ReadLinesAsync().GetAsyncEnumerator();

        do
        {
            var a = await enumerator.MoveNextAsync() ?
                GetButton(enumerator.Current, "Button A: X+")
                    : throw new InvalidOperationException("Expected a button.");
            var b = await enumerator.MoveNextAsync() ?
                GetButton(enumerator.Current, "Button B: X+")
                : throw new InvalidOperationException("Expected a button.");
            var prize = await enumerator.MoveNextAsync() ?
                GetButton(enumerator.Current, "Prize: X=")
                : throw new InvalidOperationException("Expected a button.");

            yield return new Machine(a, b, prize);
        } while (await enumerator.MoveNextAsync() /* Read empty line between inputs */);

        yield break;

        static Point GetButton(ReadOnlySpan<char> line, ReadOnlySpan<char> prefix)
        {
            var prefixLength = line.IndexOfAny(Numbers);
            line = line[prefixLength..];
            var xLenght = line.IndexOfAnyExcept(Numbers);

            var x = long.Parse(line[..xLenght], null);

            line = line[xLenght..];
            var betweenLength = line.IndexOfAny(Numbers);
            var y = long.Parse(line[betweenLength..], null);

            return new Point(x, y);
        }
    }

    private readonly record struct Point(long X, long Y)
    {
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
        public static Point operator *(long factor, Point point) => new(factor * point.X, factor * point.Y);
    }

    private readonly record struct Machine(Point A, Point B, Point Prize);
}