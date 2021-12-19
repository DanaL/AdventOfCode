using System;
using System.IO;
using System.Text;

using AoC;

namespace _2021
{    
    enum TokenType
    {
        LeftBracket,
        RightBracket,
        Digit,
        Comma
    }

    class Token
    {
        public ulong Val { get; set; }
        public Token Left { get; set; }
        public Token Right { get; set; }
        public TokenType TType { get; set; }

        public static Token GetToken(char c)
        {
            return c switch
            {
                '[' => new Token() { TType = TokenType.LeftBracket },
                ']' => new Token() { TType = TokenType.RightBracket },
                ',' => new Token() { TType = TokenType.Comma },
                _ => new Token() { TType = TokenType.Digit, Val = (ulong) (c - '0') }
            };            
        }

        public Token Tail()
        {
            Token n;
            for (n = this; n.Right != null; n = n.Right)
                ;
            return n;
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
            _head = Token.GetToken(snailStr[0]);
            Token curr = _head, prev = _head;
            for (int j = 1; j < snailStr.Length; j++)
            {
                var d = Token.GetToken(snailStr[j]);
                d.Left = curr;
                curr.Right = d;
                curr = d;
            }
        }

        public SnailNum Add(SnailNum other)
        {
            var newHead = Token.GetToken('[');
            var comma = Token.GetToken(',');
            var close = Token.GetToken(']');

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

            var newSN = new SnailNum(newHead);
            newSN.Reduce();

            return newSN;
        }

        private void Reduce()
        {            
            do
            {
                if (CheckForExplosion())
                    continue;
                if (CheckForSplit())
                    continue;
                break;
            } while (true);
        }

        private void ExplodeAt(Token d)
        {
            var v = d.Right;
            // scan left looking for a number
            ulong leftVal = v.Val;

            for (var n = v.Left; n.Left != null; n = n.Left)
            {
                if (n.TType == TokenType.Digit)
                {
                    n.Val += leftVal;
                    break;
                }
            }

            // check for rightward number
            v = d.Right.Right.Right; // skip comma
            ulong rightVal = v.Val;
            v = v.Right; // pass right value
            for (var n = v.Right; n.Right != null; n = n.Right)
            {
                if (n.TType == TokenType.Digit)
                {
                    n.Val += rightVal;
                    break;
                }
            }

            // Now, replace pair with 0
            var prev = d.Left;
            var next = d.Right.Right.Right.Right.Right;
            var zero = Token.GetToken('0');
            prev.Right = zero;
            zero.Left = prev;
            zero.Right = next;
            next.Left = zero;

            //Console.WriteLine($"After explosion: {this}");
        }

        public bool CheckForExplosion()
        {
            int bcount = 0;
            for (var t = _head; t != null; t = t.Right)
            {
                if (t.TType == TokenType.LeftBracket)
                    ++bcount;
                if (t.TType == TokenType.RightBracket)
                    --bcount;
                if (bcount >= 5 && t.Right.TType == TokenType.Digit && t.Right.Right.Right.TType == TokenType.Digit)
                {
                    ExplodeAt(t);
                    return true;
                }
            }

            return false;
        }

        void DoSplit(Token t)
        {
            float tval = t.Val;
            int leftVal = (int)Math.Floor(tval / 2);
            int rightVal = (int)Math.Ceiling(tval / 2);

            var newPair = new SnailNum($"[{(char)(leftVal +'0')},{(char)(rightVal +'0')}]");
            t.Left.Right = newPair._head;
            newPair._head.Left = t.Left;
            var tail = newPair._head.Tail();
            tail.Right = t.Right;
            tail.Right.Left = tail;

            //Console.WriteLine($"After split: {this}");
        }

        public bool CheckForSplit()
        {
            for (var t = _head; t != null; t = t.Right)
            {
                if (t.TType == TokenType.Digit && t.Val > 9)
                {
                    DoSplit(t);
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (var n = _head; n != null; n = n.Right)
            {
                string s = n.TType switch
                {
                    TokenType.LeftBracket => "[",
                    TokenType.RightBracket => "]",
                    TokenType.Comma => ",",
                    _ => n.Val.ToString()
                };
                sb.Append(s);
            }

            return sb.ToString();
        }

        public ulong Magnitiude()
        {
            var sn = new SnailNum(this.ToString());

            while (true)
            {
                bool simplePair = false;
                var n = sn._head;
                while (n.Right != null)
                {
                    if (n.Right.TType == TokenType.Digit && n.Right.Right.Right.TType == TokenType.Digit)
                    {
                        var left = n.Right.Val;
                        var right = n.Right.Right.Right.Val;
                        var m = n.Right;
                        m.Val = left * 3 + right * 2;
                        if (n.Left == null || n.Right.Right.Right.Right == null)
                            return m.Val;
                        n.Left.Right = m;
                        m.Right = n.Right.Right.Right.Right.Right; // perfectly ordinary double linked list code...
                        simplePair = true;                        
                    }
                    n = n.Right;
                }

                if (!simplePair)
                    break;
            }
            
            return 0;
        }
    }

    public class Day18 : IDay
    {       
        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day18.txt");
            var res = new SnailNum(lines[0]);
            for (int j = 1; j < lines.Length; j++)
                res = res.Add(new SnailNum(lines[j]));
            Console.WriteLine($"P1: {res.Magnitiude()}");

            var sn0 = new SnailNum(lines[68]);
            var sn1 = new SnailNum(lines[84]);           
            //var r = sn0.Add(sn1);
            //Console.WriteLine(r);
            //Console.WriteLine(r.Magnitiude());
            //ulong greatest = 0;
            //for (int j = 0; j < lines.Length - 1; j++)
            //{
            //    for (int k = j + 1; k < lines.Length - 2; k++)
            //    {
            //        var sn0 = new SnailNum(lines[j]);
            //        var sn1 = new SnailNum(lines[k]);
            //        try
            //        {                        
            //            ulong mn = sn0.Add(sn1).Magnitiude();
            //            if (mn > greatest)
            //                greatest = mn;
            //            //sn0 = new SnailNum(lines[j]);
            //            //sn1 = new SnailNum(lines[k]);
            //            //mn = sn0.Add(sn1).Magnitiude();
            //            //if (mn > greatest)
            //            //    greatest = mn;
            //        }
            //        catch (Exception)
            //        {
            //            Console.WriteLine($"{j}, {k}");
            //            Console.WriteLine(sn0);
            //            Console.WriteLine(sn1);
            //        }
            //    }
            //}
            //Console.WriteLine($"P2: {greatest}");
        }
    }
}
