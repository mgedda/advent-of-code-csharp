using System;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Updater
    {
        public async Task Update(int year, int day)
        {
            var dir = Dir(year, day);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }            

            UpdateInput(year, day);
            UpdateRefout(year, day);
            UpdateSolutionTemplate(year, day);
        }

        void WriteFile(string file, string content)
        {
            Console.WriteLine($"Writing {file}");
            File.WriteAllText(file, content);
        }

        string Dir(int year, int day) => SolverExtensions.WorkingDir(year, day);
    

        void UpdateInput(int year, int day)
        {
            var file = Path.Combine(Dir(year, day), "input.in");
            WriteFile(file, "<replace with puzzle input>");
        }

        void UpdateRefout(int year, int day)
        {
            var file = Path.Combine(Dir(year, day), "input.refout");
            WriteFile(file, "<replace with solver output>");
        }

        void UpdateSolutionTemplate(int year, int day)
        {
            var file = Path.Combine(Dir(year, day), "Solution.cs");
            if (!File.Exists(file))
            {
                WriteFile(file, new SolutionTemplateGenerator().Generate(year, day));
            }
        }
    }
}