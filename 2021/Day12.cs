using System;
using System.Collections.Generic;
using System.Linq;

using AoC;

namespace _2021
{
    // For part 1, I implemented generating the paths with the first, simple idea that occurred to
    // and didn't worry too much about computational efficiency. So, for example, I'm storing the paths
    // I'm exploring as growing strings and when checking if a small cave has been previously visited, I
    // just linear search through the string.
    class Part1Solver
    {
        static bool SmallCaveInPath(string path, string cave)
        {
            if (cave.Where(c => char.IsUpper(c)).Count() > 0)
                return false;
            return path.IndexOf($"-{cave}-") > -1;
        }

        public static void SolvePart1(List<(string, string)> input)
        {
            var createdPaths = new HashSet<string>();
            var nodes = new Dictionary<string, HashSet<string>>();
            var beingBuilt = new Queue<(string, string)>();
            beingBuilt.Enqueue(("start", "start"));
            foreach (var pair in input)
            {
                if (!nodes.ContainsKey(pair.Item1))
                    nodes.Add(pair.Item1, new HashSet<string>());
                nodes[pair.Item1].Add(pair.Item2);

                // Invert the paths because they should be directional
                if (pair.Item1 == "start" || pair.Item2 == "end")
                    continue;

                if (!nodes.ContainsKey(pair.Item2))
                    nodes.Add(pair.Item2, new HashSet<string>());
                nodes[pair.Item2].Add(pair.Item1);
            }

            int pathCount = 0;
            while (beingBuilt.Count > 0)
            {
                var pathInfo = beingBuilt.Dequeue();
                var node = pathInfo.Item1;
                var path = pathInfo.Item2;
                foreach (var nextNode in nodes[node])
                {
                    var nextPath = $"{path}-{nextNode}";
                    if (!createdPaths.Contains(nextPath))
                    {
                        createdPaths.Add(nextPath);

                        if (nextNode != "end" && !SmallCaveInPath(path, nextNode))
                            beingBuilt.Enqueue((nextNode, nextPath));
                        else if (nextNode == "end")
                            ++pathCount;
                    }
                }
            }

            Console.WriteLine($"P1: {pathCount}");
        }
    }

    // For part 2, since the # of paths through the graph are much, much bigger and
    // I need to allow one loop through a small cave, I got slightly more sophisticated
    // and use a Route struct to track info about the path we are following, and use
    // an integer and bitmasks to track which caves we've visited. It's much, much
    // more performant than Part 1 :P
    struct Route
    {
        public uint Visited { get; set; }
        public bool Loop;
    }

    class Part2Solver
    {
        readonly Dictionary<string, HashSet<string>> _vertexes;
        readonly Dictionary<string, uint> _smallCaveBitmasks;        
        int _pathCount;

        public Part2Solver(List<(string, string)> input)
        {
            _vertexes = new Dictionary<string, HashSet<string>>();
            
            // Build dictionary of paths through graph
            foreach (var pair in input)
            {
                if (!_vertexes.ContainsKey(pair.Item1))
                    _vertexes[pair.Item1] = new HashSet<string>();
                _vertexes[pair.Item1].Add(pair.Item2);

                if (pair.Item1 == "start" || pair.Item2 == "end") continue;

                // Add the path back the other direction (if vertex isn't to/from start or end)
                if (!_vertexes.ContainsKey(pair.Item2))
                    _vertexes[pair.Item2] = new HashSet<string>();
                _vertexes[pair.Item2].Add(pair.Item1);
            }

            // Next, build the tables of bitmasks for the small caves
            _smallCaveBitmasks = new Dictionary<string, uint>();
            uint bitmask = 1;
            foreach (string node in _vertexes.Keys.Where(n => !n.Any(c => char.IsUpper(c)) && n != "start" && n != "end"))
            {
                _smallCaveBitmasks.Add(node, bitmask);
                bitmask <<= 1;
            }
        }

        void FollowPath(Route route, string currCave)
        {
            if (_smallCaveBitmasks.ContainsKey(currCave))
            {
                if ((_smallCaveBitmasks[currCave] & route.Visited) == 0)
                    route.Visited |= _smallCaveBitmasks[currCave];
                else
                    route.Loop = true;
            }

            foreach (string nextNode in _vertexes[currCave])
            {
                if (nextNode == "end")
                    ++_pathCount;
                else if (!_smallCaveBitmasks.ContainsKey(nextNode))
                    FollowPath(route, nextNode);
                else if (!route.Loop || (_smallCaveBitmasks[nextNode] & route.Visited) == 0)                
                    FollowPath(route, nextNode);                
            }
        }

        public void Solve()
        {
            _pathCount = 0;
            FollowPath(new Route() { Visited = 0, Loop = false }, "start");
            Console.WriteLine($"P2: {_pathCount}");
        }
    }

    public class Day12 : IDay
    {
        static List<(string, string)> FetchInput()
        {
            var txt = @"
          VJ-nx
start-sv
nx-UL
FN-nx
FN-zl
end-VJ
sv-hi
em-VJ
start-hi
sv-em
end-zl
zl-em
hi-VJ
FN-em
start-VJ
jx-FN
zl-sv
FN-sv
FN-hi
nx-end";
            return txt.Trim().Split('\n').Select(l => l.Split('-')).Select(l => (l[0].Trim(), l[1].Trim())).ToList();
        }

        public void Solve()
        {
            Part1Solver.SolvePart1(FetchInput());
            var part2 = new Part2Solver(FetchInput());
            part2.Solve();
        }
    }
}
