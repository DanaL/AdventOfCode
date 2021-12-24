using System;
using System.Collections.Generic;
using System.Linq;

using AoC;

namespace _2021
{
    // Extreme over-engineering but I got curious about how operator overloading
    // works in C# (in like almost 20 years of using the language I've never felt
    // a strong need for it)
    struct State
    {
        public byte Player1Pos { get; set; }
        public byte Player1Score { get; set; }
        public byte Player2Pos { get; set; }
        public byte Player2Score { get; set; }

        public override int GetHashCode() => Player1Pos ^ Player1Score ^ Player2Pos ^ Player2Score;

        public override bool Equals(object obj)
        {
            if (!(obj is State))
                return false;

            return Equals((State)obj);
        }

        public bool Equals(State other)
        {
            return Player1Pos == other.Player1Pos && Player1Score == other.Player1Score
                && Player2Pos == other.Player2Pos && Player2Score == other.Player2Score;
        }

        public static bool operator ==(State point1, State point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(State point1, State point2)
        {
            return !point1.Equals(point2);
        }
    }

    public class Day21 : IDay
    {
        static byte[] _qrolls = new byte[] { 3, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 8, 8, 8, 9 };                                             
        static int[] _rolls = new int[100];
        static int _rollPos = 0;
        Dictionary<State, (ulong, ulong)> _outcomes;

        int Roll(int rolls)
        {
            int sum = 0;
            for (int _ = 0; _ < rolls; ++_)
            {
                sum += _rolls[_rollPos++];
                _rollPos %= 100;
            }

            return sum;
        }

        void PartOne()
        {
            for (int j = 0; j < 100; j++)
                _rolls[j] = j + 1;

            // Okay, gonna write part 1 in a naive loop. I'm going into this with
            // eyes wide open knowing it will be inadequate for part 2, but
            // I don't have any guesses what part 2 might entail, other than mabye
            // going around the track a quadrillion times?
            var player1Pos = 8;
            var player1Score = 0;
            var player2Pos = 3;
            var player2Score = 0;
            var rollNum = 0;
            
            while (player1Score < 1000 && player2Score < 1000)
            {
                if (rollNum % 2 == 0)
                {
                    var roll = Roll(3);
                    player1Pos = (player1Pos + roll) % 10;
                    player1Score += player1Pos + 1;
                }
                else
                {
                    var roll = Roll(3);
                    player2Pos = (player2Pos + roll) % 10;
                    player2Score += player2Pos + 1;
                }

                ++rollNum;
            }

            int loser = player1Score > player2Score ? player2Score : player1Score;
            Console.WriteLine($"Player 1: {player1Score}");
            Console.WriteLine($"Player 2: {player2Score}");
            Console.WriteLine($"P1: {rollNum * 3 * loser}");
        }

        byte calcNewPos(byte currPos, byte roll)
        {
            return (byte)(currPos + roll == 10 ? 10 : (currPos + roll) % 10);
        }

        (ulong, ulong) Resolve(State curr, bool player1)
        {
            if (_outcomes.ContainsKey(curr))
                return _outcomes[curr];

            if (curr.Player1Score >= 21)
                return (1, 0);
            else if (curr.Player2Score >= 21)
                return (0, 1);

            ulong p1wins = 0;
            ulong p2wins = 0;
            foreach (byte roll in _qrolls)
            {
                if (player1)
                {
                    byte newPos = calcNewPos(curr.Player1Pos, roll);
                    byte newScore = (byte) (curr.Player1Score + newPos);
                    var newState = new State() { Player1Pos = newPos, Player1Score = newScore, Player2Pos = curr.Player2Pos, Player2Score = curr.Player2Score };
                    (ulong, ulong) res;
                    if (_outcomes.ContainsKey(newState))
                        res = _outcomes[newState];
                    else
                        res = Resolve(newState, !player1);
                    p1wins += res.Item1;
                    p2wins += res.Item2;
                }
                else
                {
                    byte newPos = calcNewPos(curr.Player2Pos, roll);
                    byte newScore = (byte)(curr.Player2Score + newPos);
                    var newState = new State() { Player1Pos = curr.Player1Pos, Player1Score = curr.Player1Score, Player2Pos = newPos, Player2Score = newScore };
                    (ulong, ulong) res;
                    if (_outcomes.ContainsKey(newState))
                        res = _outcomes[newState];
                    else
                        res = Resolve(newState, !player1);
                    p1wins += res.Item1;
                    p2wins += res.Item2;
                }
            }

            _outcomes.Add(curr, (p1wins, p2wins));

            return (p1wins, p2wins);
        }

        // It turns out Part Two was a fairly different problem than part one so
        // Part One code wouldn't really have been transferable anyhow!
        void PartTwo()
        {
            // We need to calculate all possible outcomes. That is, all possible
            // combos of starting positions and scores (I think?).
            // Remember each turn is 3 die rolls
            // Maybe something like:
            // Dictionary<(int, int, int, int), (ulong, ulong)>
            // key is player 1 pos, player 2 pos, score, score and
            // value is # of player 1 wins, # of player 2 wins
            // So gist of logic is:
            // start with (1, 1) board pos, either look up result at this config
            // or call Simulate() (which will populate our table as it runs)

            _outcomes = new();
            var initial = new State() { Player1Pos = 4, Player1Score = 0, Player2Pos = 8, Player2Score = 0 };
            var res = Resolve(initial, true);
            Console.WriteLine(res);
        }

        public void Solve()
        {
            PartOne();
            PartTwo();
        }
    }
}
