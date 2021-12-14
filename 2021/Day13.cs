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
        static (GridInfo, List<Fold>) FetchInput()
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

        static GridInfo DoFold(GridInfo grid, Fold fold)
        {
            GridInfo result;

            if (fold.X)
            {
                result = new GridInfo(grid.Height, fold.Loc);

                for (int r = 0; r < result.Height; r++)
                {
                    for (int c = 0; c < fold.Loc; c++)
                    {
                        int ri = r * result.Width + c;
                        int si = r * grid.Width + c;
                        result.Grid[ri] = grid.Grid[si];
                    }
                }

                for (int r = 0; r < result.Height; r++)
                {
                    for (int c = fold.Loc + 1; c < grid.Width; c++)
                    {
                        int i = r * grid.Width + c;
                        if (grid.Grid[i])
                        {
                            int resultCol = fold.Loc - (c - fold.Loc);
                            result.Grid[r * result.Width + resultCol] = true;
                        }
                    }
                }
            }
            else
            {
                result = new GridInfo(fold.Loc, grid.Width);
                for (int j = 0; j < result.Height * result.Width; j++)
                    result.Grid[j] = grid.Grid[j];
                for (int j = (fold.Loc + 1) * grid.Width; j < grid.Width * grid.Height; j++)
                {
                    if (grid.Grid[j])
                    {
                        int row = j / grid.Width;
                        int delta = (fold.Loc - row) * 2;
                        result.Grid[j + delta * result.Width] = true;
                    }
                }
            }

            return result;
        }

        public void Solve()
        {
            var input = FetchInput();
            GridInfo grid = input.Item1;
            List<Fold> folds = input.Item2;
            
            grid = DoFold(grid, folds[0]);
            Console.WriteLine($"P1: {grid.Grid.Where(s => s).Count()}");
        }
    }
}
