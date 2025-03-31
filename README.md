# B4rr's Advent of Code 2024

My solutions to the problems of [Advent of Code 2024](https://adventofcode.com/2024).

Using C#, I focus on the shinier and newer features, that have rarer usecases in my daily programming, resulting in a semi-intentional overuse of things types like `Span<T>`, `IAsyncEnumerable<T>`, or `ParallelQuery<T>`.

## Development

For development, install the [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0).

To run the solution, open `./Aoc24` in a terminal and run `dotnet run`.

To run the tests, open `./Aoc24.Test` in a terminal and run `dotnet test`. Due to using the rather new [TUnit](https://github.com/thomhurst/TUnit), IDEs might need some of their settings changed a bit, see their [documentation](https://github.com/thomhurst/TUnit?tab=readme-ov-file#ide).

## Ahead of Time compilation

The project supports AOT compilation, if Visual Studio 2022, including the Desktop development with C++ workload with all default components is installed.
Run `dotnet publish ./Aoc24/Aoc24.csproj -o publish` to build the project into a single native executable for the current environment.
Consider the [options](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish#options) for the publish command for cross-compilation for other platforms.
