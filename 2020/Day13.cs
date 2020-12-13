using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day13
    {
        public Day13() { }

        private void part1()
        {
            TextReader tr = new StreamReader("inputs/day13.txt");
            var input = tr.ReadToEnd().Split('\n');
            int timestamp = int.Parse(input[0]);
            int[] buses = input[1].Split(',').Where(a => a != "x").Select(a => int.Parse(a)).ToArray();

            // Naive brute force. There's gotta be something to all the bus #s being prime
            int x = 0;
            while (true)
            {
                ++x;
                var result = buses.Where(a => (timestamp + x) % a == 0).ToArray();
                if (result.Length == 1)
                {
                    Console.WriteLine($"P1: {x * result[0]}");
                    break;
                }
            }
        }

        private void part2()
        {
            TextReader tr = new StreamReader("inputs/day13.txt");
            string txt = tr.ReadToEnd();
            List<(long, long)> buses = new List<(long, long)>();
            int offset = 0;
            foreach (var busID in txt.Substring(txt.IndexOf('\n') + 1).Split(','))
            {
                if (busID != "x")
                    buses.Add((long.Parse(busID), offset));
                ++offset;
            }

            var before = DateTime.Now;
            // Cobbled together from skimming hints on reddit. Honestly, the articles
            // on the Chinese Remainder Theorem whooshed past me and I should go back
            // and properly try to crack them. Anyhow, here I am building toward the
            // correct timestamp by looking for parts. Finding a number divisible by
            // a pair of the bus IDs by adding their least common multiple to the
            // timestamp until I find a number evenly divisible by t + the offset
            // Multiple the LCM by the next bus ID and keep going            
            long timestamp = 0;
            long lcm = buses[0].Item1;

            foreach (var bus in buses.Skip(1))
            {
                while ((timestamp + bus.Item2) % bus.Item1 != 0)
                    timestamp += lcm;

                lcm *= bus.Item1;
            }
            var delta = DateTime.Now.Subtract(before);

            Console.WriteLine($"P2: {timestamp}");
            Console.WriteLine($"time needed: {delta.TotalMilliseconds} ms");
        }

        public void Solve()
        {
            part1();
            part2();
        }
    }
}
