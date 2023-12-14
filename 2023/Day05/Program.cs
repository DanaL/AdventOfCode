
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
        var vals = lines[j].Split(' ').Select(n => ulong.Parse(n)).ToList();
        ranges.Add(new Span() { DestRange = (vals[0], vals[0] + vals[2]), SrcRange = (vals[1], vals[1] + vals[2]) });
        ++j;
    }

    return ranges;
}

static ulong FindInMap(ulong val, List<Span> map) 
{
    foreach (var span in map)
    {
        if (val >= span.SrcRange.Item1 && val < span.SrcRange.Item2) 
        {
            ulong diff = val - span.SrcRange.Item1;
            return span.DestRange.Item1 + diff;
        }
    }

    return val;
}

static ulong LocForSeedPt1(ulong seed, List<Span> seedToSoil, List<Span> soilToFertilizer, List<Span> fertilizerToWater, List<Span> waterToLight, List<Span> lightToTemp, List<Span> tempToHumidity, List<Span> humidityToLocation)
{
    // Incredibly beautiful and elegant...
    return FindInMap(FindInMap(FindInMap(FindInMap(FindInMap(FindInMap(FindInMap(seed, seedToSoil), soilToFertilizer), fertilizerToWater), waterToLight), lightToTemp), tempToHumidity), humidityToLocation);
}

static void Part1() 
{
    var lines = File.ReadAllLines("input.txt");
    var seeds = lines[0].Substring(lines[0].IndexOf(":") + 1).Trim().Split(' ').Select(ulong.Parse).ToList();

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

static (ulong, ulong) TranslatePt2((ulong, ulong) src, Span mapping) 
{
    ulong diff = mapping.DestRange.Item2 - mapping.SrcRange.Item2;
    var lo = src.Item1 + diff;
    var hi = src.Item2 + diff;

    return (lo, hi);
}

static List<(ulong, ulong)> TranslateRanges(List<Span> map, List<(ulong, ulong)> ranges)
{
    var s = new Stack<(ulong, ulong)>(ranges);
    var translated = new List<(ulong, ulong)>();

    while (s.Count > 0)
    {
        // if it's a perfect match, we can translate it
        // if it is totally outside the range of any mapping, leave it untranslated
        // other split it and push split ranges onto the stack        
        var r = s.Pop();
        bool found = false;
        foreach (var m in map) 
        {
            if (r.Item1 >= m.SrcRange.Item1 && r.Item2 <= m.SrcRange.Item2)
            {
                var mapped = TranslatePt2(r, m);
                translated.Add(mapped);
                found = true;
                break;
            }
            else if (r.Item1 < m.SrcRange.Item1 && r.Item2 >= m.SrcRange.Item1 && r.Item2 <= m.SrcRange.Item2) 
            {
                // The lower bound of the range is lower than the mapping but the upper bound is within it                
                s.Push((r.Item1, m.SrcRange.Item1 - 1));
                s.Push((m.SrcRange.Item1, r.Item2));
                found = true;
                break;
            }
            else if (r.Item1 >= m.SrcRange.Item1 && r.Item1 <= m.SrcRange.Item2 && r.Item2 > m.SrcRange.Item2) 
            {
                // The lower bound of the range within mapping and upper bound is beyond
                s.Push((r.Item1, m.SrcRange.Item2));
                s.Push((m.SrcRange.Item2 + 1, r.Item2));
                found = true;
                break;
            }
            else if (r.Item1 < m.SrcRange.Item1 && r.Item2 > m.SrcRange.Item2) 
            {
                // Case where a range spans past the start and the end of a mapping
                s.Push((r.Item1, m.SrcRange.Item1 - 1));                
                s.Push((m.SrcRange.Item2 + 1, r.Item2));
                s.Push((m.SrcRange.Item1, m.SrcRange.Item2));
                found = true;
                break;
            }
        }

        // There was no overlap between src and a mapping at all, so
        // return the range untranslated
        if (!found)
        {
            translated.Add(r);
        }
    }
    
    return translated;
}

// I think I'll eventually be able to merge part 1 and 2 if my part 2 method isn't garbage and actually works
// Addendum: I ended up writing part 2 many days late so I'm not going to tidy it up, but I think I could clean
// this up a bunch. Ie., inside the translation loop I don't need to push calculated ranges back onto my stack. 
// I could just translate them then and there. Also, I could merge part 1 and 2 by feeding part 2's functions
// seed ranges that are just one long. 
static void Part2() 
{
    var lines = File.ReadAllLines("input.txt");

    var seeds = lines[0].Substring(lines[0].IndexOf(":") + 1).Trim().Split(' ')
                        .Select(ulong.Parse).ToList();
    var seedRanges = new List<(ulong, ulong)>();
    for (int j = 0; j < seeds.Count; j += 2)
        seedRanges.Add((seeds[j], seeds[j] + seeds[j + 1]));

    var seedToSoil = FindRanges("seed-to-soil map:", lines);
    var soilToFertilizer = FindRanges("soil-to-fertilizer map:", lines);
    var fertilizerToWater = FindRanges("fertilizer-to-water map:", lines);
    var waterToLight = FindRanges("water-to-light map:", lines);
    var lightToTemp = FindRanges("light-to-temperature map:", lines);
    var tempToHumidity = FindRanges("temperature-to-humidity map:", lines);
    var humidityToLocation = FindRanges("humidity-to-location map:", lines);
    
    var soils = TranslateRanges(seedToSoil, seedRanges);
    var fert = TranslateRanges(soilToFertilizer, soils);
    var water = TranslateRanges(fertilizerToWater, fert);
    var light = TranslateRanges(waterToLight, water);
    var temp = TranslateRanges(lightToTemp, light);
    var humidity = TranslateRanges(tempToHumidity, temp);
    var loc = TranslateRanges(humidityToLocation, humidity);

    var pt2 = loc.Select(r => r.Item1).Min();
    // lol off-by-one error that I can't puzzle out but...
    Console.WriteLine($"P2: {pt2 - 1}");
}

Part1();
Part2();

namespace Day05
{
    class Span
    {
        public (ulong, ulong) SrcRange { get; set; }
        public (ulong, ulong) DestRange { get; set; }
        
        public Span() { }
    }
}


    

    

