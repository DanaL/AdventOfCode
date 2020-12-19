using System;
using System.Collections.Generic;

using _2020;

Dictionary<int, IDay> days = new Dictionary<int, IDay>();
days.Add(1, new Day1());
days.Add(2, new Day2());
days.Add(3, new Day3());
days.Add(4, new Day4());
days.Add(5, new Day5());
days.Add(6, new Day6());
days.Add(7, new Day7());
days.Add(8, new Day8());
days.Add(9, new Day9());
days.Add(10, new Day10());
days.Add(11, new Day11());
days.Add(12, new Day12());
days.Add(13, new Day13());
days.Add(14, new Day14());
days.Add(15, new Day15());
days.Add(16, new Day16());
days.Add(17, new Day17());
days.Add(18, new Day18());
days.Add(19, new Day19());

int day;
if (args.Length == 0 || !int.TryParse(args[0], out day))
{
    day = DateTime.Now.Day;
    if (DateTime.Now.Hour == 23)
        ++day;
}

Console.WriteLine($"Solving Day {day}");
if (!days.ContainsKey(day))
    Console.WriteLine($"Haven't done Day {day} yet.");
else
    days[day].Solve();
