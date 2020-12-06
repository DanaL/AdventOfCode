using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    public class Path
    {
        public int Length { get; set; }
        public HashSet<char> KeysNeeded { get; set; }
        public char Start { get; set; }
        public char End { get; set; }

        public Path() { }
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
                _bitmasks.Add(Char.ToUpper(c), this.letterToNum(c));
            }
        }

        private uint letterToNum(char c)
        {
            uint exp = (uint)Char.ToLower(c) - (uint)'a';
            
            return (uint)Math.Pow(2, exp);
        }

        private void floodfill(int start_r, int start_c, string[] map)
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
                    
                    if (ch == '.')
                        nodes.Enqueue(new_node);
                    else if (ch >= 'A' && ch <= 'Z')
                    {
                        new_node.Doors = node.Doors | _bitmasks[ch];                        
                        nodes.Enqueue(new_node);
                    }
                    else if (ch >= 'a' && ch <= 'z')
                    {
                        Console.WriteLine($"Key {ch} found! D: {new_node.Distance} Doors passed: {new_node.Doors} ");
                        // What we really need to do is store the path from start to the key, which is a vertex of our
                        // graph and then keep going
                    }
                }
            }
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
            // which doors must be passed through

            this.floodfill(keys['c'].Row, keys['c'].Col, lines);
        }
    }
}
