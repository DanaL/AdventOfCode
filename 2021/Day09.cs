using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day09 : IDay
    {
        int _height;
        int _width;
        string _data;

        void FetchInput()
        {
            //_data = "2199943210\n3987894921\n9856789892\n8767896789\n9899965678";
            _data = File.ReadAllText("inputs/day09.txt").Trim();
            _height = _data.Where(c => c == '\n').Count() + 1;
            _data = _data.Replace("\n", "");
            _width = _data.Length / _height;
        }

        bool IsLowPoint(int row, int col)
        {
            char v = _data[row * _width + col];

            foreach ((int R, int C) d in Util.Neighbours())
            {
                int nr = row + d.R;
                int nc = col + d.C;
                int x = nr * _width + nc;

                if (x >= 0 && x < _data.Length && nc >= 0 && nc < _width && _data[x] <= v)
                    return false;
            }

            return true;
        }

        int BasinSize(int startR, int startC)
        {
            // It's doesn't really feel like Advent of Code until I've written a floodfill routine!
            var visited = new HashSet<(int, int)>();
            var toVisit = new Queue<(int, int)>();
            toVisit.Enqueue((startR, startC));

            while (toVisit.Count > 0)
            {               
                var curr = toVisit.Dequeue();
                if (visited.Contains(curr))
                    continue;
                visited.Add(curr);

                foreach (var d in Util.Neighbours())
                {
                    var next = (curr.Item1 + d.Item1, curr.Item2 + d.Item2);                    
                    if (visited.Contains(next))
                        continue;

                    var x = next.Item1 * _width + next.Item2;
                    if (!(x >= 0 && x < _data.Length && next.Item2 >= 0 && next.Item2 < _width))
                        continue;
                    if (_data[x] != '9')
                        toVisit.Enqueue(next);
                }
            }

            return visited.Count;
        }

        public void Solve()
        {
            FetchInput();

            int totalPt1 = 0;
            var basins = new List<int>();
            for (int x = 0; x < _data.Length; x++)
            {
                int r = x / _width;
                int c = x - r * _width;
                if (IsLowPoint(r, c))
                {
                    totalPt1 += _data[r * _width + c] - '0' + 1;
                    basins.Add(BasinSize(r, c));
                }
            }

            basins.Sort();
            long totalPt2 = basins.TakeLast(3).Aggregate((a, b) => a * b);

            Console.WriteLine($"P1: {totalPt1}");
            Console.WriteLine($"P2: {totalPt2}");
        }
    }
}