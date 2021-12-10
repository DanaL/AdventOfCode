using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day10.txt");

            var values = new Dictionary<char, int>() { { '}', 1197 }, { ')', 3 }, { '>', 25137 }, { ']', 57 }, { '\0', 0 } };
            int total = lines.Select(l => values[ParseLine(l)]).Sum();

            Console.WriteLine($"P1: {total}");
        }
    }
}
