using System.Numerics;

namespace AoC.Console.Utilities;

using Point2D = (int X, int Y);

public readonly ref struct Map
{
    private readonly ReadOnlySpan<char> _map;

    private static readonly int NewLineLength = Environment.NewLine.Length;

    public Map(ReadOnlySpan<char> map)
    {
        _map = map;
        Width = map.IndexOf(Environment.NewLine);
        Height = (map.Length + NewLineLength) / (Width + NewLineLength);
    }

    public int Width { get; }

    public int Height { get; }

    public char Get(Point2D loc)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(loc.X);
        ArgumentOutOfRangeException.ThrowIfNegative(loc.Y);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(loc.X, Width);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(loc.Y, Height);

        return _map[loc.X + (loc.Y * (Width + NewLineLength))];
    }

    public bool Contains(Point2D newPos)
    {
        return newPos.X >= 0 && newPos.X < Width && newPos.Y >= 0 && newPos.Y < Height;
    }

    public PointEnumerable EnumeratePoints() => new PointEnumerable(Width, Height);

    public ref struct PointEnumerable(int width, int height)
    {
        public PointEnumerator GetEnumerator() => new PointEnumerator(width, height);
    }

    public ref struct PointEnumerator(int width, int height)
    {
        private int _x = 0;
        private int _y = 0;

        public Point2D Current => (_x, _y);

        public bool MoveNext()
        {
            if (_x == width - 1)
            {
                if (_y == height - 1)
                {
                    return false;
                }

                _x = 0;
                _y++;
            }

            _x++;
            return true;
        }
    }
}