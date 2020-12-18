using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day10 : IDay
    {
        private Dictionary<int, long> _branches;

        public Day10()
        {
            _branches = new Dictionary<int, long>();
        }

        private long branchesFrom(int[] adapters, int index)
        {
            int a = adapters[index];

            if (_branches.ContainsKey(a))
                return _branches[a];

            long count = 0;
            int x = index + 1;                        
            while (x < adapters.Length && adapters[x] - a <= 3)
            {
                int b = adapters[x];
                count += 1 + (_branches.ContainsKey(b) ? _branches[b] : branchesFrom(adapters, x));
                ++x;
            }

            _branches[a] = count - 1;

            return _branches[a];            
        }

        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day10.txt");
            var adapters = tr.ReadToEnd().Split('\n')
                             .Select(n => int.Parse(n)).OrderBy(n => n).ToArray();

            var v = adapters.Zip(adapters.Skip(1), (a, b) => b - a);
            Console.WriteLine($"P1: {(v.Where(x => x == 1).Count() + 1) * (v.Where(x => x == 3).Count() + 1)}");

            _branches[adapters.Max()] = 0;
            long p2 = 0;
            int i = 0;
            do
            {
                p2 += 1 + branchesFrom(adapters, i);
                ++i;
            } while (adapters[i] <= 3);

            Console.WriteLine($"P2: {p2}");
        }
    }
}
