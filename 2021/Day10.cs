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
                switch (c)
                {
                    case '{':
                    case '[':
                    case '<':
                    case '(':
                        stack.Push(c);
                        break;
                    default:
                        char p = stack.Pop();
                        if (matches[c] != p)
                            return c;
                        break;                       
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
                switch (c)
                {
                    case ')':
                        total += 3;
                        break;
                    case ']':
                        total += 57;
                        break;
                    case '}':
                        total += 1197;
                        break;
                    case '>':
                        total += 25137;
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"P1: {total}");
        }
    }
}
