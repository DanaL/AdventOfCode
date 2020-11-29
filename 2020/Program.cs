using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using _2020;

using TextReader _tr = new StreamReader("inputs/day1.txt");

List<int> _vals = new();

while (_tr.Peek() != -1)
{
    _vals.Add(int.Parse(_tr.ReadLine()));
}

var _numbers = _vals.ToArray();
var _subset = _numbers[1..4];
var _subset2 = _numbers[^4..^0];

Console.WriteLine(_subset.Sum());

foreach (var j in _subset2)
    Console.WriteLine(j);

Console.WriteLine($"d: {Util.TaxiDistance(4, 16, 7, 8)}");