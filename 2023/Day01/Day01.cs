using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AoC;

public class Day01 : IDay 
{
    public void Solve()
    {
       var p1 = File.ReadAllLines(@"input.txt")
                         .Select(l => l.ToCharArray().Where(ch => char.IsNumber(ch)).ToList())
                         .Select(chs => int.Parse($"{chs.First()}{chs.Last()}"))
                         .Sum();

        Console.WriteLine($"P1: {p1}");        
    }
}