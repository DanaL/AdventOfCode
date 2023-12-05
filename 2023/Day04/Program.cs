namespace Day04;

class Program
{
    static int ScoreCard(string card) 
    {
        string[] pieces = card.Split('|');
        var winningNums = pieces[0].Substring(pieces[0].IndexOf(':') + 1).Trim().Split(' ')
                                   .Where(n => n != "")
                                   .Select(n => int.Parse(n)).ToHashSet<int>();
        var playerNums = pieces[1].Trim().Split(' ')
                                  .Select(n => n != "" ? int.Parse(n) : -1).ToList();
        return playerNums.Where(n => winningNums.Contains(n)).Count();        
    }

    static void CountCards(int cardNum, int winners, Dictionary<int, int> counts) 
    {
        for (int j = 1; j <= winners; j++)
        {
            if (counts.ContainsKey(cardNum + j))
                counts[cardNum + j] += counts[cardNum];            
        }
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        // Part One
        var winningNums = lines.Select(line => ScoreCard(line)).ToList();
        var p1 = winningNums.Select(w => w == 0 ? 0 : (int) Math.Pow(2, w - 1)).Sum();
        Console.WriteLine($"P1: {p1}");

        // Part Two
        // Duh, I don't actually need a dictionary since I could just use the array index
        // as the card number but I'm too lazy to change it now...
        var counts = new Dictionary<int, int>();
        for (int x = 1; x <= lines.Length; x++) 
        {
            counts.Add(x, 1);            
        }
        for (int j = 0; j < winningNums.Count; j++) 
        {
            if (winningNums[j] > 0)
                CountCards(j + 1, winningNums[j], counts);
        }
        Console.WriteLine($"P2: {counts.Values.Sum()}");
    }
}
