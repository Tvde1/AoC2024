// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AoC.Console.Generated;
using Spectre.Console;

var text = new FigletText("Advent of Code 2024")
    .Centered()
    .Color(Color.Green);
AnsiConsole.Write(text);

var puzzle = DiscoverPuzzle();

var c = new Calendar(puzzle.Year, 12)
    .Culture("en-GB")
    .HighlightStyle(Style.Parse("yellow"))
    .AddCalendarEvent("Today", puzzle.Year, 12, puzzle.Day)
    .Centered();

AnsiConsole.Write(c);
AnsiConsole.MarkupLine($"[bold]Selected:[/] [u]{puzzle.Year}[/] Day [u]{puzzle.Day}[/]");

var runSelection = new SelectionPrompt<string>()
    .AddChoices("Run", "Benchmark");

var runAnswer = runSelection.Show(AnsiConsole.Console);

if (runAnswer == "Run")
{
    var table = new Table()
        .AddColumns(new TableColumn("Part 1"), new TableColumn("Part 2"))
        .AddEmptyRow()
        .AddEmptyRow();

    await AnsiConsole.Live(table)
        .StartAsync(async ctx =>
        {
            var t1 = Task.Run(() =>
            {
                var timestamp = Stopwatch.GetTimestamp();
                var part1 = puzzle.RunPart1();
                var elapsed = Stopwatch.GetElapsedTime(timestamp);
                table.UpdateCell(0, 0, part1);
                table.UpdateCell(1, 0, new Markup($"Took {elapsed}"));
                ctx.Refresh();
            });

            var t2 = Task.Run(() =>
            {
                var timestamp = Stopwatch.GetTimestamp();
                var part2 = puzzle.RunPart2();
                var elapsed = Stopwatch.GetElapsedTime(timestamp);
                table.UpdateCell(0, 1, part2);
                table.UpdateCell(1, 1, new Markup($"Took {elapsed}"));
                ctx.Refresh();
            });

            await Task.WhenAll(t1, t2);
        });
}
else if (runAnswer == "Benchmark")
{
    puzzle.RunBenchmark();
}

return;

IGeneratedPuzzle DiscoverPuzzle()
{
    return PuzzleCollection.Puzzles.OrderByDescending(x => x.Year).ThenByDescending(x => x.Day).First();
}