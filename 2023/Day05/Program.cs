
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
        ranges.Add(new Span() { Dest = vals[0], Src = vals[1], Len = vals[2] });
        ++j;
    }

    return ranges;
}

var lines = File.ReadAllLines("input.txt");
var seeds = lines[0].Substring(lines[0].IndexOf(":") + 1).Trim().Split(' ').Select(n => long.Parse(n)).ToList();

var seedToSoil = FindRanges("seed-to-soil map:", lines);
var soilToFertilizer = FindRanges("soil-to-fertilizer map:", lines);
var fertilizerToWater = FindRanges("fertilizer-to-water map:", lines);
var waterToLight = FindRanges("water-to-light map:", lines);
var lightToTemp = FindRanges("light-to-temperature map:", lines);
var tempToHumidity = FindRanges("temperature-to-humidity map:", lines);
var humidityToLocation = FindRanges("humidity-to-location map:", lines);

namespace Day05
{
    class Span
    {
        public long Src { get; set; }
        public long Dest { get; set; }
        public long Len { get; set; }

        public Span() { }
    }
}


    

    

