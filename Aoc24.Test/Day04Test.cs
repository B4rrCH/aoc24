namespace Aoc24.Test;

public class Day04Test
{
    [Test]
    [Arguments("""
        ..X...
        .SAMX.
        .A..A.
        XMAS.S
        .X....
        """, 4)]
    [Arguments("""
        MMMSXXMASM
        MSAMXMSMSA
        AMXSXMAAMM
        MSAMASMSMX
        XMASAMXAMM
        XXAMMXXAMA
        SMSMSASXSS
        SAXAMASAAA
        MAMMMXMMMM
        MXMXAXMASX
        """, 18)]
    public async Task Part1(string exampleInput, int expected)
    {
        // Arrange
        using var reader = new StringReader(exampleInput);
        var day04 = new Day04(reader);

        // Act
        var part1 = await day04.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }

    [Test]
    [Arguments("""
        M.S
        .A.
        M.S
        """, 1)]
    [Arguments("""
        .M.S......
        ..A..MSMS.
        .M.S.MAA..
        ..A.ASMSM.
        .M.S.M....
        ..........
        S.S.S.S.S.
        .A.A.A.A..
        M.M.M.M.M.
        ..........
        """, 9)]
    public async Task Part2(string exampleInput, int expected)
    {
        // Arrange
        using var reader = new StringReader(exampleInput);
        var day04 = new Day04(reader);

        // Act
        var part2 = await day04.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(expected);
    }
}
