namespace Day03;

class Part 
{
    public int Num { get; set; }
    public HashSet<(int, int)> Coords { get; set; }

    public Part() => Coords = new HashSet<(int, int)>();
}

class Program
{
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

    //static bool 
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var parts = new List<Part>();
        for (int j = 0; j < lines.Length; j++) 
        {
            var p = ParseLine(j, lines[j]);
            parts.AddRange(p);
        }

        Console.WriteLine(parts.Count);
    }
}
