using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aoc24.IO;

public static class TextReaderExtensions
{
    public static IAsyncEnumerable<string> ReadLinesAsync(
        this TextReader reader)
    {
        return Impl(reader, CancellationToken.None);

        static async IAsyncEnumerable<string> Impl(TextReader reader,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await reader.ReadLineAsync(cancellationToken) is { } line)
            {
                yield return line;
            }
        }
    }

    public static ValueTask<char[][]> ReadToGrid(this TextReader reader, CancellationToken cancellationToken = default)
        => reader.ReadLinesAsync().Select(line => line.ToCharArray()).ToArrayAsync(cancellationToken);

    public static async ValueTask<char[,]> ReadTo2DArray(this TextReader reader, CancellationToken cancellationToken = default)
    {
        var lines = await reader.ReadLinesAsync().ToArrayAsync(cancellationToken);

        var width = lines.First().Length;
        var array = new char[lines.Length, width];

        for (var i = 0; i < lines.Length; i++)
        {
            lines[i].CopyTo(array.Column(i));
        }

        return array;
    }
}
