using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2021
{
    public class Day04 : IDay
    {
        public Day04() { }

        bool bingo(int[] board, int width)
        {
            // check rows
            foreach (var row in board.Chunk(width))
            {
                if (row.Sum() == 5)
                    return true;                
            }

            // check cols
            for (int r = 0; r < width; r++)
            {
                int sum = 0;
                for (int c = 0; c < width; c++)               
                    sum += board[r * width + c];
                if (sum == 5)
                    return true;
            }

            return false;
        }

        int calcPt1Score(int[] board, int[] marks, int n)
        {
            int sum = 0;
            for (int j = 0; j < board.Length; j++)
            {
                if (marks[j] == 0)
                    sum += board[j];
            }

            return sum * n;
        }

        public void Solve()
        {
            int width = 5;
            var lines = File.ReadAllLines("inputs/day04.txt");
            var numbers = lines[0].Split(',').Select(n => int.Parse(n));

            var marks = new List<int[]>();
            var boards = new List<int[]>();
            foreach (var board in lines.Skip(2).Chunk(width + 1))
            {
                var b = string.Join(" ", board.Take(width)).Trim().Replace("  ", " ")
                              .Split(' ')
                              .Select(n => int.Parse(n)).ToArray();
                boards.Add(b);
                marks.Add(new int[width * width]);
            }

            foreach (int n in numbers)
            {
                for (int b = 0; b < boards.Count; b++)
                {
                    for (int x = 0; x < boards[b].Length; x++)
                    {
                        if (boards[b][x] == n)
                            marks[b][x] = 1;
                        if (bingo(marks[b], width))
                        {
                            Console.WriteLine($"P1: {calcPt1Score(boards[b], marks[b], n)}");
                            return;
                        }
                    }
                }
            }
        }
    }
}
