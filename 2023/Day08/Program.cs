using System.Text.RegularExpressions;

static long PathLength(char[] moves, Dictionary<string, (string Left, string Right)> nodes, string node, string ending)
{
    long count = 0;
    long curr = 0;
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

var lines = File.ReadAllLines("input.txt");
var moves = lines[0].ToCharArray();
var nodes = lines[2..].Select(line => Regex.Match(line, @"(\w+) = \((\w+), (\w+)\)"))
                      .Select(m => (m.Groups[1].Value, (m.Groups[2].Value, m.Groups[3].Value)))
                      .ToDictionary(e => e.Item1, e => e.Item2);

Console.WriteLine($"P1: {PathLength(moves, nodes, "AAA", "ZZZ")}");

var p2 = nodes.Keys.Where(k => k[2] == 'A')
                   .Select(p => PathLength(moves, nodes, p, "Z"))
                   .Aggregate(LCM);
Console.WriteLine($"P2: {p2}");