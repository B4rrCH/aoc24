using Aoc24.Solutions;
using FluentAssertions;

namespace Aoc24.Test;

public class Day03Test
{
    [Fact]
    public async Task Part1()
    {
        // Arrange
        const string exampleInput = 
            "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        using var reader = new StringReader(exampleInput);
        var day03 = new Day03(reader);

        // Act
        var part1 = await day03.Part1();

        // Assert
        part1.Should().Be(161);
    }

    [Fact]
    public async Task Part2()
    {
        // Arrange
        const string exampleInput =
            "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))\n";
        using var reader = new StringReader(exampleInput);
        var day03 = new Day03(reader);

        // Act
        var part1 = await day03.Part2();

        // Assert
        part1.Should().Be(48);
    }
}
