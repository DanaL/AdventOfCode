using System;
using System.Collections.Generic;
using System.Diagnostics;

using AoC;
using _2021;

Dictionary<int, IDay> days = new Dictionary<int, IDay>();
days.Add(1, new Day01());
days.Add(2, new Day02());
days.Add(3, new Day03());
days.Add(4, new Day04());
days.Add(5, new Day05());
days.Add(6, new Day06());
days.Add(7, new Day07());
days.Add(8, new Day08());
days.Add(9, new Day09());
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
days.Add(20, new Day20());

int day;
if (args.Length == 0 || !int.TryParse(args[0], out day))
{
    day = DateTime.Now.Day;
    if (DateTime.Now.Hour == 23)
        ++day;
}

day = 20;

Console.WriteLine($"Solving day {day}...");
if (!days.ContainsKey(day))
    Console.WriteLine($"Haven't done day {day} yet.");
else
{
    Stopwatch sw = Stopwatch.StartNew();
    days[day].Solve();
    sw.Stop();
    Console.WriteLine($"Execution time: {sw.ElapsedMilliseconds} ms.");
}
