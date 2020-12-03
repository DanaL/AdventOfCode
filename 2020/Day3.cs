using System;
using System.IO;

namespace _2020
{
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
            int _row = 0;
            int _col = 0;
            int _trees = 0;
            while (_row < _length)
            {
                int _pos = _col + (_row * _width);
                if (_map[_pos] == '#')
                    ++_trees;
                ++_row;
                _col = (_col + 3) % _width;                
            }

            Console.WriteLine($"P1: {_trees}");            
        }
    }
}
