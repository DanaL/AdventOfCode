using System;
using System.Collections.Generic;
using System.Linq;

using AoC;

namespace _2021
{    
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

    class Day12Solver
    {
        readonly Dictionary<string, HashSet<string>> _vertexes;
        readonly Dictionary<string, uint> _smallCaveBitmasks;        
        int _pathCount;
        bool _allowOneLoop;

        public Day12Solver(List<(string, string)> input, bool allowOneLoop)
        {
            _allowOneLoop = allowOneLoop;
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
                else if ((_allowOneLoop && !route.Loop) || (_smallCaveBitmasks[nextNode] & route.Visited) == 0)                
                    FollowPath(route, nextNode);                
            }
        }

        public int Solve()
        {
            _pathCount = 0;
            FollowPath(new Route() { Visited = 0, Loop = false }, "start");
            return _pathCount;
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
            var p1 = new Day12Solver(FetchInput(), false);
            Console.WriteLine($"P1: {p1.Solve()}");
            var p2 = new Day12Solver(FetchInput(), true);
            Console.WriteLine($"P2: {p2.Solve()}");
        }
    }
}
