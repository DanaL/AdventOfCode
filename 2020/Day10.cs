using System;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day10
    {
        public Day10() { }

        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day10.txt");
            var adapters = tr.ReadToEnd().Split('\n')
                             .Select(n => int.Parse(n)).OrderBy(n => n).ToArray();

            var v = adapters.Zip(adapters.Skip(1), (a, b) => b - a);
            Console.WriteLine($"P1: {(v.Where(x => x == 1).Count() + 1) * (v.Where(x => x == 3).Count() + 1)}");
        }
    }
}
