using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class Image
    {
        public int Generation { get; set; }
        public HashSet<(int, int)> Pixels;

        public Image(int gen)
        {
            Generation = gen;
            Pixels = new HashSet<(int, int)>();
        }

        public (int, int, int, int) Boundaries()
        {
            int topLeftX = int.MaxValue;
            int topLeftY = int.MaxValue;
            int bottomRightX = int.MinValue;
            int bottomRightY = int.MinValue;
            foreach ((int x, int y) pixel in Pixels)
            {
                if (pixel.x < topLeftX) topLeftX = pixel.x;
                if (pixel.y < topLeftY) topLeftY = pixel.y;
                if (pixel.x > bottomRightX) bottomRightX = pixel.x;
                if (pixel.y > bottomRightY) bottomRightY = pixel.y;
            }

            return (topLeftX, topLeftY, bottomRightX, bottomRightY);
        }

        public int ValueOfNeighours(int x, int y)
        {
            var s = "";
            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2; c++)
                    s += Pixels.Contains((c + x, r + y)) ? '1' : '0';
            }

            return Convert.ToInt32(s, 2);
        }

        public void Print()
        {            
            (int tx, int ty, int bx, int by) = Boundaries();
            for (int y = ty - 1; y < by + 1; y++)
            {
                string row = "";
                for (int x = tx - 1; x < bx + 1; x++)
                    row += Pixels.Contains((x, y)) ? '#' : '.';
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

            Image img = new Image(0);
            int row = 0;
            foreach (var line in lines.Skip(2))
            {
                for (int c = 0; c < line.Length; c++)
                {
                    if (line[c] == '#')
                        img.Pixels.Add((c, row));
                        
                }
                ++row;
            }

            return img;
        }

        public Image Retouch(Image src)
        {
            var retouched = new Image(src.Generation + 1);
            (int tx, int ty, int bx, int by) = src.Boundaries();
            for (int y = ty - 3; y < by + 3; y++)                
            {
                for (int x = tx - 3; x < bx + 3; x++)
                {
                    char c = _alg[src.ValueOfNeighours(x, y)];
                    if (c == '#')
                        retouched.Pixels.Add((x, y));
                }
            }

            return retouched;
        }

        public void Solve()
        {
            var img = Input();
            img.Print();

            var retouched = Retouch(img);
            retouched.Print();
            Console.WriteLine(retouched.Pixels.Count);
            retouched = Retouch(retouched);
            retouched.Print();

            Console.WriteLine($"P1: {retouched.Pixels.Count}");
        }
    }
}
