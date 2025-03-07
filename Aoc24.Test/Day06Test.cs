namespace Aoc24.Test;

public class Day06Test
{
    private const string ExampleInput =
        """
        ....#.....
        .........#
        ..........
        ..#.......
        .......#..
        ..........
        .#..^.....
        ........#.
        #.........
        ......#...
        """;

    [Test]
    public async Task Part1()
    {
        // Arrange
        var day06 = new Day06(new StringReader(ExampleInput));

        // Act
        var part1 = await day06.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(41);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        var day06 = new Day06(new StringReader(ExampleInput));

        // Act
        var part2 = await day06.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(6);
    }
}