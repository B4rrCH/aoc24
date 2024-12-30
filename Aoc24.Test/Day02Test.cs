using Aoc24.Solutions;
using FluentAssertions;

namespace Aoc24.Test;

public class Day02Test
{
    [Fact]
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
        part1.Should().Be(2);
    }

    [Fact]
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
        var part1 = await day02.Part2();

        // Assert
        part1.Should().Be(4);
    }
}
