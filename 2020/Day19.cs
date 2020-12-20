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

        private string genRegExPattern(string start)
        {
            StringBuilder sb = new StringBuilder();

            var str = "";
            for (int j = 0; j < start.Length; j++)
            {
                char c = start[j];
                if (c == '(' || c == 'a' || c == 'b' || c == '|')
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

            int total = 0;
            var pattern = "^" + genRegExPattern(_rules["0"]).Replace(" ", "") + "$";
            foreach (string msg in _messages)
            {
                var m = Regex.Match(msg, pattern, RegexOptions.ExplicitCapture);
                if (m.Success)
                {
                    //Console.WriteLine(msg);
                    total += 1;
                }
            }
            
            Console.WriteLine($"P1: {total}");
        }
    }
}
