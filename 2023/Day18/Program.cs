static void Part1()
{
    var instrs = File.ReadAllLines("input.txt").Select(l => l.Split(' '))
                     .Select(l => (l[0][0], int.Parse(l[1]))).ToList();
    HashSet<(int, int)> sqs = [(0, 0)];
    List<(int, int)> neighours = [ (0, 1), (0, -1), (-1, 0), (1, 0)];

    int r = 0, c = 0;
    foreach (var instr in instrs)
    {
        (int Row, int Col) dir = instr.Item1 switch
        {
            'R' => neighours[0],
            'L' => neighours[1],
            'U' => neighours[2],
            _ => neighours[3]
        };
        for (int n = 0; n < instr.Item2; n++, r+= dir.Row, c += dir.Col)
            sqs.Add((r, c));
    }

    // Fill in interior (not strictly needed for pt 1 but I assume I'll need to for pt 2)    
    var q = new Queue<(int Row, int Col)>();
    q.Enqueue((1, 1));
    while (q.Count > 0)
    {
        var sq = q.Dequeue();
        if (sqs.Contains(sq))
            continue;        
        sqs.Add(sq);
        foreach (var n in neighours)
        {
            var next = (sq.Item1 + n.Item1, sq.Item2 + n.Item2);
            if (!sqs.Contains(next)) 
                q.Enqueue(next);            
        }
    }

    Console.WriteLine($"P1: {sqs.Count}");
}

Part1();