using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AoC;

namespace _2021
{
    public class Day05 : IDay
    {
        public Day05() { }
        
        bool at45((int x, int y) p1, (int x, int y) p2)
        {
            return Math.Abs(p1.x - p2.x) == Math.Abs(p1.y - p2.y);
        }

        public void writePts(Dictionary<(int x, int y), int> pts, (int x, int y) p1, (int x, int y) p2, bool with45s)
        {
            if (p1.x != p2.x && p1.y != p2.y && !(with45s && at45(p1, p2)))
                return;

            (int dx, int dy) = (Math.Sign(p2.x - p1.x), Math.Sign(p2.y - p1.y));

            while (p1 != p2)
            {
                pts[p1] = pts.ContainsKey(p1) ? pts[p1] + 1 : 1;
                p1 = (p1.x + dx, p1.y + dy);
            }

            pts[p2] = pts.ContainsKey(p2) ? pts[p2] + 1 : 1;
        }

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day05.txt");
            var pts1 = new Dictionary<(int, int), int>();
            var pts2 = new Dictionary<(int, int), int>();

            Regex re = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)");
            foreach (var line in File.ReadAllLines("inputs/day05.txt"))
            {
                var m = re.Match(line);
                (int, int) p1 = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                (int, int) p2 = (int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
                writePts(pts1, p1, p2, false);
                writePts(pts2, p1, p2, true);
            }

            Console.WriteLine($"P1: {pts1.Values.Where(x => x > 1).Count()}");
            Console.WriteLine($"P2: {pts2.Values.Where(x => x > 1).Count()}");
        }
    }
}