using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    class App
    {
        static void Main(string[] args)
        {
            var tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToArray();

            var action =
                // Generate solution template for [year]/[day]
                Command(args, Args("update", "([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[1]);
                    var day = int.Parse(m[2]);
                    return () => new Updater().Update(year, day).Wait();
                }) ??
                Command(args, Args("([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[0]);
                    var day = int.Parse(m[1]);
                    var tSolversSelected = tsolvers.First(tSolver =>
                    SolverExtensions.Year(tSolver) == year &&
                    SolverExtensions.Day(tSolver) == day);
                    return () => Runner.RunAll(tSolversSelected);
                }) ??
                Command(args, Args("[0-9]+"), m => {
                    var year = int.Parse(m[0]);
                    var tSolversSelected = tsolvers.Where(tSolver =>
                    SolverExtensions.Year(tSolver) == year);
                    return () => Runner.RunAll(tSolversSelected.ToArray());
                }) ??
                Command(args, Args("last"), m => {
                    var tSolversSelected = tsolvers.Last();
                    return () => Runner.RunAll(tSolversSelected);
                }) ??
                Command(args, Args("all"), m => {
                    return () => Runner.RunAll(tsolvers);
                }) ??
                new Action(() => {
                    Console.WriteLine(Usage.Get());
                });

            action();
        }

        static Action Command(string[] args, string[] regexes, Func<string[], Action> parse)
        {
            if (args.Length != regexes.Length)
            {
                // Input arguments do not correspond to number of regular expressions.
                return null;
            }

            IEnumerable<Match> matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));

            if (!matches.All(match => match.Success))
            {
                // Not all regular expressions matched the input arguments.
                return null;
            }

            try
            {
                // SelectMany = mapFlatten
                string[] input = matches.SelectMany(m => m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) : new[] { m.Value }).ToArray();
                return parse(input);
            }
            catch
            {
                return null;
            }
        }

        static string[] Args(params string[] regex)
        {
            return regex;
        }
    }

    public class Usage
    {
        public static string Get(){
            return $@"
               > Usage: dotnet run [arguments]
               > Supported arguments:

               >  [year]/[day]   Solve the specified problems
               >  [year]         Solve the whole year
               >  last           Solve the last problem
               >  all            Solve everything

               > To start working on new problems:
               > run the app with

               >  update [year]/[day]   Prepares a folder for the given day, writes an input file (you
               >                        need to fill in the puzzle input manually), and creates a
               >                        solution template.
               > ".StripMargin("> ");
        }
    }

}
