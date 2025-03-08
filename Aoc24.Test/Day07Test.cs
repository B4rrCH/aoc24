namespace Aoc24.Test;

public class Day07Test
{
    private const string ExampleInput = """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """;

    [Test]
    public async Task Part1()
    {
        // Arrange
        var day07 = new Day07(new StringReader(ExampleInput));

        // Act
        var result = await day07.Part1();

        // Assert
        await Assert.That(result).IsEqualTo(3749);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        var day07 = new Day07(new StringReader(ExampleInput));

        // Act
        var result = await day07.Part2();

        // Assert
        await Assert.That(result).IsEqualTo(11387);
    }
}
