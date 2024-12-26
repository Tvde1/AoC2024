using System.Diagnostics;
using AoC.Puzzles;

namespace AoC.Console._2024;

[Puzzle(2024, 08)]
public class Day08 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        // input = """
        //         ............
        //         ........0...
        //         .....0......
        //         .......0....
        //         ....0.......
        //         ......A.....
        //         ............
        //         ............
        //         ........A...
        //         .........A..
        //         ............
        //         ............
        //         """;

        var antennaMap = CreateAntennaMap(input);

        var mapWidth = input.IndexOf(Environment.NewLine);
        var mapHeight = (input.Length + Environment.NewLine.Length) / (mapWidth + Environment.NewLine.Length);

#if DEBUG
        var amountSpaces = input.ToString().Count(x => x is '.');
        var amountOfAntennas = antennaMap.Sum(x => x.Value.Count);

        Debug.Assert((mapWidth * mapHeight) - amountSpaces == amountOfAntennas);
#endif

        return antennaMap.SelectMany(allInOneColor =>
                allInOneColor.Value
                    .SelectMany(a =>
                        allInOneColor.Value.Select(b => (a, b)))
                    .Where(p => p.a.X != p.b.X && p.a.Y != p.b.Y)
                    .Select(p => CalculateInterferencePoint(p.a, p.b))
                    .Where(IsWithinMap)
            )
            .Distinct()
            .Count();

        bool IsWithinMap((int x, int y) p) => p.x >= 0 && p.x < mapWidth &&
                                              p.y >= 0 && p.y < mapHeight;

        (int X, int Y) CalculateInterferencePoint((int X, int Y) a, (int X, int Y) b)
        {
            return (a.X * 2 - b.X, a.Y * 2 - b.Y);
        }
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        // input = """
        //         ............
        //         ........0...
        //         .....0......
        //         .......0....
        //         ....0.......
        //         ......A.....
        //         ............
        //         ............
        //         ........A...
        //         .........A..
        //         ............
        //         ............
        //         """;

        var antennaMap = CreateAntennaMap(input);

        var mapWidth = input.IndexOf(Environment.NewLine);
        var mapHeight = (input.Length + Environment.NewLine.Length) / (mapWidth + Environment.NewLine.Length);

        return antennaMap.SelectMany(allInOneColor =>
            allInOneColor.Value
                .SelectMany(a =>
                    allInOneColor.Value.Select(b => (a, b))
                        .Where(p => p.a.X != p.b.X && p.a.Y != p.b.Y)))
            .SelectMany(p => CalculateInterferencePoints(p.a, p.b)
                .TakeWhile(IsWithinMap))
            .Distinct()
            .Count();

        bool IsWithinMap((int x, int y) p) => p.x >= 0 && p.x < mapWidth &&
                                              p.y >= 0 && p.y < mapHeight;

        IEnumerable<(int X, int Y)> CalculateInterferencePoints((int X, int Y) a, (int X, int Y) b)
        {
            var xDiff = (a.X - b.X);
            var yDiff = (a.Y - b.Y);

            yield return a;

            var iterCount = 1;
            while (true)
            {
                yield return (
                    a.X + (iterCount * xDiff),
                    a.Y + (iterCount * yDiff)
                );
                iterCount++;
            }
        }
    }

    public string PrettyPrint(long output) => output.ToString();

    Dictionary<char, List<(int X, int Y)>> CreateAntennaMap(ReadOnlySpan<char> map)
    {
        var lines = map.Split(Environment.NewLine);

        Dictionary<char, List<(int, int)>> mapp = new();

        var y = 0;
        foreach (var lineRange in lines)
        {
            var line = map[lineRange];

            for (var x = 0; x < line.Length; x++)
            {
                var charr = line[x];
                if (charr is '.') continue;

                if (mapp.TryGetValue(charr, out var value))
                {
                    value.Add((x, y));
                }
                else
                {
                    mapp[charr] = [(x, y)];
                }
            }

            y++;
        }

        return mapp;
    }
}