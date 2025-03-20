using System.Runtime.InteropServices;

namespace Aoc24.IO;

public static class ArrayExtensions
{
    public static Span<T> Column<T>(this T[,] array, int index) =>
        MemoryMarshal.CreateSpan(ref array[index, 0], array.GetLength(1));

    public static IEnumerable<(int X, int Y)> Indexes<T>(this T[,] array) =>
        Enumerable.Range(0, array.GetLength(0))
            .SelectMany(x => Enumerable.Range(0, array.GetLength(1)).Select(y => (x, y)));
}