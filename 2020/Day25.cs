using System;

namespace _2020
{
    public class Day25 : IDay
    {
        public Day25() { }

        private int findLoopSize(ulong target)
        {
            ulong val = 1;
            int loop = 0;
            while (val != target)
            {
                val = (val * 7) % 20201227;
                ++loop;
            }

            return loop;
        }

        private ulong calcKey(ulong subjectNum, int loopSize)
        {
            ulong val = 1;
            for (int j = 0; j < loopSize; j++)
            {
                val *= subjectNum;
                val = val % 20201227;
            }

            return val;
        }

        public void Solve()
        {
            ulong publicKeyDoor = 18_356_117;
            ulong publicKeyCard = 5_909_654;

            int loopDoor = findLoopSize(publicKeyDoor);
            int loopCard = findLoopSize(publicKeyCard);

            Console.WriteLine($"Loop for door: {loopDoor}");
            Console.WriteLine($"Loop for card: {loopCard}");

            Console.WriteLine($"P1: {calcKey(publicKeyCard, loopDoor)}");                        
        }
    }
}
