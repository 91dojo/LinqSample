using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LinqTests;

namespace JoeyIsFat
{
    internal static class WithoutLinq
    {
        public static IEnumerable<TSource> JoeyWhere<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<string> ToHttps(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                yield return url.Replace(@"http://", @"https://");
            }
        }

        public static IEnumerable<TResult> JoeySelect<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            foreach (var item in source)
            {
                yield return selector(item);
            }
        }

        public static IEnumerable<TSource> JoeyTake<TSource>(IEnumerable<TSource> employees, int count)
        {
            var enumerator = employees.GetEnumerator();

            var index = 0;

            while (enumerator.MoveNext())
            {
                if (index >= count)
                {
                    yield break;
                }

                yield return enumerator.Current;
                index++;
            }
        }

        public static IEnumerable<TSource> JoeySkip<TSource>(IEnumerable<TSource> employees, int count)
        {
            var enumerator = employees.GetEnumerator();

            var index = 0;

            while (enumerator.MoveNext())
            {
                if (index >= count)
                {
                    yield return enumerator.Current;
                }

                index++;
            }
        }

        public static IEnumerable<int> GetSum<TSource>(IEnumerable<TSource> source, int pageSize,
            Func<TSource, int> sumFunc)
        {
            var rowIndex = 0;

            while (rowIndex < source.Count())
            {
                yield return source.Skip(rowIndex).Take(pageSize).Sum(sumFunc);
                rowIndex += pageSize;
            }
        }

        public static IEnumerable<TSource> TakeWhile<TSource>(IEnumerable<TSource> source, int count,
            Func<TSource, bool> predicate)
        {
            var enumerator = source.GetEnumerator();
            var index = 0;

            while (enumerator.MoveNext() && index < count)
            {
                if (predicate(enumerator.Current))
                {
                    yield return enumerator.Current;
                    index++;
                }
            }
        }

        public static IEnumerable<TSource> SkipWhile<TSource>(IEnumerable<TSource> employees, int count,
            Func<TSource, bool> predicate)
        {
            var enumerator = employees.GetEnumerator();

            var skipCounter = 0;

            while (enumerator.MoveNext())
            {
                if (skipCounter >= count || !predicate(enumerator.Current))
                {
                    yield return enumerator.Current;
                }
                else
                {
                    skipCounter++;
                }
            }
        }

        public static bool JoeyAny<TSource>(this IEnumerable<TSource> source)
        {
            return source.GetEnumerator().MoveNext();
        }

        public static bool JoeyAny<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return true;
                }
            }

            return false;

            foreach (var employee in source)
            {
                if (predicate(employee))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool JoeyAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            //return employees.All(e => e.MonthSalary > 200);
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!predicate(enumerator.Current))
                {
                    return false;
                }
            }

            return true;
        }

        public static TSource JoeyFirstOrDefault<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }

            return default(TSource);
        }

        public static TSource EltonFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }

            throw new InvalidOperationException();
        }


        public static TSource JasonFirstOrDefault<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate, TSource item)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    return enumerator.Current;
                }
            }

            return item;
        }

        public static TSource SamSingle<TSource>(this IEnumerable<TSource> source)
        {
            var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                var result = enumerator.Current;
                if (!enumerator.MoveNext())
                {
                    return result;
                }
            }

            throw new InvalidOperationException();
        }

        public static TSource JoeySingle<TSource>(this IEnumerable<TSource> employees, Func<TSource, bool> predicate)
        {
            var enumerator = employees.GetEnumerator();
            var isMatch = false;
            var singleItem = default(TSource);
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    if (isMatch)
                    {
                        throw new InvalidOperationException();
                    }

                    isMatch = true;
                    singleItem = enumerator.Current;
                }
            }

            if (!isMatch)
            {
                throw new InvalidOperationException();
            }

            return singleItem;
        }

        public static IEnumerable<Employee> luluDefaultEmpty(this IEnumerable<Employee> source)
        {
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                yield return new Employee() { Name = "Lulu" };
                yield break;
            }

            yield return enumerator.Current;
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

        }
    }
}