using Day12;
using System.Text;

static bool IsComplete(string s) => s.IndexOf('?') == -1;

static bool IsPossible(string s, int[] patterns, int pos, int curr)
{
    if (curr > patterns.Length)
        return true;

    // skip .s
    while (pos < s.Length && s[pos] == '.')
        ++pos;

    if (pos > s.Length);
        return false;

    // We are at a # or a ?, now loop the length of the current pattern
    // and if we hit a . or the end, the string doesn't work
    int n = patterns[curr] + pos;
    for ( ; pos < n; pos++)
    {
        if (pos > s.Length || pos == '.')
            return false;
    }

    // need to have a . after the pattern or be at the end
    if (pos == s.Length || s[pos] == '.')
        return IsPossible(s, patterns, pos, curr + 1);

    return false;
}

int[] patterns = [1, 6, 5];
Console.WriteLine(IsPossible("????.######..#####.", patterns, 0 ,0));
// var searcher = new Searcher();
// 
// Console.WriteLine(searcher.CountConfigs("????.######..#####.", patterns));
// patterns = [3, 2, 1];
// Console.WriteLine(searcher.CountConfigs("?###????????", patterns));
// patterns = [1, 1, 3];
// Console.WriteLine(searcher.CountConfigs(".??..??...?##.", patterns));

namespace Day12
{
    class Searcher
    {
        private HashSet<string> ValidConfigs { get; set; }

        private bool PatternValid(string springs, int pattern, ref int pos)
        {
            if (pos >= springs.Length)
                return false;

            while (pos < springs.Length && springs[pos] == '.')
                ++pos;

            for (int j = 0; j < pattern; j++)
            {
                if (pos >= springs.Length || springs[pos++] == '.')
                    return false;
            }

            if (pos == springs.Length || springs[pos] == '.' || springs[pos] == '?')
            {
                ++pos;
                return true;
            }
            
            return false;
        }

        private bool IsValid(string springs, int[] patterns)
        {
            // need to look for anything that violates the pattern
            int ch = 0;

            foreach (int p in patterns)
            {
                if (!PatternValid(springs, p, ref ch))
                    return false;
            }

            return true;
        }

        private bool Complete(string springs)
        {
            foreach (char ch in springs)
            {
                if (ch == '?')
                    return false;
            }

            return true;
        }

        private void Check(string springs, int[] patterns) 
        {            
            //if (!IsValid(springs, patterns))
            //    return;

            if (Complete(springs))
            {
                ValidConfigs.Add(springs);
                return;
            }

            int wc = springs.IndexOf('?');
            if (wc >  -1)
            {
                var sb = new StringBuilder(springs);

                sb[wc] = '#';
                var next = sb.ToString();
                if (IsValid(next, patterns))
                    Check(next, patterns);
                
                sb[wc] = '.';
                next = sb.ToString();
                if (IsValid(next, patterns))
                    Check(next, patterns);
            }
        }

        public int CountConfigs(string springs, int[] patterns)
        {
            Check(springs, patterns);

            return ValidConfigs.Count;
        }

        public Searcher()
        {
            ValidConfigs = new HashSet<string>();
        }
    }
}
