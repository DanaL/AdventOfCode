
using Day17;

static int Manhattan((int, int) a, (int, int) b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);

static List<Node> Neighbours(Node node, int size)
{
    var neighbours = new List<Node>();
    
    (int, int)[] adj = [ (-1, 0), (1, 0), (0, 1), (0, -1)];

    foreach (var a in adj)
    {
        (int, int) n = (node.Sq.Item1 + a.Item1, node.Sq.Item2 + a.Item2);
        var moves = node.Moves.TakeLast(3).ToList();
        
        // If the last 3 moves have been in the same direction, we can't go that way
        if (moves.Where(p => p == a).Count() >= 3 && (moves[0] == a))
            continue;
        if (n.Item1 < 0 ||  n.Item2 < 0 || n.Item1 >= size || n.Item2 >= size) 
            continue;

        var path = node.Path.Select(n => n).ToList();
        path.Add(n);
        moves.Add(a);
        neighbours.Add(new Node(n, moves, path));
    }

    return neighbours;    
}

static void DumpPath(int[,] map, int size, Node goal)
{
    var goalPath = new HashSet<(int, int)>(goal.Path);
    for (int r = 0; r < size; r++)
    {
        for (int c = 0; c < size; c++)
        {
            if (goalPath.Contains((r, c)))
                Console.Write('*');
            else
                Console.Write(map[r, c]);
        }
        Console.WriteLine("");
    }
}

static int Pathfind(int[,] map, int size)
{
    var goal = (size - 1, size - 1);
    var q = new PriorityQueue<Node, int>();
    q.Enqueue(new Node((0, 0), new List<(int, int)>(), new List<(int, int)>()), 0);
    var costs = new Dictionary<(int, int), int>() { { (0, 0), 0 } };
    
    while (q.Count > 0)
    {
        var curr = q.Dequeue();

        if (curr.Sq == goal)
        {
            Console.WriteLine($"hurrah we're done! {costs[goal]}, cost to (1,5): {costs[(1, 5)]}");
            DumpPath(map, size, curr);
            return costs[goal];            
        }

        if (curr.Sq == (0, 2))
            Console.WriteLine("");
        if (curr.Sq == (1, 2))
            Console.WriteLine("");
        if (curr.Sq == (1, 8))
            Console.WriteLine("");
        foreach (var n in Neighbours(curr, size))
        {
            int nextCost = costs[curr.Sq] + map[n.Sq.Item1, n.Sq.Item2];
            if (!costs.ContainsKey(n.Sq) || nextCost <= costs[n.Sq])
            {
                costs[n.Sq] = nextCost;
                q.Enqueue(n, nextCost + Manhattan(n.Sq, goal));
            }            
        }
    }

    return -1;
}

var lines = File.ReadAllLines("input.txt");
int size = lines.Length;
int[,] map = new int[size, size];

for (int r = 0; r < size; r++)
{
    for (int c = 0; c < size; c++)
        map[r, c] = Convert.ToInt32(lines[r][c].ToString());
}

Console.WriteLine(Pathfind(map, size));

int[] x = [ map[0, 1], map[0, 2], map[1,2], map[1,3], map[1,4], map[1,5], map[0,5]
    , map[0,6], map[0,7], map[0,8], map[1,8], map[2,8], map[2, 9], map[2, 10]
    , map[3, 10], map[4, 10], map[4, 11], map[5, 11], map[6, 11], map[7, 11]
    , map[7, 12], map[8, 12], map[9, 12], map[10, 12], map[10, 11], map[11, 11], map[12, 11], map[12, 12]
        ];
Console.WriteLine($"foo {x.Sum()}");


namespace Day17
{
    record Node((int, int) Sq, List<(int, int)> Moves, List<(int, int)> Path);
}