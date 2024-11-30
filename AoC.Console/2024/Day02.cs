using AoC.Puzzles;

namespace AoC.Console._2024;

[Puzzle(2024, 2)]
public class Day02 : IPuzzle<int, string>
{
    public int SolvePart1(ReadOnlySpan<char> input)
    {
        return 1234;
    }

    public string PrettyPrint(int output) => output.ToString();

    public string SolvePart2(ReadOnlySpan<char> input)
    {
        return "Hello, World!";
    }

    public string PrettyPrint(string output) => output;
}