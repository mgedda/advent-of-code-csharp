using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day06
{
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return CountYes(input, (a, b) => a.Union(b));
            yield return CountYes(input, (a, b) => a.Intersect(b));
        }

        private int CountYes(string input, Func<ImmutableHashSet<char>, ImmutableHashSet<char>, ImmutableHashSet<char>> combine)
        {
            return input.Split("\r\n\r\n").Select(group => group.Split("\r\n").Select(line => line.ToImmutableHashSet()).Aggregate(combine).Count()).Sum();
        }
   }
}