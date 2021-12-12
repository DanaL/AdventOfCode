using System;
using System.Collections.Generic;
using System.Linq;

using AoC;

namespace _2021
{
    public class Day12 : IDay
    {
        static List<(string, string)> FetchInput()
        {
            var txt = @"
                start-A
                start-b
                A-c
                A-b
                b-d
                A-end
                b-end";

            return txt.Trim().Split('\n').Select(l => l.Split('-')).Select(l => (l[0].Trim(), l[1].Trim())).ToList();
        }

        void FindPath(string start, Dictionary<string, HashSet<string>> nodes)
        {            
            Console.WriteLine(start);

            if (!nodes.ContainsKey(start))
                return;
            foreach (var n in nodes[start])
            {
                FindPath(n, nodes);
                //break;
            }
        }

        public void Solve()
        {
            // the paths need to be directional
            var nodes = new Dictionary<string, HashSet<string>>();
            foreach (var pair in FetchInput())
            {
                if (!nodes.ContainsKey(pair.Item1))
                    nodes.Add(pair.Item1, new HashSet<string>());
                nodes[pair.Item1].Add(pair.Item2);
            }

            // may as well store path and track whcih small caves have been visited
            FindPath("start", nodes);
        }
    }
}
