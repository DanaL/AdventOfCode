﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

using _2021;

Dictionary<int, IDay> days = new Dictionary<int, IDay>();
days.Add(1, new Day01());
days.Add(2, new Day02());
days.Add(3, new Day03());

int day;
if (args.Length == 0 || !int.TryParse(args[0], out day))
{
    day = DateTime.Now.Day;
    if (DateTime.Now.Hour == 23)
        ++day;
}

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
