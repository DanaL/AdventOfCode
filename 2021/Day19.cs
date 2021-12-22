using System;
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
        HashSet<int> _found;

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
                //Console.WriteLine($"Fuck yeah, we found {count} matching points!");
                //Console.WriteLine($"Source rotation: {rotation}");
                //foreach (var m in matches)
                //{
                //    Console.WriteLine(m);
                //}
                throw new MatchSuccess() { Transpose = transpose };
            }
        }

        bool CheckScanner(int sourceScanner, int targetScanner)
        {            
            var sourcePts = _scanners[sourceScanner];

            // Generated the rotations for target pts
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

            CheckScanner(0, 13);
            CheckScanner(13, 11);
            CheckScanner(13, 26);
            CheckScanner(11, 2);
            CheckScanner(11, 15);
            CheckScanner(11, 20);
            CheckScanner(11, 25);
            CheckScanner(20, 7);
            CheckScanner(15, 21);
            CheckScanner(2, 28);
            CheckScanner(7, 5);
            CheckScanner(21, 9);
            CheckScanner(21, 12);
            CheckScanner(21, 32);
            CheckScanner(21, 35);
            CheckScanner(12, 17);
            CheckScanner(5, 34);
            CheckScanner(17, 4);
            CheckScanner(17, 19);
            CheckScanner(17, 22);
            CheckScanner(17, 24);
            CheckScanner(22, 1);
            CheckScanner(19, 6);
            CheckScanner(4, 10);
            CheckScanner(4, 23);
            CheckScanner(19, 37);
            CheckScanner(6, 14);
            CheckScanner(1, 16);
            CheckScanner(1, 18);
            CheckScanner(6, 29);
            CheckScanner(23, 33);
            CheckScanner(1, 38);
            CheckScanner(18, 27);
            CheckScanner(29, 30);
            CheckScanner(18, 31);
            CheckScanner(30, 3);
            CheckScanner(27, 8);
            CheckScanner(31, 36);

            for (int x = 1; x < 38; x++)
                CheckScanner(36, x);

            Console.WriteLine(_scanners.Values.Select(s => s).SelectMany(s => s).Distinct().Count());
        }
    }
}
