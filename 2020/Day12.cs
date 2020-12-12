using System;
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

        private (char, int)[] _instructions;

        public Day12()
        {
            TextReader tr = new StreamReader("inputs/day12.txt");
            var lines = tr.ReadToEnd().Split('\n');
            _instructions = lines.Select(d => (d[..1][0], int.Parse(d[1..]))).ToArray();
        }

        private void part1()
        {            
            (int, int, (int, int)) pos = (0, 0, EAST);

            foreach (var instr in _instructions)
            {
                if (instr.Item1 == 'L' || instr.Item1 == 'R')
                {
                    pos.Item3 = rotate(pos.Item3, instr.Item1, instr.Item2);
                }
                else
                {
                    var dir = instr.Item1 switch
                    {
                        'N' => NORTH,
                        'S' => SOUTH,
                        'E' => EAST,
                        'W' => WEST,
                        _ => pos.Item3
                    };

                    pos = (pos.Item1 + dir.Item1 * instr.Item2, pos.Item2 + dir.Item2 * instr.Item2, pos.Item3);
                }
            }

            Console.WriteLine($"P1: {Util.TaxiDistance(0, 0, pos.Item1, pos.Item2)}");
        }
        
        private (int, int) rotate((int, int) wp, char dir, int degree)
        {
            // Okay, so clockwise rotations are just the inversion of counterclockwise rotations (ie.,
            // R90 is the same as L270) so we'll just worry about the conterclockwise rotations
            if (dir == 'R')
                degree = degree == 180 ? degree : (degree + 180) % 360;
            
            // Rotating a vector is a matrix multiplcation but they boil down to swapping
            // x and y and sometimes inverting the signs. Ie., 90' rotation counterclockwise
            // is multiplying (x, y) by [-1 1] to give you (-y, x)
            if (degree == 90)
                wp = (-wp.Item2, wp.Item1);
            else if (degree == 180)
                wp = (-wp.Item1, -wp.Item2);
            else
                wp = (wp.Item2, -wp.Item1);

            return wp;
        }

        private void part2()
        {
            (int, int) ship = (0, 0);
            (int, int) waypoint = (-1, 10);

            foreach (var instr in _instructions)
            {
                switch (instr.Item1)
                {
                    case 'N':
                        waypoint.Item1 -= instr.Item2;
                        break;
                    case 'S':
                        waypoint.Item1 += instr.Item2;
                        break;
                    case 'E':
                        waypoint.Item2 += instr.Item2;
                        break;
                    case 'W':
                        waypoint.Item2 -= instr.Item2;
                        break;
                    case 'F':
                        ship.Item1 += waypoint.Item1 * instr.Item2;
                        ship.Item2 += waypoint.Item2 * instr.Item2;
                        break;
                    case 'R':
                    case 'L':
                        waypoint = rotate(waypoint, instr.Item1, instr.Item2);
                        break;                    
                }
            }

            Console.WriteLine($"P2: {Util.TaxiDistance(0, 0, ship.Item1, ship.Item2)}");
        }

        public void Solve()
        {
            part1();
            part2();
        }
    }
}
