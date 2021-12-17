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

                // TODO: We can bail out if we've reached the apex and it is 
                // less than _yApex
                if (y > _yApex)
                    _yApex = y;

                if (x >= targetMinX && x <= targetMaxX && y <= targetYLower && y >= targetYUpper)
                    return true;
            }

            return false;
        }

        public void Solve()
        {
            // target area: x=25..67, y=-260..-200
            //int minX = 20, maxX = 30;
            //int minY = -5, maxY = -10;
            int minX = 25, maxX = 67;
            int minY = -200, maxY = -260;
            
            int maxDistance = Util.TaxiDistance(0, 0, maxX, maxY);

            _yApex = 0;
            int loftiest = 0;
            for (int x = 0; x < minX; x++)
            {
                for (int y = 1; Util.TaxiDistance(0, 0, x ,y) < maxDistance; y++)                
                {
                    _yApex = 0;
                    if (FireProbe(x, y, minX, maxX, minY, maxY) && _yApex > loftiest)
                        loftiest = _yApex;                  
                }                
            }
            
            Console.WriteLine(loftiest);
        }
    }
}
