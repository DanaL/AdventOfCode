using System.Text;

static void RollNorth(char[,] map, int size)
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

static void RollSouth(char[,] map, int size)
{
    for (int r = size - 1; r >= 0; r--)
    {
        for (int c = 0; c < size; c++)
        {
            if (map[r, c] == 'O')
            {
                int nr = r;
                while (nr + 1 < size && map[nr + 1, c] == '.') 
                {
                    map[nr + 1, c] = 'O';
                    map[nr, c] = '.';
                    ++nr;
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

static void RollWest(char[,] map, int size)
{
    for (int r = 0; r < size; r++)
    {
        for (int c = 1; c < size; c++)
        {
            if (map[r, c] == 'O')
            {
                int nc = c;
                while (nc - 1 >= 0 && map[r, nc - 1] == '.') 
                {
                    map[r, nc - 1] = 'O';
                    map[r, nc] = '.';
                    --nc;
                }
            }
        }
    }
}

static void RollEast(char[,] map, int size)
{
    for (int r = 0; r < size; r++)
    {
        for (int c = size - 1; c >= 0 ; c--)
        {
            if (map[r, c] == 'O')
            {
                int nc = c;
                while (nc + 1 < size && map[r, nc + 1] == '.') 
                {
                    map[r, nc + 1] = 'O';
                    map[r, nc] = '.';
                    ++nc;
                }
            }
        }
    }
}

static void Cycle(char[,] map, int size) 
{
    RollNorth(map, size);
    RollWest(map, size);
    RollSouth(map, size);
    RollEast(map, size);
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

Cycle(map, size);
Cycle(map, size);
Cycle(map, size);
Console.WriteLine($"P1: {CountLoad(map, size)}");

for (int r = 0; r < size; r++) 
{
    var sb = new StringBuilder();
    for (int c = 0; c < size; c++) 
        sb.Append(map[r, c]);
    Console.WriteLine(sb.ToString());
}
