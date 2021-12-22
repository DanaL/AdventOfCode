using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day19 : IDay
    {
        Dictionary<int, HashSet<(int, int, int)>> _scanners;

        static int[] _r0  = new int[] {  0,  0,  1,  0,  1,  0, -1,  0,  0 };
        static int[] _r1  = new int[] { -1,  0,  0,  0,  1,  0,  0,  0, -1 };
        static int[] _r2  = new int[] {  0,  0, -1,  0,  1,  0,  1,  0,  0 };

        static int[] _r3  = new int[] {  0, -1,  0,  1,  0,  0,  0,  0,  1 };
        static int[] _r4  = new int[] {  0,  0,  1,  1,  0,  0,  0,  1,  0 };
        static int[] _r5  = new int[] {  0,  1,  0,  1,  0,  0,  0,  0, -1 };
        static int[] _r6  = new int[] {  0,  0, -1,  1,  0,  0,  0, -1,  0 };

        static int[] _r7  = new int[] {  0,  1,  0, -1,  0,  0,  0,  0,  1 };
        static int[] _r8  = new int[] {  0,  0,  1, -1,  0,  0,  0, -1,  0 };
        static int[] _r9  = new int[] {  0, -1,  0, -1,  0,  0,  0,  0, -1 };
        static int[] _r10 = new int[] {  0,  0, -1, -1,  0,  0,  0, -1,  0 };

        static int[] _r11 = new int[] {  1,  0,  0,  0,  0, -1,  0,  1,  0 };
        static int[] _r12 = new int[] {  0,  1,  0,  0,  0, -1, -1,  0,  0 };
        static int[] _r13 = new int[] { -1,  0,  0,  0,  0, -1,  0,  1,  0 };
        static int[] _r14 = new int[] {  0, -1,  0,  0,  0, -1,  1,  0,  0 };

        static int[] _r15 = new int[] {  1,  0,  0,  0, -1,  0,  0,  0, -1 };
        static int[] _r16 = new int[] {  0,  0, -1,  0, -1,  0, -1,  0,  0 };
        static int[] _r17 = new int[] { -1,  0,  0,  0, -1,  0,  0,  0,  1 };
        static int[] _r18 = new int[] {  0,  0,  1,  0, -1,  0,  1,  0,  0 };

        static int[] _r19 = new int[] {  1,  0,  0,  0,  0,  1,  0, -1,  0 };
        static int[] _r20 = new int[] {  0, -1,  0,  0,  0,  1, -1,  0,  0 };
        static int[] _r21 = new int[] { -1,  0,  0,  0,  0,  1,  0,  1,  0 };
        static int[] _r22 = new int[] {  0,  1,  0,  0,  0,  1,  1,  0,  0 };

        static int[] _r23 = new int[] {  1,  0,  0,  0,  1,  0,  0,  0,  1 };

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

        bool AreEqual((int, int, int) a, (int, int, int) b)
        {
            return a.Item1 == b.Item1 && a.Item2 == b.Item2 && a.Item3 == b.Item3;
        }

        void PretendPointIsOtherPoint((int X, int Y, int Z) pt, (int X, int Y, int Z) otherPt, List<(int X, int Y, int Z)> pts, HashSet<(int, int, int)> otherPts)
        {
            var transpose = (otherPt.X - pt.X, otherPt.Y - pt.Y, otherPt.Z - pt.Z);

            var matches = new List<((int, int, int), (int, int, int))>();
            int count = 0;
            foreach (var p in pts)
            {
                var tp = (p.X + transpose.Item1, p.Y + transpose.Item2, p.Z + transpose.Item3);
                if (otherPts.Contains(tp))
                {
                    ++count;
                    matches.Add((p, tp));
                }
            }

            if (count >= 12)
            {
                Console.WriteLine($"Fuck yeah, we found {count} matching points!");
                foreach (var m in matches)
                {
                    Console.WriteLine(m);
                }
            }
        }

        void ComparePtsToOtherScanners(List<(int, int, int)> pts, int otherScannerID)
        {
            var otherPts = _scanners[otherScannerID];
            foreach (var pt in pts)
            {
                foreach (var otherPt in otherPts)
                {
                    PretendPointIsOtherPoint(pt, otherPt, pts, otherPts);
                }
            }            
        }

        void CompareToOtherScanners((int, int, int)[,] allRotations, int otherScannerID)
        {
            for (int rot = 0; rot < allRotations.GetLength(1); rot++)
            {
                var pts = new List<(int, int, int)>();
                for (int pt = 0; pt < allRotations.GetLength(0); pt++)
                    pts.Add(allRotations[pt, rot]);

                // Okay, pts is one of the sets of rotated points from a scanner. We
                // want to transpose/compare them to the other sets
                ComparePtsToOtherScanners(pts, otherScannerID);
            }
        }

        void CheckScanner(int scannerNum)
        {
            // First, generate all the rotations of our selected scanner
            var pts = _scanners[scannerNum];
            (int, int, int)[,] rotated = new (int, int, int)[pts.Count, _rotations.Length];

            int p = 0;
            foreach (var pt in pts)
            {
                for (int r = 0; r < _rotations.Length; r++)
                {
                    rotated[p, r] = Rotate(pt, r);
                }
                ++p;
            }

            int targetScanner = 1;
            for (int col = 0; col < rotated.GetLength(1); col++)
            {
                // We don't actually need to build the list of rotated points everytime, but this is helping
                // me to conceptualize what is happening. If performance matters later on I can, say, build
                // a giant array of rotated points and just know that I need to look at, say, indexes 25-50
                var rotatedPoints = new List<(int, int, int)>();
                for (int row = 0; row < rotated.GetLength(0); row++)
                    rotatedPoints.Add(rotated[row, col]);

                foreach (var srcPt in rotatedPoints)
                {
                    foreach (var otherPt in _scanners[targetScanner])
                    {
                        PretendPointIsOtherPoint(srcPt, otherPt, rotatedPoints, _scanners[targetScanner]);
                    }
                }
            }
            
            // Next up: compare to the other points
            //foreach (var otherScanner in _scanners.Keys.Where(k => k != scannerNum))
            //{
            //    CompareToOtherScanners(rotated, 1);
            //}
        }

        public void Solve()
        {
            Input();

            CheckScanner(0);
        }
    }
}
