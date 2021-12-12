using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    enum Direction { Forward, Down, Up }

    public class Day02 : IDay
    {
        public Day02() { }

        private Direction toDir(string d)
        {
            switch (d)
            {
                case "forward":
                    return Direction.Forward;
                case "down":
                    return Direction.Down;
                default:
                    return Direction.Up;
            }
        }

        private List<(Direction, int)> parseFile()
        {
            return File.ReadAllLines("inputs/day02.txt")
                .Select(s => s.Split(' '))
                .Select(i => (toDir(i[0]), int.Parse(i[1])))
                .ToList();
        }
        
        public void Solve()
        {
            int fwd = 0, depthPt1 = 0, depthPt2 = 0, aim = 0;
            foreach (var line in parseFile())
            {
                int x = line.Item2;
                switch (line.Item1)
                {
                    case Direction.Forward:
                        fwd += x;
                        depthPt2 += aim * x;
                        break;
                    case Direction.Up:
                        depthPt1 -= x;
                        aim -= x;
                        break;
                    case Direction.Down:
                        depthPt1 += x;
                        aim += x;
                        break;
                }
            }

            Console.WriteLine($"P1: {fwd * depthPt1}");
            Console.WriteLine($"P2: {fwd * depthPt2}");
        }
    }
}