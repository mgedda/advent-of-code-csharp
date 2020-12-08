using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class StringExtensions
    {
        public static string StripMargin(this string st, string margin = "|")
        {
            return string.Join("\n",
                from line in Regex.Split(st, "\r?\n")
                select Regex.Replace(line, @"^\s*"+Regex.Escape(margin), "")
            );
        }
    }
}