using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day1
    {
        private List<int> expenses;

        public Day1()
        {
            using TextReader tr = new StreamReader("inputs/day1.txt");

            this.expenses = (from line in tr.ReadToEnd().Split("\n")
                              select int.Parse(line)).ToList();
        }

        public void SolvePart1()
        {
            // Keep track of numbers we've seen in the input so far so that we
            // don't actually have to do two loops like my original naive
            // implementation. And then I can use a break statement instead of
            // a goto
            HashSet<int> seen = new HashSet<int>();
            for (int j = 0; j < expenses.Count; j++)
            {
                seen.Add(expenses[j]);
                int lookingFor = 2020 - expenses[j];

                if (seen.Contains(lookingFor))
                {
                    Console.WriteLine($"P1: {expenses[j] * lookingFor}");
                    return;
                }
            }
        }

        public void SolvePart2()
        {
            // sample/test input
            //_expenses = new List<int>() { 1721, 979, 366, 299, 675, 1456 };

            // Similar to part one, use a hashset to track values we've seen so
            // that we only need one nested loop O(n^2) instead of O(n^3)
            HashSet<int> seen = new HashSet<int>();
            for (int a = 0; a < expenses.Count - 1; a++)
            {
                seen.Add(expenses[a]);
                for (int b = a + 1; b < expenses.Count; b++)
                {
                    seen.Add(expenses[b]);

                    int lookingFor = 2020 - expenses[a] - expenses[b];
                    if (seen.Contains(lookingFor))
                    {
                        int res = expenses[a] * expenses[b] * lookingFor;
                        Console.WriteLine($"P2: {res}");
                        return;
                    }
                }
            }
        }
    }
}
