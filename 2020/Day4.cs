using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day4
    {
        private List<Regex> regexes;

        public Day4()
        {
            this.regexes = new List<Regex>() { new Regex(@"byr:(?<p>\d+)"), new Regex(@"iyr:(?<p>\d+)"),
                new Regex(@"eyr:(?<p>\d+)"), new Regex(@"hgt:(?<p>\d+(cm|in)*)"), new Regex(@"hcl:(?<p>#*[a-z0-9]+)"),
                new Regex(@"ecl:(?<p>#*[a-z0-9]+)"), new Regex(@"pid:(?<p>#*[a-z0-9]+)"), new Regex(@"cid:(?<p>\d+)")
            };
        }

        private bool isValid(string password)
        {
            int count = 0;
            foreach (var re in this.regexes)
            {
                var m = re.Match(password);                
                if (m.Success) ++count;                
            }

            return count == this.regexes.Count
                || (count == this.regexes.Count - 1 && !this.regexes[7].Match(password).Success);
        }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day4.txt");

            string passport = tr.ReadLine();
            int numValid = 0;
            do
            {
                var line = tr.ReadLine();
                if (line.Length == 0)
                {
                    // check passport
                    if (isValid(passport))
                        ++numValid;
                    passport = "";
                }
                else
                {                    
                    passport += passport.Length == 0 ? line : " " + line;
                }
            } while (tr.Peek() != -1);

            Console.WriteLine($"P1: {numValid}");
        }
    }
}
