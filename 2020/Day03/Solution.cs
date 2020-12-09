using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AdventOfCode.Y2020.Day03
{
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input)
        {
            return CountTrees(input.Split("\r\n"), (3, 1));
        }

        long PartTwo(string input)
        {
            string[] lines = input.Split("\r\n");
            var slopes = new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            IEnumerable<long> treesForAllSlopes = slopes.Select(slope => CountTrees(lines, slope));
            long result = treesForAllSlopes.Aggregate((long)1, (acc, trees) => acc * trees);

            return result;
        }

        long CountTrees(string[] lines, (int, int) slope)
        {
            (int slopeX, int slopeY) = slope;
            TreeMap treeMap = new TreeMap(lines);
            //treeMap.Print();
            int steps = (treeMap.ys - 1) / slopeY;
            IEnumerable<(int, int)> stepCoords = Enumerable.Range(1, steps).Select(step => (step * slopeX, step * slopeY));
            IEnumerable<long> trees = stepCoords.Select(c => (long)treeMap.Get(c));
            //treeMap.PrintWithPositions(stepCoords);

            return trees.Sum();
        }
    }

    internal class TreeMap
    {
        internal int xs { get; set; }
        internal int ys { get; set; }

        internal int[] map;

        public TreeMap(string[] lines)
        {
            xs = lines[0].Length;
            ys = lines.Length;

            map = new int[xs * ys];

            foreach (int y in Enumerable.Range(0, ys))
            {
                foreach (int x in Enumerable.Range(0, xs))
                {
                    map[x + y * xs] = lines[y][x] == '#' ? 1 : 0;
                }
            }
        }

        public void Print()
        {
            foreach (int y in Enumerable.Range(0, ys))
            {
                foreach (int x in Enumerable.Range(0, xs))
                {
                    char c = map[x + y * xs] == 1 ? '#' : '.';
                    Console.Write(c);
                }
                Console.Write("\r\n");
            }
        }
        public void PrintWithPositions(IEnumerable<(int, int)> coords)
        {
            int maxX = coords.Select(c => c.Item1).Max();

            foreach (int y in Enumerable.Range(0, ys))
            {
                foreach (int x in Enumerable.Range(0, maxX + 1))
                {
                    bool stepsOnSpot = coords.Any(c => c == (x, y));

                    char c = map[(x % xs) + y * xs] == 1
                        ? stepsOnSpot ? 'X' : '#'
                        : stepsOnSpot ? 'O' : '.';
                    Console.Write(c);
                }
                Console.Write("\r\n");
            }
        }

        public int Get((int, int) coord)
        {
            int x = coord.Item1;
            int y = coord.Item2;
            Debug.Assert(y < ys);
            return map[(x % xs) + y * xs];
        }
    }
}