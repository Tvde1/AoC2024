using AoC.Puzzles;
using Spectre.Console;

namespace AoC.Console._2024;

[Puzzle(2024, 02)]
public class Day02 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        var allLines = input.ToString().Split(Environment.NewLine)
            .Select(x => x.Split(' ').Select(long.Parse).ToList())
            .Select(IsSafe)
            .Count(x => x);

        return allLines;

        bool IsSafe(List<long> levels)
        {
            var totalDiff = 0L;
            var absTotalDiff = 0L;
            for (int i = 0; i < levels.Count - 1; i++)
            {
                var diff = levels[i + 1] - levels[i];
                var absDiff = Math.Abs(diff);
                if (absDiff is 0 or > 3)
                {
                    return false;
                }

                totalDiff += diff;
                absTotalDiff += absDiff;

                if (absTotalDiff != Math.Abs(totalDiff))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        // input = """
        //         7 6 4 2 1
        //         1 2 7 8 9
        //         9 7 6 2 1
        //         1 3 2 4 5
        //         8 6 4 4 1
        //         1 3 6 7 9
        //         """;
        var allLines = input.ToString().Split(Environment.NewLine)
            .Select(x => x.Split(' ').Select(long.Parse).ToList())
            .Select(IsSafeWhenAnyRemoved)
            .Count(x => x);

        return allLines;

        bool IsSafeWhenAnyRemoved(List<long> levels)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                var newList = new List<long>(levels);
                newList.RemoveAt(i);
                if (IsSafe(newList))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsSafe(List<long> levels)
        {
            var totalDiff = 0L;
            var absTotalDiff = 0L;
            for (int i = 0; i < levels.Count - 1; i++)
            {
                var diff = levels[i + 1] - levels[i];
                var absDiff = Math.Abs(diff);
                if (absDiff is 0 or > 3)
                {
                    return false;
                }

                totalDiff += diff;
                absTotalDiff += absDiff;

                if (absTotalDiff != Math.Abs(totalDiff))
                {
                    return false;
                }
            }

            return true;
        }


        // var allLines = input.ToString().Split(Environment.NewLine)
        //     .Select(x => x.Split(' ').Select(long.Parse).ToList())
        //     .Select(CountUnsafeItems)
        //     .Count(x => x is 0 or 1);
        //
        // return allLines;
        //
        // int CountUnsafeItems(List<long> levels)
        // {
        //     var diffs = new List<long>();
        //
        //     for (int i = 0; i < levels.Count - 1; i++)
        //     {
        //         diffs.Add(levels[i + 1] - levels[i]);
        //     }
        //
        //
        // }
        //
        // bool IsSafe(long a, long b, bool isNegative)
        // {
        //     var diff = b - a;
        //     if (isNegative && diff is >= 0 or < -3)
        //     {
        //         return false;
        //     }
        //
        //     return diff is 0 or > 3;
        // }
    }

    public string PrettyPrint(long output) => output.ToString();
}