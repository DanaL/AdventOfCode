using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class Image
    {
        public bool InfinityOn { get; set; }
        public bool Negative { get; set; }
        HashSet<(int, int)> _pixels;

        public Image()
        {
            _pixels = new HashSet<(int, int)>();
        }

        public int PixelCount { get => _pixels.Count; }

        public (int, int, int, int) Boundaries()
        {
            int topLeftX = int.MaxValue;
            int topLeftY = int.MaxValue;
            int bottomRightX = int.MinValue;
            int bottomRightY = int.MinValue;
            foreach ((int x, int y) pixel in _pixels)
            {
                if (pixel.x < topLeftX) topLeftX = pixel.x;
                if (pixel.y < topLeftY) topLeftY = pixel.y;
                if (pixel.x > bottomRightX) bottomRightX = pixel.x;
                if (pixel.y > bottomRightY) bottomRightY = pixel.y;
            }

            return (topLeftX, topLeftY, bottomRightX, bottomRightY);
        }

        public bool InBounds(int x, int y)
        {
            (int tx, int ty, int bx, int by) = Boundaries();

            return x >= tx && x <= bx && y >= ty && y <= by;
        }

        public void WritePixel(int x, int y)
        {
            _pixels.Add((x, y));
        }

        // Okay, if we are on an even generation, a point contained within the hashset
        // represents an On pixel, but in an *odd* generation, _pixels is storing
        // Off pixels...
        public int ValueOfNeighours(int x, int y)
        {

            var s = "";
            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2; c++)
                {
                    if (!InBounds(c + x, r + y))
                        s += InfinityOn ? '1' : '0';                    
                    else if (Negative)
                        s += _pixels.Contains((c + x, r + y)) ? '0' : '1';
                    else                    
                        s += _pixels.Contains((c + x, r + y)) ? '1' : '0';                    
                }
            }

            return Convert.ToInt32(s, 2);
        }

        public Image Retouch(string alg)
        {
            var retouched = new Image()
            {
                InfinityOn = !InfinityOn,
                Negative = !Negative
            };

            (int tx, int ty, int bx, int by) = Boundaries();
            if (Negative)
            {
                for (int y = ty - 2; y <= by + 2; y++)
                {
                    for (int x = tx - 2; x <= bx + 2; x++)
                    {
                        int v = ValueOfNeighours(x, y);
                        char c = alg[v];

                        if (c == '#')
                            retouched._pixels.Add((x, y));
                    }
                }
            }
            else
            {
                for (int y = ty - 2; y <= by + 2; y++)
                {
                    for (int x = tx - 2; x <= bx + 2; x++)
                    {
                        int v = ValueOfNeighours(x, y);
                        char c = alg[v];

                        if (c == '.')
                            retouched._pixels.Add((x, y));
                    }
                }
            }
            return retouched;
        }

        public void Print()
        {            
            (int tx, int ty, int bx, int by) = Boundaries();
            for (int y = ty - 2; y <= by + 2; y++)
            {
                string row = "";
                for (int x = tx - 2; x <= bx + 2; x++)
                {
                    if (!InBounds(x, y) && InfinityOn)
                        row += '#';
                    else if (!InBounds(x, y) && !InfinityOn)
                        row += '.';
                    else if (Negative)
                        row += _pixels.Contains((x, y)) ? '.' : '#';
                    else
                        row += _pixels.Contains((x, y)) ? '#' : '.';
                }
                Console.WriteLine(row);
            }

            Console.WriteLine();
        }
    }

    internal class Day20 : IDay
    {
        string _alg;
        
        Image Input()
        {
            var lines = File.ReadAllLines("inputs/day20.txt");
            _alg = lines[0];

            Image img = new Image() {
                InfinityOn = false,
                Negative = false
            };
            int row = 0;
            foreach (var line in lines.Skip(2))
            {
                for (int c = 0; c < line.Length; c++)
                {
                    if (line[c] == '#')
                        img.WritePixel(c, row);
                        
                }
                ++row;
            }

            return img;
        }

        public void Solve()
        {
            var img = Input();            
            Image retouched = img.Retouch(_alg);
            for (int j = 0; j < 1; j++)
            {
                retouched = retouched.Retouch(_alg);
                if (j == 0)
                    Console.WriteLine($"P1: {retouched.PixelCount}");
            }

            Console.WriteLine($"P2: {retouched.PixelCount}");
        }
    }
}
