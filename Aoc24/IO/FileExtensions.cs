namespace Aoc24.IO;

public static class FileExtensions
{
    public static TextReader OpenAsyncText(string path)
    {
        var stream = File.Open(
            path,
            new FileStreamOptions
            {
                Access = FileAccess.Read,
                Mode = FileMode.Open,
                Options = FileOptions.Asynchronous | FileOptions.SequentialScan,
                Share = FileShare.Read,
            });

        return new StreamReader(stream, leaveOpen: false);
    }
}
