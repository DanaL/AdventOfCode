using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day19 : IDay
    {
        Dictionary<int, List<(int, int, int)>> _scanners;

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
            _scanners = new Dictionary<int, List<(int, int, int)>>();

            foreach (var line in lines)
            {
                if (line.StartsWith("---")) {
                    // I'm too tired and sad to remember how to format regexes...
                    int scannerNum = int.Parse(line.Split(' ')[2]);
                    _scanners.Add(scannerNum, new List<(int, int, int)>());                   
                }
                else
                {
                    int[] b = line.Split(',').Select(int.Parse).ToArray();
                    _scanners[_scanners.Count - 1].Add((b[0], b[1], b[2]));
                }
                        
            }
        }
        
        void CheckScanner(int scannerNum)
        {
            // First, generate all the rotations of our selected scanner
            var pts = _scanners[scannerNum];
            (int, int, int)[,] rotated = new (int, int, int)[pts.Count, _rotations.Length];

            for (int p = 0; p < pts.Count; p++)
            {
                for (int r = 0; r < _rotations.Length; r++)
                {
                    rotated[p, r] = Rotate(pts[p], r);
                }
            }            
        }

        public void Solve()
        {
            Input();

            CheckScanner(0);
        }
    }
}
