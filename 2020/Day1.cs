using System;
using System.Collections.Generic;
using System.IO;

namespace _2020
{
    public class Day1
    {
        private List<int> _expenses;

        public Day1()
        {
            this.fetchInput();
        }

        private void fetchInput()
        {
            using TextReader _tr = new StreamReader("inputs/day1.txt");

            this._expenses = new List<int>();
            while (_tr.Peek() != -1)
            {
                var _line = _tr.ReadLine();
                _expenses.Add(int.Parse(_line));
            }
        }

        public void SolvePart1()
        {
            int _res = 0;

            // Keep track of numbers we've seen in the input so on each iteration
            // we can find out what number will give us 2020 and check the hashset
            // to see if it is in the list and save ourselves a bit of looping
            HashSet<int> _seen = new HashSet<int>();
            for (int j = 0; j < _expenses.Count; j++)
            {
                _seen.Add(_expenses[j]);
                int _lookingFor = 2020 - _expenses[j];

                if (_seen.Contains(_lookingFor))
                {
                     _res = _expenses[j] * _lookingFor;
                    break;
                }

                for (int k = j + 1; k < _expenses.Count; k++)
                {
                    _seen.Add(_expenses[k]);
                    if (_expenses[k] == _lookingFor)
                    {
                        _res = _expenses[j] * _expenses[k];
                        goto Done;
                    }
                }
            }

        Done:
            Console.WriteLine($"P1: {_res}");
        }

        public void SolvePart2()
        {
            // sample/test input
            //_expenses = new List<int>() { 1721, 979, 366, 299, 675, 1456 };

            // Time to brute force this shit! Puzzle input is 200 lines, so all possible combos of 3
            // numbers is less than 8 million sets of three I think??
            
            for (int a = 0; a < _expenses.Count - 2; a++)
            {
                for (int b = a + 1; b < _expenses.Count - 1; b++)
                {
                    for (int c = b + 1; c < _expenses.Count; c++)
                    {
                        if (_expenses[a] + _expenses[b] + _expenses[c] == 2020)
                        {
                            Console.WriteLine($"P2: {_expenses[a] * _expenses[b] * _expenses[c]}");
                            goto Done;
                        }                        
                    }
                }
            }

        Done:
            ;
        }
    }
}
