using System.Collections;
using System.Diagnostics;
using AoC.Puzzles;
using Microsoft.VisualBasic.CompilerServices;

namespace AoC.Console._2024;

[Puzzle(2024, 07)]
public class Day07 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        // input = """
        //         190: 10 19
        //         3267: 81 40 27
        //         83: 17 5
        //         156: 15 6
        //         7290: 6 8 6 15
        //         161011: 16 10 13
        //         192: 17 8 14
        //         21037: 9 7 18 13
        //         292: 11 6 16 20
        //         """;

        var equations = ParseEquations(input);

        ReadOnlySpan<Operator> operators = [Operator.Add, Operator.Multiply];

        long sum = 0;

        foreach (var eq in equations)
        {
            if (CanParse(eq, operators))
            {
                sum += eq.TestValue;
            }
        }

        return sum;
    }

    private static bool CanParse(Equation equation, ReadOnlySpan<Operator> operators)
    {
        return AddsUp(equation.TestValue, 0, equation.Numbers, operators);
    }

    private static bool AddsUp(long testValue, long currValue, ReadOnlySpan<long> numbers, ReadOnlySpan<Operator> operators)
    {
        if (numbers.Length == 0)
        {
            return currValue == testValue;
        }

        var nextNum = numbers[0];

        foreach (var op in operators)
        {
            var newVal = op switch
            {
                Operator.Add => currValue + nextNum,
                Operator.Multiply => currValue * nextNum,
                Operator.Concat => ((long)Math.Pow(10, Math.Floor(Math.Log10(nextNum)) + 1) * currValue) + nextNum,
                _ => throw new UnreachableException(),
            };

            if (AddsUp(testValue, newVal, numbers[1..], operators))
            {
                return true;
            }
        }

        return false;
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
        // input = """
        //         190: 10 19
        //         3267: 81 40 27
        //         83: 17 5
        //         156: 15 6
        //         7290: 6 8 6 15
        //         161011: 16 10 13
        //         192: 17 8 14
        //         21037: 9 7 18 13
        //         292: 11 6 16 20
        //         """;

        var equations = ParseEquations(input);

        ReadOnlySpan<Operator> operators = [Operator.Add, Operator.Multiply, Operator.Concat];

        long sum = 0;

        foreach (var eq in equations)
        {
            if (CanParse(eq, operators))
            {
                sum += eq.TestValue;
            }
        }

        return sum;
    }

    private static List<Equation> ParseEquations(ReadOnlySpan<char> input)
    {
        return input.ToString().Split(Environment.NewLine)
            .Select(line =>
            {
                var parts = line.Split(": ");
                var expected = long.Parse(parts[0]);
                var numbers = parts[1].Split(' ').Select(long.Parse).ToArray();

                return new Equation(expected, numbers);
            })
            .ToList();
    }

    public string PrettyPrint(long output) => output.ToString();

    record Equation(long TestValue, long[] Numbers);

    enum Operator
    {
        Add,
        Multiply,
        Concat,
    }
}