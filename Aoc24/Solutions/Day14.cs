using System.Buffers;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day14(TextReader reader, int width, int height, int seconds, TextWriter? printTree) : SolutionBase<int, int>, IConstructFromReader<Day14>
{
    private static readonly SearchValues<char> NumericCharacters = SearchValues.Create("-0123456789");

    public static Day14 Construct(TextReader reader) => new(reader, 101, 103, 100, null);
    public override async Task<int> Part1()
    {
        var positionsAfterTime = await reader.ReadLinesAsync()
            .Select(line => ParseRobot(line))
            .Select(
                robot =>
                {
                    var x = (robot.Position.X + seconds * robot.Velocity.X) % width;
                    if (x < 0)
                    {
                        x += width;
                    }

                    var y = (robot.Position.Y + seconds * robot.Velocity.Y) % height;
                    if (y < 0)
                    {
                        y += height;
                    }

                    return new Point(x, y);
                })
            .ToArrayAsync();

        if (printTree is not null)
        {
            await printTree.WriteAsync(DrawMap(positionsAfterTime, width, height));
        }

        return positionsAfterTime
            .GroupBy(
                position => (position.X - width / 2, position.Y - height / 2) switch
                {
                    (0, _) or (_, 0) => Quadrant.None,
                    (> 0, > 0) => Quadrant.One,
                    (> 0, < 0) => Quadrant.Two,
                    (< 0, > 0) => Quadrant.Three,
                    (< 0, < 0) => Quadrant.Four,
                })
            .Where(g => g.Key is not Quadrant.None)
            .Select(g => g.Count())
            .Aggregate(1, (a, b) => a * b);
    }

    public override async Task<int> Part2()
    {
        var robots = await reader.ReadLinesAsync()
            .Select(line => ParseRobot(line))
            .ToArrayAsync();

        // The map repeats after an insanely long time, i.e.
        // robots.Select(r => Lcm(Lcm(r.Velocity.X, width), Lcm(r.Velocity.Y, height))).Aggregate(Lcm)
        // but the AOC 2024 authors were nice, so this suffices
        var upperBound = width * height;

        var squareDistanceSums = new int[upperBound];
        var positions = robots.Select(r => r.Position).ToArray();
        var velocities = robots.Select(r => r.Velocity).ToArray();
        for (var time = 0 ; time < upperBound; ++time)
        {
            // The sought after tree happens to be drawn in the center of the picture
            // => heuristically the sum of distances to the center will be low
            squareDistanceSums[time] = positions.Sum(
                p =>
                {
                    var dx = p.X - width / 2;
                    var dy = p.Y - height / 2;
                    return dx * dx + dy * dy;
                });
            for (var i = 0; i < positions.Length; ++i)
            {
                var (x, y) = positions[i] + velocities[i];
                positions[i] = new Point((x + width) % width, (y+height) % height);
            }
        }
        var result = squareDistanceSums.Index().MinBy(i => i.Item).Index;
        if (printTree is null)
        {
            return result;
        }

        var positionsAsTree = robots.Select(
            robot =>
            {
                var x = (robot.Position.X + result * robot.Velocity.X) % width;
                if (x < 0)
                {
                    x += width;
                }

                var y = (robot.Position.Y + result * robot.Velocity.Y) % height;
                if (y < 0)
                {
                    y += height;
                }

                return new Point(x, y);
            });
        await printTree.WriteAsync(DrawMap(positionsAsTree, width, height));

        return result;
    }

    private enum Quadrant
    {
        None,
        One,
        Two,
        Three,
        Four,
    }

    private static Robot ParseRobot(ReadOnlySpan<char> text)
    {
        return new Robot(
            new Point(ParseNumber(ref text), ParseNumber(ref text)),
            new Direction(ParseNumber(ref text), ParseNumber(ref text)));

        static int ParseNumber(ref ReadOnlySpan<char> text)
        {
            var start = text.IndexOfAny(NumericCharacters);
            text = text[start..];
            var end = text.IndexOfAnyExcept(NumericCharacters);
            if (end < 0)
            {
                end = text.Length;
            }
            var result = int.Parse(text[..end]);
            text = text[end..];
            return result;
        }
    }

    private readonly record struct Point(int X, int Y)
    {
        public static Point operator +(Point point, Direction direction) =>
            new(point.X + direction.X, point.Y + direction.Y);
    }

    private readonly record struct Direction(int X, int Y);
    private readonly record struct Robot(Point Position, Direction Velocity);

    private static string DrawMap(IEnumerable<Point> positions, int width, int height) =>
        string.Create(
            height * (width + 1) - 1,
            positions,
            (span, points) =>
            {
                span.Fill(' ');
                for (var i = width; i < span.Length; i += width + 1)
                {
                    span[i] = '\n';
                }

                foreach (var (x, y) in points)
                {
                    span[(width + 1) * y + x] = '*';
                }
            });
}
