using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    internal class Day20 : IDay
    {
        string _alg;
        HashSet<(int, int)> _img;

        void Input()
        {
            var lines = File.ReadAllLines("inputs/day20.txt");
            _alg = lines[0];

            _img = new HashSet<(int, int)>();
            int row = 0;
            foreach (var line in lines.Skip(2))
            {
                for (int c = 0; c < line.Length; c++)
                {
                    if (line[c] == '#')
                        _img.Add((row, c));
                        
                }
                ++row;
            }
        }

        int NeighboursValue((int y, int x) pixel)
        {
            string s = "";
            for (int r = -1; r < 2; r ++)
            {
                for (int c = -1; c < 2; c++)
                    s += _img.Contains((r + pixel.y, c + pixel.x)) ? '1' : '0';                
            }

            return Convert.ToInt32(s, 2);
        }

        public void Solve()
        {
            Input();
            var x = NeighboursValue((2, 2));
            Console.WriteLine($"{x} {_alg[x]}");       
        }
    }
}
