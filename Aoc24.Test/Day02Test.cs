namespace Aoc24.Test;

public class Day02Test
{
    [Test]
    public async Task Part1()
    {
        // Arrange
        const string exampleInput = """
            7 6 4 2 1
            1 2 7 8 9
            9 7 6 2 1
            1 3 2 4 5
            8 6 4 4 1
            1 3 6 7 9
            """;
        using var reader = new StringReader(exampleInput);
        var day02 = new Day02(reader);

        // Act
        var part1 = await day02.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(2);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        const string exampleInput = """
            7 6 4 2 1
            1 2 7 8 9
            9 7 6 2 1
            1 3 2 4 5
            8 6 4 4 1
            1 3 6 7 9
            """;
        using var reader = new StringReader(exampleInput);
        var day02 = new Day02(reader);

        // Act
        var part2 = await day02.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(4);
    }
}
