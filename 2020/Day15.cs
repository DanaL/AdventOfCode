using System;
using System.Collections.Generic;

namespace _2020
{
    public class Day15
    {
        public Day15() { }

        public void Solve()
        {
            int[] start = { 2, 0, 1, 9, 5, 19 };
            Stack<int> history = new Stack<int>();            
            Dictionary<int, int> memory = new Dictionary<int, int>();
            foreach (var v in start)
                history.Push(v);
            int c = 0;
            foreach (var v in start[..^1])
                memory[v] = ++c;
            int turn = history.Count;

            while (turn < 2020)
            {
                int prev = history.Peek();
                if (!memory.ContainsKey(prev))
                {
                    memory[prev] = turn;
                    history.Push(0);
                }
                else
                {
                    var diff = turn - memory[prev];
                    memory[prev] = turn;
                    history.Push(diff);
                }

                ++turn;
            }

            Console.WriteLine($"P1: {history.Peek()}");
        }
    }
}
