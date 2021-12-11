using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2021
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
    }

    public static class CollectionExtentions
    {
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

