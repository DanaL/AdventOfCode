using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;

namespace _2020
{
    public class Day11
    {
        public Day11() { }

        private (int, int)[] _dirs = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

        private void dump(List<List<int>> grid)
        {
            foreach (var row in grid)
            {
                var s = string.Concat(row.Select(c =>
                {
                    switch (c)
                    {
                        case -1:
                            return '.';
                        case 0:
                            return 'L';
                        default:
                            return '#';
                    }
                }));

                Console.WriteLine(s);                
            }

            Console.Write('\n');
        }

        private bool same(List<List<int>> g0, List<List<int>> g1)
        {
            for (int r = 0; r < g0.Count; r++)
            {
                for (int c = 0; c < g0[0].Count; c++)
                {
                    if (g0[r][c] != g1[r][c])
                        return false;
                }
            }

            return true;
        }

        private List<List<int>> iterate(List<List<int>> grid)
        {
            List<List<int>> next = new List<List<int>>();

            for (int r = 0; r < grid.Count; r++)
            {
                List<int> row = new List<int>();
                for (int c = 0; c < grid[0].Count; c++)
                {
                    // Count adjacent occupied seats and see if we have a state change
                    if (grid[r][c] == -1)
                    {
                        row.Add(-1);
                        continue; // skip floor spaces
                    }

                    int occupied = 0;
                    foreach (var d in _dirs)
                    {
                        int nr = r + d.Item1;
                        int nc = c + d.Item2;

                        if (nr < 0 || nc < 0 || nr >= grid.Count || nc >= grid[0].Count)
                            continue;

                        if (grid[nr][nc] == 1)
                            ++occupied;
                    }

                    if (grid[r][c] == 0 && occupied == 0)
                        row.Add(1);
                    else if (grid[r][c] == 1 && occupied >= 4)
                        row.Add(0);
                    else
                        row.Add(grid[r][c]);
                }
                next.Add(row);
            }

            return next;
        }

        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day11.txt");

            List<List<int>> grid = new List<List<int>>();
            foreach (var line in tr.ReadToEnd().Split('\n'))
            {
                grid.Add(line.ToCharArray().Select(c => c == '.' ? -1 : 0).ToList());
            }
            
            var next = iterate(grid);
            while (!same(grid, next))
            {
                var tmp = next;
                next = iterate(next);
                grid = tmp;
            }

            int p1 = grid.SelectMany(a => a).Where(a => a == 1).Count();
            Console.WriteLine($"P1: {p1}");
        }
    }
}
