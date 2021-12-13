using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AoC;

namespace _2021
{
    class GridInfo
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public bool[] Grid { get; set; }

        public GridInfo(int height, int width)
        {
            Height = height;
            Width = width;
            Grid = new bool[height * width];
        }
    }

    struct Fold
    {
        public bool X { get; set; }
        public int Loc { get; set; }
    }

    internal class Day13 : IDay
    {
        private (GridInfo, List<Fold>) FetchInput()
        {
            Regex coord = new Regex(@"^(\d+),(\d+)$");
            Regex fold = new Regex(@"^fold along ([x|y])=(\d+)$");
            List<(int, int)> coords = new List<(int, int)>();
            List<Fold> folds = new List<Fold>();

            foreach (var line in File.ReadAllLines("inputs/day13.txt"))
            {
                // int.Parse(match.Groups[2].Value)
                var matchCoord = coord.Match(line);
                var matchFold = fold.Match(line);
                if (matchCoord.Success)                
                    coords.Add((int.Parse(matchCoord.Groups[1].Value), int.Parse(matchCoord.Groups[2].Value)));
                else if (matchFold.Success)
                {
                    folds.Add(new Fold()
                    {
                        X = matchFold.Groups[1].Value == "x",
                        Loc = int.Parse(matchFold.Groups[2].Value)
                    });
                }
            }

            var maxX = coords.Select(i => i.Item1).Max() + 1;
            var maxY = coords.Select(i => i.Item2).Max() + 1;

            GridInfo grid = new GridInfo(maxY, maxX);
            foreach (var c in coords)
                grid.Grid[c.Item2 * grid.Width + c.Item1] = true;

            return (grid, folds);
        }

        public void Solve()
        {
            var input = FetchInput();
            GridInfo grid = input.Item1;
            List<Fold> folds = input.Item2;
            grid.Grid.PrintGrid(grid.Width, d => d ? "#" : ".");
        }
    }
}
