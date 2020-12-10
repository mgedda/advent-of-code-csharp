using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day09
{
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input)
        {
            int preambleSize = 25;
            var values = ParseInput(input);            
            return FindNonConformativeValue(values, preambleSize, preambleSize).Item1;
        }

        long PartTwo(string input)
        {
            int preambleSize = 25;
            var values = ParseInput(input);            
            (long nonConformativeValue, int index) = FindNonConformativeValue(values, preambleSize, preambleSize);
            long sum = FindSum(values.AsSpan().Slice(0, index), nonConformativeValue);
            return sum;
        }


        private ImmutableArray<long> ParseInput(string input)
        {
            return input.Split("\r\n").Select(v => long.Parse(v)).ToImmutableArray();
        }

        private (long, int) FindNonConformativeValue(ImmutableArray<long> values, int preambleSize, int i)
        {
            if (!IsSumOfTwoDistinctValues(values[i], values.AsSpan().Slice(i - preambleSize, preambleSize))) {
                return (values[i], i);
            }
            return FindNonConformativeValue(values, preambleSize, i + 1);
        }

        private bool IsSumOfTwoDistinctValues(long value, ReadOnlySpan<long> values)
        {
            for (int i = 0; i < values.Length - 1; i++) {
                for (int j = i + 1; j < values.Length; j++) {
                    if (values[i] != values[j] && values[i] + values[j] == value) {
                        return true;
                    }
                }
            }
            return false;
        }

        private long FindSum(ReadOnlySpan<long> values, long nonConformativeValue)
        {
            long sum = -1;
            int i = 0;
            do
            {
                sum = FindSum(values, i, long.MaxValue, long.MinValue, 0, nonConformativeValue);
                i++;
            } while (sum < 0 && i < values.Length);
            return sum;
        }

        private long FindSum(ReadOnlySpan<long> values, int i, long min, long max, long acc, long nonConformativeValue)
        {
            if (acc == nonConformativeValue) {
                return min + max;
            }

            if (acc > nonConformativeValue || i >= values.Length) {
                return -1;
            }

            long v = values[i];
            return FindSum(values, i + 1, v < min ? v : min, v > max ? v : max, acc + v, nonConformativeValue);
        }
    }
}