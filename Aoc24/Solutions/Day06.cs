using System.Buffers;
using System.Collections.Immutable;
using Aoc24.IO;
using Microsoft.Extensions.ObjectPool;

namespace Aoc24.Solutions;

public class Day06(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day06>
{
    private static readonly DefaultObjectPool<HashSet<(Position, Direction)>> SetPool =
        HashSetPool.Create<(Position, Direction)>();

    public static Day06 Construct(TextReader reader) => new(reader);

    public override async Task<int> Part1()
    {
        var map = Map.Parse(await reader.ReadToGrid());
        return SeenUntilExit(map)?.Count ?? throw new InvalidOperationException("Guard does not exit");
    }

    public override async Task<int> Part2()
    {
        var map = Map.Parse(await reader.ReadToGrid());
        return CountLoopsWhenObstructionsAdded(map);
    }

    private static HashSet<Position>? SeenUntilExit(Map map)
    {
        var (height, width, obstructions, (position, direction)) = map;

        var positions = new HashSet<Position>();
        var seen = new HashSet<(Position, Direction)>();

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
                direction = TurnRight(direction);
            }
            else
            {
                position = nextPosition;
            }
        }
    }

    private static int CountLoopsWhenObstructionsAdded(Map map)
    {
        var (height, width, obstructions, (position, direction)) = map;

        HashSet<Position> addedObstructions = [position];
        var seen = new HashSet<(Position, Direction)>();

        var loopCount = 0;
        while (true)
        {
            if (seen.Add((position, direction)) is false)
            {
                throw new InvalidOperationException("Guard does not exit.");
            }

            var nextPosition = position + direction;
            if (nextPosition.X < 0 || height <= nextPosition.X || nextPosition.Y < 0 || width <= nextPosition.Y)
            {
                break;
            }

            if (obstructions.Contains(nextPosition))
            {
                direction = TurnRight(direction);
                continue;
            }

            if (addedObstructions.Add(nextPosition)
                && DoesLoop(
                    map with
                    {
                        Start = (position, direction),
                        Obstructions = obstructions.Add(nextPosition),
                    },
                    seen))
            {
                ++loopCount;
            }
            position = nextPosition;
        }

        return loopCount;
    }

    private static bool DoesLoop(Map map, HashSet<(Position, Direction)> seenBefore)
    {
        var (height, width, obstructions, (position, direction)) = map;
        var seenHere = SetPool.Get();

        while (true)
        {
            var nextPosition = position + direction;
            if (nextPosition.X < 0 || height <= nextPosition.X || nextPosition.Y < 0 || width <= nextPosition.Y)
            {
                SetPool.Return(seenHere);
                return false;
            }

            (position, direction) = obstructions.Contains(nextPosition)
                ? (position, TurnRight(direction))
                : (nextPosition, direction);

            if (seenBefore.Contains((position, direction)) ||
                seenHere.Add((position, direction)) is false)
            {
                SetPool.Return(seenHere);
                return true;
            }
        }
    }

    private readonly record struct Position(int X, int Y)
    {
        public static Position operator +(Position pos, Direction dir) =>
            dir switch
            {
                Direction.Up => (pos.X - 1, pos.Y),
                Direction.Down => (pos.X + 1, pos.Y),
                Direction.Left => (pos.X, pos.Y - 1),
                Direction.Right => (pos.X, pos.Y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };

        public static implicit operator Position((int X, int Y) a) => new(a.X, a.Y);
    }

    private static Direction TurnRight(Direction direction) =>
        direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    private readonly record struct Map(
        int Height,
        int Width,
        ImmutableHashSet<Position> Obstructions,
        (Position Postion, Direction Direction) Start)
    {
        private static readonly SearchValues<char> Interesting = SearchValues.Create("^v<>#");
        public static Map Parse(char[][] map)
        {
            if (map.Length == 0)
            {
                throw new InvalidOperationException("Map is empty.");
            }

            var height = map.Length;
            var width = map[0].Length;

            var obstructions = ImmutableHashSet.CreateBuilder<Position>();
            (Position Postion, Direction Direction)? start = null;

            foreach (var (x, chars) in map.Index())
            {
                var line = chars.AsSpan();
                var y = 0;

                while (line[y..].IndexOfAny(Interesting) is var offset and >= 0)
                {
                    y += offset;
                    switch (line[y])
                    {
                        case '#':
                            obstructions.Add((x, y));
                            break;
                        case '^':
                            start = ((x, y), Direction.Up);
                            break;
                        case 'v':
                            start = ((x, y), Direction.Down);
                            break;
                        case '<':
                            start = ((x, y), Direction.Left);
                            break;
                        case '>':
                            start = ((x, y), Direction.Right);
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