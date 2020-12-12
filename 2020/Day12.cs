using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020
{
    public class Day12
    {
        private static readonly (int, int) NORTH = (-1, 0);
        private static readonly (int, int) SOUTH = (1, 0);
        private static readonly (int, int) EAST = (0, 1);
        private static readonly (int, int) WEST = (0, -1);

        public Day12() { }

        private (int, int) rotate((int, int) curr, string dir, int degree)
        {
            for (int j = 0; j < degree / 90; j++)
            {
                if (curr == NORTH || curr == SOUTH)
                    curr = dir == "L" ? (curr.Item2, curr.Item1) : (curr.Item2, -curr.Item1);
                else
                    curr = dir == "L" ? (-curr.Item2, curr.Item1) : (curr.Item2, curr.Item1);
            }

            return curr;
        }

        public void Solve()
        {
            TextReader tr = new StreamReader("inputs/day12.txt");

            var lines = tr.ReadToEnd().Split('\n');
            (int, int, (int, int)) pos = (0, 0, EAST);

            //string[] test = { "F10", "N3", "F7", "R90", "F11" };
            
            var instrs =  lines.Select(d => (d[..1], int.Parse(d[1..]))).ToList();
            foreach (var instr in instrs)
            {
                if (instr.Item1 == "L" || instr.Item1 == "R")
                {
                    pos.Item3 = rotate(pos.Item3, instr.Item1, instr.Item2);
                }
                else
                {
                    var dir = instr.Item1 switch
                    {
                        "N" => NORTH,
                        "S" => SOUTH,
                        "E" => EAST,
                        "W" => WEST,
                        _ => pos.Item3
                    };

                    pos = (pos.Item1 + dir.Item1 * instr.Item2, pos.Item2 + dir.Item2 * instr.Item2, pos.Item3);
                }
            }

            Console.WriteLine($"P1: {Util.TaxiDistance(0, 0, pos.Item1, pos.Item2)}");
        }
    }
}
