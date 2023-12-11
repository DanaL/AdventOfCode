static ((int, int), (int, int)) ASCIIToCoords(char ch) 
{
    return ch switch
    {
        '|' => ((-1, 0), (1, 0)),
        '-' => ((0, -1), (0, 1)),
        'L' => ((-1, 0), (0, 1)),
        'J' => ((-1, 0), (0, -1)),
        '7' => ((1, 0), (0, -1)),
        'F' => ((1, 0), (0, 1)),
        _ => throw new Exception("Hmm this shouldn't happen")
    };
}

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
                var exits = ASCIIToCoords(lines[r][c]);
                sqs.Add((r, c), ((r + exits.Item1.Item1, c + exits.Item1.Item2), (r + exits.Item2.Item1, c + exits.Item2.Item2)));
            }
        }            
    }

    // Add the connections for the start square, ie the two squares that connect to it
    var startAdj = sqs.Keys.Where(k => sqs[k].Item1 == start || sqs[k].Item2 == start).ToList();
    sqs.Add(start, (startAdj[0], startAdj[1]));

    return (start, sqs);
}

var (start, sqs) = FetchMaze();
//Console.WriteLine(sqs[(3, 1)]);