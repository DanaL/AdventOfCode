using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day9
    {
        public Day9() { }

        private bool numInRange(long[] nums, long target, int start, int end)
        {
            for (int j = start; j < nums.Length && j < end; j++)
            {
                if (nums[j] == target)
                    return true;
            }

            return false;
        }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day9.txt");
            var nums = tr.ReadToEnd().Split('\n').Select(n => long.Parse(n)).ToArray();

            for (int i = 25; i < nums.Length; i++)
            {
                bool found = false;
                long target = nums[i];
                for (int j = i - 25; j < i; j++)
                {
                    long a = nums[j];
                    if (a >= target)
                        continue;
                    long b = target - a;
                    if (numInRange(nums, b, j + 1, i))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"P1: {target}");
                    break;
                }
            }
        }
    }
}
