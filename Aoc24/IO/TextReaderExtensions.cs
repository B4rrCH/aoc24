using System.Runtime.CompilerServices;

namespace Aoc24.IO;

public static class TextReaderExtensions
{
    public static async IAsyncEnumerable<string> ReadLinesAsync(
        this TextReader reader,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await reader.ReadLineAsync(cancellationToken) is {} line)
        {
            yield return line;
        }
    }

    public static ValueTask<char[][]> ReadToGrid(this TextReader reader, CancellationToken cancellationToken = default)
        => reader.ReadLinesAsync(cancellationToken).Select(line => line.ToCharArray()).ToArrayAsync(cancellationToken);
}
