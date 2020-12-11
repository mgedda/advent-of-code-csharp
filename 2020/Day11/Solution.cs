using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day11
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
            int neighborThreshold = 4;

            var lines = input.Split("\r\n");
            var seatIndices = GetSeatIndices(input);
            var emptySeatMap = new SeatMap(lines[0].Length, lines.Count(), seatIndices);
            var fullSeatMap = new SeatMap(emptySeatMap._xs, emptySeatMap._ys, seatIndices, true);
            var steadyStateMap = FindSteadyState(emptySeatMap, fullSeatMap, GetNeighboursAdjacent, neighborThreshold);

            return steadyStateMap._occupied.Count(c => c);
        }

        IEnumerable<int> GetSeatIndices(string input)
        {
            var seats = input.Where(c => c == '.' || c == 'L');
            return seats.Zip(Enumerable.Range(0, seats.Count())).Where(s => s.ToTuple().Item1 == 'L').Select(s => s.ToTuple().Item2);
        }

        private Func<SeatMap, int, int, IEnumerable<(int, int)>> GetNeighboursAdjacent = (seatMap, x, y) => {
            var neighbors = new List<(int, int)>();
            for (int j = y - 1; j <= y + 1; j++)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    if (!(i == x && j == y) && i >= 0 && i < seatMap._xs && j >= 0 && j < seatMap._ys)
                    {
                        neighbors.Add((i, j));
                    }
                }
            }
            return neighbors;
        };

        private SeatMap FindSteadyState(SeatMap current, SeatMap previous, Func<SeatMap, int, int, IEnumerable<(int, int)>> GetNeighbors, int threshold)
        {
            if (current.IsEqual(previous))
            {
                return current;
            }

            return FindSteadyState(current.Update(GetNeighbors, threshold), current, GetNeighbors, threshold);
        } 

        int PartTwo(string input)
        {
            int neighborThreshold = 5;

            var lines = input.Split("\r\n");
            var seatIndices = GetSeatIndices(input);
            var emptySeatMap = new SeatMap(lines[0].Length, lines.Count(), seatIndices);
            var fullSeatMap = new SeatMap(emptySeatMap._xs, emptySeatMap._ys, seatIndices, true);
            var steadyStateMap = FindSteadyState(emptySeatMap, fullSeatMap, GetNeighboursDirectional, neighborThreshold);

            return steadyStateMap._occupied.Count(c => c);
        }

        private Func<SeatMap, int, int, IEnumerable<(int, int)>> GetNeighboursDirectional = (seatMap, x, y) => {
            (int, int) GetFirstSeatInDirection(SeatMap seatMap, int x, int y, (int, int) direction)
            {
                if (x < 0 || x >= seatMap._xs || y < 0 || y >= seatMap._ys)
                {
                    return (-1, -1);
                }

                if (seatMap.isSeat(x, y))
                {
                    return (x, y);
                }

                return GetFirstSeatInDirection(seatMap, x + direction.Item1, y + direction.Item2, direction);
            }

            List<(int, int)> directions = new List<(int, int)> { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)};

            return directions.Select(direction => GetFirstSeatInDirection(seatMap, x + direction.Item1, y + direction.Item2, direction)).Where(c => c.Item1 != -1); 
        };

    }

    internal class SeatMap
    {
        public bool[] _occupied { get; init; } // true: occupied, false: vacant
        public bool[] _seats { get; init; } // true: chair, false: no chair
        public readonly int _xs;
        public readonly int _ys;

        public SeatMap(int xs, int ys, IEnumerable<int> seatIndices, bool val = false)
        {
            _xs = xs;
            _ys = ys;
            _occupied = new bool[_xs * _ys];
            Array.Fill(_occupied, val);

            _seats = new bool[_xs * _ys];
            seatIndices.ToList().ForEach(i => _seats[i] = true);
        }

        public SeatMap(bool[] occupied, bool[] seats, int xs, int ys)
        {
            _occupied = occupied;
            _seats = seats;
            _xs = xs;
            _ys = ys;
        }

        public bool IsOccupied(int x, int y)
        {
            return _occupied[x + y * _xs];
        }

        public bool isSeat(int x, int y)
        {
            return _seats[x + y * _xs];
        }

        internal SeatMap Update(Func<SeatMap, int, int, IEnumerable<(int, int)>> GetNeighbors, int threshold)
        {
            var occupied = GetUpdatedOccupiedMap(GetNeighbors, threshold);
            return new SeatMap(occupied, _seats, _xs, _ys);            
        }

        internal bool[] GetUpdatedOccupiedMap(Func<SeatMap, int, int, IEnumerable<(int, int)>> GetNeighbors, int threshold)
        {
            bool[] copy = new bool[_occupied.Length];
            Array.Copy(_occupied, copy, _occupied.Length);
            Enumerable.Range(0, _seats.Length).ToList().ForEach(i => copy[i] = GetNewOccupiedState(i, GetNeighbors, threshold));
            return copy;
        }

        internal bool GetNewOccupiedState(int i, Func<SeatMap, int, int, IEnumerable<(int, int)>> GetNeighbors, int threshold)
        {
            (int x, int y) = (i % _xs, i / _xs);

            if (!isSeat(x, y))
            {
                return false;
            }

            var neighbors = GetNeighbors(this, x, y);
            var numOccupiedNeighbours = neighbors.Select(n => IsOccupied(n.Item1, n.Item2)).Count(c => c);
            
            if (!IsOccupied(x, y) && numOccupiedNeighbours == 0)
            {
                return true;
            }
            if (IsOccupied(x, y) && numOccupiedNeighbours >= threshold)
            {
                return false;
            }
            return IsOccupied(x, y);
        } 

        internal bool IsEqual(SeatMap other)
        {
            return Enumerable.SequenceEqual(_occupied, other._occupied);
        }
    }
}