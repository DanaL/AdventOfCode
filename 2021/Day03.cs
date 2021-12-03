using System;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day03 : IDay
    {
        public Day03() { }

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day03.txt");
            int[] signals = new int[lines[0].Length];

            foreach (string line in lines)
            {
                for (int j = 0; j < line.Length; j++)
                    signals[j] += line[j] == '1' ? 1 : 0;
            }

            var gamma = new string(signals.Select(b => b > lines.Length / 2 ? '1' : '0').ToArray());
            var epsilon = new string(signals.Select(b => b < lines.Length / 2 ? '1' : '0').ToArray());

            Console.WriteLine($"P1: {Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2)}");
        }
    }
}
