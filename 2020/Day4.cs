using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day4 : IDay
    {
        private Dictionary<string, Regex> regexesPt2;

        public Day4()
        {            
            this.regexesPt2 = new Dictionary<string, Regex>() { ["byr"] = new Regex(@"byr:(19[2-9][0-9]|200[0-2])"),
                ["iyr"] = new Regex(@"iyr:201[0-9]|2020"), ["eyr"] = new Regex(@"eyr:(202[0-9]|2030)"),
                ["hgt"] = new Regex(@"hgt:(?<p>\d+(cm|in))"), ["hcl"] = new Regex(@"hcl:#[a-f0-9]{6}"),
                ["ecl"] = new Regex(@"ecl:(amb|blu|brn|gry|grn|hzl|oth)"), ["pid"] = new Regex(@"pid:(\d{9})(?!\d)")
            };
        }

        private bool isValidPt1(string passport)
        {
            var pieces = new HashSet<string>(passport.Split().Select(p => p.Split(':')[0]));
            
            return pieces.Count == 8 || (pieces.Count == 7 && !pieces.Contains("cid"));            
        }

        private bool isValidPt2(string passport)
        {
            foreach (var k in new string[] { "byr", "iyr", "eyr", "hcl", "ecl", "pid" })
            {
                if (!this.regexesPt2[k].Match(passport).Success)
                    return false;
            }

            // I could just do these checks in the regex too, but I think it would start to get
            // real ugly
            var m = this.regexesPt2["hgt"].Match(passport);
            if (!m.Success)
                return false;
            else
            {
                var val = int.Parse(m.Groups[2].Value[0..^2]);
                var unit = m.Groups[1].Value;
                if (unit == "in" && (val < 59 || val > 76))
                    return false;
                else if (unit == "cm" && (val < 150 || val > 193))
                    return false;
            }

            return true;
        }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day4.txt");

            string passport = tr.ReadLine();
            int numValidPt1 = 0;
            int numValidPt2 = 0;
            do
            {
                var line = tr.ReadLine();
                if (line.Length == 0)
                {
                    if (isValidPt1(passport))
                        ++numValidPt1;
                    if (isValidPt2(passport))
                        ++numValidPt2;                    
                    passport = "";
                }
                else
                {                    
                    passport += passport.Length == 0 ? line : " " + line;
                }
            } while (tr.Peek() != -1);

            Console.WriteLine($"P1: {numValidPt1}");
            Console.WriteLine($"P2: {numValidPt2}");
        }
    }
}
