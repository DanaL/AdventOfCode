using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AoC;

namespace _2021
{    
    public class Day14 : IDay
    {
        Dictionary<char, int> _counts;

        static (Dictionary<string, char>, string) FetchInput()
        {
            var rules = new Dictionary<string, char>();
            Regex re = new Regex(@"^(\w+) -> (\w+)$");
            var lines = File.ReadAllLines("inputs/day14.txt");
            foreach (var line in lines.Skip(2))
            {
                Match m = re.Match(line.Trim());
                if (m.Success)
                    rules.Add(m.Groups[1].Value, m.Groups[2].Value[0]);
            }

            return (rules, lines[0]);
        }

        string Step(string chain, Dictionary<string, char> rules)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < chain.Length - 1; j++)
            {
                sb.Append(chain[j]);
                string pair = $"{chain[j]}{chain[j + 1]}";
                if (rules.ContainsKey(pair))
                {
                    sb.Append(rules[pair]);
                    ++_counts[rules[pair]];                    
                }
            }
            sb.Append(chain[chain.Length - 1]);

            return sb.ToString();
        }

        void Part1()
        {
            var result = FetchInput();
            var rules = result.Item1;            
            var chain = result.Item2;

            _counts = rules.Values.Distinct().Select(c => (c, 0)).ToDictionary(c => c.Item1, c => c.Item2);
            foreach (char ch in chain)
                ++_counts[ch];

            for (int _ = 0; _ < 10; _++)
                chain = Step(chain, rules);
            Console.WriteLine($"P1: {_counts.Values.Max() - _counts.Values.Min()}");
        }

        public void Solve()
        {
            Part1();
        }
    }
}
