using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC
{
    public interface IDay
    {
        void Solve();
    }

    public class Util
    {
        public static List<(int, int)> Neighbours()
        {
            return new List<(int, int)>() { (-1, 0), (1, 0), (0, -1), (0, 1) };
        }

        public static int TaxiDistance(int x0, int y0, int x1, int y1)
        {
            return Math.Abs(x0 - x1) + Math.Abs(y0 - y1);
        }
    }

    public static class CollectionExtentions
    {
        public static IEnumerable<int> AdjTo<T>(this IList<T> src, int x, int width)
        {
            int count = src.Count();
            int startC = x % width == 0 ? 0 : -1;
            int endC = (x + 1) % width == 0 ? 0 : 1;
            int startR = x < width ? 0 : -1;
            int endR = x >= count - width ? 0 : 1;

            for (int r = startR; r <= endR; r++)
            {
                for (int c = startC; c <= endC; c++)
                {
                    if (r == 0 && c == 0)
                        continue;
                    yield return x + r * width + c;
                }
            }
        }

        public static void PrintGrid<T>(this IList<T> src, int width)
        {
            int height = src.Count() / width;
            for (int r = 0; r < height; r++)
            {
                StringBuilder sb = new StringBuilder();
                for (int c = 0; c < width; c++)
                    sb.Append(src[r * width + c]);
                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine("");
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> src, int size)
        {
            while (src.Any())
            {
                yield return src.Take(size);
                src = src.Skip(size);
            }
        }
    }
}
