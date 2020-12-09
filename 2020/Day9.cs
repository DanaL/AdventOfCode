﻿using System;
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

            // For Part 1, we are seeking a number where in the 25 preceeding numbers,
            // there aren't two numbers that add up to our target
            long partOne = -1;
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
                    partOne = target;
                    break;
                }
            }

            Console.WriteLine($"P1: {partOne}");

            // For Part 2, we need to find a contiguous block of numbers which add up
            // to the number we found in Part 1 and then add the smallest and largest in
            // that series together
            for (int j = 0; j < nums.Length; j++)
            {
                long sum = nums[j];
                long smallest = nums[j];
                long largest = nums[j];
                int k = j + 1;
                while (sum < partOne)
                {
                    if (nums[k] < smallest)
                        smallest = nums[k];
                    if (nums[k] > largest)
                        largest = nums[k];
                    sum += nums[k];
                    ++k;
                }

                if (sum == partOne)
                {
                    Console.WriteLine($"P2: {smallest + largest}");
                    break;
                }                
            }
        }
    }
}
