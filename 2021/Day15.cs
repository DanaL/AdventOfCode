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

        public void Solve()
        {
            GridInfo grid = new GridInfo();

            Console.WriteLine($"P1: {dijkstra(grid.Grid, 0, grid.Width, grid.Grid.Length - 1)}");
        }
    }
}
