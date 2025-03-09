namespace Aoc24.Test;

public class Day09Test
{
    [Test]
    public async Task Part1_Example1()
    {
        // Arrange
        var day09 = new Day09(new StringReader("2333133121414131402"));

        // Act
        var part1 = await day09.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(1928ul);
    }

    [Test]
    public async Task Part1_Example2()
    {
        // Arrange
        var day09 = new Day09(new StringReader("12345"));
        const string compacted = "022111222";
        var expected = (ulong)compacted.Select((c, index) => (c - '0') * index).Sum();

        // Act
        var part1 = await day09.Part1();

        // Assert
        await Assert.That(part1).IsEqualTo(expected);
    }
    
    
    [Test]
    public async Task Part2_Example1()
    {
        // Arrange
        var day09 = new Day09(new StringReader("2333133121414131402"));

        // Act
        var part2 = await day09.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(2858ul);
    }

    [Test]
    public async Task Part2_Example2()
    {
        // Arrange
        var day09 = new Day09(new StringReader("12345"));
        const string compacted = "0..111....22222";
        var expected = (ulong)compacted.Select((c, index) => c is '.' ? 0 : (c - '0') * index).Sum();

        // Act
        var part2 = await day09.Part2();

        // Assert
        await Assert.That(part2).IsEqualTo(expected);
    }
}