using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    interface Solver
    {
        IEnumerable<object> Solve(string input);
    }

    static class SolverExtensions
    {
        public static string WorkingDir(int year)
        {
            return Path.Combine(year.ToString());
        }

        public static string WorkingDir(int year, int day)
        {
            return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
        }

        public static int Year(Type t)
        {
            return int.Parse(t.FullName.Split('.')[1].Substring(1));
        }

        public static int Day(Type t)
        {
            return int.Parse(t.FullName.Split('.')[2].Substring(3));
        }

        public static int Year(this Solver solver)
        {
            return Year(solver.GetType());
        }

        public static int Day(this Solver solver)
        {
            return Day(solver.GetType());
        }

        public static string WorkingDir(this Solver solver)
        {
            return WorkingDir(solver.Year(), solver.Day());
        }

        public static string DayName(this Solver solver)
        {
            return $"Day {solver.Day()}";
        }
    }

    class Runner
    {
        public static void RunAll(params Type[] tsolvers)
        {
            var errors = new List<string>();

            var lastYear = -1;
            foreach (var solver in tsolvers.Select(tsolver => Activator.CreateInstance(tsolver) as Solver))
            {
                if (lastYear != solver.Year())
                {
                    Console.WriteLine($@"[{solver.Year()}]
                    >".StripMargin(">"));
                    lastYear = solver.Year();
                }

                var workingDir = solver.WorkingDir();
                Console.WriteLine($"{solver.DayName()}");
                Console.WriteLine();

                foreach (var dir in new[] { workingDir, Path.Combine(workingDir, "test") })
                {
                    if (!Directory.Exists(dir))
                    {
                        continue;
                    }

                    var files = Directory.EnumerateFiles(dir).Where(file => file.EndsWith(".in")).ToArray();
                    foreach (var file in files)
                    {
                        if (files.Count() > 1)
                        {
                            Console.WriteLine("  " + file + ":");
                        }

                        var refoutFile = file.Replace(".in", ".refout");
                        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                        var input  = File.ReadAllText(file);
                        if (input.EndsWith("\n"))
                        {
                            input = input.Substring(0, input.Length - 1);
                        }
                        
                        var dt = DateTime.Now;
                        var iline = 0;
                        foreach (var line in solver.Solve(input))
                        {
                            var now = DateTime.Now;
                            var (status, err) = refout == null || refout.Length <= iline
                                ? ("?", null) 
                                : refout [iline] == line.ToString()
                                    ? ("correct", null)
                                    : ("wrong", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");
                        
                            if (err != null)
                            {
                                errors.Add(err);
                            }

                            Console.Write($"  {status}");
                            Console.Write($" {line} ");
                            var diff = (now - dt).TotalMilliseconds;
                            Console.WriteLine($"({diff.ToString("F3")} ms)");

                            dt = now;
                            iline++;
                        }
                    }
                }

                Console.WriteLine();
            }

            if (errors.Any())
            {
                Console.WriteLine("Errors:\n" + string.Join("\n", errors));
            }
        }
    }
}