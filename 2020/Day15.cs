using System;

namespace _2020
{
    public class Day15
    {
        public Day15() { }

        private int findNth(int[] start, int to)
        {
            int prev = start[^1];

            // Using an array to store when the number was previous makes the
            // processing time for part 2 drop from ~2.4 seconds on my iMac to
            // 0.5!
            int[] memory = new int[30_000_000];
            //Dictionary<int, int> memory = new Dictionary<int, int>();
            int c = 0;
            foreach (var v in start[..^1])
                memory[v] = ++c;
            int turn = start.Length;

            while (turn < to)
            {
                if (memory[prev] == 0)
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
            Console.WriteLine($"P1: {findNth(start, 2020)}");
            DateTime before = DateTime.Now;
            Console.WriteLine($"P1: {findNth(start, 30_000_000)}");
            var delta = DateTime.Now.Subtract(before);
            Console.WriteLine($"Time for part 2: {delta.TotalMilliseconds} ms");
        }
    }
}
