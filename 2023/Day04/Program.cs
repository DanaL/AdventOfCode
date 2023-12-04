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
        return playerNums.Where(n => winningNums.Contains(n)).Count();        
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var winningNums = lines.Select(line => ScoreCard(line)).ToList();
        var p1 = winningNums.Select(w => w == 0 ? 0 : (long) Math.Pow(2, w - 1)).Sum();
        Console.WriteLine($"P1: {p1}");

        foreach (var x in winningNums) Console.WriteLine(x);
    }
}
