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
        Dictionary<string, string[]> _flattenedRules;

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

        private bool complete(string msg)
        {
            return msg.ToCharArray().Where(c => c != 'a' && c != 'b' && c != ' ').Count() == 0;
        }

        private List<string> produceString(string production)
        {
            List<string> newPhrases = new List<string>();
            string[] pieces = production.Split(' ');
            for (int j = 0; j < pieces.Length; j++)
            {
                if (_rules.ContainsKey(pieces[j]))
                {
                    foreach (var rule in _rules[pieces[j]])
                    {
                        pieces[j] = rule;
                        newPhrases.Add(string.Join(' ', pieces).Trim());
                    }
                    break; // Only doing one bit at a time to avoid creating redundant phrases
                }
            }

            return newPhrases;
        }

        public void Solve()
        {
            parseInput();
            _flattenedRules = new Dictionary<string, string[]>();

            List<string> productions = new List<string>() { _rules["0"][0] } ;
            bool incomplete = true;

            while (incomplete)
            {
                incomplete = false;
                List<string> next = new List<string>();
                foreach (var pr in productions)
                {
                    foreach (var result in produceString(pr))
                    {
                        next.Add(result);
                        if (!complete(result))
                            incomplete = true;
                    }
                }
                productions = next;                
            }

            HashSet<string> possibleMessages = new HashSet<string>();
            foreach (string msg in productions)
                possibleMessages.Add(msg.Replace(" ", ""));

            foreach (string m in _messages)
            {
                if (possibleMessages.Contains(m))
                    Console.WriteLine(m);
            }
                
            
            Console.WriteLine($"P1: {_messages.Where(m => possibleMessages.Contains(m)).Count()}");
        }
    }
}
