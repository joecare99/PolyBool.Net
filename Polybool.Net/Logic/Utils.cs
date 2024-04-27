using System.Collections.Generic;

namespace Polybool.Net.Logic
{
    internal static class Utils
    {
        public static void Shift<T>(this IList<T> lst)
        {
            lst.RemoveAt(0);
        }

        public static void Pop<T>(this IList<T> lst)
        {
            lst.RemoveAt(lst.Count-1);
        }

        public static void Splice<T>(this IList<T> source, int index, int count)
        {

            if (source is List<T> l)
                l.RemoveRange(index, count);
            else
            {
                for (var i = 0; i < count && index < source.Count; i++)
                    source.RemoveAt(index);
            }
        }

        public static void Push<T>(this IList<T> source, T elem)
        {
            source.Add(elem);
        }

        public static void Unshift<T>(this IList<T> source, T elem)
        {
            source.Insert(0, elem);
        }
    }
}