using Day16;

static Photon Move(Photon p)
{
    return p.Dir switch
    {
        Dir.North => new Photon(p.Row - 1, p.Col, p.Dir),
        Dir.South => new Photon(p.Row + 1, p.Col, p.Dir),
        Dir.East => new Photon(p.Row, p.Col + 1, p.Dir),
        Dir.West => new Photon(p.Row, p.Col - 1, p.Dir)
    };
}

static Photon Reflect(Photon p, char mirror)
{
    Dir dir;
    if (mirror == '\\')
    {
        dir = p.Dir switch 
        {
            Dir.North => Dir.West,
            Dir.South => Dir.East,
            Dir.East => Dir.South,
            Dir.West => Dir.North
        };
    }
    else
    {
        dir = p.Dir switch 
        {
            Dir.North => Dir.East,
            Dir.South => Dir.West,
            Dir.East => Dir.North,
            Dir.West => Dir.South
        };
    }

    return new Photon(p.Row, p.Col, dir);
}

static void FollowBeam(Dictionary<(int, int), char> sqs, HashSet<Photon> paths, Photon photon)
{   
    do
    {
        if (!sqs.ContainsKey((photon.Row, photon.Col)) || paths.Contains(photon))
            break; // beam has moved out of bounds or beam is looping

        paths.Add(photon);

        char ch = sqs[(photon.Row, photon.Col)];
        if (ch == '/' || ch == '\\')
        {
            photon = Move(Reflect(photon, ch));
        }
        else if (ch == '|' && (photon.Dir == Dir.East || photon.Dir == Dir.West))
        {
            FollowBeam(sqs, paths,  Move(new Photon(photon.Row, photon.Col, Dir.North)));
            FollowBeam(sqs, paths,  Move(new Photon(photon.Row, photon.Col, Dir.South)));
        }
        else if (ch == '-' && (photon.Dir == Dir.North || photon.Dir == Dir.South))
        {
            FollowBeam(sqs, paths,  Move(new Photon(photon.Row, photon.Col, Dir.East)));
            FollowBeam(sqs, paths,  Move(new Photon(photon.Row, photon.Col, Dir.West)));
        }
        else 
        {
            photon = Move(photon);
        }
    }
    while (true);
}

static int TestConfig(Dictionary<(int, int), char> sqs, Photon start)
{
    HashSet<Photon> paths = new();
    FollowBeam(sqs, paths, start);
    return paths.Select(ph => (ph.Row, ph.Col))
                .Distinct()
                .Count();
}

var lines = File.ReadAllLines("input.txt");
var sqs = new Dictionary<(int, int), char>();

for (int r = 0; r < lines.Length; r++)
{
    for (int c = 0; c < lines[r].Length; c++)
        sqs.Add((r, c), lines[r][c]);
}

var start = new Photon(0, 0, Dir.East);
Console.WriteLine($"P1: {TestConfig(sqs, start)}");

var results = new List<int>();
for (int c = 1; c < lines[0].Length - 1; c++) 
{
    results.Add(TestConfig(sqs, new Photon(0, c, Dir.South)));
    results.Add(TestConfig(sqs, new Photon(lines.Length - 1, c, Dir.North)));
}
for (int r = 1; r < lines.Length - 1; r++) 
{
    results.Add(TestConfig(sqs, new Photon(r, 0, Dir.East)));
    results.Add(TestConfig(sqs, new Photon(r, lines[0].Length - 1, Dir.West)));
}
Console.WriteLine($"P2: {results.Max()}");

namespace Day16
{
    enum Dir { North, South, East, West }
    record Photon(int Row, int Col, Dir Dir);    
}