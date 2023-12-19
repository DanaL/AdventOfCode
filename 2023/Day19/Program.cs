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
        Op.Pipe => steps[s].Out,
        _ => throw new Exception("Hmm this shouldn't happen")
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

static void PartOne(Dictionary<string, Step[]> rules, List<Dictionary<Field, int>> parts)
{
    var p1 = parts.Where(p => TestPart(p, rules))
               .Select(p => p.Values.Sum())
               .Sum();
    Console.WriteLine($"P1: {p1}");
}

static List<(Dictionary<Field, PRange>, string, int)> SplitRange(Dictionary<Field, PRange> parts, Step step, string stepName, int stepNum)
{   
    var result = new List<(Dictionary<Field, PRange> parts, string, int)>();
    var pass = parts.ToDictionary(e => e.Key, e => e.Value);
    var fail = parts.ToDictionary(e => e.Key, e => e.Value);
    int lo, hi;

    switch (step.Op)
    {
        case Op.LT:
            lo = parts[step.In].Lo;
            hi = parts[step.In].Hi;
            pass[step.In] = new PRange(lo, step.Val - 1);
            fail[step.In] = new PRange(step.Val, hi);
            result.Add((pass, step.Out, 0));
            result.Add((fail, stepName, stepNum + 1));
            break;
        case Op.GT:
            lo = parts[step.In].Lo;
            hi = parts[step.In].Hi;
            pass[step.In] = new PRange(step.Val + 1, hi);
            fail[step.In] = new PRange(lo, step.Val);
            result.Add((pass, step.Out, 0));
            result.Add((fail, stepName, stepNum + 1));
            break;
        case Op.Pipe:
            result.Add((parts, step.Out, 0));
            break;
    }
    
    return result;
}

static void PartTwo(Dictionary<string, Step[]> rules)
{
    var complete = new List<Dictionary<Field, PRange>>();    
    var initial = new Dictionary<Field, PRange>()
    {
        { Field.X, new PRange(1, 4000) }, { Field.M, new PRange(1, 4000) },
        { Field.A, new PRange(1, 4000) }, { Field.S, new PRange(1, 4000) }
    };
    var q = new Queue<(Dictionary<Field, PRange>, string, int)>();
    q.Enqueue((initial, "in", 0));
   
    while (q.Count > 0)
    {
        var (parts, name, stepNum) = q.Dequeue();
        var rule = rules[name];
        var step = rule[stepNum];
        foreach (var res in SplitRange(parts, step, name, stepNum))
        {
            if (res.Item2 == "A")
                complete.Add(res.Item1);
            else if (res.Item2 != "R")
                q.Enqueue(res);            
        }
    }

    ulong total = 0UL;
    foreach (var r in complete)
    {
        ulong a = Convert.ToUInt64(r[Field.X].Hi - r[Field.X].Lo + 1);
        ulong b = Convert.ToUInt64(r[Field.M].Hi - r[Field.M].Lo + 1);
        ulong c = Convert.ToUInt64(r[Field.A].Hi - r[Field.A].Lo + 1);
        ulong d = Convert.ToUInt64(r[Field.S].Hi - r[Field.S].Lo + 1);
        total += a * b * c * d;        
    }
    Console.WriteLine($"P2: {total}");
}

var txt = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);
var rules = txt[0].Split(Environment.NewLine).Select(ParseRule).ToDictionary();
var parts = txt[1].Split(Environment.NewLine).Select(ParsePart).ToList();

PartOne(rules, parts);
PartTwo(rules);

namespace Day19
{
    enum Op { LT, GT, Pipe }
    enum Field { X, M, A, S, Any }
    record Step(Field In, string Out, Op Op, int Val);
    record PRange(int Lo, int Hi);
}