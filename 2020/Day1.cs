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
                        Console.WriteLine($"Answer: {_res}");
                        goto Done;
                    }
                }
            }

        Done:
            ;
        }        
    }
}
