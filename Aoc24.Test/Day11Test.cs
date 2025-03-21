namespace Aoc24.Test;

public class Day11Test
{
    [Test]
    public async Task Part1()
    {
        // Arrange
        var day11 = new Day11(new StringReader("125 17"));

        // Act
        var part1 = await day11.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(55_312ul);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        var day11 = new Day11(new StringReader("125 17"));

        // Act
        var part2 = await day11.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(65_601_038_650_482ul);
    }
}