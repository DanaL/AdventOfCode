namespace Day03;

class Part 
{
    public int Num { get; set; }
    public HashSet<(int, int)> Coords { get; set; }

    public Part() => Coords = new HashSet<(int, int)>();
}

class Program
{
    static List<(int, int)> _neighbours = new() { (-1, -1), (-1, 0), (-1, 1), 
                                                  ( 0, -1),          ( 0, 1), 
                                                  ( 1, -1), ( 1, 0), ( 1, 1)};

    static List<Part> ParseLine(int row, string line, HashSet<(int, int)> gears) 
    {
        int c = 0;
        var parts = new List<Part>();
        while (c < line.Length) 
        {
            // skip non-digits, and look for gears
            while (c < line.Length && !char.IsNumber(line[c])) 
            {
                if (line[c] == '*')
                    gears.Add((row, c));
                ++c;
            }

            var p = new Part();
            var n = "";
            while (c < line.Length && char.IsNumber(line[c])) 
            {
                n += line[c];
                p.Coords.Add((row, c));                
                ++c;
            }
            if (n.Length > 0 && p.Coords.Count > 0) 
            {
                p.Num = int.Parse(n);
                parts.Add(p);
            }
        }

        return parts;
    }

    static bool IsPartNumber(Part part, string[] lines) 
    {
        int length = lines.Length - 1;
        int width = lines[0].Length - 1;

        foreach (var coord in part.Coords)
        {
            foreach (var n in _neighbours) 
            {
                int row = coord.Item1 + n.Item1;
                int col = coord.Item2 + n.Item2;
                if (row < 0 || col < 0 || row > length || col > width || part.Coords.Contains((row, col)))
                    continue;
                if (lines[row][col] != '.')
                    return true;
            }
        }

        return false;
    }

    static long GearRatio((int, int) gear, List<Part> parts) 
    {
        var adj = new HashSet<int>();
        foreach (var n in _neighbours) 
        {
            var coord = (gear.Item1 + n.Item1, gear.Item2 + n.Item2);
            foreach (var p in parts) 
            {
                if (p.Coords.Contains(coord))
                    adj.Add(p.Num);
            }
        }

        var pns = adj.ToList();
        return pns.Count == 2 ? pns[0] * pns[1] : 0;
    }

    static void Main(string[] args)
    {
        var gears = new HashSet<(int, int)>();
        var lines = File.ReadAllLines("input.txt");

        var parts = new List<Part>();
        for (int j = 0; j < lines.Length; j++) 
        {
            var p = ParseLine(j, lines[j], gears);
            parts.AddRange(p);
        }

        // for part 1, we need to find the sum of any part #s
        // which are adjacent to a symbol
        var p1 = parts.Where(p => IsPartNumber(p, lines))
                      .Select(p => p.Num)
                      .Sum();
        Console.WriteLine($"P1: {p1}");
        
        // for part 2, we need to check all gears (the *s) and if they are
        // adjacent to two parts, multiply their part numbers for the 'gear ratio'
        // and sum them up
        var p2 = gears.Select(g => GearRatio(g, parts)).Sum();
        Console.WriteLine($"P2: {p2}");
    }
}
