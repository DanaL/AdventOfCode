using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day03 : IDay
    {
        public Day03() { }

        List<string> fetchProblemInput()
        {
            return File.ReadAllLines("inputs/day03.txt").ToList();
        }

        void part1()
        {
            var lines = fetchProblemInput();
            int[] signals = new int[lines[0].Length];
            foreach (string line in lines)
            {
                for (int j = 0; j < line.Length; j++)
                    signals[j] += line[j] == '1' ? 1 : 0;
            }

            var gamma = new string(signals.Select(b => b > lines.Count / 2 ? '1' : '0').ToArray());
            var epsilon = new string(signals.Select(b => b < lines.Count / 2 ? '1' : '0').ToArray());

            Console.WriteLine($"P1: {Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2)}");
        }

        string filter(List<string> lines, int position, Func<int, int, char> f)
        {
            if (lines.Count == 1)
                return lines[0];

            int ones = lines.Where(line => line[position] == '1').Count();
            int zeroes = lines.Count - ones;
            char seeking = f(ones, zeroes);
            var keep = lines.Where(l => l[position] == seeking).ToList();

            return filter(keep, position + 1, f);
        }

        void part2()
        {
            var lines = fetchProblemInput();

            var o2 = filter(lines, 0, (a, b) => { return a >= b ? '1' : '0'; });
            var co2 = filter(lines, 0, (a, b) => { return a < b ? '1' : '0'; });
            
            Console.WriteLine($"P2: {Convert.ToInt32(o2, 2) * Convert.ToInt32(co2, 2)}");
        }

        public void Solve()
        {
            part1();
            part2();
        }
    }
}
