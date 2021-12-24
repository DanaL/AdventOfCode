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
        public bool IsPlayer1 { get; set; }

        public State(byte p1p, byte p1s, byte p2p, byte p2s, bool p1)
        {
            Player1Pos = p1p;
            Player1Score = p1s;
            Player2Pos = p2p;
            Player2Score = p2s;
            IsPlayer1 = p1;
        }

        public override int GetHashCode() => Player1Pos ^ Player1Score ^ Player2Pos ^ Player2Score ^ (IsPlayer1 ? 1 : 0);

        public override bool Equals(object obj)
        {
            if (!(obj is State))
                return false;

            return Equals((State)obj);
        }

        public bool Equals(State other)
        {
            return Player1Pos == other.Player1Pos && Player1Score == other.Player1Score
                && Player2Pos == other.Player2Pos && Player2Score == other.Player2Score
                && IsPlayer1 == other.IsPlayer1;
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
        static List<(byte, byte)> _qrolls = new List<(byte, byte)>() { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) };
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
            var n = currPos + roll;
            return (byte)(n <= 10 ? n : n - 10);            
        }

        (ulong, ulong) Resolve(State curr)
        {
            if (_outcomes.ContainsKey(curr))
                return _outcomes[curr];

            ulong p1wins = 0;
            ulong p2wins = 0;
            foreach ((byte Roll, byte Times) roll in _qrolls)
            {
                if (curr.IsPlayer1)
                {
                    byte newPos = calcNewPos(curr.Player1Pos, roll.Roll);
                    byte newScore = (byte) (curr.Player1Score + newPos);
                    var newState = new State(newPos, newScore, curr.Player2Pos, curr.Player2Score, false);
                    (ulong, ulong) res;
                    if (_outcomes.ContainsKey(newState))
                        res = _outcomes[newState];
                    else if (newScore >= 21)
                        res = (1, 0);
                    else
                        res = Resolve(newState);
                    p1wins += res.Item1 * roll.Times;
                    p2wins += res.Item2 * roll.Times;
                }
                else
                {
                    byte newPos = calcNewPos(curr.Player2Pos, roll.Roll);
                    byte newScore = (byte)(curr.Player2Score + newPos);
                    var newState = new State(curr.Player1Pos, curr.Player1Score, newPos, newScore, true);
                    (ulong, ulong) res;
                    if (_outcomes.ContainsKey(newState))
                        res = _outcomes[newState];
                    else if (newScore >= 21)
                        res = (0, 1);
                    else
                        res = Resolve(newState);
                    p1wins += res.Item1 * roll.Times;
                    p2wins += res.Item2 * roll.Times;
                }
            }

            _outcomes.Add(curr, (p1wins, p2wins));

            return (p1wins, p2wins);
        }

        // It turns out Part Two was a fairly different problem than part one so
        // Part One code wouldn't really have been transferable anyhow!
        void PartTwo()
        {            
            _outcomes = new();
            var initial = new State(9, 0, 4, 0, true);
            var res = Resolve(initial);
            var winner = res.Item1 > res.Item2 ? res.Item1 : res.Item2;
            Console.WriteLine($"P2: {winner}");
        }

        public void Solve()
        {
            PartOne();
            PartTwo();
        }
    }
}
