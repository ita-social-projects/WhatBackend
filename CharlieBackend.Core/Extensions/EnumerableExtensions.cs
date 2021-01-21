using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CharlieBackend.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Dublicates<T>(this IEnumerable<T> list)
        {
            return list.GroupBy(x => x)
                 .Where(el => el.Count() > 1)
                 .Select(dubl => dubl.Key);
        }

        public static string IEnumerableToString<T>(this IEnumerable<T> list)
        {
            string result = "";
            foreach (var el in list)
                result += el + " ";
            return result;
        }
    }
}
