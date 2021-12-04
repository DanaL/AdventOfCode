using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day04 : IDay
    {
        public Day04() { }

        public void Solve()
        {
            var lines = File.ReadAllLines("inputs/day04.txt");
            var numbers = lines[0].Split(',').Select(n => int.Parse(n));

            var boards = new List<int[]>();
            foreach (var board in lines.Skip(2).Chunk(6))
            {
                var b = string.Join(" ", board.Take(5)).Trim().Replace("  ", " ")
                              .Split(' ')
                              .Select(n => int.Parse(n)).ToArray();
                boards.Add(b);
            }

            foreach (var board in boards)
            {
                Console.WriteLine($"{board[0]}, {board[1]}, {board[2]}");
            }
        }
    }
}
