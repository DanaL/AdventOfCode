using AoC;

public class Day01 : IDay 
{    
    bool SubstringMatch(string s, int startPos, string target) 
    {        
        if (s.Length < startPos + target.Length)
            return false;

        return target == s.Substring(startPos, target.Length);
    }

    string FindNumber(string s, List<string> words) 
    {
        for (int c = 0; c < s.Length; c++) 
        {
            foreach (var w in words) 
            {
                if (SubstringMatch(s, c, w))
                    return w;
            }
        }

        return ""; // this shouldn't happen :o
    }

    string ToDigits(string num) 
    {
        return num switch
        {
            "zero" => "0",
            "one" => "1",
            "two" => "2",
            "three" => "3",
            "four" => "4",
            "five" => "5",
            "six" => "6",
            "seven" => "7",
            "eight" => "8",
            "nine" => "9",
            _ => num,
        };
    }

    int CalcLine(string line, List<string> words) 
    {
        var reversed = words.Select(w => w.Reversed()).ToList();
        var x = ToDigits(FindNumber(line, words));
        var y = ToDigits(FindNumber(line.Reversed(), reversed).Reversed());

        return int.Parse($"{x}{y}");
    }

    public void Solve()
    {
        List<string> words = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };                                                    
        var lines = File.ReadAllLines("input.txt");

        var p1 = lines.Select(line => CalcLine(line, words)).Sum();
        Console.WriteLine($"P1: {p1}");

        words.AddRange(new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "zero" });
        var p2 = lines.Select(line => CalcLine(line, words)).Sum();
        Console.WriteLine($"P2: {p2}");
    }
}