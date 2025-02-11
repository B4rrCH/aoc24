using System.Buffers;
using System.Collections.Immutable;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day06(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day06>
{
    public static Day06 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var map = Map.Parse(await reader.ReadToGrid());
        return SeenUntilExit(map)?.Count ?? throw new InvalidOperationException("Guard does not exit");
    }

    public override async Task<int> Part2()
    {
        var map = Map.Parse(await reader.ReadToGrid());
        var seenUntilExit = SeenUntilExit(map) ?? throw new InvalidOperationException("Guard does not exit");
        // TODO: improve backtracking
        return seenUntilExit.Except([map.Start.Postion])
            .Select(newObstruction => map with { Obstructions = map.Obstructions.Add(newObstruction) })
            .AsParallel()
            .Count(newMap => SeenUntilExit(newMap) is null);
    }

    private static HashSet<Position>? SeenUntilExit(Map map)
    {
        var (height, width, obstructions, (position, direction)) = map;

        var positions = new HashSet<Position>();
        var seen = new HashSet<(Position, Position)>();

        while (true)
        {
            positions.Add(position);
            if (seen.Add((position, direction)) is false)
            {
                return null;
            }

            var nextPosition = position + direction;
            if (nextPosition.X < 0 || height <= nextPosition.X || nextPosition.Y < 0 || width <= nextPosition.Y)
            {
                return positions;
            }

            if (obstructions.Contains(nextPosition))
            {
                direction = direction.TurnRight();
            }
            else
            {
                position = nextPosition;
            }
        }
    }

    private readonly record struct Position(int X, int Y)
    {
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        public static implicit operator Position((int X, int Y) a) => new(a.X, a.Y);
        public static Position Up => (-1, 0);
        public static Position Down => (1, 0);
        public static Position Left => (0, -1);
        public static Position Right => (0, 1);
        public Position TurnRight() => (this.Y, -this.X);
    }

    private readonly record struct Map(
        int Height,
        int Width,
        ImmutableHashSet<Position> Obstructions,
        (Position Postion, Position Direction) Start)
    {
        public static Map Parse(char[][] map)
        {
            if (map.Length == 0)
            {
                throw new InvalidOperationException("Map is empty.");
            }

            var height = map.Length;
            var width = map[0].Length;

            var obstructions = ImmutableHashSet.CreateBuilder<Position>();
            (Position Postion, Position Direction)? start = null;

            var interesting = SearchValues.Create("^v<>#");

            foreach (var (x, chars) in map.Index())
            {
                var line = chars.AsSpan();
                var y = 0;

                while (line[y..].IndexOfAny(interesting) is var offset and >= 0)
                {
                    y += offset;
                    switch (line[y])
                    {
                        case '#':
                            obstructions.Add((x, y));
                            break;
                        case '^':
                            start = ((x, y), Position.Up);
                            break;
                        case 'v':
                            start = ((x, y), Position.Down);
                            break;
                        case '<':
                            start = ((x, y), Position.Left);
                            break;
                        case '>':
                            start = ((x, y), Position.Right);
                            break;
                    }

                    ++y;
                }
            }

            return new Map(
                height,
                width,
                obstructions.ToImmutable(),
                start ?? throw new InvalidOperationException("Start position not found."));
        }
    }
}