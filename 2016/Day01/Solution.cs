using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day01
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
            ImmutableList<string> steps = input.Split(' ').Select(s => s.TrimEnd(',')).ToImmutableList();

            ImmutableList<Movement> moves = steps.Select(
                s => s[0] == 'L'
                    ? (Movement) new Left {Steps = int.Parse(s.Substring(1, s.Length - 1))}
                    : (Movement) new Right {Steps = int.Parse(s.Substring(1, s.Length - 1))})
                .ToImmutableList<Movement>();

            Position startPosition = new Position {X = 0, Y = 0};
            Direction direction = new Direction {X = 0, Y = 1};  // Pointing north

            Position endPosition = Walk(moves, startPosition, direction);

            return Math.Abs(endPosition.X) + Math.Abs(endPosition.Y);
        }

        int PartTwo(string input)
        {
            return 0;
        }

        private Position Walk(ImmutableList<Movement> moves, Position p, Direction d)
        {
            if (moves.IsEmpty)
            {
                return p;
            }

            switch(moves.First()) 
            {
                case Left m:
                    return Walk(moves.RemoveAt(0), p + new Direction { X = -d.Y, Y = d.X } * m.Steps, new Direction { X = -d.Y, Y = d.X });

                case Right m:
                    return Walk(moves.RemoveAt(0), p + new Direction { X = d.Y, Y = -d.X } * m.Steps, new Direction { X = d.Y, Y = -d.X });

                default:
                    return p;
            };
        }

    }


    internal class Movement { internal int Steps { get; set; } = 0; }

    internal class Left : Movement { }

    internal class Right : Movement { }

    internal class Position
    {
        internal int X { get; set;  } = 0;
        internal int Y { get; set;  } = 0;

        public static Position operator +(Position a, Position b) => new Position { X = a.X + b.X, Y = a.Y + b.Y };
    }

    internal class Direction
    {
        internal int X { get; set; } = 0;
        internal int Y { get; set; } = 0;

        public static Position operator *(Direction a, int steps) => new Position { X = a.X * steps, Y = a.Y * steps };
    }
}