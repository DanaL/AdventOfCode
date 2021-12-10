using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _2021
{
    public class Day10 : IDay
    {        
        private char ParseLine(string line)
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
                        return c;
                }             
            }

            return '\0';
        }

        private string CompleteLine(string line)
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
                        return "error";
                }
            }

            // We should have only unmatched symbols left
            StringBuilder closers = new StringBuilder();
            while (stack.Count > 0)
            {
                char c = stack.Pop();
                if (c == '{')
                    closers.Append('}');
                else if (c == '(')
                    closers.Append(')');
                else if (c == '<')
                    closers.Append('>');
                else if (c == '[')
                    closers.Append(']');
            }

            return closers.ToString();
        }

        private long ScorePart2(string s)
        {
            var values = new Dictionary<char, long>() { { '}', 3 }, { ')', 1 }, { '>', 4 }, { ']', 2 } };
            long score = 0;
            foreach (char c in s)
            {
                score *= 5;
                score += values[c];
            }

            return score;
        }

        private void Part1()
        {
            var lines = File.ReadAllLines("inputs/day10.txt");

            var values = new Dictionary<char, int>() { { '}', 1197 }, { ')', 3 }, { '>', 25137 }, { ']', 57 }, { '\0', 0 } };
            int total = lines.Select(l => values[ParseLine(l)]).Sum();

            Console.WriteLine($"P1: {total}");
        }

        private void Part2()
        {
            var lines = File.ReadAllLines("inputs/day10.txt");

            var scores = new List<long>();
            foreach (string line in lines)
            {
                var s = CompleteLine(line);
                if (s != "error")
                    scores.Add(ScorePart2(s));
            }
            scores.Sort();
            Console.WriteLine($"P2: {scores[scores.Count / 2]}");
        }

        public void Solve()
        {
            Part1();
            Part2();
        }
    }
}
