using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day07 : IDay
    {
        public Day07() { }

        List<int> fetchInput()
        {            
            return File.ReadAllText("inputs/day07.txt").Split(',')
                           .Select(int.Parse).ToList();
        }

        public int calcOptimalFuelUsage(List<int> startingPositions, Func<int, int> fuelCalc)
        {
            int maxDistance = startingPositions.Max();

            // Count up how many crabs start at each starting position
            var crabs = startingPositions
                                .GroupBy(x => x)
                                .Select(x => (x.Key, x.Count()));

            // Sum up the total amount of distances for crabs to travel to each
            // point. (Mulitple crabs may begin at the same starting points)
            var sumsOfDistances = new int[maxDistance + 1];
            foreach (var c in crabs)
            {
                for (int j = 0; j <= maxDistance; j++)
                    sumsOfDistances[j] += fuelCalc(Math.Abs(j - c.Item1)) * c.Item2;
            }
           
            return sumsOfDistances.Min();            
        }

        public void Solve()
        {
            Console.WriteLine($"P1: {calcOptimalFuelUsage(fetchInput(), d => d)}");
            Console.WriteLine($"P1: {calcOptimalFuelUsage(fetchInput(), d => d * (1 + d) / 2)}");
        }
    }
}
