using System;
using System.Collections.Generic;
using System.Linq;

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

