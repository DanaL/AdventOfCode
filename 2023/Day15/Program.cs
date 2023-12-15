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