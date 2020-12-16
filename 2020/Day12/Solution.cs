using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day12
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
            var boat = new Boat();
            boat.Move(input.Split("\r\n"));
            return boat.Distance();
        }

        int PartTwo(string input)
        {
            var boat = new BoatWithWaypoint();
            boat.Update(input.Split("\r\n"));
            return boat.Distance();
        }
    }

    internal class Boat
    {
        private (int x, int y) Pos = (0, 0);
        private (int x, int y) Dir = (1, 0);   // facing east

        public void Move(IEnumerable<string> actions)
        {
            actions.ToList().ForEach(action => {
                int v = int.Parse(action.Substring(1));
                switch (action[0]) {
                    case 'N':
                        Pos = (Pos.x, Pos.y + v);
                        break;
                    case 'S':
                        Pos = (Pos.x, Pos.y - v);
                        break;
                    case 'E':
                        Pos = (Pos.x + v, Pos.y);
                        break;
                    case 'W':
                        Pos = (Pos.x - v, Pos.y);
                        break;
                    case 'F':
                        Pos = (Pos.x + v * Dir.x, Pos.y + v * Dir.y);
                        break;
                    case 'L':
                        Dir = v == 90 ? (-Dir.y, Dir.x) : v == 180 ? (-Dir.x, -Dir.y) : (Dir.y, -Dir.x);
                        break;
                    case 'R':
                        Dir = v == 90 ? (Dir.y, -Dir.x) : v == 180 ? (-Dir.x, -Dir.y) : (-Dir.y, Dir.x);
                        break;
                }
            });
        }

        public int Distance()
        {
            return Math.Abs(Pos.x) + Math.Abs(Pos.y);
        }
    }

    internal class BoatWithWaypoint
    {
        private (int x, int y) Pos = (0, 0);
        private (int x, int y) Waypoint = (10, 1);

        public void Update(IEnumerable<string> actions)
        {
            (int, int) Rotate((int x, int y) v, int degrees)
            {
                return ((v.x * Math.Cos(degrees.ToRadians()) - v.y * Math.Sin(degrees.ToRadians())).ToInt(),
                        (v.x * Math.Sin(degrees.ToRadians()) + v.y * Math.Cos(degrees.ToRadians())).ToInt());
            }

            (int, int) ComputeWaypoint(char action, int v) => action switch {
                    'N' => (Waypoint.x, Waypoint.y + v),
                    'S' => (Waypoint.x, Waypoint.y - v),
                    'E' => (Waypoint.x + v, Waypoint.y),
                    'W' => (Waypoint.x - v, Waypoint.y),
                    'L' => Rotate(Waypoint, v),
                    'R' => Rotate(Waypoint, 360 - v),
                    _ => throw new ArgumentException(),
            };

            actions.ToList().ForEach(action => {
                int v = int.Parse(action.Substring(1));
                char a = action[0];
                switch (a) {
                    case 'F':
                        Pos = (Pos.x + v * Waypoint.x, Pos.y + v * Waypoint.y);
                        break;
                    default:
                        Waypoint = ComputeWaypoint(a, v);
                        break;
                }
            });
        }

        public int Distance()
        {
            return Math.Abs(Pos.x) + Math.Abs(Pos.y);
        }
   }

    public static class NumericExtensions
    {
        public static double ToRadians(this int val)
        {
            return (Math.PI / 180) * (double)val;
        }
        public static int ToInt(this double val)
        {
            return (int)Math.Round(val);
        }
    }
}