using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021
{
    public interface IDay
    {
        void Solve();
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

