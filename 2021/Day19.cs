using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day19 : IDay
    {
        List<List<int[]>> _scanners;

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

        int[] Rotate(int[] v0, int r)
        {
            var v1 = new int[3];
            var rot = _rotations[r];

            v1[0] = v0[0] * rot[0] + v0[1] * rot[1] + v0[2] * rot[2];
            v1[1] = v0[0] * rot[3] + v0[1] * rot[4] + v0[2] * rot[5];
            v1[2] = v0[0] * rot[6] + v0[1] * rot[7] + v0[2] * rot[8];

            return v1;
        }

        void Input()
        {
            var lines = File.ReadAllLines("inputs/day19.txt").Where(l => l.Trim() != "");
            _scanners = new List<List<int[]>>();

            foreach (var line in lines)
            {
                if (line.StartsWith("---"))
                {
                    _scanners.Add(new List<int[]>());
                }
                else
                {
                    int[] beacon = line.Split(',').Select(int.Parse).ToArray();
                    _scanners[_scanners.Count - 1].Add(beacon);
                }
                        
            }
        }

        bool Same(int[] a, int[] b)
        {
            return a[0] == b[0] && a[1] == b[1] && a[2] == b[2];
        }

        void Blah()
        {
            var s0 = _scanners[0];
            var setS0 = new HashSet<(int, int, int)>();
            foreach (var b in s0)
            {
                setS0.Add((b[0], b[1], b[2]));
            }

            var s1 = _scanners[1];

            List<(int[], int[])> samsies = new();
            foreach (var b0 in s0)
            {
                // 1. For each point in s0, test by pretending it is the same
                //    as a point in s1.
                // 2. Calc transpose.
                // 3. See if any other transposed point from s1 is the same as
                //    a point in s0

                for (int j = 0; j < s1.Count; j++)
                {
                    var b1 = s1[j];
                    int dx = b0[0] - b1[0];
                    int dy = b0[1] - b1[1];
                    int dz = b0[2] - b1[2];

                    List<(int, int, int)> transposed = new();
                    for (int k = 0; k < s1.Count; k++)
                    {
                        if (j == k) continue;
                        var bk = s1[k];
                        var t = (bk[0] + dx, bk[1] + dy, bk[2] + dz);
                        if (setS0.Contains(t))
                        {
                            Console.WriteLine($"holy fucking shit {t}");
                        }

                    }
                }
            }
        }

        public void Solve()
        {
            var v = new int[] { 8, 0, 7 };

            Input();

            var a = ( -7, 4, 14 );
            var b = ( -7, 4, 14 );

            var set = new HashSet<(int, int, int)>();

            set.Add(a);
            Console.WriteLine(set.Contains(b));

            Blah();            
        }
    }
}
