using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2021
{
    public class Day05 : IDay
    {
        Regex _re;

        public Day05()
        {
            string pattern = @"(\d+),(\d+) -> (\d+),(\d+)";

            _re = new Regex(pattern);
        }

        ((int, int), (int, int)) parseLine(string line)
        {
            var matches = _re.Match(line);

            return ((int.Parse(matches.Groups[1].Value), int.Parse(matches.Groups[2].Value)),
                        (int.Parse(matches.Groups[3].Value), int.Parse(matches.Groups[4].Value)));
        }

        bool at45((int x, int y) p1, (int x, int y) p2)
        {
            return Math.Abs(p1.x - p2.x) == Math.Abs(p1.y - p2.y);
        }

        (int, int) delta45((int x, int y) p1, (int x, int y) p2)
        {
            int dx = p1.x < p2.x ? 1 : -1;
            int dy = p1.y < p2.y ? 1 : -1;

            return (dx, dy);
        }

        public void writePts(Dictionary<(int x, int y), int> pts, (int x, int y) p1, (int x, int y) p2, bool with45s)
        {
            (int x, int y) delta;
            if (p1.x == p2.x)
                delta = p1.y < p2.y ? (0, 1) : (0, -1);
            else if (p1.y == p2.y)
                delta = p1.x < p2.x ? (1, 0) : (-1, 0);
            else if (with45s && at45(p1, p2))
                delta = delta45(p1, p2);
            else
                return;

            while (p1 != p2)
            {
                pts[p1] = pts.ContainsKey(p1) ? pts[p1] + 1 : 1;
                p1 = (p1.x + delta.x, p1.y + delta.y);
            }

            pts[p2] = pts.ContainsKey(p2) ? pts[p2] + 1 : 1;
        }

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day05.txt");
            var pts1 = new Dictionary<(int, int), int>();
            var pts2 = new Dictionary<(int, int), int>();
            foreach (((int x, int y), (int x, int y)) pair in lines.Select(p => parseLine(p)))
            {
                var (p1, p2) = pair;
                writePts(pts1, p1, p2, false);
                writePts(pts2, p1, p2, true);
            }

            Console.WriteLine($"P1: {pts1.Values.Where(x => x > 1).Count()}");
            Console.WriteLine($"P2: {pts2.Values.Where(x => x > 1).Count()}");
        }
    }
}