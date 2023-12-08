using System.Text.RegularExpressions;
using Day08;

static int Part1() 
{
    var lines = File.ReadAllLines("input.txt");
    var moves = lines[0].ToCharArray();
    var r = new Regex(@"(\w+) = \((\w+), (\w+)\)");
    var nodes = lines[2..].Select(line => r.Match(line))
                        .Select(m => (m.Groups[1].Value, new Exits(m.Groups[2].Value, m.Groups[3].Value)))
                        .ToDictionary(e => e.Item1, e => e.Item2);

    int count = 0;
    string node = "AAA";
    int curr = 0;
    while (node != "ZZZ") 
    {
        node = moves[curr] == 'L' ? nodes[node].Left : nodes[node].Right;
        curr = (curr + 1) % moves.Length;
        ++count;
    }

    return count;
}

Console.WriteLine($"P1: {Part1()}");

namespace Day08
{
    record Exits(string Left, string Right);
}
