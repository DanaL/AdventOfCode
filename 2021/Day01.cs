using System;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day01 : IDay
    {
        public Day01() {}

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day01.txt").Select(i => int.Parse(i)).ToArray();
            int sum = lines[0] + lines[1] + lines[2];

            int increasesPt1 = 0;
            int increasesPt2 = 0;
            for (int j = 1; j < lines.Length; j++)
            {
                if (lines[j] > lines[j - 1])
                    ++increasesPt1;

                if (j < 3)
                    continue;

                int nextSum = sum - lines[j - 3] + lines[j];
                if (nextSum > sum)
                    ++increasesPt2;
                sum = nextSum;
            }

            Console.WriteLine($"Part 1: {increasesPt1}");
            Console.WriteLine($"Part 2: {increasesPt2}");
        }
    }
}
