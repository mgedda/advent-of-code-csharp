using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day05
{
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
        {
            return input.Split("\r\n").Select(GetSeatID).Max();
        }

        int PartTwo(string input)
        {
            ImmutableList<int> sortedIDs = input.Split("\r\n").Select(GetSeatID).ToImmutableList().Sort();

            for (int i = 1; i < sortedIDs.Count; i++)
            {
                if (sortedIDs[i] != sortedIDs[i - 1] + 1)
                {
                    return sortedIDs[i] - 1;
                }
            }
            return -1;
        }

        private static int GetSeatID(string line)
        {
            return GetRow(line) * 8 + GetColumn(line);
        }

        private static int GetRow(string line)
        {
            return GetPosition(line, new int[] { 64, 32, 16, 8, 4, 2, 1 }, 'B');
        }

        private static int GetColumn(string line)
        {
            return GetPosition(line.Substring(7, 3), new int[] { 4, 2, 1 }, 'R');
        }

        private static int GetPosition(string characters, int[] terms, char upper)
        {
            int row = 0;
            for (int i = 0; i < terms.Length; i++)
            {
                row += characters[i] == upper ? terms[i] : 0;
            }
            return row;
        }
    }
}