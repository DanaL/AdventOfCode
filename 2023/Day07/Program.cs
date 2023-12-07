using Day07;

// Should I just do a linked list and insertion sort the hands??
// I think this ends up being O(n^2) but that should suffice
// for part 1

var lines = File.ReadAllLines("input.txt");
var pieces = lines[0].Split(' ');
var head = new Hand { Cards = pieces[0], Winnings = int.Parse(pieces[1]) };

foreach (var line in lines[1..]) 
{
    pieces = line.Split(' ');
    head = Insert(head, new Hand { Cards = pieces[0], Winnings = int.Parse(pieces[1]) });
}

var total = 0;
var rank = 1;
var p = head;
while (p is not null) 
{
    total += p.Winnings * rank++;
    p = p.Next;
}

Console.WriteLine($"P1: {total}");

static Hand Insert(Hand head, Hand hand)
{
    if (hand < head) 
    {
        hand.Next = head;
        return hand;
    }

    var prev = head;
    var curr = head;    
    while (curr is not null && hand > curr!)
    {
        prev = curr;
        curr = curr.Next;
    }

    prev.Next = hand;
    hand.Next = curr;

    return head;
}

namespace Day07 {
    enum HandType 
    {
        FiveOfAKind = 6,
        FourOfAKind = 5,
        FullHouse = 4,
        ThreeOfAKind = 3,
        TwoPair = 2,
        Pair = 1,
        HighCard = 0,
        Unknown = -1
    }

    class Hand : IComparable, IComparable<Hand>
    {
        public Hand? Next { get; set; }
        public string? Cards { get; set; }
        public int Winnings { get; set; }

        private HandType _cat;
        public HandType Cat 
        {
            get 
            {
                if (_cat == HandType.Unknown)
                    _cat = Categorize();
                return _cat;
            }
        }

        public Hand()
        {
            _cat = HandType.Unknown;
            Next = null;
        }

        public override int GetHashCode() => Cards!.GetHashCode();

        private HandType Categorize() 
        {
            var counts = new Dictionary<char, int>();
            foreach (char ch in Cards!.ToCharArray()) 
            {
                if (counts.ContainsKey(ch)) 
                    counts[ch]++;
                else 
                    counts.Add(ch, 1);
            }

            var vals = counts.Values.OrderByDescending(v => v).ToArray();
            
            if (vals[0] == 5)
                return HandType.FiveOfAKind;
            else if (vals[0] == 4)
                return HandType.FourOfAKind;
            else if (vals[0] == 3 && vals[1] == 2)
                return HandType.FullHouse;
            else if (vals[0] == 3)
                return HandType.ThreeOfAKind;
            else if (vals[0] == 2 && vals[1] == 2)
                return HandType.TwoPair;
            else if (vals[0] == 2)
                return HandType.Pair;
            else 
                return HandType.HighCard;
        }

        public int CompareTo(object? obj)
        {
            return obj is null ? 1 : CompareTo(obj as Hand);
        }

        static private int CardValue(char c)
        {
            return c switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                '9' => 9,
                '8' => 8,
                '7' => 7,
                '6' => 6,
                '5' => 5,
                '4' => 4,
                '3' => 3,
                _ => 2
            };
        }

        public int CompareTo(Hand? other)
        {
            if (Cat > other!.Cat)
                return 1;
            else if (Cat < other!.Cat) 
                return -1;

            for (int c = 0; c < 5; c++)
            {
                if (CardValue(Cards![c]) < CardValue(other.Cards![c]))
                    return -1;
                else if (CardValue(Cards![c]) > CardValue(other.Cards![c]))
                    return 1;
            }

            return 0;
        }

        public static int Compare(Hand? left, Hand? right)
        {
            return left!.CompareTo(right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Hand other)
            {
                return this.CompareTo(other) == 0;
            }

            return false;
        }

        public static bool operator <(Hand left, Hand right)
        {
            return Compare(left, right) < 0;
        }

        public static bool operator >(Hand left, Hand right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator ==(Hand left, Hand right)
        {
            if (left is null)        
                return right is null;        
            return left.Equals(right);
        }

        public static bool operator !=(Hand left, Hand right)
        {            
            return !(left == right);
        }
    }
}