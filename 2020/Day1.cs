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
            for (int j = 0; j < _expenses.Count; j++)
            {
                int _lookingFor = 2020 - _expenses[j];

                for (int k = j + 1; k < _expenses.Count; k++)
                {
                    if (_expenses[k] == _lookingFor)
                    {
                        int _res = _expenses[j] * _expenses[k];
                        Console.WriteLine($"P1: {_res}");
                        goto Done;
                    }
                }
            }

        Done:
            ;
        }

        public void SolvePart2()
        {
            // sample/test input
            //_expenses = new List<int>() { 1721, 979, 366, 299, 675, 1456 };

            // Time to brute force this shit! Puzzle input is 200 lines, so all possible combos of 3
            // numbers is less than 8 million sets of three I think??
            List<int[]> _combos = new List<int[]>();

            for (int a = 0; a < _expenses.Count - 2; a++)
            {
                for (int b = a + 1; b < _expenses.Count - 1; b++)
                {
                    for (int c = b + 1; c < _expenses.Count; c++)
                    {
                        int _sum = _expenses[a] + _expenses[b] + _expenses[c];
                        if (_sum == 2020)
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
