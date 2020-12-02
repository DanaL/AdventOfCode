using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    class PasswordDetails
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public char Key { get; set; }
        public string Text { get; set; }

        public PasswordDetails() { }
    }

    public class Day2
    {
        private List<PasswordDetails> _pwds;

        public Day2()
        {            
            using TextReader _tr = new StreamReader("inputs/day2.txt");

            _pwds = new List<PasswordDetails>();
            foreach (string _line in _tr.ReadToEnd().Split("\n"))
            {
                // 2-12 w: jwkfktkbthcwvfrkwgz is the sort of input we are expecting
                var _matches = Regex.Match(_line, @"(?<p0>\d+)-(?<p1>\d+) (?<p2>[a-z]): (?<p3>[a-z]+)");
                var _pw = new PasswordDetails()
                {
                    Min = int.Parse(_matches.Groups["p0"].Value),
                    Max = int.Parse(_matches.Groups["p1"].Value),
                    Key = _matches.Groups["p2"].Value[0],
                    Text = _matches.Groups["p3"].Value
                };
                _pwds.Add(_pw);
            }
        }

        public void SolvePart1()
        {
            int _numValid = 0;
            foreach (var _pw in _pwds)
            {
                var _letters = _pw.Text.ToCharArray()
                            .GroupBy(ch => ch)
                            .ToDictionary(g => g.Key, g => g.ToList());

                int _count = _letters.ContainsKey(_pw.Key) ? _letters[_pw.Key].Count : 0;
                if (_count >= _pw.Min && _count <= _pw.Max)
                    _numValid++;                
            }

            Console.WriteLine($"P1: {_numValid}");
        }
    }
}
