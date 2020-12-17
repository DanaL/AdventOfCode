using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020
{
    public class Day17
    {
        private List<(int, int, int)> _neighbouringLocs3d;

        public Day17()
        {
            _neighbouringLocs3d = Enumerable.Range(-1, 3)
                                    .SelectMany(x => Enumerable.Range(-1, 3), (x, y) => (x, y))
                                    .SelectMany(y => Enumerable.Range(-1, 3), (a, b) => (a.x, a.y, b))
                                    .Where(a => a != (0, 0, 0)).ToList();
        }

        private List<(int, int, int)> neighboursOf((int, int, int) loc)
        {
            return _neighbouringLocs3d.Select(a => (a.Item1 + loc.Item1, a.Item2 + loc.Item2, a.Item3 + loc .Item3)).ToList();
        }

        private int activeNeighboursCount(HashSet<(int, int, int)> grid, (int, int, int) loc)
        {
            return _neighbouringLocs3d.Select(a => (a.Item1 + loc.Item1, a.Item2 + loc.Item2, a.Item3 + loc.Item3))
                             .Where(l => grid.Contains(l)).Count();
        }

        private HashSet<(int, int, int)> iterate(HashSet<(int, int, int)> cubes)
        {
            var next = new HashSet<(int, int, int)>();
            //var indices = cubes.Keys.SelectMany(x => _neighbours, (x, y) => ( x.Item1 + y.Item1, x.Item2 + y.Item2, x.Item3 + y.Item3));

            foreach (var c in cubes)
            {
                // Cubes contains all the active cubes so for each one, we need to check them all their neighbours.
                // This'll end up doing some unnecessary work because an active square may have active neighbours but
                // I'm not sure it's worth tracking that and skipping them.
                var toCheck = neighboursOf(c);
                toCheck.Add(c);

                foreach (var loc in toCheck)
                {
                    int active = activeNeighboursCount(cubes, loc);
                    if (cubes.Contains(loc) && (active == 2 || active == 3))
                        next.Add(loc);
                    else if (!cubes.Contains(loc) && active == 3)
                        next.Add(loc);               
                }
            }
            
            return next;
        }

        public void Solve()
        {
            var input = @"#.#.#.##
                          .####..#
                          #####.#.
                          #####..#
                          #....###
                          ###...##
                          ...#.#.#
                          #.##..##";
            var testInput = @".#.
                              ..#
                              ###";
            HashSet<(int, int, int)> cubes = new HashSet<(int, int, int)>();

            int r = 0;
            foreach (var line in input.Split('\n'))
            {
                var row = line.Trim();
                for (int c = 0; c < row.Length; c++)
                    if (row[c] == '#')
                        cubes.Add((r, c, 0));
                ++r;
            }

            for (int j = 0; j < 6; j++)
                cubes = iterate(cubes);

            Console.WriteLine($"P1: {cubes.Count}");
        }
    }
}
