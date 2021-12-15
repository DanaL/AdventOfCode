using System;
using System.IO;
using System.Linq;

using AoC;

namespace _2021
{
    class GridInfo
    {
        public int[] Grid { get; }
        public int Width { get; }

        public GridInfo()
        {
            var lines = File.ReadAllLines("inputs/day15.txt");
            Width = lines[0].Length;

            Grid = string.Join("", lines).Replace("\n", "")
                        .Select(c => c - '0').ToArray();
        }
    }

    public class Day15 : IDay
    {        
        // Pretty much straight out of the Algorithm Design Manual by Skiena
        int dijkstra(int[] graph, int start, int width, int target)
        {
            bool[] visited = new bool[graph.Length];
            int[] distances = new int[graph.Length];

            distances[start] = 0;
            for (int j = 1; j < graph.Length; j++)
                distances[j] = int.MaxValue;

            int v = start;
            while (!visited[v])
            {
                visited[v] = true;
                foreach (int x in graph.CardinalTo(v, width))
                {
                    int weight = graph[x];
                    if (distances[x] > distances[v] + weight)                    
                        distances[x] = distances[v] + weight;                    
                }

                int dist = int.MaxValue;
                for (int j = 1;j < graph.Length; j++)
                {
                    if (!visited[j] && dist > distances[j])
                    {
                        dist = distances[j];
                        v = j;
                    }
                }
            }

            return distances[target];
        }

        int[] DangerUp(int[] prev)
        {
            int[] next = new int[prev.Length];
            for (int i = 0; i < prev.Length; i++)            
                next[i] = prev[i] < 9 ? prev[i] + 1 : 1;
            return next;
        }

        void CopyInto(int[] desc, int[] src, int destStartR, int destStartC, int destWidth, int srcStartR, int srcStartC, int srcWidth, int size)
        {
            for (int r = srcStartR; r < srcStartR + size; r++)
            {
                for (int srcC = srcStartC, destC = 0; srcC < srcStartC + size; srcC++, destC++)
                {
                    var di = (destStartR + r) * destWidth + destStartC + destC;
                    desc[di] = src[r * srcWidth + srcC];
                }
            }
        }

        int[] BuildPart2Grid(int[] initialGrid, int initialWidth)
        {
            int destWidth = initialWidth * 5;
            int[] part2 = new int[initialGrid.Length * 25];
            CopyInto(part2, initialGrid, 0, 0, destWidth, 0, 0, initialWidth, initialWidth);

            int[] next = new int[initialGrid.Length];
            CopyInto(next, initialGrid, 0, 0,initialWidth, 0, 0, initialWidth, initialWidth);
            for (int c = initialWidth; c < destWidth; c += initialWidth)
            {
                next = DangerUp(next);
                CopyInto(part2, next, 0, c, destWidth, 0, 0, initialWidth, initialWidth);
            }

            for (int c = 0; c < destWidth; c+= initialWidth)
            {
                CopyInto(next, part2, 0, 0, initialWidth, 0, c, initialWidth * 5, initialWidth);
                
                for (int r = initialWidth; r < destWidth; r += initialWidth)
                {
                    next = DangerUp(next);
                    CopyInto(part2, next, r, c, destWidth, 0, 0, initialWidth, initialWidth);
                }                
            }

            return part2;
        }

        public void Solve()
        {
            GridInfo grid = new GridInfo();
            Console.WriteLine($"P1: {dijkstra(grid.Grid, 0, grid.Width, grid.Grid.Length - 1)}");

            var part2Grid = BuildPart2Grid(grid.Grid, grid.Width);
            Console.WriteLine($"p2: {dijkstra(part2Grid, 0, grid.Width * 5, part2Grid.Length - 1)}");
        }
    }
}
