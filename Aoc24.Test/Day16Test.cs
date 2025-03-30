namespace Aoc24.Test;

public class Day16Test
{
    public const string Example1 = """
        ###############
        #.......#....E#
        #.#.###.#.###.#
        #.....#.#...#.#
        #.###.#####.#.#
        #.#.#.......#.#
        #.#.#####.###.#
        #...........#.#
        ###.#.#####.#.#
        #...#.....#.#.#
        #.#.#.###.#.#.#
        #.....#...#.#.#
        #.###.#.#.#.#.#
        #S..#.....#...#
        ###############
        """;

    private const string Example2 = """
        #################
        #...#...#...#..E#
        #.#.#.#.#.#.#.#.#
        #.#.#.#...#...#.#
        #.#.#.#.###.#.#.#
        #...#.#.#.....#.#
        #.#.#.#.#.#####.#
        #.#...#.#.#.....#
        #.#.#####.#.###.#
        #.#.#.......#...#
        #.#.###.#####.###
        #.#.#...#.....#.#
        #.#.#.#####.###.#
        #.#.#.........#.#
        #.#.#.#########.#
        #S#.............#
        #################
        """;

    [Test]
    [Arguments("SE", 1)]
    [Arguments("E\nS", 1001)]
    [Arguments("S\nE", 1001)]
    [Arguments("ES", 2001)]
    [Arguments(Example1, 7036)]
    [Arguments(Example2, 11048)]
    public async Task Part1(string maze, int expected)
    {
        // Arrange
        var day16 = new Day16(new StringReader(maze));

        // Act
        var part1 = await day16.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }

    [Test]
    [Arguments(Example1, 45)]
    [Arguments(Example2, 64)]
    public async Task Part2(string maze, int expected)
    {
        // Arrange
        var day16 = new Day16(new StringReader(maze));

        // Act
        var part2 = await day16.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(expected);
    }
}
