using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day14
    {
        public Day14() { }        
    
        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day14.txt");

            Dictionary<long, long> mem = new Dictionary<long, long>();
            (long Or, long And) mask = (0, 0);
            foreach (var line in tr.ReadToEnd().Split('\n'))
            {
                if (line.StartsWith("mask"))
                {
                    var pieces = line.Split(" = ");
                    mask = (Or: Convert.ToInt64(pieces[1].Replace("X", "0"), 2), And: Convert.ToInt64(pieces[1].Replace("X", "1"), 2));
                }
                else
                {
                    var pieces = Regex.Match(line, @"mem\[(?<p0>\d+)\] = (?<p1>\d+)");
                    long memLoc = long.Parse(pieces.Groups["p0"].Value);
                    long val = long.Parse(pieces.Groups["p1"].Value);

                    val &= mask.And;
                    val |= mask.Or;

                    mem[memLoc] = val;
                }
            }

            Console.WriteLine($"P1: {mem.Values.Sum()}");
        }
    }
}
