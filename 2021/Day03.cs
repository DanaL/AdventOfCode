using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day03 : IDay
    {
        public Day03() { }

        private List<string> fetchProblemInput()
        {
            return File.ReadAllLines("inputs/day03.txt").ToList();
        }

        private int[] countSignals(List<string> lines)
        {
            int[] signals = new int[lines[0].Length];

            foreach (string line in lines)
            {
                for (int j = 0; j < line.Length; j++)
                    signals[j] += line[j] == '1' ? 1 : 0;
            }

            return signals;
        }

        private void part1()
        {
            var lines = fetchProblemInput();
            int[] signals = countSignals(lines);
            var gamma = new string(signals.Select(b => b > lines.Count / 2 ? '1' : '0').ToArray());
            var epsilon = new string(signals.Select(b => b < lines.Count / 2 ? '1' : '0').ToArray());

            Console.WriteLine($"P1: {Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2)}");
        }

        private string filterO2(List<string> lines, int position)
        {
            if (lines.Count == 1)
                return lines[0];

            int ones = lines.Select(l => l[position]).Where(i => i == '1').Count();
            int zeroes = lines.Count - ones;
            char seeking = ones >= zeroes ? '1' : '0';
            var keep = lines.Where(l => l[position] == seeking).ToList();

            return filterO2(keep, position + 1);
        }

        private string filterCO2(List<string> lines, int position)
        {
            if (lines.Count == 1)
                return lines[0];

            int ones = lines.Select(l => l[position]).Where(i => i == '1').Count();
            int zeroes = lines.Count - ones;
            char seeking = ones >= zeroes ? '0' : '1';
            var keep = lines.Where(l => l[position] == seeking).ToList();

            return filterCO2(keep, position + 1);
        }

        private void part2()
        {
            var lines = fetchProblemInput();

            var o2 = filterO2(lines, 0);
            var co2 = filterCO2(lines, 0);

            Console.WriteLine($"P1: {Convert.ToInt32(o2, 2) * Convert.ToInt32(co2, 2)}");
        }

        public void Solve()
        {
            part1();
            part2();
        }
    }
}
