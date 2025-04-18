namespace Aoc24.Test;

public class Day17Test
{
    private const string LongExample =
        """
        Register A: 729
        Register B: 0
        Register C: 0

        Program: 0,1,5,4,3,0
        """;

    private const string Example2 =
        """
        10
        0
        0

        5,0,5,1,5,4
        """;

    private const string Example3 =
        """
        2024
        0
        0

        0,1,5,4,3,0 
        """;

    [Test]
    //[Arguments(Example2, "0,1,2")]
    //[Arguments(Example3, "4,2,5,6,7,7,7,7,3,1,0")]
    //[Arguments(LongExample, "4,6,3,5,6,3,5,2,1,0")]
    [Arguments("""
    Register A: 25986278
    Register B: 0
    Register C: 0
    
    Program: 2,4,1,4,7,5,4,1,1,4,5,5,0,3,3,0
    """, "7,0,7,3,4,1,3,0,1")]
    public async Task Part1(string input, string expected)
    {
        // Arrange
        var day17 = new Day17(new StringReader(input));

        // Act
        var part1 = await day17.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }

    private const string Part2Example =
        """
        Register A: 2024
        Register B: 0
        Register C: 0

        Program: 0,3,5,4,3,0
        """;

    [Test]
    public async Task Part2()
    {
        return;
        // Arrange
        var day17 = new Day17(new StringReader(Part2Example));

        // Act
        var part2 = await day17.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(117_440);
    }

    private const long target = 0b010_100_001_100_111_101_100_001_001_100_101_101_000_011_011_000;

    private static readonly IReadOnlyList<byte> targetInstructions = [2, 4, 1, 4, 7, 5, 4, 1, 1, 4, 5, 5, 0, 3, 3, 0];

    [Test]
    public async Task AsdF()
    {
        await Assert.That(target.Split3().SequenceEqual(targetInstructions)).IsTrue();
    }
    
    [Test]
    public async Task Run2()
    {
        for (var i = 3; i < 31; i += 3)
        {
            var best = Enumerable.Range(0, int.MaxValue >> i)
                .Select(a => (a, Day17.Run2(a)))
                .Select(t => (t.a, t.Item2,
                    targetInstructions.Zip(t.Item2.Split3()).TakeWhile(tt => tt.First == tt.Second).Count()))
                .MaxBy(t => t.Item3);
            Console.WriteLine($"{best.a:b} {string.Join(',', best.Item2.Split3())}, {best.Item3}");

            best = Enumerable.Range(0, int.MaxValue >> i)
                .Select(a => (a, Day17.Run2(a)))
                .Select(t => (t.a, t.Item2,
                    targetInstructions.Reverse().Zip(t.Item2.Split3().Reverse()).TakeWhile(tt => tt.First == tt.Second)
                        .Count()))
                .MaxBy(t => t.Item3);
            Console.WriteLine($"{best.a:b} {string.Join(',', best.Item2.Split3())}, {best.Item3}");
        }
    }
}

file static class Extensions
{
    public static IEnumerable<byte> Split3(this long l)
    {
        var stack = new Stack<byte>();

        while (l > 0)
        {
            stack.Push((byte)(l & 0b111));
            l >>= 3;
        }

        return stack;
    }

    public static long Combine(this IEnumerable<byte> bytes) =>
        bytes.Aggregate(0L, (a, b) => (a << 3) | b);
}