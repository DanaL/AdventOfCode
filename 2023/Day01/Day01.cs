using AoC;

public class Day01 : IDay 
{
    bool SubstringMatch(string s, int startPos, string target) 
    {        
        if (s.Length < startPos + target.Length)
            return false;

        string sub = s.Substring(startPos, target.Length);

        return target == sub;
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

    public void Solve()
    {
    //    var p1 = File.ReadAllLines(@"input.txt")
    //                      .Select(l => l.ToCharArray().Where(ch => char.IsNumber(ch)))
    //                      .Select(chs => int.Parse($"{chs.First()}{chs.Last()}"))
    //                      .Sum();

    //     Console.WriteLine($"P1: {p1}");
        var words = new List<string>() { "four", "5", "six", "7" };
        //List<string> reversed = words.Select(w => w.ToReverse()).ToList();

        var s = "jsmmgrjsix5zqsnbfmgjlmrptqzvzmjr7brm9";
        var result = FindNumber(s, words);

        Console.WriteLine(result);

        //Console.WriteLine(FindNumber(s, reversed));
        //Console.WriteLine(FindNumber(s.Reverse(), reversed));

        string f = "fuck off";
        Console.WriteLine(f.ToReverse());
    }
}