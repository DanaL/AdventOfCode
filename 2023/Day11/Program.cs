static ulong Shortest((int, int) start, (int, int) goal, int size, HashSet<int> emptyRows, HashSet<int> emptyCols, ulong expansion)
{
    var neighbours = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    var q = new Queue<(int, int)>();
    q.Enqueue(start);
    var distances = new Dictionary<(int, int), ulong>() { {start, 0}};
    ulong shortest = UInt64.MaxValue;

    do
    {
        var curr = q.Dequeue();
        ulong cost = distances[curr];
        foreach (var n in neighbours)
        {
            var nr = curr.Item1 + n.Item1;
            var nc = curr.Item2 + n.Item2;
            if (nr < 0 || nr >= size || nc < 0 || nc >= size)
                continue;
                        
            ulong delta = emptyCols.Contains(nc) || emptyRows.Contains(nr) ? expansion : 1;
            ulong nextCost = cost + delta;

            if (nextCost > shortest)
                continue;
            if ((nr, nc) == goal && nextCost < shortest) 
            {
                distances[goal] = nextCost;
                shortest = nextCost;
                continue;
            }
            if (!distances.ContainsKey((nr, nc))) 
            {           
                distances.Add((nr, nc), nextCost);
                q.Enqueue((nr, nc));
            }
            else if (nextCost < distances[(nr, nc)]) 
            {
                distances[(nr, nc)] = nextCost;
                q.Enqueue((nr, nc));
            }            
        }
    }
    while (q.Count > 0);

    return distances[goal];
}

static void CalcDistances(ulong expansion)
{
    var lines = File.ReadAllLines("input.txt");
    var size = lines.Length;
    
    // Find the rows & columns we need to 'expand'
    var emptyRows = new HashSet<int>();
    for (int r = 0; r < size; r++) 
    {
        if (!lines[r].ToCharArray().Any(ch => ch != '.'))
            emptyRows.Add(r);
    }
    var emptyCols = new HashSet<int>();
    for (int c = 0; c < size; c++) 
    {
        bool empty = true;
        for (int r = 0; r < size; r++) 
        {
            if (lines[r][c] != '.') 
            {
                empty = false;
                break;
            }            
        }
        if (empty) 
            emptyCols.Add(c);
    }

    var galaxies = new List<(int, int)>();
    for (int r = 0; r < size; r++) 
    {
        for (int c = 0; c < size; c++) 
        {
            if (lines[r][c] == '#')
                galaxies.Add((r, c));
        }
    }
        
    var distances = new Dictionary<((int, int), (int, int)), ulong>();
    var pairs = (from a in galaxies
                 from b in galaxies
                 where a != b
                 select (a, b)).ToList();

    foreach (var p in pairs) 
    {
        if (!distances.ContainsKey(p)) 
        {
            var d = Shortest(p.Item1, p.Item2, size, emptyRows, emptyCols, expansion);
            distances.Add(p, d);
            distances.Add((p.Item2, p.Item1), d);
        }
    }

    ulong total = 0;
    foreach (var d in distances.Values)
        total += d;
    Console.WriteLine($"P1: {total / 2}");
}

CalcDistances(2);
CalcDistances(1_000_000);