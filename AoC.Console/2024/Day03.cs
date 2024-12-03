using System.Diagnostics;
using System.Text.RegularExpressions;
using AoC.Puzzles;
using Spectre.Console;

namespace AoC.Console._2024;

[Puzzle(2024, 03)]
public partial class Day03 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        // input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

        long counter = 0;
        var groups = MulRegex().Matches(input.ToString());

        foreach (Match group in groups)
        {
            counter += long.Parse(group.Groups["one"].ValueSpan) *
                       long.Parse(group.Groups["two"].ValueSpan);
        }

        return counter;
    }

    [GeneratedRegex(@"mul\((?<one>\d{1,3}),(?<two>\d{1,3})\)")]
    public partial Regex MulRegex();

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        long counter = 0;
        var groups = InstructionRegex().Matches(input.ToString());

        var isDo = true;

        foreach (Match group in groups)
        {
            switch (group.ValueSpan)
            {
                case "do()":
                    isDo = true;
                    continue;
                case "don't()":
                    isDo = false;
                    continue;
            }

            Debug.Assert(group.ValueSpan.StartsWith("mul("));

            if (!isDo)
            {
                continue;
            }

            counter += long.Parse(group.Groups["one"].ValueSpan) *
                       long.Parse(group.Groups["two"].ValueSpan);
        }

        return counter;
    }

    [GeneratedRegex(@"(mul\((?<one>\d{1,3}),(?<two>\d{1,3})\)|do\(\)|don't\(\))")]
    public partial Regex InstructionRegex();

    public string PrettyPrint(long output) => output.ToString();
}