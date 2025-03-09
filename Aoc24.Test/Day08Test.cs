namespace Aoc24.Test;

public class Day08Test
{
    private const string ExampleInput = """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """;

    [Test]
    public async Task Part1()
    {
        // Arrange
        var day08 = new Day08(new StringReader(ExampleInput));

        // Act
        var part1 = await day08.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(14);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        var day08 = new Day08(new StringReader(ExampleInput));

        // Act
        var part1 = await day08.Part2();

        // Assert
        await Assert.That(part1).IsEqualTo(34);
    }
}
