using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day22 : IDay
    {
        public Day22() { }

        public void Solve()
        {
            var cards = File.ReadAllText("inputs/day22.txt").Split("\n\n");
            var deck1 = new Queue<int>(cards[0].Split('\n').Skip(1).Select(i => int.Parse(i)).ToList());
            var deck2 = new Queue<int>(cards[1].Split('\n').Skip(1).Select(i => int.Parse(i)).ToList());

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                var card1 = deck1.Dequeue();
                var card2 = deck2.Dequeue();

                if (card1 > card2)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }
            
            var winner = deck1.Count > 0 ? deck1.ToArray() : deck2.ToArray();
            Console.WriteLine($"P1: {winner.Zip(Enumerable.Range(1, winner.Length).Reverse(), (a, b) => a * b).Sum()}");

            
        }
    }
}
