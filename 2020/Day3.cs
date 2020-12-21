using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    class Route
    {
        public int Down { get; set; }
        public int Right { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public long Trees { get; set; }
        public int AtEnd { get; set; }

        public Route (int d, int r)
        {
            this.Down = d;
            this.Right = r;
            this.Row = 0;
            this.Column = 0;
            this.Trees = 0;
            this.AtEnd = 0;
        }
    }

    public class Day3 : IDay
    {
        public Day3() { }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day3.txt");

            string txt = tr.ReadToEnd();
            int width = txt.IndexOf('\n');
            int length = txt.Length / (width + 1);
            char[] map = txt.ToCharArray().Where(ch => ch != '\n').ToArray();

            // Okay, so the map repeats itself over and over as you move to the right.
            // So, just loop, incrementing row and column as per the puzzle instructions
            // (3 right, 1 down in part 1), while using % to simulate this being a 2D array
            // that is square instead of a narrow rectangle.

            // For part 2, we have several routes we want to test through the map.
            // Right 1, down 1
            // Right 3, down 1 (part 1)            
            // Right 5, down 1
            // Right 7, down 1
            // Right 1, down 2

            // The simplest-to-code version would be to loop over the map once per
            // route but let's complicate things
            List<Route> routes = new List<Route>() { new Route(1, 1), new Route(1, 3),
                new Route(1, 5), new Route(1, 7), new Route(2, 1)};

            while (routes.Sum(r => r.AtEnd) < routes.Count)
            {
                foreach (var rt in routes)
                {
                    if (rt.AtEnd == 1)
                        continue;

                    int _pos = rt.Row * width + rt.Column;
                    if (map[_pos] == '#')
                        ++rt.Trees;
                    rt.Row += rt.Down;
                    rt.Column = (rt.Column + rt.Right) % width;

                    if (rt.Row > length)
                        rt.AtEnd = 1;
                }
            }

            Console.WriteLine($"P1: {routes[1].Trees}");            
            long pt2 = routes.Aggregate(1L, (v, r) => v * r.Trees);
            Console.WriteLine($"P2: {pt2}");
        }
    }
}

