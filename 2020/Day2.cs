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
        public string Key { get; set; }
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
                    Key = _matches.Groups["p2"].Value,
                    Text = _matches.Groups["p3"].Value
                };
                _pwds.Add(_pw);
            }
        }

        public void SolvePart1()
        {            
            foreach (var _pw in _pwds)
            {
                Console.WriteLine($"Looking for '{_pw.Key}' in {_pw.Text} {_pw.Min} to {_pw.Max} times.");
            }
        }
    }
}
