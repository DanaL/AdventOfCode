using System.Text;
using Day12;

static bool IsComplete(string s) => s.IndexOf('?') == -1;

static int PatternInString(string s, int pos, int length)
{
    while (pos < s.Length - length + 1)
    {
        if (s[pos] == '#' || s[pos] == '?')
        {
            for (int c = pos; c < pos + length; c++)
            {
                if (s[c] == '.')
                    return -1;
            }
            return pos;
        }
        ++pos;
    }

    return -1;
}

static bool MaybeValid(string s, int[] patterns)
{
    int pos = 0;
    foreach (int p in patterns)
    {
        pos = PatternInString(s, pos, p);
        if (pos == -1)
            return false;
        pos += p + 1;
    }

    return true;
}

int[] patterns = [2, 1, 6, 5];
Console.WriteLine(MaybeValid("??.#.######..#####.", patterns));
//Console.WriteLine(IsPossible("????.######..#####.", patterns, 0 , 0));
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
