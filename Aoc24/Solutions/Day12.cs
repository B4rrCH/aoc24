using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day12(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day12>
{
    public static Day12 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var map = await reader.ReadTo2DArrayAsync();
        return GetPlots(map).Sum(plot => plot.Area.Count * GetPerimeter(plot));
    }

    public override async Task<int> Part2()
    {
        var map = await reader.ReadTo2DArrayAsync();
        return GetPlots(map).Sum(plot => plot.Area.Count * CountSides(plot));
    }

    private static IEnumerable<(char Plant, HashSet<Position> Area)> GetPlots(char[,] map)
    {
        HashSet<Position> seen = [];
        for (var x = 0; x < map.GetLength(0); x++)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                var position = new Position(x, y);
                if (seen.Contains(position))
                {
                    continue;
                }

                var plot = GetPlot(map, position);
                seen.UnionWith(plot.Area);
                yield return plot;
            }
        }
    }

    private static (char Plant, HashSet<Position> Area) GetPlot(char[,] map, Position around)
    {
        var plant = map[around.X, around.Y];
        HashSet<Position> area = [around];
        var toVisit = new Stack<Position>();
        toVisit.Push(around);

        while (toVisit.Count > 0)
        {
            var current = toVisit.Pop();
            foreach (var neighbour in current.Neighbours.Where(n => n.Value(map) == plant && area.Add(n)))
            {
                toVisit.Push(neighbour);
            }
        }

        return (plant, area);
    }

    private static int GetPerimeter((char Plant, HashSet<Position> Area) plot) =>
        plot.Area.Select(p => p.Neighbours.Count(candidate => plot.Area.Contains(candidate) is false)).Sum();

    private static int CountSides((char Plant, HashSet<Position> Area) plot)
    {
        var borderCount = 0;
        var seenFences = new HashSet<(Position Inside, Position Outside)>();
        foreach (var position in plot.Area)
        {
            foreach (var candidate in position.Neighbours)
            {
                if (plot.Area.Contains(candidate) || seenFences.Contains((position, candidate)))
                {
                    continue;
                }

                var direction = candidate - position;
                var borderDirection = new Direction(direction.Y, -direction.X);

                for (var (inside, outside) = (position, candidate);
                     plot.Area.Contains(inside) && plot.Area.Contains(outside) is false;
                     (inside, outside) = (inside + borderDirection, outside + borderDirection))
                {
                    seenFences.Add((inside, outside));
                }

                for (var (inside, outside) = (position, candidate);
                     plot.Area.Contains(inside) && plot.Area.Contains(outside) is false;
                     (inside, outside) = (inside - borderDirection, outside - borderDirection))
                {
                    seenFences.Add((inside, outside));
                }

                ++borderCount;
            }
        }

        return borderCount;
    }

    private readonly record struct Position(int X, int Y)
    {
        public IEnumerable<Position> Neighbours =>
        [
            this + Direction.Up,
            this + Direction.Right,
            this + Direction.Down,
            this + Direction.Left,
        ];

        public char? Value(char[,] map) =>
            0 <= this.X
            && this.X < map.GetLength(0)
            && 0 <= this.Y
            && this.Y < map.GetLength(1)
                ? map[this.X, this.Y]
                : null;

        public static Position operator +(Position a, Direction b) => new(a.X + b.Dx, a.Y + b.Dy);

        public static Position operator -(Position a, Direction b) => new(a.X - b.Dx, a.Y - b.Dy);

        public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);

        public static implicit operator Position((int, int) tuple) => new(tuple.Item1, tuple.Item2);
    }

    private readonly record struct Direction(int Dx, int Dy)
    {
        public static Direction Up => new(-1, 0);
        public static Direction Down => new(1, 0);
        public static Direction Left => new(0, -1);
        public static Direction Right => new(0, 1);
    }
}