using System.Text.RegularExpressions;

using Day19;

static Field ToField(char ch)
{
    return ch switch 
    {
        'x' => Field.X,
        'm' => Field.M,
        'a' => Field.A,
        's' => Field.S,
        _ => throw new Exception("Hmm this shouldn't happen")
    };
}

static Dictionary<Field, int> ParsePart(string line)
{    
    var m = Regex.Match(line, @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
    return new Dictionary<Field, int>
    {
        { Field.X, int.Parse(m.Groups[1].Value) }, { Field.M, int.Parse(m.Groups[2].Value) },
        { Field.A, int.Parse(m.Groups[3].Value) }, { Field.S, int.Parse(m.Groups[4].Value) }
    };    
}

static (string, Step[]) ParseRule(string line)
{
    int n = line.IndexOf('{');
    string name = line.Substring(0, n);
    var steps = new List<Step>();
    
    foreach (var p in line[(n+1)..^1].Split(','))
    {
        int sc = p.IndexOf(':');
        if (p.IndexOf('<') > -1) 
        {
            Field f = ToField(p[0]);
            int v = int.Parse(p[(p.IndexOf('<')+1)..sc]);
            steps.Add(new Step(f, p[(sc+1)..], Op.LT, v));
        }
        else if (p.IndexOf('>') > -1)
        {
            Field f = ToField(p[0]);
            int v = int.Parse(p[(p.IndexOf('>')+1)..sc]);
            steps.Add(new Step(f, p[(sc+1)..], Op.GT, v));
        }
        else
        {
            steps.Add(new Step(Field.Any, p, Op.Pipe, -1));
        }
    }
    
    return (name, steps.ToArray());
}

static string Classify(Dictionary<Field, int> part, Step[] steps, int s)
{
    return steps[s].Op switch
    {
        Op.LT => part[steps[s].In] < steps[s].Val ? steps[s].Out : Classify(part, steps, s + 1),
        Op.GT => part[steps[s].In] > steps[s].Val ? steps[s].Out : Classify(part, steps, s + 1),        
        Op.Pipe => steps[s].Out//,
       // _ => throw new Exception("Hmm this shouldn't happen")
    };
}

static bool TestPart(Dictionary<Field, int> part, Dictionary<string, Step[]> rules)
{
    string stage = "in";

    do
    {
        stage = Classify(part, rules[stage], 0);
    }
    while (!(stage == "A" || stage == "R"));
    
    return stage == "A";
}

var txt = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);
var rules = txt[0].Split(Environment.NewLine).Select(ParseRule).ToDictionary();
var parts = txt[1].Split(Environment.NewLine).Select(ParsePart).ToList();

var p1 = parts.Where(p => TestPart(p, rules))
               .Select(p => p.Values.Sum())
               .Sum();

Console.WriteLine($"P1: {p1}");

namespace Day19
{
    enum Op { LT, GT, Pipe }
    enum Field { X, M, A, S, Any }
    record Step(Field In, string Out, Op Op, int Val);
}