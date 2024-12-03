using AoC.Puzzles;

namespace AoC.Console._2024;

[Puzzle(2024, 01)]
public class Day01 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        var allLines = input.ToString().Split(Environment.NewLine);

        var allLeftNumbers = allLines.Select(x => long.Parse(x.Split("   ")[0])).OrderBy(x => x).ToList();
        var allRightNumbers = allLines.Select(x => long.Parse(x.Split("   ")[1])).OrderBy(x => x).ToList();

        return allLeftNumbers.Zip(allRightNumbers)
            .Select(x => Math.Max(x.First, x.Second) - Math.Min(x.First, x.Second))
            .Sum();
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        var allLines = input.ToString().Split(Environment.NewLine);

        var allLeftNumbers = allLines.Select(x => long.Parse(x.Split("   ")[0])).OrderBy(x => x).ToList();
        var allRightNumbers = allLines.Select(x => long.Parse(x.Split("   ")[1])).OrderBy(x => x).ToList();

        return allLeftNumbers.Select(x => allRightNumbers.Count(num => num == x) * x).Sum();
    }

    public string PrettyPrint(long output) => output.ToString();
}