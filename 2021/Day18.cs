using System;
using System.Text;

using AoC;

namespace _2021
{
    class Pair
    {
        public Elt Left { get; set; }
        public Elt Right { get; set; }

        public Pair() { }

        public Pair(int left, int right)
        {
            Left = new Elt(left);
            Right = new Elt(right);
        }

        public Pair(Elt left, Elt right)
        {
            Left = left;
            Right = right;
        }

        public Pair Add(Elt left, Elt right)
        {
            return new Pair()
            {
                Left = left,
                Right = right
            };
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            if (Left.IsNum)
                sb.Append(Left.Val);
            else
                sb.Append(Left.Pair.ToString());
            sb.Append(", ");
            if (Right.IsNum)
                sb.Append(Right.Val);
            else
                sb.Append(Right.Pair.ToString());
            sb.Append(']');

            return sb.ToString();
        }
    }

    class Elt
    {
        public bool IsNum { get; set; }
        public int Val { get; set; }
        public Pair Pair { get; set; }

        public Elt(int x)
        {
            Val = x;
            IsNum = true;
        }

        public Elt(Pair p)
        {
            Pair = p;
            IsNum = false;
        }

        public override string ToString()
        {
            return IsNum ? Val.ToString() : Pair.ToString();
        }
    }

    public class Day18 : IDay
    {
        Elt ParseSnailNumber(string txt, ref int pos)
        {
            if (txt[pos] == '[')
            {
                // start new pair
                pos += 1;
                var left = ParseSnailNumber(txt, ref pos);
                var right = ParseSnailNumber(txt, ref pos);
                pos += 1; // skip the closing ]
                Pair p = new Pair(left, right);
                return new Elt(p);
            }
            else
            {
                int x = txt[pos] - '0';
                pos += 2;
                return new Elt(x);
            }
        }

        public void Solve()
        {
            int x = 0;
            var elt = ParseSnailNumber("[[9,9],[[3,[1,4]],[7,0]]]", ref x);
            Console.WriteLine(elt.ToString());
            Console.WriteLine(elt.Pair.Right.ToString());

            x = 0;
            var elt2 = ParseSnailNumber("[[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]", ref x);
            Console.WriteLine(elt2.ToString());
            Console.WriteLine(elt2.Pair.Left.ToString());

            var a = new Elt(new Pair(4, 5));
            var b = new Elt(new Pair(7, 2));
            var c = new Elt(new Pair(a, b));
            Console.WriteLine(c.ToString());
        }
    }
}
