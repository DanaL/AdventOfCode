﻿using System;
using _2020;

int _day;
if (args.Length == 0 || !int.TryParse(args[0], out _day))
{
    _day = DateTime.Now.Day;
    if (DateTime.Now.Hour == 23)
        ++_day;
}

Console.WriteLine($"Solving Day {_day}");
switch (_day)
{
    case 1:
        Day1 _d1 = new Day1();
        _d1.SolvePart1();
        _d1.SolvePart2();
        break;
    case 2:
        Day2.Solve();
        break;
    case 3:
        Day3 _d3 = new Day3();
        _d3.Solve();
        break;
    case 4:
        Day4 _d4 = new Day4();
        _d4.Solve();
        break;
    case 5:
        Day5 _d5 = new Day5();
        _d5.Solve();
        break;
    case 6:
        Day6 _d6 = new Day6();
        _d6.Solve();
        break;
    case 7:
        Day7 _d7 = new Day7();
        _d7.Solve();
        break;
    case 8:
        Day8 _d8 = new Day8();
        _d8.SolveP1();
        _d8.SolveP2();
        break;
    case 9:
        Day9 _d9 = new Day9();
        _d9.Solve();
        break;
    case 10:
        Day10 _d10 = new Day10();
        _d10.Solve();
        break;
    case 11:
        Day11 _d11 = new Day11();
        _d11.Solve();
        break;
    case 12:
        Day12 _d12 = new Day12();
        _d12.Solve();
        break;
    case 13:
        Day13 _d13 = new Day13();
        _d13.Solve();
        break;
    default:
        Console.WriteLine($"Haven't done Day {_day} yet.");
        break;
}
