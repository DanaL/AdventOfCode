using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day19 : IDay 
    {
        List<string> _messages;
        Dictionary<string, string> _rules;
        
        public Day19() { }

        private void parseInput()
        {
            _messages = new List<string>();
            _rules = new Dictionary<string, string>();

            using TextReader tr = new StreamReader("inputs/day19.txt");            
            var lines = tr.ReadToEnd().Split('\n');
            foreach (var line in lines.Where(l => l.Trim() != ""))
            {
                var sci = line.IndexOf(':');
                if (sci > 0 && line.IndexOf('"') > 0)
                {
                    var ruleNum = line.Substring(0, sci);
                    var subrule = line.Replace("\"", "").Substring(sci + 1).Trim();
                    _rules.Add(ruleNum, subrule);
                }
                else if (sci > 0)
                {
                    var ruleNum = line.Substring(0, sci);
                    var subrule = line.Substring(sci + 1).Trim();
                    _rules.Add(ruleNum, subrule);
                }
                else
                {
                    _messages.Add(line);
                }
            }
        }

        // I bet this doesn't need to be this fucking ugly...
        private string genRegExPattern(string start)
        {
            StringBuilder sb = new StringBuilder();

            var str = "";
            for (int j = 0; j < start.Length; j++)
            {
                char c = start[j];
                if (c == '(' || c == 'a' || c == 'b' || c == '|' || c == '+')
                    sb.Append(c);
                else if (c >= '0' && c <= '9')
                    str += c;
                else if (c == ' ' || c == ')')
                {
                    if (str != "")
                    {
                        sb.Append('(');
                        sb.Append(_rules[str]);
                        sb.Append(')');
                    }
                    sb.Append(c);
                    str = "";
                }
            }

            if (str != "")
            {
                sb.Append('(');
                sb.Append(_rules[str]);
                sb.Append(')');
            }

            string next = sb.ToString();
            if (Regex.Match(next, @"\d+").Success)
                return genRegExPattern(next);
            else
                return next;
        }

        public void Solve()
        {
            parseInput();

            var pattern = "^" + genRegExPattern(_rules["0"]).Replace(" ", "") + "$";
            int p1 = _messages.Where(m => Regex.Match(m, pattern, RegexOptions.ExplicitCapture).Success)
                                 .Count();
            Console.WriteLine($"P1: {p1}");

            //_rules["8"] = "42 | 42 8";
            //_rules["11"] = "42 31 | 42 11 31";
            // For P2, just keep growing rules 8 and 11 until we stop matching additional messages
            string expand8 = "42";
            string expand11 = "42 31";
            int p2 = 0, prevCount = p1;
            while (p2 != prevCount)
            {
                // Expand the recursive rules another iteration
                expand8 += " 42";
                _rules["8"] += " | " + expand8;
                expand11 = "42 " + expand11 + " 31";
                _rules["11"] += " | " + expand11;
                
                pattern = "^" + genRegExPattern(_rules["0"]).Replace(" ", "") + "$";
                int count = _messages.Where(m => Regex.Match(m, pattern, RegexOptions.ExplicitCapture).Success)
                                     .Count();
                prevCount = p2;
                p2 = count;                
            }
            Console.WriteLine($"P2: {p2}");
        }
    }
}
