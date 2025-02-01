namespace Aoc24.Test;

public class Day01Test
{
    [Test]
    public async Task Part1()
    {
        // Arrange
        var input = new StringReader(
            """
            3   4
            4   3
            2   5
            1   3
            3   9
            3   3
            """);
        Day01 day01 = new(input);

        // Act
        var part1 = await day01.Part1();
        
        // Assert
        await Assert.That(part1).IsEqualTo(11);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        var input = new StringReader(
            """
            3   4
            4   3
            2   5
            1   3
            3   9
            3   3
            """);
        Day01 day01 = new(input);

        // Act
        var part2 = await day01.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(31);
    }
}
