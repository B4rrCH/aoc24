namespace Aoc24.Test;

public class Day05Test
{
    private const string ExampleInput = """
        47|53
        97|13
        97|61
        97|47
        75|29
        61|13
        75|53
        29|13
        97|29
        53|29
        61|53
        97|53
        61|29
        47|13
        75|47
        97|75
        47|61
        75|61
        47|29
        75|13
        53|13

        75,47,61,53,29
        97,61,53,29,13
        75,29,13
        75,97,47,61,53
        61,13,29
        97,13,75,29,47
        """;

    [Test]
    public async Task Part1()
    {
        // Arrange
        var day05 = new Day05(new StringReader(ExampleInput));

        // Act
        var part1 = await day05.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(143);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        var day05 = new Day05(new StringReader(ExampleInput));

        // Act
        var part2 = await day05.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(123);
    }
}