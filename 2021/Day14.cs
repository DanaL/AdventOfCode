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
        static (Dictionary<string, (string, string, char)>, string) FetchInput()
        {
            var rules = new Dictionary<string, (string, string, char)>();
            Regex re = new Regex(@"^(\w+) -> (\w+)$"); var lines = File.ReadAllLines("inputs/day14.txt");
            foreach (var line in lines.Skip(2))
            {
                Match m = re.Match(line.Trim());
                if (m.Success) 
                {
                    string lt = m.Groups[1].Value;
                    string rt = m.Groups[2].Value;
                    rules.Add(lt, ($"{lt[0]}{rt}", $"{rt}{lt[1]}", rt[0]));
                }
            }

            return (rules, lines[0]);
        }

        ulong ProcessPolymer(int generations)
        {
            var result = FetchInput();
            var rules = result.Item1;
            var template = result.Item2;

            // Okay, set up the dictionary that tracks the number of times each pair
            // appears in the current polymer
            var pairCounts = new Dictionary<string, ulong>();
            foreach (var k in rules.Keys)
            {
                if (!pairCounts.ContainsKey(k))
                    pairCounts[k] = 0;
            }
            foreach (var v in rules.Values)
            {
                if (!pairCounts.ContainsKey(v.Item1))
                    pairCounts[v.Item1] = 0;
                if (!pairCounts.ContainsKey(v.Item2))
                    pairCounts[v.Item2] = 0;
            }

            // Pairs found in the initial template
            for (int j = 0; j < template.Length - 1; j++)
                ++pairCounts[$"{template[j]}{template[j + 1]}"];

            // Set up the dictionary to track the counts of each element
            Dictionary<char, ulong> eltCount = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(c => (c, (ulong) 0)).ToDictionary(c => c.Item1, c => c.Item2);
            foreach (char c in template)
                ++eltCount[c];

            // There's probably a less clunky way to do this, but I am figuring out, from the 
            // current state, what new pairs will be added. I've built the table so that as the 
            // original rule is, NN -> C, this turns NN into an NC and a CN. (So we have to subtract
            // the removed NNs from their counts. So first generate the list of what will be changed
            // in the current polymer, then actually do the changes and count the # of elements added.
            for (int _ = 0; _ < generations; ++_)
            {
                var changes = new List<(string, ulong)>();
                foreach (var k in pairCounts.Keys)
                {                    
                    ulong count = pairCounts[k];
                    if (count > 0)
                        changes.Add((k, count));                    
                }

                foreach (var change in changes)
                {
                    string pair = change.Item1;
                    ulong count = change.Item2;
                    eltCount[rules[pair].Item3] += count;
                    pairCounts[rules[pair].Item1] += count;
                    pairCounts[rules[pair].Item2] += count;
                    pairCounts[pair] -= count;                   
                }
            }

            return eltCount.Values.Max() - eltCount.Values.Where(v => v > 0).Min();
        }

        public void Solve()
        {
            Console.WriteLine($"P1: {ProcessPolymer(10)}");
            Console.WriteLine($"P2: {ProcessPolymer(40)}");
        }
    }
}
