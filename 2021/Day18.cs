using System;
using System.Text;

using AoC;

namespace _2021
{    
    class Token
    {
        public char Bit { get; set; }
        public Token Left { get; set; }
        public Token Right { get; set; }

        public Token(char bit)
        {
            Bit = bit;            
        }

        public Token Tail()
        {
            Token n;
            for (n = this; n.Right != null; n = n.Right)
                ;
            return n;
        }

        public bool IsDigit()
        {
            return Bit switch
            {
                '[' => false,
                ']' => false,
                ',' => false,
                _ => true
            };
        }
    }

    class SnailNum
    {
        Token _head;

        public SnailNum(Token d)
        {
            _head = d;
        }

        public SnailNum(string snailStr)
        {
            _head = new Token(snailStr[0]);
            Token curr = _head, prev = _head;
            for (int j = 1; j < snailStr.Length; j++)
            {
                var d = new Token(snailStr[j]);
                d.Left = curr;
                curr.Right = d;
                curr = d;
            }
        }

        public SnailNum Add(SnailNum other)
        {
            var newHead = new Token('[');
            var comma = new Token(',');
            var close = new Token(']');

            newHead.Right = _head;
            _head.Left = newHead;

            var tail = _head.Tail();
            tail.Right = comma;
            comma.Left = tail;

            comma.Right = other._head;
            other._head.Left = comma;

            tail = other._head.Tail();
            tail.Right = close;
            close.Left = tail;

            return new SnailNum(newHead);
        }

        private void ExplodeAt(Token d)
        {
            var v = d.Right;
            // scan left looking for a number
            int leftVal = v.Bit - '0';

            for (var n = v.Left; n.Left != null; n = n.Left)
            {
                if (n.IsDigit())
                {
                    n.Bit = (char)(n.Bit + leftVal);   // I think this should temporarily handle the case of 9 + 3 = 12?
                    break;                             // It'll store 12 as '<' and those are the numbers to shrink later
                }
            }

            // check for rightward number
            v = d.Right.Right.Right; // skip comma
            int rightVal = v.Bit - '0';
            v = v.Right; // pass right value
            for (var n = v.Right; n.Right != null; n = n.Right)
            {
                if (n.IsDigit())
                {
                    n.Bit = (char)(n.Bit + rightVal);   // I think this should temporarily handle the case of 9 + 3 = 12?
                    break;                             // It'll store 12 as '<' and those are the numbers to shrink later
                }
            }

            // Now, replace pair with 0
            var prev = d.Left;
            var next = d.Right.Right.Right.Right.Right;
            var zero = new Token('0');
            prev.Right = zero;
            zero.Left = prev;
            zero.Right = next;
            next.Left = zero;
        }

        public void CheckForExplosion()
        {
            int bcount = 0;
            Token d = _head;
            while (d != null)
            {
                if (d.Bit == '[')
                    ++bcount;
                if (bcount >= 5 && d.Right.IsDigit() && d.Right.Right.Right.IsDigit())
                {
                    ExplodeAt(d);
                    break;
                }
                d = d.Right;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (var n = _head; n != null; n = n.Right)
            {
                sb.Append(n.Bit);
                if (n.Bit == ',')
                    sb.Append(' ');
            }

            return sb.ToString();
        }
    }

    public class Day18 : IDay
    {       
        public void Solve()
        {
            var sn = new SnailNum("[[[[[9,8],1],2],3],4]");            
            Console.WriteLine(sn);
            sn.CheckForExplosion();
            Console.WriteLine(sn);

            var sn2 = new SnailNum("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
            Console.WriteLine(sn2);
            sn2.CheckForExplosion();
            Console.WriteLine(sn2);
        }
    }
}
