using System;
using System.Collections.Generic;
using System.Linq;

using AoC;

namespace _2021
{
    class Day06 : IDay
    {
        string _input;

        public Day06()
        {
            _input = "4,1,1,4,1,1,1,1,1,1,1,1,3,4,1,1,1,3,1,3,1,1,1,1,1,1,1,1,1,3,1,3,1,1,1,5,1,2,1,1,5,3,4,2,1,1,4,1,1,5,1,1,5,5,1,1,5,2,1,4,1,2,1,4,5,4,1,1,1,1,3,1,1,1,4,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,1,2,1,1,1,1,1,1,1,2,4,4,1,1,3,1,3,2,4,3,1,1,1,1,1,2,1,1,1,1,2,5,1,1,1,1,2,1,1,1,1,1,1,1,2,1,1,4,1,5,1,3,1,1,1,1,1,5,1,1,1,3,1,2,1,2,1,3,4,5,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,3,1,1,3,1,1,4,1,1,1,1,1,2,1,1,1,1,3,2,1,1,1,4,2,1,1,1,4,1,1,2,3,1,4,1,5,1,1,1,2,1,5,3,3,3,1,5,3,1,1,1,1,1,1,1,1,4,5,3,1,1,5,1,1,1,4,1,1,5,1,2,3,4,2,1,5,2,1,2,5,1,1,1,1,4,1,2,1,1,1,2,5,1,1,5,1,1,1,3,2,4,1,3,1,1,2,1,5,1,3,4,4,2,2,1,1,1,1,5,1,5,2";            
        }

        // The dumb, brute-force solution I was certain would be absolutely inadequate for part 2
        void partOne()
        {
            List<long> fish = _input.Split(',').Select(long.Parse).ToList();

            for (long _ = 0; _ < 80; _++)
            {
                var nextFish = new List<long>();
                for (int f = 0; f < fish.Count; f++)
                {
                    if (fish[f] == 0)
                    {
                        nextFish.Add(6);
                        nextFish.Add(8);
                    }
                    else
                    {
                        nextFish.Add(fish[f] - 1);
                    }
                }
                fish = nextFish;
            }

            Console.WriteLine($"P1: {fish.Count}");
        }

        // But really, we just have to keep track of the # of fish in each day of their reproductive 
        // cycle. Each day we shift them all and then if there were any on day 0, add that number to
        // days 6 and 8. (So, say, if 3 fish were on day 0, their counter resets to 6 and we add 3
        // new fish with counter set to 8)
        void partTwo()
        {
            var fishState = new ulong[9];
            foreach (int x in _input.Split(',').Select(ulong.Parse))
                fishState[x] += 1;

            for (int _ = 0; _ < 256; _++)
            {
                var fish0 = fishState[0];
                for (int j = 0; j < 8; j++)
                    fishState[j] = fishState[j + 1];
                fishState[6] += fish0;
                fishState[8] = fish0;
            }

            // huh there isn't a Sum() extension method for collections of ulong I guess?
            ulong total = 0;
            foreach (var x in fishState)
                total += x;

            Console.WriteLine($"P2: {total}");
        }

        public void Solve()
        {
            partOne();
            partTwo();
        }
    }
}
