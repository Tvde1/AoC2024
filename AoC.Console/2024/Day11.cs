using AoC.Puzzles;

namespace AoC.Console._2024;

[Puzzle(2024, 11)]
public class Day11 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        // input = "125 17";

        var initalStones = input.ToString().Split(' ').Select(int.Parse).Select(x => new Stone(x)).ToArray();

        var stones = initalStones.ToList();
        for (int i = 0; i < 25; i++)
        {
            stones = stones
                .SelectMany(x => x.Step())
                .ToList();
        }

        return stones.Count;
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        var initalStones = input.ToString().Split(' ').Select(int.Parse).Select(x => new Stone(x)).ToArray();

        var stones = initalStones.Select(x => (Stone: x, Amount: 1L)).ToList();
        for (var i = 0; i < 75; i++)
        {
            stones = stones.SelectMany(x => x.Stone.Step().Select(s => x with { Stone = s }))
                .GroupBy(x => x.Stone, (stone, group) => (stone, group.Sum(x => x.Amount)))
                .ToList();
        }

        return stones.Sum(x => x.Amount);
    }

    public string PrettyPrint(long output) => output.ToString();

    private readonly record struct Stone(long Value)
    {
        public Stone[] Step()
        {
            if (Value is 0)
            {
                return [new(1)];
            }

            var stringed = Value.ToString();

            if (stringed.Length % 2 == 0)
            {
                var a = stringed[..(stringed.Length / 2)];
                var b = stringed[(stringed.Length / 2)..];
                return [new(long.Parse(a)), new(long.Parse(b))];
            }

            return [new(Value * 2024)];
        }
    }
}