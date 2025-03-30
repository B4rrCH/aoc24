namespace Aoc24.Test;

public class Day15Test
{
    private const string Example1 =
        """
        ##########
        #..O..O.O#
        #......O.#
        #.OO..O.O#
        #..O@..O.#
        #O#..O...#
        #O..O..O.#
        #.OO.O.OO#
        #....O...#
        ##########

        <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
        vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
        ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
        <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
        ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
        ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
        >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
        <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
        ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
        v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
        """;

    private const string Example2 =
        """
        ########
        #..O.O.#
        ##@.O..#
        #...O..#
        #.#.O..#
        #...O..#
        #......#
        ########

        <^^>>>vv<v>>v<<
        """;

    private const string EmptyExample =
        """
        @


        """;

    private const string NoMoveExample =
        """
        #######
        #...O@.
        #......

        
        """;

    private const string SimpleExample =
        """
        #####
        # O@#
        #####

        <
        """;

    private const string TwoExample =
        """
        #######
        #..OO@.
        #......

        <<
        """;

    [Test]
    [Arguments(EmptyExample, 0)]
    [Arguments(NoMoveExample, 104)]
    [Arguments(SimpleExample, 101)]
    [Arguments(TwoExample, 101 + 102)]
    [Arguments(Example1, 10_092)]
    [Arguments(Example2, 2028)]
    public async Task Part1(string input, int expected)
    {
        // Arrange
        var day15 = new Day15(new StringReader(input));

        // Act
        var part1 = await day15.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }

    [Test]
    [Arguments(EmptyExample, 0)]
    [Arguments(NoMoveExample, 108)]
    [Arguments("..OO@\n\n<<", 2 + 4)]
    [Arguments(TwoExample, 104 + 106)]
    [Arguments(TwoExample + "<", 103 + 105)]
    [Arguments(Example1, 9021)]
    public async Task Part2(string input, int expected)
    {
        // Arrange
        var day15 = new Day15(new StringReader(input));

        // Act
        var part2 = await day15.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(expected);
    }
}
