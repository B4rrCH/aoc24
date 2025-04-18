using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Aoc24.Solutions;

public class Day17(TextReader reader) : SolutionBase<string, long>, IConstructFromReader<Day17>
{
    private static readonly SearchValues<char> Digits = SearchValues.Create("0123456789");
    public static Day17 Construct(TextReader reader) => new(reader);

    public override async Task<string> Part1()
    {
        var a = ParseNumber(await reader.ReadLineAsync());
        var b = ParseNumber(await reader.ReadLineAsync());
        var c = ParseNumber(await reader.ReadLineAsync());
        await reader.ReadLineAsync();

        var numbers = ParseNumbers(await reader.ReadLineAsync());
        var enumerator = new Enumerator(a, b, c, numbers.ToArray());

        if (enumerator.MoveNext() is false)
        {
            return "";
        }

        var sb = new StringBuilder();
        sb.Append(enumerator.Current);

        while (enumerator.MoveNext())
        {
            sb.Append(',');
            sb.Append(enumerator.Current);
        }
        //return sb.ToString();

        return string.Join(',', Run(a));
    }

    public override async Task<long> Part2()
    {
        //return 0L;
        ParseNumber(await reader.ReadLineAsync());
        var b = ParseNumber(await reader.ReadLineAsync());
        var c = ParseNumber(await reader.ReadLineAsync());
        await reader.ReadLineAsync();

        var numbers = ParseNumbers(await reader.ReadLineAsync()).ToArray();

        long minimum = 0;
        var cts = new CancellationTokenSource();

        var stopwatch = Stopwatch.StartNew();

        for (var a = 0L;; ++a)
        {
            if ((int)a == 0L)
            {
                Console.WriteLine($"Checking at {a / stopwatch.Elapsed.TotalSeconds:#,##0} /s");
            }

            //                      2,  4,  1,  4,  7,  5,  4,  1,  1,  4,  5,  5,  0,  3,  3,  0
            const long target = 0b010_100_001_100_111_101_100_001_001_100_101_101_000_011_011_000;
            if (Run2(a) is target)
            {
                return a;
            }
        }
        
        var tasks = Enumerable.Range(0, Environment.ProcessorCount)
            .Select(_ => FindGood(cts.Token))
            .ToArray();

        await Task.WhenAny(tasks);
        await cts.CancelAsync();
        return (await Task.WhenAll(tasks)).Min();

        async Task<long> FindGood(CancellationToken ct)
        {
            await Task.Yield();

            while (ct.IsCancellationRequested is false)
            {
                var a = Interlocked.Increment(ref minimum);
                if ((int)a == 0L)
                {
                    Console.WriteLine($"Checking at {a / stopwatch.Elapsed.TotalSeconds:#,##0} /s");
                }

                //                      2,  4,  1,  4,  7,  5,  4,  1,  1,  4,  5,  5,  0,  3,  3,  0
                const long target = 0b010_100_001_100_111_101_100_001_001_100_101_101_000_011_011_000;
                if (Run2(a) is target)
                {
                    return a;
                }
                continue;
                var e = new CompiledEnumerator(a);
                for (
                    var i = 0;
                    ct.IsCancellationRequested is false && i < numbers.Length;
                    i++)
                {
                    if (e.MoveNext() is false || e.Current != numbers[i])
                    {
                        break;
                    }

                    if (i == numbers.Length - 1)
                    {
                        return a;
                    }
                }
            }

            return long.MaxValue;
        }
    }

    private static IEnumerable<byte> Run(long a)
    { 
        do
        {
            yield return (byte)(( (0b111 & a)
                                  ^ (a >> (int)((0b111 & a) ^ 0b100)))
                                & 0b111);

            // 0,3
            a >>= 3;

            // 3,0
        } while (a != 0);
    }

    public static long Run2(long a)
    {
        var res = 0L;
        do
        {
            res <<= 3;
            res |= (byte)(( (0b111 & a)
                                  ^ (a >> (int)((0b111 & a) ^ 0b100)))
                                & 0b111);

            // 0,3
            a >>= 3;

            // 3,0
        } while (a != 0);

        return res;
    }

    private struct CompiledEnumerator(long a)
    {
        private long a = a;
        public byte Current { get; private set; }

        public bool MoveNext()
        {
            if (this.a is 0L)
            {
                return false;
            }

            this.Current = (byte)(0b100 >> (int)(0b111 & this.a));
            this.a >>= 3;
            return true;
        }
    }

    private struct Enumerator(long a, long b, long c, IReadOnlyList<byte> instructions) : IEnumerator<byte>
    {
        private int instructionPointer = 0;
        private long a = a;
        private long b = b;
        private long c = c;

        public void Dispose() { }

        private int ComboOperand() =>
            this.LiteralOperand() switch
            {
                4 => (int)this.a,
                5 => (int)this.b,
                6 => (int)this.c,
                var value => value,
            };

        private byte LiteralOperand() => instructions[this.instructionPointer + 1];


        public bool MoveNext()
        {
            while (this.instructionPointer < instructions.Count - 1)
            {
                switch (instructions[this.instructionPointer])
                {
                    case 0:
                        this.a >>= this.ComboOperand();
                        break;
                    case 1:
                        this.b ^= this.LiteralOperand();
                        break;
                    case 2:
                        this.b = 0x7 & this.ComboOperand();
                        break;
                    case 3:
                        if (this.a is not 0L)
                        {
                            this.instructionPointer = this.LiteralOperand();
                            continue;
                        }
                        break;
                    case 4:
                        this.b ^= this.c;
                        break;
                    case 5:
                        this.Current = (byte)(this.ComboOperand() & 0x7);
                        this.instructionPointer += 2;
                        return true;
                    case 6:
                        this.b = this.a >> this.ComboOperand();
                        break;
                    case 7:
                        this.c = this.a >> this.ComboOperand();
                        break;
                }

                this.instructionPointer += 2;
            }

            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public byte Current { get; private set; }

        object IEnumerator.Current => this.Current;
    }

    private static long ParseNumber(ReadOnlySpan<char> line) =>
        long.Parse(line[line.IndexOfAny(Digits)..]);

    private static IEnumerable<byte> ParseNumbers(string? line)
    {
        var firstIndex = line.AsSpan().IndexOfAny(Digits);
        for (var start = firstIndex; start < line?.Length;)
        {
            if (line.AsSpan(start).IndexOf(',') is not (> 0 and var numberLength))
            {
                yield return byte.Parse(line.AsSpan(start));
                yield break;
            }

            yield return byte.Parse(line.AsSpan(start, numberLength));
            start += numberLength + 1;
        }
    }
}