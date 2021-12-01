using System;
using System.Collections.Generic;

using _2021;

Dictionary<int, IDay> days = new Dictionary<int, IDay>();

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
    days[day].Solve();
