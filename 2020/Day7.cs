using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    class Bag
    {
        public string Name { get; set; }
        public Dictionary<string, int> Contains { get; set; }

        public Bag()
        {
            this.Contains = new Dictionary<string, int>();
        }
    }

    public class Day7
    {
        public Day7() { }

        private Bag parse(string line)
        {
            var pieces = line.Split(" ");

            Bag bag = new Bag() { Name = $"{pieces[0]} {pieces[1]}" };

            if (pieces.Length > 7)
            {
                for (int j = 4; j < pieces.Length; j += 4)
                {
                    var count = int.Parse(pieces[j]);
                    bag.Contains.Add($"{pieces[j + 1]} {pieces[j + 2]}", count);
                }
            }

            return bag;
        }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day7.txt");

            var txt = tr.ReadToEnd().Trim().Split("\n");

            var bags = txt.Select(b => parse(b));
            Console.WriteLine($"{bags.Count()}");
            
        }
    }
}
