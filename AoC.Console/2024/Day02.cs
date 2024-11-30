namespace AoC.Puzzles._2024;

[Puzzle(2024, 2)]
public class Day02 : IPuzzle<DateTime, Day02.Something>
{
    public DateTime SolvePart1(ReadOnlySpan<char> input)
    {
        Task.Delay(1000).Wait();
        return DateTime.Today;
    }

    public string PrettyPrint(DateTime output)
    {
        return output.ToString("O");
    }

    public Something SolvePart2(ReadOnlySpan<char> input)
    {
        return new Something();
    }

    public string PrettyPrint(Something output)
    {
        return output.ToString()!;
    }


    public class Something
    {
    }
}