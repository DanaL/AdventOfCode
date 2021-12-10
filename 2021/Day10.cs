using System;
using System.Collections.Generic;
using System.IO;

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

            int total = 0;
            foreach (var line in lines)
            {
                var c = ParseLine(line);
                if (c == ')')
                    total += 3;
                else if (c == ']')
                    total += 57;
                else if (c == '}')
                    total += 1197;
                else if (c == '>')
                    total += 25137;
            }

            Console.WriteLine($"P1: {total}");
        }
    }
}
