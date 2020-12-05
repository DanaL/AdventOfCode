using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    public class Path
    {
        public int Length { get; set; }
        public HashSet<char> KeysNeeded { get; set; }

        public Path() { }
    }

    public class Day18Solver
    {
        public Day18Solver() { }

        public void Solve()
        {
            string file = "/Users/dana/Development/AdventOfCode/2019/inputs/day18_test.txt";

            using TextReader tr = new StreamReader(file);
            List<char[]> grid = new List<char[]>();
            HashSet<char> keys = new HashSet<char>();
            while (tr.Peek() != -1)
            {
                var row = tr.ReadLine().ToCharArray();
                foreach (char ch in row.Where(ch => ch == '@' || (char.IsLetter(ch) && char.IsLower(ch))))
                    keys.Add(ch);                   
                grid.Add(row);                
            }

            //Console.WriteLine(keys.ToString());
            // So first, lets build a set of how far each key is from each other key, and the
            // doors required to each it.

            Console.WriteLine(grid[3][6]);
        }
    }
}
