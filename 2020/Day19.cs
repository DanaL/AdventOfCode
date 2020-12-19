using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace _2020
{
    public class Day19 : IDay 
    {
        List<string> _messages;
        Dictionary<string, string[]> _rules;

        public Day19() { }

        private void parseInput()
        {
            _messages = new List<string>();
            _rules = new Dictionary<string, string[]>();

            using TextReader tr = new StreamReader("inputs/day19.txt");            
            var lines = tr.ReadToEnd().Split('\n');
            foreach (var line in lines.Where(l => l.Trim() != ""))
            {
                var sci = line.IndexOf(':');
                if (sci > 0 && line.IndexOf('"') > 0)
                {
                    var ruleNum = line.Substring(0, sci);
                    var subrule = new string[] { line.Replace("\"", "").Substring(sci + 1).Trim() };
                    _rules.Add(ruleNum, subrule);
                }
                else if (sci > 0)
                {
                    var ruleNum = line.Substring(0, sci);
                    var subrules = line.Substring(sci + 1).Split('|').Select(l => l.Trim()).ToArray();
                    _rules.Add(ruleNum, subrules);
                }
                else
                {
                    _messages.Add(line);
                }
            }
        }

        public void Solve()
        {
            parseInput();

        }
    }
}
