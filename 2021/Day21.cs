using System;

using AoC;

namespace _2021
{
    public class Day21 : IDay
    {
        static int[] _rolls = new int[100];
        static int _rollPos = 0;

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

        public void Solve()
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
            Console.WriteLine($"{rollNum * 3 * loser}");
        }
    }
}
