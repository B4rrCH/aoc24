using Aoc24.Solutions;
using FluentAssertions;

namespace Aoc24.Test;

public class Day01Test
{
    [Fact]
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
        part1.Should().Be(11);
    }

    [Fact]
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
        part2.Should().Be(31);
    }
}
