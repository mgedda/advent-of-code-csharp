using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day04
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
            var passports = ParsePassports(input.Split("\r\n"));
            string[] requiredFields = {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
            int numPassportsWithRequiredFields = passports.Select(p => requiredFields.All(key => p.ContainsKey(key))).Count(c => c);
            return numPassportsWithRequiredFields;
        }

        int PartTwo(string input)
        {
            return 0;
        }

        private static IEnumerable<Dictionary<string, string>> ParsePassports(IEnumerable<string> lines)
        {
            List<List<(string, string)>> passportsTempContainer = new List<List<(string, string)>> { new List<(string, string)>() };

            foreach (string line in lines)
            {
                if (String.Equals(line, String.Empty))
                {
                    passportsTempContainer.Add(new List<(string, string)>());
                }
                else
                {
                    List<(string, string)> tuples = line.Split(' ').Select(s => s.Split(':')).Select(s => (s[0], s[1])).ToList();
                    passportsTempContainer.Last().AddRange(tuples);
                }
            }

            return passportsTempContainer.Select(p => p.ToDictionary(x => x.Item1, x => x.Item2));
        }
    }
}