﻿
using Day05;

static List<Span> FindRanges(string section, string[] lines)
{
    var ranges = new List<Span>();

    int j = 0;
    while (lines[j] != section)
        ++j;
    ++j;
    while (j < lines.Length && lines[j] != "")
    {
        var vals = lines[j].Split(' ').Select(n => long.Parse(n)).ToList();
        ranges.Add(new Span() { DestRange = (vals[0], vals[0] + vals[2]), SrcRange = (vals[1], vals[1] + vals[2]) });
        ++j;
    }

    return ranges;
}

static long FindInMap(long val, List<Span> map) 
{
    foreach (var span in map)
    {
        if (val >= span.SrcRange.Item1 && val < span.SrcRange.Item2) 
        {
            long diff = val - span.SrcRange.Item1;
            return span.DestRange.Item1 + diff;
        }
    }

    return val;
}

static long LocForSeedPt1(long seed, List<Span> seedToSoil, List<Span> soilToFertilizer, List<Span> fertilizerToWater, List<Span> waterToLight, List<Span> lightToTemp, List<Span> tempToHumidity, List<Span> humidityToLocation)
{
    // Incredibly beautiful and elegant...
    return FindInMap(FindInMap(FindInMap(FindInMap(FindInMap(FindInMap(FindInMap(seed, seedToSoil), soilToFertilizer), fertilizerToWater), waterToLight), lightToTemp), tempToHumidity), humidityToLocation);
}

static void Part1() 
{
    var lines = File.ReadAllLines("input.txt");
    var seeds = lines[0].Substring(lines[0].IndexOf(":") + 1).Trim().Split(' ').Select(n => long.Parse(n)).ToList();

    var seedToSoil = FindRanges("seed-to-soil map:", lines);
    var soilToFertilizer = FindRanges("soil-to-fertilizer map:", lines);
    var fertilizerToWater = FindRanges("fertilizer-to-water map:", lines);
    var waterToLight = FindRanges("water-to-light map:", lines);
    var lightToTemp = FindRanges("light-to-temperature map:", lines);
    var tempToHumidity = FindRanges("temperature-to-humidity map:", lines);
    var humidityToLocation = FindRanges("humidity-to-location map:", lines);

    var p1 = seeds.Select(s => LocForSeedPt1(s, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemp, tempToHumidity, humidityToLocation)).Min();
    Console.WriteLine($"P1: {p1}");
}

// I think I'll eventually be able to merge part 1 and 2 if my part 2 method isn't garbage and actually works
static void Part2() 
{
    var lines = File.ReadAllLines("input.txt");

    var seeds = lines[0].Substring(lines[0].IndexOf(":") + 1).Trim().Split(' ')
                        .Select(n => long.Parse(n)).ToList();
    var seedRanges = new List<(long, long)>();
    for (int j = 0; j < seeds.Count; j += 2)
        seedRanges.Add((seeds[j], seeds[j] + seeds[j + 1] - 1));

    Console.WriteLine(seedRanges[0]);
    Console.WriteLine(seedRanges[1]);

    var seedToSoil = FindRanges("seed-to-soil map:", lines);
    var soilToFertilizer = FindRanges("soil-to-fertilizer map:", lines);
    var fertilizerToWater = FindRanges("fertilizer-to-water map:", lines);
    var waterToLight = FindRanges("water-to-light map:", lines);
    var lightToTemp = FindRanges("light-to-temperature map:", lines);
    var tempToHumidity = FindRanges("temperature-to-humidity map:", lines);
    var humidityToLocation = FindRanges("humidity-to-location map:", lines);

    Console.WriteLine(LocForSeedPt1(82, seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemp, tempToHumidity, humidityToLocation));
}

Part1();
Part2();

namespace Day05
{
    class Span
    {
        public (long, long) SrcRange { get; set; }
        public (long, long) DestRange { get; set; }
        
        public Span() { }
    }
}


    

    
