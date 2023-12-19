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

// xb{m<2340:gjx,m<2911:shg,s<293:qz,bqc}
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

var step = new Step(Field.M, "gjx", Op.LT, 2340);

var part = new Dictionary<Field, int>();
part.Add(Field.M, 1000);
part.Add(Field.X, 1700);
part.Add(Field.A, 3400);
part.Add(Field.S, 37);

var (n, steps) = ParseRule("px{a<2006:qkq,m>2090:A,rfg}");
foreach (var s in steps)
    Console.WriteLine(s);

namespace Day19
{
    enum Op { LT, GT, Pipe }
    enum Field { X, M, A, S, Any }
    record Step(Field In, string Out, Op Op, int Val);
}