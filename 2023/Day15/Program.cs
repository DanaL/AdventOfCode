static int Hash(string s)
{
    int v = 0;
    foreach (char c in s)
    {
        v += (int)c;
        v *= 17;
        v %= 256;
    }

    return v;
}

var steps = File.ReadAllText("input.txt").Trim().Split(',');
Console.WriteLine($"P1: {steps.Select(Hash).Sum()}");

static void UpdateBox(Dictionary<int, List<(string, int)>> boxes, string instruction)
{   
    int eq = instruction.IndexOf('=');
    if (eq > -1)
    {
        string label = instruction.Substring(0, eq);
        int box = Hash(label);
        int i = boxes[box].FindIndex(p => p.Item1 == label);
        if (i == -1)
            boxes[box].Add((label, int.Parse(instruction.Substring(eq + 1))));
        else
            boxes[box][i] = (label, int.Parse(instruction.Substring(eq + 1)));
    }
    else
    {
        string label = instruction.Substring(0, instruction.IndexOf('-'));
        int box = Hash(label);
        boxes[box].RemoveAll(p => p.Item1 == label);
    }
}

static int CalcFocusPower(Dictionary<int, List<(string, int)>> boxes)
{
    int fp = 0;
    for (int j = 0; j < 256; j++)
    {
        int sum = 0;
        for (int k = 0; k < boxes[j].Count; k++)
            sum += (k + 1) * boxes[j][k].Item2;
        fp += (j + 1) * sum;
    }

    return fp;
}

var boxes = new Dictionary<int, List<(string, int)>>();
for (int j = 0; j < 256; j++)
    boxes[j] = new List<(string, int)>();

foreach (string s in steps)
    UpdateBox(boxes, s);

Console.WriteLine($"P2: {CalcFocusPower(boxes)}");

