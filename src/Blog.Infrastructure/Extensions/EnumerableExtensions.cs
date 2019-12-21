using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (source.IsNullOrEmpty()) return;
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}