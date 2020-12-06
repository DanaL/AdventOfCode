using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Priority_Queue;

namespace Day18
{
    public class Route
    {
        public int Total { get; set; }
        public char Curr { get; set; }
        public uint Keys { get; set; }
        public HashSet<char> Visited { get; set; }

        public Route()
        {
            this.Visited = new HashSet<char>();
        }
    }

    public class Edge
    {
        public int Length { get; set; }
        public uint KeysNeeded { get; set; }
        public char Start { get; set; }
        public char End { get; set; }

        public Edge() { }
    }

    public class FloodFillNode
    {
        public int Distance { get; set; }
        public uint Doors { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public FloodFillNode() { }

        public FloodFillNode(int d, int r, int c, uint doors)
        {
            this.Distance = d;
            this.Row = r;
            this.Col = c;
            this.Doors = doors;
        }
    }

    public class Day18Solver
    {        
        private (int, int)[] _dirs = { (-1, 0), (0, -1), (0, 1), (1, 0) };
        private Dictionary<char, uint> _bitmasks;

        public Day18Solver()
        {
            _bitmasks = new Dictionary<char, uint>();
            foreach (char c in "abcdefghijklmnopqrstuvwxyz")
            {
                _bitmasks.Add(c, this.letterToNum(c));
                _bitmasks.Add(char.ToUpper(c), this.letterToNum(c));
            }
        }

        private uint letterToNum(char c)
        {
            uint exp = (uint)Char.ToLower(c) - (uint)'a';
            
            return (uint)Math.Pow(2, exp);
        }

        // Find path from the given start node to all of the other keys
        private void floodfill(int start_r, int start_c, string[] map, List<Edge> edges)
        {
            char start = map[start_r][start_c];
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            Queue<FloodFillNode> nodes = new Queue<FloodFillNode>();
            nodes.Enqueue(new FloodFillNode(0, start_r, start_c, 0));

            while (nodes.Count() > 0)
            {
                var node = nodes.Dequeue();
                visited.Add((node.Row, node.Col));

                foreach (var d in this._dirs)
                {
                    int row = node.Row + d.Item1;
                    int col = node.Col + d.Item2;

                    if (visited.Contains((row, col)))
                        continue;

                    char ch = map[row][col];
                    if (ch == '#')
                        continue;

                    var new_node = new FloodFillNode(node.Distance + 1, row, col, node.Doors);
                    
                    if (ch == '.' || ch == '@')
                        nodes.Enqueue(new_node);
                    else if (ch >= 'A' && ch <= 'Z')
                    {
                        new_node.Doors = node.Doors | _bitmasks[ch];                        
                        nodes.Enqueue(new_node);
                    }
                    else if (ch >= 'a' && ch <= 'z')
                    {
                        var p = new Edge
                        {
                            Length = new_node.Distance,
                            Start = start,
                            End = ch,
                            KeysNeeded = new_node.Doors
                        };
                        edges.Add(p);

                        nodes.Enqueue(new_node);                        
                    }
                }
            }
        }

        private int shortestPath(List<Edge> edges)
        {
            // Should convert the list of paths into a dictionary? for easy
            // look up of where we can go
            SimplePriorityQueue<Edge> pq = new SimplePriorityQueue<Edge>();
            var keys = 0;
            foreach (Edge path in edges.Where(e => e.Start == '@' && (e.KeysNeeded == 0 || (e.KeysNeeded & keys) > 0)))
            {
                pq.Enqueue(path, path.Length);
            }

            while (pq.Count > 0)
            {
                var path = pq.Dequeue();
                Console.WriteLine($"Path from {path.Start} to {path.End} through doors {path.KeysNeeded} takes {path.Length} steps.");
            }

            return -1;
        }

        public void Solve()
        {
            string file = "/Users/dana/Development/AdventOfCode/2019/inputs/day18_test.txt";
            using TextReader tr = new StreamReader(file);

            // Scan through the map and find all the keys and their locations
            Dictionary<char, (int Row, int Col)> keys = new Dictionary<char, (int,  int)>();
            var lines = tr.ReadToEnd().Trim().Split('\n');
            for (int r = 0; r < lines.Length; r ++)
            {
                for (int c = 0; c < lines[0].Length; c++)
                {
                    if (lines[r][c] == '@' || (char.IsLetter(lines[r][c]) && char.IsLower(lines[r][c])))
                        keys.Add(lines[r][c], (Row: r, Col: c));
                }
            }

            // Breadthfirst search to calculate the distances from each key to each other key, including
            // which doors must be passed through=
            List<Edge> edges = new List<Edge>();
            foreach (var k in keys.Keys)
                this.floodfill(keys[k].Row, keys[k].Col, lines, edges);

            Console.WriteLine($"P1: {shortestPath(edges)}");
        }
    }
}
