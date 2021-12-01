using System;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day01 : IDay
    {
        public Day01() {}

        private void part1()
        {
            var lines = File.ReadAllLines("inputs/day01.txt").Select(i => int.Parse(i)).ToArray();
            
            int increases = 0;
            for (int j = 1; j < lines.Length; j++)
            {
                if (lines[j] > lines[j - 1])
                    ++increases;
            }

            Console.WriteLine($"Part 1: {increases}");
        }

        public void Solve()
        {
            part1();
        }
    }
}
