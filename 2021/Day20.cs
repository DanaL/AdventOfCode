using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class Image
    {
        bool _infinityLit;
        int _imgWidth;
        int _imgHeight;
        bool[] _pixels;
        char[] _alg;

        public Image(bool[] pixels, int width, bool infinityLit, char[] alg)
        {
            _pixels = pixels;
            _imgWidth = width;
            _imgHeight = pixels.Length / width;
            _infinityLit = infinityLit;
            _alg = alg;
        }

        public int PixelCount {
            get => _pixels.Where(p => p).Count();       
        }

        public bool InBounds(int x, int y)
        {
            return x >= 0 && x < _imgWidth && y >= 0 && y < _imgHeight;
        }

        bool CalcPixel(int x, int y)
        {
            int val = 0;

            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2; c++)
                {
                    val *= 2;
                    if (InBounds(x + c, y + r)) {
                        if (_pixels[(y + r) * _imgWidth + x + c])
                            val |= 1;
                    }
                    else if (_infinityLit)
                        val |= 1;
                }
            }

            return _alg[val] == '#';
        }

        public Image Retouch()
        {            
            var pts = new HashSet<(int, int)>();
            for (int y = -1; y <= _imgHeight; y++)
            {
                for (int x = -1; x <= _imgWidth; x++)
                {
                    var result = CalcPixel(x, y);
                    if (result)
                        pts.Add((x, y));
                }
            }

            int newWidth = _imgWidth + 2;
            var retouched = new bool[newWidth * (_imgHeight + 2)];
            foreach (var pt in pts)
                retouched[(pt.Item2 + 1) * newWidth + pt.Item1 + 1] = true;

            // Toggle whether or not infinity is lit up or not
            // I had to go to reddit to realize I had to account for the fact that
            // 9 lit sqs will be extinguished every other generation but the last
            // bit in the alg string
            if (!_infinityLit && _alg[0] == '#' || _infinityLit && _alg.Last() == '.')
                _infinityLit = !_infinityLit;

            return new Image(retouched, newWidth, _infinityLit, _alg);
        }
    }

    internal class Day20 : IDay
    {
        Image Input()
        {
            var lines = File.ReadAllLines("inputs/day20.txt");           
            var pixels = string.Join("", lines.Skip(2)).Select(c => c == '#').ToArray();
            return new Image(pixels, lines[2].Length, false, lines[0].Select(c => c).ToArray());
        }

        public void Solve()
        {
            var img = Input();
            
            Image retouched = img.Retouch();
            for (int j = 0; j < 49; j++)
            {
                retouched = retouched.Retouch();
                if (j == 0)
                    Console.WriteLine($"P1: {retouched.PixelCount}");
            }

            Console.WriteLine($"P2: {retouched.PixelCount}");
        }
    }
}
