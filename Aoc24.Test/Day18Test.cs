namespace Aoc24.Test;

public class Day18Test
{
    [Test]
    public async Task Dijkstra7By7()
    {
        var x =
            """
            ...#...
            ..#..#.
            ....#..
            ...#..#
            ..#..#.
            .#..#..
            #.#....
            """;

        var map = new Map(7, 7)
        {
            [4, 5] = -2,
            [2, 4] = -2,
            [5, 4] = -2,
            [0, 3] = -2,
            [1, 2] = -2,
            [3, 6] = -2,
            [4, 2] = -2,
            [5, 1] = -2,
            [6, 0] = -2,
            [3, 3] = -2,
            [6, 2] = -2,
            [1, 5] = -2,
        };

        var distance = map.Find<Map, int>((0, 0), (6, 6));

        Console.WriteLine(map.ToString());

        await Assert.That(distance).IsEqualTo(22);
    }
}

