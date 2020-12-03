using System;
using System.IO;

namespace _2020
{
    class Route
    {
        public int Down { get; set; }
        public int Right { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Trees { get; set; }

        public Route (int d, int r)
        {
            this.Down = d;
            this.Right = r;
            this.Row = 0;
            this.Column = 0;
            this.Trees = 0;
        }
    }

    public class Day3
    {
        public Day3() { }

        public void SolvePt1()
        {
            using TextReader _tr = new StreamReader("inputs/day3.txt");

            string _txt = _tr.ReadToEnd();
            int _width = _txt.IndexOf('\n');
            int _length = _txt.Length / (_width + 1);
            _txt = _txt.Replace("\n", "");
            char[] _map = _txt.ToCharArray();

            // Okay, so the map repeats itself over and over as you move to the right.
            // So, just loop, incrementing row and column as per the puzzle instructions
            // (3 right, 1 down in part 1), while using % to simulate this being a 2D array
            // that is square instead of a narrow rectangle.

            // For part 2, we have several routes we want to test through the map.
            // Right 1, down 1
            // Right 3, down 1 (part 1)            
            // Right 5, down 1
            // Right 7, down 1
            // Right 1, down 2

            // The simplest-to-code version would be to loop over the map once per
            // route but let's complicate things
            Route _pt1 = new Route(1, 3);
            while (_pt1.Row < _length)
            {
                int _pos = _pt1.Column + (_pt1.Row * _width);
                if (_map[_pos] == '#')
                    ++_pt1.Trees;
                _pt1.Row += _pt1.Down;
                _pt1.Column = (_pt1.Column + _pt1.Right) % _width;                
            }

            Console.WriteLine($"P1: {_pt1.Trees}");            
        }
    }
}
