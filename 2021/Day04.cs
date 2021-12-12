using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day04 : IDay
    {
        public Day04() { }

        bool isBingo(int[] board, int width)
        {
            // check rows
            foreach (var row in board.Chunk(width))
            {
                if (row.Sum() == -width)
                    return true;                
            }

            // check cols
            for (int c = 0; c < width; c++)
            {
                int sum = 0;
                for (int r = 0; r < width; r++)               
                    sum += board[r * width + c];
                if (sum == -width)
                    return true;
            }

            return false;
        }

        public void Solve()
        {
            int width = 5;
            var lines = File.ReadAllLines("inputs/day04.txt");
            var numbers = lines[0].Split(',').Select(n => int.Parse(n));

            var boards = new List<int[]>();
            foreach (var board in lines.Skip(2).Chunk(width + 1))
            {
                var b = string.Join(" ", board.Take(width)).Trim().Replace("  ", " ")
                              .Split(' ')
                              .Select(n => int.Parse(n)).ToArray();
                boards.Add(b);                
            }

            // Track which boards have already had bingos, just in case the number sequence
            // is such that a board could get a second bingo before another had its first
            int bingoCounts = 0;
            bool[] bingoed = new bool[boards.Count];
            foreach (int n in numbers)
            {
                for (int b = 0; b < boards.Count; b++)
                {
                    for (int x = 0; x < boards[b].Length; x++)
                    {
                        if (boards[b][x] == n)
                            boards[b][x] = -1;

                        if (!isBingo(boards[b], width) || bingoed[b])                        
                            continue;
                       
                        bingoed[b] = true;
                        if (++bingoCounts == 1)
                            Console.WriteLine($"P1: {boards[b].Where(sq => sq != -1).Sum() * n}");
                        else if (bingoCounts == boards.Count)
                        {
                            Console.WriteLine($"P2: {boards[b].Where(sq => sq != -1).Sum() * n}");
                            return;
                        }
                    }
                }
            }
        }
    }
}
