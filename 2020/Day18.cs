using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day18 : IDay
    {
        public Day18() { }

        private ulong doOp(Stack<ulong> stash, char op)
        {
            if (op == '+')
                return stash.Pop() + stash.Pop();
            else
                return stash.Pop() * stash.Pop();            
        }

        private ulong eval(Queue<char> expression, Dictionary<char, int> precedence)
        {
            Stack<ulong> stash = new Stack<ulong>();
            Stack<char> ops = new Stack<char>();
            
            while (expression.Count > 0)
            {
                char c = expression.Dequeue();
                if (c >= '0' && c <= '9')
                    stash.Push((ulong)c - 48);
                else if (c == '(')
                    stash.Push(eval(expression, precedence));
                else if (c == ')')
                    break;
                else if (ops.Count == 0 || precedence[c] < precedence[ops.Peek()])
                    ops.Push(c);
                else
                {
                    stash.Push(doOp(stash, ops.Pop()));
                    ops.Push(c);
                }                
            }
                    
            while (ops.Count > 0)
                stash.Push(doOp(stash, ops.Pop()));

            return stash.Peek();
        }

        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day18.txt");
            var lines = tr.ReadToEnd().Split('\n');

            var precPt1 = new Dictionary<char, int>() { ['+'] = 0, ['*'] = 0 };
            var precPt2 = new Dictionary<char, int>() { ['+'] = 0, ['*'] = 1 };
            ulong p1 = 0, p2 = 0;
            foreach (string line in lines)
            {
                p1 += eval(new Queue<char>(line.ToCharArray().Where(c => c != ' ')), precPt1);
                p2 += eval(new Queue<char>(line.ToCharArray().Where(c => c != ' ')), precPt2);
            }
            Console.WriteLine($"P1: {p1}");
            Console.WriteLine($"P2: {p2}");
        }
    }
}
