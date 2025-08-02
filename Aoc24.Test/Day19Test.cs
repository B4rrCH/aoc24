namespace Aoc24.Test;

public class Day19Test
{
    [Test]
    [Arguments("""
    r
    
    r
    b
    """, 1)]
    [Arguments(
        """
        r, wr, b, g, bwu, rb, gb, br

        brwrr
        bggr
        gbbr
        rrbgbr
        ubwu
        bwurrg
        brgr
        bbrgwb
        """, 6)]
    public async Task Part1(string example, int expected)
    {
        // Arrange
        var day19 = new Day19(new StringReader(example));

        // Act
        var result = await day19.Part1();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("""
        r

        r
        b
        """, 1)]
    [Arguments("""
        r, rr

        rr
        """, 2)]
    [Arguments(
        """
        r, wr, b, g, bwu, rb, gb, br

        brwrr
        bggr
        gbbr
        rrbgbr
        ubwu
        bwurrg
        brgr
        bbrgwb
        """, 16)]
    public async Task Part2(string example, int expected)
    {
        // Arrange
        var day19 = new Day19(new StringReader(example));

        // Act
        var result = await day19.Part2();

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}
