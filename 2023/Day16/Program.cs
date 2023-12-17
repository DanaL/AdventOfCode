using Day16;

static void PrintEnergized(HashSet<Photon> paths, Dictionary<(int, int), char> sqs, int length, int width)
{
    var visited = paths.Select(p => (p.Row, p.Col)).ToHashSet();
    for (int r = 0; r < length; r++)
    {
        string s = "";
        for (int c = 0; c < width; c++) 
        {
            if (visited.Contains((r, c)))
                s += '*';
            else
                s += sqs[(r, c)];
        }
        Console.WriteLine(s);
    }
}

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

        // The very first square in my input is a \, which I am not correctly handling.
        // So maybe I have to handle the effects of the current square, then move the 
        // photons
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

var lines = File.ReadAllLines("input.txt");
var sqs = new Dictionary<(int, int), char>();

for (int r = 0; r < lines.Length; r++)
{
    for (int c = 0; c < lines[r].Length; c++)
        sqs.Add((r, c), lines[r][c]);
}

var start = new Photon(0, 0, Dir.East);
HashSet<Photon> paths = new();

FollowBeam(sqs, paths, start);
int pt1 = paths.Select(ph => (ph.Row, ph.Col))
               .Distinct()
               .Count();
Console.WriteLine($"P1: {pt1}");
//PrintEnergized(paths, sqs, lines.Length, lines[0].Length);

namespace Day16
{
    enum Dir { North, South, East, West }
    record Photon(int Row, int Col, Dir Dir);    
}