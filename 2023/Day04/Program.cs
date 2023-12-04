namespace Day04;

class Program
{
    static long ScoreCard(string card) 
    {
        string[] pieces = card.Split('|');
        var winningNums = pieces[0].Substring(pieces[0].IndexOf(':') + 1).Trim().Split(' ')
                                   .Where(n => n != "")
                                   .Select(n => long.Parse(n)).ToHashSet<long>();
        var playerNums = pieces[1].Trim().Split(' ')
                                  .Select(n => n != "" ? long.Parse(n) : -1).ToList();
        var winners = playerNums.Where(n => winningNums.Contains(n)).Count();
        
        return winners == 0 ? 0 : (long) Math.Pow(2, winners - 1);
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var p1 = lines.Select(line => ScoreCard(line)).Sum();
        Console.WriteLine($"P1: {p1}");
    }
}
