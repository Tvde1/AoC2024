using System.Diagnostics;
using System.Numerics;
using System.Text;
using AoC.Puzzles;
using Spectre.Console;

namespace AoC.Console._2024;

[Puzzle(2024, 10)]
public class Day10 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        var testInput = """
                        89010123
                        78121874
                        87430965
                        96549874
                        45678903
                        32019012
                        01329801
                        10456732
                        """;

        // input = testInput;

        // input = """
        //         ...0...
        //         ...1...
        //         ...2...
        //         6543456
        //         7.....7
        //         8.....8
        //         9.....9
        //         """;

        // input = """
        //         10..9..
        //         2...8..
        //         3...7..
        //         4567654
        //         ...8..3
        //         ...9..2
        //         .....01
        //         """;

        var width = input.IndexOf(Environment.NewLine);
        var height = (input.Length + Environment.NewLine.Length) / (width + Environment.NewLine.Length);

        List<(int X, int Y)> zeros = new();

        var map = new int[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var val = input[x + (y * (width + Environment.NewLine.Length))] - '0';
                map[x, y] = val;
                if (val == 0)
                {
                    zeros.Add((x, y));
                }
            }
        }

        // Debug.Assert(map[0, 0] == 8);
        // Debug.Assert(map[1, 0] == 9);
        // Debug.Assert(map[2, 0] == 0);
        // Debug.Assert(map[3, 0] == 1);
        // Debug.Assert(map[0, 1] == 7);

        var a = zeros
            .Select(x => (Location: x, NineCount: TraverseAndCountNines(map, x, 0, [])))
            .Where(x => x.NineCount != 0)
            .Sum(x => x.NineCount);

        return a;
    }

    private static int TraverseAndCountNines(int[,] map, (int X, int Y) location, int currValue,
        HashSet<(int, int)> visited)
    {
        Span<(int DX, int DY)> translations = [(-1, 0), (0, -1), (1, 0), (0, 1)];

        visited.Add(location);

        var nines = currValue == 9 ? 1 : 0;

        foreach (var trans in translations)
        {
            var newLocation = (X: location.X + trans.DX, Y: location.Y + trans.DY);

            if (!map.IsInRange(newLocation) || visited.Contains(newLocation))
            {
                continue;
            }

            var newValue = map[newLocation.X, newLocation.Y];
            if ((newValue - currValue) is 1)
            {
                nines += TraverseAndCountNines(map, newLocation, newValue, visited);
            }
        }

        return nines;
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        var testInput = """
                        89010123
                        78121874
                        87430965
                        96549874
                        45678903
                        32019012
                        01329801
                        10456732
                        """;

        // input = testInput;

        // input = """
        //         ...0...
        //         ...1...
        //         ...2...
        //         6543456
        //         7.....7
        //         8.....8
        //         9.....9
        //         """;

        // input = """
        //         10..9..
        //         2...8..
        //         3...7..
        //         4567654
        //         ...8..3
        //         ...9..2
        //         .....01
        //         """;

        var width = input.IndexOf(Environment.NewLine);
        var height = (input.Length + Environment.NewLine.Length) / (width + Environment.NewLine.Length);

        List<(int X, int Y)> zeros = new();

        var map = new int[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var val = input[x + (y * (width + Environment.NewLine.Length))] - '0';
                map[x, y] = val;
                if (val == 0)
                {
                    zeros.Add((x, y));
                }
            }
        }

        // Debug.Assert(map[0, 0] == 8);
        // Debug.Assert(map[1, 0] == 9);
        // Debug.Assert(map[2, 0] == 0);
        // Debug.Assert(map[3, 0] == 1);
        // Debug.Assert(map[0, 1] == 7);

        var a = zeros
            .Select(x => (Location: x, NineCount: TraverseAndCountNinesTwo(map, x, 0)))
            .Where(x => x.NineCount != 0)
            .Sum(x => x.NineCount);

        return a;
    }

    private static int TraverseAndCountNinesTwo(int[,] map, (int X, int Y) location, int currValue)
    {
        Span<(int DX, int DY)> translations = [(-1, 0), (0, -1), (1, 0), (0, 1)];

        var nines = currValue == 9 ? 1 : 0;

        foreach (var trans in translations)
        {
            var newLocation = (X: location.X + trans.DX, Y: location.Y + trans.DY);

            if (!map.IsInRange(newLocation))
            {
                continue;
            }

            var newValue = map[newLocation.X, newLocation.Y];
            if ((newValue - currValue) is 1)
            {
                nines += TraverseAndCountNinesTwo(map, newLocation, newValue);
            }
        }

        return nines;
    }

    public string PrettyPrint(long output) => output.ToString();
}

public static class MapExtensions
{
    public static bool IsInRange<T>(this T[,] map, (T X, T Y) location)
        where T : INumber<T>, IComparisonOperators<T, int, bool>
    {
        return location.X >= T.Zero && location.X < map.GetLength(0) &&
               location.Y >= T.Zero && location.Y < map.GetLength(1);
    }
}