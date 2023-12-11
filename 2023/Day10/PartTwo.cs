namespace Day10 
{
    public class PartTwo
    {        
        private void Scale(int row, int col, char ch, char[,] maze)
        {           
            (int, int, char)[] pattern = ch switch 
            {
                '.' => [ (0,0,'.'), (0,1,'.'), (0,2,'.'),
                         (1,0,'.'), (1,1,'.'), (1,2,'.'),
                         (2,0,'.'), (2,1,'.'), (2,2,'.')],
                'S' => [ (0,0,'.'), (0,1,'S'), (0,2,'.'),
                         (1,0,'S'), (1,1,'S'), (1,2,'S'),
                         (2,0,'.'), (2,1,'S'), (2,2,'.')],
                '|' => [ (0,0,'.'), (0,1,'|'), (0,2,'.'),
                         (1,0,'.'), (1,1,'|'), (1,2,'.'),
                         (2,0,'.'), (2,1,'|'), (2,2,'.')],
                '-' => [ (0,0,'.'), (0,1,'.'), (0,2,'.'),
                         (1,0,'-'), (1,1,'-'), (1,2,'-'),
                         (2,0,'.'), (2,1,'.'), (2,2,'.')],
                'L' => [ (0,0,'.'), (0,1,'|'), (0,2,'.'),
                         (1,0,'.'), (1,1,'+'), (1,2,'-'),
                         (2,0,'.'), (2,1,'.'), (2,2,'.')],
                'J' => [ (0,0,'.'), (0,1,'|'), (0,2,'.'),
                         (1,0,'-'), (1,1,'+'), (1,2,'.'),
                         (2,0,'.'), (2,1,'.'), (2,2,'.')],
                '7' => [ (0,0,'.'), (0,1,'.'), (0,2,'.'),
                         (1,0,'-'), (1,1,'+'), (1,2,'.'),
                         (2,0,'.'), (2,1,'|'), (2,2,'.')],
                'F' => [ (0,0,'.'), (0,1,'.'), (0,2,'.'),
                         (1,0,'.'), (1,1,'+'), (1,2,'-'),
                         (2,0,'.'), (2,1,'|'), (2,2,'.')],
                _ => throw new Exception("hmm this shouldn't happen")
            };

            foreach (var p in pattern)             
                maze[row + p.Item1, col + p.Item2] = p.Item3;
        }

        private char[,] FetchMaze()
        {
            var lines = File.ReadAllLines("input.txt");
            int length = lines.Length;
            int width = lines[0].Length;

            // I'm going to put a border of .s around the actual maze to make 
            // the flood fill simpler (if longer)
            char[,] maze = new char[(length + 1) * 3, (width + 1) * 3];
            for (int r = 0; r < maze.GetLength(0) - 1; r++) 
            {
                for (int c = 0; c < maze.GetLength(1) - 1; c++) 
                {
                    maze[r, c] = '.';
                }
            }
           
            for (int r = 0; r < length; r++) 
            {
                for (int c = 0; c < width; c++)
                {
                    Scale( r * 3 + 1, c * 3 + 1, lines[r][c], maze);                    
                }                
            }

            return maze;
        }

        private void Dump(char[,] maze) 
        {
            var length = maze.GetLength(0);
            var width = maze.GetLength(1);

            for (int r = 0; r < length; r++) 
            {
                string line = "";
                for (int c = 0; c < width; c++)
                    line += maze[r, c];
                Console.WriteLine(line);
            }
        }

        private void FloodFill(char[,] maze) 
        {
            var neighbours = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
            var length = maze.GetLength(0);
            var width = maze.GetLength(1);
            var locs = new Queue<(int, int)>();
            locs.Enqueue((0, 0));
            var visited = new HashSet<(int, int)>();
            
            do 
            {
                var loc = locs.Dequeue();
                if (visited.Contains(loc))
                    continue;
                maze[loc.Item1, loc.Item2] = 'o';
                visited.Add((loc.Item1, loc.Item2));

                foreach (var n in neighbours)
                {                    
                    var nr = loc.Item1 + n.Item1;
                    var nc = loc.Item2 + n.Item2;
                    if (nr < 0 || nr >= length || nc < 0 || nc >= width || visited.Contains((nr, nc)))
                        continue;
                    if (maze[nr, nc] == ' ' || maze[nr, nc] == '.')
                        locs.Enqueue((nr, nc));
                }
            } 
            while (locs.Count > 0);
        }

        private int CountInner(char[,] maze)
        {
            var length = maze.GetLength(0) - 1;
            var width = maze.GetLength(1) - 1;
            int count = 0;
            var pixels = new (int, int)[] { (0, 0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2), (2, 0), (2, 1), (2, 2) };

            for (int r = 1; r < length; r += 3) 
            {
                for (int c = 1; c < width; c += 3)
                {
                    bool isInner = true;
                    foreach (var p in pixels) 
                    {
                        if (maze[r + p.Item1, c + p.Item2] == 'o')
                        {
                            isInner = false;
                            break;
                        }
                    }
                    count += isInner ? 1 : 0;
                }                    
            }

            return count;
        }

        public void Solve() 
        {
            var maze = FetchMaze();
            FloodFill(maze);
            //Dump(maze);

            Console.WriteLine($"P2: {CountInner(maze)}");
        }
    }
}