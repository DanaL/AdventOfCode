using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day18
    {
        public Day18() { }

        private ulong doOp(Stack<ulong> stash, char op)
        {
            ulong x = op == '+' ? 0 : 1;
            while (stash.Count > 0)
            {
                if (op == '+')
                    x += stash.Pop();
                else
                    x *= stash.Pop();
            }

            return x;
        }

        private ulong eval(Queue<char> expression)
        {
            Stack<ulong> stash = new Stack<ulong>();
            char op = '\0';

            while (expression.Count > 0)
            {
                char c = expression.Dequeue();
                if (c >= '0' && c <= '9')
                    stash.Push((ulong)c - 48);
                else if (c == '(')
                    stash.Push(eval(expression));
                else if (c == ')')
                    return doOp(stash, op);
                else if (op == '\0')
                    op = c;
                else if (op != c)
                {
                    stash.Push(doOp(stash, op));
                    op = c;
                }
            }

            return doOp(stash, op);
        }

        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day18.txt");
            var lines = tr.ReadToEnd().Split('\n');

            ulong sum = 0;
            foreach (string line in lines)
                sum += eval(new Queue<char>(line.ToCharArray().Where(c => c != ' ')));
            Console.WriteLine($"P1: {sum}");
        }
    }
}
