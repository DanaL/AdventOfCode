namespace Day02;

class Game 
{
    public int ID { get; set; }
    public List<(int, int, int)> Rounds { get; set; }

    public Game() => Rounds = new List<(int, int, int)>();
}

class Program
{
    static Game ParseLine(string line) 
    {
        int sc = line.IndexOf(':');
        int gameID = int.Parse(line.Substring(5, sc - 5));
        var game = new Game() { ID = gameID };

        foreach (var piece in line.Substring(sc + 1).Trim().Split(';').Select(p => p.Trim())) 
        {
            var round = (0, 0, 0);
            foreach (var color in piece.Split(',').Select(c => c.Trim())) 
            {
                int sp = color.IndexOf(' ');
                int v = int.Parse(color.Substring(0, sp));
                string c = color.Substring(sp + 1);
                if (c == "red" && v > round.Item1)
                    round.Item1 = v;
                else if (c == "green" && v > round.Item2)
                    round.Item2 = v;
                else if (c == "blue" && v > round.Item3)
                    round.Item3 = v;                
            }

            game.Rounds.Add(round);
        }

        return game;
    }

    static bool ValidGame(Game game, int red, int green, int blue) 
    {
        foreach (var round in game.Rounds) 
        {
            if (round.Item1 > red || round.Item2 > green || round.Item3 > blue)
                return false;
        }

        return true;
    }

    static void Part1(string[] lines) 
    {
        var sum = lines.Select(l => ParseLine(l))
                        .Where(g => ValidGame(g, 12, 13, 14))
                        .Select(g => g.ID)
                        .Sum();
        
        Console.WriteLine($"P1: {sum}");
    }

    static int PowerSet(Game game) 
    {
        int red = 0; int green = 0; int blue = 0;

        foreach (var round in game.Rounds)
        {
            if (round.Item1 > red) red = round.Item1;
            if (round.Item2 > green) green = round.Item2;
            if (round.Item3 > blue) blue = round.Item3;
        }

        return red * green * blue;
    }

    static void Part2(string[] lines)
    {
        var sum = lines.Select(l => PowerSet(ParseLine(l)))
                       .Sum();
        Console.WriteLine($"P2: {sum}");
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        Part1(lines);
        Part2(lines);
    }
}
