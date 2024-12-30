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
}
