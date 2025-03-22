namespace Aoc24.Test;

public class Day12Test
{
    private const string Example1 =
        """
        AAAA
        BBCD
        BBCC
        EEEC
        """;

    private const string Example2 =
        """
        OOOOO
        OXOXO
        OOOOO
        OXOXO
        OOOOO
        """;

    private const string Example3 = """
        RRRRIICCFF
        RRRRIICCCF
        VVRRRCCFFF
        VVRCCCJFFF
        VVVVCJJCFE
        VVIVCCJJEE
        VVIIICJJEE
        MIIIIIJJEE
        MIIISIJEEE
        MMMISSJEEE
        """;

    private const string Example4 =
        """
        EEEEE
        EXXXX
        EEEEE
        EXXXX
        EEEEE
        """;

    private const string Example5 =
        """
        AAAAAA
        AAABBA
        AAABBA
        ABBAAA
        ABBAAA
        AAAAAA
        """;

    [Test]
    [Arguments("A", 1 * 1 * 4)]
    [Arguments("AA", 1 * 2 * 6)]
    [Arguments("AB", 2 * 1 * 4)]
    [Arguments(Example1, 140)]
    [Arguments(Example2, 772)]
    [Arguments(Example3, 1930)]
    public async Task Part1(string map, int expected)
    {
        // Arrange
        var day12 = new Day12(new StringReader(map));

        // Act
        var part1 = await day12.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }

    [Test]
    [Arguments("A", 1 * 1 * 4)]
    [Arguments("AA\nAA", 1 * 4 * 4)]
    [Arguments("AABB\nAABB", 2 * 4 * 4)]
    [Arguments("AB", 2 * 1 * 4)]
    [Arguments("AA", 1 * 2 * 4)]
    [Arguments("AABB", 2 * 2 * 4)]
    [Arguments(Example1, 80)]
    [Arguments(Example3, 1206)]
    [Arguments(Example4, 236)]
    [Arguments(Example5, 368)]

    public async Task Part2(string map, int expected)
    {
        // Arrange
        var day12 = new Day12(new StringReader(map));

        // Act
        var part2 = await day12.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(expected);
    }
}
