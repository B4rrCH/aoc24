using Aoc24;
using Aoc24.Solutions;

Console.WriteLine("Day 1");
using (var reader = File.OpenText(@"Data\day01.txt"))
{
    var day01 = new Day01(reader);
    var part1 = await day01.Part1();
    Console.WriteLine($"\t Part 1: {part1}");
}
using (var reader = File.OpenText(@"Data\day01.txt"))
{
    var day01 = new Day01(reader);
    var part2 = await day01.Part2();
    Console.WriteLine($"\t Part 2: {part2}");
}

Console.WriteLine("Goodbye");
await Solution.Run<Day01>();
