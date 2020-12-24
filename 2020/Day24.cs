using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day24 : IDay
    {
        private static string[] _dirs = { "e", "se", "sw", "w", "nw", "ne" };

        public Day24() { }

        private static (int, int, int) dirToCoords(string dir)
        {
            return dir switch
            {
                "e" => (1, 0, -1),
                "se" => (0, 1, -1),
                "sw" => (-1, 1, 0),
                "w" => (-1, 0, 1),
                "nw" => (0, -1, 1),
                "ne" => (1, -1, 0),
                _ => throw new Exception("Hmm this shouldn't happen...")
            };
        }

        private static Stack<char> pathToStack(string path)
        {
            Stack<char> stack = new Stack<char>();

            foreach (char c in path.ToCharArray().Reverse())
                stack.Push(c);

            return stack;
        }

        private (int, int, int) walkPath(string path, Dictionary<(int, int, int), bool> tiles)
        {
            var hex = (0, 0, 0);
            var stack = pathToStack(path);

            while (stack.Count > 0)
            {
                string s = "";
                char c = stack.Pop();
                s += c;

                if (c == 'n' || c == 's')
                    s+= stack.Pop();
                var move = dirToCoords(s);
                hex.Item1 += move.Item1;
                hex.Item2 += move.Item2;
                hex.Item3 += move.Item3;

                fillInAdjacent(hex, tiles);
            }

            return hex;
        }

        private int countAdjBlack((int, int, int) tile, Dictionary<(int, int, int), bool> tiles)
        {
            // A for loop is probably cleaner but this is more fun to write!
            return _dirs.Select(a => dirToCoords(a))
                 .Select(b => (tile.Item1 + b.Item1, tile.Item2 + b.Item2, tile.Item3 + b.Item3))
                 .Where(c => tiles.ContainsKey(c) && tiles[c])
                 .Count();
        }

        private void fillInAdjacent((int, int, int) tile, Dictionary<(int, int, int), bool> tiles)
        {
            foreach (string d in _dirs)
            {
                var delta = dirToCoords(d);
                var adj = (tile.Item1 + delta.Item1, tile.Item2 + delta.Item2, tile.Item3 + delta.Item3);
                if (!tiles.ContainsKey(adj))
                    tiles.Add(adj, false);
            }
        }

        private Dictionary<(int, int, int), bool> iterate(Dictionary<(int, int, int), bool> tiles)
        {
            var next = new Dictionary<(int, int, int), bool>();
            foreach (var k in tiles.Keys)
            {
                var adj = countAdjBlack(k, tiles);
                if (tiles[k])
                    next.Add(k, adj == 1 || adj == 2);
                else
                    next.Add(k, adj == 2);
            }

            foreach (var k in next.Keys.Select(k => k).ToArray())
                fillInAdjacent(k, next);

            return next;
        }

        public void Solve()
        {
            Dictionary<(int, int, int), bool> tiles = new Dictionary<(int, int, int), bool>();
            tiles.Add((0, 0, 0), false);

            foreach (var line in File.ReadAllLines("inputs/day24.txt"))
            {
                var tile = walkPath(line, tiles);
                if (!tiles.ContainsKey(tile))
                    tiles.Add(tile, true);                    
                else 
                    tiles[tile] = !tiles[tile];
            }

            Console.WriteLine($"P1: {tiles.Values.Where(t => t).Count()}");

            for (int j = 0; j < 100; j++)
                tiles = iterate(tiles);
                
            Console.WriteLine($"P2: {tiles.Values.Where(t => t).Count()}");
        }        
    }
}
