using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020
{
    public class Day17 : IDay
    {
        private List<(int, int, int)> _neighbouringLocs3d;
        private List<(int, int, int, int)> _neighbouringLocs4d;

        public Day17()
        {
            /* One day I will be made to atone for my terrible LINQ sins */
            _neighbouringLocs3d = Enumerable.Range(-1, 3)
                                    .SelectMany(x => Enumerable.Range(-1, 3), (x, y) => (x, y))
                                    .SelectMany(y => Enumerable.Range(-1, 3), (a, b) => (a.x, a.y, b))
                                    .Where(a => a != (0, 0, 0)).ToList();

            _neighbouringLocs4d = Enumerable.Range(-1, 3)
                                    .SelectMany(x => Enumerable.Range(-1, 3), (x, y) => (x, y))
                                    .SelectMany(y => Enumerable.Range(-1, 3), (a, b) => (a.x, a.y, b))
                                    .SelectMany(z => Enumerable.Range(-1, 3), (c, d) => (c.x, c.y, c.b, d))
                                    .Where(a => a != (0, 0, 0, 0)).ToList();
        }

        private List<(int, int, int)> neighboursOf3d((int, int, int) loc)
        {
            return _neighbouringLocs3d.Select(a => (a.Item1 + loc.Item1, a.Item2 + loc.Item2, a.Item3 + loc .Item3)).ToList();
        }

        private int activeNeighboursCount3d(HashSet<(int, int, int)> grid, (int, int, int) loc)
        {
            return _neighbouringLocs3d.Select(a => (a.Item1 + loc.Item1, a.Item2 + loc.Item2, a.Item3 + loc.Item3))
                             .Where(l => grid.Contains(l)).Count();
        }

        private List<(int, int, int, int)> neighboursOf4d((int, int, int, int) loc)
        {
            return _neighbouringLocs4d.Select(a => (a.Item1 + loc.Item1, a.Item2 + loc.Item2, a.Item3 + loc.Item3, a.Item4 + loc.Item4)).ToList();
        }

        private int activeNeighboursCount4d(HashSet<(int, int, int, int)> grid, (int, int, int, int) loc)
        {
            return _neighbouringLocs4d.Select(a => (a.Item1 + loc.Item1, a.Item2 + loc.Item2, a.Item3 + loc.Item3, a.Item4 + loc.Item4))
                             .Where(l => grid.Contains(l)).Count();
        }

        private HashSet<(int, int, int)> iterate(HashSet<(int, int, int)> cubes)
        {
            var next = new HashSet<(int, int, int)>();
            
            foreach (var c in cubes)
            {
                // Cubes contains all the active cubes so for each one, we need to check them all their neighbours.
                // This'll end up doing some unnecessary work because an active square may have active neighbours but
                // I'm not sure it's worth tracking that and skipping them.
                var toCheck = neighboursOf3d(c);
                toCheck.Add(c);

                foreach (var loc in toCheck)
                {
                    int active = activeNeighboursCount3d(cubes, loc);
                    if (cubes.Contains(loc) && (active == 2 || active == 3))
                        next.Add(loc);
                    else if (!cubes.Contains(loc) && active == 3)
                        next.Add(loc);               
                }
            }
            
            return next;
        }

        private HashSet<(int, int, int, int)> iteratePt2(HashSet<(int, int, int, int)> cubes)
        {
            var next = new HashSet<(int, int, int, int)>();

            foreach (var c in cubes)
            {
                // Cubes contains all the active cubes so for each one, we need to check them all their neighbours.
                // This'll end up doing some unnecessary work because an active square may have active neighbours but
                // I'm not sure it's worth tracking that and skipping them.
                var toCheck = neighboursOf4d(c);
                toCheck.Add(c);

                foreach (var loc in toCheck)
                {
                    int active = activeNeighboursCount4d(cubes, loc);
                    if (cubes.Contains(loc) && (active == 2 || active == 3))
                        next.Add(loc);
                    else if (!cubes.Contains(loc) && active == 3)
                        next.Add(loc);
                }
            }

            return next;
        }

        private void part1()
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

        private void part2()
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
            HashSet<(int, int, int, int)> cubes = new HashSet<(int, int, int, int)>();

            int r = 0;
            foreach (var line in input.Split('\n'))
            {
                var row = line.Trim();
                for (int c = 0; c < row.Length; c++)
                    if (row[c] == '#')
                        cubes.Add((r, c, 0, 0));
                ++r;
            }

            for (int j = 0; j < 6; j++)
                cubes = iteratePt2(cubes);

            Console.WriteLine($"P2: {cubes.Count}");
        }

        public void Solve()
        {
            part1();
            part2();
        }
    }
}
