﻿using System.Diagnostics;
using AoC.Puzzles;

namespace AoC.Console._2024;

[Puzzle(2024, 04)]
public class Day04 : IPuzzle<long>
{
    public long SolvePart1(ReadOnlySpan<char> input)
    {
        // input = """
        //         MMMSXXMASM
        //         MSAMXMSMSA
        //         AMXSXMAAMM
        //         MSAMASMSMX
        //         XMASAMXAMM
        //         XXAMMXXAMA
        //         SMSMSASXSS
        //         SAXAMASAAA
        //         MAMMMXMMMM
        //         MXMXAXMASX
        //         """;
        var newlineLength = Environment.NewLine.Length;
        var newlineIndex = input.IndexOf(Environment.NewLine);
        var rowCount = (input.Length + newlineLength) / (newlineIndex + (newlineLength));

        var counter = 0;

        Span<(int RowDiff, int ColumnDiff)> directions =
        [
            // Right & left
            (0, 1), (0, -1),
            // Down & up
            (1, 0), (-1, 0),
            // Some diagonal
            (1, 1), (1, -1),
            // The other diagonals
            (-1, 1), (-1, -1)
        ];

        for (var currRow = 0; currRow < rowCount; currRow++)
        {
            for (var currColumn = 0; currColumn < newlineIndex; currColumn++)
            {
                var currIndex = GetIndex(currRow, currColumn);
                Debug.Assert(currIndex is not null);
                if (input[currIndex.Value] != 'X')
                {
                    continue;
                }

                foreach (var (rowDiff, columnDiff) in directions)
                {
                    var m = GetIndex(currRow + rowDiff, currColumn + columnDiff);
                    var a = GetIndex(currRow + (rowDiff * 2), currColumn + (columnDiff * 2));
                    var s = GetIndex(currRow + (rowDiff * 3), currColumn + (columnDiff * 3));

                    if (m is null || a is null || s is null)
                    {
                        continue;
                    }

                    if (input[m.Value] == 'M' && input[a.Value] == 'A' && input[s.Value] == 'S')
                    {
                        counter++;
                    }
                }
            }
        }

        return counter;

        int? GetIndex(int row, int column)
        {
            if (row < 0 || row >= rowCount)
            {
                return null;
            }

            if (column < 0 || column >= newlineIndex)
            {
                return null;
            }

            return column + (row * (newlineIndex + newlineLength));
        }
    }

    public long SolvePart2(ReadOnlySpan<char> input)
    {
          // input = """
          //       MMMSXXMASM
          //       MSAMXMSMSA
          //       AMXSXMAAMM
          //       MSAMASMSMX
          //       XMASAMXAMM
          //       XXAMMXXAMA
          //       SMSMSASXSS
          //       SAXAMASAAA
          //       MAMMMXMMMM
          //       MXMXAXMASX
          //       """;
        var newlineLength = Environment.NewLine.Length;
        var newlineIndex = input.IndexOf(Environment.NewLine);
        var rowCount = (input.Length + newlineLength) / (newlineIndex + newlineLength);

        var counter = 0;

        Span<(int RowDiff, int ColumnDiff)> diagonals =
        [
            ( 1, 1), ( 1, -1),
            (-1, 1), (-1, -1)
        ];

        var totalCount = 0;

        for (var currRow = 0; currRow < rowCount; currRow++)
        {
            for (var currColumn = 0; currColumn < newlineIndex; currColumn++)
            {
                totalCount++;
                var currIndex = GetIndex(currRow, currColumn);
                Debug.Assert(currIndex is not null);
                if (input[currIndex.Value] != 'A')
                {
                    continue;
                }

                var countM = 0;
                var countS = 0;

                foreach (var (rowDiff, columnDiff) in diagonals)
                {
                    var idx = GetIndex(currRow + rowDiff, currColumn + columnDiff);
                    if (idx is null)
                    {
                        break;
                    }

                    switch (input[idx.Value])
                    {
                        case 'M': countM++;
                            break;
                        case 'S': countS++;
                            break;
                    }
                }

                if (countM is 2 && countS is 2)
                {
                    counter++;
                }
            }
        }

        return counter;

        int? GetIndex(int row, int column)
        {
            if (row < 0 || row >= rowCount)
            {
                return null;
            }

            if (column < 0 || column >= newlineIndex)
            {
                return null;
            }

            return column + (row * (newlineIndex + newlineLength));
        }
    }

    public string PrettyPrint(long output) => output.ToString();
}