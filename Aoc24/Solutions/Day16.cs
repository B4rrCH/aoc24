using System.Diagnostics;
using System.Runtime.InteropServices;
using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day16(TextReader reader) : SolutionBase<int, int>, IConstructFromReader<Day16>
{

    public static Day16 Construct(TextReader reader) => new Day16(reader);
    public override async Task<int> Part1()
    {
        var maze = await reader.ReadTo2DArrayAsync();
        var bestCosts = new Dictionary<Position, int>();
        return Dijkstra(maze, bestCosts).Min(p => bestCosts[p]);
    }

    public override async Task<int> Part2()
    {
        var map = await reader.ReadTo2DArrayAsync();

        var bestCosts = new Dictionary<Position, int>();
        var endPositions = Dijkstra(map, bestCosts);

        var bestCost = endPositions.Min(p => bestCosts[p]);
        return CountOnBestPaths(endPositions.Where(p => bestCosts[p] == bestCost), bestCosts, map);
    }

    private static int CountOnBestPaths(
        IEnumerable<Position> endPositions,
        Dictionary<Position, int> bestCosts,
        char[,] map)
    {
        var onBestPath = new HashSet<(int X, int Y)>();
        var predecessorQueue = new Queue<Position>(endPositions);

        while (predecessorQueue.TryDequeue(out var reverse))
        {
            onBestPath.Add((reverse.X, reverse.Y));
            var cost = bestCosts[reverse];

            foreach (var (possiblePredecessor, expectedCost) in reverse.PossiblePredecessors(map, cost))
            {
                if (bestCosts[possiblePredecessor] == expectedCost)
                {
                    predecessorQueue.Enqueue(possiblePredecessor);
                }
            }
        }

        return onBestPath.Count;
    }

    private static IReadOnlyList<Position> Dijkstra(char[,] maze, Dictionary<Position, int> bestCosts)
    {
        var start = maze.IndexOf('S') ?? throw new InvalidOperationException("Could not find start.");
        var end = maze.IndexOf('E') ?? throw new InvalidOperationException("Could not find end.");
        var queue = new PriorityQueue<Position, int>();
        queue.Enqueue(new Position(start.Item1, start.Item2, Direction.East), 0);

        while (queue.TryDequeue(out var reindeer, out var cost))
        {
            ref var bestCostSoFar = ref CollectionsMarshal.GetValueRefOrAddDefault(bestCosts, reindeer, out var existed);
            if (existed && cost >= bestCostSoFar)
            {
                continue;
            }
            bestCostSoFar = cost;

            queue.EnqueueRange(reindeer.Forward(maze, cost));
        }

        return
        [
            new Position(end.Item1, end.Item2, Direction.East),
            new Position(end.Item1, end.Item2, Direction.South),
            new Position(end.Item1, end.Item2, Direction.West),
            new Position(end.Item1, end.Item2, Direction.North),
        ];
    }

    private readonly record struct Position(int X, int Y, Direction Direction)
    {
        public IEnumerable<(Position Position, int Cost)> Forward(char[,] maze, int cost)
        {
            if (this.Step(maze) is {} step)
            {
                yield return (step, cost + 1);
            }
            yield return (this.TurnClockwise(), cost + 1000);
            yield return (this.TurnCounterClockwise(), cost + 1000);
        }

        public IEnumerable<(Position Position, int ExpectedCost)> PossiblePredecessors(char[,] maze, int cost)
        {
            if (this.StepBack(maze) is {} back)
            {
                yield return (back, cost - 1);
            }
            yield return (this.TurnClockwise(), cost - 1000);
            yield return (this.TurnCounterClockwise(), cost - 1000);
        }

        private Position? Step(char[,] maze)
        {
            var (x, y) = this.Direction switch
            {
                Direction.East => (this.X, this.Y + 1),
                Direction.South => (this.X + 1, this.Y),
                Direction.West => (this.X, this.Y - 1),
                Direction.North => (this.X - 1, this.Y),
                _ => throw new UnreachableException(),
            };

            if (x < 0
                || maze.GetLength(0) <= x
                || y < 0
                || maze.GetLength(1) <= y
                || maze[x, y] is '#')
            {
                return null;
            }

            return new Position(x, y, this.Direction);
        }

        private Position? StepBack(char[,] map)
        {
            var (x, y) = this.Direction switch
            {
                Direction.East => (this.X, this.Y - 1),
                Direction.South => (this.X - 1, this.Y),
                Direction.West => (this.X, this.Y + 1),
                Direction.North => (this.X + 1, this.Y),
                _ => throw new UnreachableException(),
            };

            if (x < 0
                || map.GetLength(0) <= x
                || y < 0
                || map.GetLength(1) <= y
                || map[x, y] is '#')
            {
                return null;
            }

            return new Position(x, y, this.Direction);
        }

        private Position TurnClockwise() =>
            this with
            {
                Direction = this.Direction switch
                {
                    Direction.East => Direction.South,
                    Direction.South => Direction.West,
                    Direction.West => Direction.North,
                    Direction.North => Direction.East,
                    _ => throw new UnreachableException(),
                },
            };

        private Position TurnCounterClockwise() =>
            this with
            {
                Direction = this.Direction switch
                {
                    Direction.East => Direction.North,
                    Direction.North => Direction.West,
                    Direction.West => Direction.South,
                    Direction.South => Direction.East,
                    _ => throw new UnreachableException(),
                },
            };
    }

    private enum Direction
    {
        East,
        South,
        West,
        North,
    }
}

file static class ArrayExtensions
{
    public static (int, int)? IndexOf<T>(this T[,] array, T element)
        where T : IEquatable<T>
    {
        var span = MemoryMarshal.CreateSpan(ref array[0, 0], array.Length);
        return span.IndexOf(element) switch
        {
            < 0 => null,
            var i => int.DivRem(i, array.GetLength(1)),
        };
    }
}
