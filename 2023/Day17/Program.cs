
using Day17;

static int Manhattan((int, int) a, (int, int) b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);

static List<Node> Neighbours(Node node, int size)
{
    var neighbours = new List<Node>();

    //(int, int)[] adj = [ (-1, 0), (1, 0), (0, 1), (0, -1)];
    (int, int)[] adj = new (int, int)[] { (-1, 0), (1, 0), (0, 1), (0, -1) };
    foreach (var a in adj)
    {
        (int, int) n = (node.Sq.Item1 + a.Item1, node.Sq.Item2 + a.Item2);        
        var moves = node.Moves.TakeLast(3).ToList();
        
        // If the last 3 moves have been in the same direction, we can't go that way
        if (moves.Where(p => p == a).Count() >= 3 && moves[0] == a)
            continue;

        if (n.Item1 < 0 ||  n.Item2 < 0 || n.Item1 >= size || n.Item2 >= size) 
            continue;

        var path = node.Path.Select(n => n).ToList();
        char ch = a switch
        {
            (-1, 0) => '^',
            (1, 0) => 'v',
            (0, -1) => '<',
            _ => '>'
        };

        path.Add((n.Item1, n.Item2, ch));
        moves.Add(a);
        neighbours.Add(new Node(n, moves, path));
    }

    return neighbours;    
}

static void DumpPath(int[,] map, int size, Node goal)
{
    var goalPath = new Dictionary<(int, int), char>();
    foreach (var p in goal.Path)
    {
        goalPath.Add((p.Item1, p.Item2), p.Item3);
    }

    for (int r = 0; r < size; r++)
    {
        for (int c = 0; c < size; c++)
        {
            if (goalPath.ContainsKey((r, c)))
                Console.Write(goalPath[(r,c)]);
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
    q.Enqueue(new Node((0, 0), new List<(int, int)>(), new List<(int, int, char)>()), 0);
    var costs = new Dictionary<(int, int), int>() { { (0, 0), 0 } };
    
    while (q.Count > 0)
    {
        var curr = q.Dequeue();

        if (curr.Sq == goal)
        {
            Console.WriteLine($"hurrah we're done! {costs[goal]} {costs[curr.Sq]}, cost to (5,11): {costs[(5,11)]}");        
            DumpPath(map, size, curr);

            var pathCost = curr.Path.Select(p => map[p.Item1, p.Item2]).Sum();
            foreach (var p in curr.Path)
                 Console.WriteLine($"{p.Item1},{p.Item2} -> {map[p.Item1, p.Item2]}");
            Console.WriteLine($"Path cost: {pathCost}");
            return costs[goal];            
        }
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

namespace Day17
{
    record Node((int, int) Sq, List<(int, int)> Moves, List<(int, int, char)> Path);
}