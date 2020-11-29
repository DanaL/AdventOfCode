using System;

namespace _2020
{
    public class Util
    {
        public Util() { }

        public static int TaxiDistance(int x0, int y0, int x1, int y1)
        {
            return Math.Abs(x0 - x1) + Math.Abs(y0 - y1);
        }
    }
}
