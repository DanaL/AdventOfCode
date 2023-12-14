using System.Text;

static void MoveRocks(char[,] map, int size)
{
    for (int r = 1; r < size; r++)
    {
        for (int c = 0; c < size; c++)
        {
            if (map[r, c] == 'O')
            {
                int nr = r;
                while (nr - 1 >= 0 && map[nr - 1, c] == '.') 
                {
                    map[nr - 1, c] = 'O';
                    map[nr, c] = '.';
                    --nr;
                }
            }
        }
    }
}

static int CountLoad(char[,] map, int size)
{
    int load = 0;
    for (int r = 0; r < size; r++) 
    {
        for (int c = 0; c < size; c++) 
        {
            if (map[r, c] == 'O')
                load += size - r;
        }
    }

    return load;
}

var input = File.ReadAllLines("input.txt");
int size = input.Length;

char[,] map = new char[size, size];

for (int r = 0; r < size; r++) 
{
    for (int c = 0; c < size; c++) 
    {
        map[r, c] = input[r][c];
    }
}

MoveRocks(map, size);
Console.WriteLine($"P1: {CountLoad(map, size)}");

// for (int r = 0; r < size; r++) 
// {
//     var sb = new StringBuilder();
//     for (int c = 0; c < size; c++) 
//         sb.Append(map[r, c]);
//     Console.WriteLine(sb.ToString());
// }
