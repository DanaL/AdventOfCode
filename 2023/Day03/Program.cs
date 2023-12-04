namespace Day03;

class Part 
{
    public int Num { get; set; }
    public HashSet<(int, int)> Coords { get; set; }

    public Part() => Coords = new HashSet<(int, int)>();
}

class Program
{
    public static List<(int, int)> _neighbours = new() { (-1, -1), (-1, 0), (-1, 1), 
                                                         ( 0, -1),          ( 0, 1), 
                                                         ( 1, -1), ( 1, 0), ( 1, 1)};
                                                             
    static List<Part> ParseLine(int row, string line) 
    {
        int c = 0;
        var parts = new List<Part>();
        while (c < line.Length) 
        {
            // find a digit
            while (c < line.Length && !char.IsNumber(line[c]))
                ++c;

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
            ++c;
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

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var parts = new List<Part>();
        for (int j = 0; j < lines.Length; j++) 
        {
            var p = ParseLine(j, lines[j]);
            parts.AddRange(p);
        }

        var p1 = parts.Where(p => IsPartNumber(p, lines))
                      .Select(p => p.Num)
                      .Sum();

        Console.WriteLine($"P1: {p1}");
    }
}
