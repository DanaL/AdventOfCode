using Day18;

static (char, long) ParseLine(string line)
{
    int i = line.IndexOf('#') + 1;
    long d = Convert.ToInt64(line[i..(i+5)], 16);
    char ch = line[i + 5] switch
    {
        '0' => 'R',
        '1' => 'D',
        '2' => 'L',
        _ => 'U'
    };

    return (ch, d);
}

static long Determinant(long x1, long y1, long x2, long y2)
{
    return x1 * y2 - x2 * y1;
}

static long CalcArea2(List<(char, long)> instrs)
{
    Pt pt = new Pt(0, 0);
    List<Pt> pts = [new Pt(0, 0)];
    long perimeter = 0;

    foreach (var p in instrs)
    {
        long d = p.Item2;        
        pt = p.Item1 switch
        {
            'R' => pt with { X = pt.X + d},
            'L' => pt with { X = pt.X - d},
            'U' => pt with { Y = pt.Y + d},
            _ => pt with { Y = pt.Y - d}
        };

        perimeter += d;
        pts.Add(pt);        
    }

    // The shoelace formula sums the determinants of the points in sequence. These are the interior
    // squares but we need to add the perimiter and divide by 2 + 1 because we have chonky ASCII lines
    long area = 0;
    for (int j = 0; j < pts.Count - 1; j++)
          area += Determinant(pts[j].X, pts[j].Y, pts[j + 1].X, pts[j + 1].Y);
    area = Math.Abs(area);
        
    return (area + perimeter) / 2 + 1;
}

var instrs = File.ReadAllLines("input.txt").Select(l => l.Split(' '))
                 .Select(l => (l[0][0], long.Parse(l[1]))).ToList();
Console.WriteLine($"P1: {CalcArea2(instrs)}");
Console.WriteLine($"P2: {CalcArea2(File.ReadAllLines("input.txt").Select(ParseLine).ToList())}");

namespace Day18
{
    public record Pt(long X, long Y);
}