using System.Collections;
using System.Diagnostics;
using AoC.Puzzles;
using Spectre.Console;

namespace AoC.Console._2024;

[Puzzle(2024, 05)]
public class Day05 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
//         input = """
//                 47|53
//                 97|13
//                 97|61
//                 97|47
//                 75|29
//                 61|13
//                 75|53
//                 29|13
//                 97|29
//                 53|29
//                 61|53
//                 97|53
//                 61|29
//                 47|13
//                 75|47
//                 97|75
//                 47|61
//                 75|61
//                 47|29
//                 75|13
//                 53|13
//
//                 75,47,61,53,29
//                 97,61,53,29,13
//                 75,29,13
//                 75,97,47,61,53
//                 61,13,29
//                 97,13,75,29,47
//                 """;

        var split = input.ToString().Split(Environment.NewLine + Environment.NewLine);

        var mustBeEarlierThanByNumber = split[0].Split(Environment.NewLine)
            .Select(x => x.Split('|'))
            .GroupBy(x => int.Parse(x[0]), x => int.Parse(x[1]))
            .ToDictionary(x => x.Key, x => x.ToList());

        var updates = split[1].Split(Environment.NewLine)
            .Select(x => x.Split(',').Select(int.Parse).ToList());

        return updates.Where(IsUpdateCorrect)
            .Select(GetMiddle)
            .Sum();

        static int GetMiddle(List<int> x) => x[(x.Count - 1) / 2];

        bool IsUpdateCorrect(List<int> updates)
        {
            for (var index = 0; index < updates.Count; index++)
            {
                var nums = updates[index];
                if (!mustBeEarlierThanByNumber.TryGetValue(nums, out var rule))
                {
                    continue;
                }

                for (var i = 0; i < index; i++)
                {
                    if (rule.Contains(updates[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
//         input = """
//                 47|53
//                 97|13
//                 97|61
//                 97|47
//                 75|29
//                 61|13
//                 75|53
//                 29|13
//                 97|29
//                 53|29
//                 61|53
//                 97|53
//                 61|29
//                 47|13
//                 75|47
//                 97|75
//                 47|61
//                 75|61
//                 47|29
//                 75|13
//                 53|13
//
//                 75,47,61,53,29
//                 97,61,53,29,13
//                 75,29,13
//                 75,97,47,61,53
//                 61,13,29
//                 97,13,75,29,47
//                 """;

        var split = input.ToString().Split(Environment.NewLine + Environment.NewLine);

        var mustBeEarlierThanByNumber = split[0].Split(Environment.NewLine)
            .Select(x => x.Split('|'))
            .GroupBy(x => int.Parse(x[0]), x => int.Parse(x[1]))
            .ToDictionary(x => x.Key, x => x.ToList());

        var updates = split[1].Split(Environment.NewLine)
            .Select(x => x.Split(',').Select(int.Parse).ToList());

        List<int> FixUp(List<int> x)
        {
            // while (!IsUpdateCorrect(x))
            // {
            //     Shuffle(x);
            // }

            var now = string.Join(",", x);

            x.Sort(new RuleComparer(mustBeEarlierThanByNumber));

            var sorted = string.Join(",", x);

            var isCorrectNow = IsUpdateCorrect(x);

            var text = $"""
                        Was:  {now}
                        Sort: {sorted}
                        Is correct? {isCorrectNow}
                        """;

            AnsiConsole.WriteLine(text);

            return x;
        }

        return updates.Where(x => !IsUpdateCorrect(x))
            .AsParallel()
            .Select(FixUp)
            .Select(GetMiddle)
            .Sum();

        static int GetMiddle(List<int> x) => x[(x.Count - 1) / 2];

        bool IsUpdateCorrect(List<int> updates)
        {
            for (var index = 0; index < updates.Count; index++)
            {
                var nums = updates[index];
                if (!mustBeEarlierThanByNumber.TryGetValue(nums, out var rule))
                {
                    continue;
                }

                for (var i = 0; i < index; i++)
                {
                    if (rule.Contains(updates[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public string PrettyPrint(long output) => output.ToString();

    private static void Shuffle<T>(List<T> items)
    {
        for (var i = 0; i < items.Count - 1; i++)
        {
            var pos = Random.Shared.Next(i, items.Count);
            (items[i], items[pos]) = (items[pos], items[i]);
        }
    }
}

public class RuleComparer : IComparer<int>
{
    private readonly Dictionary<int, List<int>> _mustBeEarlierThanByNumber;

    public RuleComparer(Dictionary<int, List<int>> mustBeEarlierThanByNumber)
    {
        _mustBeEarlierThanByNumber = mustBeEarlierThanByNumber;
    }

    public int Compare(int x, int y)
    {
        if (!_mustBeEarlierThanByNumber.TryGetValue(x, out var found))
        {
            return 1;
        }

        if (found.Contains(y))
        {
            return -1;
        }

        return 1;
    }
}