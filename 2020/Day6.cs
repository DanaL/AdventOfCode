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

            var sum = groups.Sum(g => g.Replace("\n", "").Distinct().Count());
            Console.WriteLine($"P1: {sum}");

            /* I feel like this is starting to get into abusing LINQ territory... */
            sum = groups.Sum(grp => {
                var answers = grp.ToCharArray()
                                .GroupBy(ch => ch)
                                .ToDictionary(g => g.Key, g => g.ToList().Count);

                var group_size = answers.ContainsKey('\n') ? answers['\n'] + 1 : 1;
                return answers.Keys.Select(k => answers[k]).Where(v => v == group_size).Count();
            });            
            Console.WriteLine($"P2: {sum}");
        }
    }
}
