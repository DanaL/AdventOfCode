using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class Image
    {
        public bool InfinityLit { get; set; }
        List<List<bool>> _pixels;

        public Image(List<List<bool>> pixels)
        {
            _pixels = pixels;
        }

        public int PixelCount {
            get {
                int sum = 0;

                foreach (var row in _pixels)
                    sum += row.Where(p => p).Count();
                return sum;
            }            
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < _pixels[0].Count && y >= 0 && y < _pixels.Count;
        }

        bool CalcPixel(int x, int y, bool[] alg)
        {
            int val = 0;

            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2; c++)
                {
                    val <<= 1;
                    if (InBounds(x + c, y + r)) {
                        if (_pixels[y + r][x + c])
                            val |= 1;
                    }
                    else if (InfinityLit)
                        val |= 1;
                }
            }

            return alg[val];
        }

        public Image Retouch(bool[] alg)
        {
            int height = _pixels.Count;
            int width = _pixels[0].Count;
            int buffer = 2;

            var retouched = new List<List<bool>>();
            for (int y = 0; y < height + buffer * 2; y++)
            {
                List<bool> row = new List<bool>();
                for (int x = 0; x < width + buffer * 2; x++)
                    row.Add(false);
                retouched.Add(row);
            }
            
            for (int y = -buffer; y <= height + buffer - 1; y++)
            {
                for (int x = -buffer; x <= width + buffer - 1; x++)
                {
                    var result = CalcPixel(x, y, alg);
                    retouched[y + buffer][x + buffer] = result;
                }
            }

            // Toggle whether or not infinity is lit up or not
            if (!InfinityLit && alg[0] || InfinityLit && !alg.Last())
                InfinityLit = !InfinityLit;

            return new Image(retouched) {
                InfinityLit = InfinityLit
            };
        }
    }

    internal class Day20 : IDay
    {
        bool[] _alg;
        
        Image Input()
        {
            var lines = File.ReadAllLines("inputs/day20.txt");
            _alg = lines[0].Select(c => c == '#').ToArray();

            List<List<bool>> pixels = new List<List<bool>>();
            foreach (var line in lines.Skip(2))
            {
                var row = line.Select(c => c == '#').ToList();
                pixels.Add(row);
            }

            Image img = new Image(pixels)
            {
                InfinityLit = false
            };

            return img;
        }

        public void Solve()
        {
            var img = Input();
            
            Image retouched = img.Retouch(_alg);
            for (int j = 0; j < 49; j++)
            {
                retouched = retouched.Retouch(_alg);
                if (j == 0)
                    Console.WriteLine($"P1: {retouched.PixelCount}");
            }

            Console.WriteLine($"P2: {retouched.PixelCount}");
        }
    }
}
