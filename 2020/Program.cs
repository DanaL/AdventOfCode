using System;
using _2020;

int _day;
if (args.Length == 0 || !int.TryParse(args[0], out _day))
    _day = DateTime.Now.Day;

Console.WriteLine($"Solving Day {_day}");
switch (_day)
{
    case 1:
        Day1 _d1 = new Day1();
        _d1.SolvePart1();
        _d1.SolvePart2();
        break;
    case 2:
        Day2 _d2 = new Day2();
        _d2.SolvePart1();
        break;
    default:
        Console.WriteLine($"Haven't done Day {_day} yet.");
        break;
}
