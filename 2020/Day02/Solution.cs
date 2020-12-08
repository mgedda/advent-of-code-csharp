using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day02
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
            int CheckPasswordValidity(string s)
            {
                int low = int.Parse(s.Split('-').First());
                int high = int.Parse(s.Split('-')[1].Split(' ').First());
                char letter = s.Split(' ')[1].TrimEnd(':')[0];
                string password = s.Split(' ')[2];
                int count = password.Count(c => c == letter);

                return count >= low && count <= high ? 1 : 0;
            }

            IEnumerable<int> validPasswordFlags = input.Split("\n").Select(CheckPasswordValidity);
            return validPasswordFlags.Sum();
        }


        int PartTwo(string input)
        {
            int CheckPasswordValidity(string s)
            {
                int i1 = int.Parse(s.Split('-').First());
                int i2 = int.Parse(s.Split('-')[1].Split(' ').First());
                char letter = s.Split(' ')[1].TrimEnd(':')[0];
                string password = s.Split(' ')[2];

                return password[i1 - 1] == letter ^ password[i2 - 1] == letter ? 1 : 0;
            }

            IEnumerable<int> validPasswordFlags = input.Split("\n").Select(CheckPasswordValidity);
            return validPasswordFlags.Sum();
        }
    }
}