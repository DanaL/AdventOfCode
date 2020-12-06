using System;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day6
    {
        public Day6() { }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day6.txt");
            var groups = tr.ReadToEnd().Split("\n\n");
            
            var sum = 0;
            foreach (var g in groups.Select(g => g.Replace("\n", "")))
            {
                sum += g.ToCharArray().Distinct().Count();
            }
            Console.WriteLine($"P1: {sum}");

            var totalYes = 0;
            foreach (var grp in groups)
            {
                var answers = grp.ToCharArray()
                                .GroupBy(ch => ch)
                                .ToDictionary(g => g.Key, g => g.ToList().Count);

                var group_size = answers.ContainsKey('\n') ? answers['\n'] + 1 : 1;
                foreach (var k in answers.Keys)
                {
                    if (answers[k] == group_size)
                        totalYes += 1;
                }
            }
            Console.WriteLine($"P2: {totalYes}");
        }
    }
}
