namespace AoC.Puzzles;

[AttributeUsage(AttributeTargets.Class)]
public class PuzzleAttribute(int year, int day) : Attribute
{
    public int Year { get; } = year;
    public int Day { get; } = day;
}