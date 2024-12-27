using System.Diagnostics;
using AoC.Console.Utilities;
using AoC.Puzzles;
using Loc = (int X, int Y);

namespace AoC.Console._2024;

[Puzzle(2024, 16)]
public class Day16 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        // input = """
        //         ###############
        //         #.......#....E#
        //         #.#.###.#.###.#
        //         #.....#.#...#.#
        //         #.###.#####.#.#
        //         #.#.#.......#.#
        //         #.#.#####.###.#
        //         #...........#.#
        //         ###.#.#####.#.#
        //         #...#.....#.#.#
        //         #.#.#.###.#.#.#
        //         #.....#...#.#.#
        //         #.###.#.#.#.#.#
        //         #S..#.....#...#
        //         ###############
        //         """;

        var map = new Map(input);

        Dictionary<Direction, (Direction NewDirection, int AddedCost)[]> directionMap = new()
        {
            [Direction.North] = [(Direction.North, 1), (Direction.East, 1001), (Direction.West, 1001)],
            [Direction.South] = [(Direction.South, 1), (Direction.East, 1001), (Direction.West, 1001)],
            [Direction.East] = [(Direction.East, 1), (Direction.South, 1001), (Direction.North, 1001)],
            [Direction.West] = [(Direction.West, 1), (Direction.South, 1001), (Direction.North, 1001)]
        };

        Loc start = default;
        Loc end = default;

        foreach (var point in map.EnumeratePoints())
        {
            switch (map.Get(point))
            {
                case 'S':
                    start = point;
                    break;
                case 'E':
                    end = point;
                    break;
            }
        }

        PriorityQueue<(Loc Position, Direction Direction), int> cursors = new();
        cursors.Enqueue((start, Direction.East), 0);

        HashSet<(Loc, Direction)> visited = [];

        while (cursors.TryDequeue(out var pos, out var cost))
        {
            if (pos.Position == end)
            {
                return cost;
            }

            visited.Add(pos);

            foreach (var translation in directionMap[pos.Direction])
            {
                var newPos = translation.NewDirection switch
                {
                    Direction.North => pos.Position with { Y = pos.Position.Y - 1 },
                    Direction.East => pos.Position with { X = pos.Position.X + 1 },
                    Direction.South => pos.Position with { Y = pos.Position.Y + 1 },
                    Direction.West => pos.Position with { X = pos.Position.X - 1 },
                    _ => throw new UnreachableException()
                };

                if (!map.Contains(newPos))
                {
                    continue;
                }

                if (map.Get(newPos) is '#')
                {
                    continue;
                }

                var group = (newPos, translation.NewDirection);
                if (visited.Contains(group))
                {
                    continue;
                }

                cursors.Enqueue(group, cost + translation.AddedCost);
            }
        }

        return -1;
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        // input = """
        //         ###############
        //         #.......#....E#
        //         #.#.###.#.###.#
        //         #.....#.#...#.#
        //         #.###.#####.#.#
        //         #.#.#.......#.#
        //         #.#.#####.###.#
        //         #...........#.#
        //         ###.#.#####.#.#
        //         #...#.....#.#.#
        //         #.#.#.###.#.#.#
        //         #.....#...#.#.#
        //         #.###.#.#.#.#.#
        //         #S..#.....#...#
        //         ###############
        //         """;

        var map = new Map(input);

        Dictionary<Direction, (Direction NewDirection, int AddedCost)[]> directionMap = new()
        {
            [Direction.North] = [(Direction.North, 1), (Direction.East, 1001), (Direction.West, 1001)],
            [Direction.South] = [(Direction.South, 1), (Direction.East, 1001), (Direction.West, 1001)],
            [Direction.East] = [(Direction.East, 1), (Direction.South, 1001), (Direction.North, 1001)],
            [Direction.West] = [(Direction.West, 1), (Direction.South, 1001), (Direction.North, 1001)]
        };

        Loc start = default;
        Loc end = default;

        foreach (var point in map.EnumeratePoints())
        {
            switch (map.Get(point))
            {
                case 'S':
                    start = point;
                    break;
                case 'E':
                    end = point;
                    break;
            }
        }

        PriorityQueue<(Loc Position, Direction Direction, HashSet<Loc> Visited), int> cursors = new();
        cursors.Enqueue((start, Direction.East, []), 0);

        HashSet<(Loc, Direction)> visited = [];

        HashSet<Loc> bestTiles = [];

        int cheapestRoute = -1;

        while (cursors.TryDequeue(out var pos, out var cost))
        {
            pos.Visited.Add(pos.Position);

            if (pos.Position == end)
            {
                if (cheapestRoute == -1)
                {
                    cheapestRoute = cost;
                }

                if (cost == cheapestRoute)
                {
                    foreach (var v in pos.Visited) bestTiles.Add(v);
                }
                else
                {
                    return bestTiles.Count;
                }
            }

            visited.Add((pos.Position, pos.Direction));
            pos.Visited.Add(pos.Position);

            foreach (var translation in directionMap[pos.Direction])
            {
                var newPos = translation.NewDirection switch
                {
                    Direction.North => pos.Position with { Y = pos.Position.Y - 1 },
                    Direction.East => pos.Position with { X = pos.Position.X + 1 },
                    Direction.South => pos.Position with { Y = pos.Position.Y + 1 },
                    Direction.West => pos.Position with { X = pos.Position.X - 1 },
                    _ => throw new UnreachableException()
                };

                if (!map.Contains(newPos))
                {
                    continue;
                }

                if (map.Get(newPos) is '#')
                {
                    continue;
                }

                if (visited.Contains((newPos, translation.NewDirection)))
                {
                    continue;
                }

                cursors.Enqueue((newPos, translation.NewDirection, [..pos.Visited]), cost + translation.AddedCost);
            }
        }

        return -1;
    }

    public string PrettyPrint(long output) => output.ToString();

    enum Direction
    {
        North,
        East,
        South,
        West,
    }
}