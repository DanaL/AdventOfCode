using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day4
    {
        private List<Regex> regexesPt1;
        private Dictionary<string, Regex> regexesPt2;

        public Day4()
        {
            this.regexesPt1 = new List<Regex>() { new Regex(@"byr:(?<p>\d+)"), new Regex(@"iyr:(?<p>\d+)"),
                new Regex(@"eyr:(?<p>\d+)"), new Regex(@"hgt:(?<p>\d+(cm|in)*)"), new Regex(@"hcl:(?<p>#*[a-z0-9]+)"),
                new Regex(@"ecl:(?<p>#*[a-z0-9]+)"), new Regex(@"pid:(?<p>#*[a-z0-9]+)"), new Regex(@"cid:(?<p>\d+)")
            };

            this.regexesPt2 = new Dictionary<string, Regex>() { ["byr"] = new Regex(@"byr:(?<p>\d{4})"), ["iyr"] = new Regex(@"iyr:(?<p>\d{4})"),
                ["eyr"] = new Regex(@"eyr:(?<p>\d{4})"), ["hgt"] = new Regex(@"hgt:(?<p>\d+(cm|in))"), ["hcl"] = new Regex(@"hcl:(?<p>#[a-f0-9]{6})"),
                ["ecl"] = new Regex(@"ecl:(?<p>amb|blu|brn|gry|grn|hzl|oth)"), ["pid"] = new Regex(@"pid:(?<p>\d{9})(?!\d)")
            };
        }

        private bool isValidPt1(string passport)
        {
            int count = 0;
            foreach (var re in this.regexesPt1)
            {
                var m = re.Match(passport);                
                if (m.Success) ++count;                
            }

            return count == this.regexesPt1.Count
                || (count == this.regexesPt1.Count - 1 && !this.regexesPt1[7].Match(passport).Success);
        }

        private bool yearMatch(Match m, int floor, int ceiling)
        {
            if (!m.Success)
                return false;

            int yr = int.Parse(m.Groups[1].Value);
            if (yr < floor || yr > ceiling)
                return false;

            return true;
        }

        private bool isValidPt2(string passport)
        {
            if (!yearMatch(this.regexesPt2["byr"].Match(passport), 1920, 2002))
                return false;
            if (!yearMatch(this.regexesPt2["iyr"].Match(passport), 2010, 2020))
                return false;
            if (!yearMatch(this.regexesPt2["eyr"].Match(passport), 2020, 2030))
                return false;
            if (!this.regexesPt2["hcl"].Match(passport).Success)
                return false;
            if (!this.regexesPt2["ecl"].Match(passport).Success)
                return false;
            if (!this.regexesPt2["pid"].Match(passport).Success)
                return false;
            
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
