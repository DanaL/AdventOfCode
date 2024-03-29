﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class MatchSuccess : Exception
    {
        public (int, int, int) Transpose { get; set; }

        public MatchSuccess() { }
    }

    public class Day19 : IDay
    {
        Dictionary<int, HashSet<(int, int, int)>> _scanners;
        Dictionary<int, (int, int, int)> _scannerLocations;

        static int[] _r0 = new int[] { 1,  0,  0,  0,  1,  0,  0,  0,  1 };
        static int[] _r1 = new int[] { 1, 0, 0, 0, 0, -1, 0, 1, 0 };
        static int[] _r2 = new int[] { 1, 0, 0, 0, -1, 0, 0, 0, -1 };
        static int[] _r3 = new int[] { 1, 0, 0, 0, 0, 1, 0, -1, 0 };

        static int[] _r4 = new int[] { 0, -1, 0, 1, 0, 0, 0, 0, 1 };
        static int[] _r5 = new int[] { 0, 0, 1, 1, 0, 0, 0, 1, 0 };
        static int[] _r6 = new int[] { 0, 1, 0, 1, 0, 0, 0, 0, -1 };
        static int[] _r7 = new int[] { 0, 0, -1, 1, 0, 0, 0, -1, 0 };

        static int[] _r8 = new int[] { -1, 0, 0, 0, -1, 0, 0, 0, 1 };
        static int[] _r9 = new int[] { -1, 0, 0, 0, 0, -1, 0, -1, 0 };
        static int[] _r10 = new int[] { -1, 0, 0, 0, 1, 0, 0, 0, -1 };
        static int[] _r11 = new int[] { -1, 0, 0, 0, 0, 1, 0, 1, 0 };

        static int[] _r12 = new int[] { 0, 1, 0, -1, 0, 0, 0, 0, 1 };
        static int[] _r13 = new int[] { 0, 0, 1, -1, 0, 0, 0, -1, 0 };
        static int[] _r14 = new int[] { 0, -1, 0, -1, 0, 0, 0, 0, -1 };
        static int[] _r15 = new int[] { 0, 0, -1, -1, 0, 0, 0, 1, 0 };

        static int[] _r16 = new int[] { 0, 0, -1, 0, 1, 0, 1, 0, 0 };
        static int[] _r17 = new int[] { 0, 1, 0, 0, 0, 1, 1, 0, 0 };
        static int[] _r18 = new int[] { 0, 0, 1, 0, -1, 0, 1, 0, 0 };
        static int[] _r19 = new int[] { 0, -1, 0, 0, 0, -1, 1, 0 ,0 };

        static int[] _r20 = new int[] { 0, 0, -1, 0, -1, 0, -1, 0 ,0 };
        static int[] _r21 = new int[] {0,-1,0, 0, 0, 1, -1, 0 ,0 };
        static int[] _r22 = new int[] {0, 0, 1, 0, 1, 0, -1, 0 ,0};
        static int[] _r23 = new int[] { 0, 1, 0, 0, 0, -1, -1, 0, 0 };
        
        static int[][] _rotations = new int[][] { _r0, _r1, _r2, _r3, _r4, _r5, _r6, _r7, _r8, _r9, _r10,
                                                  _r11, _r12, _r13, _r14, _r15, _r16, _r17, _r18, _r19,
                                                  _r20, _r21, _r22, _r23 };

        (int, int, int) Rotate((int X, int Y, int Z) v0, int r)
        {
            var rot = _rotations[r];

            var r0 = v0.X * rot[0] + v0.Y * rot[1] + v0.Z * rot[2];
            var r1 = v0.X * rot[3] + v0.Y * rot[4] + v0.Z * rot[5];
            var r2 = v0.X * rot[6] + v0.Y * rot[7] + v0.Z * rot[8];

            return (r0, r1, r2);
        }

        void Input()
        {
            var lines = File.ReadAllLines("inputs/day19.txt").Where(l => l.Trim() != "");
            _scanners = new Dictionary<int, HashSet<(int, int, int)>>();

            foreach (var line in lines)
            {
                if (line.StartsWith("---")) {
                    // I'm too tired and sad to remember how to format regexes...
                    int scannerNum = int.Parse(line.Split(' ')[2]);
                    _scanners.Add(scannerNum, new HashSet<(int, int, int)>());                   
                }
                else
                {
                    int[] b = line.Split(',').Select(int.Parse).ToArray();
                    _scanners[_scanners.Count - 1].Add((b[0], b[1], b[2]));
                }
            }
        }

        void PretendPointIsOtherPoint((int X, int Y, int Z) targetPt, (int X, int Y, int Z) sourcePt, List<(int X, int Y, int Z)> pts, HashSet<(int, int, int)> sourcePts, int rotation)
        {
            var transpose = (sourcePt.X - targetPt.X, sourcePt.Y - targetPt.Y, sourcePt.Z - targetPt.Z);

            var matches = new List<((int, int, int), (int, int, int), (int, int, int))>();
            int count = 0;
            foreach (var p in pts)
            {
                var tp = (p.X + transpose.Item1, p.Y + transpose.Item2, p.Z + transpose.Item3);
                if (sourcePts.Contains(tp))
                {
                    ++count;
                    matches.Add((p, tp, transpose));
                }
            }

            if (count >= 12)
            {
                throw new MatchSuccess() { Transpose = transpose };
            }
        }

        bool CheckScanner(int sourceScanner, int targetScanner)
        {            
            var sourcePts = _scanners[sourceScanner];

            var targetPts = _scanners[targetScanner];
            (int, int, int)[,] rotated = new (int, int, int)[targetPts.Count, _rotations.Length];
            int p = 0;
            foreach (var pt in targetPts)
            {
                for (int r = 0; r < _rotations.Length; r++)
                {
                    rotated[p, r] = Rotate(pt, r);
                }
                ++p;
            }

            for (int col = 0; col < rotated.GetLength(1); col++)
            {
                // We don't actually need to build the list of rotated points everytime, but this is helping
                // me to conceptualize what is happening. If performance matters later on I can, say, build
                // a giant array of rotated points and just know that I need to look at, say, indexes 25-50
                var rotatedTargetPts = new List<(int, int, int)>();
                for (int row = 0; row < rotated.GetLength(0); row++)
                    rotatedTargetPts.Add(rotated[row, col]);

                foreach (var srcPt in sourcePts)
                {
                    foreach (var targetPt in rotatedTargetPts)
                    {
                        try
                        {
                            PretendPointIsOtherPoint(targetPt, srcPt, rotatedTargetPts, sourcePts, col);                            
                        }
                        catch (MatchSuccess ms)
                        {
                            HashSet<(int, int, int)> normalized = new();
                            (int X, int Y, int Z) tranpose = ms.Transpose;
                            foreach ((int X, int Y, int Z) in rotatedTargetPts)
                                normalized.Add((X + tranpose.X, Y + tranpose.Y, Z + tranpose.Z));
                            _scanners[targetScanner] = normalized;
                            Console.WriteLine($"Success! {sourceScanner} -> {targetScanner}: {tranpose}");
                            if (!_scannerLocations.ContainsKey(targetScanner))
                                _scannerLocations.Add(targetScanner, tranpose);

                            return true;
                        }
                    }
                }                
            }

            return false;
        }

        public void Solve()
        {
            Input();

            _scannerLocations = new Dictionary<int, (int, int, int)>();
            _scannerLocations.Add(0, (0, 0, 0));

            var alreadyChecked = new HashSet<(int, int)>();
            var toCheck = new Queue<int>();
            toCheck.Enqueue(0);

            var scannerNums = _scanners.Keys.OrderBy(k => k);
            while (toCheck.Count > 0)
            {
                var s = toCheck.Dequeue();
                
                Console.WriteLine($"Checking against {s}");
                foreach (var t in scannerNums)
                {
                    if (s == t) continue;
                    if (alreadyChecked.Contains((s, t)) || alreadyChecked.Contains((t, s)))
                        continue;
                    alreadyChecked.Add((s, t));
                    alreadyChecked.Add((t, s));                    
                    if (CheckScanner(s, t)) 
                        toCheck.Enqueue(t);
                }
            }

            Console.WriteLine($"P1: {_scanners.Values.Select(s => s).SelectMany(s => s).Distinct().Count()}");

            ulong greatestDistance = 0;
            foreach ((int x0, int y0, int z0) in _scannerLocations.Values)
            {
                foreach ((int x1, int y1, int z1)in _scannerLocations.Values)
                {
                    ulong d = (ulong) Util.TaxiDistance(x0, y0, z0, x1, y1, z1);
                    if (d > greatestDistance)
                        greatestDistance = d;
                }
            }
            Console.WriteLine($"P2: {greatestDistance}");
        }
    }
}
