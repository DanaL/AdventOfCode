using Day11;

static Info FetchInput()
{
    var info = new Info();
    var lines = File.ReadAllLines("input.txt");
    info.Size = lines.Length;

    // Find the rows & columns we need to 'expand'
    for (int r = 0; r < info.Size; r++) 
    {
        if (!lines[r].ToCharArray().Any(ch => ch != '.'))
            info.EmptyRows.Add(r);
    }
    
    for (int c = 0; c < info.Size; c++) 
    {
        bool empty = true;
        for (int r = 0; r < info.Size; r++) 
        {
            if (lines[r][c] != '.') 
            {
                empty = false;
                break;
            }            
        }
        if (empty) 
            info.EmptyCols.Add(c);
    }

    // Find the galaxies
    for (int r = 0; r < info.Size; r++) 
    {
        for (int c = 0; c < info.Size; c++) 
        {
            if (lines[r][c] == '#')
                info.Galaxies.Add((r, c));
        }
    }

    return info;
}

static Dictionary<((int, int), (int, int)), ulong> FloodSearch(Info info, (int, int) start)
{    
    ulong[,] distances = new ulong[info.Size, info.Size];
    for (int r = 0; r < info.Size; r++) 
    {
        for (int c = 0; c < info.Size; c++)
            distances[r, c] = UInt64.MaxValue;
    }
    distances[start.Item1, start.Item2] = 0;

    var q = new Queue<(int R, int C)>();
    q.Enqueue(start);

    var neighbours = new (int R, int C)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    do
    {
        var curr = q.Dequeue();

        foreach (var n in neighbours) 
        {
            var nr = curr.R + n.R;
            var nc = curr.C + n.C;

            // make sure neighbour is in bounds
            if (nr < 0 || nr >= info.Size || nc < 0 || nc >= info.Size)
                continue;
            ulong move = info.EmptyRows.Contains(nr) || info.EmptyCols.Contains(nc) ? info.Expansion : 1;
            ulong cost = move + distances[curr.R, curr.C];
            if (cost < distances[nr, nc])
            {
                q.Enqueue((nr, nc));
                distances[nr, nc] = cost;
            }
        }
    }
    while (q.Count > 0);

    var allDistances = new Dictionary<((int, int), (int, int)), ulong>();        
    foreach (var g in info.Galaxies.Where(g => g != start)) 
    {
        allDistances.Add((start, g), distances[g.Item1, g.Item2]);
        allDistances.Add((g, start), distances[g.Item1, g.Item2]);
    }

    return allDistances;
}

static ulong CalcDistances(Info info)
{
    var allDistances = new Dictionary<((int, int), (int, int)), ulong>();

    foreach (var g in info.Galaxies)
    {
        var distances = FloodSearch(info, g);
        foreach (var k in distances.Keys) 
        {
            if (!allDistances.ContainsKey(k))
                allDistances.Add(k, distances[k]);
        }
    }
    
    ulong total = 0;
    foreach (var d in allDistances.Values)
        total += d;
    return total / 2;
}

var info = FetchInput();

info.Expansion = 2;
Console.WriteLine($"P1: {CalcDistances(info)}");
info.Expansion = 1_000_000;
Console.WriteLine($"P2: {CalcDistances(info)}");

namespace Day11
{
    class Info
    {
        public int Size { get; set; }
        public List<(int, int)> Galaxies { get; set; }
        public HashSet<int> EmptyRows { get; set; }
        public HashSet<int> EmptyCols { get; set; }
        public ulong Expansion { get; set; }

        public Info()
        {
            Galaxies = new List<(int, int)>();
            EmptyCols = new HashSet<int>();
            EmptyRows = new HashSet<int>();
        }
    }
}