using Aoc24.IO;

namespace Aoc24.Solutions;

public class Day09(TextReader reader) : SolutionBase<ulong, ulong>, IConstructFromReader<Day09>
{
    public static Day09 Construct(TextReader reader) => new(reader);

    public override async Task<ulong> Part1()
    {
        var fragmentedFiles = await GetPaddedFiles(reader).ToArrayAsync();

        var index = 0ul;
        var checksum = 0ul;
        foreach (var block in CompactEager(fragmentedFiles).ToArray())
        {
            var sumOfIndices =
                index * block.Length
                + block.Length * (block.Length - 1) / 2;
            checksum += block.Id * sumOfIndices;
            index += block.Length;
        }

        return checksum;
    }

    public override async Task<ulong> Part2()
    {
        var fragmentedFiles = await GetPaddedFiles(reader).ToListAsync();
        CompactEntire(fragmentedFiles);

        var index = 0ul;
        var checksum = 0ul;
        foreach (var block in fragmentedFiles)
        {
            var sumOfIndices =
                index * block.Length
                + block.Length * (block.Length - 1) / 2;
            checksum += block.Id * sumOfIndices;
            index += block.Length + block.Padding;
        }

        return checksum;
    }

    private static IEnumerable<Blocks> CompactEager(ReadOnlyMemory<PaddedFile> files)
    {
        PaddedFile filler = default;
        while (0 < files.Length)
        {
            (var head, files) = (files.Span[0], files[1..]);
            yield return new Blocks(head.Id, head.Length);

            for (var padding = head.Padding; padding > 0;)
            {
                if (files.Length is 0)
                {
                    break;
                }

                if (filler.Length is 0)
                {
                    (files, filler) = (files[..^1], files.Span[^1]);
                }

                switch (filler.Length)
                {
                    case var longTail when longTail >= padding:
                        yield return new Blocks(filler.Id, padding);
                        filler = filler with { Length = longTail - padding };
                        padding = 0;
                        continue;
                    case var shortTail:
                        yield return new Blocks(filler.Id, shortTail);
                        filler = filler with { Length = 0 };
                        padding -= shortTail;
                        continue;
                }
            }
        }

        if (filler.Length > 0)
        {
            yield return new Blocks(filler.Id, filler.Length);
        }
    }

    private static void CompactEntire(List<PaddedFile> files)
    {
        var firstPadding = 0;
        for (var from = files.Count - 1; from > firstPadding;)
        {
            while (files[firstPadding].Padding is 0ul)
            {
                ++firstPadding;
            }

            var moved = false;
            for (var to = firstPadding; to < from; ++to)
            {
                if (TrySwap(from, to) is false)
                {
                    continue;
                }

                moved = true;
                break;
            }

            if (moved is false)
            {
                --from;
            }
        }

        return;

        bool TrySwap(int from, int to)
        {
            if (files[to].Padding < files[from].Length)
            {
                return false;
            }

            var fileToMove = files[from];
            files.RemoveAt(from);
            files[from - 1] = files[from - 1] with
            {
                Padding = files[from - 1].Padding + fileToMove.Length + fileToMove.Padding,
            };

            var fileToUsePadding = files[to];
            files[to] = fileToUsePadding with { Padding = 0 };
            files.Insert(to + 1, fileToMove with { Padding = fileToUsePadding.Padding - fileToMove.Length });
            return true;
        }
    }

    private static async IAsyncEnumerable<PaddedFile> GetPaddedFiles(TextReader reader)
    {
        await using var enumerator = GetDigits(reader).GetAsyncEnumerator();

        for (var id = 0ul; await enumerator.MoveNextAsync(); ++id)
        {
            var length = enumerator.Current;
            var padding = await enumerator.MoveNextAsync() ? enumerator.Current : 0;
            yield return new PaddedFile(id, length, padding);
        }
    }

    private static async IAsyncEnumerable<ulong> GetDigits(TextReader reader)
    {
        var buffer = new char[1];
        while (await reader.ReadAsync(buffer) > 0)
        {
            yield return (ulong)(buffer[0] - '0');
        }
    }

    private readonly record struct Blocks(ulong Id, ulong Length);
    private readonly record struct PaddedFile(ulong Id, ulong Length, ulong Padding);
}
