using System;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day5
    {
        public Day5() { }

        private int binarySearch(int min, int max, string word, int pos)
        {
            if (max == min)
                return min;

            int half = (max - min) / 2;

            if (word[pos] == 'B' || word[pos] == 'R')
                return binarySearch(min + half + 1, max, word, pos + 1);
            else
                return binarySearch(min, max - half - 1, word, pos + 1);
        }

        private int calcSeatID(string word)
        {
            int row = binarySearch(0, 127, word, 0);
            int col = binarySearch(0, 7, word, 7);

            return row * 8 + col;
        }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day5.txt");
            var seats = tr.ReadToEnd().Split('\n').Select(line => calcSeatID(line)).OrderBy(s => s);

            Console.WriteLine($"P1: {seats.Last()}");

            var mySeat = seats.Zip(seats.Skip(1), (a, b) => b - a > 1 ? a + 1 : 0).Max();
            Console.WriteLine($"P2: {mySeat}");
        }
    }
}
