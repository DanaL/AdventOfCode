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

        private (long Loc, long Val) parseWrite(string line)
        {
            var pieces = Regex.Match(line, @"mem\[(?<p0>\d+)\] = (?<p1>\d+)");

            return (Loc: long.Parse(pieces.Groups["p0"].Value), Val: long.Parse(pieces.Groups["p1"].Value));
        }

        private void part1()
        {
            TextReader tr = new StreamReader("inputs/day14.txt");

            Dictionary<long, long> mem = new Dictionary<long, long>();
            (long Or, long And) mask = (0, 0);
            foreach (var line in tr.ReadToEnd().Split('\n'))
            {
                if (line.StartsWith("mask"))
                {
                    var m = line.Split(" = ")[1];
                    mask = (Or: Convert.ToInt64(m.Replace("X", "0"), 2), And: Convert.ToInt64(m.Replace("X", "1"), 2));
                }
                else
                {
                    var a = parseWrite(line);
                    a.Val &= mask.And;
                    a.Val |= mask.Or;
                    mem[a.Loc] = a.Val;
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
            TextReader tr = new StreamReader("inputs/day14.txt");

            Dictionary<long, long> mem = new Dictionary<long, long>();
            string mask = "";
            foreach (var line in tr.ReadToEnd().Split('\n'))
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Split(" = ")[1];
                }
                else
                {
                    var a = parseWrite(line);

                    // First, flip any bits that are 1 in the mask
                    long or = Convert.ToInt64(mask.Replace('X', '0'), 2);
                    a.Loc |= or;

                    var loc = Convert.ToString(a.Loc, 2).PadLeft(36, '0').ToCharArray();
                    var revMask = string.Concat(mask.ToCharArray().Reverse());
                    for (int j = 0; j < revMask.Length; j++)
                    {
                        if (revMask[j] == 'X')
                            loc[^(j + 1)] = 'X';
                    }
                    writeToMem(string.Concat(loc), a.Val, mem);
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
