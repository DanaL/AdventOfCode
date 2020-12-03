using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{    
    public class Day2
    {        
        public static void Solve()
        {
            int _numValidPt1 = 0;
            int _numValidPt2 = 0;

            using TextReader _tr = new StreamReader("inputs/day2.txt");
        
            foreach (string _line in _tr.ReadToEnd().Split('\n'))
            {
                var _matches = Regex.Match(_line, @"(?<p0>\d+)-(?<p1>\d+) (?<p2>[a-z]): (?<p3>[a-z]+)");
                int _min = int.Parse(_matches.Groups["p0"].Value);
                int _max = int.Parse(_matches.Groups["p1"].Value);
                char _key = _matches.Groups["p2"].Value[0];
                string _pw = _matches.Groups["p3"].Value;

                int _count = _pw.Count(ch => ch == _key);
                if (_count >= _min && _count <= _max)
                    ++_numValidPt1;

                // Check the characters in position X to see if exactly one matches the key for the rule.
                // (Note that the password rules use index 1, not index 0)
                // This may well be the first time I've used C#'s XOR operator!!
                if (_pw[_min - 1] == _key ^ _pw[_max - 1] == _key)
                    ++_numValidPt2;
            }

            Console.WriteLine($"P1: {_numValidPt1}");
            Console.WriteLine($"P2: {_numValidPt2}");
        }
    }
}
