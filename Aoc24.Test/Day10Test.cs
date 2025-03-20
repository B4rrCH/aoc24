namespace Aoc24.Test;

public class Day10Test
{
    [Test]
    [Arguments("""
        0123
        1234
        8765
        9876
        """, 1)]
    [Arguments("""
        10..9..
        2...8..
        3...7..
        4567654
        ...8..3
        ...9..2
        .....01
        """, 3)]
    [Arguments("""
        89010123
        78121874
        87430965
        96549874
        45678903
        32019012
        01329801
        10456732
        """, 36)]
    public async Task Part1(string input, int expected)
    {
        var day10 = new Day10(new StringReader(input));

        var part1 = await day10.Part1();

        await Assert.That(part1).IsEqualTo(expected);
    }

    [Test]
    [Arguments("""
        .....0.
        ..4321.
        ..5..2.
        ..6543.
        ..7..4.
        ..8765.
        ..9....
        """, 3)]
    [Arguments("""
        ..90..9
        ...1.98
        ...2..7
        6543456
        765.987
        876....
        987....
        """, 13)]
    [Arguments("""
        012345
        123456
        234567
        345678
        4.6789
        56789.
        """, 227)]
    public async Task Part2(string input, int expected)
    {
        var day10 = new Day10(new StringReader(input));

        var part2 = await day10.Part2();

        await Assert.That(part2).IsEqualTo(expected);
    }
}