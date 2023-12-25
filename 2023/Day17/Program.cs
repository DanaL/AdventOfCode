
using Day17;

static int Manhattan((int, int) a, (int, int) b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);

static List<Route> Neighbours2(Route route, int size)
{
    var neighbours = new List<Route>();
    (int, int)[] adj = new (int, int)[] { (-1, 0), (1, 0), (0, 1), (0, -1) };

    foreach (var a in adj)
    {
        (int, int) n = (route.Head.Item1 + a.Item1, route.Head.Item2 + a.Item2);
        if (n.Item1 < 0 || n.Item2 < 0 || n.Item1 >= size || n.Item2 >= size)
            continue;

        // We can't go backwards
        if (route.Nodes.Count > 2)
        {
            var secondLast = route.Nodes[route.Nodes.Count - 2];
            if (n == secondLast)
                continue;
        }

        char ch = a switch
        {
            (-1, 0) => '^',
            (1, 0) => 'v',
            (0, -1) => '<',
            _ => '>'
        };

        // If we've gone the same direction 3 times in a row, we can't go
        // in that direction again
        var moves = route.Moves.TakeLast(3)
                         .Where(mv => mv == ch).Count();
        if (moves >= 3)
            continue;

        var neighbour = route.Copy();
        neighbour.Nodes.Add(n);
        neighbour.Moves.Add(ch);
        neighbours.Add(neighbour);
    }

    return neighbours;
}

static void DumpPath(int[,] map, int size, Route goal)
{
    var goalPath = new Dictionary<(int, int), char>();
    for (int m = 0; m < goal.Moves.Count; m++)
    {
        goalPath.Add(goal.Nodes[m + 1], goal.Moves[m]);
    }

    for (int r = 0; r < size; r++)
    {
        for (int c = 0; c < size; c++)
        {
            if (goalPath.ContainsKey((r, c)))
                Console.Write(goalPath[(r, c)]);
            else
                Console.Write(map[r, c]);
        }
        Console.WriteLine("");
    }
}

static int Pathfind2(int[,] map, int size)
{
    var goal = (size - 1, size -1);
    var q = new PriorityQueue<Route, int>();
    var start = new Route();
    start.Nodes.Add((0, 0));
    q.Enqueue(start, 0);
    var costs = new Dictionary<(int, int), int>() { { (0, 0), 2 } };
    int cheapest = int.MaxValue;

    while (q.Count > 0)
    {
        var curr = q.Dequeue();

        if (curr.Cost >= cheapest)
            continue;

        if (curr.Head == goal)
        {
            if (curr.Cost < cheapest)
                cheapest = curr.Cost;
            Console.WriteLine($"hurrah! {curr.Cost}");
            var pathCost = curr.Nodes.Select(p => map[p.Item1, p.Item2]).Sum();
            Console.WriteLine($"Path cost: {pathCost}");
            //DumpPath(map, size, curr);
            continue;
        }

        // This is causing me to miss moves it's more expensive to get to
        // a node along one path, but will ultimately be cheaper because
        // of the no three-in-a-row rule so I need to prune fewer routes
        // (but also not be a computational nightmare)
        if (costs.TryGetValue(curr.Head, out int value) && curr.Cost - value > 2)
        {
            continue;
        }

        foreach (var n in Neighbours2(curr, size))
        {
            n.Cost += map[n.Head.Item1, n.Head.Item2];
            if (!costs.ContainsKey(n.Head) || n.Cost < costs[n.Head])
            {
                costs[n.Head] = n.Cost;                
            }
            q.Enqueue(n, n.Cost + Manhattan(n.Head, goal));
        }
    }

    return costs[goal];
}

var lines = File.ReadAllLines("input.txt");
int size = lines.Length;
int[,] map = new int[size, size];

for (int r = 0; r < size; r++)
{
    for (int c = 0; c < size; c++)
        map[r, c] = Convert.ToInt32(lines[r][c].ToString());
}

Console.WriteLine(Pathfind2(map, size));

namespace Day17
{
    class Route
    {
        public List<(int, int)> Nodes { get; set; }
        public List<char> Moves { get; set; }
        public int Cost { get; set; }

        public (int, int) Head => Nodes.Count > 0 ? Nodes.Last() : (-1, -1);

        public Route() 
        {
            Nodes = new List<(int, int)>();
            Moves = new List<char>();
            Cost = 0;
        }

        public Route Copy()
        {
            var copy = new Route
            {
                Nodes = Nodes.Select(n => n).ToList(),
                Moves = Moves.Select(m => m).ToList(),
                Cost = Cost
            };

            return copy;
        }
    }
}