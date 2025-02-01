namespace Aoc24.Test;

public class Day03Test
{
    [Test]
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
        await Assert.That(part1).IsEqualTo(161);
    }

    [Test]
    public async Task Part2()
    {
        // Arrange
        const string exampleInput =
            "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))\n";
        using var reader = new StringReader(exampleInput);
        var day03 = new Day03(reader);

        // Act
        var part2 = await day03.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(48);
    }
}
