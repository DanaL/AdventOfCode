using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day16
    {
        HashSet<int> _myTicket;
        List<HashSet<int>> _nearbyTickets;
        Dictionary<string, List<int>> _fields;

        public Day16()
        {
            bool otherTickets = false;
            string pattern = @"(?<cat>[a-z ]+): (?<a>\d+)-(?<b>\d+) or (?<c>\d+)-(?<d>\d+)";
            _nearbyTickets = new List<HashSet<int>>();
            _fields = new Dictionary<string, List<int>>();
            using TextReader tr = new StreamReader("inputs/day16.txt");

            while (tr.Peek() != -1)
            {                
                var line = tr.ReadLine().Trim();
                if (line == "")
                    continue;
                else if (line == "your ticket:")
                    _myTicket = new HashSet<int>(tr.ReadLine().Split(',').Select(n => int.Parse(n)));
                else if (line == "nearby tickets:")
                    otherTickets = true;
                else if (otherTickets)
                    _nearbyTickets.Add(new HashSet<int>(line.Split(',').Select(n => int.Parse(n))));
                else
                {
                    var matches = Regex.Match(line, pattern);
                    List<int> nums = new List<int>() { int.Parse(matches.Groups["a"].Value), int.Parse(matches.Groups["b"].Value),
                                int.Parse(matches.Groups["c"].Value), int.Parse(matches.Groups["d"].Value) };
                    _fields.Add(matches.Groups["cat"].Value, nums);
                }
            }
        }

        private bool checkField(int val, List<int> ranges)
        {
            return (val >= ranges[0] && val <= ranges[1]) || (val >= ranges[2] && val <= ranges[3]);
        }

        private List<int> invalidValues(HashSet<int> ticket)
        {
            List<int> invalid = new List<int>();
            foreach (var v in ticket)
            {
                bool valid = false;

                foreach (var field in _fields.Keys)
                {
                    if (checkField(v, _fields[field]))
                    {
                        valid = true;
                        break;
                    }
                }

                if (!valid)
                    invalid.Add(v);
            }

            return invalid;
        }

        public void Solve()
        {
            Console.WriteLine($"P1: {_nearbyTickets.SelectMany(t => invalidValues(t)).Sum()}");
        }
    }
}
