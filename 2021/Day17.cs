using System;

using AoC;

namespace _2021
{
    internal class Day17 : IDay
    {
        int _yApex;

        bool FireProbe(int initialVx, int initialVy, int targetMinX, int targetMaxX, int targetYLower, int targetYUpper)
        {
            int x = 0, y =0;
            int xv = initialVx;
            int yv = initialVy;

            while (x <= targetMaxX && y >= targetYUpper)
            {
                x += xv;
                if (xv > 0)
                    --xv;
                y += yv--;

                if (y > _yApex)
                    _yApex = y;

                if (x >= targetMinX && x <= targetMaxX && y <= targetYLower && y >= targetYUpper)
                    return true;
            }

            return false;
        }

        int MinStartX(int leftBound)
        {
            int n = 0;
            for ( ; n * (n + 1) < 2 * leftBound; n++)
                ;
            return n;
        }

        public void Solve()
        {
            // target area: x=25..67, y=-260..-200
            //int minX = 20, maxX = 30;
            //int minY = -5, maxY = -10;
            int minX = 25, maxX = 67;
            int minY = -200, maxY = -260;

            int startX = MinStartX(minX);
            _yApex = 0;
            int loftiest = 0;
            int hitTarget = 0;
            for (int x = startX; x <= maxX; x++)
            {
                for (int y = maxY; y < -maxY; y++)
                {
                    _yApex = 0;
                    if (FireProbe(x, y, minX, maxX, minY, maxY))
                    {
                        if (_yApex > loftiest)
                            loftiest = _yApex;
                        ++hitTarget;                
                    }
                }     
            }
            
            Console.WriteLine($"P1: {loftiest}");
            Console.WriteLine($"P2: {hitTarget}");
        }
    }
}
