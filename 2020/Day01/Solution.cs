using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day01
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
            long ComputeResult(ImmutableList<long> values)
            {
                long head = values.First();
                ImmutableList<long> tail = values.RemoveAt(0);

                foreach (long value in tail)
                {
                    if (head + value == 2020)
                    {
                        return head * value;
                    }
                }

                return ComputeResult(tail);
            }

            ImmutableList<long> values = input.Split("\n").Select(l => long.Parse(l)).ToImmutableList();
            return ComputeResult(values);
        }


        long PartTwo(string input)
        {
            long ComputeResult(ImmutableList<long> values)
            {
                long v1 = values.First();
                ImmutableList<long> tail = values.RemoveAt(0);

                foreach (long v2 in tail)
                {
                    foreach (long v3 in tail.RemoveAt(0))
                    {
                        if (v1 + v2 + v3 == 2020)
                        {
                            return v1 * v2 * v3;
                        }
                    }
                }

                return ComputeResult(tail);
            }

            ImmutableList<long> values = input.Split("\n").Select(l => long.Parse(l)).ToImmutableList();
            return ComputeResult(values);
        }
    }
}