using System.Buffers;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day15(TextReader reader) : SolutionBase<long, long>, IConstructFromReader<Day15>
{
    public static Day15 Construct(TextReader reader) => new(reader);

    public override async Task<long> Part1()
    {
        var warehouse = await WarehousePart1.Parse(reader.ReadLinesAsync());

        await foreach (var direction in GetDirectionsAsync(reader))
        {
            warehouse.MoveRobot(direction);
        }

        return warehouse.Boxes.Sum(box => 100 * box.X + box.Y);
    }

    public override async Task<long> Part2()
    {
        var warehouse = await WarehousePart2.Parse(reader.ReadLinesAsync());

        await foreach (var direction in GetDirectionsAsync(reader))
        {
            warehouse.MoveRobot(direction);
        }

        return warehouse.LeftSideOfBoxes.Sum(box => 100 * box.X + box.Y);
    }

    private record struct WarehousePart1(FrozenSet<Position> Walls, HashSet<Position> Boxes, Position Robot)
    {
        public void MoveRobot(Direction direction)
        {
            var newRobotPosition = this.Robot + direction;
            var pos = this.Robot + direction;

            while (true)
            {
                if (this.Walls.Contains(pos))
                {
                    return;
                }

                if (this.Boxes.Contains(pos))
                {
                    pos += direction;
                    continue;
                }

                // Here we conceptually don't push all boxes in the way of the robot
                // we teleport the box right in front of it to the end
                // E.g. when we number the boxes and the robot pushes to the right
                // @1234. -> .@2341
                // instead of
                // @1234. -> .@1234
                // However, because the boxes are indistinguishable, the result is the same
                // but with only two set operations
                if (this.Boxes.Remove(newRobotPosition))
                {
                    this.Boxes.Add(pos);
                }
                this.Robot = newRobotPosition;
                return;
            }
        }

        public static async Task<WarehousePart1> Parse(IAsyncEnumerable<string> lines)
        {
            var x = 0;
            await using var enumerator = lines.GetAsyncEnumerator();
            var walls = new List<Position>();
            var boxes = new List<Position>();
            var robot = default(Position?);

            while (await enumerator.MoveNextAsync() && enumerator.Current is { Length: > 0 } line)
            {
                for (var y = 0; y < line.Length; ++y)
                {
                    switch (line[y])
                    {
                        case '#':
                            walls.Add(new Position(x, y));
                            break;
                        case 'O':
                            boxes.Add(new Position(x, y));
                            break;
                        case '@':
                            robot = new Position(x, y);
                            break;
                    }
                }
                ++x;
            }

            if (robot is null)
            {
                throw new InvalidOperationException("No robot found on map.");
            }

            return new WarehousePart1(walls.ToFrozenSet(), boxes.ToHashSet(), robot.Value);
        }
    }

    private record struct WarehousePart2(FrozenSet<Position> Walls, HashSet<Position> LeftSideOfBoxes, Position Robot)
    {
        public void MoveRobot(Direction direction)
        {
            var newRobotPosition = this.Robot + direction;
            if (this.Walls.Contains(newRobotPosition))
            {
                return;
            }

            var boxesToPush = new HashSet<Position>();
            if (this.LeftSideOfBoxes.Contains(newRobotPosition))
            {
                boxesToPush.Add(newRobotPosition);
            }
            else if (this.LeftSideOfBoxes.Contains(newRobotPosition + Direction.Left))
            {
                boxesToPush.Add(newRobotPosition + Direction.Left);
            }

            var queue = new Queue<Position>(boxesToPush);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var newBoxPosition = current + direction;

                if (this.Walls.Contains(newBoxPosition) || this.Walls.Contains(newBoxPosition + Direction.Right))
                {
                    // Cannot be pushed
                    return;
                }

                foreach (var box in
                         (IEnumerable<Position>)[
                             newBoxPosition + Direction.Left,
                             newBoxPosition,
                             newBoxPosition + Direction.Right,
                         ])
                {
                    if (this.LeftSideOfBoxes.Contains(box) && boxesToPush.Add(box))
                    {
                        queue.Enqueue(box);
                    }
                }
            }

            this.Robot = newRobotPosition;
            this.LeftSideOfBoxes.ExceptWith(boxesToPush);
            this.LeftSideOfBoxes.UnionWith(boxesToPush.Select(b => b + direction));
        }

        public static async Task<WarehousePart2> Parse(IAsyncEnumerable<string> lines)
        {
            var x = 0;
            await using var enumerator = lines.GetAsyncEnumerator();
            var walls = new List<Position>();
            var boxes = new List<Position>();
            var robot = default(Position?);

            while (await enumerator.MoveNextAsync() && enumerator.Current is { Length: > 0 } line)
            {
                for (var y = 0; y < 2 * line.Length; y += 2)
                {
                    switch (line[y/2])
                    {
                        case '#':
                            walls.Add(new Position(x, y));
                            walls.Add(new Position(x, y + 1));
                            break;
                        case 'O':
                            boxes.Add(new Position(x, y));
                            break;
                        case '@':
                            robot = new Position(x, y);
                            break;
                    }
                }
                ++x;
            }

            if (robot is null)
            {
                throw new InvalidOperationException("No robot found on map.");
            }

            return new WarehousePart2(walls.ToFrozenSet(), boxes.ToHashSet(), robot.Value);
        }
    }

    private readonly record struct Position(int X, int Y)
    {
        public static Position operator +(Position a, Direction b) => new(a.X + b.X, a.Y + b.Y);
    }

    private readonly record struct Direction
    {
        public required int X { get; init; }
        public required int Y { get; init; }

        [SetsRequiredMembers]
        private Direction(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Direction Up => new(-1, 0);
        public static Direction Right => new(0, 1);
        public static Direction Down => new(1, 0);
        public static Direction Left => new(0, -1);
    }

    private static async IAsyncEnumerable<Direction> GetDirectionsAsync(TextReader reader)
    {
        var memory = MemoryPool<char>.Shared.Rent();
        while (await reader.ReadAsync(memory.Memory) is var read and > 0)
        {
            for (var i = 0; i < read; ++i)
            {
                switch (memory.Memory.Span[i])
                {
                    case '^':
                        yield return Direction.Up;
                        break;
                    case '>':
                        yield return Direction.Right;
                        break;
                    case 'v':
                        yield return Direction.Down;
                        break;
                    case '<':
                        yield return Direction.Left;
                        break;
                }
            }
        }
    }
}
