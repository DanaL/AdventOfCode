static ((int, int), Dictionary<(int, int), ((int, int), (int, int))>) FetchMaze()
{
    var sqs = new Dictionary<(int, int), ((int, int), (int, int))>();
    var lines = File.ReadAllLines("input.txt");
    (int, int) start = (-1, -1);

    for (int r = 0; r < lines.Length; r++) 
    {
        for (int c = 0; c < lines[r].Length; c++)
        {
            if (lines[r][c] == '.')
                continue;

            if (lines[r][c] == 'S')
                start = (r, c);
            else 
            {
                //var exits = ASCIIToCoords(lines[r][c]);
                var exits = lines[r][c] switch
                {
                    '|' => ((-1, 0), (1, 0)),
                    '-' => ((0, -1), (0, 1)),
                    'L' => ((-1, 0), (0, 1)),
                    'J' => ((-1, 0), (0, -1)),
                    '7' => ((1, 0), (0, -1)),
                    'F' => ((1, 0), (0, 1)),
                    _ => throw new Exception("Hmm this shouldn't happen")
                };
                sqs.Add((r, c), ((r + exits.Item1.Item1, c + exits.Item1.Item2), (r + exits.Item2.Item1, c + exits.Item2.Item2)));
            }
        }            
    }

    // Add the connections for the start square, ie the two squares that connect to it
    var startAdj = sqs.Keys.Where(k => sqs[k].Item1 == start || sqs[k].Item2 == start).ToList();
    sqs.Add(start, (startAdj[0], startAdj[1]));

    return (start, sqs);
}

static int FollowTunnels((int, int) start, Dictionary<(int, int), ((int, int), (int, int))> sqs)
{
    int steps = 1;
    (int, int) a = sqs[start].Item1;
    (int, int) b = sqs[start].Item2;
    (int, int) prevA = start, prevB = start;

    do 
    {
        var nextA = sqs[a].Item1 == prevA ? sqs[a].Item2 : sqs[a].Item1;
        var nextB = sqs[b].Item1 == prevB ? sqs[b].Item2 : sqs[b].Item1;
        prevA = a;
        prevB = b;
        a = nextA;
        b = nextB;
        ++steps;
    } while (a != b);

    return steps;
}

var (start, sqs) = FetchMaze();
Console.WriteLine($"P1 {FollowTunnels(start, sqs)}");
