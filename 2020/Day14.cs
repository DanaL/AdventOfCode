using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day14 : IDay
    {
        private List<Match> _lines;

        public Day14()
        {
            string pattern = @"(mask = (?<mask>[01X]{36}))|(mem\[(?<memLoc>\d+)\] = (?<val>\d+))";
            
            TextReader tr = new StreamReader("inputs/day14.txt");
            _lines = tr.ReadToEnd().Split('\n')
                       .Select(line => Regex.Match(line, pattern))
                       .ToList();
        }        

        private void part1()
        {
            Dictionary<long, long> mem = new Dictionary<long, long>();
            (long Or, long And) mask = (0, 0);
            foreach (var line in _lines)
            {
                if (line.Groups["mask"].Success)
                {
                    var m = line.Groups["mask"].Value;
                    mask = (Or: Convert.ToInt64(m.Replace("X", "0"), 2), And: Convert.ToInt64(m.Replace("X", "1"), 2));
                }
                else
                {
                    mem[long.Parse(line.Groups["memLoc"].Value)] = long.Parse(line.Groups["val"].Value) & mask.And | mask.Or;
                }
            }

            Console.WriteLine($"P1: {mem.Values.Sum()}");
        }

        private void writeToMem(string loc, long val, Dictionary<long, long> mem)
        {
            var xes = loc.Select((c, i) => c == 'X' ? i : -1).Where(n => n > -1).ToArray();

            if (xes.Length == 0)
            {
                mem[Convert.ToInt64(string.Concat(loc), 2)] = val;
            }
            else
            {
                var floating = loc.ToCharArray();
                floating[xes[0]] = '1';
                writeToMem(string.Concat(floating), val, mem);
                floating[xes[0]] = '0';
                writeToMem(string.Concat(floating), val, mem);                
            }            
        }

        private void part2()
        {
            Dictionary<long, long> mem = new Dictionary<long, long>();
            string mask = "";
            foreach (var line in _lines)
            {
                if (line.Groups["mask"].Success)
                {
                    mask = line.Groups["mask"].Value;
                }
                else
                {                    
                    // First, flip any bits that are 1 in the mask
                    long or = Convert.ToInt64(mask.Replace('X', '0'), 2);
                    var memLoc = long.Parse(line.Groups["memLoc"].Value) | or;
                    
                    var loc = Convert.ToString(memLoc, 2).PadLeft(36, '0').ToCharArray();
                    var revMask = string.Concat(mask.ToCharArray().Reverse());
                    for (int j = 0; j < revMask.Length; j++)
                    {
                        if (revMask[j] == 'X')
                            loc[^(j + 1)] = 'X';
                    }
                    writeToMem(string.Concat(loc), long.Parse(line.Groups["val"].Value), mem);
                }
            }

            Console.WriteLine($"P2: {mem.Values.Sum()}");
        }

        public void Solve()
        {
            part1();
            part2();
        }
    }
}
