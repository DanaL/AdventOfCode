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
                'S' => [ (0,0,'S'), (0,1,'S'), (0,2,'S'),
                         (1,0,'S'), (1,1,'S'), (1,2,'S'),
                         (2,0,'S'), (2,1,'S'), (2,2,'S')],
                '|' => [ (0,0,' '), (0,1,'|'), (0,2,' '),
                         (1,0,' '), (1,1,'|'), (1,2,' '),
                         (2,0,' '), (2,1,'|'), (2,2,' ')],
                '-' => [ (0,0,' '), (0,1,' '), (0,2,' '),
                         (1,0,'-'), (1,1,'-'), (1,2,'-'),
                         (2,0,' '), (2,1,' '), (2,2,' ')],
                'L' => [ (0,0,' '), (0,1,'|'), (0,2,' '),
                         (1,0,' '), (1,1,'+'), (1,2,'-'),
                         (2,0,' '), (2,1,' '), (2,2,' ')],
                'J' => [ (0,0,' '), (0,1,'|'), (0,2,' '),
                         (1,0,'-'), (1,1,'+'), (1,2,' '),
                         (2,0,' '), (2,1,' '), (2,2,' ')],
                _ => throw new Exception("hmm this shouldn't happen")
            };

            foreach (var p in pattern)             
                maze[row + p.Item1, col + p.Item2] = p.Item3;
        }

        private void FetchMaze()
        {
            var lines = File.ReadAllLines("input.txt");
            int length = lines.Length;
            int width = lines[0].Length;

            char[,] maze = new char[length, width];

            for (int r = 0; r < length; r++) 
            {
                for (int c = 0; c < width; c++)
                {
                    Scale(r, c, lines[r][c], maze);

                    Console.WriteLine($"{maze[1, 1]}");
                    return;
                }
            }
        }

        public void Solve() 
        {
            FetchMaze();
            Console.WriteLine("Hmm don't know yet");
        }
    }
}