using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AoC;

namespace _2021
{    
    struct Fold
    {
        public bool X { get; set; }
        public int Loc { get; set; }
    }

    internal class Day13 : IDay
    {
        static (HashSet<(int, int)>, List<Fold>) FetchInput()
        {
            var coords = new HashSet<(int, int)>();
            Regex coord = new Regex(@"^(\d+),(\d+)$");
            Regex fold = new Regex(@"^fold along ([x|y])=(\d+)$");
            var folds = new List<Fold>();

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

            return (coords, folds);
        }

        static void DoFold(HashSet<(int, int)> coords, Fold fold)
        {
            var toAdd = new HashSet<(int, int)>();
            var toDel = new HashSet<(int, int)>();
            if (fold.X)
            {
                foreach (var pt in coords.Where(p => p.Item1 > fold.Loc))
                {
                    toDel.Add(pt);
                    var newCol = fold.Loc - (pt.Item1 - fold.Loc);
                    toAdd.Add((newCol, pt.Item2));
                }
            }
            else
            {
                foreach (var pt in coords.Where(p => p.Item2 > fold.Loc))
                {
                    toDel.Add(pt);
                    var newRow = fold.Loc - (pt.Item2 - fold.Loc);
                    toAdd.Add((pt.Item1, newRow));
                }
            }

            foreach (var pt in toDel)
                coords.Remove(pt);
            foreach (var pt in toAdd)
                coords.Add(pt);            
        }

        static void PrintGrid(HashSet<(int, int)> coords)
        {
            int maxCol = coords.Select(c => c.Item1).Max() + 1;
            int maxRow = coords.Select(c => c.Item2).Max() + 1;

            for (int r = 0; r < maxRow; r++)
            {
                var s = "";
                for (int c = 0; c < maxCol; c++)
                    s += coords.Contains((c, r)) ? '#' : ' ';
                Console.WriteLine(s);
            }
            Console.WriteLine("");
        }

        public void Solve()
        {
            var input = FetchInput();
            var coords = input.Item1;
            var folds = input.Item2;

            DoFold(coords, folds[0]);
            Console.WriteLine($"P1: {coords.Count}");
            foreach (var fold in folds.Skip(1))
                DoFold(coords, fold);
            PrintGrid(coords);
        }
    }
}
