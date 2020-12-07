using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    class Bag
    {
        public string Name { get; set; }
        public Dictionary<string, int> Contents { get; set; }

        public Bag()
        {
            this.Contents = new Dictionary<string, int>();
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
                    bag.Contents.Add($"{pieces[j + 1]} {pieces[j + 2]}", count);
                }
            }

            return bag;
        }

        // Recursively search through our collection of bags to see if one of the nested
        // bags contains a shiny gold bag
        private bool simpleSearch(string start, string key, Dictionary<string, Bag> bags)
        {
            var bag = bags[start];

            foreach (string name in bag.Contents.Keys)
            {
                if (name == key)
                    return true;

                var res = simpleSearch(name, key, bags);
                if (res)
                    return true;
            }

            return false;
        }

        public void Solve()
        {
            using TextReader tr = new StreamReader("inputs/day7.txt");

            var txt = tr.ReadToEnd().Trim().Split("\n");

            var bags = txt.Select(a => parse(a))
                          .GroupBy(b => b.Name, b => b)
                          .ToDictionary(c => c.Key, c => c.ElementAt(0));

            Console.WriteLine($"P1: {bags.Select(b => b.Key).Where(k => simpleSearch(k, "shiny gold", bags)).Count()}");            
        }
    }
}
