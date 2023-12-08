using Day07;

// Just did a linked list/insertion sort of the hands
// Then just had to iterate over the sorted hands to calculate the total winnings

static int Solve(HandFactory factory, int jokerValue) {
    Hand.JokerValue = jokerValue;
    var lines = File.ReadAllLines("input.txt");
    var pieces = lines[0].Split(' ');
    var head = factory.Make(pieces[0], pieces[1]);

    foreach (var line in lines[1..]) 
    {
        pieces = line.Split(' ');
        head = Insert(head, factory.Make(pieces[0], pieces[1]));
    }

    var total = 0;
    var rank = 1;
    while (head is not null) 
    {
            total += head.Winnings * rank++;
            head = head.Next;
    }

    return total;
}

Console.WriteLine($"P1: {Solve(new HandFactory(), 11)}");
Console.WriteLine($"P2: {Solve(new WildCardHandFactory(), 1)}");

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

namespace Day07 
{
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

    class HandFactory 
    {
        public virtual Hand Make(string cards, string val) => new Hand { Cards = cards, Winnings = int.Parse(val) };
    }

    class WildCardHandFactory : HandFactory
    {
        public override Hand Make(string cards, string val) => new WildCardHand { Cards = cards, Winnings = int.Parse(val) };
    }

    class Hand : IComparable, IComparable<Hand>
    {
        public static int JokerValue;
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

        protected Dictionary<char, int> IndividualCards()
        {
            var counts = new Dictionary<char, int>();
            foreach (char ch in Cards!.ToCharArray())
            {
                if (counts.ContainsKey(ch))
                    counts[ch]++;
                else
                    counts.Add(ch, 1);
            }

            return counts;
        }

        protected HandType CalcType(int[] vals)
        {
            return vals[0] switch
            {
                5 => HandType.FiveOfAKind,
                4 => HandType.FourOfAKind,
                3 when vals[1] == 2 => HandType.FullHouse,
                3 => HandType.ThreeOfAKind,
                2 when vals[1] == 2 => HandType.TwoPair,
                2 => HandType.Pair,
                _ => HandType.HighCard
            };
        }

        virtual protected HandType Categorize()
        {
            var counts = IndividualCards();
            var vals = counts.Values.OrderByDescending(v => v).ToArray();
            
            return CalcType(vals);
        }

        public int CompareTo(object? obj) => obj is null ? 1 : CompareTo(obj as Hand);

        static private int CardValue(char c)
        {
            return c switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => JokerValue,
                'T' => 10,
                _   => c - '0'
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

        public static int Compare(Hand? left, Hand? right) => left!.CompareTo(right);
        public static bool operator <(Hand left, Hand right) => Compare(left, right) < 0;
        public static bool operator >(Hand left, Hand right) => Compare(left, right) > 0;
    }

    class WildCardHand : Hand
    {
        protected override HandType Categorize()
        {            
            // Okay, how about this: if there are jokers, still use the dictionary of letters
            // if there are 5 Js, return FiveOfAKind, otherwise remove Js from dic, add J count
            // to largest remaining category
            int[] vals;
            var counts = IndividualCards();
            if (counts.ContainsKey('J')) 
            {
                var js = counts['J'];

                if (js == 5) 
                {
                    return HandType.FiveOfAKind;
                }
                else
                {
                    counts.Remove('J');
                    vals = counts.Values.OrderByDescending(v => v).ToArray();
                    vals[0] += js;
                }
            }
            else 
            {
                vals = counts.Values.OrderByDescending(v => v).ToArray();
            }

            return CalcType(vals);
        }
    }
}