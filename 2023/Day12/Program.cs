
using System.Text;

using Day12;

var lines = File.ReadAllLines("input.txt");
var checker = new Checker();
int p1 = lines.Select(checker.Arrangements).Sum();
Console.WriteLine($"P1: {p1}");

namespace Day12
{
    class Checker
    {
        private int Count { get; set; }

        public int Arrangements(string line)
        {
            Count = 0;
            string s = line.Split(' ')[0];
            int[] patterns = line.Split(' ')[1].Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            FindArragements(s, patterns);

            return Count;
        }

        void FindArragements(string s, int[] patterns)
        {
            int q = s.IndexOf('?');
            if (q == -1 && MaybeValid(s, patterns)) 
            {
                ++Count;
                return;
            }

            var sb = new StringBuilder(s);
            sb[q] = '#';
            string s1 = sb.ToString();
            if (MaybeValid(s1, patterns))
                FindArragements(s1, patterns);
            sb[q] = '.';
            string s2 = sb.ToString();
            if (MaybeValid(s2, patterns))
                FindArragements(s2, patterns);
        }

        static bool MaybeValid(string s, int[] patterns)
        {
            int pos = 0;
            foreach (int p in patterns)
            {
                int loc = PatternInString(s, pos, p);
                if (loc == -1)
                    return false;

                // Need to check if everything between pos + 1 and loc - 1 is . or ?
                // (this feels dumb but I can't think of a better way atm)
                if (pos > 0)
                {
                    for (int j = pos; j < loc - 1; j++) 
                    {
                        if (s[j] == '#')
                            return false;
                    }
                }

                // Gotta have a . after the pattern
                if (loc + p <= s.Length - 1 && s[loc + p] == '#')
                    return false;

                pos = loc + p + 1;
            }

            // We've checked all the patterns, need to make sure all the rest of the 
            // string is . or ?
            while (pos < s.Length)
            {
                if (s[pos++] == '#')
                    return false;
            }

            return true;
        }

        static int PatternInString(string s, int pos, int length)
        {            
            while (pos < s.Length - length + 1)
            {
                if (s[pos] == '#' || s[pos] == '?')
                {
                    for (int c = pos; c < pos + length; c++)
                    {
                        if (s[c] == '.')
                            goto keep_going;
                    }
                    return pos;
                }
keep_going:
                ++pos;
            }

            return -1;
        }
    }
}

