using System.Diagnostics;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using AoC.Puzzles;

namespace AoC.Console._2024;

[Puzzle(2024, 06)]
public class Day06 : IPuzzle<long>
{
    readonly ref struct Map
    {
        private static readonly int NewLineLength = Environment.NewLine.Length;
        private readonly Span<char> _data;
        private readonly int _width;
        private readonly int _height;

        public Map(Span<char> input)
        {
            _data = input;

            _width = input.IndexOf(Environment.NewLine);
            _height = (input.Length + NewLineLength) / (_width + NewLineLength);
        }

        public void PutObstacle((int X, int Y) location)
        {
            Debug.Assert(ReadAt(location) is '.' or '^');
            _data[GetIndex(location)] = '#';
        }

        public void RemoveObstacle((int X, int Y) location)
        {
            Debug.Assert(ReadAt(location) is '#');
            _data[GetIndex(location)] = '.';
        }

        public (int X, int Y) GetStartingPos()
        {
            var idx = _data.IndexOf('^');
            var x = idx % (_width + NewLineLength);

            var y = (idx - x) / (_width + NewLineLength);

            return (x, y);
        }

        public char? ReadAt((int x, int y) pos)
        {
            if (pos.x < 0 || pos.x >= _width)
            {
                return null;
            }

            if (pos.y < 0 || pos.y >= _height)
            {
                return null;
            }

            return _data[GetIndex(pos)];
        }

        private int GetIndex((int x, int y) pos)
        {
            return pos.x + ((_width + NewLineLength) * pos.y);
        }
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    private static readonly Dictionary<Direction, (int DX, int DY)> directionDxDy = new()
    {
        { Direction.Up, (0, -1) },
        { Direction.Right, (1, 0) },
        { Direction.Down, (0, 1) },
        { Direction.Left, (-1, 0) },
    };

    private static readonly Dictionary<Direction, Direction> turnRight = new()
    {
        { Direction.Up, Direction.Right },
        { Direction.Right, Direction.Down },
        { Direction.Down, Direction.Left },
        { Direction.Left, Direction.Up },
    };

    ref struct Guard
    {
        private readonly Map _map;

        private Direction _direction;
        private (int X, int Y) _position;
        private readonly HashSet<(int X, int Y)> _hasVisited;

        public int VisitedCount => _hasVisited.Count;
        public IReadOnlySet<(int X, int Y)> VisitedSpots => _hasVisited;

        public Guard(
            Map map,
            (int X, int Y) position,
            Direction direction)
        {
            _map = map;
            _position = position;
            _direction = direction;
            _hasVisited = [_position];
        }

        public bool StepIntoSomething()
        {
            var step = directionDxDy[_direction];
            var attemptingToWalkTo = (_position.X + step.DX, _position.Y + step.DY);

            var newChar = _map.ReadAt(attemptingToWalkTo);
            switch (newChar)
            {
                case '.' or '^':
                    _position = attemptingToWalkTo;
                    _hasVisited.Add(_position);
                    break;
                case '#':
                    _direction = turnRight[_direction];
                    break;
                case null:
                    break;
                default: throw new ArgumentOutOfRangeException(newChar.ToString(), nameof(newChar));
            }

            return newChar is not null;
        }
    }

    public long SolvePart1(ReadOnlySpan<char> input)
    {
        var writableMap = new char[input.Length];
        input.CopyTo(writableMap);

        var map = new Map(writableMap);

        var littleGuy = new Guard(map, map.GetStartingPos(), Direction.Up);

        while (littleGuy.StepIntoSomething())
        {

        }

        return littleGuy.VisitedCount;
    }

    ref struct StuckGuard
    {
        private readonly Map _map;

        private Direction _direction;
        private (int X, int Y) _position;
        private readonly HashSet<((int X, int Y) Position, Direction direction)> _hasVisited;

        public StuckGuard(
            Map map,
            (int X, int Y) position,
            Direction direction)
        {
            _map = map;
            _position = position;
            _direction = direction;
            _hasVisited = [(_position, _direction)];
        }

        public bool? StepAndHasVisitedBefore()
        {
            var step = directionDxDy[_direction];
            var attemptingToWalkTo = (_position.X + step.DX, _position.Y + step.DY);

            var newChar = _map.ReadAt(attemptingToWalkTo);
            switch (newChar)
            {
                case '.' or '^':
                    _position = attemptingToWalkTo;
                    break;
                case '#':
                    _direction = turnRight[_direction];
                    break;
                case null: return null;
                default: throw new ArgumentOutOfRangeException(newChar.ToString(), nameof(newChar));
            }

            return !_hasVisited.Add((_position, _direction));
        }
    }


    public long SolvePart2(ReadOnlySpan<char> input)
    {
        // input = """
        //         ....#.....
        //         .........#
        //         ..........
        //         ..#.......
        //         .......#..
        //         ..........
        //         .#..^.....
        //         ........#.
        //         #.........
        //         ......#...
        //         """;

        var writableMap = new char[input.Length];
        input.CopyTo(writableMap);

        var map = new Map(writableMap);

        var startingPosition = map.GetStartingPos();
        var startingDirection = Direction.Up;

        var littleGuy = new Guard(map, startingPosition, startingDirection);

        while (littleGuy.StepIntoSomething())
        {
        }

        var allVisitedSpots = littleGuy.VisitedSpots;

        var unversesWhereItLoops = 0;

        foreach (var possibleBarrier in allVisitedSpots)
        {
            map.PutObstacle(possibleBarrier);

            var newGuard = new StuckGuard(map, startingPosition, startingDirection);

            while (true)
            {
                var outcome = newGuard.StepAndHasVisitedBefore();
                if (outcome is null)
                {
                    break;
                }

                if (outcome is true)
                {
                    unversesWhereItLoops++;
                    break;
                }
            }

            map.RemoveObstacle(possibleBarrier);
        }

        return unversesWhereItLoops;
    }

    public string PrettyPrint(long output) => output.ToString();
}