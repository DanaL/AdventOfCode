using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    internal struct Signal
    {
        private bool[] _bits;
        
        public int Value
        {
            get
            {
                int x = 0;
                int p = 0;
                for (int j = 6; j >= 0; j--)
                {
                    if (_bits[j])
                        x += (int)Math.Pow(2, p);                    
                    p++;
                }

                switch (x)
                {
                    case 119:
                        return 0;
                    case 18:
                        return 1;
                    case 93:
                        return 2;
                    case 91:
                        return 3;
                    case 58:
                        return 4;
                    case 107:
                        return 5;
                    case 111:
                        return 6;
                    case 82:
                        return 7;
                    case 127:
                        return 8;
                    case 123:
                        return 9;
                    default:
                        // This is an error condition actually but...
                        return -1;
                }
            }
        }

        public Signal(string bitString) 
        {
            _bits = new bool[7];
            foreach (char c in bitString)
                _bits[(int)c - (int)'a'] = true;
        }
    }

    public class Day08 : IDay
    {        
        void PartOne()
        {
            // This was...rather trivial for a part 1? It's barely more than 'successfully read in the file'
            int sum = 0;
            foreach (var line in File.ReadAllLines("inputs/day08.txt"))
            {
                var pieces = line.Split('|');
                string signal = pieces[0].Trim();
                string output = pieces[1].Trim();
                sum += output.Split(' ')
                            .Where(w => w.Length == 2 || w.Length == 3 || w.Length == 4 || w.Length == 7)
                            .Count();
            }

            Console.WriteLine($"P1: {sum}");
        }

        void PartTwo()
        {
            Signal s = new Signal("abcdefg");
            Console.WriteLine((new Signal("efgabc")).Value);
            Console.WriteLine((new Signal("fc")).Value);
            Console.WriteLine((new Signal("acdeg")).Value);
            Console.WriteLine((new Signal("acdgf")).Value);
            Console.WriteLine((new Signal("fdbc")).Value);
            Console.WriteLine((new Signal("abdfg")).Value);
            Console.WriteLine((new Signal("abdfeg")).Value);
            Console.WriteLine((new Signal("caf")).Value);
            Console.WriteLine((new Signal("gfedcba")).Value);
            Console.WriteLine((new Signal("abcdfg")).Value);
        }

        public void Solve()
        {
            PartOne();
            PartTwo();
        }
    }
}
