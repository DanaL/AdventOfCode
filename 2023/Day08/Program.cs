using System.Text.RegularExpressions;

static int PathLength(char[] moves, Dictionary<string, (string Left, string Right)> nodes, string node, string ending)
{
    int count = 0;
    int curr = 0;
    while (!node.EndsWith(ending)) 
    {
        node = moves[curr] == 'L' ? nodes[node].Left : nodes[node].Right;
        curr = (curr + 1) % moves.Length;
        ++count;
    }

    return count;
}

static long GCD(long a, long b)
{
    long remainder;

    while (b != 0)
    {
        remainder = a % b;
        a = b;
        b = remainder;
    }

    return a;
}

static long LCM(long a, long b) => (a * b) / GCD(a, b);
static int Part1(char[] moves, Dictionary<string, (string Left, string Right)> nodes) => PathLength(moves, nodes, "AAA", "ZZZ");

static long Part2(char[] moves, Dictionary<string, (string Left, string Right)> nodes) 
{
    var starts = nodes.Keys.Where(k => k[2] == 'A').ToList();
    var paths = starts.Select(p => PathLength(moves, nodes, p, "Z"))
                      .Select(n => (long) n).ToList();
    
    return paths.Skip(1).Aggregate(paths[0], (lcm, n) => LCM(lcm, n));
}

var lines = File.ReadAllLines("input.txt");
var moves = lines[0].ToCharArray();
var nodes = lines[2..].Select(line => Regex.Match(line, @"(\w+) = \((\w+), (\w+)\)"))
                      .Select(m => (m.Groups[1].Value, (m.Groups[2].Value, m.Groups[3].Value)))
                      .ToDictionary(e => e.Item1, e => e.Item2);

Console.WriteLine($"P1: {Part1(moves, nodes)}");
Console.WriteLine($"P2: {Part2(moves, nodes)}");