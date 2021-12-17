using AoC;

namespace _2021
{
    internal class Day17 : IDay
    {
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

                if (x >= targetMinX && x <= targetMaxX && y <= targetYLower && y >= targetYUpper)
                    return true;
            }

            return false;
        }

        public void Solve()
        {
            FireProbe(17, -4, 20, 30, -5, -10);
        }
    }
}
