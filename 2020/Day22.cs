using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day22 : IDay
    {
        public Day22() { }

        private (Queue<int>, Queue<int>) play(Queue<int> deck1, Queue<int> deck2, bool recursiveRules)
        {            
            var previousRoundsP1 = new HashSet<string>();
            var previousRoundsP2 = new HashSet<string>();

            while (deck1.Count > 0 && deck2.Count > 0)
            {
                // Prevent an endless game
                if (recursiveRules)
                {
                    var s1 = string.Join('+', deck1.ToArray().Select(c => c.ToString()));
                    var s2 = string.Join('+', deck2.ToArray().Select(c => c.ToString()));
                    if (previousRoundsP1.Contains(s1) || previousRoundsP2.Contains(s2))
                        return (deck1, new Queue<int>()); // Infinite loop so Player 1 automatically wins
                    else
                    {
                        previousRoundsP1.Add(s1);
                        previousRoundsP2.Add(s2);
                    }
                }
                
                var card1 = deck1.Dequeue();
                var card2 = deck2.Dequeue();
                int winner;

                if (recursiveRules && deck1.Count >= card1 && deck2.Count >= card2)
                {
                    var sub1 = new Queue<int>(deck1.ToArray().Take(card1));
                    var sub2 = new Queue<int>(deck2.ToArray().Take(card2));
                    var result = play(sub1, sub2, recursiveRules);

                    winner = result.Item1.Count > result.Item2.Count ? 1 : 2;                    
                }
                else
                {
                    winner = card1 > card2 ? 1 : 2;                   
                }

                if (winner == 1)
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

            return (deck1, deck2);
        }

        public void Solve()
        {
            var cards = File.ReadAllText("inputs/day22.txt").Split("\n\n");
            var deck1 = cards[0].Split('\n').Skip(1).Select(i => int.Parse(i)).ToList();
            var deck2 = cards[1].Split('\n').Skip(1).Select(i => int.Parse(i)).ToList();

            var result = play(new Queue<int>(deck1), new Queue<int>(deck2), false);
            var winner = result.Item1.Count > 0 ? result.Item1.ToArray() : result.Item2.ToArray();
            Console.WriteLine($"P1: {winner.Zip(Enumerable.Range(1, winner.Length).Reverse(), (a, b) => a * b).Sum()}");

            var sw = new Stopwatch();
            sw.Start();
            result = play(new Queue<int>(deck1), new Queue<int>(deck2), true);
            sw.Stop();
            winner = result.Item1.Count > 0 ? result.Item1.ToArray() : result.Item2.ToArray();
            Console.WriteLine($"P2: {winner.Zip(Enumerable.Range(1, winner.Length).Reverse(), (a, b) => a * b).Sum()}");
            var ts = sw.Elapsed;
            Console.WriteLine($"{ts.TotalMilliseconds} ms elapsed.");
        }
    }
}
