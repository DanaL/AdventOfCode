static int Djikstra((int, int) start, (int, int) goal, int size, HashSet<int> emptyRows, HashSet<int> emptyCols, int expansion)
{
    var neighbours = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    var pq = new PriorityQueue<((int, int), int), int>();
    pq.Enqueue((start, 0), 0);
    var visited = new HashSet<(int, int)>();
    var shortest = Int32.MaxValue;

    do 
    {
        var (curr, d) = pq.Dequeue();
        
        if (visited.Contains(curr))
            continue;

        visited.Add(curr);
        
        foreach (var n in neighbours) 
        {
            var nr = curr.Item1 + n.Item1;
            var nc = curr.Item2 + n.Item2;
            if (nr < 0 || nr >= size || nc < 0 || nc >= size || visited.Contains((nr, nc)))
                continue;
            int cost = emptyRows.Contains(nr) || emptyCols.Contains(nc) ? expansion : 1;
            cost += d;

            if ((nr, nc) == goal && cost < shortest)
                shortest = cost;
            
            pq.Enqueue(((nr, nc), cost), cost);
        }
    }
    while (pq.Count > 0);

    return shortest;
}

static void GetInput(int expansion)
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
        
    var distances = new Dictionary<((int, int), (int, int)), int>();
    var pairs = (from a in galaxies
                 from b in galaxies
                 where a != b
                 select (a, b)).ToList();

    foreach (var p in pairs) 
    {
        if (!distances.ContainsKey(p)) 
        {
            var d = Djikstra(p.Item1, p.Item2, size, emptyRows, emptyCols, expansion);
            distances.Add(p, d);
            distances.Add((p.Item2, p.Item1), d);
        }
    }

    Console.WriteLine($"P1: {distances.Values.Sum() / 2}");
}

GetInput(100);