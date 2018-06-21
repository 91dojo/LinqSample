using System;
using System.Collections.Generic;

internal static class YourOwnLinq
{
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source,
        Predicate<TSource> predicate)
    {
        var index = 0;

        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source,
        Func<TSource, int, bool> predicate)
    {
        var index = 0;

        foreach (var item in source)
        {
            if (predicate(item, index))
            {
                yield return item;
            }
        }
    }
}