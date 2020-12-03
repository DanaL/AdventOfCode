using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day1
    {
        private List<int> _expenses;

        public Day1()
        {
            using TextReader _tr = new StreamReader("inputs/day1.txt");

            this._expenses = (from _line in _tr.ReadToEnd().Split("\n")
                              select int.Parse(_line)).ToList();
        }

        public void SolvePart1()
        {
            // Keep track of numbers we've seen in the input so far so that we
            // don't actually have to do two loops like my original naive
            // implementation. And then I can use a break statement instead of
            // a goto
            HashSet<int> _seen = new HashSet<int>();
            for (int j = 0; j < _expenses.Count; j++)
            {
                _seen.Add(_expenses[j]);
                int _lookingFor = 2020 - _expenses[j];

                if (_seen.Contains(_lookingFor))
                {
                    Console.WriteLine($"P1: {_expenses[j] * _lookingFor}");
                    return;
                }
            }
        }

        public void SolvePart2()
        {
            // sample/test input
            //_expenses = new List<int>() { 1721, 979, 366, 299, 675, 1456 };

            // Similar to part one, use a hashset to track values we've seen so
            // that we only need one nested loop O(n^2) instead of O(n^3)
            HashSet<int> _seen = new HashSet<int>();
            for (int a = 0; a < _expenses.Count - 1; a++)
            {
                _seen.Add(_expenses[a]);
                for (int b = a + 1; b < _expenses.Count; b++)
                {
                    _seen.Add(_expenses[b]);

                    int _lookingFor = 2020 - _expenses[a] - _expenses[b];
                    if (_seen.Contains(_lookingFor))
                    {
                        int _res = _expenses[a] * _expenses[b] * _lookingFor;
                        Console.WriteLine($"P2: {_res}");
                        return;
                    }
                }
            }
        }
    }
}
