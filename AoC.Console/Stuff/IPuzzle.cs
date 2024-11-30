namespace AoC.Puzzles;

public interface IPuzzle<TPart1Output, TPart2Output>
{
    TPart1Output SolvePart1(ReadOnlySpan<char> input);
    string PrettyPrint(TPart1Output output);

    TPart2Output SolvePart2(ReadOnlySpan<char> input);
    string PrettyPrint(TPart2Output output);
}

public interface IPuzzle<TOutput> : IPuzzle<TOutput, TOutput>;
