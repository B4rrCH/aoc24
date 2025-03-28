namespace Aoc24.Test;

public class Day13Test
{
    private const string Machine1 =
        """
        Button A: X+94, Y+34
        Button B: X+22, Y+67
        Prize: X=8400, Y=5400
        """;

    private const string Machine2 =
        """
        Button A: X+26, Y+66
        Button B: X+67, Y+21
        Prize: X=12748, Y=12176
        """;

    private const string Machine3 =
        """
        Button A: X+17, Y+86
        Button B: X+84, Y+37
        Prize: X=7870, Y=6450
        """;

    private const string Machine4 =
        """
        Button A: X+69, Y+23
        Button B: X+27, Y+71
        Prize: X=18641, Y=10279
        """;

    private const string AllMachines =
        $"""
        {Machine1}

        {Machine2}

        {Machine3}

        {Machine4}
        """;

    [Test]
    [Arguments(Machine1, 280)]
    [Arguments(Machine2, 0)]
    [Arguments(Machine3, 200)]
    [Arguments(Machine4, 0)]
    [Arguments(AllMachines, 480)]
    public async Task Part1(string machines, int expected)
    {
        // Arrange
        var day13 = new Day13(new StringReader(machines));

        // Act
        var part1 = await day13.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }

    [Test]
    [Arguments(Machine1, 0)]
    [Arguments(Machine2, 459_236_326_669L)]
    [Arguments(Machine3, 0)]
    [Arguments(Machine4, 416_082_282_239L)]
    [Arguments(AllMachines, 875_318_608_908L)]
    public async Task Part2(string machines, long expected)
    {
        // Arrange
        var day13 = new Day13(new StringReader(machines));

        // Act
        var part2 = await day13.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(expected);
    }
}
