using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AoC;

namespace _2021
{
    public class Day14 : IDay
    {
        static (Dictionary<string, string>, string) FetchInput()
        {
            var rules = new Dictionary<string, string>();
            Regex re = new Regex(@"^(\w+) -> (\w+)$");
            var lines = File.ReadAllLines("inputs/day14.txt");
            foreach (var line in lines.Skip(2))
            {
                Match m = re.Match(line.Trim());
                if (m.Success)
                    rules.Add(m.Groups[1].Value, m.Groups[2].Value);
            }

            return (rules, lines[0]);
        }

        public void Solve()
        {
            var result = FetchInput();
            var start = result.Item2;
            foreach (var item in result.Item1.Keys)
                Console.WriteLine($"{item} makes {result.Item1[item]}");
        }
    }
}
