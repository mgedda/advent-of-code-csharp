using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day08
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
            var program = ParseInput(input);
            return RunProgram(program, 0, 0, ImmutableHashSet<int>.Empty).Item2;
        }

        int PartTwo(string input)
        {
            var program = ParseInput(input);
            var programs = AlterProgram(program, 0, ImmutableHashSet<ImmutableArray<Instruction>>.Empty);
            return GetAccumulatorForTerminatingProgram(programs);
        }

        private int GetAccumulatorForTerminatingProgram(ImmutableHashSet<ImmutableArray<Instruction>> programs)
        {
            foreach (var p in programs)
            {
                (int i, int accumulator) = RunProgram(p, 0, 0, ImmutableHashSet<int>.Empty);
                if (i == p.Length) {
                    return accumulator;
                }
            }

            throw new IndexOutOfRangeException();
        }

        (int, int) RunProgram(ImmutableArray<Instruction> program, int i, int accumulator, ImmutableHashSet<int> visited)
        {
            if (visited.Contains(i) || i == program.Length) {
                return (i, accumulator);
            }

            return program[i] switch {
                { Code: "acc" } => RunProgram(program, i + 1, accumulator + program[i].Value, visited.Add(i)),
                { Code: "jmp" } => RunProgram(program, i + program[i].Value, accumulator, visited.Add(i)),
                { Code: "nop" } => RunProgram(program, i + 1, accumulator, visited.Add(i)),
                _ => throw new IndexOutOfRangeException()
            };
        }

        ImmutableHashSet<ImmutableArray<Instruction>> AlterProgram(ImmutableArray<Instruction> program, int i, ImmutableHashSet<ImmutableArray<Instruction>> programs)
        {
            if (i == program.Length) {
                return programs;
            }

            return program[i] switch {
                { Code: "nop" } => AlterProgram(program, i + 1, programs.Add(program.SetItem(i, new Instruction { Code = "jmp", Value = program[i].Value }))),
                { Code: "jmp" } => AlterProgram(program, i + 1, programs.Add(program.SetItem(i, new Instruction { Code = "nop", Value = 0 } ))),
                _ => AlterProgram(program, i + 1, programs)
            };
        }

        private ImmutableArray<Instruction> ParseInput(string input)
        {
            return input.Split("\n").Select(row => new Instruction { Code = row.Split(' ')[0], Value = int.Parse(row.Split(' ')[1]) } ).ToImmutableArray();
        }
    }

    internal class Instruction
    {
        internal string Code { get; init; } 
        internal int Value { get; init; } 
    }
}