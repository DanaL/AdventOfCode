using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class GridInfo
    {
        public int[] Grid { get; }
        public int Height { get; }
        public int Width { get; }

        public GridInfo()
        {
            var lines = File.ReadAllLines("inputs/day15.txt");
            Height = lines.Length;
            Width = lines[0].Length;

            Grid = string.Join("", lines).Replace("\n", "")
                        .Select(c => c - '0').ToArray();
        }
    }

    class PathInfo
    {
        public HashSet<int> Visited { get; set; }

        public PathInfo()
        {
            Visited = new HashSet<int>();
        }
    }

    public class Day15 : IDay
    {
        void foo(PathInfo path)
        {
            path.Visited.Add(4);
            path.Visited.Add(7);
        }

        public void Solve()
        {
            GridInfo grid = new GridInfo();

            PathInfo p = new PathInfo();
            foo(p);
            Console.WriteLine(p.Visited.Count);
        }
    }
}
