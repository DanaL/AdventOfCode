using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    enum Direction
    {
        Forward,
        Down,
        Up
    }

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

        private void partTwo()
        {
            var lines = parseFile();

            int fwd = 0;
            int depth = 0;
            int aim = 0;
            foreach (var line in lines)
            {
                int x = line.Item2;
                switch (line.Item1)
                {
                    case Direction.Forward:
                        fwd += x;
                        depth += aim * x;
                        break;
                    case Direction.Up:
                        aim -= x;
                        break;
                    case Direction.Down:
                        aim += x;
                        break;
                }
            }
            
            Console.WriteLine($"P2: {fwd * depth}");
        }

        private void partOne()
        {
            var lines = parseFile();
            var fwd = lines.Where(i => i.Item1 == Direction.Forward).Select(i => i.Item2).Sum();
            var depth = lines.Where(i => i.Item1 == Direction.Down).Select(i => i.Item2).Sum();
            depth -= lines.Where(i => i.Item1 == Direction.Up).Select(i => i.Item2).Sum();

            Console.WriteLine($"P1: {fwd * depth}");
        }

        public void Solve()
        {
            partOne();
            partTwo();
        }
    }
}
