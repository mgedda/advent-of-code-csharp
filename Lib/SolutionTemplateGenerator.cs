using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class SolutionTemplateGenerator
    {
        public string Generate(int year, int day)
        {
            return $@"using System;
                 |using System.Collections.Generic;
                 |using System.Collections.Immutable;
                 |using System.Linq;
                 |using System.Text.RegularExpressions;
                 |using System.Text;
                 |
                 |namespace AdventOfCode.Y{year}.Day{day.ToString("00")}
                 |{{
                 |    class Solution : Solver
                 |    {{
                 |        public IEnumerable<object> Solve(string input)
                 |        {{
                 |            yield return PartOne(input);
                 |            yield return PartTwo(input);
                 |        }}
                 |
                 |        int PartOne(string input)
                 |        {{
                 |            return 0;
                 |        }}
                 |
                 |        int PartTwo(string input)
                 |        {{
                 |            return 0;
                 |        }}
                 |    }}
                 |}}".StripMargin();
        }
    }
}