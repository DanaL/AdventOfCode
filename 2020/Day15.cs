using System;
using System.Collections.Generic;

namespace _2020
{
    public class Day15
    {
        public Day15() { }

        private int findNth(int[] start, int to, bool verbose)
        {
            int prev = start[^1];
            Dictionary<int, int> memory = new Dictionary<int, int>();
            int c = 0;
            foreach (var v in start[..^1])
                memory[v] = ++c;
            int turn = start.Length;

            while (turn < to)
            {
                if (!memory.ContainsKey(prev))
                {
                    memory[prev] = turn;
                    prev = 0;
                }
                else
                {
                    var diff = turn - memory[prev];
                    memory[prev] = turn;
                    prev = diff;
                }

                ++turn;
            }

            return prev;
        }

        public void Solve()
        {
            int[] start = { 2, 0, 1, 9, 5, 19 };
            Console.WriteLine($"P1: {findNth(start, 2020, false)}");
            DateTime before = DateTime.Now;
            Console.WriteLine($"P1: {findNth(start, 30_000_000, false)}");
            var delta = DateTime.Now.Subtract(before);
            Console.WriteLine($"Time for part 2: {delta.TotalMilliseconds} ms");
        }
    }
}
