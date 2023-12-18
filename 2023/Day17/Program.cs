var lines = File.ReadAllLines("input.txt");
int size = lines.Length;
int[,] map = new int[size, size];

for (int r = 0; r < size; r++)
{
    for (int c = 0; c < size; c++)
        map[r, c] = Convert.ToInt32(lines[r][c].ToString());
}

Console.WriteLine(map[3,7]);