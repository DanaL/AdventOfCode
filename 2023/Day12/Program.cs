
using System.Text;

using Day12;

//var lines = File.ReadAllLines("input.txt");
var checker = new Checker();
//int p1 = lines.Select(checker.Arrangements).Sum();
//Console.WriteLine($"P1: {p1}");

//Console.WriteLine(checker.Arrangements(".?????.????.??? 3,1"));

Console.WriteLine(checker.Arrangements("??????????#??? 4,4,1"));


namespace Day12
{
    class Checker
    {
        private HashSet<string> ValidStrings { get; set; }

        public Checker() => ValidStrings = new HashSet<string>();

        public int Arrangements(string line)
        {            
            string s = line.Split(' ')[0];
            int[] patterns = line.Split(' ')[1].Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            // https://www.reddit.com/r/adventofcode/comments/18ihhhm/2023_day_12_part_1_c_im_at_my_wits_end/
            Console.WriteLine(IsValid("####.####.#...", patterns));
            Console.WriteLine(IsValid("####...####.#.", patterns));
            Console.WriteLine(IsValid("####...####..#", patterns));
            Console.WriteLine(IsValid("####....####.#", patterns));
            Console.WriteLine(IsValid(".####..####.#.", patterns));
            Console.WriteLine(IsValid(".####..####..#", patterns));
            Console.WriteLine(IsValid(".####...####.#", patterns));
            Console.WriteLine(IsValid("..####.####.#.", patterns));
            Console.WriteLine(IsValid("..####.####..#", patterns));
            Console.WriteLine(IsValid("..####..####.#", patterns));
            Console.WriteLine(IsValid("...####.####.#", patterns));

            FindArragements(s, patterns);

            foreach (var v in ValidStrings)
                Console.WriteLine(v);

            return ValidStrings.Count;
        }

        void FindArragements(string s, int[] patterns)
        {
            int q = s.IndexOf('?');
            if (q == -1)
            {
                if (IsValid(s, patterns))
                    ValidStrings.Add(s);
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

        static bool IsValid(string s, int[] patterns)
        {
            int ch = 0;

            foreach (int p in patterns)
            {
                while (ch < s.Length && s[ch] != '#')
                    ++ch;

                int start = ch;
                while (ch < s.Length && s[ch] == '#')
                    ++ch;
                if (ch - start != p)
                    return false;
            }

            // We've found all our patterns to make sure there are no extra #s
            // at the end of string.
            // ie: ####..##..#.# 4,2,1 is invalid
            while (ch < s.Length)
            {
                if (s[ch++] == '#')
                    return false;
            }

            return true;
        }

        // All we want to do is see if it's possible to fit the patterns into the string
        static bool MaybeValid(string s, int[] patterns)
        {
            var blocks = new List<int>();
            int count = 0;
            for (int ch = 0; ch < s.Length; ch++)
            {
                if (s[ch] == '.' && count > 0)
                {
                    blocks.Add(count);
                    count = 0;
                }
                else
                {
                    ++count;
                }
            }

            if (count > 0)
                blocks.Add(count);

            if (blocks.Count == 0)
                return false;
                        
            int avail = blocks[0];
            int b = 1;
            foreach (int p in patterns)
            {
                if (avail < p && b < blocks.Count)
                    avail = blocks[b++];
                
                if (avail < p)
                    return false;

                avail -= p + 1;
            }

            return true;
        }
    }
}

