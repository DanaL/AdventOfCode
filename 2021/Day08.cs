using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021
{
    public class Day08 : IDay
    {        
        public void Solve()
        {
            // This seems...trivial for a part 1? I guess it's just a warm-up to having to parse and 
            // actually decode the numbers in part 2?
            int sum = 0;
            foreach (var line in File.ReadAllLines("inputs/day08.txt"))
            {
                var pieces = line.Split('|');
                string signal = pieces[0].Trim();
                string output = pieces[1].Trim();
                sum += output.Split(' ')
                            .Where(w => w.Length == 2 || w.Length == 3 || w.Length == 4 || w.Length == 7)
                            .Count();
            }

            Console.WriteLine($"P1: {sum}");
        }
    }
}
