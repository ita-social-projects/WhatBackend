using System.Collections.Generic;
using System.Linq;

namespace CharlieBackend.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Dublicates<T>(this IEnumerable<T> values)
        {
            return values.GroupBy(x => x)
                 .Where(element => element.Count() > 1)
                 .Select(dublicate => dublicate.Key);
        }
    }
}
