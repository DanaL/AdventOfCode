using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// https://adventofcode.com/2021/day/8

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

        int ParseSiginals(IEnumerable<string> signals, IEnumerable<string> outputs)
        {
            // Every line in file contains at least one of each digit

            // Step 1 -- find the 1
            string one = signals.Where(s => s.Length == 2).First();
            
            // Step 2 -- find the 7. The 'a' wire is the char that is in 7 and not in 1
            string seven = signals.Where(s => s.Length == 3).First();
            char aWire = seven.Where(c => !one.Contains(c)).First();
            
            // Step 3 -- find the 4. The two characters not in 1 must correspond to b and d
            string four = signals.Where(s => s.Length == 4).First();
            string bd = new string(four.Where(c => !one.Contains(c)).ToArray());

            // Step 4 -- Knowing which two wires must be b or d, 0, 6 and 9 all have six segments
            // but 6 and 9 contain wires b and d. 0 will contain only d so whichever of the possibilities
            // isn't found in 0, 6, or 9 is wire d. Once we know wire d, we know wire b.
            char dWire = '\0';
            foreach (var opt in signals.Where(s => s.Length == 6))
            {
                foreach (char c in bd)
                {
                    if (!opt.Contains(c))
                    {
                        dWire = c;
                        goto FOUND_D;
                    }
                }
            }

        FOUND_D:
            char bWire = bd.Where(c => c != dWire).First();

            // Step 5 -- Knowing a, b, and d wires, the 5 digit is the only length 5
            // string that contains the b wire. From that I can get the f wire (the c wire
            // is not contained in 5 so the wire from 1 that IS in five is c) and that
            // leaves only the g wire in 5 unaccounted for
            var five = signals.Where(s => s.Length == 5 && s.Contains(bWire)).First();
            char fWire = five.Contains(one[0]) ? one[0] : one[1];
            char cWire = one.Where(c => c != fWire).First();
            char gWire = five.Where(c => c != aWire && c != bWire && c != dWire && c != fWire).First();
            char eWire = signals.Where(s => s.Length == 7).First()
                                .Where(c => c != aWire && c != bWire && c != cWire && c != dWire && c != fWire && c != gWire).First();

            var ttable = new Dictionary<char, char>();
            ttable[aWire] = 'a';
            ttable[bWire] = 'b';
            ttable[cWire] = 'c';
            ttable[dWire] = 'd';
            ttable[eWire] = 'e';
            ttable[fWire] = 'f';
            ttable[gWire] = 'g';

            int sum = 0;
            int m = 1000;
            foreach (var output in outputs)
            {
                var translated = new string(output.Select(c => ttable[c]).ToArray());
                Signal s = new Signal(translated);
                sum += s.Value * m;
                m /= 10;
            }

            return sum;
        }

        void PartTwo()
        {            
            int total = 0;
            foreach (var line in File.ReadAllLines("inputs/day08.txt"))
            {
                var pieces = line.Split('|');
                var signal = pieces[0].Trim().Split(' ');
                var output = pieces[1].Trim().Split(' ');

                total += ParseSiginals(signal, output);
            }

            Console.WriteLine($"P2: {total}");
        }

        public void Solve()
        {
            PartOne();
            PartTwo();
        }
    }
}
