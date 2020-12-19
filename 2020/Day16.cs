using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020
{
    public class Day16 : IDay
    {
        List<int> _myTicket;
        List<List<int>> _nearbyTickets;
        Dictionary<string, List<int>> _fields;

        public Day16() { }
        
        private void parseInput()
        {
            bool otherTickets = false;
            string pattern = @"(?<cat>[a-z ]+): (?<a>\d+)-(?<b>\d+) or (?<c>\d+)-(?<d>\d+)";
            _nearbyTickets = new List<List<int>>();
            _fields = new Dictionary<string, List<int>>();
            using TextReader tr = new StreamReader("inputs/day16.txt");

            while (tr.Peek() != -1)
            {
                var line = tr.ReadLine().Trim();
                if (line == "")
                    continue;
                else if (line == "your ticket:")
                    _myTicket = new List<int>(tr.ReadLine().Split(',').Select(n => int.Parse(n)));
                else if (line == "nearby tickets:")
                    otherTickets = true;
                else if (otherTickets)
                    _nearbyTickets.Add(new List<int>(line.Split(',').Select(n => int.Parse(n))));
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

        private bool checkAllFields(int val)
        {
            foreach (var field in _fields.Keys)
            {
                if (checkField(val, _fields[field]))
                    return true;
            }

            return false;
        }

        private List<int> invalidValues(List<int> ticket)
        {
            return ticket.Where(v => !checkAllFields(v)).ToList();
        }

        private bool validTicket(List<int> ticket)
        {
            return ticket.Where(v => checkAllFields(v)).Count() == ticket.Count;
        }

        private List<string> findPossibleFields(int val)
        {
            return _fields.Keys.Where(k => checkField(val, _fields[k])).ToList();
        }

        private void removeOption(Dictionary<int, HashSet<string>> possibilities, int keeperIndex, string toRemove)
        {
            foreach (int k in possibilities.Keys.Where(n => n != keeperIndex))
                possibilities[k].Remove(toRemove);            
        }

        private void filterPossibilities(Dictionary<int, HashSet<string>> possibilities)
        {
            var total = possibilities.Values.Select(v => v.Count).Sum();

            while (true)
            {
                foreach (int k in possibilities.Keys)
                {
                    if (possibilities[k].Count == 1)
                        removeOption(possibilities, k, possibilities[k].ElementAt(0));
                }

                var newTotal = possibilities.Values.Select(v => v.Count).Sum();
                if (newTotal == total)
                {
                    Console.WriteLine("We've filtered as many possibilities as we can!");
                    break;
                }
                total = newTotal;
            }
        }

        public void Solve()
        {
            parseInput();

            // For Part One, we just need to sum up all of the invalid fields found across the
            // various tickets
            Console.WriteLine($"P1: {_nearbyTickets.SelectMany(t => invalidValues(t)).Sum()}");

            // For Part Two, we want to toss out any of those invalid tickets and then use the
            // nearby tickets to determine which field is which. (Order will be consistent across
            // tickets
            var validTickets = _nearbyTickets.Where(t => validTicket(t)).ToList();

            // First, find possibilities looking at tickets and "what can possibly fit" based on the
            // value in that column
            Dictionary<int, HashSet<string>> fieldPossibilities = new Dictionary<int, HashSet<string>>();
            for (int j = 0; j < _myTicket.Count; j++)
                fieldPossibilities.Add(j, new HashSet<string>(_fields.Keys));

            foreach (var ticket in validTickets)
            {
                for (int j = 0; j < _myTicket.Count; j++)
                {
                    fieldPossibilities[j].IntersectWith(findPossibleFields(ticket[j]));
                }
            }

            filterPossibilities(fieldPossibilities);

            // Luckily filtering down the possibilities left us with a unique set so we don't have
            // to try remaining combos to find which ones result in invalid tickets, which I figured
            // we were going to have to.
            ulong product = 1;
            foreach (var k in fieldPossibilities.Keys)
            {
                if (fieldPossibilities[k].ElementAt(0).StartsWith("departure"))
                    product *= (ulong)_myTicket[k];
            }

            Console.WriteLine($"P2: {product}");
        }
    }
}
