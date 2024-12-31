using System.Buffers;
using System.Text.RegularExpressions;
using Aoc24.IO;
using OneOf;

namespace Aoc24.Solutions;

public partial class Day03(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day03>
{
    private const string First = nameof(First);
    private const string Second = nameof(Second);

    [GeneratedRegex(@$"mul\((?<{First}>\d+),(?<{Second}>\d+)\)")]
    private static partial Regex MulRegex { get; }

    public static Day03 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1() =>
        await reader.ReadLinesAsync()
            .SelectMany(line => new Parser(line))
            .Select(token => token.Match(f0: mul => mul.First * mul.Second, f1: _ => 0, f2: _ => 0))
            .SumAsync(CancellationToken.None);

    public override async Task<int> Part2()
    {
        var sum = 0;
        var enabled = true;

        await foreach (var token in reader.ReadLinesAsync()
                           .SelectMany(line => new Parser(line)))
        {
            token.Switch(
                mul =>
                {
                    if (enabled)
                    {
                        sum += mul.First * mul.Second;
                    }
                },
                (Do _) => enabled = true,
                (Dont _) => enabled = false);
        }

        return sum;
    }

    private readonly record struct Parser(string Line) : IAsyncEnumerable<OneOf<Mul, Do, Dont>>
    {
        IAsyncEnumerator<OneOf<Mul, Do, Dont>> IAsyncEnumerable<OneOf<Mul, Do, Dont>>
            .GetAsyncEnumerator(CancellationToken cancellationToken) =>
            new Enumerator(this.Line);


        private record struct Enumerator(string Line) : IAsyncEnumerator<OneOf<Mul, Do, Dont>>
        {
            private static readonly SearchValues<string> Tokens =
                SearchValues.Create(["do()", "don't()", "mul("], StringComparison.Ordinal);

            private int searched = 0;

            public OneOf<Mul, Do, Dont> Current { get; private set; }
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;

            public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(this.MoveNext());

            private bool MoveNext()
            {
                while (this.searched < this.Line.Length)
                {
                    var nextPossibleStart =
                        this.Line.AsSpan(this.searched)
                            .IndexOfAny(Tokens);
                    if (nextPossibleStart < 0)
                    {
                        return false;
                    }

                    this.searched += nextPossibleStart;

                    switch (this.Line.AsSpan(this.searched))
                    {
                        case ['d', 'o', '(', ')', ..]:
                            this.searched += "do()".Length;
                            this.Current = new Do();
                            return true;
                        case ['d', 'o', 'n', '\'', 't', '(', ')', ..]:
                            this.searched += "dont()".Length;
                            this.Current = new Dont();
                            return true;
                        case ['m', 'u', 'l', '(', ..]:
                            var parsed = TryParseMul(this.Line.AsSpan(this.searched), out var mul);
                            if (parsed > 0)
                            {
                                this.searched += parsed;
                                this.Current = mul;
                                return true;
                            }
                            this.searched += "mul(".Length;
                            break;
                        default:
                            this.Current = new OneOf<Mul, Do, Dont>();
                            break;
                    }
                }

                return false;


            }

            private static int TryParseMul(ReadOnlySpan<char> chars, out Mul mul)
            {
                mul = default;
                chars = chars["mul(".Length..];
                var comma = chars.IndexOf(',');
                if (comma < 0)
                {
                    return 0;
                }
                var closeBracket = chars.IndexOf(')');
                if (closeBracket < 0)
                {
                    return 0;
                }

                if (int.TryParse(chars[..comma], out var left) is false
                    || int.TryParse(chars[(comma + 1)..closeBracket], out var right) is false)
                {
                    return 0;
                }

                mul = new Mul(left, right);
                return closeBracket + 1;
            }
        }
    }

    private readonly record struct Mul(int First, int Second);

    private readonly record struct Do;

    private readonly record struct Dont;
}
