using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2021
{
    class AllFashedException : Exception { }

    public class Day11 : IDay
    {
        static int _height = 10;
        static int _width = 10;
        static int _area = _height * _width;
        static int[] _adjAll = new int[] { -11, -10, -9, -1, 1, 9, 10, 11 };
        static int[] _leftSide = new int[] { -10, 1, -9, 10, 11 };
        static int[] _rightSide = new int[] { -11, -10, 9, -1, 10 };
        int _flashes;
        bool _part2;

        int[] FetchData()
        {
            string txt = @"
                    3172537688
                    4566483125
                    6374512653
                    8321148885
                    4342747758
                    1362188582
                    7582213132
                    6887875268
                    7635112787
                    7242787273";
      
            return txt.Where(c => char.IsDigit(c)).Select(c => Convert.ToInt32(c) - (int) '0').ToArray();
        }

        // All this to avoid bounds checking on array calculations...should have just
        // set a sentinel perimiter around the grid
        IEnumerable<int> adjTo(int x)
        {
            int[] adj;
            if (x % 10 == 0)
                adj = _leftSide;
            else if ((x + 1) % 10 == 0)
                adj = _rightSide;
            else
                adj = _adjAll;
            foreach (int d in adj)
            {
                int r = x + d;
                if (r >= 0 && r < _area)
                    yield return r;
            }
        }

        int[] Iterate(int[] src)
        {
            int[] dest = new int[_area];
            for (int j = 0; j < _area; j++)
                dest[j] = src[j] + 1;

            
            Queue<int> tens = new Queue<int>();
            for (int j = 0; j < _area; j++)
            {
                if (dest[j] == 10)
                {
                    tens.Enqueue(j);
                    ++_flashes;
                }
            }

            while (tens.Count > 0)
            {
                int x = tens.Dequeue();
                dest[x] = 0;
                foreach (int d in adjTo(x))
                {                    
                    if (dest[d] > 0)
                        ++dest[d];
                    if (dest[d] == 10)
                    {
                        ++_flashes;
                        tens.Enqueue(d);
                    }                   
                }
            }

            if (_part2 && dest.Sum() == 0)
                throw new AllFashedException();
            return dest;
        }

        void PrintGrid(int[] grid)
        {
            for (int r = 0; r < _height; r++)
            {
                StringBuilder sb = new StringBuilder();
                for (int c = 0; c < _width; c++)
                    sb.Append(grid[r * _width + c]);
                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine("");
        }

        public void Solve()
        {
            var grid = FetchData();

            for (int _ = 0; _ < 100; _++)
                grid = Iterate(grid);
            Console.WriteLine($"P1: {_flashes}");

            _part2 = true;
            grid = FetchData();
            int step = 0;
            while (true)
            {
                try
                {
                    ++step;
                    grid = Iterate(grid);
                }
                catch (AllFashedException)
                {
                    Console.WriteLine($"P2: {step}");
                    break;
                }
            }

            grid.PrintGrid(_width);
        }
    }
}
