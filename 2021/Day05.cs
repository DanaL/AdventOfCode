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
            string pattern = @"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)";

            _re = new Regex(pattern);
        }

        List<string> testLines()
        {
            var input = @"  0,9 -> 5,9
                            8,0 -> 0,8
                            9,4 -> 3,4
                            2,2 -> 2,1
                            7,0 -> 7,4
                            6,4 -> 2,0
                            0,9 -> 2,9
                            3,4 -> 1,4
                            0,0 -> 8,8
                            5,5 -> 8,2";

            var lines = input.Split('\n', StringSplitOptions.TrimEntries).ToList();

            return lines;
        }

        ((int, int), (int, int)) parseLine(string line)
        {
            var matches = _re.Match(line);

            return ((int.Parse(matches.Groups["x1"].Value), int.Parse(matches.Groups["y1"].Value)),
                        (int.Parse(matches.Groups["x2"].Value), int.Parse(matches.Groups["y2"].Value)));
        }

        public void writePts(Dictionary<(int x, int y), int> pts, (int x, int y) p1, (int x, int y) p2)
        {
            (int x, int y) delta;
            if (p1.x == p2.x)
                delta = p1.y < p2.y ? (0, 1) : (0, -1);
            else if (p1.y == p2.y)
                delta = p1.x < p2.x ? (1, 0) : (-1, 0);
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
            var pts = new Dictionary<(int, int), int>();
            foreach (((int x, int y), (int x, int y)) pair in lines.Select(p => parseLine(p)))
            {
                var (p1, p2) = pair;
                writePts(pts, p1, p2);
            }

            var sum = pts.Values.Where(x => x > 1).Count();
            Console.WriteLine($"P1: {sum}");
        }
    }
}
