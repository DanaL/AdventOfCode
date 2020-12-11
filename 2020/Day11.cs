using System;
using System.Collections.Generic;
using System.IO;

namespace _2020
{
    public class Day11
    {
        private static readonly int SEAT = 1;
        private static readonly int OCCUPIED = 2;

        public Day11() { }

        private (int, int)[] _dirs = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

        private bool same(int[,] g0, int[,] g1, HashSet<(int, int)> seats)
        {
            foreach (var seat in seats) {
                if (g0[seat.Item1, seat.Item2] != g1[seat.Item1, seat.Item2])
                    return false;
            }

            return true;
        }

        private int raycast(int[,] grid, int row, int col, (int, int) dir, int range, int width)
        {
            for (int j = 0; j < range; j++) {
                row += dir.Item1;
                col += dir.Item2;

                if (row < 0 || col < 0 || row >= width || col >= width)
                    return 0;
                if (grid[row, col] == OCCUPIED)
                    return 1;
                if (grid[row, col] == SEAT)
                    return 0;
            }

            return 0;
        }

        // Part 1 is just the case of Part 2's raycasting, but with a range of 1
        private int[,] iterate(int[,] grid, HashSet<(int, int)> seats, int tolerance, int range, int width)
        {
            int[,] next = new int[width, width];

            foreach (var seat in seats) {                                   
                // Replacing:
                // int occupied = _dirs.Select(d => raycast(grid, r, c, d, range)).Sum();
                // halves the run time!
                int occupied = 0;
                foreach (var d in _dirs)
                    occupied += raycast(grid, seat.Item1, seat.Item2, d, range, width);

                if (grid[seat.Item1, seat.Item2] == SEAT && occupied == 0)
                    next[seat.Item1, seat.Item2] = OCCUPIED;
                else if (grid[seat.Item1, seat.Item2] == OCCUPIED && occupied >= tolerance)
                    next[seat.Item1, seat.Item2] = SEAT;
                else
                    next[seat.Item1, seat.Item2] = grid[seat.Item1, seat.Item2];                
            }

            return next;
        }

        private int calcEquilibrium(int tolerance, int range)
        {
            TextReader tr = new StreamReader("inputs/day11.txt");
            var lines = tr.ReadToEnd().Split('\n');

            HashSet<(int, int)> seats = new HashSet<(int, int)>();
            int[,] grid = new int[lines.Length, lines.Length];
            
            int row = 0;
            foreach (var line in lines)
            {
                var col = 0;
                foreach (var c in line)
                {
                    if (c == 'L')
                    {                    
                        grid[row, col] = SEAT;
                        seats.Add((row, col));
                    }
                    ++col;
                }                
                ++row;
            }

            var next = iterate(grid, seats, tolerance, range, lines.Length);
            while (!same(grid, next, seats))
            {
                var tmp = next;
                next = iterate(next, seats, tolerance, range, lines.Length);
                grid = tmp;
            }

            int count = 0;
            for (int r = 0; r < lines.Length; r++)
            {
                for (int c = 0; c < lines.Length; c++)
                {
                    if (grid[r, c] == OCCUPIED)
                        ++count;
                }
            }
            return count;
        }

        public void Solve()
        {
            var before = DateTime.Now;
            var p1 = calcEquilibrium(4, 1);
            var after = DateTime.Now;
            Console.WriteLine($"P1: {p1}, run time: {after.Subtract(before).Milliseconds}");

            before = DateTime.Now;
            var p2 = calcEquilibrium(5, 100);
            after = DateTime.Now;
            Console.WriteLine($"P2: {p2}, run time: {after.Subtract(before).Milliseconds}");
        }
    }
}