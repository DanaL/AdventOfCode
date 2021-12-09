using System;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day09 : IDay
    {
        int _height;
        int _width;
        string _data;

        public Day09() { }

        void FetchInput()
        {
            //string data = "2199943210\n3987894921\n2856789892\n8767896789989\n9965678";
            _data = File.ReadAllText("inputs/day09.txt").Trim();
            _height = _data.Where(c => c == '\n').Count() + 1;
            _data = _data.Replace("\n", "");
            _width = _data.Length / _height;
        }

        bool IsLowPoint(string map, int row, int col)
        {
            char v = map[row * _width + col];

            foreach ((int R, int C) d in Util.Neighbours())
            {
                int nr = row + d.R;
                int nc = col + d.C;
                int x = nr * _width + nc;

                if (x >= 0 && x < map.Length && nc >= 0 && nc < _width && map[x] <= v)
                    return false;
            }

            return true;
        }

        public void Solve()
        {
            FetchInput();
            
            int sum = 0;
            for (int r = 0; r < _height; r++)
            {
                for (int c = 0; c < _width; c++)
                {
                    if (IsLowPoint(_data, r, c))
                        sum += _data[r * _width + c] - '0' + 1;                    
                }
            }

            Console.WriteLine($"P1: {sum}");

            // Part 2 will be classic flood fill
        }
    }
}
