using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AoC;

namespace _2021
{
    public class Day10 : IDay
    {
        private Stack<char> ParseLine(string line)
        {
            var matches = new Dictionary<char, char>() { { '}', '{' }, { ')', '(' }, { '>', '<' }, { ']', '[' } };
            var stack = new Stack<char>();

            foreach (char c in line)
            {
                if (c == '{' || c == '[' || c == '<' || c == '(')
                    stack.Push(c);
                else
                {
                    char p = stack.Pop();
                    if (matches[c] != p)
                        throw new Exception(c.ToString());                    
                }             
            }

            return stack;
        }

        private char CheckForCorruptedLine(string line)
        {
            try
            {
                ParseLine(line);
                return '\0';
            }
            catch (Exception ex)
            {
                return ex.Message[0];
            }
        }

        private string CompleteLine(string line)
        {
            var matches = new Dictionary<char, char>() { { '{', '}' }, { '(', ')' }, { '<', '>' }, { '[', ']' } };
            StringBuilder closers = new StringBuilder();
            try
            {
                Stack<char> stack = ParseLine(line);
                // We should have only unmatched symbols left            
                while (stack.Count > 0)
                    closers.Append(matches[stack.Pop()]);
            }
            catch (Exception)
            {
                return "error";
            }
            
            return closers.ToString();
        }

        private long ScorePart2(string s)
        {
            var values = new Dictionary<char, long>() { { '}', 3 }, { ')', 1 }, { '>', 4 }, { ']', 2 } };
            return s.Aggregate(0, (long a, char b) => a * 5 + values[b]);            
        }

        private void Part1()
        {
            var lines = File.ReadAllLines("inputs/day10.txt");

            var values = new Dictionary<char, int>() { { '}', 1197 }, { ')', 3 }, { '>', 25137 }, { ']', 57 }, { '\0', 0 } };
            int total = lines.Select(l => values[CheckForCorruptedLine(l)]).Sum();

            Console.WriteLine($"P1: {total}");
        }

        private void Part2()
        {
            var lines = File.ReadAllLines("inputs/day10.txt");

            List<long> scores = lines.Select(CompleteLine)
                                     .Where(l => l != "error")
                                     .Select(ScorePart2)
                                     .OrderBy(s => s).ToList();            
            Console.WriteLine($"P2: {scores[scores.Count / 2]}");
        }

        public void Solve()
        {
            Part1();
            Part2();
        }
    }
}