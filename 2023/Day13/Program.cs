using System.Text;

static uint ToNum(string line) => Convert.ToUInt32(line.Replace('.', '0').Replace('#', '1').PadLeft(32, '0'), 2);

static bool IsReflection(uint[] lines, int a, int b)
{
    while (a >= 0 && b < lines.Length)
    {
        if (lines[a--] != lines[b++])
            return false;
    }

    return true;
}

static int ReflectionInArray(uint[] arr)
{
    for (int j = 0; j < arr.Length - 1; j++)
    {
        if (arr[j] == arr[j + 1] && IsReflection(arr, j, j + 1)) 
            return j + 1;
    }
    
    return 0;
}

static int CheckForRowReflect(string[] block) => ReflectionInArray(block.Select(line => ToNum(line)).ToArray());

static int CheckForColumnReflection(string[] block)
{
    uint[] cols = new uint[block[0].Length];
    for (int c = 0; c < block[0].Length; c++) 
    {
        StringBuilder col = new StringBuilder();
        for (int r = 0; r < block.Length; r++)
            col.Append(block[r][c]);
        cols[c] = ToNum(col.ToString());
    }

    return ReflectionInArray(cols);
}

var input = File.ReadAllText("input.txt").Split("\n\n").Select(b => b.Split('\n')).ToList();

int p1 = input.Select(CheckForRowReflect).Sum() * 100 + input.Select(CheckForColumnReflection).Sum();
Console.WriteLine($"P1: {p1}");
