using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day07 : IDay
    {
        public Day07() { }

        List<int> fetchInput()
        {
            //string line = "16,1,2,0,4,2,7,1,2,14";
            //return line.Split(',').Select(int.Parse).ToList();

            return File.ReadAllText("inputs/day07.txt").Split(',')
                           .Select(int.Parse).ToList();
        }

        public void Solve()
        {
            var numbers = fetchInput();            
            var crabs = new Dictionary<int, int>();
            var fuelNeeded = new Dictionary<int, int[]>();
            int maxDistance = numbers.Max();

            foreach (var x in numbers)
            {
                if (crabs.ContainsKey(x))
                    crabs[x] += 1;
                else
                {
                    crabs[x] = 1;
                    fuelNeeded[x] = new int[maxDistance + 1];
                }                
            }

            // Sum up the total amount of distances for crabs to travel to each
            // point. (Mulitple crabs may begin at the same starting points)
            var sumsOfDistances = new int[maxDistance + 1];
            foreach (var d in crabs.Keys)
            {
                for (int j = 0; j <= maxDistance; j++)                
                    sumsOfDistances[j] += Math.Abs(j - d) * crabs[d];
            }

            Console.WriteLine($"P1: {sumsOfDistances.Min()}");
        }
    }
}
