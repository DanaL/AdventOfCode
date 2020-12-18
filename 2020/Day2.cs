using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{    
    public class Day2 : IDay
    {        
        public void Solve()
        {
            int numValidPt1 = 0;
            int numValidPt2 = 0;

            using TextReader tr = new StreamReader("inputs/day2.txt");
        
            foreach (string line in tr.ReadToEnd().Split('\n'))
            {
                var matches = Regex.Match(line, @"(?<p0>\d+)-(?<p1>\d+) (?<p2>[a-z]): (?<p3>[a-z]+)");
                int min = int.Parse(matches.Groups["p0"].Value);
                int max = int.Parse(matches.Groups["p1"].Value);
                char key = matches.Groups["p2"].Value[0];
                string pw = matches.Groups["p3"].Value;

                int count = pw.Count(ch => ch == key);
                if (count >= min && count <= max)
                    ++numValidPt1;

                // Check the characters in position X to see if exactly one matches the key for the rule.
                // (Note that the password rules use index 1, not index 0)
                // This may well be the first time I've used C#'s XOR operator!!
                if (pw[min - 1] == key ^ pw[max - 1] == key)
                    ++numValidPt2;
            }

            Console.WriteLine($"P1: {numValidPt1}");
            Console.WriteLine($"P2: {numValidPt2}");
        }
    }
}
